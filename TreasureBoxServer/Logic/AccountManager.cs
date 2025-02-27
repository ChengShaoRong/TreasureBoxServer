using CSharpLike;
using KissFramework;
using KissServerFramework;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TreasureBox
{
    public class AccountManager : Singleton<AccountManager>
    {
        /// <summary>
        /// All account unique key by uid
        /// </summary>
        Dictionary<int, Account> accounts = new Dictionary<int, Account>();
        /// <summary>
        /// All account unique key by player instance
        /// </summary>
        Dictionary<Account, Player> playersByAccount = new Dictionary<Account, Player>();
        /// <summary>
        /// All account unique key by 'name+"_"+acctType'
        /// </summary>
        Dictionary<string, Account> accountsByName = new Dictionary<string, Account>();
        /// <summary>
        /// Get account by uid
        /// </summary>
        /// <param name="uid">unique id of account</param>
        public Account GetAccount(int uid)
        {
            Account account;
            if (accounts.TryGetValue(uid, out account))
                return account;
            return null;
        }
        /// <summary>
        /// Get account by login name and account type.
        /// </summary>
        /// <param name="name">login name, not nickname</param>
        /// <param name="acctType">account type</param>
        public Account GetAccount(string name, int acctType)
        {
            if (accountsByName.TryGetValue(name + "_" + acctType, out Account account))
                return account;
            return null;
        }
        /// <summary>
        /// Get player by account.
        /// </summary>
        /// <param name="account">The account instance</param>
        public Player GetPlayer(Account account)
        {
            if (playersByAccount.TryGetValue(account, out Player player))
                return player;
            return null;
        }
        /// <summary>
        /// Get player by account.
        /// </summary>
        /// <param name="account">The account instance</param>
        public Player GetPlayer(int uid)
        {
            if (accounts.TryGetValue(uid, out Account account))
            {
                if (playersByAccount.TryGetValue(account, out Player player))
                    return player;
                else
                    Logger.LogInfo("GetPlayer : 1 Not exist in playersByAccount : uid " + uid);
            }
            else
                Logger.LogInfo("GetPlayer : 2 Not exist in accounts : uid " + uid);
            return null;
        }
        /// <summary>
        /// Check the account whether exist in cache.
        /// If exist use the data in cache, otherwise use the current data and cache it.
        /// You should check it because you can't make sure other request whether
        /// had loaded it or modified it while you loading it from database in multi-threading.
        /// </summary>
        /// <param name="account">The account just load from database</param>
        public bool GetAccount(ref Account account)
        {
            if (accounts.TryGetValue(account.uid, out Account accountCache))
            {
                Logger.LogInfo("GetAccount : Exist in cache and using the old account");
                account = accountCache;
                return true;
            }
            else//if not exist we cache it
            {
                Logger.LogInfo("GetAccount : Not exist in cache and store in cache");
                accounts[account.uid] = account;
                accountsByName[account.name + "_" + account.acctType] = account;
                return false;
            }
        }
        public Account GetAccountByNickname(string nickname)
        {
            foreach(var account in accounts.Values)
            {
                if (account.nickname == nickname)
                    return account;
            }
            return null;
        }
        public void ClearAccountCache(Account account)
        {
            Logger.LogWarning($"ClearAccountCache : {account}");
            accounts.Remove(account.uid);
            playersByAccount.Remove(account);
            accountsByName.Remove(account.name + "_" + account.acctType);
        }
        static int mLastOnlineCount = 0;
        /// <summary>
        /// Print information
        /// </summary>
        [EventMethod(IntervalTime = 60f)]
        static void PrintInformation()
        {
            int onlineCount = Instance.playersByAccount.Count;
            if (mLastOnlineCount != onlineCount)
            {
                Logger.LogInfo($"AccountManager: online count {mLastOnlineCount} -> {onlineCount}");
                mLastOnlineCount = onlineCount;
            }
        }
        /// <summary>
        /// Broadcast message to all players. e.g. Send maintenance message to all players.
        /// </summary>
        public void BroadcastToAllPlayer(string msg)
        {
            foreach (Player player in playersByAccount.Values)
                player.Send(msg);
        }
        /// <summary>
        /// Broadcast message to all players. e.g. Send maintenance message to all players.
        /// </summary>
        public void BroadcastToAllPlayer(JSONData msg)
        {
            BroadcastToAllPlayer(msg.ToString());
        }
        /// <summary>
        /// Login server.
        /// This step check the third party SDK.
        /// If OK will go to next step.
        /// </summary>
        /// <param name="name">login name</param>
        /// <param name="acctType">account type</param>
        /// <param name="password">password</param>
        /// <param name="player">current player</param>
        public void Login(string name, int acctType, string password, Player player)
        {
            //You may process re login, we send back the account instance to client
            if (player.account != null && player.account.acctType == acctType)
            {
                Player oldPlayer = GetPlayer(player.account);
                if (oldPlayer != player)
                {
                    oldPlayer.IsReplace = true;
                    oldPlayer.Replace(player);//will disconnect the old player
                    //player = oldPlayer;
                }
                Account account = player.account;
                GetAccount(ref account);
                playersByAccount[account] = player;
                player.account = account;
                Logger.LogInfo("Account info still in cache, resend them.");

                player.CB_AccountLogin(player.account.ToJSONData());

                account.MarkModifyMaskAllSubSystem();
                account.SyncToClient();
                return;
            }

            switch ((Account.AccountType)acctType)
            {
                case Account.AccountType.BuildIn:
                    {
                        JSONData jsonPost = JSONData.NewDictionary();
                        jsonPost["uid"] = name;
                        jsonPost["token"] = password;
                        Framework.Sign(jsonPost);
                        //You can post JSONData or string like below.
                        //string strPost = $"uid={jsonData["name"]}&token={jsonData["password"]}&sign={FrameworkBase.GetMD5((string)jsonData["name"] + jsonData["password"] + "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz")}";
                        new ThreadPoolHttp(Framework.config.buildInAccountURL, jsonPost.ToJson(),
                            (callback) =>//The HTTP request running in background thread, and this callback run in main thread.
                            {
                                JSONData jsonCallback = KissJson.ToJSONData(callback);
                                if (jsonCallback["code"] == 0)//success
                                {
                                    if (player == null)
                                    {
                                        Logger.LogWarning("player offline, ignore request in Login");
                                        return;
                                    }
                                    LoginStep2(name, acctType, password, player);
                                }
                                else
                                {
                                    Logger.LogError($"url={Framework.config.buildInAccountURL} post={jsonPost} callback={callback}");
                                    player.CB_Error(jsonCallback["error"], true);
                                }
                            });
                    }
                    return;
                case Account.AccountType.ThirdParty_Test://We check from third party server, we simulate it by using our HTTP server.
                    {
                        //Sample of using POST
                        string strURL = $"http://127.0.0.1:{Framework.config.httpServerPort}/TestThirdPartyAccount";
                        JSONData jsonPost = JSONData.NewDictionary();
                        jsonPost["uid"] = name;
                        jsonPost["token"] = password;
                        jsonPost["sign"] = FrameworkBase.GetMD5((string)jsonPost["uid"] + jsonPost["token"] + "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
                        //You can post JSONData or string like below.
                        //string strPost = $"uid={jsonData["name"]}&token={jsonData["password"]}&sign={FrameworkBase.GetMD5((string)jsonData["name"] + jsonData["password"] + "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz")}";
                        new ThreadPoolHttp(strURL, jsonPost.ToJson(),
                            (callback) =>//The HTTP request running in background thread, and this callback run in main thread.
                            {
                                JSONData jsonCallback = KissJson.ToJSONData(callback);
                                if (jsonCallback["state"] == 0)//success
                                {
                                    if (player == null)
                                    {
                                        Logger.LogWarning("player offline, ignore request in Login");
                                        return;
                                    }
                                    LoginStep2(name, acctType, password, player);
                                }
                                else
                                {
                                    player.CB_Error($"url={strURL} post={jsonPost} callback={callback}", true);
                                }
                            });
                        /*
                        //Sample of using GET
                        string strURL = $"http://127.0.0.1:{Framework.config.httpServerPort}/TestThirdPartyAccount?uid={jsonData["name"]}&token={jsonData["password"]}&sign={FrameworkBase.GetMD5((string)jsonData["name"] + jsonData["password"] + "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz")}";
                        new ThreadPoolHttp(strURL, 
                            (callback) =>//The HTTP request running in background thread, and this callback run in main thread.
                            {
                                JSONData jsonCallback = KissJson.ToJSONData(callback);
                                if (jsonCallback["state"] == 0)//success
                                {
                                    if (player == null)
                                    {
                                        Logger.LogWarning("player offline, ignore request in LoginStep2");
                                        return;
                                    }
                                    LoginStep2(jsonData, player);
                                }
                                else
                                {
                                    player.CallbackError($"url={strURL} callback={callback}");
                                }
                            });
                        */
                        /*
                        //If you have to custom the WebRequest by yourself.
                        string strURL = $"http://127.0.0.1:{Framework.config.httpServerPort}/TestThirdPartyAccount";
                        var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strURL);
                        //To do your special process to the WebRequest.
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Method = "GET";

                        new ThreadPoolHttp(request, 
                            (callback) =>
                            {
                                JSONData jsonCallback = KissJson.ToJSONData(callback);
                                if (jsonCallback["state"] == 0)//success
                                {
                                    if (player == null)
                                    {
                                        Logger.LogWarning("player offline, ignore request in LoginStep2");
                                        return;
                                    }
                                    LoginStep2(jsonData, player);
                                }
                                else
                                {
                                    player.CallbackError($"url={strURL} callback={callback}");
                                }
                            });
                        */
                    }
                    break;
                default:
                    player.CB_Error("Not support acctType:" + "acctType", true);
                    return;
            }
        }
        /// <summary>
        /// Select data from Database if not in cache, and insert into database if not exist.
        /// And then go to next step.
        /// </summary>
        /// <param name="name">login name</param>
        /// <param name="acctType">account type</param>
        /// <param name="password">password</param>
        /// <param name="player">current player</param>
        void LoginStep2(string name, int acctType, string password, Player player)
        {
            name = ThreadPoolMySql.ReplaceInjectString(name);
            //check the cache
            Account account;
            if (!accountsByName.TryGetValue(name + "_" + acctType, out account))//no cache
            {
                Account.SelectByAcctTypeAndName(acctType, name, (accounts, error) =>
                {
                    //Select account occur error
                    if (!string.IsNullOrEmpty(error))
                    {
                        player.CB_Error($"Select account occur error : {error}.", true);
                        return;
                    }
                    //New account if not exist account
                    if (accounts.Count == 0)
                    {
                        DateTime dtNow = DateTime.Now;
                        Account.Insert(acctType, name, dtNow,
                            "Guest" + FrameworkBase.GetRand(100000), 1, 0, 0, dtNow, 1, 0, 120, dtNow,0,0,
                            (newAccount, error) =>
                            {
                                //select account occur error
                                if (!string.IsNullOrEmpty(error))
                                {
                                    player.CB_Error($"Insert database error : {error}.", true);
                                    return;
                                }
                                //Using the account that just inserted into database
                                LoginStep3(name, acctType, password, newAccount, player, true);
                            });
                        return;
                    }
                    //Using the account select from database
                    LoginStep3(name, acctType, password, accounts[0], player, false);
                });
                return;
            }
            //Using the account in cache
            LoginStep3(name, acctType, password, account, player, false);
        }
        /// <summary>
        /// Check account info whether valid, and then send to client.
        /// </summary>
        /// <param name="name">login name</param>
        /// <param name="acctType">account type</param>
        /// <param name="password">password</param>
        /// <param name="account">the account instance</param>
        /// <param name="player">current player</param>
        /// <param name="bCreate">Whether just was created this account right now</param>
        void LoginStep3(string name, int acctType, string password, Account account, Player player, bool bCreate)
        {
            //cache account
            bool existInCache = GetAccount(ref account);

            //whether exist the old player instance
            Player oldPlayer = GetPlayer(account);
            if (oldPlayer != null)
            {
                oldPlayer.IsReplace = true;
                oldPlayer.Replace(player);//will disconnect the old player
                //player = oldPlayer;
            }
            playersByAccount[account] = player;
            player.account = account;
            //callback the account to client immediately
            player.CB_AccountLogin(account.ToJSONData());

            //Log account
            LogManager.LogAccount(account.uid, 0, player.IP);

            account.LastLoginTime = account.lastLoginTime;
            account.OnOnline();

            //Process the first time account login.
            if (bCreate)
            {
                int _countSubSystem = 1;
                //We send a mail here for example, with item id 3 and item count 2 as mail appendix 
                account.SendMail("Create account gift", "Welcome, here are the gift for you.", 3, 2);
                //We add a item here for example
                account.ChangeItem(3, 1);
                //Initialize some sub system here, You MUST manual insert if not exist
                SignIn.Insert(account.uid, DateTime.Now.Month, "", (signIn, error) =>
                {
                    player = GetPlayer(account.uid);//May be player instance was changed after DB multi-threading operation done
                    if (player != null)//May be player was offline
                    {
                        player.account.SetSignIn(signIn);
                        player.account.OnSignInLoaded();
                        _countSubSystem--;
                        if (_countSubSystem == 0)
                            player.account.OnAllSubSystemLoaded();
                    }
                });
            }
            else
            {
                //Load all system into account, it will automatically synchronize to client.
                //If you want to do something when all system loaded, you can override OnAllSubSystemLoaded in Account.
                //If you want to do something when some system loaded, you can override OnXXXXXLoaded in Account.
                if (existInCache)
                {
                    account.MarkModifyMaskAllSubSystem();
                    account.SyncToClient();
                    account.OnAllSubSystemLoaded();
                }
                else
                    account.LoadAllSubSystem(player);//callback in account override function 'OnAllSubSystemLoaded'
            }
            account.lastLoginTime = DateTime.Now;
        }
    }
}

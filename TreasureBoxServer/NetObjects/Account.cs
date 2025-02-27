
using CSharpLike;
using KissFramework;
using KissServerFramework;
using System;
using System.Collections.Generic;

namespace TreasureBox
{
    /// <summary>
    /// This class include all the system belong to player, such as items/mails...
    /// </summary>
    public sealed class Account : Account_Base
    {
        public override bool IsHttp
        {
            get
            {
                return acctType == (int)AccountType.BuildInHTTP;
            }
        }
        /// <summary>
        /// JSON data for HTTP
        /// </summary>
        public JSONData jsonDataHTTP
        {
            get
            {
                JSONData data = ToJSONData();
                data["msg"] = "";
                data.RemoveKey("_uid_");
                data.RemoveKey("_sendMask_");
                data.RemoveKey("password");
                return data;
            }
        }
        public enum AccountType
        {
            BuildIn,//Build-in account
            ThirdParty_Test,//Sample for test third party account.
                            //Login flow :
                            //Client got uid and token from third party SDK. 
                            //-> send to our server.
                            //-> our server confirm that uid and token from third party server by HTTP(s).
                            //-> login success/fail
            BuildInHTTP,//Build-in HTTP account, this account type won't sync to client automatically due to HTTP(s) is short connection
        }
        public override void OnItemLoaded()
        {
            Logger.LogInfo("OnItemLoaded:count=" + items.Count);
        }
        public override void OnMailLoaded()
        {
            Logger.LogInfo("OnMailLoaded:count=" + mails.Count);
        }
        public override void OnSignInLoaded()
        {
            Logger.LogInfo("OnSignInLoaded:" + signIn);
        }
        public override void OnAllSubSystemLoaded()
        {
            Logger.LogInfo("OnAllSubSystemLoaded");
            if (LastLoginTime.Month != signIn.month)
            {
                signIn.month = LastLoginTime.Month;
                signIn.signInList = "";
            }
            if (LastLoginTime.Day != DateTime.Now.Day)
            {
                //We send a mail here for example, with item id 1 and item count 2 as mail appendix 
                SendMail("Login gift", "Welcome to KissServerFramework, here are the gift for you.", 1, 2);
                //We add a item here for example
                ChangeItem(2, 1);
            }

            LastLoginTime = lastLoginTime;
        }
        public override Func<int, PlayerBase> GetPlayer()
        {
            return (int uid) => { return AccountManager.Instance.GetPlayer(uid); };
        }
        /// <summary>
        /// Send message to client
        /// </summary>
        public void Send(string msg)
        {
            Player player = AccountManager.Instance.GetPlayer(uid);
            if (player != null)
                player.Send(msg);
        }
        /// <summary>
        /// Send message to client
        /// </summary>
        public void Send(JSONData jsonData)
        {
            Send(jsonData.ToString());
        }
        string offlineEventKey = "";
        /// <summary>
        /// Process your custom action when account online
        /// </summary>
        public void OnOnline()
        {
            //Cancel the event for clear the cache of this account in 1 hour.
            if (!string.IsNullOrEmpty(offlineEventKey))
            {
                Framework.CancelEvent(offlineEventKey);
                offlineEventKey = "";
            }

            if (GangManager.IsGangInited)
            {
                GangMember member = GangManager.GetGangMemberById(uid);
                if (member != null)
                {
                    member.online = true;
                    if (member.gangId != gangId)
                    {
                        gang?.members.Remove(uid);
                        gangId = member.gangId;
                    }
                }
            }
        }
        /// <summary>
        /// Process your custom action when account offline
        /// </summary>
        public void OnOffline()
        {
            //Raise the event for clear the cache of this account in 1 hour.
            if (string.IsNullOrEmpty(offlineEventKey))
                offlineEventKey = Framework.RaiseEvent((deltaTime) =>
                {
                    AccountManager.Instance.ClearAccountCache(this);

                }, Framework.config.accountCacheTime, 1);

            if (GangManager.IsGangInited)
            {
                GangMember member = GangManager.GetGangMemberById(uid);
                if (member != null)
                {
                    member.offline = DateTime.Now;
                    member.online = false;
                }
            }
        }

        /// <summary>
        /// Change item count
        /// </summary>
        /// <param name="itemId">item id</param>
        /// <param name="count">item count</param>
        /// <param name="logType">type for log in database, default is 0</param>
        /// <returns>Change item action whether success</returns>
        public bool ChangeItem(int itemId, int count, int logType = 0)
        {
            if (count == 0)
                return false;
            Item item = GetItem(itemId);
            if (count > 0)//AddItem
            {
                if (item != null)
                {
                    item.count += count;
                    LogManager.LogItem(uid, logType, count, item.count);
                }
                else
                {
                    Item.Insert(itemId, uid, count, (newItem, error) =>
                    {
                        if (string.IsNullOrEmpty(error))
                            return;
                        item = GetItem(itemId);
                        if (item != null)//Has exist the item due to was add same item id while inserting into DB. It probably happen!!!
                        {
                            //We increase the count and remove the duplicate one
                            item.count += newItem.count;//Change the item count, and it will auto save to database.
                            newItem.Delete();//Delete the duplicate item from database.
                        }
                        else
                        {
                            SetItems(new List<Item>() { newItem });
                            item = newItem;
                        }
                        LogManager.LogItem(uid, logType, count, item.count);
                    });
                }
            }
            else//RemoveItem
            {
                if (item == null || item.count < count)//Not exist or not enough count
                {
                    return false;
                }
                else
                {
                    item.count -= count;
                    LogManager.LogItem(uid, logType, count, item.count);
                }
            }
            return true;
        }
//#if FREE_VERSION//We recommend set `appendix` type in NotObject `Mail` as `Dictionary<int, int>` in C#Like, but must set as `string` in C#LikeFree.
        /// <summary>
        /// Send mail to player
        /// </summary>
        /// <param name="title">mail title</param>
        /// <param name="content">mail content</param>
        /// <param name="appendix">multiple item for mail appendix, the key is item id and the value is item count, default value is null</param>
        /// <param name="senderId">sender uid, default is 0</param>
        /// <param name="senderName">sender name, default is 'System'</param>
        public void SendMail(string title, string content, Dictionary<int, int> appendix = null, int senderId = 0, string senderName = "System")
        {
            SendMail(title, content, NetObjectUtils.DictionaryToString(appendix), senderId, senderName);
        }
        /// <summary>
        /// Send mail to player
        /// </summary>
        /// <param name="title">mail title</param>
        /// <param name="content">mail content</param>
        /// <param name="appendix">multiple item for mail appendix, `itemId1,itemId2 itemId2,itemId2...` e.g. string `123,1 234,2` will convert to dictionary `{{123,1},{234,2}}`</param>
        /// <param name="senderId">sender uid, default is 0</param>
        /// <param name="senderName">sender name, default is 'System'</param>
        public void SendMail(string title, string content, string appendix = "", int senderId = 0, string senderName = "System")
        {
            //Insert into database
            Mail.Insert(uid, senderId, senderName, title, content, appendix, DateTime.Now, 0, 0,
                (mail, error) =>//callback from database
                {
                    if (string.IsNullOrEmpty(error))
                    {
                        //Sync to client after inserted into database
                        SetMails(new List<Mail>() { mail });
                        LogManager.LogMail(uid, 0, NetObjectUtils.StringToDictionary<int, int>(appendix), content, title);//You may don't need log this.
                    }
                    else
                        Logger.LogError(error);
                });
        }
//#else
//        /// <summary>
//        /// Send mail to player
//        /// </summary>
//        /// <param name="title">mail title</param>
//        /// <param name="content">mail content</param>
//        /// <param name="appendix">multiple item for mail appendix, the key is item id and the value is item count, default value is null</param>
//        /// <param name="senderId">sender uid, default is 0</param>
//        /// <param name="senderName">sender name, default is 'System'</param>
//        public void SendMail(string title, string content, Dictionary<int, int> appendix = null, int senderId = 0, string senderName = "System")
//        {
//            //Insert into database
//            Mail.Insert(uid, senderId, senderName, title, content, appendix, DateTime.Now, 0, 0,
//                (mail, error) =>//callback from database
//                {
//                    if (string.IsNullOrEmpty(error))
//                    {
//                        //Sync to client after inserted into database
//                        SetMails(new List<Mail>() { mail });
//                        LogManager.LogMail(uid, 0, appendix, content, title);//You may don't need log this.
//                    }
//                    else
//                        Logger.LogError(error);
//                });
//        }
//#endif
        /// <summary>
        /// Send mail to player
        /// </summary>
        /// <param name="title">mail title</param>
        /// <param name="content">mail content</param>
        /// <param name="itemId">One item id for mail appendix</param>
        /// <param name="itemCount">One item count for mail appendix</param>
        /// <param name="senderId">sender uid, default is 0</param>
        /// <param name="senderName">sender name, default is 'System'</param>
        public void SendMail(string title, string content, int itemId, int itemCount, int senderId = 0, string senderName = "System")
        {
            if (itemId != 0 && itemCount != 0)
                SendMail(title, content, new Dictionary<int, int>() { { itemId, itemCount } }, senderId, senderName);
            else
                SendMail(title, content, new Dictionary<int, int>(), senderId, senderName);
        }
        /// <summary>
        /// Mark mail was read
        /// </summary>
        /// <param name="uid">The mail UID</param>
        public void ReadMail(int uid)
        {
            Mail mail = GetMail(uid);
            if (mail != null && mail.wasRead == 0)
                mail.wasRead = 1;
        }
        /// <summary>
        /// Take mail appendix
        /// </summary>
        /// <param name="uids">The mail uids.</param>
        public void TakeMailAppendix(List<int> uids)
        {
            Dictionary<int, int> appendixs = new Dictionary<int, int>();
            foreach(int uid in uids)
            {
                Mail mail = GetMail(uid);
//#if FREE_VERSION//We recommend set `appendix` type in NotObject `Mail` as `Dictionary<int, int>` in C#Like, but must set as `string` in C#LikeFree.
                if (mail != null && mail.received == 0 && mail.Appendix.Count > 0)
                {
                    foreach (var appendix in mail.Appendix)
                    {
                        if (appendixs.ContainsKey(appendix.Key))//merge count
                            appendixs[appendix.Key] += appendix.Value;
                        else
                            appendixs[appendix.Key] = appendix.Value;
                    }
                    mail.received = 1;
                    if (mail.wasRead == 0)
                        mail.wasRead = 1;
                }
//#else
//                if (mail != null && mail.received == 0 && mail.appendix.Count > 0)
//                {
//                    foreach (var appendix in mail.appendix)
//                    {
//                        if (appendixs.ContainsKey(appendix.Key))//merge count
//                            appendixs[appendix.Key] += appendix.Value;
//                        else
//                            appendixs[appendix.Key] = appendix.Value;
//                    }
//                    mail.received = 1;
//                    if (mail.wasRead == 0)
//                        mail.wasRead = 1;
//                }
//#endif
            }
            CB_GetReward(appendixs);
        }
        public void CB_GetReward(Dictionary<int, int> items, LogManagerEnum.LogItemType logItemType = LogManagerEnum.LogItemType.None)
        {
            if (items.Count == 0)
                return;
            List<int> itemIds = new List<int>();
            List<int> itemCounts = new List<int>();
            foreach (var item in items)
            {
                itemIds.Add(item.Key);
                itemCounts.Add(item.Value);
            }
            CB_GetReward(itemIds, itemCounts, logItemType);
        }
        public void CB_GetReward(List<int> itemIds, List<int> itemCounts, LogManagerEnum.LogItemType logItemType = LogManagerEnum.LogItemType.None)
        {
            int count = itemIds.Count;
            if (count == 0 || count != itemCounts.Count)
            {
                Logger.LogError($"CB_GetReward: itemIds.Count = {count}, itemCounts.Count = {itemCounts.Count}");
                return;
            }
            for (int i = 0; i < count; i++)
            {
                ChangeItem(itemIds[i], itemCounts[i], (int)logItemType);
            }
            AccountManager.Instance.GetPlayer(uid)?.CB_GetReward(itemIds, itemCounts);
        }
        /// <summary>
        /// Delete mail
        /// </summary>
        /// <param name="uids">The mail uids.</param>
        public void DeleteMail(List<int> uids)
        {
            foreach (int uid in uids)
                RemoveMail(uid);
        }
        /// <summary>
        /// Change the gold value
        /// </summary>
        public bool ChangeDiamond(int value)
        {
            //Check the valueChange whether valid
            if (value == 0 || diamond < value)
                return false;
            //change value
            if ((diamond + (long)value) > int.MaxValue)//prevent overflow
                diamond = int.MaxValue;
            else
                diamond += value;
            return true;
        }
        /// <summary>
        /// Change the money value
        /// </summary>
        public bool ChangeMoney(int value)
        {
            //Check the valueChange whether valid
            if (value == 0 || money < value)
                return false;
            //change value
            if ((money + (long)value) > int.MaxValue)//prevent overflow
                money = int.MaxValue;
            else
                money += value;
            return true;
        }

        public int Vip => VipCsv.GetVip(vipExp);

        public void SignInForGift(Player player)
        {
            if (signIn == null)
            {
                player.CB_Tips("LT_Tips_SignInNull");
                return;
            }
            signIn.GetReward(player);
        }
        /// <summary>
        /// Last login time for cache. will reset after all sub system loaded.
        /// </summary>
        public DateTime LastLoginTime
        {
            get;
            set;
        }

        public Gang gang => GangManager.GetGangById(gangId);
    }
}

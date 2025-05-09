﻿using CSharpLike;
using System;
using KissFramework;
using System.Collections.Generic;

namespace TreasureBox
{
    /// <summary>
    /// The class for transfer JSON object between server and client, whatever the client using WebSocket or Socket.
    /// 1 Received JSON object from client by 'void OnMessage(JSONData jsonData)'. 
    /// 2 Send JSON object to client by 'void Send(JSONData jsonData)'.
    /// 3 Accept player info by account.
    /// </summary>
    public sealed class Player : Player_Base
    {
        #region Override WebSocket/Socket action
        ///// <summary>
        ///// Received JSON object from client, that run in main thread.
        ///// </summary>
        ///// <param name="jsonData">The JSON object from client</param>
        //public override void OnMessage(JSONData jsonData)
        //{
        //    base.OnMessage(jsonData);
        //}
        /// <summary>
        /// Whether called 'Player.Replace'
        /// </summary>
        public bool IsReplace = false;
        /// <summary>
        /// When the player disconnect, that run in main thread.
        /// The client if not complete the close action,
        /// the server will check whether the WebSocket is dead in every 30 seconds.
        /// </summary>
        public override void OnDisconnect()
        {
            Logger.LogInfo($"Player:OnDisconnect:IsReplace={IsReplace}");
            if (account != null)
            {
                LogManager.LogAccount(account.uid, 1, IP);
                if (!IsReplace)
                {
                    Player player = AccountManager.Instance.GetPlayer(account.uid);
                    if (player == this)
                        account.OnOffline();
                    else
                        Logger.LogInfo("Player:OnDisconnect:Not the player in cache, that may be relogin, and will not call OnOffline function.");
                }
            }
        }
        ///// <summary>
        ///// When player connected, that run in main thread
        ///// </summary>
        //public override void OnConnect()
        //{
        //    Logger.LogInfo("Player:OnConnect");
        //}
        ///// <summary>
        ///// The WebSocket occur error, that run in main thread
        ///// </summary>
        //public override void OnError(string msg)
        //{
        //    Logger.LogInfo("Player:OnError:"+ msg);
        //}
        #endregion//Override WebSocket/Socket action

        public void CB_Tips(string tips) => CB_Tips(tips, true);
        public void CB_Error(string error) => CB_Error(error, true);

        #region Override packet received from client
        public override void AccountLogin(string name, int acctType, string password)
        {
            AccountManager.Instance.Login(name, acctType, password, this);
        }
        public override void SignInForGift()
        {
            account.SignInForGift(this);
        }
        public override void ReadMail(int uid)
        {
            account.ReadMail(uid);
        }
        public override void DeleteMail(List<int> uids)
        {
            account.DeleteMail(uids);
        }
        public override void TakeMailAppendix(List<int> uids)
        {
            account.TakeMailAppendix(uids);
        }

        public override void SignInForGift24()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift23()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift22()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift21()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift20()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift19()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift18()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift17()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift12()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift11()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift10()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift9()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift8()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift6()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift5()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift4()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift3()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift2()
        {
            throw new NotImplementedException();
        }

        public override void SignInForGift1()
        {
            throw new NotImplementedException();
        }

        public override void GangGet()
        {
            GangManager.GangGet(this);
        }

        public override void GangCreate(string name)
        {
            GangManager.GangCreate(this, name);
        }

        public override void GangQuit()
        {
            GangManager.GangQuit(this);
        }

        public override void GangGetList(int page)
        {
            GangManager.GangGetList(this, page);
        }

        public override void GangGetApplies()
        {
            GangManager.GangGetApplies(this);
        }

        public override void GangApply(int id)
        {
            GangManager.GangApply(this, id);
        }

        public override void GangApplyManager(int uid, bool accept)
        {
            GangManager.GangApplyManager(this, uid, accept);
        }

        public override void GangKick(int uid)
        {
            GangManager.GangKick(this, uid);
        }

        public override void GangPosition(int uid, int position)
        {
            GangManager.GangPosition(this, uid, position);
        }

        public override void GangAutoAccept(bool autoAccept)
        {
            GangManager.GangAutoAccept(this, autoAccept);
        }

        public override void ChangeNickname(string name)
        {
            if (name.Length >= 3 && name.Length <= 12)
            {
                account.nickname = name;
                GangMember gangMember = GangManager.GetGangMemberById(account.uid);
                if (gangMember != null)
                {
                    gangMember.name = name;
                    if (gangMember.position == (int)GangMember.Position.Leader)
                    {
                        Gang gang = account.gang;
                        if (gang != null)
                            gang.leaderName = name;
                    }
                }
            }
        }
        #endregion //Override packet received from client
    }
}

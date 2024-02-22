using CSharpLike;
using KissFramework;

namespace TreasureBox
{
    /// <summary>
    /// The class for transfer JSON object between server and client, whatever the client using WebSocket or Socket.
    /// 1 Received JSON object from client by 'void OnMessage(JSONData jsonData)'. 
    /// 2 Send JSON object to client by 'void Send(JSONData jsonData)'.
    /// 3 Accept player info by account.
    /// </summary>
    public sealed class Player : PlayerBase
    {
        /// <summary>
        /// Loaded account instance
        /// </summary>
        public Account account;
        /// <summary>
        /// Received JSON object from client, that run in main thread.
        /// </summary>
        /// <param name="jsonData">The JSON object from client</param>
        public override void OnMessage(JSONData jsonData)
        {
            if (jsonData == null)
                return;
            //Set config JSON printSendAndReceived = true to enable log the send and received packet
            switch (jsonData.GetPacketType<PacketType>())
            {
                //Account system
                case PacketType.AccountLogin:
                    AccountManager.Instance.Login(jsonData, this);
                    break;

                //Item system
                case PacketType.UseItem:
                    break;

                //Mail system
                case PacketType.ReadMail:
                    account.ReadMail(jsonData["uid"]);
                    break;
                case PacketType.TakeMailAppendix:
                    account.TakeMailAppendix(jsonData["uids"]);
                    break;
                case PacketType.DeleteMail:
                    account.DeleteMail(jsonData["uids"]);
                    break;

                //SignIn system
                case PacketType.SignInForGift:
                    account.SignInForGift(this);
                    break;

                default:
                    Logger.LogError("Player : OnMessage : Unknown packet type:" + jsonData["PacketType"]);
                    break;
            }
        }
        /// <summary>
        /// Simplify callback a tips string to client
        /// </summary>
        /// <param name="tips">The tips string send to client</param>
        /// <param name="hideWaiting">Mark the client to hide the waiting panel</param>
        /// <param name="bPrintLog">Whether print a log in server</param>
        public void CallbackTip(string tips, bool hideWaiting = true, bool bPrintLog = true)
        {
            if (bPrintLog)
                Logger.LogInfo("CallbackTip : " + tips);
            JSONData jsonData = JSONData.NewPacket(PacketType.CB_Tips);
            jsonData["tips"] = tips;
            jsonData["hideWaiting"] = hideWaiting;
            Send(jsonData);
        }
        /// <summary>
        /// Simplify callback a error string to client
        /// </summary>
        /// <param name="error">The error string send to client</param>
        /// <param name="hideWaiting">Mark the client to hide the waitting panel</param>
        /// <param name="bPrintLog">Whether print a log in server</param>
        public void CallbackError(string error, bool hideWaiting = true, bool bPrintLog = true)
        {
            if (bPrintLog)
                Logger.LogError("CallbackError : " + error);
            JSONData jsonData = JSONData.NewPacket(PacketType.CB_Error);
            jsonData["error"] = error;
            jsonData["hideWaiting"] = hideWaiting;
            Send(jsonData);
        }
        /// <summary>
        /// When the player disconnect, that run in main thread.
        /// The client if not complete the close action,
        /// the server will check whether the WebSocket is dead in every 30 seconds.
        /// </summary>
        public override void OnDisconnect()
        {
            Logger.LogInfo("Player:OnDisconnect");
            if (account != null)
            {
                LogManager.LogAccount(account.uid, 1, IP);
                account.OnOffline();
            }
        }
        /// <summary>
        /// When player connected, that run in main thread
        /// </summary>
        public override void OnConnect()
        {
            Logger.LogInfo("Player:OnConnect");
        }
        /// <summary>
        /// The WebSocket occur error, that run in main thread
        /// </summary>
        public override void OnError(string msg)
        {
            Logger.LogInfo("Player:OnError:"+ msg);
        }
    }
}

/*
 * It's automatic generate by 'KissNetObject', DON'T modify this file.
 * You should modify 'Player.cs' or edit by 'KissNetObject'.
 */
using System;
using CSharpLike;
using KissFramework;
using System.Collections.Generic;

namespace TreasureBox
{
	public abstract class Player_Base : PlayerBase
	{
		/// <summary>
		/// Loaded account instance
		/// </summary>
		public Account account;
		#region Override WebSocket/Socket action
		/// <summary>
		/// When the player disconnect, that run in main thread.
		/// The client if not complete the close action,
		/// the server will check whether the WebSocket is dead in every 30 seconds.
		/// </summary>
		public override void OnDisconnect()
		{
			Logger.LogInfo("Player_Base:OnDisconnect");
		}
		/// <summary>
		/// When player connected, that run in main thread
		/// </summary>
		public override void OnConnect()
		{
			Logger.LogInfo("Player_Base:OnConnect");
		}
		/// <summary>
		/// The WebSocket occur error, that run in main thread
		/// </summary>
		public override void OnError(string msg)
		{
			Logger.LogInfo("Player_Base:OnError:" + msg);
		}
		/// <summary>
		/// Received JSON object from client, that run in main thread.
		/// </summary>
		/// <param name="jsonData">The JSON object from client</param>
		public override void OnMessage(JSONData jsonData)
		{
			if (jsonData == null || !jsonData.TryGetValue("packetType", out JSONData packetType))
			{
				OnMessageUnknownPacketType(jsonData);
				return;
			}
			switch ((string)packetType)
			{
				case "AccountLogin": AccountLogin(jsonData["name"], jsonData["acctType"], jsonData["password"]); break;
				case "ReadMail": ReadMail(jsonData["uid"]); break;
				case "DeleteMail": DeleteMail(jsonData["uids"]); break;
				case "TakeMailAppendix": TakeMailAppendix(jsonData["uids"]); break;
				case "GangGet": GangGet(); break;
				case "GangCreate": GangCreate(jsonData["name"]); break;
				case "GangQuit": GangQuit(); break;
				case "GangGetList": GangGetList(jsonData["page"]); break;
				case "GangGetApplies": GangGetApplies(); break;
				case "GangApply": GangApply(jsonData["id"]); break;
				case "GangApplyManager": GangApplyManager(jsonData["uid"], jsonData["accept"]); break;
				case "GangKick": GangKick(jsonData["uid"]); break;
				case "GangPosition": GangPosition(jsonData["uid"], jsonData["position"]); break;
				case "GangAutoAccept": GangAutoAccept(jsonData["autoAccept"]); break;
				case "SignInForGift": SignInForGift(); break;
				case "SignInForGift24": SignInForGift24(); break;
				case "SignInForGift23": SignInForGift23(); break;
				case "SignInForGift22": SignInForGift22(); break;
				case "SignInForGift21": SignInForGift21(); break;
				case "SignInForGift20": SignInForGift20(); break;
				case "SignInForGift19": SignInForGift19(); break;
				case "SignInForGift18": SignInForGift18(); break;
				case "SignInForGift17": SignInForGift17(); break;
				case "SignInForGift12": SignInForGift12(); break;
				case "SignInForGift11": SignInForGift11(); break;
				case "SignInForGift10": SignInForGift10(); break;
				case "SignInForGift9": SignInForGift9(); break;
				case "SignInForGift8": SignInForGift8(); break;
				case "SignInForGift6": SignInForGift6(); break;
				case "SignInForGift5": SignInForGift5(); break;
				case "SignInForGift4": SignInForGift4(); break;
				case "SignInForGift3": SignInForGift3(); break;
				case "SignInForGift2": SignInForGift2(); break;
				case "SignInForGift1": SignInForGift1(); break;
				case "ChangeNickname": ChangeNickname(jsonData["name"]); break;
				default: OnMessageUnknownPacketType(jsonData); break;
			}
		}
		#endregion //Override WebSocket/Socket action

		#region Process packet received from client
		public abstract void AccountLogin(string name, int acctType, string password);
		public abstract void ReadMail(int uid);
		public abstract void DeleteMail(List<int> uids);
		public abstract void TakeMailAppendix(List<int> uids);
		public abstract void GangGet();
		public abstract void GangCreate(string name);
		public abstract void GangQuit();
		public abstract void GangGetList(int page);
		public abstract void GangGetApplies();
		public abstract void GangApply(int id);
		public abstract void GangApplyManager(int uid, bool accept);
		public abstract void GangKick(int uid);
		public abstract void GangPosition(int uid, int position);
		public abstract void GangAutoAccept(bool autoAccept);
		public abstract void SignInForGift();
		public abstract void SignInForGift24();
		public abstract void SignInForGift23();
		public abstract void SignInForGift22();
		public abstract void SignInForGift21();
		public abstract void SignInForGift20();
		public abstract void SignInForGift19();
		public abstract void SignInForGift18();
		public abstract void SignInForGift17();
		public abstract void SignInForGift12();
		public abstract void SignInForGift11();
		public abstract void SignInForGift10();
		public abstract void SignInForGift9();
		public abstract void SignInForGift8();
		public abstract void SignInForGift6();
		public abstract void SignInForGift5();
		public abstract void SignInForGift4();
		public abstract void SignInForGift3();
		public abstract void SignInForGift2();
		public abstract void SignInForGift1();
		public abstract void ChangeNickname(string name);
		protected virtual void OnMessageUnknownPacketType(JSONData jsonData)
		{
			Logger.LogError($"Player_Base : OnMessageUnknownPacketType : {jsonData}");
		}
		#endregion //Process packet received from client

		#region Process packet send to client
		static JSONData _NewPacket_(string name)
        {
			JSONData _data_ = JSONData.NewDictionary();
			_data_["packetType"] = name;
			return _data_;
		}
		public void CB_Object(JSONData jsonData)
		{
			JSONData _data_ = _NewPacket_("CB_Object");
			_data_["jsonData"] = jsonData;
			Send(_data_);
		}
		public void CB_Delete(JSONData jsonData)
		{
			JSONData _data_ = _NewPacket_("CB_Delete");
			_data_["jsonData"] = jsonData;
			Send(_data_);
		}
		public void CB_Error(string error, bool hideWaiting)
		{
			JSONData _data_ = _NewPacket_("CB_Error");
			_data_["error"] = error;
			_data_["hideWaiting"] = hideWaiting;
			Send(_data_);
		}
		public void CB_Tips(string tips, bool hideWaiting)
		{
			JSONData _data_ = _NewPacket_("CB_Tips");
			_data_["tips"] = tips;
			_data_["hideWaiting"] = hideWaiting;
			Send(_data_);
		}
		public void CB_GetReward(List<int> itemIds, List<int> itemCounts)
		{
			JSONData _data_ = _NewPacket_("CB_GetReward");
			_data_["itemIds"] = itemIds;
			_data_["itemCounts"] = itemCounts;
			Send(_data_);
		}
		public void CB_AccountLogin(JSONData account)
		{
			JSONData _data_ = _NewPacket_("CB_AccountLogin");
			_data_["account"] = account;
			Send(_data_);
		}
		public void CB_GangGet(JSONData gang, JSONData members)
		{
			JSONData _data_ = _NewPacket_("CB_GangGet");
			_data_["gang"] = gang;
			_data_["members"] = members;
			Send(_data_);
		}
		public void CB_GangCreate(JSONData gang, JSONData members)
		{
			JSONData _data_ = _NewPacket_("CB_GangCreate");
			_data_["gang"] = gang;
			_data_["members"] = members;
			Send(_data_);
		}
		public void CB_GangQuit() => Send(_NewPacket_("CB_GangQuit"));
		public void CB_GangGetList(int page, int allCount, JSONData gangs)
		{
			JSONData _data_ = _NewPacket_("CB_GangGetList");
			_data_["page"] = page;
			_data_["allCount"] = allCount;
			_data_["gangs"] = gangs;
			Send(_data_);
		}
		public void CB_GangGetApplies(JSONData members)
		{
			JSONData _data_ = _NewPacket_("CB_GangGetApplies");
			_data_["members"] = members;
			Send(_data_);
		}
		public void CB_GangApply(int id)
		{
			JSONData _data_ = _NewPacket_("CB_GangApply");
			_data_["id"] = id;
			Send(_data_);
		}
		public void CB_GangApplyManager(int uid, bool accept, JSONData member)
		{
			JSONData _data_ = _NewPacket_("CB_GangApplyManager");
			_data_["uid"] = uid;
			_data_["accept"] = accept;
			_data_["member"] = member;
			Send(_data_);
		}
		public void CB_GangKick(int uid)
		{
			JSONData _data_ = _NewPacket_("CB_GangKick");
			_data_["uid"] = uid;
			Send(_data_);
		}
		public void CB_GangPosition(int uid, int position)
		{
			JSONData _data_ = _NewPacket_("CB_GangPosition");
			_data_["uid"] = uid;
			_data_["position"] = position;
			Send(_data_);
		}
		public void CB_GangAutoAccept(bool autoAccept)
		{
			JSONData _data_ = _NewPacket_("CB_GangAutoAccept");
			_data_["autoAccept"] = autoAccept;
			Send(_data_);
		}
		#endregion //Process packet send to client
	}
}

/*
 * It's automatic generate by 'KissNetObject', DON'T modify this file.
 * You should modify 'GangMember.cs' or edit by 'KissNetObject'.
 */

using KissFramework;
using System;
using CSharpLike;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreasureBox
{
	public abstract class GangMember_Base : NetObject<GangMember, Account>
	{
		#region Update
		/// <summary>
		/// For internal call only. You can override it if you want customize it.
		/// </summary>
		public override void Update(ref string _strSQL_, ref MySqlParameter[] _mySqlParameters_)
		{
			StringBuilder _sb_ = new StringBuilder();
			_sb_.Append("UPDATE `GangMember` SET ");
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			if (HasUpdate(4ul))//UpdateMask.nameMask)
			{
				_sb_.Append("`name` = @name,");
				_param_ = new MySqlParameter("@name", MySqlDbType.String);
				_param_.Value = _attribute_.name;
				_ps_.Add(_param_);
			}
			if (HasUpdate(8ul))//UpdateMask.gangIdMask)
			{
				_sb_.Append("`gangId` = @gangId,");
				_param_ = new MySqlParameter("@gangId", MySqlDbType.Int32);
				_param_.Value = _attribute_.gangId;
				_ps_.Add(_param_);
			}
			if (HasUpdate(16ul))//UpdateMask.positionMask)
			{
				_sb_.Append("`position` = @position,");
				_param_ = new MySqlParameter("@position", MySqlDbType.Int32);
				_param_.Value = _attribute_.position;
				_ps_.Add(_param_);
			}
			if (HasUpdate(32ul))//UpdateMask.lvMask)
			{
				_sb_.Append("`lv` = @lv,");
				_param_ = new MySqlParameter("@lv", MySqlDbType.Int32);
				_param_.Value = _attribute_.lv;
				_ps_.Add(_param_);
			}
			if (HasUpdate(64ul))//UpdateMask.offlineMask)
			{
				_sb_.Append("`offline` = @offline,");
				_param_ = new MySqlParameter("@offline", MySqlDbType.DateTime);
				_param_.Value = _attribute_.offline;
				_ps_.Add(_param_);
				_sb_.Append("`online` = @online,");
				_param_ = new MySqlParameter("@online", MySqlDbType.UByte);
				_param_.Value = _attribute_.online;
				_ps_.Add(_param_);
			}
			_waitingUpdate_ = false;
			ClearUpdateMask();
			if (_ps_.Count > 0)
			{
				_sb_.Remove(_sb_.Length - 1, 1);
				_sb_.Append(" WHERE `uid` = @uid");
				_param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
				_param_.Value = uid;
				_ps_.Add(_param_);
				_strSQL_ = _sb_.ToString();
				_mySqlParameters_ = _ps_.ToArray();
	}
			else
			{
				Logger.LogWarning("No need update 'GangMember', you should call 'GangMember.Update()' or 'GangMember.UpdateImmediately()' after change something need to update to database.");
			}
		}
		#endregion //Update

		#region Select
		protected override void SetData(DataRow data)
		{
			_attribute_.uid = Convert.ToInt32(data["uid"]);
			_attribute_.acctId = Convert.ToInt32(data["acctId"]);
			_attribute_.name = Convert.ToString(data["name"]);
			_attribute_.gangId = Convert.ToInt32(data["gangId"]);
			_attribute_.position = Convert.ToInt32(data["position"]);
			_attribute_.lv = Convert.ToInt32(data["lv"]);
			_attribute_.offline = Convert2DateTime(data["offline"]);
			_attribute_.online = Convert.ToBoolean(data["online"]);
		}
		/// <summary>
		/// Select all data from database. The select operation run in background thread. The callback action occur after database operation done.
		/// </summary>
		/// <param name="callback">This callback occur after database operation done.</param>
		public static void SelectAll(Action<List<GangMember>, string> callback)
		{
			Select("SELECT * FROM `GangMember`", new List<MySqlParameter>(), callback);
		}
		/// <summary>
		/// Select data from database by acctId. The select operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void SelectByAcctId(int acctId, Action<List<GangMember>, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			Select("SELECT * FROM `GangMember` WHERE `acctId` = @acctId", _ps_, _callback_);
		}
		#endregion //Select

		#region Delete
		/// <summary>
		/// Delete self from database. The delete operation run in background thread. The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public void Delete(Action<int, string> _callback_ = null)
		{
			StringBuilder _sb_ = new StringBuilder();
			MySqlParameter _param_;
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			_sb_.Append("DELETE FROM `GangMember`");
			_sb_.Append(" WHERE `uid` = @uid");
			_param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
			_param_.Value = uid;
			_ps_.Add(_param_);
			Delete(_sb_.ToString(), _ps_, _callback_);
		}
		/// <summary>
		/// Delete data from database by acctId. The delete operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void DeleteByAcctId(int acctId, Action<int, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			Delete("DELETE FROM `GangMember` WHERE `acctId` = @acctId", _ps_, _callback_);
		}
		/// <summary>
		/// Delete data from database by uid. The delete operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void DeleteByUid(int uid, Action<int, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
			_param_.Value = uid;
			_ps_.Add(_param_);
			Delete("DELETE FROM `GangMember` WHERE `uid` = @uid", _ps_, _callback_);
		}
		#endregion //Delete

		#region Insert
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (All params)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int acctId, string name, int gangId, int position, int lv, DateTime offline, bool online, Action<GangMember, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@name", MySqlDbType.String);
			_param_.Value = name;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@gangId", MySqlDbType.Int32);
			_param_.Value = gangId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@position", MySqlDbType.Int32);
			_param_.Value = position;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@lv", MySqlDbType.Int32);
			_param_.Value = lv;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@offline", MySqlDbType.DateTime);
			_param_.Value = offline;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@online", MySqlDbType.UByte);
			_param_.Value = online;
			_ps_.Add(_param_);
			Insert("INSERT INTO `GangMember` (`acctId`,`name`,`gangId`,`position`,`lv`,`offline`,`online`) VALUES (@acctId,@name,@gangId,@position,@lv,@offline,@online)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					GangMember _gangMember_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_gangMember_ = new GangMember();
						_gangMember_._attribute_.uid = (int)_lastInsertedId_;
						_gangMember_._attribute_.acctId = acctId;
						_gangMember_._attribute_.name = name;
						_gangMember_._attribute_.gangId = gangId;
						_gangMember_._attribute_.position = position;
						_gangMember_._attribute_.lv = lv;
						_gangMember_._attribute_.offline = offline;
						_gangMember_._attribute_.online = online;
					}
					_callback_?.Invoke(_gangMember_, _error_);
				});
		}
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (Selected param only.)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int acctId, int gangId, Action<GangMember, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@gangId", MySqlDbType.Int32);
			_param_.Value = gangId;
			_ps_.Add(_param_);
			Insert("INSERT INTO `GangMember` (`acctId`,`gangId`) VALUES (@acctId,@gangId)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					GangMember _gangMember_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_gangMember_ = new GangMember();
						_gangMember_._attribute_.uid = (int)_lastInsertedId_;
						_gangMember_._attribute_.acctId = acctId;
						_gangMember_._attribute_.gangId = gangId;
					}
					_callback_?.Invoke(_gangMember_, _error_);
				});
		}

		#endregion //Insert

		#region Property
		public enum UpdateMask : ulong
		{
			UseSendMask_ = 0ul,
			uidMask = 1ul,
			acctIdMask = 2ul,
			nameMask = 4ul,
			gangIdMask = 8ul,
			positionMask = 16ul,
			lvMask = 32ul,
			offlineMask = 64ul,
			AllMask_ = 127ul
		};

		[KissJsonDontSerialize]
		protected override ulong DefaultSendMask => 127ul;
		[KissJsonSerializeProperty]
		public int uid
		{
			get => _attribute_.uid; 
			set => _attribute_.uid = value;
		}
		[KissJsonSerializeProperty]
		public int acctId
		{
			get => _attribute_.acctId; 
			set => _attribute_.acctId = value;
		}
		[KissJsonSerializeProperty]
		public string name
		{
			get => _attribute_.name; 
			set
			{
				if (_attribute_.name == value) return;
				_attribute_.name = value;
				MarkUpdateAndModifyMask(4ul);//UpdateMask.nameMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangmembers");
			}
		}
		[KissJsonSerializeProperty]
		public int gangId
		{
			get => _attribute_.gangId; 
			set
			{
				if (_attribute_.gangId == value) return;
				_attribute_.gangId = value;
				MarkUpdateAndModifyMask(8ul);//UpdateMask.gangIdMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangmembers");
			}
		}
		[KissJsonSerializeProperty]
		public int position
		{
			get => _attribute_.position; 
			set
			{
				if (_attribute_.position == value) return;
				_attribute_.position = value;
				MarkUpdateAndModifyMask(16ul);//UpdateMask.positionMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangmembers");
			}
		}
		[KissJsonSerializeProperty]
		public int lv
		{
			get => _attribute_.lv; 
			set
			{
				if (_attribute_.lv == value) return;
				_attribute_.lv = value;
				MarkUpdateAndModifyMask(32ul);//UpdateMask.lvMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangmembers");
			}
		}
		[KissJsonSerializeProperty]
		public DateTime offline
		{
			get => _attribute_.offline; 
			set
			{
				if (_attribute_.offline == value) return;
				_attribute_.offline = value;
				MarkUpdateAndModifyMask(64ul);//UpdateMask.offlineMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangmembers");
			}
		}
		[KissJsonSerializeProperty]
		public bool online
		{
			get => _attribute_.online; 
			set
			{
				if (_attribute_.online == value) return;
				_attribute_.online = value;
				MarkUpdateAndModifyMask(64ul);//UpdateMask.offlineMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangmembers");
			}
		}
		#endregion //Property

		#region JSON
		/// <summary>
		/// Convert this object to JSONData with custom mask.
		/// </summary>
		/// <param name="mask">Which property you want to be included, default is send mask and the send mask default is all proterty.</param>
		public override JSONData ToJSONData(ulong mask = 0ul)
		{
			JSONData _jsonData_ = JSONData.NewDictionary();
			if (mask == 0ul) mask = GetSendMask();
			if ((mask & 1ul) > 0)//UpdateMask.uidMask
				_jsonData_["uid"] = _attribute_.uid;
			if ((mask & 2ul) > 0)//UpdateMask.acctIdMask
				_jsonData_["acctId"] = _attribute_.acctId;
			if ((mask & 4ul) > 0)//UpdateMask.nameMask
				_jsonData_["name"] = _attribute_.name;
			if ((mask & 8ul) > 0)//UpdateMask.gangIdMask
				_jsonData_["gangId"] = _attribute_.gangId;
			if ((mask & 16ul) > 0)//UpdateMask.positionMask
				_jsonData_["position"] = _attribute_.position;
			if ((mask & 32ul) > 0)//UpdateMask.lvMask
				_jsonData_["lv"] = _attribute_.lv;
			if ((mask & 64ul) > 0)//UpdateMask.offlineMask
			{
				_jsonData_["offline"] = _attribute_.offline;
				_jsonData_["online"] = _attribute_.online;
			}

			_jsonData_["_uid_"] = _uid_;
			_jsonData_["_sendMask_"] = mask;
			return _jsonData_;
		}
		/// <summary>
		/// Clone data from the GangMember instance with custom mask.
		/// <param name="_source_">The source GangMember</param>
		/// <param name="_mask_">Which part you want to clone, default is all.</param>
		/// </summary>
		public void Clone(GangMember _source_, ulong _mask_ = ulong.MaxValue)
		{
			if ((_mask_ & 1ul) > 0)//UpdateMask.uidMask
				uid = _source_.uid;
			if ((_mask_ & 2ul) > 0)//UpdateMask.acctIdMask
				acctId = _source_.acctId;
			if ((_mask_ & 4ul) > 0)//UpdateMask.nameMask
				name = _source_.name;
			if ((_mask_ & 8ul) > 0)//UpdateMask.gangIdMask
				gangId = _source_.gangId;
			if ((_mask_ & 16ul) > 0)//UpdateMask.positionMask
				position = _source_.position;
			if ((_mask_ & 32ul) > 0)//UpdateMask.lvMask
				lv = _source_.lv;
			if ((_mask_ & 64ul) > 0)//UpdateMask.offlineMask
			{
				offline = _source_.offline;
				online = _source_.online;
			}

		}
		#endregion //JSON

		#region PrivateFields
		[KissJsonDontSerialize]
		private struct _fields_
		{
			// uid
			public int uid;

			// acctId
			public int acctId;

			// name
			public string name;

			// gangId
			public int gangId;

			// position
			public int position;

			// lv
			public int lv;

			// offline
			public DateTime offline;
			public bool online;

		}
		[KissJsonDontSerialize]
		public bool SyncToDB = true;
		private _fields_ _attribute_ = new _fields_();
		#endregion //PrivateFields
	}
}

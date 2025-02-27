/*
 * It's automatic generate by 'KissNetObject', DON'T modify this file.
 * You should modify 'Gang.cs' or edit by 'KissNetObject'.
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
	public abstract class Gang_Base : NetObject<Gang, Account>
	{
		#region Update
		/// <summary>
		/// For internal call only. You can override it if you want customize it.
		/// </summary>
		public override void Update(ref string _strSQL_, ref MySqlParameter[] _mySqlParameters_)
		{
			StringBuilder _sb_ = new StringBuilder();
			_sb_.Append("UPDATE `Gang` SET ");
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			if (HasUpdate(1ul))//UpdateMask.uidMask)
			{
				_sb_.Append("`name` = @name,");
				_param_ = new MySqlParameter("@name", MySqlDbType.String);
				_param_.Value = _attribute_.name;
				_ps_.Add(_param_);
			}
			if (HasUpdate(2ul))//UpdateMask.leaderIdMask)
			{
				_sb_.Append("`leaderId` = @leaderId,");
				_param_ = new MySqlParameter("@leaderId", MySqlDbType.Int32);
				_param_.Value = _attribute_.leaderId;
				_ps_.Add(_param_);
				_sb_.Append("`leaderName` = @leaderName,");
				_param_ = new MySqlParameter("@leaderName", MySqlDbType.String);
				_param_.Value = _attribute_.leaderName;
				_ps_.Add(_param_);
			}
			if (HasUpdate(4ul))//UpdateMask.lvMask)
			{
				_sb_.Append("`lv` = @lv,");
				_param_ = new MySqlParameter("@lv", MySqlDbType.Int32);
				_param_.Value = _attribute_.lv;
				_ps_.Add(_param_);
			}
			if (HasUpdate(8ul))//UpdateMask.expMask)
			{
				_sb_.Append("`exp` = @exp,");
				_param_ = new MySqlParameter("@exp", MySqlDbType.Int32);
				_param_.Value = _attribute_.exp;
				_ps_.Add(_param_);
				_sb_.Append("`logs` = @logs,");
				_param_ = new MySqlParameter("@logs", MySqlDbType.String);
				_param_.Value = _attribute_.logs;
				_ps_.Add(_param_);
			}
			if (HasUpdate(16ul))//UpdateMask.countMask)
			{
				_sb_.Append("`count` = @count,");
				_param_ = new MySqlParameter("@count", MySqlDbType.Int32);
				_param_.Value = _attribute_.count;
				_ps_.Add(_param_);
			}
			if (HasUpdate(32ul))//UpdateMask.appliesMask)
			{
				_sb_.Append("`applies` = @applies,");
				_param_ = new MySqlParameter("@applies", MySqlDbType.String);
				_param_.Value = _attribute_.applies;
				_ps_.Add(_param_);
			}
			if (HasUpdate(64ul))//UpdateMask.autoAcceptMask)
			{
				_sb_.Append("`autoAccept` = @autoAccept,");
				_param_ = new MySqlParameter("@autoAccept", MySqlDbType.UByte);
				_param_.Value = _attribute_.autoAccept;
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
				Logger.LogWarning("No need update 'Gang', you should call 'Gang.Update()' or 'Gang.UpdateImmediately()' after change something need to update to database.");
			}
		}
		#endregion //Update

		#region Select
		protected override void SetData(DataRow data)
		{
			_attribute_.uid = Convert.ToInt32(data["uid"]);
			_attribute_.name = Convert.ToString(data["name"]);
			_attribute_.leaderId = Convert.ToInt32(data["leaderId"]);
			_attribute_.leaderName = Convert.ToString(data["leaderName"]);
			_attribute_.lv = Convert.ToInt32(data["lv"]);
			_attribute_.exp = Convert.ToInt32(data["exp"]);
			_attribute_.logs = Convert.ToString(data["logs"]);
			_attribute_.count = Convert.ToInt32(data["count"]);
			_attribute_.applies = Convert.ToString(data["applies"]);
			_attribute_.autoAccept = Convert.ToBoolean(data["autoAccept"]);
		}
		/// <summary>
		/// Select all data from database. The select operation run in background thread. The callback action occur after database operation done.
		/// </summary>
		/// <param name="callback">This callback occur after database operation done.</param>
		public static void SelectAll(Action<List<Gang>, string> callback)
		{
			Select("SELECT * FROM `Gang`", new List<MySqlParameter>(), callback);
		}
		/// <summary>
		/// Select data from database by leaderId. The select operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void SelectByLeaderId(int leaderId, Action<List<Gang>, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@leaderId", MySqlDbType.Int32);
			_param_.Value = leaderId;
			_ps_.Add(_param_);
			Select("SELECT * FROM `Gang` WHERE `leaderId` = @leaderId", _ps_, _callback_);
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
			_sb_.Append("DELETE FROM `Gang`");
			_sb_.Append(" WHERE `uid` = @uid");
			_param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
			_param_.Value = uid;
			_ps_.Add(_param_);
			Delete(_sb_.ToString(), _ps_, _callback_);
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
			Delete("DELETE FROM `Gang` WHERE `uid` = @uid", _ps_, _callback_);
		}
		#endregion //Delete

		#region Insert
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (All params)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(string name, int leaderId, string leaderName, int lv, int exp, List<string> logs, int count, List<int> applies, bool autoAccept, Action<Gang, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@name", MySqlDbType.String);
			_param_.Value = name;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@leaderId", MySqlDbType.Int32);
			_param_.Value = leaderId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@leaderName", MySqlDbType.String);
			_param_.Value = leaderName;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@lv", MySqlDbType.Int32);
			_param_.Value = lv;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@exp", MySqlDbType.Int32);
			_param_.Value = exp;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@logs", MySqlDbType.String);
			string _logs = NetObjectUtils.ListToString(logs);
			_param_.Value = _logs;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@count", MySqlDbType.Int32);
			_param_.Value = count;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@applies", MySqlDbType.String);
			string _applies = NetObjectUtils.ListToString(applies);
			_param_.Value = _applies;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@autoAccept", MySqlDbType.UByte);
			_param_.Value = autoAccept;
			_ps_.Add(_param_);
			Insert("INSERT INTO `Gang` (`name`,`leaderId`,`leaderName`,`lv`,`exp`,`logs`,`count`,`applies`,`autoAccept`) VALUES (@name,@leaderId,@leaderName,@lv,@exp,@logs,@count,@applies,@autoAccept)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					Gang _gang_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_gang_ = new Gang();
						_gang_._attribute_.uid = (int)_lastInsertedId_;
						_gang_._attribute_.name = name;
						_gang_._attribute_.leaderId = leaderId;
						_gang_._attribute_.leaderName = leaderName;
						_gang_._attribute_.lv = lv;
						_gang_._attribute_.exp = exp;
						_gang_._attribute_.logs = _logs;
						_gang_._attribute_.count = count;
						_gang_._attribute_.applies = _applies;
						_gang_._attribute_.autoAccept = autoAccept;
					}
					_callback_?.Invoke(_gang_, _error_);
				});
		}
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (Selected param only.)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(string name, int leaderId, Action<Gang, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@name", MySqlDbType.String);
			_param_.Value = name;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@leaderId", MySqlDbType.Int32);
			_param_.Value = leaderId;
			_ps_.Add(_param_);
			Insert("INSERT INTO `Gang` (`name`,`leaderId`) VALUES (@name,@leaderId)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					Gang _gang_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_gang_ = new Gang();
						_gang_._attribute_.uid = (int)_lastInsertedId_;
						_gang_._attribute_.name = name;
						_gang_._attribute_.leaderId = leaderId;
					}
					_callback_?.Invoke(_gang_, _error_);
				});
		}

		#endregion //Insert

		#region Property
		public enum UpdateMask : ulong
		{
			UseSendMask_ = 0ul,
			uidMask = 1ul,
			leaderIdMask = 2ul,
			lvMask = 4ul,
			expMask = 8ul,
			countMask = 16ul,
			appliesMask = 32ul,
			autoAcceptMask = 64ul,
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
		public string name
		{
			get => _attribute_.name; 
			set
			{
				if (_attribute_.name == value) return;
				_attribute_.name = value;
				MarkUpdateAndModifyMask(1ul);//UpdateMask.uidMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
			}
		}
		[KissJsonSerializeProperty]
		public int leaderId
		{
			get => _attribute_.leaderId; 
			set
			{
				if (_attribute_.leaderId == value) return;
				_attribute_.leaderId = value;
				MarkUpdateAndModifyMask(2ul);//UpdateMask.leaderIdMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
			}
		}
		[KissJsonSerializeProperty]
		public string leaderName
		{
			get => _attribute_.leaderName; 
			set
			{
				if (_attribute_.leaderName == value) return;
				_attribute_.leaderName = value;
				MarkUpdateAndModifyMask(2ul);//UpdateMask.leaderIdMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
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
				MarkUpdateAndModifyMask(4ul);//UpdateMask.lvMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
			}
		}
		[KissJsonSerializeProperty]
		public int exp
		{
			get => _attribute_.exp; 
			set
			{
				if (_attribute_.exp == value) return;
				_attribute_.exp = value;
				MarkUpdateAndModifyMask(8ul);//UpdateMask.expMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
			}
		}
		#region Property_logs
		[KissJsonSerializeProperty]
		string _logs
		{
			get => _attribute_.logs; 
			set
			{
				if (_attribute_.logs == value) return;
				_attribute_.logs = value;
				MarkUpdateAndModifyMask(8ul);//UpdateMask.expMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
			}
		}
		[KissJsonDontSerialize]
		List<string> __logs;
		[KissJsonDontSerialize]
		public List<string> logs
		{
			get
			{
				if (__logs == null) __logs = NetObjectUtils.StringToList<string>(_logs);
				return __logs;
			}
			set
			{
				__logs = value;
				logsMarkChanged();
			}
		}
		/// <summary>
		///Call this function after you modified logs (e.g. insert/add/remove/clear operation.). 
		/// <summary>
		public void logsMarkChanged()
		{
			_logs = NetObjectUtils.ListToString(logs);
		}
		#endregion Property_logs
		[KissJsonSerializeProperty]
		public int count
		{
			get => _attribute_.count; 
			set
			{
				if (_attribute_.count == value) return;
				_attribute_.count = value;
				MarkUpdateAndModifyMask(16ul);//UpdateMask.countMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
			}
		}
		#region Property_applies
		[KissJsonSerializeProperty]
		string _applies
		{
			get => _attribute_.applies; 
			set
			{
				if (_attribute_.applies == value) return;
				_attribute_.applies = value;
				MarkUpdateAndModifyMask(32ul);//UpdateMask.appliesMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
			}
		}
		[KissJsonDontSerialize]
		List<int> __applies;
		[KissJsonDontSerialize]
		public List<int> applies
		{
			get
			{
				if (__applies == null) __applies = NetObjectUtils.StringToList<int>(_applies);
				return __applies;
			}
			set
			{
				__applies = value;
				appliesMarkChanged();
			}
		}
		/// <summary>
		///Call this function after you modified applies (e.g. insert/add/remove/clear operation.). 
		/// <summary>
		public void appliesMarkChanged()
		{
			_applies = NetObjectUtils.ListToString(applies);
		}
		#endregion Property_applies
		[KissJsonSerializeProperty]
		public bool autoAccept
		{
			get => _attribute_.autoAccept; 
			set
			{
				if (_attribute_.autoAccept == value) return;
				_attribute_.autoAccept = value;
				MarkUpdateAndModifyMask(64ul);//UpdateMask.autoAcceptMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("gangs");
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
			{
				_jsonData_["uid"] = _attribute_.uid;
				_jsonData_["name"] = _attribute_.name;
			}
			if ((mask & 2ul) > 0)//UpdateMask.leaderIdMask
			{
				_jsonData_["leaderId"] = _attribute_.leaderId;
				_jsonData_["leaderName"] = _attribute_.leaderName;
			}
			if ((mask & 4ul) > 0)//UpdateMask.lvMask
				_jsonData_["lv"] = _attribute_.lv;
			if ((mask & 8ul) > 0)//UpdateMask.expMask
			{
				_jsonData_["exp"] = _attribute_.exp;
				_jsonData_["_logs"] = _attribute_.logs;
			}
			if ((mask & 16ul) > 0)//UpdateMask.countMask
				_jsonData_["count"] = _attribute_.count;
			if ((mask & 32ul) > 0)//UpdateMask.appliesMask
				_jsonData_["_applies"] = _attribute_.applies;
			if ((mask & 64ul) > 0)//UpdateMask.autoAcceptMask
				_jsonData_["autoAccept"] = _attribute_.autoAccept;

			_jsonData_["_uid_"] = _uid_;
			_jsonData_["_sendMask_"] = mask;
			return _jsonData_;
		}
		/// <summary>
		/// Clone data from the Gang instance with custom mask.
		/// <param name="_source_">The source Gang</param>
		/// <param name="_mask_">Which part you want to clone, default is all.</param>
		/// </summary>
		public void Clone(Gang _source_, ulong _mask_ = ulong.MaxValue)
		{
			if ((_mask_ & 1ul) > 0)//UpdateMask.uidMask
			{
				uid = _source_.uid;
				name = _source_.name;
			}
			if ((_mask_ & 2ul) > 0)//UpdateMask.leaderIdMask
			{
				leaderId = _source_.leaderId;
				leaderName = _source_.leaderName;
			}
			if ((_mask_ & 4ul) > 0)//UpdateMask.lvMask
				lv = _source_.lv;
			if ((_mask_ & 8ul) > 0)//UpdateMask.expMask
			{
				exp = _source_.exp;
				_logs = _source_._logs;
			}
			if ((_mask_ & 16ul) > 0)//UpdateMask.countMask
				count = _source_.count;
			if ((_mask_ & 32ul) > 0)//UpdateMask.appliesMask
				_applies = _source_._applies;
			if ((_mask_ & 64ul) > 0)//UpdateMask.autoAcceptMask
				autoAccept = _source_.autoAccept;

		}
		#endregion //JSON

		#region PrivateFields
		[KissJsonDontSerialize]
		private struct _fields_
		{
			// uid
			public int uid;
			public string name;

			// leaderId
			public int leaderId;
			public string leaderName;

			// lv
			public int lv;

			// exp
			public int exp;
			public string logs;

			// count
			public int count;

			// applies
			public string applies;

			// autoAccept
			public bool autoAccept;

		}
		[KissJsonDontSerialize]
		public bool SyncToDB = true;
		private _fields_ _attribute_ = new _fields_();
		#endregion //PrivateFields
	}
}

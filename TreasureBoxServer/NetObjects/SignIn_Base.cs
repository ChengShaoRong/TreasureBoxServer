/*
 * It's automatic generate by 'KissNetObject', DON'T modify this file.
 * You should modify 'SignIn.cs' or edit by 'KissNetObject'.
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
	public abstract class SignIn_Base : NetObject<SignIn, Account>
	{
		#region Update
		/// <summary>
		/// For internal call only. You can override it if you want customize it.
		/// </summary>
		public override void Update(ref string _strSQL_, ref MySqlParameter[] _mySqlParameters_)
		{
			StringBuilder _sb_ = new StringBuilder();
			_sb_.Append("UPDATE `SignIn` SET ");
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			if (HasUpdate(2ul))//UpdateMask.monthMask)
			{
				_sb_.Append("`month` = @month,");
				_param_ = new MySqlParameter("@month", MySqlDbType.Int32);
				_param_.Value = _attribute_.month;
				_ps_.Add(_param_);
			}
			if (HasUpdate(4ul))//UpdateMask.signInListMask)
			{
				_sb_.Append("`signInList` = @signInList,");
				_param_ = new MySqlParameter("@signInList", MySqlDbType.String);
				_param_.Value = _attribute_.signInList;
				_ps_.Add(_param_);
			}
			_waitingUpdate_ = false;
			ClearUpdateMask();
			if (_ps_.Count > 0)
			{
				_sb_.Remove(_sb_.Length - 1, 1);
				_sb_.Append(" WHERE `acctId` = @acctId");
				_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
				_param_.Value = acctId;
				_ps_.Add(_param_);
				_strSQL_ = _sb_.ToString();
				_mySqlParameters_ = _ps_.ToArray();
	}
			else
			{
				Logger.LogWarning("No need update 'SignIn', you should call 'SignIn.Update()' or 'SignIn.UpdateImmediately()' after change something need to update to database.");
			}
		}
		#endregion //Update

		#region Select
		protected override void SetData(DataRow data)
		{
			_attribute_.acctId = Convert.ToInt32(data["acctId"]);
			_attribute_.month = Convert.ToInt32(data["month"]);
			_attribute_.signInList = Convert.ToString(data["signInList"]);
		}
		/// <summary>
		/// Select all data from database. The select operation run in background thread. The callback action occur after database operation done.
		/// </summary>
		/// <param name="callback">This callback occur after database operation done.</param>
		public static void SelectAll(Action<List<SignIn>, string> callback)
		{
			Select("SELECT * FROM `SignIn`", new List<MySqlParameter>(), callback);
		}
		/// <summary>
		/// Select data from database by acctId. The select operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void SelectByAcctId(int acctId, Action<List<SignIn>, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			Select("SELECT * FROM `SignIn` WHERE `acctId` = @acctId", _ps_, _callback_);
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
			_sb_.Append("DELETE FROM `SignIn`");
			_sb_.Append(" WHERE `acctId` = @acctId");
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
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
			Delete("DELETE FROM `SignIn` WHERE `acctId` = @acctId", _ps_, _callback_);
		}
		#endregion //Delete

		#region Insert
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (All params)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int acctId, int month, string signInList, Action<SignIn, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@month", MySqlDbType.Int32);
			_param_.Value = month;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@signInList", MySqlDbType.String);
			_param_.Value = signInList;
			_ps_.Add(_param_);
			Insert("INSERT INTO `SignIn` (`acctId`,`month`,`signInList`) VALUES (@acctId,@month,@signInList)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					SignIn _signIn_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_signIn_ = new SignIn();
						_signIn_._attribute_.acctId = acctId;
						_signIn_._attribute_.month = month;
						_signIn_._attribute_.signInList = signInList;
					}
					_callback_?.Invoke(_signIn_, _error_);
				});
		}
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (Selected param only.)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int acctId, Action<SignIn, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			Insert("INSERT INTO `SignIn` (`acctId`) VALUES (@acctId)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					SignIn _signIn_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_signIn_ = new SignIn();
						_signIn_._attribute_.acctId = acctId;
					}
					_callback_?.Invoke(_signIn_, _error_);
				});
		}

		#endregion //Insert

		#region Property
		public enum UpdateMask : ulong
		{
			UseSendMask_ = 0ul,
			acctIdMask = 1ul,
			monthMask = 2ul,
			signInListMask = 4ul,
			AllMask_ = 7ul
		};

		[KissJsonDontSerialize]
		protected override ulong DefaultSendMask => 7ul;
		[KissJsonSerializeProperty]
		public int acctId
		{
			get => _attribute_.acctId; 
			set => _attribute_.acctId = value;
		}
		[KissJsonSerializeProperty]
		public int month
		{
			get => _attribute_.month; 
			set
			{
				if (_attribute_.month == value) return;
				_attribute_.month = value;
				MarkUpdateAndModifyMask(2ul);//UpdateMask.monthMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("signIn");
			}
		}
		[KissJsonSerializeProperty]
		public string signInList
		{
			get => _attribute_.signInList; 
			set
			{
				if (_attribute_.signInList == value) return;
				_attribute_.signInList = value;
				MarkUpdateAndModifyMask(4ul);//UpdateMask.signInListMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("signIn");
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
			if ((mask & 1ul) > 0)//UpdateMask.acctIdMask
				_jsonData_["acctId"] = _attribute_.acctId;
			if ((mask & 2ul) > 0)//UpdateMask.monthMask
				_jsonData_["month"] = _attribute_.month;
			if ((mask & 4ul) > 0)//UpdateMask.signInListMask
				_jsonData_["signInList"] = _attribute_.signInList;

			_jsonData_["_uid_"] = _uid_;
			_jsonData_["_sendMask_"] = mask;
			return _jsonData_;
		}
		/// <summary>
		/// Clone data from the SignIn instance with custom mask.
		/// <param name="_source_">The source SignIn</param>
		/// <param name="_mask_">Which part you want to clone, default is all.</param>
		/// </summary>
		public void Clone(SignIn _source_, ulong _mask_ = ulong.MaxValue)
		{
			if ((_mask_ & 1ul) > 0)//UpdateMask.acctIdMask
				acctId = _source_.acctId;
			if ((_mask_ & 2ul) > 0)//UpdateMask.monthMask
				month = _source_.month;
			if ((_mask_ & 4ul) > 0)//UpdateMask.signInListMask
				signInList = _source_.signInList;

		}
		#endregion //JSON

		#region PrivateFields
		[KissJsonDontSerialize]
		private struct _fields_
		{
			// acctId
			public int acctId;

			// month
			public int month;

			// signInList
			public string signInList;

		}
		[KissJsonDontSerialize]
		public bool SyncToDB = true;
		private _fields_ _attribute_ = new _fields_();
		#endregion //PrivateFields
	}
}

/*
 * It's automatic generate by 'KissNetObject', DON'T modify this file.
 * You should modify 'Item.cs' or edit by 'KissNetObject'.
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
	public abstract class Item_Base : NetObject<Item, Account>
	{
		#region Update
		/// <summary>
		/// For internal call only. You can override it if you want customize it.
		/// </summary>
		public override void Update(ref string _strSQL_, ref MySqlParameter[] _mySqlParameters_)
		{
			StringBuilder _sb_ = new StringBuilder();
			_sb_.Append("UPDATE `Item` SET ");
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			if (HasUpdate(2ul))//UpdateMask.countMask)
			{
				_sb_.Append("`count` = @count,");
				_param_ = new MySqlParameter("@count", MySqlDbType.Int32);
				_param_.Value = _attribute_.count;
				_ps_.Add(_param_);
			}
			if (HasUpdate(4ul))//UpdateMask.testListMask)
			{
				_sb_.Append("`testList` = @testList,");
				_param_ = new MySqlParameter("@testList", MySqlDbType.String);
				_param_.Value = _attribute_.testList;
				_ps_.Add(_param_);
				_sb_.Append("`testDic` = @testDic,");
				_param_ = new MySqlParameter("@testDic", MySqlDbType.String);
				_param_.Value = _attribute_.testDic;
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
				Logger.LogWarning("No need update 'Item', you should call 'Item.Update()' or 'Item.UpdateImmediately()' after change something need to update to database.");
			}
		}
		#endregion //Update

		#region Select
		protected override void SetData(DataRow data)
		{
			_attribute_.uid = Convert.ToInt32(data["uid"]);
			_attribute_.itemId = Convert.ToInt32(data["itemId"]);
			_attribute_.acctId = Convert.ToInt32(data["acctId"]);
			_attribute_.count = Convert.ToInt32(data["count"]);
			_attribute_.testList = Convert.ToString(data["testList"]);
			_attribute_.testDic = Convert.ToString(data["testDic"]);
		}
		/// <summary>
		/// Select all data from database. The select operation run in background thread. The callback action occur after database operation done.
		/// </summary>
		/// <param name="callback">This callback occur after database operation done.</param>
		public static void SelectAll(Action<List<Item>, string> callback)
		{
			Select("SELECT * FROM `Item`", new List<MySqlParameter>(), callback);
		}
		/// <summary>
		/// Select data from database by acctId. The select operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void SelectByAcctId(int acctId, Action<List<Item>, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			Select("SELECT * FROM `Item` WHERE `acctId` = @acctId", _ps_, _callback_);
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
			_sb_.Append("DELETE FROM `Item`");
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
			Delete("DELETE FROM `Item` WHERE `acctId` = @acctId", _ps_, _callback_);
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
			Delete("DELETE FROM `Item` WHERE `uid` = @uid", _ps_, _callback_);
		}
		#endregion //Delete

		#region Insert
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (All params)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int itemId, int acctId, int count, List<int> testList, Dictionary<int,bool> testDic, Action<Item, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@itemId", MySqlDbType.Int32);
			_param_.Value = itemId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@count", MySqlDbType.Int32);
			_param_.Value = count;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@testList", MySqlDbType.String);
			string _testList = NetObjectUtils.ListToString(testList);
			_param_.Value = _testList;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@testDic", MySqlDbType.String);
			string _testDic = NetObjectUtils.DictionaryToString(testDic);
			_param_.Value = _testDic;
			_ps_.Add(_param_);
			Insert("INSERT INTO `Item` (`itemId`,`acctId`,`count`,`testList`,`testDic`) VALUES (@itemId,@acctId,@count,@testList,@testDic)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					Item _item_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_item_ = new Item();
						_item_._attribute_.uid = (int)_lastInsertedId_;
						_item_._attribute_.itemId = itemId;
						_item_._attribute_.acctId = acctId;
						_item_._attribute_.count = count;
						_item_._attribute_.testList = _testList;
						_item_._attribute_.testDic = _testDic;
					}
					if (_callback_ != null)
						_callback_(_item_, _error_);
				});
		}
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (Selected param only.)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int itemId, int acctId, int count, Action<Item, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@itemId", MySqlDbType.Int32);
			_param_.Value = itemId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@acctId", MySqlDbType.Int32);
			_param_.Value = acctId;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@count", MySqlDbType.Int32);
			_param_.Value = count;
			_ps_.Add(_param_);
			Insert("INSERT INTO `Item` (`itemId`,`acctId`,`count`) VALUES (@itemId,@acctId,@count)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					Item _item_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_item_ = new Item();
						_item_._attribute_.uid = (int)_lastInsertedId_;
						_item_._attribute_.itemId = itemId;
						_item_._attribute_.acctId = acctId;
						_item_._attribute_.count = count;
					}
					if (_callback_ != null)
						_callback_(_item_, _error_);
				});
		}

		#endregion //Insert

		#region Property
		public enum UpdateMask : ulong
		{
			UseSendMask_ = 0ul,
			uidMask = 1ul,
			countMask = 2ul,
			testListMask = 4ul,
			AllMask_ = 7ul
		};

		[KissJsonDontSerialize]
		protected override ulong DefaultSendMask => 7ul;
		[KissJsonSerializeProperty]
		public int uid
		{
			get => _attribute_.uid; 
			set => _attribute_.uid = value;
		}
		[KissJsonSerializeProperty]
		public int itemId
		{
			get => _attribute_.itemId; 
			set => _attribute_.itemId = value;
		}
		[KissJsonSerializeProperty]
		public int acctId
		{
			get => _attribute_.acctId; 
			set => _attribute_.acctId = value;
		}
		[KissJsonSerializeProperty]
		public int count
		{
			get => _attribute_.count; 
			set
			{
				_attribute_.count = value;
				MarkUpdateAndModifyMask(2ul);//UpdateMask.countMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("items");
			}
		}
		#region Property_testList
		[KissJsonSerializeProperty]
		string _testList
		{
			get => _attribute_.testList; 
			set
			{
				_attribute_.testList = value;
				MarkUpdateAndModifyMask(4ul);//UpdateMask.testListMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("items");
			}
		}
		[KissJsonDontSerialize]
		List<int> __testList;
		[KissJsonDontSerialize]
		public List<int> testList
		{
			get
			{
				if (_testList == null) __testList = NetObjectUtils.StringToList<int>(_testList);
				return __testList;
			}
			set
			{
				__testList = value;
				testListMarkChanged();
			}
		}
		/// <summary>
		///Call this function after you modified testList (e.g. insert/add/remove/clear operation.). 
		/// <summary>
		public void testListMarkChanged()
		{
			_testList = NetObjectUtils.ListToString(testList);
		}
		#endregion Property_testList
		#region Property_testDic
		[KissJsonSerializeProperty]
		string _testDic
		{
			get => _attribute_.testDic; 
			set
			{
				_attribute_.testDic = value;
				MarkUpdateAndModifyMask(4ul);//UpdateMask.testListMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				_mainObject_?.SyncToClient("items");
			}
		}
		[KissJsonDontSerialize]
		Dictionary<int,bool> __testDic;
		[KissJsonDontSerialize]
		public Dictionary<int,bool> testDic
		{
			get
			{
				if (_testDic == null) __testDic = NetObjectUtils.StringToDictionary<int,bool>(_testDic);
				return __testDic;
			}
			set
			{
				__testDic = value;
				testDicMarkChanged();
			}
		}
		public bool testDicGet(int _key_, bool _default_ = true)
		{
			return testDic.TryGetValue(_key_, out bool _value_) ? _value_ : _default_;
		}
		public void testDicSet(int _key_, bool _value_, bool _default_ = true)
		{
			if (testDic.TryGetValue(_key_, out bool _value_old_) && _value_old_ == _default_)
				testDic.Remove(_key_);
			else
				testDic[_key_] = _value_;
			testDicMarkChanged();
		}
		/// <summary>
		///Call this function after you modified testDic (e.g. insert/add/remove/clear operation.). 
		/// <summary>
		public void testDicMarkChanged()
		{
			_testDic = NetObjectUtils.DictionaryToString(testDic);
		}
		#endregion Property_testDic
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
				_jsonData_["itemId"] = _attribute_.itemId;
				_jsonData_["acctId"] = _attribute_.acctId;
			}
			if ((mask & 2ul) > 0)//UpdateMask.countMask
				_jsonData_["count"] = _attribute_.count;
			if ((mask & 4ul) > 0)//UpdateMask.testListMask
			{
				_jsonData_["_testList"] = _attribute_.testList;
				_jsonData_["_testDic"] = _attribute_.testDic;
			}

			_jsonData_["_uid_"] = _uid_;
			_jsonData_["_sendMask_"] = mask;
			return _jsonData_;
		}
		/// <summary>
		/// Clone data from the Item instance with custom mask.
		/// <param name="_source_">The source Item</param>
		/// <param name="_mask_">Which part you want to clone, default is all.</param>
		/// </summary>
		public void Clone(Item _source_, ulong _mask_ = ulong.MaxValue)
		{
			if ((_mask_ & 1ul) > 0)//UpdateMask.uidMask
			{
				uid = _source_.uid;
				itemId = _source_.itemId;
				acctId = _source_.acctId;
			}
			if ((_mask_ & 2ul) > 0)//UpdateMask.countMask
				count = _source_.count;
			if ((_mask_ & 4ul) > 0)//UpdateMask.testListMask
			{
				_testList = _source_._testList;
				_testDic = _source_._testDic;
			}

		}
		#endregion //JSON

		#region PrivateFields
		[KissJsonDontSerialize]
		private struct _fields_
		{
			// uid
			public int uid;
			public int itemId;
			public int acctId;

			// count
			public int count;

			// testList
			public string testList;
			public string testDic;

		}
		[KissJsonDontSerialize]
		public bool SyncToDB = true;
		private _fields_ _attribute_ = new _fields_();
		#endregion //PrivateFields
	}
}

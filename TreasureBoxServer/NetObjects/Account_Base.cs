/*
 * It's automatic generate by 'KissNetObject', DON'T modify this file.
 * You should modify 'Account.cs' or edit by 'KissNetObject'.
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
	public abstract class Account_Base : MainNetObject<Account>
	{
		#region Update
		/// <summary>
		/// For internal call only. You can override it if you want customize it.
		/// </summary>
		public override void Update(ref string _strSQL_, ref MySqlParameter[] _mySqlParameters_)
		{
			StringBuilder _sb_ = new StringBuilder();
			_sb_.Append("UPDATE `Account` SET ");
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			if (HasUpdate(1ul))//UpdateMask.uidMask)
			{
				_sb_.Append("`name` = @name,");
				_param_ = new MySqlParameter("@name", MySqlDbType.VarChar, 64);
				_param_.Value = _attribute_.name;
				_ps_.Add(_param_);
			}
			if (HasUpdate(2ul))//UpdateMask.nicknameMask)
			{
				_sb_.Append("`nickname` = @nickname,");
				_param_ = new MySqlParameter("@nickname", MySqlDbType.VarChar, 64);
				_param_.Value = _attribute_.nickname;
				_ps_.Add(_param_);
			}
			if (HasUpdate(8ul))//UpdateMask.moneyMask)
			{
				_sb_.Append("`money` = @money,");
				_param_ = new MySqlParameter("@money", MySqlDbType.Int32);
				_param_.Value = _attribute_.money;
				_ps_.Add(_param_);
			}
			if (HasUpdate(16ul))//UpdateMask.diamondMask)
			{
				_sb_.Append("`diamond` = @diamond,");
				_param_ = new MySqlParameter("@diamond", MySqlDbType.Int32);
				_param_.Value = _attribute_.diamond;
				_ps_.Add(_param_);
			}
			if (HasUpdate(32ul))//UpdateMask.lastLoginTimeMask)
			{
				_sb_.Append("`lastLoginTime` = @lastLoginTime,");
				_param_ = new MySqlParameter("@lastLoginTime", MySqlDbType.DateTime);
				_param_.Value = _attribute_.lastLoginTime;
				_ps_.Add(_param_);
			}
			if (HasUpdate(64ul))//UpdateMask.lvMask)
			{
				_sb_.Append("`lv` = @lv,");
				_param_ = new MySqlParameter("@lv", MySqlDbType.Int32);
				_param_.Value = _attribute_.lv;
				_ps_.Add(_param_);
			}
			if (HasUpdate(128ul))//UpdateMask.expMask)
			{
				_sb_.Append("`exp` = @exp,");
				_param_ = new MySqlParameter("@exp", MySqlDbType.Int32);
				_param_.Value = _attribute_.exp;
				_ps_.Add(_param_);
			}
			if (HasUpdate(256ul))//UpdateMask.vpMask)
			{
				_sb_.Append("`vp` = @vp,");
				_param_ = new MySqlParameter("@vp", MySqlDbType.Int32);
				_param_.Value = _attribute_.vp;
				_ps_.Add(_param_);
			}
			if (HasUpdate(512ul))//UpdateMask.vpTimeMask)
			{
				_sb_.Append("`vpTime` = @vpTime,");
				_param_ = new MySqlParameter("@vpTime", MySqlDbType.DateTime);
				_param_.Value = _attribute_.vpTime;
				_ps_.Add(_param_);
			}
			if (HasUpdate(1024ul))//UpdateMask.vipExpMask)
			{
				_sb_.Append("`vipExp` = @vipExp,");
				_param_ = new MySqlParameter("@vipExp", MySqlDbType.Int32);
				_param_.Value = _attribute_.vipExp;
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
				Logger.LogWarning("No need update 'Account', you should call 'Account.Update()' or 'Account.UpdateImmediately()' after change something need to update to database.");
			}
		}
		#endregion //Update

		#region Select
		protected override void SetData(DataRow data)
		{
			_attribute_.uid = Convert.ToInt32(data["uid"]);
			_attribute_.acctType = Convert.ToInt32(data["acctType"]);
			_attribute_.name = Convert.ToString(data["name"]);
			_attribute_.createTime = Convert2DateTime(data["createTime"]);
			_attribute_.nickname = Convert.ToString(data["nickname"]);
			_attribute_.icon = Convert.ToInt32(data["icon"]);
			_attribute_.money = Convert.ToInt32(data["money"]);
			_attribute_.diamond = Convert.ToInt32(data["diamond"]);
			_attribute_.lastLoginTime = Convert2DateTime(data["lastLoginTime"]);
			_attribute_.lv = Convert.ToInt32(data["lv"]);
			_attribute_.exp = Convert.ToInt32(data["exp"]);
			_attribute_.vp = Convert.ToInt32(data["vp"]);
			_attribute_.vpTime = Convert2DateTime(data["vpTime"]);
			_attribute_.vipExp = Convert.ToInt32(data["vipExp"]);
			RegisterSync();
		}
		/// <summary>
		/// Select all data from database. The select operation run in background thread. The callback action occur after database operation done.
		/// </summary>
		/// <param name="callback">This callback occur after database operation done.</param>
		public static void SelectAll(Action<List<Account>, string> callback)
		{
			Select("SELECT * FROM `Account`", new List<MySqlParameter>(), callback);
		}
		/// <summary>
		/// Select data from database by uid. The select operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void SelectByUid(int uid, Action<List<Account>, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
			_param_.Value = uid;
			_ps_.Add(_param_);
			Select("SELECT * FROM `Account` WHERE `uid` = @uid", _ps_, _callback_);
		}		/// <summary>
		/// Select data from database by acctType and name. The select operation run in background thread.The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void SelectByAcctTypeAndName(int acctType, string name, Action<List<Account>, string> _callback_)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctType", MySqlDbType.Int32);
			_param_.Value = acctType;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@name", MySqlDbType.VarChar, 64);
			_param_.Value = name;
			_ps_.Add(_param_);
			Select("SELECT * FROM `Account` WHERE `acctType` = @acctType AND `name` = @name", _ps_, _callback_);
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
			_sb_.Append("DELETE FROM `Account`");
			_sb_.Append(" WHERE `uid` = @uid");
			_param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
			_param_.Value = uid;
			_ps_.Add(_param_);
			Delete(_sb_.ToString(), _ps_, _callback_);
		}
		/// <summary>
		/// Delete all data from database. The delete operation run in background thread. The callback action occur after database operation done.
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done.</param>
		public static void DeleteAll(Action<int, string> _callback_)
		{
			Delete("DELETE FROM `Account`", new List<MySqlParameter>(), _callback_);
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
			Delete("DELETE FROM `Account` WHERE `uid` = @uid", _ps_, _callback_);
		}
		#endregion //Delete

		#region Insert
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (All params)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int acctType, string name, DateTime createTime, string nickname, int icon, int money, int diamond, DateTime lastLoginTime, int lv, int exp, int vp, DateTime vpTime, int vipExp, Action<Account, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctType", MySqlDbType.Int32);
			_param_.Value = acctType;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@name", MySqlDbType.VarChar, 64);
			_param_.Value = name;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@createTime", MySqlDbType.DateTime);
			_param_.Value = createTime;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@nickname", MySqlDbType.VarChar, 64);
			_param_.Value = nickname;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@icon", MySqlDbType.Int32);
			_param_.Value = icon;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@money", MySqlDbType.Int32);
			_param_.Value = money;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@diamond", MySqlDbType.Int32);
			_param_.Value = diamond;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@lastLoginTime", MySqlDbType.DateTime);
			_param_.Value = lastLoginTime;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@lv", MySqlDbType.Int32);
			_param_.Value = lv;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@exp", MySqlDbType.Int32);
			_param_.Value = exp;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@vp", MySqlDbType.Int32);
			_param_.Value = vp;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@vpTime", MySqlDbType.DateTime);
			_param_.Value = vpTime;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@vipExp", MySqlDbType.Int32);
			_param_.Value = vipExp;
			_ps_.Add(_param_);
			Insert("INSERT INTO `Account` (`acctType`,`name`,`createTime`,`nickname`,`icon`,`money`,`diamond`,`lastLoginTime`,`lv`,`exp`,`vp`,`vpTime`,`vipExp`) VALUES (@acctType,@name,@createTime,@nickname,@icon,@money,@diamond,@lastLoginTime,@lv,@exp,@vp,@vpTime,@vipExp)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					Account _account_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_account_ = new Account();
						_account_._attribute_.uid = (int)_lastInsertedId_;
						_account_._attribute_.acctType = acctType;
						_account_._attribute_.name = name;
						_account_._attribute_.createTime = createTime;
						_account_._attribute_.nickname = nickname;
						_account_._attribute_.icon = icon;
						_account_._attribute_.money = money;
						_account_._attribute_.diamond = diamond;
						_account_._attribute_.lastLoginTime = lastLoginTime;
						_account_._attribute_.lv = lv;
						_account_._attribute_.exp = exp;
						_account_._attribute_.vp = vp;
						_account_._attribute_.vpTime = vpTime;
						_account_._attribute_.vipExp = vipExp;
						_account_.RegisterSync();
					}
					if (_callback_ != null)
						_callback_(_account_, _error_);
				});
		}
		/// <summary>
		/// Insert into database. The insert operation run in background thread. The callback occur after insert into database. (Selected param only.)
		/// </summary>
		/// <param name="_callback_">This callback occur after database operation done. You can ignore it if you don't care about the callback.</param>
		public static void Insert(int acctType, string name, string nickname, Action<Account, string> _callback_ = null)
		{
			List<MySqlParameter> _ps_ = new List<MySqlParameter>();
			MySqlParameter _param_;
			_param_ = new MySqlParameter("@acctType", MySqlDbType.Int32);
			_param_.Value = acctType;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@name", MySqlDbType.VarChar, 64);
			_param_.Value = name;
			_ps_.Add(_param_);
			_param_ = new MySqlParameter("@nickname", MySqlDbType.VarChar, 64);
			_param_.Value = nickname;
			_ps_.Add(_param_);
			Insert("INSERT INTO `Account` (`acctType`,`name`,`nickname`) VALUES (@acctType,@name,@nickname)",
				_ps_,
				(_lastInsertedId_, _error_) =>
				{
					Account _account_ = null;
					if (string.IsNullOrEmpty(_error_))
					{
						_account_ = new Account();
						_account_._attribute_.uid = (int)_lastInsertedId_;
						_account_._attribute_.acctType = acctType;
						_account_._attribute_.name = name;
						_account_._attribute_.nickname = nickname;
						_account_.RegisterSync();
					}
					if (_callback_ != null)
						_callback_(_account_, _error_);
				});
		}

		#endregion //Insert
		#region Sync
		public void RegisterSync()
		{
			int _uid_ = _attribute_.uid;
			Func<int, PlayerBase> _getPlayer_ = GetPlayer();
			RegisterSync("mails", (player) => { Mail.Sync(mails.Values, player, "mails", 50); }, _getPlayer_, _uid_, 0.1f);
			RegisterSync("items", (player) => { Item.Sync(items.Values, player, "items", 100); }, _getPlayer_, _uid_, 0.1f);
			RegisterSync("signIn", (player) => { signIn.Sync(player, "signIn"); }, _getPlayer_, _uid_, 0.1f);
			RegisterSync("account", (player) => { Sync(player, "account"); }, _getPlayer_, _uid_, 0.1f);
		}
		[KissJsonDontSerialize]
		public Dictionary<int, Mail> mails = new Dictionary<int, Mail>();
		public void SetMails(List<Mail> mails)
		{
			if (mails.Count == 0)
				return;
			if (this.mails.Count == 0)
			{
				foreach (Mail one in mails)
				{
					one._mainObject_ = this;
					one.MarkModifyMaskAll();
					this.mails[one.uid] = one;
				}
			}
			else
			{
				foreach (Mail one in mails)
				{
					if (!this.mails.ContainsKey(one.uid))
					{
						one._mainObject_ = this;
						one.MarkModifyMaskAll();
						this.mails[one.uid] = one;
					}
				}
			}
			SyncToClient("mails");
		}
		public Mail GetMail(int uid)
		{
			if (mails.TryGetValue(uid, out Mail _mail_))
				return _mail_;
			return null;
		}
		public bool RemoveMail(int uid)
		{
			Mail _mail_ = GetMail(uid);
			if (_mail_ != null)
			{
				mails.Remove(uid);
				_mail_.Delete();
				AddToDelete("mails", uid);
				return true;
			}
			return false;
		}
		[KissJsonDontSerialize]
		public Dictionary<int, Item> items = new Dictionary<int, Item>();
		public void SetItems(List<Item> items)
		{
			if (items.Count == 0)
				return;
			if (this.items.Count == 0)
			{
				foreach (Item one in items)
				{
					one._mainObject_ = this;
					one.MarkModifyMaskAll();
					this.items[one.itemId] = one;
				}
			}
			else
			{
				foreach (Item one in items)
				{
					if (!this.items.ContainsKey(one.itemId))
					{
						one._mainObject_ = this;
						one.MarkModifyMaskAll();
						this.items[one.itemId] = one;
					}
					else
					{
						Item old = this.items[one.itemId];
						if (old.itemId != one.itemId)
						{
							old.count += one.count;
							one.Delete();
						}
					}
				}
			}
			SyncToClient("items");
		}
		public Item GetItem(int itemId)
		{
			if (items.TryGetValue(itemId, out Item _item_))
				return _item_;
			return null;
		}
		public bool RemoveItem(int itemId)
		{
			Item _item_ = GetItem(itemId);
			if (_item_ != null)
			{
				items.Remove(itemId);
				_item_.Delete();
				AddToDelete("items", itemId);
				return true;
			}
			return false;
		}
		[KissJsonDontSerialize]
		public SignIn signIn = null;
		public SignIn GetSignIn()
		{
			return signIn;
		}
		public void SetSignIn(SignIn signIn)
		{
			if (signIn == null) return;
			this.signIn = signIn;
			signIn.MarkModifyMaskAll();
			signIn._mainObject_ = this;
			SyncToClient("signIn");
		}
		public bool RemoveSignIn()
		{
			if (signIn != null)
			{
				signIn.Delete();
				AddToDelete("signIn", 0);
				return true;
			}
			return false;
		}
		public void MarkModifyMaskAllSubSystem()
		{
			foreach(Mail one in mails.Values)
				one?.MarkModifyMaskAll();
			foreach(Item one in items.Values)
				one?.MarkModifyMaskAll();
			signIn?.MarkModifyMaskAll();
		}
		public virtual void OnMailLoaded() { }
		public virtual void OnItemLoaded() { }
		public virtual void OnSignInLoaded() { }
		public virtual void OnAllSubSystemLoaded() { }
		public void LoadAllSubSystem(PlayerBase player)
		{
			int _uid_ = _attribute_.uid;
			Func<int, PlayerBase> _getPlayer_ = GetPlayer();
			int _count_ = 3;
			Mail.SelectByAcctId(_uid_, (mails, error) =>
			{
				if (string.IsNullOrEmpty(error))
				{
					PlayerBase _playerNow_ = _getPlayer_(_uid_);
					if (_playerNow_ != null && player == _playerNow_)
					{
						SetMails(mails);
						OnMailLoaded();
					}
				}
				else
					Logger.LogError(error);
				_count_--;
				if (_count_ <= 0)
					OnAllSubSystemLoaded();
			});
			Item.SelectByAcctId(_uid_, (items, error) =>
			{
				if (string.IsNullOrEmpty(error))
				{
					PlayerBase _playerNow_ = _getPlayer_(_uid_);
					if (_playerNow_ != null && player == _playerNow_)
					{
						SetItems(items);
						OnItemLoaded();
					}
				}
				else
					Logger.LogError(error);
				_count_--;
				if (_count_ <= 0)
					OnAllSubSystemLoaded();
			});
			SignIn.SelectByAcctId(_uid_, (signIns, error) =>
			{
				if (string.IsNullOrEmpty(error))
				{
					PlayerBase _playerNow_ = _getPlayer_(_uid_);
					if (signIns.Count != 0 && _playerNow_ != null && player == _playerNow_)
					{
						SetSignIn(signIns[0]);
						OnSignInLoaded();
					}
				}
				else
					Logger.LogError(error);
				_count_--;
				if (_count_ <= 0)
					OnAllSubSystemLoaded();
			});
		}
		#endregion //Sync
		#region Property
		public enum UpdateMask : ulong
		{
			UseSendMask_ = 0ul,
			uidMask = 1ul,
			nicknameMask = 2ul,
			iconMask = 4ul,
			moneyMask = 8ul,
			diamondMask = 16ul,
			lastLoginTimeMask = 32ul,
			lvMask = 64ul,
			expMask = 128ul,
			vpMask = 256ul,
			vpTimeMask = 512ul,
			vipExpMask = 1024ul,
			AllMask_ = 2047ul
		};

		[KissJsonDontSerialize]
		protected override ulong DefaultSendMask => 2047ul;
		[KissJsonSerializeProperty]
		public int uid
		{
			get => _attribute_.uid; 
			set => _attribute_.uid = value;
		}
		[KissJsonSerializeProperty]
		public int acctType
		{
			get => _attribute_.acctType; 
			set => _attribute_.acctType = value;
		}
		[KissJsonSerializeProperty]
		public string name
		{
			get => _attribute_.name; 
			set
			{
				_attribute_.name = value;
				MarkUpdateAndModifyMask(1ul);//UpdateMask.uidMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public DateTime createTime
		{
			get => _attribute_.createTime; 
			set => _attribute_.createTime = value;
		}
		[KissJsonSerializeProperty]
		public string nickname
		{
			get => _attribute_.nickname; 
			set
			{
				_attribute_.nickname = value;
				MarkUpdateAndModifyMask(2ul);//UpdateMask.nicknameMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public int icon
		{
			get => _attribute_.icon; 
			set => _attribute_.icon = value;
		}
		[KissJsonSerializeProperty]
		public int money
		{
			get => _attribute_.money; 
			set
			{
				_attribute_.money = value;
				MarkUpdateAndModifyMask(8ul);//UpdateMask.moneyMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public int diamond
		{
			get => _attribute_.diamond; 
			set
			{
				_attribute_.diamond = value;
				MarkUpdateAndModifyMask(16ul);//UpdateMask.diamondMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public DateTime lastLoginTime
		{
			get => _attribute_.lastLoginTime; 
			set
			{
				_attribute_.lastLoginTime = value;
				MarkUpdateAndModifyMask(32ul);//UpdateMask.lastLoginTimeMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public int lv
		{
			get => _attribute_.lv; 
			set
			{
				_attribute_.lv = value;
				MarkUpdateAndModifyMask(64ul);//UpdateMask.lvMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public int exp
		{
			get => _attribute_.exp; 
			set
			{
				_attribute_.exp = value;
				MarkUpdateAndModifyMask(128ul);//UpdateMask.expMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public int vp
		{
			get => _attribute_.vp; 
			set
			{
				_attribute_.vp = value;
				MarkUpdateAndModifyMask(256ul);//UpdateMask.vpMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public DateTime vpTime
		{
			get => _attribute_.vpTime; 
			set
			{
				_attribute_.vpTime = value;
				MarkUpdateAndModifyMask(512ul);//UpdateMask.vpTimeMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
			}
		}
		[KissJsonSerializeProperty]
		public int vipExp
		{
			get => _attribute_.vipExp; 
			set
			{
				_attribute_.vipExp = value;
				MarkUpdateAndModifyMask(1024ul);//UpdateMask.vipExpMask
				if (SyncToDB) AsyncDatabaseManager.UpdateDelayInBackgroundThread(this);
				SyncToClient("account");
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
				_jsonData_["acctType"] = _attribute_.acctType;
				_jsonData_["name"] = _attribute_.name;
				_jsonData_["createTime"] = _attribute_.createTime;
			}
			if ((mask & 2ul) > 0)//UpdateMask.nicknameMask
				_jsonData_["nickname"] = _attribute_.nickname;
			if ((mask & 4ul) > 0)//UpdateMask.iconMask
				_jsonData_["icon"] = _attribute_.icon;
			if ((mask & 8ul) > 0)//UpdateMask.moneyMask
				_jsonData_["money"] = _attribute_.money;
			if ((mask & 16ul) > 0)//UpdateMask.diamondMask
				_jsonData_["diamond"] = _attribute_.diamond;
			if ((mask & 32ul) > 0)//UpdateMask.lastLoginTimeMask
				_jsonData_["lastLoginTime"] = _attribute_.lastLoginTime;
			if ((mask & 64ul) > 0)//UpdateMask.lvMask
				_jsonData_["lv"] = _attribute_.lv;
			if ((mask & 128ul) > 0)//UpdateMask.expMask
				_jsonData_["exp"] = _attribute_.exp;
			if ((mask & 256ul) > 0)//UpdateMask.vpMask
				_jsonData_["vp"] = _attribute_.vp;
			if ((mask & 512ul) > 0)//UpdateMask.vpTimeMask
				_jsonData_["vpTime"] = _attribute_.vpTime;
			if ((mask & 1024ul) > 0)//UpdateMask.vipExpMask
				_jsonData_["vipExp"] = _attribute_.vipExp;

			_jsonData_["_uid_"] = _uid_;
			_jsonData_["_sendMask_"] = mask;
			return _jsonData_;
		}
		/// <summary>
		/// Clone data from the Account instance with custom mask.
		/// <param name="_source_">The source Account</param>
		/// <param name="_mask_">Which part you want to clone, default is all.</param>
		/// </summary>
		public void Clone(Account _source_, ulong _mask_ = ulong.MaxValue)
		{
			if ((_mask_ & 1ul) > 0)//UpdateMask.uidMask
			{
				uid = _source_.uid;
				acctType = _source_.acctType;
				name = _source_.name;
				createTime = _source_.createTime;
			}
			if ((_mask_ & 2ul) > 0)//UpdateMask.nicknameMask
				nickname = _source_.nickname;
			if ((_mask_ & 4ul) > 0)//UpdateMask.iconMask
				icon = _source_.icon;
			if ((_mask_ & 8ul) > 0)//UpdateMask.moneyMask
				money = _source_.money;
			if ((_mask_ & 16ul) > 0)//UpdateMask.diamondMask
				diamond = _source_.diamond;
			if ((_mask_ & 32ul) > 0)//UpdateMask.lastLoginTimeMask
				lastLoginTime = _source_.lastLoginTime;
			if ((_mask_ & 64ul) > 0)//UpdateMask.lvMask
				lv = _source_.lv;
			if ((_mask_ & 128ul) > 0)//UpdateMask.expMask
				exp = _source_.exp;
			if ((_mask_ & 256ul) > 0)//UpdateMask.vpMask
				vp = _source_.vp;
			if ((_mask_ & 512ul) > 0)//UpdateMask.vpTimeMask
				vpTime = _source_.vpTime;
			if ((_mask_ & 1024ul) > 0)//UpdateMask.vipExpMask
				vipExp = _source_.vipExp;

		}
		#endregion //JSON

		#region PrivateFields
		[KissJsonDontSerialize]
		private struct _fields_
		{
			// uid
			public int uid;
			public int acctType;
			public string name;
			public DateTime createTime;

			// nickname
			public string nickname;

			// icon
			public int icon;

			// money
			public int money;

			// diamond
			public int diamond;

			// lastLoginTime
			public DateTime lastLoginTime;

			// lv
			public int lv;

			// exp
			public int exp;

			// vp
			public int vp;

			// vpTime
			public DateTime vpTime;

			// vipExp
			public int vipExp;

		}
		[KissJsonDontSerialize]
		public bool SyncToDB = true;
		private _fields_ _attribute_ = new _fields_();
		#endregion //PrivateFields
	}
}

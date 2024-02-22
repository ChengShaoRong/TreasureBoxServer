using CSharpLike;
using KissFramework;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace TreasureBox
{
    public class AuthenticationManager : Singleton<AuthenticationManager>
    {
        enum ErrorCode
        {
            Success = 0,
            FailDB = 1000,
            FailNotExist = 1001,
            FailTokenExpire = 1002,
            FailTokenNotMatch = 1003,
            FailGuestNameInvalid = 1004,
            FailLoginNameInvalid = 1005,
            FailLoginPasswordInvalid = 1006,
            FailLoginNameExist = 1007,
            FailLoginPasswordNotMatch = 1008,
            FailInvalidIP = 1009,
        }
        class Authentication
        {
            public int uid;
            public string loginName;
            [KissJsonDontSerialize]
            public string password;
            public string token;
            public DateTime tokenExpireTime;
            [KissJsonDontSerialize]
            public DateTime createTime;
            [KissJsonDontSerialize]
            public DateTime lastLoginTime;
            [KissJsonDontSerialize]
            public DateTime cacheTime;
            [KissJsonDontSerialize]
            public string Password
            {
                set
                {
                    if (password != value)
                    {
                        password = value;
                        mask |= 1ul;
                        RaiseSync();
                    }
                }
            }
            [KissJsonDontSerialize]
            public DateTime TokenExpireTime
            {
                set
                {
                    if (tokenExpireTime != value)
                    {
                        tokenExpireTime = value;
                        mask |= 2ul;
                        RaiseSync();
                    }
                }
            }
            [KissJsonDontSerialize]
            public string Token
            {
                set
                {
                    if (token != value)
                    {
                        token = value;
                        mask |= 4ul;
                        RaiseSync();
                    }
                }
            }
            [KissJsonDontSerialize]
            public DateTime LastLoginTime
            {
                set
                {
                    if (lastLoginTime != value)
                    {
                        lastLoginTime = value;
                        mask |= 8ul;
                        RaiseSync();
                    }
                }
            }
            [KissJsonDontSerialize]
            public string LoginName
            {
                set
                {
                    if (loginName != value)
                    {
                        loginName = value;
                        mask |= 16ul;
                        RaiseSync();
                    }
                }
            }
            [KissJsonDontSerialize]
            ulong mask = 0;

            public override string ToString()
            {
                return KissJson.ToJSONData(this).ToJson();
            }
            void RaiseSync()
            {
                FrameworkBase.RaiseUniqueEvent(Sync, "AuthenticationSync" + uid, 100f, 1);
            }
            void Sync()
            {
                StringBuilder _sb_ = new StringBuilder();
                _sb_.Append("UPDATE `Authentication` SET ");
                List<MySqlParameter> _ps_ = new List<MySqlParameter>();
                MySqlParameter _param_;
                if ((mask & 1ul) > 0)
                {
                    _sb_.Append("`password` = @password,");
                    _param_ = new MySqlParameter("@password", MySqlDbType.VarChar, 32);
                    _param_.Value = password;
                    _ps_.Add(_param_);
                }
                if ((mask & 2ul) > 0)
                {
                    _sb_.Append("`tokenExpireTime` = @tokenExpireTime,");
                    _param_ = new MySqlParameter("@tokenExpireTime", MySqlDbType.DateTime);
                    _param_.Value = tokenExpireTime;
                    _ps_.Add(_param_);
                }
                if ((mask & 4ul) > 0)
                {
                    _sb_.Append("`token` = @token,");
                    _param_ = new MySqlParameter("@token", MySqlDbType.VarChar, 32);
                    _param_.Value = token;
                    _ps_.Add(_param_);
                }
                if ((mask & 8ul) > 0)
                {
                    _sb_.Append("`tokenExpireTime` = @lastLoginTime,");
                    _param_ = new MySqlParameter("@lastLoginTime", MySqlDbType.DateTime);
                    _param_.Value = lastLoginTime;
                    _ps_.Add(_param_);
                }
                if ((mask & 16ul) > 0)
                {
                    _sb_.Append("`loginName` = @loginName,");
                    _param_ = new MySqlParameter("@loginName", MySqlDbType.VarChar, 50);
                    _param_.Value = loginName;
                    _ps_.Add(_param_);
                }
                mask = 0ul;
                if (_ps_.Count > 0)
                {
                    _sb_.Remove(_sb_.Length - 1, 1);
                    _sb_.Append(" WHERE `uid` = @uid");
                    _param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
                    _param_.Value = uid;
                    _ps_.Add(_param_);
                    AsyncDatabaseManager.ExecuteSQLInBackgroundThread(_sb_.ToString(), _ps_.ToArray());
                }
            }

        }
        static Dictionary<int, Authentication> mAuthentications = new Dictionary<int, Authentication>();
        static Dictionary<string, Authentication> mAuthenticationsByName = new Dictionary<string, Authentication>();
        [EventMethod(IntervalTime = 3600f)]
        static void OnUpdate()
        {
            List<Authentication> removes = new List<Authentication>();
            DateTime dtNow = DateTime.Now;
            foreach(Authentication one in mAuthentications.Values)
            {
                if (one.cacheTime < dtNow)
                    removes.Add(one);
            }
            foreach (Authentication one in removes)
            {
                mAuthentications.Remove(one.uid);
                mAuthenticationsByName.Remove(one.loginName);
            }
        }
        static Authentication Add(Authentication authentication)
        {
            if (authentication == null)
                return null;
            if (!mAuthentications.ContainsKey(authentication.uid))
            {
                mAuthentications[authentication.uid] = authentication;
                mAuthenticationsByName[authentication.loginName] = authentication;
            }
            return mAuthentications[authentication.uid];
        }
        static DateTime Convert2DateTime(object obj)
        {
            if (obj.GetType() == typeof(DBNull))
                return new DateTime();
            return ((MySql.Data.Types.MySqlDateTime)obj).GetDateTime();
        }
        static void GetAuthentication(string loginName, Action<Authentication, ErrorCode> callback)
        {
            if (mAuthenticationsByName.TryGetValue(loginName, out Authentication authentication))
                callback(authentication, ErrorCode.Success);
            else
            {
                ErrorCode errorCode = ErrorCode.Success;
                new ThreadPoolMySql(
                    (connection) =>
                    {
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `Authentication` WHERE `loginName` = @loginName", connection);
                            cmd.CommandType = CommandType.Text;
                            MySqlParameter _param_;
                            _param_ = new MySqlParameter("@loginName", MySqlDbType.VarChar, 50);
                            _param_.Value = loginName;
                            cmd.Parameters.Add(_param_);
                            using (MySqlDataAdapter msda = new MySqlDataAdapter())
                            {
                                msda.SelectCommand = cmd;
                                DataTable dt = new DataTable();
                                msda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    DataRow data = dt.Rows[0];
                                    authentication = new Authentication()
                                    {
                                        uid = Convert.ToInt32(data["uid"]),
                                        loginName = Convert.ToString(data["loginName"]),
                                        password = Convert.ToString(data["password"]),
                                        token = Convert.ToString(data["token"]),
                                        tokenExpireTime = Convert2DateTime(data["tokenExpireTime"]),
                                        createTime = Convert2DateTime(data["createTime"]),
                                        lastLoginTime = Convert2DateTime(data["lastLoginTime"]),
                                        cacheTime = DateTime.Now.AddHours(24)
                                    };
                                }
                                else
                                    errorCode = ErrorCode.FailNotExist;
                            }
                        }
                        catch (Exception e)
                        {
                            errorCode = ErrorCode.FailDB;
                            Logger.LogError($"AuthenticationManager : GetAuthentication : error : {e.Message} {e.StackTrace}", false);
                        }
                    },
                    () =>
                    {
                        authentication = Add(authentication);
                        callback(authentication, errorCode);
                    });
            }
        }
        static void GetAuthentication(int uid, Action<Authentication, ErrorCode> callback)
        {
            if (mAuthentications.TryGetValue(uid, out Authentication authentication))
                callback(authentication, ErrorCode.Success);
            else
            {
                ErrorCode errorCode = ErrorCode.Success;
                new ThreadPoolMySql(
                    (connection) =>
                    {
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `Authentication` WHERE `uid` = @uid", connection);
                            cmd.CommandType = CommandType.Text;
                            MySqlParameter _param_;
                            _param_ = new MySqlParameter("@uid", MySqlDbType.Int32);
                            _param_.Value = uid;
                            cmd.Parameters.Add(_param_);
                            using (MySqlDataAdapter msda = new MySqlDataAdapter())
                            {
                                msda.SelectCommand = cmd;
                                DataTable dt = new DataTable();
                                msda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    DataRow data = dt.Rows[0];
                                    authentication = new Authentication()
                                    {
                                        uid = Convert.ToInt32(data["uid"]),
                                        loginName = Convert.ToString(data["loginName"]),
                                        password = Convert.ToString(data["password"]),
                                        token = Convert.ToString(data["token"]),
                                        tokenExpireTime = Convert2DateTime(data["tokenExpireTime"]),
                                        createTime = Convert2DateTime(data["createTime"]),
                                        lastLoginTime = Convert2DateTime(data["lastLoginTime"]),
                                        cacheTime = DateTime.Now.AddHours(24)
                                    };
                                }
                                else
                                    errorCode = ErrorCode.FailNotExist;
                            }
                        }
                        catch (Exception e)
                        {
                            errorCode = ErrorCode.FailDB;
                            Logger.LogError($"AuthenticationManager : GetAuthentication : error : {e.Message} {e.StackTrace}", false);
                        }
                    },
                    () =>
                    {
                        authentication = Add(authentication);
                        callback(authentication, errorCode);
                    });
            }
        }
        static void CreateAuthentication(string name, string password, Action<string> delayCallback)
        {
            CreateAuthentication(name, password, (authentication, errorCode) =>
            {
                JSONData ret = Framework.CreateReturnJSON();
                if (errorCode == ErrorCode.Success)
                {
                    ret["uid"] = authentication.uid;
                    ret["token"] = authentication.token;
                }
                else
                {
                    ret["code"] = (int)errorCode;
                    ret["error"] = "LT_Auth_" + errorCode;
                }
                delayCallback(ret.ToJson());
            });
        }
        static void CreateAuthentication(string loginName, string password, Action<Authentication, ErrorCode> callback)
        {
            ErrorCode errorCode = ErrorCode.Success;
            Authentication authentication = null;
            new ThreadPoolMySql(
                (connection) =>
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `Authentication` (`loginName`,`password`,`token`,`tokenExpireTime`,`createTime`,`lastLoginTime`) VALUES (@loginName,@password,@token,@tokenExpireTime,@createTime,@lastLoginTime)", connection);
                        cmd.CommandType = CommandType.Text;
                        MySqlParameter _param_;
                        _param_ = new MySqlParameter("@loginName", MySqlDbType.VarChar, 50);
                        _param_.Value = loginName;
                        cmd.Parameters.Add(_param_);
                        _param_ = new MySqlParameter("@password", MySqlDbType.String);
                        _param_.Value = password;
                        cmd.Parameters.Add(_param_);
                        _param_ = new MySqlParameter("@token", MySqlDbType.String);
                        string token = Framework.GetRandomString();
                        _param_.Value = token;
                        cmd.Parameters.Add(_param_);
                        _param_ = new MySqlParameter("@tokenExpireTime", MySqlDbType.DateTime);
                        DateTime dtNow = DateTime.Now;
                        DateTime dtNext = dtNow.AddDays(7);
                        _param_.Value = dtNext;
                        cmd.Parameters.Add(_param_);
                        _param_ = new MySqlParameter("@createTime", MySqlDbType.DateTime);
                        _param_.Value = dtNow;
                        cmd.Parameters.Add(_param_);
                        _param_ = new MySqlParameter("@lastLoginTime", MySqlDbType.DateTime);
                        _param_.Value = dtNow;
                        cmd.Parameters.Add(_param_);
                        if (cmd.ExecuteNonQuery() == 0)
                            errorCode = ErrorCode.FailDB;
                        else
                        {
                            authentication = new Authentication()
                            {
                                uid = (int)cmd.LastInsertedId,
                                loginName = loginName,
                                password = password,
                                token = token,
                                tokenExpireTime = dtNext,
                                createTime = dtNow,
                                lastLoginTime = dtNow,
                                cacheTime = dtNow.AddHours(24)
                            };

                        }
                    }
                    catch (Exception e)
                    {
                        errorCode = ErrorCode.FailDB;
                        Logger.LogError($"AuthenticationManager : CreateAuthenticationInfo : error : {e.Message} {e.StackTrace}");
                    }
                },
                () =>
                {
                    authentication = Add(authentication);
                    callback(authentication, errorCode);
                });
        }

        [WebMethod]
        public static string AuthVerify(int uid, string token, string ip, Action<string> delayCallback)
        {
            Logger.LogInfo($"AuthVerify({uid},'{token}','{ip}')");
            JSONData ret = Framework.CreateReturnJSON();
            GetAuthentication(uid, (authentication, errorCode) =>
            {
                do
                {
                    if (errorCode == ErrorCode.Success)
                    {
                        Logger.LogInfo($"AuthVerify: authentication = {authentication}");
                        if (authentication.tokenExpireTime < DateTime.Now)
                        {
                            errorCode = ErrorCode.FailTokenExpire;
                            break;
                        }
                        if (token != authentication.token)
                        {
                            errorCode = ErrorCode.FailTokenNotMatch;
                            break;
                        }
                        authentication.cacheTime = DateTime.Now;
                    }
                } while (false);
                if (errorCode != ErrorCode.Success)
                {
                    ret["code"] = (int)errorCode;
                    ret["error"] = "LT_Auth_" + errorCode;
                }
                delayCallback(ret.ToJson());
            });
            return "";
        }
        [WebMethod]
        public static string AuthGuestLogin(string name, string ip, Action<string> delayCallback)
        {
            Logger.LogInfo($"AuthGuestLogin('{name}','{ip}')");
            JSONData ret = Framework.CreateReturnJSON();
            ErrorCode errorCode = ErrorCode.Success;
            do
            {
                if (!Regex.IsMatch(name, "^[a-z0-9]{32}$"))
                {
                    errorCode = ErrorCode.FailGuestNameInvalid;
                    break;
                }
                GetAuthentication(name, (authentication, errorCode) =>
                {
                    do
                    {
                        if (errorCode != ErrorCode.Success)
                        {
                            if (errorCode == ErrorCode.FailNotExist)
                            {
                                CreateAuthentication(name, "", delayCallback);
                                return;
                            }
                            break;
                        }
                        else
                        {
                            DateTime dtNow = DateTime.Now;
                            authentication.Token = FrameworkBase.GetRandomString();
                            authentication.TokenExpireTime = dtNow.AddDays(7);
                            authentication.LastLoginTime = dtNow;
                            authentication.cacheTime = dtNow.AddHours(24);
                            ret["uid"] = authentication.uid;
                            ret["token"] = authentication.token;
                        }
                    } while (false);
                    delayCallback(ret.ToJson());
                });
            }
            while (false);
            if (errorCode != ErrorCode.Success)
            {
                ret["code"] = (int)errorCode;
                ret["error"] = "LT_Auth_" + errorCode;
                return ret.ToJson();
            }
            return "";
        }
        [WebMethod]
        public static string AuthLogin(string name, string password, string ip, Action<string> delayCallback)
        {
            Logger.LogInfo($"AuthLogin('{name}','{password}','{ip}')");
            JSONData ret = Framework.CreateReturnJSON();
            ErrorCode errorCode = ErrorCode.Success;
            do
            {
                if (name.Length < 6 || name.Length > 30)
                {
                    errorCode = ErrorCode.FailLoginNameInvalid;
                    break;
                }
                if (!Regex.IsMatch(password, "^[a-z0-9]{32}$"))
                {
                    errorCode = ErrorCode.FailLoginPasswordInvalid;
                    break;
                }
                GetAuthentication(name, (authentication, errorCode) =>
                {
                    Logger.LogInfo($"AuthLogin: authentication = {authentication}");
                    if (errorCode == ErrorCode.Success)
                    {
                        DateTime dtNow = DateTime.Now;
                        DateTime dtNext = dtNow.AddDays(7);

                        authentication.Token = FrameworkBase.GetRandomString();
                        authentication.TokenExpireTime = dtNow.AddDays(7);
                        authentication.LastLoginTime = dtNow;
                        authentication.cacheTime = dtNow.AddHours(24);
                        ret["uid"] = authentication.uid;
                        ret["token"] = authentication.token;
                        Logger.LogInfo($"AuthLogin: after: authentication = {authentication}");
                    }
                    else
                    {
                        ret["code"] = (int)errorCode;
                        ret["error"] = "LT_Auth_" + errorCode;
                    }
                    delayCallback(ret.ToJson());
                });
            }
            while (false);
            if (errorCode != ErrorCode.Success)
            {
                ret["code"] = (int)errorCode;
                ret["error"] = "LT_Auth_" + errorCode;
                return ret.ToJson();
            }
            return "";
        }
        [WebMethod]
        public static string AuthRegister(string name, string password, string guest, string ip, Action<string> delayCallback)
        {
            Logger.LogInfo($"AuthRegister('{name}','{password}','{guest}','{ip}')");
            JSONData ret = Framework.CreateReturnJSON();
            ErrorCode errorCode = ErrorCode.Success;
            do
            {
                if (name.Length < 6 || name.Length > 30)
                {
                    errorCode = ErrorCode.FailLoginNameInvalid;
                    break;
                }
                if (!Regex.IsMatch(password, "^[a-z0-9]{32}$"))
                {
                    errorCode = ErrorCode.FailLoginPasswordInvalid;
                    break;
                }
                if (guest.Length > 0 && (!Regex.IsMatch(guest, "^[a-z0-9]{32}$")))
                {
                    errorCode = ErrorCode.FailGuestNameInvalid;
                    break;
                }
                if (guest.Length > 0)
                {
                    GetAuthentication(guest, (authenticationGuest, errorCode) =>
                    {
                        if (errorCode == ErrorCode.Success)
                        {
                            GetAuthentication(name, (authentication, errorCode) =>
                            {
                                if (errorCode == ErrorCode.Success)
                                {
                                    DateTime dtNow = DateTime.Now;
                                    authenticationGuest.LoginName = name;
                                    mAuthenticationsByName.Remove(guest);
                                    mAuthenticationsByName[name] = authenticationGuest;
                                    authenticationGuest.Token = FrameworkBase.GetRandomString();
                                    authenticationGuest.TokenExpireTime = dtNow.AddDays(7);
                                    authenticationGuest.LastLoginTime = dtNow;
                                    authenticationGuest.cacheTime = dtNow.AddHours(24);
                                    ret["uid"] = authenticationGuest.uid;
                                    ret["token"] = authenticationGuest.token;
                                }
                                else
                                {
                                    ret["code"] = (int)errorCode;
                                    ret["error"] = "LT_Auth_" + errorCode;
                                }
                                delayCallback(ret.ToJson());
                            });
                        }
                        else if (errorCode == ErrorCode.FailNotExist)
                        {
                            CreateAuthentication(name, password, delayCallback);
                        }
                        else
                        {
                            ret["code"] = (int)errorCode;
                            ret["error"] = "LT_Auth_" + errorCode;
                            delayCallback(ret.ToJson());
                        }
                    });
                }
                else
                {
                    GetAuthentication(name, (authentication, errorCode) =>
                    {
                        if (errorCode == ErrorCode.Success)
                        {
                            errorCode = ErrorCode.FailLoginNameExist;
                            ret["code"] = (int)errorCode;
                            ret["error"] = "LT_Auth_" + errorCode;
                            delayCallback(ret.ToJson());
                        }
                        else if (errorCode == ErrorCode.FailNotExist)
                        {
                            CreateAuthentication(name, password, delayCallback);
                        }
                        else
                        {
                            ret["code"] = (int)errorCode;
                            ret["error"] = "LT_Auth_" + errorCode;
                            delayCallback(ret.ToJson());
                        }
                    });
                    return "";
                }
            }
            while (false);
            if (errorCode != ErrorCode.Success)
            {
                ret["code"] = (int)errorCode;
                ret["error"] = "LT_Auth_" + errorCode;
                return ret.ToJson();
            }
            return "";
        }
        [WebMethod]
        public static string AuthChangePassword(string name, string passwordOld, string passwordNew, string ip, Action<string> delayCallback)
        {
            Logger.LogInfo($"AuthChangePassword('{name}','{passwordOld}','{passwordNew}','{ip}')");
            JSONData ret = Framework.CreateReturnJSON();
            ErrorCode errorCode = ErrorCode.Success;
            do
            {
                if (name.Length < 6 || name.Length > 30)
                {
                    errorCode = ErrorCode.FailLoginNameInvalid;
                    break;
                }
                if (!Regex.IsMatch(passwordOld, "^[a-z0-9]{32}$")
                    || !Regex.IsMatch(passwordNew, "^[a-z0-9]{32}$")
                    || passwordOld == passwordNew)
                {
                    errorCode = ErrorCode.FailLoginPasswordInvalid;
                    break;
                }
                GetAuthentication(name, (authentication, errorCode) =>
                {
                    if (errorCode == ErrorCode.Success)
                    {
                        Logger.LogInfo($"authentication.password='{authentication.password}','{passwordOld}','{passwordNew}'");
                        if (authentication.password != passwordOld)
                        {
                            errorCode = ErrorCode.FailLoginPasswordNotMatch;
                        }
                        else
                        {
                            authentication.Password = passwordNew;
                            authentication.cacheTime = DateTime.Now.AddHours(24);
                        }
                    }
                    if (errorCode != ErrorCode.Success)
                    {
                        ret["code"] = (int)errorCode;
                        ret["error"] = "LT_Auth_" + errorCode;
                    }
                    delayCallback(ret.ToJson());
                });
            }
            while (false);
            if (errorCode != ErrorCode.Success)
            {
                ret["code"] = (int)errorCode;
                ret["error"] = "LT_Auth_" + errorCode;
                return ret.ToJson();
            }
            return "";
        }
    }
}

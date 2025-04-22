/*
 * It's automatic generate by 'KissJSON', DON'T modify this file.
 * You should edit it by 'KissJSON'.
 */
using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{
    public class SignInJSON
    {
        public int day; //Primary key
        public int itemId;
        public int itemCount;
        public int vip;

        /// <summary>
        /// Get SignInJSON object by day.
        /// If that JSON data wasn't loaded, it will be loaded automatically.
        /// We recommend call this method directly, and no need care about other method.
        /// </summary>
        /// <returns>SignInJSON object</returns>
        public static SignInJSON Get(int day) => (IsInitialized ? (KissJson.Get(_type_, day.ToString()) as SignInJSON) : null);
        /// <summary>
        /// Get all keys of the whole JSON.
        /// You can query the whole JSON by this keys.
        /// </summary>
        /// <returns>All keys of the whole JSON</returns>
        public static List<string> GetKeys() => (IsInitialized ? KissJson.GetKeys(_type_) : (new List<string>()));
        /// <summary>
        /// Get SignInJSON object by key that from `GetKeys()`.
        /// </summary>
        /// <param name="key">The key from `GetKeys()`</param>
        /// <returns>SignInJSON object</returns>
        public static SignInJSON GetByKey(string key) => (IsInitialized ? (KissJson.Get(_type_, key) as SignInJSON) : null);
        /// <summary>
        /// Get all header names (column names).
        /// </summary>
        /// <returns>All header names (column names)</returns>
        public static List<string> GetHeaders() => (IsInitialized ? KissJson.GetHeaders(_type_) : (new List<string>()));
        /// <summary>
        /// Synchronous initialize SignInJSON.
        /// Normally you don't need to call this manually.
        /// </summary>
        public static bool InitializeSynchronous()
        {
            KissJson.Load((object)_type_, System.Environment.CurrentDirectory + "/KissJson/SignInJSON.json");
            _IsInitialized_ = true;
            return true;
        }
        /// <summary>
        /// It's for you can reload SignInJSON from web. 
        /// e.g. you modified the SignInJSON file and want to reload that JSON file while the server is running..
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [WebMethod]
        public static string ReloadSignInJSON(string ip)
        {
            Logger.LogInfo($"ReloadSignInJSON from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            InitializeSynchronous();

            return ret.ToJson();
        }
        static System.Type _type_ = typeof(SignInJSON);
        static bool _IsInitialized_ = false;
        static bool IsInitialized => (_IsInitialized_ ? true : InitializeSynchronous());
    }
}

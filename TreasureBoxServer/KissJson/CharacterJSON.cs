/*
 * It's automatic generate by 'KissJSON', DON'T modify this file.
 * You should edit it by 'KissJSON'.
 */
using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{
    public class CharacterJSON
    {
        public int id; //Primary key
        public string name;
        public string name_zh;
        public string icon;
        public int type;
        public int aTK;
        public int dEF;
        public int mAG;
        public int mDEF;
        public int sPD;
        public int hP;
        public int hIT;
        public int eVD;
        public int cRT;
        public int cRTD;
        public int addATK;
        public int addDEF;
        public int addMAG;
        public int addMDEF;
        public int addSPD;
        public int addHP;
        public int addHIT;
        public int addEVD;
        public int addCRT;
        public int addCRTD;
        public string Name => (Framework.config.Language switch {"zh" => name_zh, _ => name });

        /// <summary>
        /// Get CharacterJSON object by id.
        /// If that JSON data wasn't loaded, it will be loaded automatically.
        /// We recommend call this method directly, and no need care about other method.
        /// </summary>
        /// <returns>CharacterJSON object</returns>
        public static CharacterJSON Get(int id) => (IsInitialized ? (KissJson.Get(_type_, id.ToString()) as CharacterJSON) : null);
        /// <summary>
        /// Get all keys of the whole JSON.
        /// You can query the whole JSON by this keys.
        /// </summary>
        /// <returns>All keys of the whole JSON</returns>
        public static List<string> GetKeys() => (IsInitialized ? KissJson.GetKeys(_type_) : (new List<string>()));
        /// <summary>
        /// Get CharacterJSON object by key that from `GetKeys()`.
        /// </summary>
        /// <param name="key">The key from `GetKeys()`</param>
        /// <returns>CharacterJSON object</returns>
        public static CharacterJSON GetByKey(string key) => (IsInitialized ? (KissJson.Get(_type_, key) as CharacterJSON) : null);
        /// <summary>
        /// Get all header names (column names).
        /// </summary>
        /// <returns>All header names (column names)</returns>
        public static List<string> GetHeaders() => (IsInitialized ? KissJson.GetHeaders(_type_) : (new List<string>()));
        /// <summary>
        /// Synchronous initialize CharacterJSON.
        /// Normally you don't need to call this manually.
        /// </summary>
        public static bool InitializeSynchronous()
        {
            KissJson.Load((object)_type_, System.Environment.CurrentDirectory + "/KissJson/CharacterJSON.json");
            _IsInitialized_ = true;
            return true;
        }
        /// <summary>
        /// It's for you can reload CharacterJSON from web. 
        /// e.g. you modified the CharacterJSON file and want to reload that JSON file while the server is running..
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [WebMethod]
        public static string ReloadCharacterJSON(string ip)
        {
            Logger.LogInfo($"ReloadCharacterJSON from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            InitializeSynchronous();

            return ret.ToJson();
        }
        static System.Type _type_ = typeof(CharacterJSON);
        static bool _IsInitialized_ = false;
        static bool IsInitialized => (_IsInitialized_ ? true : InitializeSynchronous());
    }
}

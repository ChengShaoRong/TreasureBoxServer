
using CSharpLike;
using KissFramework;
using System.Collections.Generic;

namespace KissServerFramework
{
    /// <summary>
    /// The config load from JSON file(Environment.CurrentDirectory + "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".json").
    /// The field of ConfigBase is internal use by FrameworkBase.
    /// You can custom this class and modify the JSON file.
    /// Here are the sample read the 'welcomeMsg' of this class from JSON file:
    /// string msg = Framework.config.welcomeMsg;
    /// </summary>
    public class Config : ConfigBase
    {
        /// <summary>
        /// sample for config JSON : {"sampleInt":10}
        /// </summary>
        public int sampleInt = 123;
        /// <summary>
        /// sample for config JSON : {"sampleStringList":["a","abc","cba"]}
        /// </summary>
        public List<string> sampleStringList = new List<string>();
        /// <summary>
        /// sample for config JSON : {"sampleIntList":[1,2,3]}
        /// </summary>
        public List<int> sampleIntList = new List<int>();
        /// <summary>
        /// sample for config JSON : {"sampleStringIntDictionary":{"aa":1.2,"bb":3.2}}
        /// </summary>
        public Dictionary<string, float> sampleStringIntDictionary = new Dictionary<string, float>();
        /// <summary>
        /// sample for config JSON : {"sampleStringStringDictionary":{"aa":"xyz","bb":"abc"}}
        /// </summary>
        public Dictionary<string, string> sampleStringStringDictionary = new Dictionary<string, string>();
        /// <summary>
        /// How many seconds the account data keep in cache after account offline.
        /// </summary>
        public int accountCacheTime = 60;
        /// <summary>
        /// MD5 hash salt
        /// </summary>
        public string passwordHashSalt = "K/SdftgfDJf~wusa#d(djk8@Mdm8_xT,SFwq]DSF!DFdwWqSFmbvIlO8^SdGsfg2{XC3@dfHL5Xs|dk;DS&%FS*FVrgd6fdg3";
        /// <summary>
        /// The max count of player can leave message.
        /// </summary>
        public int maxMessages = 1000;
        /// <summary>
        /// GM IP, you can protect your secure operation.
        /// </summary>
        public string gmIP = "127.0.0.1";
        /// <summary>
        /// Each IP max save message count in every 30 minutes
        /// </summary>
        public int saveMessageCount = 3;
        /// <summary>
        /// Build-in account verify URL
        /// </summary>
        public string buildInAccountURL = "http://127.0.0.1:9002/AuthVerify";
        /// <summary>
        /// Default language
        /// </summary>
        public string Language = "zh";
        /// <summary>
        /// How many hours do the log file keep in './Log/*.log'. Check it in every 1 hour.
        /// </summary>
        public int keepLogHourTime = 24;
    }
}

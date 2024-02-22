
using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{

    /// <summary>
    /// Define a class for the CSV table (SignIn).
    /// </summary>
    public class SignInCsv
    {
        public int day;
        public int itemId;
        public int itemCount;
        public int vip;

        public static SignInCsv Get(int day)
        {
            return KissCSV.Get("SignIn.csv", day) as SignInCsv;
        }
        public Dictionary<int,int> GetReward(int vip)
        {
            Dictionary<int, int> items = new Dictionary<int, int>();
            if (this.vip > 0 && this.vip <= vip)
                items[itemId] = itemCount * 10;
            else
                items[itemId] = itemCount;
            return items;
        }
        public static void Init()
        {
            KissCSV.Load(typeof(SignInCsv), "SignIn.csv", "day");
        }

        [WebMethod]
        public static string ReloadSignInCSV(string ip)
        {
            Logger.LogInfo($"ReloadSignInCSV from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            Init();

            return ret.ToJson();
        }
    }
}

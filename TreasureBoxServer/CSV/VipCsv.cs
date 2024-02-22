
using CSharpLike;
using KissFramework;
using System.Collections.Generic;

namespace TreasureBox
{

    /// <summary>
    /// Define a class for the CSV table (Vip).
    /// </summary>
    public class VipCsv
    {
        public int vip;
        public int exp;
        /// <summary>
        /// Get the localization name
        /// </summary>
        public string Name
        {
            get
            {
                return Framework.config.Language == "EN" ? nameEN : nameCN;
            }
        }
        public string nameEN;
        public string nameCN;
        /// <summary>
        /// Get the localization describe
        /// </summary>
        public string Desc
        {
            get
            {
                return Framework.config.Language == "EN" ? descEN : descCN;
            }
        }
        public string descEN;
        public string descCN;
        public string icon;

        static SortedDictionary<int, int> mVipExp = new SortedDictionary<int, int>();
        public static int GetVip(int exp)
        {
            if (exp == 0)
                return 0;
            int vip = 0;
            foreach (var one in mVipExp)
            {
                if (exp < one.Value)
                    return vip;
                vip = one.Key;
            }
            return vip;
        }
        public static VipCsv Get(int vip)
        {
            return KissCSV.Get("Vip.csv", vip) as VipCsv;
        }

        public static void Init()
        {
            KissCSV.Load(typeof(VipCsv), "Vip.csv", "vip");
        }

        [WebMethod]
        public static string ReloadVipCSV(string ip)
        {
            Logger.LogInfo($"ReloadVipCSV from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            Init();

            return ret.ToJson();
        }
        static void InitDic()
        {
            mVipExp = new SortedDictionary<int, int>();
            foreach (var one in KissCSV.GetData("VipCsv").Values)
            {
                VipCsv csv = one as VipCsv;
                mVipExp[csv.vip] = csv.exp;
            }
        }
    }
}


using CSharpLike;
using System.Collections.Generic;
using KissFramework;

namespace TreasureBox
{

    /// <summary>
    /// Define a class for the CSV table (Character).
    /// </summary>
    public class CharacterCsv
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        public enum CharacterType
        {
            /// <summary>
            /// 战士
            /// </summary>
            Warrior,
            /// <summary>
            /// 法师
            /// </summary>
            Mage,
            /// <summary>
            /// 刺客
            /// </summary>
            Rogue,
            /// <summary>
            /// 牧师
            /// </summary>
            Prisst,
        }
        public int id;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 图标
        /// </summary>
        public string icon;
        /// <summary>
        /// 角色类型
        /// </summary>
        public CharacterType Type;

        /// <summary>
        /// Physical Attack 物理攻击力
        /// </summary>
        public int ATK;
        /// <summary>
        /// Physical defense 物理防御
        /// </summary>
        public int DEF;
        /// <summary>
        /// Magic attack 魔法攻击力
        /// </summary>
        public int MAG;
        /// <summary>
        /// Magic defence 魔法防御
        /// </summary>
        public int MDEF;
        /// <summary>
        /// Speed 速度
        /// </summary>
        public int SPD;
        /// <summary>
        /// Health Point 生命
        /// </summary>
        public int HP;
        /// <summary>
        /// Hit 命中
        /// </summary>
        public int HIT;
        /// <summary>
        /// Evasion 闪避
        /// </summary>
        public int EVD;
        /// <summary>
        /// Critical Rate 暴击率
        /// </summary>
        public int CRT;
        /// <summary>
        /// Critical Damage 暴击倍数
        /// </summary>
        public int CRTD;
        public static CharacterCsv Get(int id)
        {
            return KissCSV.Get("Character.csv", id) as CharacterCsv;
        }
        public static void Init()
        {
            KissCSV.Load(typeof(CharacterCsv), "Character.csv", "id");
        }
        [WebMethod]
        public static string ReloadCharacterCSV(string ip)
        {
            Logger.LogInfo($"ReloadCharacterCSV from {ip}");
            JSONData ret = Framework.CreateReturnJSON();
            if (Framework.IsInvalidIp(ip, ret))
                return ret;
            Init();

            return ret.ToJson();
        }
    }
}

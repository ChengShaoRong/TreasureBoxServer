
using CSharpLike;
using KissFramework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KissServerFramework;
using System.Text;

namespace TreasureBox
{
    public class Global
    {
        public static string HashPassword(string password)
        {
            return Framework.GetMD5(password.Trim()+Framework.config.passwordHashSalt);
        }
        static Regex emailRegex = new Regex(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+$");
        public static bool ValidMail(string address)
        {
            if (address.Length > 64)
                return false;
            return emailRegex.IsMatch(address);
        }
        static Regex guestRegex = new Regex(@"^([0-9]{2,9})+$");
        public static bool IsGuestName(string name)
        {
            return (name.StartsWith("guest#") && name.Length > 7 && guestRegex.IsMatch(name.Substring(6)));
        }
        static Regex nameRegex = new Regex(@"^([a-zA-Z0-9_\.\@\-]{6,64})+$");
        public static bool ValidName(string name)
        {
            if (IsGuestName(name))
                return true;
            return nameRegex.IsMatch(name);
        }
        public static bool ValidPassword(string password)
        {
            return (password.Length >= 6 && password.Length <= 64) || password.Length == 0;
        }

        public static List<int> StringToList(string strs)
        {
            List<int> ret = new List<int>();
            if (!string.IsNullOrEmpty(strs))
            {
                foreach (string str in strs.Split(','))
                {
                    ret.Add(Convert.ToInt32(str));
                }
            }
            return ret;
        }
        public static string ListToString(List<int> list)
        {
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (int i in list)
                {
                    sb.Append(i + ",");
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return "";
        }
        public static Dictionary<int,int> StringToDictionary(string strs)
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(strs))
            {
                foreach (string str in strs.Split(' '))
                {
                    string[] strs2 = str.Split(',');
                    if (strs2.Length == 2)
                    {
                        ret[Convert.ToInt32(strs2[0])] = Convert.ToInt32(strs2[1]);
                    }
                }
            }
            return ret;
        }
        public static string DictionaryToString(Dictionary<int, int> dictionary)
        {
            if (dictionary != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var one in dictionary)
                {
                    sb.Append(one.Key + " " + one.Value + ",");
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return "";
        }
    }
}

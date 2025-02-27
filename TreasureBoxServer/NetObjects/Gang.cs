/*
 * C#Like 
 * Copyright © 2022-2025 RongRong 
 * It's automatic generate by KissNetObject.
 */


using System;
using CSharpLike;
using KissFramework;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace TreasureBox
{
	public sealed class Gang : Gang_Base, IComparable
    {
        #region members
        public Dictionary<int, GangMember> members = new Dictionary<int, GangMember>();
		public void LoadAllMembers()
        {

        }
        public bool IsGangMemberInited => members != null;
        public GangMember GetMemberByAcctId(int acctId)
        {
            if (members != null && members.TryGetValue(acctId, out GangMember member))
                return member;
            return null;
        }
        #endregion //members

        public JSONData Members
        {
            get
            {
                JSONData list = JSONData.NewList();
                foreach (GangMember member in members.Values)
                    list.Add(member.ToJSONData());
                return list;
            }
        }

        public int CompareTo(object obj)
        {
            Gang other = obj as Gang;
            if (lv > other.lv)
                return -1;
            else if (lv < other.lv)
                return 1;
            else if (exp > other.exp)
                return -1;
            else if (exp > other.exp)
                return 1;
            return uid < other.uid ? 1 : -1;
        }

        public void AddLog(string log)
        {
#if FREE_VERSION
            Logs.Insert(0, log);
            while (Logs.Count > 50)
                Logs.RemoveAt(Logs.Count - 1);
            logs = NetObjectUtils.ListToString(Logs);
#else
            logs.Insert(0, log);
            while (logs.Count > 50)
                logs.RemoveAt(logs.Count - 1);
            logsMarkChanged();
#endif
        }
        public int MaxCount => lv * 5 + 10;
        public int CurrentCount => members.Count;
#if FREE_VERSION
        List<string> mLog;
        public List<string> Logs
        {
            get
            {
                if (mLog == null)
                    mLog = NetObjectUtils.StringToList<string>(logs);
                return mLog;
            }
            set
            {
                mLog = value;
                logs = NetObjectUtils.ListToString(value);
            }
        }
        List<int> mApplies;
        public List<int> Applies
        {
            get
            {
                if (mApplies == null)
                    mApplies = NetObjectUtils.StringToList<int>(applies);
                return mApplies;
            }
            set
            {
                mApplies = value;
                applies = NetObjectUtils.ListToString(value);
            }
        }
#endif
    }
}

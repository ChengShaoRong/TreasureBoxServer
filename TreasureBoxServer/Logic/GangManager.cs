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
	public class GangManager
    {
        static Dictionary<int, Gang> gangs = null;
		public static void Init()
        {
			Logger.LogInfo("Start Init gang.");
            Gang_Base.SelectAll((listGangs, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    Logger.LogError($"Load gangs with error : {error}");
                    return;
                }
                GangMember_Base.Select("SELECT * FROM `GangMember`", new List<MySqlParameter>(),
                    (listMembers, error)=>
                    {
                        if (!string.IsNullOrEmpty(error))
                        {
                            Logger.LogError($"Load gang members with error : {error}");
                            return;
                        }
                        gangs = new Dictionary<int, Gang>();
                        foreach (Gang gang in listGangs)
                        {
                            gangs[gang.uid] = gang;
                        }
                        foreach (GangMember member in listMembers)
                        {
                            gangMembers[member.acctId] = member;
                            int gangId = member.gangId;
                            if (gangId > 0)
                            {
                                Gang gang = GetGangById(gangId);
                                if (gang != null)
                                {
                                    gang.members[member.acctId] = member;
                                }
                                else
                                {
                                    member.gangId = 0;//Fix it if error
                                }
                            }
                        }
                        Logger.LogInfo($"Init gang done. gang count = {gangs.Count}, gang member count = {gangMembers.Count}");
                    });
            });
        }
		public static bool IsGangInited => gangs != null;
		public static Gang GetGangById(int gangId)
        {
			if (gangs != null && gangs.TryGetValue(gangId, out Gang gang))
				return gang;
			return null;
        }
        public static bool ExistGangName(string name)
        {
            foreach(Gang gang in gangs.Values)
            {
                if (gang.name.ToLower() == name)
                    return true;
            }
            return false;
        }
        static Dictionary<int, GangMember> gangMembers = new Dictionary<int, GangMember>();
        public static GangMember GetGangMemberById(int acctId)
        {
            if (gangMembers.TryGetValue(acctId, out GangMember member))
                return member;
            return null;
        }
        public static void GetGangMemberById(int acctId, string nickname, int lv, Action<GangMember,string> callback)
        {
            if (gangMembers.TryGetValue(acctId, out GangMember member))
            {
                callback?.Invoke(member, "");
                return;
            }
            GangMember_Base.Insert(acctId, nickname, 0, 0, lv, DateTime.Now, true, (member, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    callback?.Invoke(null, error);
                    return;
                }
                if (member == null)
                {
                    callback?.Invoke(null, "member null");
                    return;
                }
                if (gangMembers.TryGetValue(acctId, out GangMember memberOld))
                {
                    if (member.uid != memberOld.uid)
                        member.Delete();
                    callback?.Invoke(memberOld, "");
                    return;
                }
                gangMembers[acctId] = member;
                callback?.Invoke(member, "");
            });
        }


        #region process packet from client
        public static void GangGet(Player player)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            Gang gang = player.account.gang;
            if (gang == null)
            {
                player.CB_Tips("LT_GangNull");
                player.account.gangId = 0;
                return;
            }
            GangMember member = gang.GetMemberByAcctId(player.account.uid);
            if (member == null)
            {
                player.CB_Tips("LT_GangMemberNull");
                player.account.gangId = 0;
                return;
            }
            player.CB_GangGet(gang.ToJSONData(), gang.Members);
        }
        public static void GangCreate(Player player, string name)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            if (ExistGangName(name.ToLower()))
            {
                player.CB_Tips("LT_GangExistName");
                return;
            }
            int acctId = player.account.uid;
            GetGangMemberById(player.account.uid, player.account.nickname, player.account.lv, (member, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    player.CB_Tips(error);
                    return;
                }
                Gang_Base.Insert(name, acctId, player.account.nickname, 1, 0, null, 0, null, true, (gang, error) =>
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        player.CB_Tips(error);
                        return;
                    }
                    if (player.account.gang != null)
                    {
                        player.CB_Tips("LT_GangAlreadyInGang");
                        gang.Delete();
                        return;
                    }
                    if (ExistGangName(name.ToLower()))//Must check again
                    {
                        player.CB_Tips("LT_GangExistName");
                        gang.Delete();
                        return;
                    }
                    gangs[gang.uid] = gang;
                    player.account.gangId = gang.uid;
                    member.gangId = gang.uid;
                    member.position = (int)GangMember.Position.Leader;
                    gang.members[acctId] = member;
                    gang.AddLog($"`{player.account.nickname}` create gang `{name}` at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    player.CB_GangCreate(gang.ToJSONData(), gang.Members);
                });
            });
        }

        public static void GangQuit(Player player)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            Gang gang = player.account.gang;
            if (gang == null)
            {
                player.CB_Tips("LT_GangNotInGang");
                if (player.account.gangId > 0)//fixes the gang id to 0
                    player.account.gangId = 0;
                return;
            }
            if (!gang.IsGangMemberInited)
            {
                player.CB_Tips("LT_GangMemberNotInited");
                return;
            }
            GetGangMemberById(player.account.uid, player.account.nickname, player.account.lv, (member, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    player.CB_Tips(error);
                    return;
                }
                gang.members.Remove(member.acctId);
                member.gangId = 0;
                player.CB_GangQuit();
                gang.AddLog($"`{player.account.nickname}` quit gang `{gang.name}` at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            });
        }

        public static void GangGetList(Player player, int page)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            List<Gang> list = new List<Gang>();
            foreach(Gang gang in gangs.Values)
            {
                list.Add(gang);
            }
            list.Sort();
            int max = Math.Min((page - 1) * 20 + 20, gangs.Count);
            JSONData data = JSONData.NewList();
            for (int i = (page - 1) * 20; i < max; i++)
            {
                Gang gang = list[i];
                data.Add(gang.ToJSONData());
            }
            player.CB_GangGetList(page, list.Count, data);
        }

        public static void GangGetApplies(Player player)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            Gang gang = player.account.gang;
            if (gang == null)
            {
                player.CB_Tips("LT_GangNotInGang");
                if (player.account.gangId > 0)//fixes the gang id to 0
                    player.account.gangId = 0;
                return;
            }
            if (!gang.IsGangMemberInited)
            {
                player.CB_Tips("LT_GangMemberNotInited");
                return;
            }
            JSONData data = JSONData.NewList();
#if FREE_VERSION
            for (int i = gang.Applies.Count - 1; i >= 0; i--)
            {
                int apply = gang.Applies[i];
                if (gangMembers.TryGetValue(apply, out GangMember member)
                    && member.gangId == 0)
                {
                    data.Add(member.ToJSONData());
                }
                else
                {
                    gang.Applies.RemoveAt(i);
                    gang.applies = NetObjectUtils.ListToString(gang.Applies);
                }
            }
#else
            for (int i = gang.applies.Count - 1; i >= 0; i--)
            {
                int apply = gang.applies[i];
                if (gangMembers.TryGetValue(apply, out GangMember member)
                    && member.gangId == 0)
                {
                    data.Add(member.ToJSONData());
                }
                else
                {
                    gang.applies.RemoveAt(i);
                    gang.appliesMarkChanged();
                }
            }
#endif
            player.CB_GangGetApplies(data);
        }

        public static void GangApply(Player player, int id)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            Gang gang = player.account.gang;
            if (gang != null)
            {
                player.CB_Tips("LT_GangAlreadyInGang");
                return;
            }
            int acctId = player.account.uid;
            GetGangMemberById(acctId, player.account.nickname, player.account.lv, (member, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    player.CB_Tips(error);
                    return;
                }
                gang = GetGangById(id);
                if (gang == null)
                {
                    player.CB_Tips("LT_GangNull");
                    return;
                }
                if (gang.CurrentCount >= gang.MaxCount)
                {
                    player.CB_Tips("LT_GangReachMaxCount");
                    return;
                }
#if FREE_VERSION
                if (gang.Applies.Count > 30)
                {
                    player.CB_Tips("LT_GangApplyReachMaxCount");
                    return;
                }
                if (gang.Applies.Contains(acctId))
                {
                    player.CB_Tips("LT_GangApplied");
                    return;
                }
#else
                if (gang.applies.Count > 30)
                {
                    player.CB_Tips("LT_GangApplyReachMaxCount");
                    return;
                }
                if (gang.applies.Contains(acctId))
                {
                    player.CB_Tips("LT_GangApplied");
                    return;
                }
#endif
                if (gang.autoAccept)
                {
                    member.position = (int)GangMember.Position.Member;
                    member.gangId = gang.uid;
                    player.account.gangId = gang.uid;
                    gang.members[acctId] = member;
                    gang.AddLog($"`{player.account.nickname}` enter gang `{gang.name}` at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    GangGet(player);
                }
                else
                {
#if FREE_VERSION
                    gang.Applies.Add(acctId);
                    gang.applies = NetObjectUtils.ListToString(gang.Applies);
#else
                    gang.applies.Add(acctId);
                    gang.appliesMarkChanged();
#endif
                    player.CB_GangApply(id);
                    foreach (GangMember one in gang.members.Values)
                    {
                        switch ((GangMember.Position)one.position)
                        {
                            case GangMember.Position.Leader:
                            case GangMember.Position.Manager:
                                {
                                    Player playerNotify = AccountManager.Instance.GetPlayer(one.acctId);
                                    playerNotify?.CB_Tips("LT_GangApply", false);
                                }
                                break;
                        }
                    }
                }
            });
        }


        public static void GangApplyManager(Player player, int uid, bool accept)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            if (uid == 0)
            {
                player.CB_Tips("uid = 0.");
                return;
            }
            Gang gang = player.account.gang;
            if (gang == null)
            {
                player.CB_Tips("LT_GangNotInGang");
                return;
            }
            GangMember self = gang.GetMemberByAcctId(player.account.uid);
            if (self == null)
            {
                player.CB_Tips("LT_GangMemberInfoError");
                return;
            }
            if (!(self.position == (int)GangMember.Position.Leader || self.position == (int)GangMember.Position.Manager))
            {
                player.CB_Tips("LT_GangNotLeaderOrManager");
                return;
            }
            if (accept)
            {
#if FREE_VERSION
                gang.Applies.Remove(uid);
#else
                gang.applies.Remove(uid);
#endif
                GangMember member = GetGangMemberById(uid);
                if (member != null && member.gangId == 0)
                {
                    member.gangId = gang.uid;
                    member.position = (int)GangMember.Position.Member;
                    gang.members[uid] = member;
                    player.CB_GangApplyManager(uid, accept, member.ToJSONData());
                }
            }
            else
            {
#if FREE_VERSION
                gang.Applies.Remove(uid);
#else
                gang.applies.Remove(uid);
#endif
                player.CB_GangApplyManager(uid, accept, null);
            }
#if FREE_VERSION
#else
            gang.appliesMarkChanged();
#endif
        }
        public static void GangKick(Player player, int uid)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            Gang gang = player.account.gang;
            if (gang == null)
            {
                player.CB_Tips("LT_GangNotInGang");
                return;
            }
            GangMember self = gang.GetMemberByAcctId(player.account.uid);
            if (self == null)
            {
                player.CB_Tips("LT_GangMemberInfoError");
                return;
            }
            if (!(self.position == (int)GangMember.Position.Leader || self.position == (int)GangMember.Position.Manager))
            {
                player.CB_Tips("LT_GangNotLeaderOrManager");
                return;
            }
            GangMember member = gang.GetMemberByAcctId(uid);
            if (member == null)
            {
                player.CB_Tips("LT_GangMemberInfoError");
                return;
            }
            if (member.position == (int)GangMember.Position.Leader)
            {
                player.CB_Tips("LT_GangCantKickLeader");
                return;
            }
            if (member == self)
            {
                player.CB_Tips("LT_GangCantKickYouself");
                return;
            }
            member.position = (int)GangMember.Position.Member;
            member.gangId = 0;
            gang.members.Remove(uid);
            gang.AddLog($"`{member.name}` was kicked out by `{self.name}` at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            player.CB_GangKick(uid);
        }

        public static void GangPosition(Player player, int uid, int position)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            Gang gang = player.account.gang;
            if (gang == null)
            {
                player.CB_Tips("LT_GangNotInGang");
                return;
            }
            GangMember self = gang.GetMemberByAcctId(player.account.uid);
            if (self == null)
            {
                player.CB_Tips("LT_GangMemberInfoError");
                return;
            }
            if (!(self.position == (int)GangMember.Position.Leader || self.position == (int)GangMember.Position.Manager))
            {
                player.CB_Tips("LT_GangNotLeaderOrManager");
                return;
            }
            GangMember member = gang.GetMemberByAcctId(uid);
            if (member == null)
            {
                player.CB_Tips("LT_GangNotExistMember");
                return;
            }
            if (member.position <= self.position)
            {
                player.CB_Tips("LT_GangPositionSmall");
                return;
            }
            if (member == self)
            {
                player.CB_Tips("LT_GangPositionSelf");
                return;
            }
            if (position < self.position)
            {
                player.CB_Tips("LT_GangPositionBig");
                return;
            }
            member.position = position;
            gang.AddLog($"`{member.name}` position change to {(GangMember.Position)position} by `{self.name}` at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            if (position == (int)GangMember.Position.Leader)//Mean change target as Leader, self position change to Member
            {
                self.position = (int)GangMember.Position.Member;
                gang.leaderId = member.acctId;
                gang.leaderName = member.name;
            }
            player.CB_GangPosition(uid, position);
        }

        public static void GangAutoAccept(Player player, bool autoAccept)
        {
            if (!IsGangInited)
            {
                player.CB_Tips("LT_GangNotInited");
                return;
            }
            Gang gang = player.account.gang;
            if (gang == null)
            {
                player.CB_Tips("LT_GangNotInGang");
                return;
            }
            GangMember self = gang.GetMemberByAcctId(player.account.uid);
            if (self == null)
            {
                player.CB_Tips("LT_GangMemberInfoError");
                return;
            }
            if (!(self.position == (int)GangMember.Position.Leader || self.position == (int)GangMember.Position.Manager))
            {
                player.CB_Tips("LT_GangNotLeaderOrManager");
                return;
            }
            gang.autoAccept = autoAccept;
            player.CB_GangAutoAccept(autoAccept);
        }
#endregion //process packet from client
    }
}

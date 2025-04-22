using CSharpLike;
using System;
using System.Collections.Generic;

namespace TreasureBox
{
	public sealed class SignIn : SignIn_Base
	{
		[KissJsonDontSerialize]
		List<int> mSignInDays = null;
		[KissJsonDontSerialize]
		List<int> SignInDays
		{
			get
			{
				if (mSignInDays == null)
					mSignInDays = Global.StringToList(signInList);
				return mSignInDays;
			}
		}
		public void GetReward(Player player)
		{
			int day = DateTime.Now.Day;
			if (SignInDays.Contains(day))
			{
				player.CB_Tips("LT_Tips_SignInRewardHadBeenToken", true);
				return;
			}
			mSignInDays.Add(day);
			signInList = Global.ListToString(mSignInDays);//This step will save to DB and sync to client in background thread
			Dictionary<int, int> items = GetReward(day, player.account.Vip);
			player.account.CB_GetReward(items, LogManagerEnum.LogItemType.SignIn);
		}

		public Dictionary<int, int> GetReward(int day, int vip)
		{
			Dictionary<int, int> items = new Dictionary<int, int>();
			SignInJSON json = SignInJSON.Get(day);
			if (json != null)
			{
				if (json.vip > 0 && json.vip <= vip)
					items[json.itemId] = json.itemCount * 10;
				else
					items[json.itemId] = json.itemCount;
			}
			return items;
		}
	}
}

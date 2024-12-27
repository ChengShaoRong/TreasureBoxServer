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
			Dictionary<int, int> items = SignInCsv.Get(day).GetReward(player.account.Vip);
			player.account.CB_GetReward(items, LogManagerEnum.LogItemType.SignIn);
		}
	}
}

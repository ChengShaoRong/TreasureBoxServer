/*
 * C#Like 
 * Copyright © 2022-2025 RongRong 
 * It's automatic generate by KissNetObject.
 */


using System;
using CSharpLike;
using KissFramework;
using System.Collections.Generic;

namespace TreasureBox
{
	public sealed class GangMember : GangMember_Base
	{ 
		public enum Position
        {
			Leader,
			Manager,
			Member
        }
		public Gang gang => GangManager.GetGangById(gangId);
	} 
}

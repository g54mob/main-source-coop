using System;
using UnityEngine;

namespace Features.CompositeItemModule.Scripts
{
	[Serializable]
	public class CompositeItemEntry
	{
		public CompositeItem Item;

		public CompositeItemBreakBehaviorType Behavior = CompositeItemBreakBehaviorType.JointBreak;

		public Joint JointToBreak;

		public float DamageToBreak = 10f;
	}
}

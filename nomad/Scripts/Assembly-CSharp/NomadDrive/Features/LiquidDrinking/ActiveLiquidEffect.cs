using System;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.LiquidDrinking
{
	[Serializable]
	public class ActiveLiquidEffect
	{
		public PlayerStatType StatType;

		public float TotalAmount;

		public float AppliedAmount;

		public float Duration;

		public float DelayRemaining;

		public float Elapsed;

		public bool IsComplete
		{
			get
			{
				if (!(Mathf.Abs(AppliedAmount) >= Mathf.Abs(TotalAmount) - 0.001f))
				{
					if (Duration > 0f)
					{
						return Elapsed >= Duration;
					}
					return false;
				}
				return true;
			}
		}

		public bool IsDelayed => DelayRemaining > 0f;

		public ActiveLiquidEffect(PlayerStatType statType, float totalAmount, float delay, float duration)
		{
			StatType = statType;
			TotalAmount = totalAmount;
			AppliedAmount = 0f;
			Duration = duration;
			DelayRemaining = delay;
			Elapsed = 0f;
		}
	}
}

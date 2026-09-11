using System;
using UnityEngine;

public class DealRewardMoney_0 : DealRewardBase
{
	public override bool UseInGame => false;

	public override DealRewardBase CreateNew()
	{
		return new DealRewardMoney_0();
	}

	public override string GetRewardDescription()
	{
		return $"In return you will get $<b><u>{GetAmount()}</b></u>";
	}

	private int GetAmount()
	{
		int num = 0;
		num = difficulty switch
		{
			DIFFICULTY.veryEasy => 0, 
			DIFFICULTY.easy => 1, 
			DIFFICULTY.medium => 2, 
			DIFFICULTY.hard => 3, 
			DIFFICULTY.veryHard => 4, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		return (int)((float)BigNumbers.GetMoneyFromScoreByRun(BigNumbers.GetQuota(num), num) * 0.5f);
	}

	public override bool ClaimReward()
	{
		SurfaceNetworkHandler.RoomStats.AddMoney(GetAmount());
		return true;
	}

	public override byte GetIndex()
	{
		return GetIndex<DealRewardMoney_0>();
	}

	public override string GetRewardClaimDescription()
	{
		return "the money will appear in your wallet";
	}

	public override void MakeLocalized()
	{
		Debug.LogError("Localization string not implemented!");
	}
}

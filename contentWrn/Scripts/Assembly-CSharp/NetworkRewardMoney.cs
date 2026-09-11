using System;
using UnityEngine;

public class NetworkRewardMoney : MonoBehaviour, NetworkReward
{
	public int veryEasyReward = 10;

	public int easyReward = 100;

	public int mediumReward = 1000;

	public int hardReward = 10000;

	public int veryHardReward = 100000;

	public void GiveReward(DIFFICULTY difficulty)
	{
	}

	public string GetRewardDescription(DIFFICULTY difficulty)
	{
		int cashRewardAmount = GetCashRewardAmount(difficulty);
		return $"${cashRewardAmount}";
	}

	public int GetCashRewardAmount(DIFFICULTY difficulty)
	{
		return difficulty switch
		{
			DIFFICULTY.veryEasy => veryEasyReward, 
			DIFFICULTY.easy => easyReward, 
			DIFFICULTY.medium => mediumReward, 
			DIFFICULTY.hard => hardReward, 
			DIFFICULTY.veryHard => veryHardReward, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}
}

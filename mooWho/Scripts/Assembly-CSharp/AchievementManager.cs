using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	private CallResult<UserStatsReceived_t> _userStatsReceivedCallResult;

	private bool _statsReady;

	private readonly HashSet<string> _unlockedCache = new HashSet<string>();

	private readonly Queue<string> _pending = new Queue<string>();

	private bool _storePending;

	public static AchievementManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		if (SteamManager.Initialized)
		{
			_userStatsReceivedCallResult = CallResult<UserStatsReceived_t>.Create(OnUserStatsReceived);
			SteamAPICall_t hAPICall = SteamUserStats.RequestUserStats(SteamUser.GetSteamID());
			_userStatsReceivedCallResult.Set(hAPICall);
		}
	}

	public void Unlock(string achievementApiName)
	{
		if (!string.IsNullOrEmpty(achievementApiName) && SteamManager.Initialized && !_unlockedCache.Contains(achievementApiName))
		{
			if (!_statsReady)
			{
				_pending.Enqueue(achievementApiName);
			}
			else
			{
				UnlockInternal(achievementApiName);
			}
		}
	}

	public void IncrementStatAndUnlock(string statKey, int threshold, string achievementApiName)
	{
		if (!string.IsNullOrEmpty(statKey))
		{
			int num = PlayerPrefs.GetInt(statKey, 0) + 1;
			PlayerPrefs.SetInt(statKey, num);
			if (num >= threshold)
			{
				Unlock(achievementApiName);
			}
		}
	}

	private void OnUserStatsReceived(UserStatsReceived_t callback, bool bIOFailure)
	{
		if (!bIOFailure && callback.m_eResult == EResult.k_EResultOK)
		{
			_statsReady = true;
			while (_pending.Count > 0)
			{
				UnlockInternal(_pending.Dequeue());
			}
		}
	}

	private void UnlockInternal(string achievementApiName)
	{
		if (SteamUserStats.GetAchievement(achievementApiName, out var pbAchieved) && pbAchieved)
		{
			_unlockedCache.Add(achievementApiName);
		}
		else if (SteamUserStats.SetAchievement(achievementApiName))
		{
			_unlockedCache.Add(achievementApiName);
			RequestStore();
			if (achievementApiName != "ACH_FARM_LEGEND")
			{
				CheckFarmLegend();
			}
		}
	}

	private void CheckFarmLegend()
	{
		if (_unlockedCache.Contains("ACH_FARM_LEGEND"))
		{
			return;
		}
		string[] allExceptFarmLegend = Achievements.AllExceptFarmLegend;
		foreach (string text in allExceptFarmLegend)
		{
			if (!_unlockedCache.Contains(text) && !(SteamUserStats.GetAchievement(text, out var pbAchieved) && pbAchieved))
			{
				return;
			}
		}
		UnlockInternal("ACH_FARM_LEGEND");
	}

	private void RequestStore()
	{
		if (!_storePending)
		{
			_storePending = true;
			StartCoroutine(StoreStatsNextFrame());
		}
	}

	private IEnumerator StoreStatsNextFrame()
	{
		yield return null;
		_storePending = false;
		SteamUserStats.StoreStats();
	}
}

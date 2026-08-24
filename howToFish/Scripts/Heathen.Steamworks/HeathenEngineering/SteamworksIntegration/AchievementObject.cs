using System;
using HeathenEngineering.Events;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[HelpURL("https://kb.heathenengineering.com/assets/steamworks/achievement-object")]
	[CreateAssetMenu(menuName = "Steamworks/Achievement Object")]
	public class AchievementObject : ScriptableObject
	{
		[HideInInspector]
		[SerializeField]
		private AchievementData data;

		public UnityBoolEvent StatusChanged = new UnityBoolEvent();

		public string Id
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
			}
		}

		public string Name => data.Name;

		public string Description => data.Description;

		public bool Hidden => data.Hidden;

		public bool IsAchieved
		{
			get
			{
				return data.IsAchieved;
			}
			set
			{
				data.IsAchieved = value;
			}
		}

		public DateTime? UnlockTime => data.UnlockTime;

		public void Unlock()
		{
			IsAchieved = true;
		}

		public void ClearAchievement()
		{
			IsAchieved = false;
		}

		public void Unlock(CSteamID user)
		{
			data.Unlock(user);
		}

		public void ClearAchievement(UserData user)
		{
			data.ClearAchievement(user);
		}

		public bool GetAchievementStatus(CSteamID user)
		{
			return data.GetAchievementStatus(user);
		}

		public (bool unlocked, DateTime unlockTime) GetAchievementAndUnlockTime(UserData user)
		{
			return data.GetAchievementAndUnlockTime(user);
		}

		public void GetIcon(Action<Texture2D> callback)
		{
			StatsAndAchievements.Client.GetAchievementIcon(data, callback);
		}

		public void Store()
		{
			StatsAndAchievements.Client.StoreStats();
		}

		public static AchievementObject CreateScriptableObject(string apiName)
		{
			AchievementObject achievementObject = ScriptableObject.CreateInstance<AchievementObject>();
			achievementObject.Id = apiName;
			return achievementObject;
		}
	}
}

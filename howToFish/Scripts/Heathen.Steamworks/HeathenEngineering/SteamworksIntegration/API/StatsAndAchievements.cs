using System;
using System.Collections.Generic;
using System.Linq;
using HeathenEngineering.Events;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.API
{
	public static class StatsAndAchievements
	{
		public static class Client
		{
			private class ImageRequestCallbackLink
			{
				public bool isAchievement;

				public string apiName;

				public Action<Texture2D> callback;
			}

			private static List<ImageRequestCallbackLink> pendingLinks = new List<ImageRequestCallbackLink>();

			private static Dictionary<int, Texture2D> loadedImages = new Dictionary<int, Texture2D>();

			private static UserStatsReceivedEvent eventUserStatsReceived = new UserStatsReceivedEvent();

			private static UserStatsUnloadedEvent eventUserStatsUnloaded = new UserStatsUnloadedEvent();

			private static UserStatsStoredEvent eventUserStatsStored = new UserStatsStoredEvent();

			private static UserAchievementStoredEvent eventUserAchievementStored = new UserAchievementStoredEvent();

			private static Callback<UserAchievementIconFetched_t> m_UserAchievementIconFetched_t;

			private static Callback<UserStatsReceived_t> m_UserStatsReceived_t;

			private static Callback<UserStatsUnloaded_t> m_UserStatsUnload_t;

			private static Callback<UserAchievementStored_t> m_UserAchievementStored_t;

			private static Callback<UserStatsStored_t> m_UserStatsStored_t;

			private static CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers_t;

			private static CallResult<GlobalAchievementPercentagesReady_t> m_GlobalAchievementPercentagesReady_t;

			private static CallResult<GlobalStatsReceived_t> m_GlobalStatsReceived_t;

			private static CallResult<UserStatsReceived_t> m_UserStatsReceived_t2;

			public static UserStatsReceivedEvent EventUserStatsReceived
			{
				get
				{
					if (m_UserStatsReceived_t == null)
					{
						m_UserStatsReceived_t = Callback<UserStatsReceived_t>.Create(delegate(UserStatsReceived_t r)
						{
							eventUserStatsReceived.Invoke(r);
						});
					}
					return eventUserStatsReceived;
				}
			}

			public static UserStatsUnloadedEvent EventUserStatsUnloaded
			{
				get
				{
					if (m_UserStatsUnload_t == null)
					{
						m_UserStatsUnload_t = Callback<UserStatsUnloaded_t>.Create(eventUserStatsUnloaded.Invoke);
					}
					return eventUserStatsUnloaded;
				}
			}

			public static UserStatsStoredEvent EventUserStatsStored
			{
				get
				{
					if (m_UserStatsStored_t == null)
					{
						m_UserStatsStored_t = Callback<UserStatsStored_t>.Create(eventUserStatsStored.Invoke);
					}
					return eventUserStatsStored;
				}
			}

			public static UserAchievementStoredEvent EventUserAchievementStored
			{
				get
				{
					if (m_UserAchievementStored_t == null)
					{
						m_UserAchievementStored_t = Callback<UserAchievementStored_t>.Create(eventUserAchievementStored.Invoke);
					}
					return eventUserAchievementStored;
				}
			}

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Init()
			{
				pendingLinks = new List<ImageRequestCallbackLink>();
				if (loadedImages != null)
				{
					foreach (KeyValuePair<int, Texture2D> loadedImage in loadedImages)
					{
						UnityEngine.Object.Destroy(loadedImage.Value);
					}
				}
				loadedImages = new Dictionary<int, Texture2D>();
				eventUserStatsReceived = new UserStatsReceivedEvent();
				eventUserStatsUnloaded = new UserStatsUnloadedEvent();
				eventUserStatsStored = new UserStatsStoredEvent();
				eventUserAchievementStored = new UserAchievementStoredEvent();
				m_UserAchievementIconFetched_t = null;
				m_UserStatsReceived_t = null;
				m_UserStatsUnload_t = null;
				m_UserAchievementStored_t = null;
				m_UserStatsStored_t = null;
				m_NumberOfCurrentPlayers_t = null;
				m_GlobalAchievementPercentagesReady_t = null;
				m_GlobalStatsReceived_t = null;
				m_UserStatsReceived_t2 = null;
			}

			public static bool ClearAchievement(string achievementApiName)
			{
				bool num = SteamUserStats.ClearAchievement(achievementApiName);
				if (num && SteamSettings.current != null)
				{
					AchievementObject achievementObject = SteamSettings.Achievements.FirstOrDefault((AchievementObject p) => p.Id == achievementApiName);
					if (achievementObject != null)
					{
						UnityBoolEvent statusChanged = achievementObject.StatusChanged;
						if (statusChanged == null)
						{
							return num;
						}
						statusChanged.Invoke(achievementObject.IsAchieved);
					}
				}
				return num;
			}

			public static bool GetAchievement(string achievementApiName, out bool achieved)
			{
				return SteamUserStats.GetAchievement(achievementApiName, out achieved);
			}

			public static bool GetAchievement(string achievementApiName, out bool achieved, out DateTime unlockTime)
			{
				uint punUnlockTime;
				bool achievementAndUnlockTime = SteamUserStats.GetAchievementAndUnlockTime(achievementApiName, out achieved, out punUnlockTime);
				unlockTime = new DateTime(1970, 1, 1).AddSeconds(punUnlockTime);
				return achievementAndUnlockTime;
			}

			public static bool GetAchievement(UserData userId, string achievementApiName, out bool achieved)
			{
				return SteamUserStats.GetUserAchievement(userId, achievementApiName, out achieved);
			}

			public static bool GetAchievement(UserData userId, string achievementApiName, out bool achieved, out DateTime unlockTime)
			{
				uint punUnlockTime;
				bool userAchievementAndUnlockTime = SteamUserStats.GetUserAchievementAndUnlockTime(userId, achievementApiName, out achieved, out punUnlockTime);
				unlockTime = new DateTime(1970, 1, 1).AddSeconds(punUnlockTime);
				return userAchievementAndUnlockTime;
			}

			public static bool GetAchievementAchievedPercent(string achievementApiName, out float percent)
			{
				return SteamUserStats.GetAchievementAchievedPercent(achievementApiName, out percent);
			}

			public static string GetAchievementDisplayAttribute(string achievementApiName, string key)
			{
				return SteamUserStats.GetAchievementDisplayAttribute(achievementApiName, key);
			}

			public static string GetAchievementDisplayAttribute(string achievementApiName, AchievementAttributes attribute)
			{
				return SteamUserStats.GetAchievementDisplayAttribute(achievementApiName, attribute.ToString());
			}

			public static void GetAchievementIcon(string achievementApiName, Action<Texture2D> callback)
			{
				if (callback == null)
				{
					return;
				}
				if (m_UserAchievementIconFetched_t == null)
				{
					m_UserAchievementIconFetched_t = Callback<UserAchievementIconFetched_t>.Create(HandleIconImageLoaded);
				}
				int achievementIcon = SteamUserStats.GetAchievementIcon(achievementApiName);
				if (achievementIcon > 0)
				{
					if (loadedImages.ContainsKey(achievementIcon))
					{
						callback(loadedImages[achievementIcon]);
						return;
					}
					if (LoadImage(achievementIcon))
					{
						callback(loadedImages[achievementIcon]);
						return;
					}
					Debug.LogWarning("Failed to load the requested avatar");
					callback(null);
				}
				else
				{
					Debug.LogWarning("No avatar available for this user");
					pendingLinks.Add(new ImageRequestCallbackLink
					{
						isAchievement = true,
						apiName = achievementApiName,
						callback = callback
					});
				}
			}

			public static string GetAchievementName(uint index)
			{
				return SteamUserStats.GetAchievementName(index);
			}

			public static string[] GetAchievementNames()
			{
				uint numAchievements = SteamUserStats.GetNumAchievements();
				string[] array = new string[numAchievements];
				for (int i = 0; i < numAchievements; i++)
				{
					array[i] = SteamUserStats.GetAchievementName((uint)i);
				}
				return array;
			}

			public static bool GetGlobalStat(string statApiName, out long data)
			{
				return SteamUserStats.GetGlobalStat(statApiName, out data);
			}

			public static bool GetGlobalStat(string statApiName, out double data)
			{
				return SteamUserStats.GetGlobalStat(statApiName, out data);
			}

			public static void GetMostAchievedAchievements(Action<EResult, (AchievementObject achievement, float percentage)[], bool> callback)
			{
				if (SteamSettings.current == null)
				{
					Debug.LogError("GetMostAchievedAchievements only works when you have initalized a SteamSettings object");
					callback?.Invoke(EResult.k_EResultInvalidParam, null, arg3: true);
				}
				RequestGlobalAchievementPercentages(delegate(GlobalAchievementPercentagesReady_t result, bool error)
				{
					if (!error && result.m_eResult == EResult.k_EResultOK)
					{
						string achievementApiName;
						float percent;
						bool achieved;
						int num = GetMostAchievedAchievementInfo(out achievementApiName, out percent, out achieved);
						if (num > -1)
						{
							List<(AchievementObject, float)> list = new List<(AchievementObject, float)>();
							while (num != -1)
							{
								AchievementObject item = SteamSettings.Achievements.FirstOrDefault((AchievementObject p) => p.Id == achievementApiName);
								list.Add((item, percent));
								num = GetNextMostAchievedAchievementInfo(num, out achievementApiName, out percent, out achieved);
							}
							callback?.Invoke(result.m_eResult, list.ToArray(), error);
						}
						else
						{
							callback?.Invoke(result.m_eResult, null, error);
						}
					}
					else
					{
						callback?.Invoke(result.m_eResult, null, error);
					}
				});
			}

			public static void GetMostAchievedAchievements(Action<EResult, (AchievementData achievement, float percentage)[], bool> callback)
			{
				RequestGlobalAchievementPercentages(delegate(GlobalAchievementPercentagesReady_t result, bool error)
				{
					if (!error && result.m_eResult == EResult.k_EResultOK)
					{
						string achievementApiName;
						float percent;
						bool achieved;
						int num = GetMostAchievedAchievementInfo(out achievementApiName, out percent, out achieved);
						if (num > -1)
						{
							List<(AchievementData, float)> list = new List<(AchievementData, float)>();
							while (num != -1)
							{
								list.Add((achievementApiName, percent));
								num = GetNextMostAchievedAchievementInfo(num, out achievementApiName, out percent, out achieved);
							}
							callback?.Invoke(result.m_eResult, list.ToArray(), error);
						}
						else
						{
							callback?.Invoke(result.m_eResult, null, error);
						}
					}
					else
					{
						callback?.Invoke(result.m_eResult, null, error);
					}
				});
			}

			public static int GetMostAchievedAchievementInfo(out string achievementApiName, out float percent, out bool achieved)
			{
				return SteamUserStats.GetMostAchievedAchievementInfo(out achievementApiName, 8193u, out percent, out achieved);
			}

			public static int GetNextMostAchievedAchievementInfo(int previousIndex, out string achievementApiName, out float percent, out bool achieved)
			{
				return SteamUserStats.GetNextMostAchievedAchievementInfo(previousIndex, out achievementApiName, 8193u, out percent, out achieved);
			}

			public static uint GetNumAchievements()
			{
				return SteamUserStats.GetNumAchievements();
			}

			public static void GetNumberOfCurrentPlayers(Action<NumberOfCurrentPlayers_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_NumberOfCurrentPlayers_t == null)
					{
						m_NumberOfCurrentPlayers_t = CallResult<NumberOfCurrentPlayers_t>.Create();
					}
					SteamAPICall_t numberOfCurrentPlayers = SteamUserStats.GetNumberOfCurrentPlayers();
					m_NumberOfCurrentPlayers_t.Set(numberOfCurrentPlayers, callback.Invoke);
				}
			}

			public static bool GetStat(string statApiName, out int data)
			{
				return SteamUserStats.GetStat(statApiName, out data);
			}

			public static bool GetStat(string statApiName, out float data)
			{
				return SteamUserStats.GetStat(statApiName, out data);
			}

			public static bool GetStat(UserData userId, string statApiName, out int data)
			{
				return SteamUserStats.GetUserStat((CSteamID)userId, statApiName, out data);
			}

			public static bool GetStat(UserData userId, string statApiName, out float data)
			{
				return SteamUserStats.GetUserStat((CSteamID)userId, statApiName, out data);
			}

			public static bool IndicateAchievementProgress(string achievementApiName, uint progress, uint maxProgress)
			{
				return SteamUserStats.IndicateAchievementProgress(achievementApiName, progress, maxProgress);
			}

			public static void RequestGlobalAchievementPercentages(Action<GlobalAchievementPercentagesReady_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_GlobalAchievementPercentagesReady_t == null)
					{
						m_GlobalAchievementPercentagesReady_t = CallResult<GlobalAchievementPercentagesReady_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamUserStats.RequestGlobalAchievementPercentages();
					m_GlobalAchievementPercentagesReady_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static void RequestGlobalStats(int historyDays, Action<GlobalStatsReceived_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_GlobalStatsReceived_t == null)
					{
						m_GlobalStatsReceived_t = CallResult<GlobalStatsReceived_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamUserStats.RequestGlobalStats(historyDays);
					m_GlobalStatsReceived_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static void RequestUserStats(UserData userId, Action<UserStatsReceived, bool> callback)
			{
				if (callback != null)
				{
					if (m_UserStatsReceived_t2 == null)
					{
						m_UserStatsReceived_t2 = CallResult<UserStatsReceived_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamUserStats.RequestUserStats(userId);
					m_UserStatsReceived_t2.Set(hAPICall, delegate(UserStatsReceived_t r, bool e)
					{
						callback(r, e);
					});
				}
			}

			public static bool ResetAllStats(bool achievementsToo)
			{
				bool flag = SteamUserStats.ResetAllStats(achievementsToo);
				if (SteamSettings.current != null && flag && achievementsToo)
				{
					foreach (AchievementObject achievement in SteamSettings.Achievements)
					{
						achievement.StatusChanged?.Invoke(achievement.IsAchieved);
					}
				}
				return flag;
			}

			public static bool SetAchievement(string achievementApiName)
			{
				bool flag = SteamUserStats.SetAchievement(achievementApiName);
				if (SteamSettings.current != null && flag)
				{
					AchievementObject achievementObject = SteamSettings.Achievements.FirstOrDefault((AchievementObject p) => p.Id == achievementApiName);
					if (achievementObject != null)
					{
						achievementObject.StatusChanged?.Invoke(achievementObject.IsAchieved);
					}
				}
				return flag;
			}

			public static bool SetStat(string statApiName, int data)
			{
				return SteamUserStats.SetStat(statApiName, data);
			}

			public static bool SetStat(string statApiName, float data)
			{
				return SteamUserStats.SetStat(statApiName, data);
			}

			public static bool StoreStats()
			{
				return SteamUserStats.StoreStats();
			}

			public static bool UpdateAvgRateStat(string statApiName, float countThisSession, double sessionLength)
			{
				return SteamUserStats.UpdateAvgRateStat(statApiName, countThisSession, sessionLength);
			}

			private static void HandleIconImageLoaded(UserAchievementIconFetched_t param)
			{
				if (LoadImage(param.m_nIconHandle))
				{
					Texture2D obj = loadedImages[param.m_nIconHandle];
					string apiName = param.m_rgchAchievementName;
					foreach (ImageRequestCallbackLink pendingLink in pendingLinks)
					{
						if (pendingLink.isAchievement && pendingLink.apiName == apiName)
						{
							pendingLink.callback?.Invoke(obj);
						}
					}
					pendingLinks.RemoveAll((ImageRequestCallbackLink p) => p.isAchievement && p.apiName == apiName);
					return;
				}
				string apiName2 = param.m_rgchAchievementName;
				foreach (ImageRequestCallbackLink pendingLink2 in pendingLinks)
				{
					if (pendingLink2.isAchievement && pendingLink2.apiName == apiName2)
					{
						pendingLink2.callback?.Invoke(null);
					}
				}
				pendingLinks.RemoveAll((ImageRequestCallbackLink p) => p.isAchievement && p.apiName == apiName2);
			}

			private static bool LoadImage(int imageHandle)
			{
				if (SteamUtils.GetImageSize(imageHandle, out var pnWidth, out var pnHeight))
				{
					Texture2D texture2D = null;
					if (loadedImages.ContainsKey(imageHandle))
					{
						texture2D = loadedImages[imageHandle];
					}
					if (texture2D == null)
					{
						texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false);
					}
					else
					{
						UnityEngine.Object.Destroy(texture2D);
						texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false);
					}
					int num = (int)(pnWidth * pnHeight * 4);
					byte[] array = new byte[num];
					if (SteamUtils.GetImageRGBA(imageHandle, array, num))
					{
						texture2D.LoadRawTextureData(Utilities.FlipImageBufferVertical((int)pnWidth, (int)pnHeight, array));
						texture2D.Apply();
					}
					if (loadedImages.ContainsKey(imageHandle))
					{
						loadedImages[imageHandle] = texture2D;
					}
					else
					{
						loadedImages.Add(imageHandle, texture2D);
					}
					return true;
				}
				return false;
			}
		}

		public static class Server
		{
			private static CallResult<GSStatsReceived_t> m_GSStatsReceived_t;

			private static CallResult<GSStatsStored_t> m_GSStatsStored_t;

			public static bool ClearUserAchievement(CSteamID userId, string achievementApiName)
			{
				return SteamGameServerStats.ClearUserAchievement(userId, achievementApiName);
			}

			public static bool GetUserAchievement(CSteamID userId, string achievementApiName, out bool achieved)
			{
				return SteamGameServerStats.GetUserAchievement(userId, achievementApiName, out achieved);
			}

			public static bool GetUserStat(CSteamID userId, string statApiName, out int data)
			{
				return SteamGameServerStats.GetUserStat(userId, statApiName, out data);
			}

			public static bool GetUserStat(CSteamID userId, string statApiName, out float data)
			{
				return SteamGameServerStats.GetUserStat(userId, statApiName, out data);
			}

			public static void RequestUserStats(CSteamID userId, Action<GSStatsReceived_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_GSStatsReceived_t == null)
					{
						m_GSStatsReceived_t = CallResult<GSStatsReceived_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamGameServerStats.RequestUserStats(userId);
					m_GSStatsReceived_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static bool SetUserAchievement(CSteamID userId, string achievementApiName)
			{
				return SteamGameServerStats.SetUserAchievement(userId, achievementApiName);
			}

			public static bool SetUserStat(CSteamID userId, string statApiName, int data)
			{
				return SteamGameServerStats.SetUserStat(userId, statApiName, data);
			}

			public static bool SetUserStat(CSteamID userId, string statApiName, float data)
			{
				return SteamGameServerStats.SetUserStat(userId, statApiName, data);
			}

			public static void StoreUserStats(CSteamID userId, Action<GSStatsStored_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_GSStatsStored_t == null)
					{
						m_GSStatsStored_t = CallResult<GSStatsStored_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamGameServerStats.StoreUserStats(userId);
					m_GSStatsStored_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static bool UpdateUserAvgRateStat(CSteamID userId, string statApiName, float count, double length)
			{
				return SteamGameServerStats.UpdateUserAvgRateStat(userId, statApiName, count, length);
			}
		}
	}
}

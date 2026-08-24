using System;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.API
{
	public static class User
	{
		public static class Client
		{
			private static CallResult<StoreAuthURLResponse_t> m_StoreAuthURLResponse_t;

			public static UserData Id => SteamUser.GetSteamID();

			public static int Level => SteamUser.GetPlayerSteamLevel();

			public static StringKeyValuePair[] RichPresence
			{
				get
				{
					int friendRichPresenceKeyCount = SteamFriends.GetFriendRichPresenceKeyCount(SteamUser.GetSteamID());
					StringKeyValuePair[] array = new StringKeyValuePair[friendRichPresenceKeyCount];
					for (int i = 0; i < friendRichPresenceKeyCount; i++)
					{
						string friendRichPresenceKeyByIndex = SteamFriends.GetFriendRichPresenceKeyByIndex(SteamUser.GetSteamID(), i);
						string friendRichPresence = SteamFriends.GetFriendRichPresence(SteamUser.GetSteamID(), friendRichPresenceKeyByIndex);
						array[i] = new StringKeyValuePair
						{
							key = friendRichPresenceKeyByIndex,
							value = friendRichPresence
						};
					}
					return array;
				}
				set
				{
					for (int i = 0; i < value.Length; i++)
					{
						StringKeyValuePair stringKeyValuePair = value[i];
						SteamFriends.ClearRichPresence();
						SteamFriends.SetRichPresence(stringKeyValuePair.key, stringKeyValuePair.value);
					}
				}
			}

			public static bool IsBehindNAT => SteamUser.BIsBehindNAT();

			public static bool IsPhoneIdentifying => SteamUser.BIsPhoneIdentifying();

			public static bool IsPhoneRequiringVerification => SteamUser.BIsPhoneRequiringVerification();

			public static bool IsPhoneVerified => SteamUser.BIsPhoneVerified();

			public static bool IsTwoFactorEnabled => SteamUser.BIsTwoFactorEnabled();

			public static bool LoggedOn => SteamUser.BLoggedOn();

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Init()
			{
				m_StoreAuthURLResponse_t = null;
			}

			public static void AdvertiseGame(CSteamID gameServerId, uint ip, ushort port)
			{
				SteamUser.AdvertiseGame(gameServerId, ip, port);
			}

			public static void AdvertiseGame(CSteamID gameServerId, string ip, ushort port)
			{
				SteamUser.AdvertiseGame(gameServerId, Utilities.IPStringToUint(ip), port);
			}

			public static int GetGameBadgeLevel(int series, bool foil)
			{
				return SteamUser.GetGameBadgeLevel(series, foil);
			}

			public static void RequestStoreAuthURL(string redirectUrl, Action<StoreAuthURLResponse_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_StoreAuthURLResponse_t == null)
					{
						m_StoreAuthURLResponse_t = CallResult<StoreAuthURLResponse_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamUser.RequestStoreAuthURL(redirectUrl);
					m_StoreAuthURLResponse_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static bool SetRichPresence(string key, string value)
			{
				return SteamFriends.SetRichPresence(key, value);
			}

			public static void ClearRichPresence()
			{
				SteamFriends.ClearRichPresence();
			}

			public static string GetRichPresence(string key)
			{
				return SteamFriends.GetFriendRichPresence(SteamUser.GetSteamID(), key);
			}
		}
	}
}

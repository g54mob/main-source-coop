using Epic.OnlineServices;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.Leaderboards;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAudio;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;
using EvilCore.Networking;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

namespace EpicTransport
{
	[DefaultExecutionOrder(-32000)]
	public class EOSSDKComponent : MonoBehaviour
	{
		[SerializeField]
		private string displayName = "User";

		[SerializeField]
		private bool collectPlayerMetrics;

		private static EOSSDKComponent instance;

		private static EOSAuthManager authManager;

		public static string DisplayName
		{
			get
			{
				if (!(Instance != null))
				{
					return "User";
				}
				return Instance.displayName;
			}
			set
			{
				if (Instance != null)
				{
					Instance.displayName = value;
				}
			}
		}

		public static bool CollectPlayerMetrics
		{
			get
			{
				if (Instance != null)
				{
					return Instance.collectPlayerMetrics;
				}
				return false;
			}
		}

		private static EOSSDKComponent Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Object.FindObjectOfType<EOSSDKComponent>();
				}
				return instance;
			}
		}

		private static EOSAuthManager AuthManager
		{
			get
			{
				if (authManager == null)
				{
					authManager = Object.FindObjectOfType<EOSAuthManager>();
				}
				return authManager;
			}
		}

		public static bool Initialized
		{
			get
			{
				try
				{
					EOSAuthManager eOSAuthManager = AuthManager;
					if (eOSAuthManager != null && eOSAuthManager.IsLoggedIn)
					{
						return true;
					}
					return EOSManager.Instance?.HasLoggedInWithConnect() ?? false;
				}
				catch
				{
					return false;
				}
			}
		}

		public static bool IsConnecting
		{
			get
			{
				try
				{
					if (Initialized)
					{
						return false;
					}
					EOSAuthManager eOSAuthManager = AuthManager;
					if (eOSAuthManager != null)
					{
						return !eOSAuthManager.IsLoggedIn && GetPlatform() != null;
					}
					return GetPlatform() != null;
				}
				catch
				{
					return false;
				}
			}
		}

		public static ProductUserId LocalUserProductId
		{
			get
			{
				try
				{
					EOSAuthManager eOSAuthManager = AuthManager;
					if (eOSAuthManager != null && eOSAuthManager.LocalProductUserId != null)
					{
						return eOSAuthManager.LocalProductUserId;
					}
					return EOSManager.Instance?.GetProductUserId();
				}
				catch
				{
					return null;
				}
			}
		}

		public static string LocalUserProductIdString
		{
			get
			{
				try
				{
					return LocalUserProductId?.ToString();
				}
				catch
				{
					return null;
				}
			}
		}

		public static EpicAccountId LocalUserAccountId
		{
			get
			{
				try
				{
					return EOSManager.Instance?.GetLocalUserId();
				}
				catch
				{
					return null;
				}
			}
		}

		public static string LocalUserAccountIdString
		{
			get
			{
				try
				{
					return EOSManager.Instance?.GetLocalUserId()?.ToString();
				}
				catch
				{
					return null;
				}
			}
		}

		private static PlatformInterface GetPlatform()
		{
			try
			{
				return EOSManager.Instance?.GetEOSPlatformInterface();
			}
			catch
			{
				return null;
			}
		}

		public static AchievementsInterface GetAchievementsInterface()
		{
			return GetPlatform()?.GetAchievementsInterface();
		}

		public static AuthInterface GetAuthInterface()
		{
			return GetPlatform()?.GetAuthInterface();
		}

		public static ConnectInterface GetConnectInterface()
		{
			return GetPlatform()?.GetConnectInterface();
		}

		public static EcomInterface GetEcomInterface()
		{
			return GetPlatform()?.GetEcomInterface();
		}

		public static FriendsInterface GetFriendsInterface()
		{
			return GetPlatform()?.GetFriendsInterface();
		}

		public static LeaderboardsInterface GetLeaderboardsInterface()
		{
			return GetPlatform()?.GetLeaderboardsInterface();
		}

		public static LobbyInterface GetLobbyInterface()
		{
			return GetPlatform()?.GetLobbyInterface();
		}

		public static MetricsInterface GetMetricsInterface()
		{
			return GetPlatform()?.GetMetricsInterface();
		}

		public static ModsInterface GetModsInterface()
		{
			return GetPlatform()?.GetModsInterface();
		}

		public static P2PInterface GetP2PInterface()
		{
			return GetPlatform()?.GetP2PInterface();
		}

		public static PlayerDataStorageInterface GetPlayerDataStorageInterface()
		{
			return GetPlatform()?.GetPlayerDataStorageInterface();
		}

		public static PresenceInterface GetPresenceInterface()
		{
			return GetPlatform()?.GetPresenceInterface();
		}

		public static SessionsInterface GetSessionsInterface()
		{
			return GetPlatform()?.GetSessionsInterface();
		}

		public static TitleStorageInterface GetTitleStorageInterface()
		{
			return GetPlatform()?.GetTitleStorageInterface();
		}

		public static RTCInterface GetRTCInterface()
		{
			return GetPlatform()?.GetRTCInterface();
		}

		public static RTCAudioInterface GetRTCAudioInterface()
		{
			return GetRTCInterface()?.GetAudioInterface();
		}

		public static UIInterface GetUIInterface()
		{
			return GetPlatform()?.GetUIInterface();
		}

		public static UserInfoInterface GetUserInfoInterface()
		{
			return GetPlatform()?.GetUserInfoInterface();
		}

		public static void Tick()
		{
		}

		private void Awake()
		{
			if (instance != null && instance != this)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				instance = this;
			}
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}
	}
}

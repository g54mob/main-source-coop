using System;
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
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;

namespace Epic.OnlineServices.Platform
{
	public sealed class PlatformInterface : Handle
	{
		public static readonly Utf8String CHECKFORLAUNCHERANDRESTART_ENV_VAR = "EOS_LAUNCHED_BY_EPIC";

		public const int CLIENTCREDENTIALS_CLIENTID_MAX_LENGTH = 64;

		public const int CLIENTCREDENTIALS_CLIENTSECRET_MAX_LENGTH = 64;

		public const int COUNTRYCODE_MAX_BUFFER_LEN = 5;

		public const int COUNTRYCODE_MAX_LENGTH = 4;

		public const int GETDESKTOPCROSSPLAYSTATUS_API_LATEST = 1;

		public const int INITIALIZEOPTIONS_PRODUCTNAME_MAX_LENGTH = 64;

		public const int INITIALIZEOPTIONS_PRODUCTVERSION_MAX_LENGTH = 64;

		public const int INITIALIZE_API_LATEST = 5;

		public const int INITIALIZE_THREADAFFINITY_API_LATEST = 4;

		public const int LOCALECODE_MAX_BUFFER_LEN = 10;

		public const int LOCALECODE_MAX_LENGTH = 9;

		public const int OPTIONS_API_LATEST = 15;

		public const int OPTIONS_DEPLOYMENTID_MAX_LENGTH = 64;

		public const int OPTIONS_ENCRYPTIONKEY_LENGTH = 64;

		public const int OPTIONS_PRODUCTID_MAX_LENGTH = 64;

		public const int OPTIONS_SANDBOXID_MAX_LENGTH = 64;

		public const int RTCOPTIONS_API_LATEST = 3;

		public const int WINDOWS_RTCOPTIONS_API_LATEST = 1;

		public PlatformInterface()
		{
		}

		public PlatformInterface(IntPtr innerHandle)
			: base(innerHandle)
		{
		}

		public static Result Initialize(ref InitializeOptions options)
		{
			InitializeOptionsInternal options2 = default(InitializeOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_Initialize(ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public AchievementsInterface GetAchievementsInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetAchievementsInterface(base.InnerHandle), out AchievementsInterface to);
			return to;
		}

		public ApplicationStatus GetApplicationStatus()
		{
			return Bindings.EOS_Platform_GetApplicationStatus(base.InnerHandle);
		}

		public AuthInterface GetAuthInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetAuthInterface(base.InnerHandle), out AuthInterface to);
			return to;
		}

		public ConnectInterface GetConnectInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetConnectInterface(base.InnerHandle), out ConnectInterface to);
			return to;
		}

		public EcomInterface GetEcomInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetEcomInterface(base.InnerHandle), out EcomInterface to);
			return to;
		}

		public FriendsInterface GetFriendsInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetFriendsInterface(base.InnerHandle), out FriendsInterface to);
			return to;
		}

		public LeaderboardsInterface GetLeaderboardsInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetLeaderboardsInterface(base.InnerHandle), out LeaderboardsInterface to);
			return to;
		}

		public LobbyInterface GetLobbyInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetLobbyInterface(base.InnerHandle), out LobbyInterface to);
			return to;
		}

		public MetricsInterface GetMetricsInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetMetricsInterface(base.InnerHandle), out MetricsInterface to);
			return to;
		}

		public ModsInterface GetModsInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetModsInterface(base.InnerHandle), out ModsInterface to);
			return to;
		}

		public P2PInterface GetP2PInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetP2PInterface(base.InnerHandle), out P2PInterface to);
			return to;
		}

		public PlayerDataStorageInterface GetPlayerDataStorageInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetPlayerDataStorageInterface(base.InnerHandle), out PlayerDataStorageInterface to);
			return to;
		}

		public PresenceInterface GetPresenceInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetPresenceInterface(base.InnerHandle), out PresenceInterface to);
			return to;
		}

		public RTCInterface GetRTCInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetRTCInterface(base.InnerHandle), out RTCInterface to);
			return to;
		}

		public SessionsInterface GetSessionsInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetSessionsInterface(base.InnerHandle), out SessionsInterface to);
			return to;
		}

		public StatsInterface GetStatsInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetStatsInterface(base.InnerHandle), out StatsInterface to);
			return to;
		}

		public TitleStorageInterface GetTitleStorageInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetTitleStorageInterface(base.InnerHandle), out TitleStorageInterface to);
			return to;
		}

		public UIInterface GetUIInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetUIInterface(base.InnerHandle), out UIInterface to);
			return to;
		}

		public UserInfoInterface GetUserInfoInterface()
		{
			Helper.Get(Bindings.EOS_Platform_GetUserInfoInterface(base.InnerHandle), out UserInfoInterface to);
			return to;
		}

		public void Release()
		{
			Bindings.EOS_Platform_Release(base.InnerHandle);
		}

		public Result SetApplicationStatus(ApplicationStatus newStatus)
		{
			return Bindings.EOS_Platform_SetApplicationStatus(base.InnerHandle, newStatus);
		}

		public void Tick()
		{
			Bindings.EOS_Platform_Tick(base.InnerHandle);
		}

		public static Result Shutdown()
		{
			return Bindings.EOS_Shutdown();
		}

		public static PlatformInterface Create(ref WindowsOptions options)
		{
			WindowsOptionsInternal options2 = default(WindowsOptionsInternal);
			options2.Set(ref options);
			IntPtr intPtr = WindowsBindings.EOS_Platform_Create_Windows(ref options2);
			Helper.Dispose(ref options2);
			Helper.Get(intPtr, out PlatformInterface to);
			return to;
		}
	}
}

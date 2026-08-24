using System;
using System.Collections.Generic;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[HelpURL("https://kb.heathenengineering.com/assets/steamworks/objects/steam-settings")]
	[CreateAssetMenu(menuName = "Steamworks/Settings")]
	public class SteamSettings : ScriptableObject
	{
		public static class Colors
		{
			public static Color SteamBlue = new Color(0.2f, 0.6f, 0.93f, 1f);

			public static Color SteamGreen = new Color(0.2f, 0.42f, 0.2f, 1f);

			public static Color BrightGreen = new Color(0.4f, 0.84f, 0.4f, 1f);

			public static Color HalfAlpha = new Color(1f, 1f, 1f, 0.5f);

			public static Color ErrorRed = new Color(1f, 0.5f, 0.5f, 1f);
		}

		[Serializable]
		public class GameServer
		{
			public bool autoInitialize;

			public bool autoLogon;

			public uint ip;

			public ushort queryPort = 27016;

			public ushort gamePort = 27015;

			public string serverVersion = "1.0.0.0";

			public ushort spectatorPort = 27017;

			public bool usingGameServerAuthApi;

			public bool enableHeartbeats = true;

			public bool supportSpectators;

			public string spectatorServerName = "Usually GameDescription + Spectator";

			public bool anonymousServerLogin;

			public string gameServerToken = "See https://steamcommunity.com/dev/managegameservers";

			public bool isPasswordProtected;

			public string serverName = "My Server Name";

			public string gameDescription = "Usually the name of your game";

			public string gameDirectory = "e.g. its folder name";

			public bool isDedicated;

			public int maxPlayerCount = 4;

			public int botPlayerCount;

			public string mapName = "";

			[Tooltip("A delimited string used for Matchmaking Filtering e.g. CoolPeopleOnly,NoWagonsAllowed.\nThe above represents 2 data points matchmaking will then filter accordingly\n... see Heathen Game Server Browser for more informaiton.")]
			public string gameData;

			public List<StringKeyValuePair> rulePairs = new List<StringKeyValuePair>();

			public bool LoggedOn { get; private set; }

			public SteamGameServerConfiguration Configuration => new SteamGameServerConfiguration
			{
				autoInitialize = autoInitialize,
				anonymousServerLogin = anonymousServerLogin,
				autoLogon = autoLogon,
				botPlayerCount = botPlayerCount,
				enableHeartbeats = enableHeartbeats,
				gameData = gameData,
				gameDescription = gameDescription,
				gameDirectory = gameDirectory,
				gamePort = gamePort,
				gameServerToken = gameServerToken,
				ip = ip,
				isDedicated = isDedicated,
				isPasswordProtected = isPasswordProtected,
				mapName = mapName,
				maxPlayerCount = maxPlayerCount,
				queryPort = queryPort,
				rulePairs = rulePairs.ToArray(),
				serverName = serverName,
				serverVersion = serverVersion,
				spectatorPort = spectatorPort,
				spectatorServerName = spectatorServerName,
				supportSpectators = supportSpectators,
				usingGameServerAuthApi = usingGameServerAuthApi
			};

			public CSteamID ServerId => SteamGameServer.GetSteamID();

			public App.Server.DisconnectedEvent EventDisconnected => App.Server.eventDisconnected;

			public App.Server.ConnectedEvent EventConnected => App.Server.eventConnected;

			public App.Server.FailureEvent EventFailure => App.Server.eventFailure;
		}

		[Serializable]
		public class GameClient
		{
		}

		public static SteamSettings current;

		public static SteamworksBehaviour behaviour;

		public AppId_t applicationId = new AppId_t(0u);

		public int callbackTick_Milliseconds = 15;

		public bool isDebugging;

		public GameServer server = new GameServer();

		public GameClient client = new GameClient();

		public List<StatObject> stats = new List<StatObject>();

		public List<AchievementObject> achievements = new List<AchievementObject>();

		public static AppId_t ApplicationId
		{
			get
			{
				if (!(current != null))
				{
					return default(AppId_t);
				}
				return current.applicationId;
			}
		}

		public static bool HasInitializationError => App.HasInitializationError;

		public static string InitializationErrorMessage => App.InitializationErrorMessage;

		public static bool Initialized => App.Initialized;

		public static GameClient Client
		{
			get
			{
				if (!(current == null))
				{
					return current.client;
				}
				return null;
			}
		}

		public static GameServer Server
		{
			get
			{
				if (!(current == null))
				{
					return current.server;
				}
				return null;
			}
		}

		public static List<AchievementObject> Achievements
		{
			get
			{
				if (!(current == null))
				{
					return current.achievements;
				}
				return null;
			}
		}

		public static List<StatObject> Stats
		{
			get
			{
				if (!(current == null))
				{
					return current.stats;
				}
				return null;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RunTimeInit()
		{
			current = null;
			behaviour = null;
		}

		public static void Unload()
		{
			App.Unload();
		}

		public void CreateBehaviour(bool doNotDestroy = false, Action initializedCallback = null, Action<string> errorCallback = null)
		{
			if (!Initialized)
			{
				GameObject gameObject = new GameObject("Steamworks");
				gameObject.SetActive(value: false);
				if (doNotDestroy)
				{
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				SteamworksBehaviour steamworksBehaviour = gameObject.AddComponent<SteamworksBehaviour>();
				if (initializedCallback != null)
				{
					steamworksBehaviour.evtSteamInitialized.AddListener(initializedCallback.Invoke);
				}
				if (errorCallback != null)
				{
					steamworksBehaviour.evtSteamInitializationError.AddListener(errorCallback.Invoke);
				}
				steamworksBehaviour.settings = this;
				gameObject.SetActive(value: true);
			}
		}

		public static void CreateBehaviour(SteamSettings settings, bool doNotDestroy = false)
		{
			settings.CreateBehaviour(doNotDestroy);
		}

		public void Initialize()
		{
			if (!Initialized)
			{
				current = this;
				App.isDebugging = isDebugging;
				if (client == null)
				{
					Debug.LogError("Invalid SteamSettings object detected. the client object is null and will not initialize properly, aborting initialization.");
				}
				else
				{
					App.Client.Initialize(applicationId);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using AOT;
using HeathenEngineering.Events;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace HeathenEngineering.SteamworksIntegration.API
{
	public static class App
	{
		public static class Client
		{
			[Serializable]
			public class UnityEventServersDisconnected : UnityEvent<EResult>
			{
			}

			[Serializable]
			public class UnityEventServersConnectFailure : UnityEvent<SteamServerConnectFailure>
			{
			}

			private static DlcInstalledEvent eventDlcInstalled = new DlcInstalledEvent();

			private static UnityEvent eventNewUrlLaunchParameters = new UnityEvent();

			private static UnityEvent eventServersConnected = new UnityEvent();

			private static UnityEventServersDisconnected eventServersDisconnected = new UnityEventServersDisconnected();

			private static UnityEventServersConnectFailure eventServersConnectFailure = new UnityEventServersConnectFailure();

			private static CallResult<FileDetailsResult_t> m_FileDetailResult_t;

			private static Callback<DlcInstalled_t> m_DlcInstalled_t;

			private static Callback<NewUrlLaunchParameters_t> m_NewUrlLaunchParameters_t;

			private static Callback<SteamServerConnectFailure_t> m_SteamServerConnectFailure_t;

			private static Callback<SteamServersConnected_t> m_SteamServersConnected_t;

			private static Callback<SteamServersDisconnected_t> m_SteamServersDisconnected_t;

			public static bool LoggedOn
			{
				get
				{
					if (Initialized)
					{
						return SteamUser.BLoggedOn();
					}
					return false;
				}
			}

			public static DlcInstalledEvent EventDlcInstalled
			{
				get
				{
					if (m_DlcInstalled_t == null)
					{
						m_DlcInstalled_t = Callback<DlcInstalled_t>.Create(delegate(DlcInstalled_t e)
						{
							eventDlcInstalled.Invoke(e.m_nAppID);
						});
					}
					return eventDlcInstalled;
				}
			}

			public static UnityEvent EventNewUrlLaunchParameters
			{
				get
				{
					if (m_NewUrlLaunchParameters_t == null)
					{
						m_NewUrlLaunchParameters_t = Callback<NewUrlLaunchParameters_t>.Create(delegate
						{
							eventNewUrlLaunchParameters.Invoke();
						});
					}
					return eventNewUrlLaunchParameters;
				}
			}

			public static UnityEvent EventServersConnected
			{
				get
				{
					if (m_SteamServersConnected_t == null)
					{
						m_SteamServersConnected_t = Callback<SteamServersConnected_t>.Create(delegate
						{
							eventServersConnected?.Invoke();
						});
					}
					return eventServersConnected;
				}
			}

			public static UnityEventServersDisconnected EventServersDisconnected
			{
				get
				{
					if (m_SteamServersConnected_t == null)
					{
						m_SteamServersDisconnected_t = Callback<SteamServersDisconnected_t>.Create(delegate(SteamServersDisconnected_t connected)
						{
							eventServersDisconnected?.Invoke(connected.m_eResult);
						});
					}
					return eventServersDisconnected;
				}
			}

			public static UnityEventServersConnectFailure EventServersConnectFailure
			{
				get
				{
					if (m_SteamServerConnectFailure_t == null)
					{
						m_SteamServerConnectFailure_t = Callback<SteamServerConnectFailure_t>.Create(delegate(SteamServerConnectFailure_t connected)
						{
							eventServersConnectFailure?.Invoke(connected);
						});
					}
					return eventServersConnectFailure;
				}
			}

			public static bool IsSubscribed => SteamApps.BIsSubscribed();

			public static bool IsSubscribedFromFamilySharing => SteamApps.BIsSubscribedFromFamilySharing();

			public static bool IsSubscribedFromFreeWeekend => SteamApps.BIsSubscribedFromFreeWeekend();

			public static bool IsVACBanned => SteamApps.BIsVACBanned();

			public static UserData Owner => SteamApps.GetAppOwner();

			public static string[] AvailableLanguages => SteamApps.GetAvailableGameLanguages().Split(',');

			public static bool IsBeta
			{
				get
				{
					string pchName;
					return SteamApps.GetCurrentBetaName(out pchName, 128);
				}
			}

			public static string CurrentBetaName
			{
				get
				{
					if (SteamApps.GetCurrentBetaName(out var pchName, 512))
					{
						return pchName;
					}
					return string.Empty;
				}
			}

			public static string CurrentGameLanguage => SteamApps.GetCurrentGameLanguage();

			public static DlcData[] Dlc
			{
				get
				{
					int dLCCount = SteamApps.GetDLCCount();
					if (dLCCount > 0)
					{
						DlcData[] array = new DlcData[dLCCount];
						for (int i = 0; i < dLCCount; i++)
						{
							if (SteamApps.BGetDLCDataByIndex(i, out var pAppID, out var pbAvailable, out var pchName, 512))
							{
								array[i] = new DlcData(pAppID, pbAvailable, pchName);
							}
							else
							{
								Debug.LogWarning("Failed to fetch DLC at index [" + i + "]");
							}
						}
						return array;
					}
					return new DlcData[0];
				}
			}

			public static bool IsCybercafe => SteamApps.BIsCybercafe();

			public static bool IsLowViolence => SteamApps.BIsLowViolence();

			public static AppId_t Id => SteamUtils.GetAppID();

			public static int BuildId => SteamApps.GetAppBuildId();

			public static string InstallDirectory
			{
				get
				{
					SteamApps.GetAppInstallDir(SteamUtils.GetAppID(), out var pchFolder, 2048u);
					return pchFolder;
				}
			}

			public static int DLCCount => SteamApps.GetDLCCount();

			public static string LaunchCommandLine
			{
				get
				{
					if (SteamApps.GetLaunchCommandLine(out var pszCommandLine, 512) > 0)
					{
						return pszCommandLine;
					}
					return string.Empty;
				}
			}

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Init()
			{
				m_FileDetailResult_t = null;
				m_DlcInstalled_t = null;
				m_NewUrlLaunchParameters_t = null;
				eventDlcInstalled = new DlcInstalledEvent();
				eventNewUrlLaunchParameters = new UnityEvent();
				eventServersConnected = new UnityEvent();
				eventServersDisconnected = new UnityEventServersDisconnected();
			}

			public static void Initialize(AppData appId)
			{
				if (Initialized)
				{
					HasInitializationError = true;
					InitializationErrorMessage = "Tried to initialize the Steamworks API twice in one session, operation aborted!";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogWarning(InitializationErrorMessage);
					return;
				}
				if (!Packsize.Test())
				{
					HasInitializationError = true;
					InitializationErrorMessage = "Packesize Test returned false, the wrong version of the Steamowrks.NET is being run in this platform.";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogError(InitializationErrorMessage);
					return;
				}
				if (!DllCheck.Test())
				{
					HasInitializationError = true;
					InitializationErrorMessage = "DLL Check Test returned false, one or more of the Steamworks binaries seems to be the wrong version.";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogError(InitializationErrorMessage);
					return;
				}
				if (SteamAPI.RestartAppIfNecessary(appId))
				{
					Application.Quit();
				}
				else
				{
					if (isDebugging)
					{
						Debug.Log("Initializing Steam Client API");
					}
					string OutSteamErrMsg;
					ESteamAPIInitResult eSteamAPIInitResult = SteamAPI.InitEx(out OutSteamErrMsg);
					Initialized = eSteamAPIInitResult == ESteamAPIInitResult.k_ESteamAPIInitResult_OK;
					if (!Initialized)
					{
						HasInitializationError = true;
						InitializationErrorMessage = $"Steamworks failed to initialize with result({eSteamAPIInitResult}) and message: {OutSteamErrMsg}";
						evtSteamInitializationError.Invoke(InitializationErrorMessage);
						Debug.LogError(InitializationErrorMessage);
					}
					else
					{
						Friends.Client.RequestUserInformation(UserData.Me, nameOnly: false);
						Debug.Log($"Local User: {UserData.Me.Name}:{UserData.Me.Level}");
						if (m_SteamAPIWarningMessageHook == null)
						{
							m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
							SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
						}
						if (!SteamUser.BLoggedOn())
						{
							Debug.LogWarning("Steam API was able to initialize however the user does not have an active logon; no real-time services provided by the Steamworks API will be enabled. The Steam client will automatically be trying to recreate the connection as often as possible. When the connection is restored a API.App.Client.EvenServersConnected event will be posted.");
						}
						if (isDebugging)
						{
							Debug.Log("Steam API has been initialized with App ID: " + SteamUtils.GetAppID().ToString());
						}
						if (appId != SteamUtils.GetAppID())
						{
							Debug.LogError($"The reported AppId is not as expected:\ntAppId Reported = {SteamUtils.GetAppID()}\n\tAppId Expected = {appId}");
							Application.Quit();
						}
					}
				}
				Application.quitting += Application_quitting;
				if (Initialized)
				{
					if (callbackWaitThread == null)
					{
						callbackWaitThread = new BackgroundWorker();
						callbackWaitThread.WorkerSupportsCancellation = true;
						callbackWaitThread.WorkerReportsProgress = true;
						callbackWaitThread.DoWork += delegate
						{
							while (true)
							{
								Thread.Sleep(callbackTick_Milliseconds);
								callbackWaitThread.ReportProgress(1);
							}
						};
						callbackWaitThread.RunWorkerCompleted += CallbackWaitThread_RunWorkerCompleted;
						callbackWaitThread.ProgressChanged += CallbackWaitThread_ProgressChanged;
					}
					callbackWaitThread.RunWorkerAsync();
					Web.LoadAppNames(null);
					Overlay.Client.RegisterEvents();
					evtSteamInitialized.Invoke();
				}
				else
				{
					HasInitializationError = true;
					InitializationErrorMessage = "Steam Initialization failed, check the log for more information.";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogError("[Steamworks.NET] Steam Initialization failed, check the log for more information");
				}
			}

			public static bool IsAppInstalled(AppData appId)
			{
				return SteamApps.BIsAppInstalled(appId);
			}

			public static bool IsDlcInstalled(AppData appId)
			{
				return SteamApps.BIsDlcInstalled(appId);
			}

			public static bool GetDlcDownloadProgress(AppData appId, out ulong bytesDownloaded, out ulong bytesTotal)
			{
				return SteamApps.GetDlcDownloadProgress(appId, out bytesDownloaded, out bytesTotal);
			}

			public static string GetAppInstallDirectory(AppData appId)
			{
				SteamApps.GetAppInstallDir(appId, out var pchFolder, 2048u);
				return pchFolder;
			}

			public static DepotId_t[] InstalledDepots(AppData appId)
			{
				DepotId_t[] array = new DepotId_t[256];
				uint installedDepots = SteamApps.GetInstalledDepots(appId, array, 256u);
				Array.Resize(ref array, (int)installedDepots);
				return array;
			}

			public static string QueryLaunchParam(string key)
			{
				return SteamApps.GetLaunchQueryParam(key);
			}

			public static void InstallDLC(AppData appId)
			{
				SteamApps.InstallDLC(appId);
			}

			public static void UninstallDLC(AppData appId)
			{
				SteamApps.UninstallDLC(appId);
			}

			public static bool IsSubscribedApp(AppData appId)
			{
				return SteamApps.BIsSubscribedApp(appId);
			}

			public static bool IsTimedTrial(out uint secondsAllowed, out uint secondsPlayed)
			{
				return SteamApps.BIsTimedTrial(out secondsAllowed, out secondsPlayed);
			}

			public static bool GetCurrentBetaName(out string name)
			{
				return SteamApps.GetCurrentBetaName(out name, 512);
			}

			public static DateTime GetEarliestPurchaseTime(AppData appId)
			{
				uint earliestPurchaseUnixTime = SteamApps.GetEarliestPurchaseUnixTime(appId);
				return new DateTime(1970, 1, 1).AddSeconds(earliestPurchaseUnixTime);
			}

			public static void GetFileDetails(string name, Action<FileDetailsResult, bool> callback)
			{
				if (callback != null)
				{
					if (m_FileDetailResult_t == null)
					{
						m_FileDetailResult_t = CallResult<FileDetailsResult_t>.Create();
					}
					SteamAPICall_t fileDetails = SteamApps.GetFileDetails(name);
					m_FileDetailResult_t.Set(fileDetails, delegate(FileDetailsResult_t r, bool e)
					{
						callback(r, e);
					});
				}
			}

			public static bool MarkContentCorrupt(bool missingFilesOnly)
			{
				return SteamApps.MarkContentCorrupt(missingFilesOnly);
			}
		}

		public static class Server
		{
			[Serializable]
			public class DisconnectedEvent : UnityEvent<SteamServersDisconnected>
			{
			}

			[Serializable]
			public class ConnectedEvent : UnityEvent<SteamServersConnected_t>
			{
			}

			[Serializable]
			public class FailureEvent : UnityEvent<SteamServerConnectFailure>
			{
			}

			public static DisconnectedEvent eventDisconnected = new DisconnectedEvent();

			public static ConnectedEvent eventConnected = new ConnectedEvent();

			public static FailureEvent eventFailure = new FailureEvent();

			internal static Callback<SteamServerConnectFailure_t> steamServerConnectFailure;

			internal static Callback<SteamServersConnected_t> steamServersConnected;

			internal static Callback<SteamServersDisconnected_t> steamServersDisconnected;

			public static CSteamID ID => SteamGameServer.GetSteamID();

			public static bool LoggedOn { get; private set; }

			public static SteamGameServerConfiguration Configuration { get; set; }

			private static void OnSteamServersDisconnected(SteamServersDisconnected_t param)
			{
				LoggedOn = false;
				if (isDebugging)
				{
					Debug.LogError("Steamworks.GameServer reported connection Closed: " + param.m_eResult);
				}
				eventDisconnected.Invoke(param);
			}

			private static void OnSteamServersConnected(SteamServersConnected_t param)
			{
				LoggedOn = true;
				if (isDebugging)
				{
					Debug.Log("Game Server connected to Steamworks successfully!\n\tMod Directory = " + Configuration.gameDirectory + $"\n\tApplication ID = {SteamGameServerUtils.GetAppID()}" + $"\n\tServer ID = {SteamGameServer.GetSteamID()}" + "\n\tServer Name = " + Configuration.serverName + "\n\tGame Description = " + Configuration.gameDescription + $"\n\tMax Player Count = {Configuration.maxPlayerCount}");
				}
				SendUpdatedServerDetailsToSteam();
				eventConnected.Invoke(param);
			}

			private static void OnSteamServerConnectFailure(SteamServerConnectFailure_t param)
			{
				LoggedOn = false;
				if (isDebugging)
				{
					Debug.LogError("Steamworks.GameServer.LogOn reported connection Failure: " + param.m_eResult);
				}
				eventFailure.Invoke(param);
			}

			public static void Initialize(AppData appId, SteamGameServerConfiguration serverConfiguration)
			{
				if (Initialized)
				{
					HasInitializationError = true;
					InitializationErrorMessage = "Tried to initialize the Steamworks API twice in one session, operation aborted!";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogWarning(InitializationErrorMessage);
					return;
				}
				if (!Packsize.Test())
				{
					HasInitializationError = true;
					InitializationErrorMessage = "Packesize Test returned false, the wrong version of the Steamowrks.NET is being run in this platform.";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogError(InitializationErrorMessage);
					return;
				}
				if (!DllCheck.Test())
				{
					HasInitializationError = true;
					InitializationErrorMessage = "DLL Check Test returned false, one or more of the Steamworks binaries seems to be the wrong version.";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogError(InitializationErrorMessage);
					return;
				}
				Configuration = serverConfiguration;
				if (isDebugging)
				{
					Debug.Log("Registering Steam Game Server callbacks.");
				}
				RegisterCallbacks();
				EServerMode eServerMode = EServerMode.eServerModeNoAuthentication;
				if (serverConfiguration.usingGameServerAuthApi)
				{
					eServerMode = EServerMode.eServerModeAuthenticationAndSecure;
				}
				if (isDebugging)
				{
					Debug.Log("Initializing Steam Game Server API: (" + serverConfiguration.ip + ", " + serverConfiguration.gamePort + ", " + serverConfiguration.queryPort + ", " + eServerMode.ToString() + ", " + serverConfiguration.serverVersion + ")");
				}
				Initialized = GameServer.Init(serverConfiguration.ip, serverConfiguration.gamePort, serverConfiguration.queryPort, eServerMode, serverConfiguration.serverVersion);
				if (!Initialized)
				{
					HasInitializationError = true;
					InitializationErrorMessage = "Steam API failed to initialize!\nOne of the following issues must be true:\n- The Steam couldn't determine the App ID of the game. If you're running your server from the executable or debugger directly then you must have a steam_appid.txt in your server directory next to the executable, with your app ID in it and nothing else. Steam will look for this file in the current working directory. If you are running your executable from a different directory you may need to relocate the steam_appid.txt file.\n- The Game port and or Query port could not be bound.\n- The App ID is not completely set up, i.e. in Release State: Unavailable, or it's missing default packages.";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogError(InitializationErrorMessage);
				}
				else
				{
					if (isDebugging && Configuration.DebugValidate())
					{
						Debug.Log("Applying Steam Game Server Settings:\n\tSetModDir: " + serverConfiguration.gameDirectory + $"\n\tSetProduct: {appId}" + "\n\tSetGameDescription: " + serverConfiguration.gameDescription + $"\n\tSetMaxPlayerCount: {serverConfiguration.maxPlayerCount}" + $"\n\tSetPasswordProtected: {serverConfiguration.isPasswordProtected}" + "\n\tSetServerName: " + serverConfiguration.serverName + $"\n\tSetBotPlayerCount: {serverConfiguration.botPlayerCount}" + "\n\tSetMapName: " + serverConfiguration.mapName + $"\n\tSetDedicatedServer: {serverConfiguration.isDedicated}");
					}
					SteamGameServer.SetModDir(serverConfiguration.gameDirectory);
					SteamGameServer.SetProduct(appId.ToString());
					SteamGameServer.SetGameDescription(serverConfiguration.gameDescription);
					SteamGameServer.SetMaxPlayerCount(serverConfiguration.maxPlayerCount);
					SteamGameServer.SetPasswordProtected(serverConfiguration.isPasswordProtected);
					SteamGameServer.SetServerName(serverConfiguration.serverName);
					SteamGameServer.SetBotPlayerCount(serverConfiguration.botPlayerCount);
					SteamGameServer.SetMapName(serverConfiguration.mapName);
					SteamGameServer.SetDedicatedServer(serverConfiguration.isDedicated);
					if (serverConfiguration.supportSpectators)
					{
						if (isDebugging)
						{
							Debug.Log("Spectator enabled:\n\tName = " + serverConfiguration.spectatorServerName + "\n\tSpectator Port = " + serverConfiguration.spectatorPort);
						}
						SteamGameServer.SetSpectatorPort(serverConfiguration.spectatorPort);
						SteamGameServer.SetSpectatorServerName(serverConfiguration.spectatorServerName);
					}
					else if (isDebugging)
					{
						Debug.Log("Spectator Set Up Skipped");
					}
					if (isDebugging)
					{
						Debug.Log("Steam API has been initialized with App ID: " + SteamGameServerUtils.GetAppID().ToString());
					}
					if (appId != SteamGameServerUtils.GetAppID())
					{
						Debug.LogError($"The reported AppId is not as expected:\nAppId Reported = {SteamGameServerUtils.GetAppID()}, AppId Expected = {appId}");
					}
				}
				Application.quitting += Application_quitting;
				if (Initialized)
				{
					if (callbackWaitThread == null)
					{
						callbackWaitThread = new BackgroundWorker();
						callbackWaitThread.WorkerSupportsCancellation = true;
						callbackWaitThread.WorkerReportsProgress = true;
						callbackWaitThread.DoWork += delegate
						{
							while (true)
							{
								Thread.Sleep(callbackTick_Milliseconds);
								callbackWaitThread.ReportProgress(1);
							}
						};
						callbackWaitThread.RunWorkerCompleted += CallbackWaitThread_RunWorkerCompleted;
						callbackWaitThread.ProgressChanged += CallbackWaitThread_ProgressChanged;
					}
					callbackWaitThread.RunWorkerAsync();
					evtSteamInitialized.Invoke();
					if (Configuration.autoLogon)
					{
						LogOn();
					}
				}
				else
				{
					HasInitializationError = true;
					InitializationErrorMessage = "Steam Initialization failed, check the log for more information.";
					evtSteamInitializationError.Invoke(InitializationErrorMessage);
					Debug.LogError("[Steamworks.NET] Steam Initialization failed, check the log for more information");
				}
			}

			public static void LogOn()
			{
				if (Configuration.anonymousServerLogin)
				{
					if (isDebugging)
					{
						Debug.Log("Logging on with Anonymous");
					}
					SteamGameServer.LogOnAnonymous();
				}
				else
				{
					if (isDebugging)
					{
						Debug.Log("Logging on with token");
					}
					SteamGameServer.LogOn(Configuration.gameServerToken);
				}
				if (Configuration.usingGameServerAuthApi || Configuration.enableHeartbeats)
				{
					if (isDebugging)
					{
						Debug.Log("Enabling server heartbeat.");
					}
					SteamGameServer.SetAdvertiseServerActive(bActive: true);
				}
				Debug.Log("Steamworks Game Server Started.\nWaiting for connection result from Steamworks");
			}

			public static void SendUpdatedServerDetailsToSteam()
			{
				if (Configuration.rulePairs != null && Configuration.rulePairs.Length != 0)
				{
					string text = "Set the following rules:\n";
					StringKeyValuePair[] rulePairs = Configuration.rulePairs;
					for (int i = 0; i < rulePairs.Length; i++)
					{
						StringKeyValuePair stringKeyValuePair = rulePairs[i];
						SteamGameServer.SetKeyValue(stringKeyValuePair.key, stringKeyValuePair.value);
						text = text + "\n\t[" + stringKeyValuePair.key + "] = [" + stringKeyValuePair.value + "]";
					}
					if (isDebugging)
					{
						Debug.Log(text);
					}
				}
			}

			public static void RegisterCallbacks()
			{
				steamServerConnectFailure = Callback<SteamServerConnectFailure_t>.CreateGameServer(OnSteamServerConnectFailure);
				steamServersConnected = Callback<SteamServersConnected_t>.CreateGameServer(OnSteamServersConnected);
				steamServersDisconnected = Callback<SteamServersDisconnected_t>.CreateGameServer(OnSteamServersDisconnected);
			}
		}

		public static class Web
		{
			[Serializable]
			private struct SteamAppsListAPI
			{
				[Serializable]
				public struct Model
				{
					[Serializable]
					public struct AppData
					{
						public ulong appid;

						public string name;
					}

					public AppData[] apps;
				}

				public Model applist;

				public static UnityWebRequest GetRequest()
				{
					return UnityWebRequest.Get("https://api.steampowered.com/ISteamApps/GetAppList/v2/?");
				}
			}

			[Serializable]
			public struct SteamAppNews
			{
				[Serializable]
				public struct SteamNewsItem
				{
					public ulong gid;

					public string title;

					public string url;

					public bool is_external_url;

					public string author;

					public string contents;

					public string feedlabel;

					public long date;

					public string feedname;

					public uint feed_type;

					public uint appid;

					public DateTime Date => new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(date);
				}

				public uint appid;

				public SteamNewsItem[] newsitems;

				public uint count;
			}

			private static bool appListLoading = false;

			private static bool appsListLoaded = false;

			private static SteamAppsListAPI appsListApi;

			private static BackgroundWorker appListWorker;

			private static BackgroundWorker getNewsForApp;

			private static List<Action> waitingForAppListLoad = new List<Action>();

			public static bool IsAppsListLoaded => appsListLoaded;

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Init()
			{
				appsListApi = default(SteamAppsListAPI);
				appsListLoaded = false;
				if (appListWorker != null)
				{
					if (appListWorker.IsBusy)
					{
						appListWorker.CancelAsync();
					}
					appListWorker.Dispose();
					appListWorker = null;
				}
				if (getNewsForApp != null)
				{
					if (getNewsForApp.IsBusy)
					{
						getNewsForApp.CancelAsync();
					}
					getNewsForApp.Dispose();
					getNewsForApp = null;
				}
				waitingForAppListLoad = new List<Action>();
			}

			public static void LoadAppNames(Action callback)
			{
				if (appsListLoaded)
				{
					callback?.Invoke();
				}
				else if (!appListLoading)
				{
					appListLoading = true;
					waitingForAppListLoad.Add(callback);
					UnityWebRequest www = SteamAppsListAPI.GetRequest();
					www.SendWebRequest().completed += delegate
					{
						if (www.result == UnityWebRequest.Result.Success)
						{
							try
							{
								appsListApi = JsonUtility.FromJson<SteamAppsListAPI>(www.downloadHandler.text);
								appsListLoaded = true;
							}
							catch (Exception ex)
							{
								Debug.LogError("Failed to load the Steam App List: Exception = " + ex.Message);
							}
						}
						else
						{
							Debug.LogError($"Failed to load the Steam App List: State = {www.result}, Error Message = {www.error}");
						}
						foreach (Action item in waitingForAppListLoad)
						{
							item?.Invoke();
						}
						appListLoading = false;
						waitingForAppListLoad.Clear();
					};
				}
				else
				{
					waitingForAppListLoad.Add(callback);
				}
			}

			public static bool GetAppName(AppData appId, out string name)
			{
				if (appsListApi.applist.apps != null && appsListApi.applist.apps.Length != 0)
				{
					SteamAppsListAPI.Model.AppData appData = appsListApi.applist.apps.FirstOrDefault((SteamAppsListAPI.Model.AppData p) => p.appid == appId);
					if (appData.appid == appId)
					{
						name = appData.name;
						return true;
					}
					name = "Unknown";
					return false;
				}
				name = "Unknown";
				return false;
			}

			public static void GetAppName(AppData appId, Action<string, bool> callback)
			{
				string name;
				if (!appsListLoaded)
				{
					LoadAppNames(delegate
					{
						if (GetAppName(appId, out var name2))
						{
							callback?.Invoke(name2, arg2: false);
						}
						else
						{
							callback?.Invoke("Unkown", arg2: true);
						}
					});
				}
				else if (GetAppName(appId, out name))
				{
					callback?.Invoke(name, arg2: false);
				}
				else
				{
					callback?.Invoke("Unkown", arg2: true);
				}
			}

			public static void GetNewsForApp(AppData appId, uint count, string feeds, string tags, Action<SteamAppNews, bool> callback)
			{
				string text = "https://api.steampowered.com/ISteamNews/GetNewsForApp/v2/?appid=" + appId;
				if (count != 0)
				{
					text = text + "&count=" + count;
				}
				if (!string.IsNullOrEmpty(feeds))
				{
					text = text + "&feeds=" + feeds.ToString();
				}
				if (!string.IsNullOrEmpty(tags))
				{
					text = text + "&tags=" + tags.ToString();
				}
				UnityWebRequest www = new UnityWebRequest(text);
				www.SendWebRequest().completed += delegate
				{
					if (www.result == UnityWebRequest.Result.Success)
					{
						try
						{
							string text2 = www.downloadHandler.text;
							callback?.Invoke(JsonUtility.FromJson<SteamAppNews>(text2), arg2: false);
							return;
						}
						catch (Exception)
						{
							callback?.Invoke(default(SteamAppNews), arg2: true);
							return;
						}
					}
					callback?.Invoke(default(SteamAppNews), arg2: true);
				};
			}
		}

		internal static readonly Dictionary<uint, (string name, bool available)> dlcAppCash = new Dictionary<uint, (string, bool)>();

		public static int callbackTick_Milliseconds = 15;

		public static bool isDebugging = false;

		public static UnityEvent evtSteamInitialized = new UnityEvent();

		public static UnityStringEvent evtSteamInitializationError = new UnityStringEvent();

		private static BackgroundWorker callbackWaitThread = null;

		private static SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

		public static bool Initialized { get; private set; }

		public static bool HasInitializationError { get; private set; }

		public static string InitializationErrorMessage { get; private set; }

		public static AppData Id => SteamUtils.GetAppID();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RunTimeInit()
		{
			dlcAppCash.Clear();
			evtSteamInitialized = new UnityEvent();
			evtSteamInitializationError = new UnityStringEvent();
			m_SteamAPIWarningMessageHook = null;
			Server.eventDisconnected = new Server.DisconnectedEvent();
			Server.eventConnected = new Server.ConnectedEvent();
			Server.eventFailure = new Server.FailureEvent();
			Server.steamServerConnectFailure = null;
			Server.steamServersConnected = null;
			Server.steamServersDisconnected = null;
			Server.Configuration = default(SteamGameServerConfiguration);
			if (callbackWaitThread != null)
			{
				if (callbackWaitThread.IsBusy)
				{
					callbackWaitThread.RunWorkerCompleted -= CallbackWaitThread_RunWorkerCompleted;
					callbackWaitThread.ProgressChanged -= CallbackWaitThread_ProgressChanged;
					callbackWaitThread.CancelAsync();
					callbackWaitThread.Dispose();
				}
				callbackWaitThread = null;
			}
		}

		private static void Application_quitting()
		{
			if (!Initialized)
			{
				return;
			}
			if (callbackWaitThread != null)
			{
				if (callbackWaitThread.IsBusy)
				{
					callbackWaitThread.RunWorkerCompleted -= CallbackWaitThread_RunWorkerCompleted;
					callbackWaitThread.ProgressChanged -= CallbackWaitThread_ProgressChanged;
					callbackWaitThread.CancelAsync();
				}
				callbackWaitThread.Dispose();
				callbackWaitThread = null;
			}
			SteamAPI.Shutdown();
			Unload();
		}

		public static void Unload()
		{
			Initialized = false;
			HasInitializationError = false;
			InitializationErrorMessage = string.Empty;
			Friends.Client.UnloadAvatarImages();
		}

		private static void CallbackWaitThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
		}

		private static void CallbackWaitThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			SteamAPI.RunCallbacks();
		}

		[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Debug.LogWarning(pchDebugText);
		}
	}
}

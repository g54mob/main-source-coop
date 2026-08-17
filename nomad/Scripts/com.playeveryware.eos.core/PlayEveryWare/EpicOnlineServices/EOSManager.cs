using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Epic.OnlineServices;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.Leaderboards;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;
using PlayEveryWare.Common;
using PlayEveryWare.EpicOnlineServices.Utility;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public class EOSManager : MonoBehaviour, IEOSCoroutineOwner
	{
		public delegate void OnAuthLoginCallback(Epic.OnlineServices.Auth.LoginCallbackInfo loginCallbackInfo);

		public delegate void OnAuthLogoutCallback(LogoutCallbackInfo data);

		public delegate void OnConnectLoginCallback(Epic.OnlineServices.Connect.LoginCallbackInfo loginCallbackInfo);

		public delegate void OnCreateConnectUserCallback(CreateUserCallbackInfo createUserCallbackInfo);

		public delegate void OnConnectLinkExternalAccountCallback(Epic.OnlineServices.Connect.LinkAccountCallbackInfo linkAccountCallbackInfo);

		public delegate void OnAuthLinkExternalAccountCallback(Epic.OnlineServices.Auth.LinkAccountCallbackInfo linkAccountCallbackInfo);

		private enum EOSState
		{
			NotStarted = 0,
			Starting = 1,
			Running = 2,
			Suspending = 3,
			Suspended = 4,
			ShuttingDown = 5,
			Shutdown = 6
		}

		public class EOSSingleton
		{
			public struct EpicLauncherArgs
			{
				public string authLogin;

				public string authPassword;

				public string authType;

				public string epicApp;

				public string epicEnv;

				public string epicUsername;

				public string epicUserID;

				public string epicLocale;

				public string epicSandboxID;

				public string epicDeploymentID;
			}

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private delegate void PrintDelegateType(string str);

			private const float NetworkStatusUpdateIntervalSecs = 0.5f;

			private static float s_nextNetworkStatusUpdateTime = 0f;

			private static EpicAccountId s_localUserId;

			private static ProductUserId s_localProductUserId;

			private static NotifyEventHandle s_notifyLoginStatusChangedCallbackHandle;

			private static NotifyEventHandle s_notifyConnectLoginStatusChangedCallbackHandle;

			private static NotifyEventHandle s_notifyConnectAuthExpirationCallbackHandle;

			private static bool hasSetLoggingCallback;

			private static bool s_hasInitializedPlatform;

			private static readonly bool s_eosUnloadSDKOnShutdown = true;

			private static PlatformInterface s_eosPlatformInterface;

			public const string EOSBinaryName = "EOSSDK-Win64-Shipping";

			public const string GfxPluginNativeRenderPath = "GfxPluginNativeRender-x64";

			private static Dictionary<string, DLLHandle> LoadedDLLs = new Dictionary<string, DLLHandle>();

			protected void SetLocalUserId(EpicAccountId localUserId)
			{
				s_localUserId = localUserId;
			}

			public EpicAccountId GetLocalUserId()
			{
				return s_localUserId;
			}

			private string PUIDToString(ProductUserId puid)
			{
				string text = null;
				if (puid != null)
				{
					text = puid.ToString();
				}
				if (text == null)
				{
					text = "null";
				}
				return text;
			}

			protected void SetLocalProductUserId(ProductUserId localProductUserId)
			{
				s_localProductUserId = localProductUserId;
			}

			public ProductUserId GetProductUserId()
			{
				return s_localProductUserId;
			}

			public string GetProductId()
			{
				return Config.Get<ProductConfig>().ProductId.ToString("N").ToLowerInvariant();
			}

			public string GetSandboxId()
			{
				return PlatformManager.GetPlatformConfig().deployment.SandboxId.ToString();
			}

			public string GetDeploymentID()
			{
				return PlatformManager.GetPlatformConfig().deployment.DeploymentId.ToString("N").ToLowerInvariant();
			}

			public bool IsEncryptionKeyValid()
			{
				return PlatformManager.GetPlatformConfig().clientCredentials.IsEncryptionKeyValid();
			}

			private bool HasShutdown()
			{
				return s_state == EOSState.Shutdown;
			}

			public bool HasLoggedInWithConnect()
			{
				return s_localProductUserId != null;
			}

			public bool ShouldOverlayReceiveInput()
			{
				if (!s_isOverlayVisible || !s_DoesOverlayHaveExcusiveInput)
				{
					return PlatformManager.GetPlatformConfig().alwaysSendInputToOverlay;
				}
				return true;
			}

			public bool IsOverlayOpenWithExclusiveInput()
			{
				if (s_isOverlayVisible)
				{
					return s_DoesOverlayHaveExcusiveInput;
				}
				return false;
			}

			[Conditional("ENABLE_DEBUG_EOSMANAGER")]
			internal static void Log(string toPrint, LogType type = LogType.Log)
			{
				Debug.LogFormat(type, LogOption.None, null, toPrint);
			}

			public void AddConnectLoginListener(IEOSOnConnectLogin connectLogin)
			{
				OnConnectLogin += connectLogin.OnConnectLogin;
			}

			public void AddAuthLoginListener(IEOSOnAuthLogin authLogin)
			{
				OnAuthLogin += authLogin.OnAuthLogin;
			}

			public void AddAuthLogoutListener(IEOSOnAuthLogout authLogout)
			{
				OnAuthLogout += authLogout.OnAuthLogout;
			}

			public void AddApplicationCloseListener(Action listener)
			{
				s_onApplicationShutdownCallbacks.Add(listener);
			}

			public void RemoveConnectLoginListener(IEOSOnConnectLogin connectLogin)
			{
				OnConnectLogin -= connectLogin.OnConnectLogin;
			}

			public void RemoveAuthLoginListener(IEOSOnAuthLogin authLogin)
			{
				OnAuthLogin -= authLogin.OnAuthLogin;
			}

			public void RemoveAuthLogoutListener(IEOSOnAuthLogout authLogout)
			{
				OnAuthLogout -= authLogout.OnAuthLogout;
			}

			public T GetOrCreateManager<T>() where T : IEOSSubManager, new()
			{
				T val = default(T);
				Type typeFromHandle = typeof(T);
				if (!s_subManagers.ContainsKey(typeFromHandle))
				{
					val = new T();
					s_subManagers.Add(typeFromHandle, val);
					if (val is IEOSOnConnectLogin iEOSOnConnectLogin)
					{
						OnConnectLogin += iEOSOnConnectLogin.OnConnectLogin;
					}
					if (val is IEOSOnAuthLogin iEOSOnAuthLogin)
					{
						OnAuthLogin += iEOSOnAuthLogin.OnAuthLogin;
					}
					if (val is IEOSOnAuthLogout iEOSOnAuthLogout)
					{
						OnAuthLogout += iEOSOnAuthLogout.OnAuthLogout;
					}
				}
				else
				{
					val = (T)s_subManagers[typeFromHandle];
				}
				return val;
			}

			public void RemoveManager<T>() where T : IEOSSubManager
			{
				Type typeFromHandle = typeof(T);
				if (s_subManagers.ContainsKey(typeFromHandle))
				{
					T val = (T)s_subManagers[typeFromHandle];
					if (val is IEOSOnConnectLogin)
					{
						RemoveConnectLoginListener(val as IEOSOnConnectLogin);
					}
					if (val is IEOSOnAuthLogin)
					{
						RemoveAuthLoginListener(val as IEOSOnAuthLogin);
					}
					if (val is IEOSOnAuthLogout)
					{
						RemoveAuthLogoutListener(val as IEOSOnAuthLogout);
					}
					s_subManagers.Remove(typeFromHandle);
				}
			}

			private Result InitializePlatformInterface()
			{
				EOSInitializeOptions eOSInitializeOptions = ConfigurationUtility.GetEOSInitializeOptions();
				RegisterForPlatformNotifications();
				return PlatformInterface.Initialize(ref eOSInitializeOptions.options);
			}

			private PlatformInterface CreatePlatformInterface()
			{
				return PlatformInterface.Create(ref ConfigurationUtility.GetEOSCreateOptions().options);
			}

			private void InitializeNetworkChecks(IEOSCoroutineOwner coroutineOwner)
			{
				EOSManagerPlatformSpecificsSingleton.Instance.InitializeNetworkChecks(coroutineOwner);
			}

			private void InitializeOverlay(IEOSCoroutineOwner coroutineOwner)
			{
				SetToggleFriendsButtonOptions options = new SetToggleFriendsButtonOptions
				{
					ButtonCombination = PlatformManager.GetPlatformConfig().toggleFriendsButtonCombination
				};
				Instance.GetEOSPlatformInterface().GetUIInterface().SetToggleFriendsButton(ref options);
				EOSManagerPlatformSpecificsSingleton.Instance.InitializeOverlay(coroutineOwner);
				AddNotifyDisplaySettingsUpdatedOptions options2 = default(AddNotifyDisplaySettingsUpdatedOptions);
				GetEOSUIInterface().AddNotifyDisplaySettingsUpdated(ref options2, null, delegate(ref OnDisplaySettingsUpdatedCallbackInfo data)
				{
					s_isOverlayVisible = data.IsVisible;
					s_DoesOverlayHaveExcusiveInput = data.IsExclusiveInput;
				});
			}

			private void ApplyCommandLineArguments()
			{
				EpicLauncherArgs commandLineArgsFromEpicLauncher = GetCommandLineArgsFromEpicLauncher();
				if (string.IsNullOrEmpty(commandLineArgsFromEpicLauncher.epicSandboxID) && string.IsNullOrEmpty(commandLineArgsFromEpicLauncher.epicDeploymentID))
				{
					return;
				}
				ProductConfig productConfig = Config.Get<ProductConfig>();
				if (!string.IsNullOrEmpty(commandLineArgsFromEpicLauncher.epicSandboxID))
				{
					bool flag = false;
					SandboxId sandboxId = SandboxId.FromString(commandLineArgsFromEpicLauncher.epicSandboxID);
					foreach (Named<SandboxId> sandbox in productConfig.Environments.Sandboxes)
					{
						if (sandbox.Value.Equals(sandboxId))
						{
							Debug.Log($"{sandbox} selected as sandbox.");
							flag = true;
							break;
						}
					}
					PlatformManager.GetPlatformConfig().deployment.SandboxId = sandboxId;
					if (!flag)
					{
						Debug.LogWarning($"Sandbox Id \"{sandboxId}\" was " + "provided on the command line, but was not found in the product config. Attempting to use it regardless.");
					}
				}
				if (string.IsNullOrEmpty(commandLineArgsFromEpicLauncher.epicDeploymentID))
				{
					return;
				}
				bool flag2 = false;
				foreach (Named<Deployment> deployment in productConfig.Environments.Deployments)
				{
					if (deployment.Value.DeploymentId.ToString().Equals(commandLineArgsFromEpicLauncher.epicDeploymentID, StringComparison.OrdinalIgnoreCase) || deployment.Value.DeploymentId.ToString("N").Equals(commandLineArgsFromEpicLauncher.epicDeploymentID, StringComparison.OrdinalIgnoreCase))
					{
						Debug.Log($"{deployment} selected as deployment.");
						flag2 = true;
						break;
					}
				}
				if (!Guid.TryParse(commandLineArgsFromEpicLauncher.epicDeploymentID, out var result))
				{
					Debug.LogWarning("ERROR: Invalid Guid \"" + commandLineArgsFromEpicLauncher.epicDeploymentID + "\" for Deployment Id was provided on the command line. EOS SDK will almost certainly fail to initialize.");
					result = Guid.Empty;
				}
				PlatformManager.GetPlatformConfig().deployment.DeploymentId = result;
				if (!result.Equals(Guid.Empty) && !flag2)
				{
					Debug.LogWarning($"Deployment \"{result}\" was " + "provided on the command line, but was not found in the product config. Attempting to use it regardless.");
				}
			}

			public void Init(IEOSCoroutineOwner coroutineOwner, string configFileName = null)
			{
				ApplyCommandLineArguments();
				if (GetEOSPlatformInterface() != null)
				{
					if (!hasSetLoggingCallback)
					{
						LoggingInterface.SetCallback(SimplePrintCallback);
						hasSetLoggingCallback = true;
					}
					InitializeLogLevels();
					InitializeNetworkChecks(coroutineOwner);
					InitializeOverlay(coroutineOwner);
					return;
				}
				s_state = EOSState.Starting;
				LoadEOSLibraries();
				InitializeLogLevels();
				Result result = InitializePlatformInterface();
				if (result != Result.Success)
				{
					if (!s_eosUnloadSDKOnShutdown)
					{
						_ = 15;
					}
					if (result != Result.Success && result != Result.Success)
					{
						throw new Exception("Epic Online Services didn't init correctly: " + result);
					}
				}
				s_hasInitializedPlatform = true;
				LoggingInterface.SetCallback(SimplePrintCallback);
				PlatformInterface platformInterface = CreatePlatformInterface();
				if (platformInterface == null)
				{
					throw new Exception("failed to create an Epic Online Services PlatformInterface");
				}
				SetEOSPlatformInterface(platformInterface);
				UpdateEOSApplicationStatus();
				InitializeNetworkChecks(coroutineOwner);
				InitializeOverlay(coroutineOwner);
			}

			public void RegisterForPlatformNotifications()
			{
				EOSManagerPlatformSpecificsSingleton.Instance?.RegisterForPlatformNotifications();
			}

			[MonoPInvokeCallback(typeof(string))]
			private static void SimplePrintStringCallback(string str)
			{
				Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "{0}", str);
			}

			[MonoPInvokeCallback(typeof(LogMessageFunc))]
			private static void SimplePrintCallback(ref LogMessage message)
			{
				DateTime now = DateTime.Now;
				Utf8String utf8String = ((message.Category.Length == 0) ? new Utf8String() : message.Category);
				LogType logType = ((message.Level >= LogLevel.Warning) ? ((message.Level <= LogLevel.Warning) ? LogType.Warning : LogType.Log) : LogType.Error);
				Debug.LogFormat(logType, LogOption.NoStacktrace, null, "{0:O} {1}({2}): {3}", now.ToString(DateTimeFormatInfo.InvariantInfo), utf8String, message.Level, message.Message);
			}

			public void SetLogLevel(LogCategory Category, LogLevel Level)
			{
				LoggingInterface.SetLogLevel(Category, Level);
				if (logLevels == null)
				{
					logLevels = new Dictionary<LogCategory, LogLevel>();
				}
				if (Category == LogCategory.AllCategories)
				{
					foreach (LogCategory value in Enum.GetValues(typeof(LogCategory)))
					{
						if (value != LogCategory.AllCategories)
						{
							logLevels[value] = Level;
						}
					}
					return;
				}
				logLevels[Category] = Level;
			}

			private void InitializeLogLevels()
			{
				List<LogLevel> logLevelList = LogLevelUtility.LogLevelList;
				if (logLevelList == null)
				{
					SetLogLevel(LogCategory.AllCategories, LogLevel.Info);
					return;
				}
				for (int i = 0; i < logLevelList.Count; i++)
				{
					SetLogLevel((LogCategory)i, logLevelList[i]);
				}
			}

			public LogLevel GetLogLevel(LogCategory Category)
			{
				if (logLevels == null)
				{
					return LogLevel.Off;
				}
				if (Category == LogCategory.AllCategories)
				{
					LogLevel logLevel = GetLogLevel(LogCategory.Core);
					{
						foreach (LogCategory value in Enum.GetValues(typeof(LogCategory)))
						{
							if (value != LogCategory.AllCategories && GetLogLevel(value) != logLevel)
							{
								return (LogLevel)(-1);
							}
						}
						return logLevel;
					}
				}
				if (logLevels.ContainsKey(Category))
				{
					return logLevels[Category];
				}
				return LogLevel.Off;
			}

			[MonoPInvokeCallback(typeof(LogMessageFunc))]
			private static void SimplePrintCallbackWithCallstack(LogMessage message)
			{
				_ = DateTime.Now;
				if (message.Category.Length != 0)
				{
					_ = message.Category;
				}
				else
				{
					new Utf8String();
				}
			}

			private static Epic.OnlineServices.Auth.LoginOptions MakeLoginOptions(LoginCredentialType loginType, ExternalCredentialType externalCredentialType, string id, string token)
			{
				Epic.OnlineServices.Auth.Credentials value = new Epic.OnlineServices.Auth.Credentials
				{
					Type = loginType,
					ExternalType = externalCredentialType,
					Id = id,
					Token = token
				};
				return new Epic.OnlineServices.Auth.LoginOptions
				{
					Credentials = value,
					ScopeFlags = PlatformManager.GetPlatformConfig().authScopeOptionsFlags
				};
			}

			public Token? GetUserAuthTokenForAccountId(EpicAccountId accountId)
			{
				AuthInterface authInterface = GetEOSPlatformInterface().GetAuthInterface();
				CopyUserAuthTokenOptions options = default(CopyUserAuthTokenOptions);
				authInterface.CopyUserAuthToken(ref options, accountId, out var outUserAuthToken);
				return outUserAuthToken;
			}

			public static EpicLauncherArgs GetCommandLineArgsFromEpicLauncher()
			{
				EpicLauncherArgs result = default(EpicLauncherArgs);
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				foreach (string text in commandLineArgs)
				{
					if (text.StartsWith("-AUTH_LOGIN="))
					{
						ConfigureEpicArgument(text, ref result.authLogin);
					}
					else if (text.StartsWith("-AUTH_PASSWORD="))
					{
						ConfigureEpicArgument(text, ref result.authPassword);
					}
					else if (text.StartsWith("-AUTH_TYPE="))
					{
						ConfigureEpicArgument(text, ref result.authType);
					}
					else if (text.StartsWith("-epicapp="))
					{
						ConfigureEpicArgument(text, ref result.epicApp);
					}
					else if (text.StartsWith("-epicenv="))
					{
						ConfigureEpicArgument(text, ref result.epicEnv);
					}
					else if (text.StartsWith("-epicusername="))
					{
						ConfigureEpicArgument(text, ref result.epicUsername);
					}
					else if (text.StartsWith("-epicuserid="))
					{
						ConfigureEpicArgument(text, ref result.epicUserID);
					}
					else if (text.StartsWith("-epiclocale="))
					{
						ConfigureEpicArgument(text, ref result.epicLocale);
					}
					else if (text.StartsWith("-epicsandboxid="))
					{
						ConfigureEpicArgument(text, ref result.epicSandboxID);
					}
					else if (text.StartsWith("-eossandboxid="))
					{
						ConfigureEpicArgument(text, ref result.epicSandboxID);
					}
					else if (text.StartsWith("-eosdeploymentid="))
					{
						ConfigureEpicArgument(text, ref result.epicDeploymentID);
					}
					else if (text.StartsWith("-epicdeploymentid="))
					{
						ConfigureEpicArgument(text, ref result.epicDeploymentID);
					}
				}
				return result;
				static void ConfigureEpicArgument(string argument, ref string argumentString)
				{
					int num = argument.IndexOf('=') + 1;
					if (num >= 0 && num <= argument.Length)
					{
						argumentString = argument.Substring(num);
					}
				}
			}

			public void CreateConnectUserWithContinuanceToken(ContinuanceToken token, OnCreateConnectUserCallback onCreateUserCallback)
			{
				ConnectInterface connectInterface = GetEOSPlatformInterface().GetConnectInterface();
				CreateUserOptions options = new CreateUserOptions
				{
					ContinuanceToken = token
				};
				connectInterface.CreateUser(ref options, null, delegate(ref CreateUserCallbackInfo createUserCallbackInfo)
				{
					if (createUserCallbackInfo.ResultCode == Result.Success)
					{
						SetLocalProductUserId(createUserCallbackInfo.LocalUserId);
					}
					if (onCreateUserCallback != null)
					{
						onCreateUserCallback(createUserCallbackInfo);
					}
				});
			}

			public void AuthLinkExternalAccountWithContinuanceToken(ContinuanceToken token, LinkAccountFlags linkAccountFlags, OnAuthLinkExternalAccountCallback callback)
			{
				AuthInterface authInterface = GetEOSPlatformInterface().GetAuthInterface();
				Epic.OnlineServices.Auth.LinkAccountOptions options = new Epic.OnlineServices.Auth.LinkAccountOptions
				{
					ContinuanceToken = token,
					LinkAccountFlags = linkAccountFlags,
					LocalUserId = null
				};
				if (linkAccountFlags.HasFlag(LinkAccountFlags.NintendoNsaId))
				{
					options.LocalUserId = Instance.GetLocalUserId();
				}
				authInterface.LinkAccount(ref options, null, delegate(ref Epic.OnlineServices.Auth.LinkAccountCallbackInfo linkAccountCallbackInfo)
				{
					Instance.SetLocalUserId(linkAccountCallbackInfo.LocalUserId);
					if (callback != null)
					{
						callback(linkAccountCallbackInfo);
					}
				});
			}

			public void ConnectLinkExternalAccountWithContinuanceToken(ContinuanceToken token, OnConnectLinkExternalAccountCallback callback)
			{
				ConnectInterface connectInterface = GetEOSPlatformInterface().GetConnectInterface();
				Epic.OnlineServices.Connect.LinkAccountOptions options = new Epic.OnlineServices.Connect.LinkAccountOptions
				{
					ContinuanceToken = token,
					LocalUserId = Instance.GetProductUserId()
				};
				connectInterface.LinkAccount(ref options, null, delegate(ref Epic.OnlineServices.Connect.LinkAccountCallbackInfo linkAccountCallbackInfo)
				{
					if (callback != null)
					{
						callback(linkAccountCallbackInfo);
					}
				});
			}

			public async void StartConnectLoginWithEpicAccount(EpicAccountId epicAccountId, OnConnectLoginCallback onConnectLoginCallback)
			{
				AuthInterface authInterface = GetEOSPlatformInterface().GetAuthInterface();
				CopyIdTokenOptions options = new CopyIdTokenOptions
				{
					AccountId = epicAccountId
				};
				IdToken? outIdToken;
				Result result = authInterface.CopyIdToken(ref options, out outIdToken);
				if (result != Result.Success || !outIdToken.HasValue)
				{
					Debug.LogError(string.Format("{0} {1}: CopyIdToken failed with result: {2}", "EOSManager", "StartConnectLoginWithEpicAccount", result));
					Epic.OnlineServices.Connect.LoginCallbackInfo loginCallbackInfo = new Epic.OnlineServices.Connect.LoginCallbackInfo
					{
						ResultCode = Result.InvalidAuth
					};
					onConnectLoginCallback?.Invoke(loginCallbackInfo);
					return;
				}
				Epic.OnlineServices.Connect.LoginOptions connectLoginOptions = new Epic.OnlineServices.Connect.LoginOptions
				{
					Credentials = new Epic.OnlineServices.Connect.Credentials
					{
						Token = outIdToken.Value.JsonWebToken,
						Type = ExternalCredentialType.EpicIdToken
					}
				};
				if (GetUserLoginInfo != null)
				{
					connectLoginOptions.UserLoginInfo = await GetUserLoginInfo();
				}
				StartConnectLoginWithOptions(connectLoginOptions, onConnectLoginCallback);
			}

			public void StartConnectLoginWithOptions(ExternalCredentialType externalCredentialType, string token, string displayname = null, string nsaIdToken = null, OnConnectLoginCallback onloginCallback = null)
			{
				Epic.OnlineServices.Connect.LoginOptions connectLoginOptions = new Epic.OnlineServices.Connect.LoginOptions
				{
					Credentials = new Epic.OnlineServices.Connect.Credentials
					{
						Token = token,
						Type = externalCredentialType
					}
				};
				switch (externalCredentialType)
				{
				case ExternalCredentialType.EpicIdToken:
					if (!string.IsNullOrEmpty(nsaIdToken))
					{
						connectLoginOptions.UserLoginInfo = new UserLoginInfo
						{
							DisplayName = displayname,
							NsaIdToken = nsaIdToken
						};
					}
					break;
				case ExternalCredentialType.XblXstsToken:
					connectLoginOptions.UserLoginInfo = null;
					break;
				case ExternalCredentialType.NintendoIdToken:
				case ExternalCredentialType.NintendoNsaIdToken:
				case ExternalCredentialType.DeviceidAccessToken:
				case ExternalCredentialType.AppleIdToken:
				case ExternalCredentialType.GoogleIdToken:
				case ExternalCredentialType.OculusUseridNonce:
				case ExternalCredentialType.AmazonAccessToken:
					connectLoginOptions.UserLoginInfo = new UserLoginInfo
					{
						DisplayName = displayname
					};
					break;
				default:
					connectLoginOptions.UserLoginInfo = null;
					break;
				}
				StartConnectLoginWithOptions(connectLoginOptions, onloginCallback);
			}

			public void StartConnectLoginWithOptions(ExternalCredentialType externalCredentialType, string token, string displayname, OnConnectLoginCallback onloginCallback)
			{
				StartConnectLoginWithOptions(externalCredentialType, token, displayname, null, onloginCallback);
			}

			public void StartConnectLoginWithOptions(Epic.OnlineServices.Connect.LoginOptions connectLoginOptions, OnConnectLoginCallback onloginCallback)
			{
				GetEOSPlatformInterface().GetConnectInterface().Login(ref connectLoginOptions, null, delegate(ref Epic.OnlineServices.Connect.LoginCallbackInfo connectLoginData)
				{
					_ = connectLoginData.ResultCode;
					if (connectLoginData.LocalUserId != null)
					{
						SetLocalProductUserId(connectLoginData.LocalUserId);
						ConfigureConnectStatusCallback();
						ConfigureConnectExpirationCallback(connectLoginOptions);
						EOSManager.OnConnectLogin?.Invoke(connectLoginData);
					}
					if (onloginCallback != null)
					{
						onloginCallback(connectLoginData);
					}
				});
			}

			public void StartConnectLoginWithDeviceToken(string displayName, OnConnectLoginCallback onLoginCallback)
			{
				GetEOSPlatformInterface().GetConnectInterface();
				StartConnectLoginWithOptions(new Epic.OnlineServices.Connect.LoginOptions
				{
					UserLoginInfo = new UserLoginInfo
					{
						DisplayName = displayName
					},
					Credentials = new Epic.OnlineServices.Connect.Credentials
					{
						Token = null,
						Type = ExternalCredentialType.DeviceidAccessToken
					}
				}, onLoginCallback);
			}

			public void ConnectTransferDeviceIDAccount(TransferDeviceIdAccountOptions options, object clientData, OnTransferDeviceIdAccountCallback completionDelegate = null)
			{
				GetEOSPlatformInterface().GetConnectInterface().TransferDeviceIdAccount(ref options, clientData, delegate(ref TransferDeviceIdAccountCallbackInfo data)
				{
					SetLocalProductUserId(data.LocalUserId);
					if (completionDelegate != null)
					{
						completionDelegate(ref data);
					}
				});
			}

			public void StartPersistentLogin(OnAuthLoginCallback onLoginCallback)
			{
				StartLoginWithLoginTypeAndToken(LoginCredentialType.PersistentAuth, null, null, delegate(Epic.OnlineServices.Auth.LoginCallbackInfo callbackInfo)
				{
					Result resultCode = callbackInfo.ResultCode;
					if (resultCode == Result.AuthInvalidRefreshToken || resultCode == Result.AuthInvalidPlatformToken)
					{
						AuthInterface authInterface = Instance.GetEOSPlatformInterface().GetAuthInterface();
						DeletePersistentAuthOptions options = default(DeletePersistentAuthOptions);
						authInterface.DeletePersistentAuth(ref options, null, delegate
						{
							if (onLoginCallback != null)
							{
								onLoginCallback(callbackInfo);
							}
						});
					}
					else if (onLoginCallback != null)
					{
						onLoginCallback(callbackInfo);
					}
				});
			}

			public void StartLoginWithLoginTypeAndToken(LoginCredentialType loginType, string id, string token, OnAuthLoginCallback onLoginCallback)
			{
				if (loginType == LoginCredentialType.ExchangeCode && string.IsNullOrEmpty(token))
				{
					Debug.LogError("EOSManager StartLoginWithLoginTypeAndToken: ExchangeCode login attempted with empty token. Abort login.");
					onLoginCallback?.Invoke(new Epic.OnlineServices.Auth.LoginCallbackInfo
					{
						ResultCode = Result.AuthExchangeCodeNotFound
					});
				}
				else
				{
					StartLoginWithLoginTypeAndToken(loginType, ExternalCredentialType.Epic, id, token, onLoginCallback);
				}
			}

			public void StartLoginWithLoginTypeAndToken(LoginCredentialType loginType, ExternalCredentialType externalCredentialType, string id, string token, OnAuthLoginCallback onLoginCallback)
			{
				Epic.OnlineServices.Auth.LoginOptions loginOptions = MakeLoginOptions(loginType, externalCredentialType, id, token);
				StartLoginWithLoginOptions(loginOptions, onLoginCallback);
			}

			private void ConfigureAuthStatusCallback()
			{
				if (s_notifyLoginStatusChangedCallbackHandle != null)
				{
					return;
				}
				AuthInterface authInterface = GetEOSPlatformInterface().GetAuthInterface();
				Epic.OnlineServices.Auth.AddNotifyLoginStatusChangedOptions options = default(Epic.OnlineServices.Auth.AddNotifyLoginStatusChangedOptions);
				s_notifyLoginStatusChangedCallbackHandle = new NotifyEventHandle(authInterface.AddNotifyLoginStatusChanged(ref options, null, delegate(ref Epic.OnlineServices.Auth.LoginStatusChangedCallbackInfo callbackInfo)
				{
					if (callbackInfo.CurrentStatus == LoginStatus.NotLoggedIn && callbackInfo.PrevStatus == LoginStatus.LoggedIn)
					{
						loggedInAccountIDs.Remove(callbackInfo.LocalUserId);
					}
				}), delegate(ulong handle)
				{
					GetEOSAuthInterface()?.RemoveNotifyLoginStatusChanged(handle);
				});
			}

			private void ConfigureConnectStatusCallback()
			{
				if (s_notifyConnectLoginStatusChangedCallbackHandle != null)
				{
					return;
				}
				ConnectInterface eOSConnectInterface = GetEOSConnectInterface();
				Epic.OnlineServices.Connect.AddNotifyLoginStatusChangedOptions options = default(Epic.OnlineServices.Connect.AddNotifyLoginStatusChangedOptions);
				s_notifyConnectLoginStatusChangedCallbackHandle = new NotifyEventHandle(eOSConnectInterface.AddNotifyLoginStatusChanged(ref options, null, delegate(ref Epic.OnlineServices.Connect.LoginStatusChangedCallbackInfo callbackInfo)
				{
					if (callbackInfo.CurrentStatus == LoginStatus.NotLoggedIn && callbackInfo.PreviousStatus == LoginStatus.LoggedIn)
					{
						SetLocalProductUserId(null);
					}
					else if (callbackInfo.CurrentStatus == LoginStatus.LoggedIn && callbackInfo.PreviousStatus == LoginStatus.NotLoggedIn)
					{
						SetLocalProductUserId(callbackInfo.LocalUserId);
					}
				}), delegate(ulong handle)
				{
					GetEOSConnectInterface()?.RemoveNotifyLoginStatusChanged(handle);
				});
			}

			private void ConfigureConnectExpirationCallback(Epic.OnlineServices.Connect.LoginOptions connectLoginOptions)
			{
				if (s_notifyConnectAuthExpirationCallbackHandle == null)
				{
					ConnectInterface eOSConnectInterface = GetEOSConnectInterface();
					AddNotifyAuthExpirationOptions options = default(AddNotifyAuthExpirationOptions);
					s_notifyConnectAuthExpirationCallbackHandle = new NotifyEventHandle(eOSConnectInterface.AddNotifyAuthExpiration(ref options, null, delegate
					{
						StartConnectLoginWithOptions(connectLoginOptions, null);
					}), delegate(ulong handle)
					{
						GetEOSConnectInterface()?.RemoveNotifyAuthExpiration(handle);
					});
				}
			}

			public void StartLoginWithLoginOptions(Epic.OnlineServices.Auth.LoginOptions loginOptions, OnAuthLoginCallback onLoginCallback)
			{
				AuthInterface authInterface = GetEOSPlatformInterface().GetAuthInterface();
				SetDisplayPreferenceOptions options = new SetDisplayPreferenceOptions
				{
					NotificationLocation = NotificationLocation.TopRight
				};
				Instance.GetEOSPlatformInterface().GetUIInterface().SetDisplayPreference(ref options);
				authInterface.Login(ref loginOptions, null, delegate(ref Epic.OnlineServices.Auth.LoginCallbackInfo data)
				{
					if (data.ResultCode == Result.Success)
					{
						loggedInAccountIDs.Add(data.LocalUserId);
						SetLocalUserId(data.LocalUserId);
						ConfigureAuthStatusCallback();
						EOSManager.OnAuthLogin?.Invoke(data);
					}
					else
					{
						string text = loginOptions.Credentials?.Type.ToString() ?? "UNKNOWN";
						Debug.LogWarning(string.Format("{0} {1}: {2} login failed with ResultCode: {3}", "EOSManager", "StartLoginWithLoginOptions", text, data.ResultCode));
					}
					if (onLoginCallback != null)
					{
						onLoginCallback(data);
					}
				});
			}

			public void SetPresenceRichTextForUser(EpicAccountId accountId, string richText)
			{
				PresenceInterface eOSPresenceInterface = GetEOSPresenceInterface();
				PresenceModification outPresenceModificationHandle = new PresenceModification();
				CreatePresenceModificationOptions options = new CreatePresenceModificationOptions
				{
					LocalUserId = accountId
				};
				eOSPresenceInterface.CreatePresenceModification(ref options, out outPresenceModificationHandle);
				PresenceModificationSetStatusOptions options2 = new PresenceModificationSetStatusOptions
				{
					Status = Status.Online
				};
				outPresenceModificationHandle.SetStatus(ref options2);
				PresenceModificationSetRawRichTextOptions options3 = new PresenceModificationSetRawRichTextOptions
				{
					RichText = richText
				};
				outPresenceModificationHandle.SetRawRichText(ref options3);
				SetPresenceOptions options4 = new SetPresenceOptions
				{
					LocalUserId = accountId,
					PresenceModificationHandle = outPresenceModificationHandle
				};
				eOSPresenceInterface.SetPresence(ref options4, null, delegate(ref SetPresenceCallbackInfo callbackInfo)
				{
					_ = callbackInfo.ResultCode;
				});
			}

			public void StartLogout(EpicAccountId accountId, OnLogoutCallback onLogoutCallback)
			{
				AuthInterface authInterface = GetEOSPlatformInterface().GetAuthInterface();
				LogoutOptions options = new LogoutOptions
				{
					LocalUserId = accountId
				};
				authInterface.Logout(ref options, null, delegate(ref LogoutCallbackInfo data)
				{
					if (onLogoutCallback != null)
					{
						SetLocalUserId(null);
						onLogoutCallback(ref data);
						EOSManager.OnAuthLogout?.Invoke(data);
					}
				});
			}

			public void ClearConnectId(ProductUserId userId)
			{
				if (GetProductUserId() == userId)
				{
					SetLocalProductUserId(null);
				}
			}

			public void RemovePersistentToken()
			{
				AuthInterface authInterface = Instance.GetEOSPlatformInterface().GetAuthInterface();
				DeletePersistentAuthOptions options = default(DeletePersistentAuthOptions);
				authInterface.DeletePersistentAuth(ref options, null, delegate(ref DeletePersistentAuthCallbackInfo deletePersistentAuthCallbackInfo)
				{
					_ = deletePersistentAuthCallbackInfo.ResultCode;
				});
			}

			public void Tick()
			{
				ExecuteQueuedMainThreadTasks();
				if (!(GetEOSPlatformInterface() != null))
				{
					return;
				}
				UpdateApplicationConstrainedState();
				if (Time.realtimeSinceStartup >= s_nextNetworkStatusUpdateTime)
				{
					UpdateNetworkStatus();
					s_nextNetworkStatusUpdateTime = Time.realtimeSinceStartup + 0.5f;
				}
				if (s_state != EOSState.Suspended)
				{
					GetEOSPlatformInterface().Tick();
					if (s_state == EOSState.Suspending)
					{
						s_state = EOSState.Suspended;
					}
				}
			}

			public void OnShutdown()
			{
				foreach (Action s_onApplicationShutdownCallback in s_onApplicationShutdownCallbacks)
				{
					s_onApplicationShutdownCallback();
				}
				PlatformInterface eOSPlatformInterface = GetEOSPlatformInterface();
				if (eOSPlatformInterface != null)
				{
					AuthInterface authInterface = eOSPlatformInterface.GetAuthInterface();
					LogoutOptions options = default(LogoutOptions);
					foreach (EpicAccountId loggedInAccountID in loggedInAccountIDs)
					{
						options.LocalUserId = loggedInAccountID;
						authInterface.Logout(ref options, null, delegate(ref LogoutCallbackInfo data)
						{
							_ = data.ResultCode;
						});
					}
				}
				if (!HasShutdown())
				{
					OnApplicationShutdown();
				}
			}

			public void OnApplicationShutdown()
			{
				if (!HasShutdown())
				{
					s_state = EOSState.ShuttingDown;
					GC.Collect();
					GC.WaitForPendingFinalizers();
					GetEOSPlatformInterface()?.Release();
					if (s_eosUnloadSDKOnShutdown)
					{
						ShutdownPlatformInterface();
					}
					SetEOSPlatformInterface(null);
					s_state = EOSState.Shutdown;
				}
			}

			private void ShutdownPlatformInterface()
			{
				if (s_hasInitializedPlatform)
				{
					PlatformInterface.Shutdown();
				}
				s_hasInitializedPlatform = false;
			}

			public ApplicationStatus GetEOSApplicationStatus()
			{
				return GetEOSPlatformInterface().GetApplicationStatus();
			}

			private void SetEOSApplicationStatus(ApplicationStatus newStatus)
			{
				if (GetEOSApplicationStatus() != newStatus)
				{
					GetEOSPlatformInterface().SetApplicationStatus(newStatus);
				}
			}

			private void UpdateEOSApplicationStatus()
			{
				if (!(GetEOSPlatformInterface() == null))
				{
					if (s_isPaused)
					{
						SetEOSApplicationStatus(ApplicationStatus.BackgroundSuspended);
					}
					else if (s_hasFocus)
					{
						SetEOSApplicationStatus(ApplicationStatus.Foreground);
					}
				}
			}

			public void OnApplicationPause(bool isPaused)
			{
				_ = s_isPaused;
				s_isPaused = isPaused;
			}

			public void OnApplicationFocus(bool hasFocus)
			{
				s_hasFocus = hasFocus;
			}

			public void OnApplicationConstrained(bool isConstrained, bool shouldUpdateEOSAppStatus)
			{
				_ = s_isConstrained;
				s_isConstrained = isConstrained;
				if (shouldUpdateEOSAppStatus)
				{
					UpdateEOSApplicationStatus();
				}
			}

			private void UpdateApplicationConstrainedState()
			{
				if (EOSManagerPlatformSpecificsSingleton.Instance != null)
				{
					bool s_isConstrained = EOSManager.s_isConstrained;
					bool flag = EOSManagerPlatformSpecificsSingleton.Instance.IsApplicationConstrainedWhenOutOfFocus();
					if (s_isConstrained != flag)
					{
						EOSManager.s_isConstrained = flag;
						UpdateEOSApplicationStatus();
					}
				}
			}

			private static void UpdateNetworkStatus()
			{
				EOSManagerPlatformSpecificsSingleton.Instance?.UpdateNetworkStatus();
			}

			[DllImport("GfxPluginNativeRender-x64", CallingConvention = CallingConvention.StdCall)]
			private static extern IntPtr EOS_GetPlatformInterface();

			[DllImport("GfxPluginNativeRender-x64", CallingConvention = CallingConvention.StdCall)]
			private static extern void global_log_flush_with_function(IntPtr ptr);

			public PlatformInterface GetEOSPlatformInterface()
			{
				if (s_eosPlatformInterface == null && s_state != EOSState.Shutdown)
				{
					IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate<PrintDelegateType>(SimplePrintStringCallback);
					SimplePrintStringCallback("Start of Early EOS LOG:");
					global_log_flush_with_function(functionPointerForDelegate);
					SimplePrintStringCallback("End of Early EOS LOG");
					if (EOS_GetPlatformInterface() == IntPtr.Zero)
					{
						throw new Exception("NULL EOS Platform returned by native code: issue probably occurred in GFX Plugin!");
					}
					SetEOSPlatformInterface(new PlatformInterface(EOS_GetPlatformInterface()));
				}
				return s_eosPlatformInterface;
			}

			private void SetEOSPlatformInterface(PlatformInterface platformInterface)
			{
				if (platformInterface != null)
				{
					s_state = EOSState.Running;
				}
				s_eosPlatformInterface = platformInterface;
			}

			private static void AddAllAssembliesInCurrentDomain(List<Assembly> list)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly item in assemblies)
				{
					list.Add(item);
				}
			}

			public static DLLHandle LoadDynamicLibrary(string libraryName)
			{
				if (LoadedDLLs.ContainsKey(libraryName))
				{
					return LoadedDLLs[libraryName];
				}
				DLLHandle dLLHandle = DLLHandle.LoadDynamicLibrary(libraryName);
				LoadedDLLs[libraryName] = dLLHandle;
				return dLLHandle;
			}

			public static void UnloadAllLibraries()
			{
				foreach (KeyValuePair<string, DLLHandle> loadedDLL in LoadedDLLs)
				{
					loadedDLL.Value.Dispose();
				}
				LoadedDLLs.Clear();
				LoadedDLLs = new Dictionary<string, DLLHandle>();
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}

			public static void LoadDelegatesWithEOSBindingAPI()
			{
			}

			private static void ForceUnloadEOSLibrary()
			{
			}

			public static void LoadEOSLibraries()
			{
			}

			public AuthInterface GetEOSAuthInterface()
			{
				return GetEOSPlatformInterface()?.GetAuthInterface();
			}

			public AchievementsInterface GetEOSAchievementInterface()
			{
				return GetEOSPlatformInterface()?.GetAchievementsInterface();
			}

			public ConnectInterface GetEOSConnectInterface()
			{
				return GetEOSPlatformInterface()?.GetConnectInterface();
			}

			public EcomInterface GetEOSEcomInterface()
			{
				return GetEOSPlatformInterface()?.GetEcomInterface();
			}

			public FriendsInterface GetEOSFriendsInterface()
			{
				return GetEOSPlatformInterface()?.GetFriendsInterface();
			}

			public LeaderboardsInterface GetEOSLeaderboardsInterface()
			{
				return GetEOSPlatformInterface()?.GetLeaderboardsInterface();
			}

			public LobbyInterface GetEOSLobbyInterface()
			{
				return GetEOSPlatformInterface()?.GetLobbyInterface();
			}

			public MetricsInterface GetEOSMetricsInterface()
			{
				return GetEOSPlatformInterface()?.GetMetricsInterface();
			}

			public ModsInterface GetEOSModsInterface()
			{
				return GetEOSPlatformInterface()?.GetModsInterface();
			}

			public P2PInterface GetEOSP2PInterface()
			{
				return GetEOSPlatformInterface()?.GetP2PInterface();
			}

			public PlayerDataStorageInterface GetPlayerDataStorageInterface()
			{
				PlayerDataStorageInterface playerDataStorageInterface = GetEOSPlatformInterface().GetPlayerDataStorageInterface();
				if (playerDataStorageInterface == null)
				{
					throw new Exception("Could not get PlayerDataStorage interface, EncryptionKey may be empty or null");
				}
				return playerDataStorageInterface;
			}

			public PresenceInterface GetEOSPresenceInterface()
			{
				return GetEOSPlatformInterface()?.GetPresenceInterface();
			}

			public RTCInterface GetEOSRTCInterface()
			{
				return GetEOSPlatformInterface()?.GetRTCInterface();
			}

			public SessionsInterface GetEOSSessionsInterface()
			{
				return GetEOSPlatformInterface()?.GetSessionsInterface();
			}

			public StatsInterface GetEOSStatsInterface()
			{
				return GetEOSPlatformInterface()?.GetStatsInterface();
			}

			public TitleStorageInterface GetEOSTitleStorageInterface()
			{
				return GetEOSPlatformInterface()?.GetTitleStorageInterface();
			}

			public UIInterface GetEOSUIInterface()
			{
				return GetEOSPlatformInterface()?.GetUIInterface();
			}

			public UserInfoInterface GetEOSUserInfoInterface()
			{
				return GetEOSPlatformInterface()?.GetUserInfoInterface();
			}
		}

		public bool ShouldShutdownOnApplicationQuit = true;

		public static Func<Task<UserLoginInfo>> GetUserLoginInfo = null;

		private static List<EpicAccountId> loggedInAccountIDs = new List<EpicAccountId>();

		private static Dictionary<Type, IEOSSubManager> s_subManagers = new Dictionary<Type, IEOSSubManager>();

		private static List<Action> s_onApplicationShutdownCallbacks = new List<Action>();

		private static bool s_isOverlayVisible;

		private static bool s_DoesOverlayHaveExcusiveInput;

		private static Dictionary<LogCategory, LogLevel> logLevels;

		private static EOSManager s_EOSManagerInstance = null;

		private static EOSState s_state = EOSState.NotStarted;

		private static bool s_isPaused;

		private static bool s_hasFocus = true;

		private static bool s_isConstrained = true;

		private static EOSSingleton s_instance;

		private static List<Action> s_enqueuedTasks;

		private static object s_enqueuedTasksLock = new object();

		public static bool ApplicationIsPaused => s_isPaused;

		public static bool ApplicationHasFocus => s_hasFocus;

		public static bool ApplicationIsConstrained => s_isConstrained;

		public static EOSSingleton Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new EOSSingleton();
				}
				return s_instance;
			}
		}

		private static event OnAuthLoginCallback OnAuthLogin;

		private static event OnAuthLogoutCallback OnAuthLogout;

		private static event OnConnectLoginCallback OnConnectLogin;

		private void Awake()
		{
			if (s_EOSManagerInstance != null)
			{
				base.enabled = false;
				return;
			}
			s_EOSManagerInstance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Instance.Init(this);
		}

		private void Update()
		{
			Instance.Tick();
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			Instance.OnApplicationFocus(hasFocus);
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			Instance.OnApplicationPause(pauseStatus);
		}

		private void OnEnable()
		{
			Application.quitting += OnApplicationQuitting;
		}

		private void OnDisable()
		{
			Application.quitting -= OnApplicationQuitting;
		}

		private void OnApplicationQuitting()
		{
			if (ShouldShutdownOnApplicationQuit)
			{
				Instance.OnShutdown();
			}
		}

		void IEOSCoroutineOwner.StartCoroutine(IEnumerator routine)
		{
			StartCoroutine(routine);
		}

		public static void DispatchAsync(Action action)
		{
			lock (s_enqueuedTasksLock)
			{
				if (s_enqueuedTasks == null)
				{
					s_enqueuedTasks = new List<Action>();
				}
				s_enqueuedTasks.Add(action);
			}
		}

		private static void ExecuteQueuedMainThreadTasks()
		{
			List<Action> list;
			lock (s_enqueuedTasksLock)
			{
				list = s_enqueuedTasks;
				s_enqueuedTasks = null;
				if (list == null)
				{
					return;
				}
			}
			foreach (Action item in list)
			{
				item();
			}
		}
	}
}

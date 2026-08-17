using System;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.UI;
using Newtonsoft.Json;
using PlayEveryWare.Common;
using PlayEveryWare.EpicOnlineServices.Utility;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public abstract class PlatformConfig : Config
	{
		protected sealed class NonOverrideableConfigValues : Config
		{
			public string deploymentID;

			public string clientID;

			public uint tickBudgetInMilliseconds;

			public double taskNetworkTimeoutSeconds;

			[JsonConverter(typeof(ListOfStringsToPlatformFlags))]
			public WrappedPlatformFlags platformOptionsFlags;

			[JsonConverter(typeof(ListOfStringsToAuthScopeFlags))]
			public AuthScopeFlags authScopeOptionsFlags;

			[JsonConverter(typeof(ListOfStringsToIntegratedPlatformManagementFlags))]
			public IntegratedPlatformManagementFlags integratedPlatformManagementFlags;

			public bool alwaysSendInputToOverlay;

			static NonOverrideableConfigValues()
			{
				Config.RegisterFactory(() => new NonOverrideableConfigValues());
			}

			internal NonOverrideableConfigValues()
				: base("EpicOnlineServicesConfig.json")
			{
			}
		}

		internal sealed class OverrideableConfigValues : Config
		{
			[JsonConverter(typeof(ListOfStringsToPlatformFlags))]
			public WrappedPlatformFlags platformOptionsFlags;

			[JsonConverter(typeof(StringToTypeConverter<float>))]
			public float? initialButtonDelayForOverlay;

			[JsonConverter(typeof(StringToTypeConverter<float>))]
			public float? repeatButtonDelayForOverlay;

			[JsonConverter(typeof(StringToTypeConverter<ulong>))]
			public ulong? ThreadAffinity_networkWork;

			[JsonConverter(typeof(StringToTypeConverter<ulong>))]
			public ulong? ThreadAffinity_storageIO;

			[JsonConverter(typeof(StringToTypeConverter<ulong>))]
			public ulong? ThreadAffinity_webSocketIO;

			[JsonConverter(typeof(StringToTypeConverter<ulong>))]
			public ulong? ThreadAffinity_P2PIO;

			[JsonConverter(typeof(StringToTypeConverter<ulong>))]
			public ulong? ThreadAffinity_HTTPRequestIO;

			[JsonConverter(typeof(StringToTypeConverter<ulong>))]
			public ulong? ThreadAffinity_RTCIO;

			static OverrideableConfigValues()
			{
				Config.RegisterFactory(() => new OverrideableConfigValues());
			}

			internal OverrideableConfigValues()
				: base("EpicOnlineServicesConfig.json")
			{
			}
		}

		private const PlatformManager.Platform OVERLAY_COMPATIBLE_PLATFORMS = ~(PlatformManager.Platform.Android | PlatformManager.Platform.iOS | PlatformManager.Platform.Linux | PlatformManager.Platform.macOS);

		[JsonProperty]
		[JsonIgnore]
		[Obsolete]
		public EOSConfig overrideValues;

		[ConfigField("Deployment", ConfigFieldType.Deployment, "Select the deployment to use.", 1, null)]
		public Deployment deployment;

		[ConfigField("Client Credentials", ConfigFieldType.ClientCredentials, "Select client credentials to use.", 1, null)]
		public EOSClientCredentials clientCredentials;

		[ConfigField("Is Server", ConfigFieldType.Flag, "Check this if your game is a dedicated game server.", 1, null)]
		public bool isServer;

		[ConfigField("Platform Flags", ConfigFieldType.Enum, "Platform option flags", 2, "https://dev.epicgames.com/docs/epic-online-services/eos-get-started/working-with-the-eos-sdk/eos-overlay-overview#eos-platform-flags-for-the-eos-overlay")]
		[JsonConverter(typeof(ListOfStringsToPlatformFlags))]
		public WrappedPlatformFlags platformOptionsFlags;

		[ConfigField("Auth Scope Flags", ConfigFieldType.Enum, "Platform option flags", 2, "https://dev.epicgames.com/docs/api-ref/enums/eos-e-auth-scope-flags?lang=en-US")]
		[JsonConverter(typeof(ListOfStringsToAuthScopeFlags))]
		public AuthScopeFlags authScopeOptionsFlags;

		[ConfigField("Integrated Platform Management Flags", ConfigFieldType.Enum, "Integrated Platform Management Flags for platform specific options.", 2, "https://dev.epicgames.com/docs/api-ref/enums/eos-e-integrated-platform-management-flags")]
		[JsonConverter(typeof(ListOfStringsToIntegratedPlatformManagementFlags))]
		public IntegratedPlatformManagementFlags integratedPlatformManagementFlags;

		[ConfigField("Tick Budget (ms)", ConfigFieldType.Uint, "Used to define the maximum amount of execution time the EOS SDK can use each frame.", 3, null)]
		public uint tickBudgetInMilliseconds;

		[ConfigField("Network Timeout Seconds", ConfigFieldType.Double, "Indicates the maximum number of seconds that (before first coming online) the EOS SDK will allow network calls to run before failing with EOS_TimedOut. This value does not apply after the EOS SDK has been initialized.", 3, null)]
		public double taskNetworkTimeoutSeconds;

		[ConfigField("Thread Affinity Options", ConfigFieldType.WrappedInitializeThreadAffinity, "Defines the thread affinity for threads started by the EOS SDK. Leave values at zero to use default platform settings.", 3, "https://dev.epicgames.com/docs/api-ref/structs/eos-initialize-thread-affinity")]
		public WrappedInitializeThreadAffinity threadAffinity;

		[ConfigField(~(PlatformManager.Platform.Android | PlatformManager.Platform.iOS | PlatformManager.Platform.Linux | PlatformManager.Platform.macOS), "Always Send Input to Overlay", ConfigFieldType.Flag, "If true, the plugin will always send input to the overlay from the C# side to native, and handle showing the overlay. This doesn't always mean input makes it to the EOS SDK.", 4, null)]
		public bool alwaysSendInputToOverlay;

		[ConfigField(~(PlatformManager.Platform.Android | PlatformManager.Platform.iOS | PlatformManager.Platform.Linux | PlatformManager.Platform.macOS), "Initial Button Delay", ConfigFieldType.Float, "Initial Button Delay (if not set, whatever the default is will be used).", 4, null)]
		[JsonConverter(typeof(StringToTypeConverter<float>))]
		public float initialButtonDelayForOverlay;

		[ConfigField(~(PlatformManager.Platform.Android | PlatformManager.Platform.iOS | PlatformManager.Platform.Linux | PlatformManager.Platform.macOS), "Repeat Button Delay", ConfigFieldType.Float, "Repeat button delay for the overlay. If not set, whatever the default is will be used.", 4, null)]
		[JsonConverter(typeof(StringToTypeConverter<float>))]
		public float repeatButtonDelayForOverlay;

		[JsonConverter(typeof(ListOfStringsToInputStateButtonFlags))]
		[ConfigField(~(PlatformManager.Platform.Android | PlatformManager.Platform.iOS | PlatformManager.Platform.Linux | PlatformManager.Platform.macOS), "Default Activate Overlay Button", ConfigFieldType.Enum, "Users can press the button's associated with this value to activate the Epic Social Overlay. Not all combinations are valid; the SDK will log an error at the start of runtime if an invalid combination is selected.", 4, null)]
		public InputStateButtonFlags toggleFriendsButtonCombination = InputStateButtonFlags.SpecialLeft;

		[JsonIgnore]
		public PlatformManager.Platform Platform { get; }

		[JsonProperty]
		[JsonIgnore]
		[Obsolete("This property is deprecated. Use the property integratedPlatformManagementFlags instead.")]
		[JsonConverter(typeof(ListOfStringsToIntegratedPlatformManagementFlags))]
		public IntegratedPlatformManagementFlags flags
		{
			get
			{
				return integratedPlatformManagementFlags;
			}
			set
			{
				integratedPlatformManagementFlags = value;
			}
		}

		protected PlatformConfig(PlatformManager.Platform platform)
			: base(PlatformManager.GetConfigFileName(platform))
		{
			Platform = platform;
		}

		protected override void OnReadCompleted()
		{
			base.OnReadCompleted();
			EOSClientCredentials eOSClientCredentials;
			if (deployment.IsComplete)
			{
				eOSClientCredentials = clientCredentials;
				if (eOSClientCredentials != null && eOSClientCredentials.IsComplete)
				{
					return;
				}
			}
			ProductConfig productConfig = Config.Get<ProductConfig>();
			if (!deployment.IsComplete && productConfig.Environments.TryGetFirstDefinedNamedDeployment(out var named))
			{
				deployment = named.Value;
				Debug.Log($"Platform {Platform} has no defined deployment, " + $"so one was selected: {named}.");
			}
			eOSClientCredentials = clientCredentials;
			if ((eOSClientCredentials == null || !eOSClientCredentials.IsComplete) && productConfig.TryGetFirstCompleteNamedClientCredentials(out var credentials))
			{
				clientCredentials = credentials.Value;
				Debug.Log($"Platform {Platform} has no defined client " + "credentials, so one was selected: " + $"{credentials}.");
			}
			if (threadAffinity == null)
			{
				threadAffinity = new WrappedInitializeThreadAffinity();
			}
		}

		private static TK SelectValue<TK>(TK overrideValuesFromFieldMember, TK mainConfigValue)
		{
			if (overrideValuesFromFieldMember.Equals(null))
			{
				return mainConfigValue;
			}
			return overrideValuesFromFieldMember;
		}

		private void MigrateButtonDelays(EOSConfig overrideValuesFromFieldMember, OverrideableConfigValues mainOverrideableConfig)
		{
			initialButtonDelayForOverlay = SelectValue(overrideValuesFromFieldMember.initialButtonDelayForOverlay.GetValueOrDefault(), mainOverrideableConfig.initialButtonDelayForOverlay.GetValueOrDefault());
			repeatButtonDelayForOverlay = SelectValue(overrideValuesFromFieldMember.repeatButtonDelayForOverlay.GetValueOrDefault(), mainOverrideableConfig.repeatButtonDelayForOverlay.GetValueOrDefault());
		}

		private void MigrateThreadAffinity(EOSConfig overrideValuesFromFieldMember, OverrideableConfigValues mainOverrideableConfig)
		{
			if (threadAffinity == null)
			{
				threadAffinity = new WrappedInitializeThreadAffinity();
			}
			threadAffinity.NetworkWork = SelectValue(overrideValuesFromFieldMember.ThreadAffinity_networkWork, mainOverrideableConfig.ThreadAffinity_networkWork).GetValueOrDefault();
			threadAffinity.StorageIo = SelectValue(overrideValuesFromFieldMember.ThreadAffinity_storageIO, mainOverrideableConfig.ThreadAffinity_storageIO).GetValueOrDefault();
			threadAffinity.WebSocketIo = SelectValue(overrideValuesFromFieldMember.ThreadAffinity_webSocketIO, mainOverrideableConfig.ThreadAffinity_webSocketIO).GetValueOrDefault();
			threadAffinity.P2PIo = SelectValue(overrideValuesFromFieldMember.ThreadAffinity_P2PIO, mainOverrideableConfig.ThreadAffinity_P2PIO).GetValueOrDefault();
			threadAffinity.HttpRequestIo = SelectValue(overrideValuesFromFieldMember.ThreadAffinity_HTTPRequestIO, mainOverrideableConfig.ThreadAffinity_HTTPRequestIO).GetValueOrDefault();
			threadAffinity.RTCIo = SelectValue(overrideValuesFromFieldMember.ThreadAffinity_RTCIO, mainOverrideableConfig.ThreadAffinity_RTCIO).GetValueOrDefault();
		}

		private void MigrateOverrideableConfigValues(EOSConfig overrideValuesFromFieldMember, OverrideableConfigValues mainOverrideableConfig)
		{
			platformOptionsFlags |= overrideValuesFromFieldMember.platformOptionsFlags;
			MigrateButtonDelays(overrideValuesFromFieldMember, mainOverrideableConfig);
			MigrateThreadAffinity(overrideValuesFromFieldMember, mainOverrideableConfig);
		}

		protected virtual void MigrateNonOverrideableConfigValues(EOSConfig overrideValuesFromFieldMember, NonOverrideableConfigValues mainNonOverrideableConfig)
		{
			authScopeOptionsFlags = mainNonOverrideableConfig.authScopeOptionsFlags;
			authScopeOptionsFlags |= AuthScopeFlags.BasicProfile;
			authScopeOptionsFlags |= AuthScopeFlags.FriendsList;
			authScopeOptionsFlags |= AuthScopeFlags.Presence;
			tickBudgetInMilliseconds = mainNonOverrideableConfig.tickBudgetInMilliseconds;
			taskNetworkTimeoutSeconds = mainNonOverrideableConfig.taskNetworkTimeoutSeconds;
			alwaysSendInputToOverlay = mainNonOverrideableConfig.alwaysSendInputToOverlay;
			MigratePlatformFlags(overrideValuesFromFieldMember, mainNonOverrideableConfig);
			if (mainNonOverrideableConfig.integratedPlatformManagementFlags == (IntegratedPlatformManagementFlags)0 || mainNonOverrideableConfig.integratedPlatformManagementFlags == IntegratedPlatformManagementFlags.Disabled)
			{
				integratedPlatformManagementFlags = GetDefaultIntegratedPlatformManagementFlags();
			}
			else
			{
				integratedPlatformManagementFlags = mainNonOverrideableConfig.integratedPlatformManagementFlags;
			}
			ProductConfig productConfig = Config.Get<ProductConfig>();
			string text = mainNonOverrideableConfig.deploymentID?.ToLower();
			if (!string.IsNullOrEmpty(text))
			{
				Deployment? deployment = null;
				foreach (Named<Deployment> deployment2 in productConfig.Environments.Deployments)
				{
					if (text.Equals(deployment2.Value.DeploymentId.ToString("N").ToLowerInvariant()))
					{
						deployment = deployment2.Value;
						break;
					}
				}
				if (deployment.HasValue)
				{
					this.deployment = deployment.Value;
				}
				else
				{
					Debug.LogWarning("The previous config explicitly identified '" + text + "' as the Deployment GUID, but could not find a deployment with that id in the config. You must set the Deployment in the EOS Configuration window.");
				}
			}
			else if (productConfig.Environments.Deployments.Count == 1)
			{
				this.deployment = productConfig.Environments.Deployments[0].Value;
				Debug.Log("The previous config did not explicitly define a deployment. There was one defined deployment, automatically selecting deployment name '" + productConfig.Environments.Deployments[0].Name + "' " + $"with id '{this.deployment.DeploymentId}'.");
			}
			else
			{
				Debug.LogWarning("The previous config did not explicitly define a deployment for this platform, and there are more than one available deployments. You must set the Deployment inthe EOS Configuration window for this platform.");
			}
			string text2 = mainNonOverrideableConfig.clientID?.ToLowerInvariant();
			if (!string.IsNullOrEmpty(text2))
			{
				EOSClientCredentials eOSClientCredentials = null;
				foreach (Named<EOSClientCredentials> client in productConfig.Clients)
				{
					if (text2.Equals(client.Value.ClientId.ToLowerInvariant()))
					{
						eOSClientCredentials = client.Value;
						break;
					}
				}
				if (eOSClientCredentials != null)
				{
					clientCredentials = eOSClientCredentials;
				}
				else
				{
					Debug.LogWarning("The previous config explicitly identified '" + text2 + "' as the Client ID, but could not find client credentials with that id in the config. You must set the Client Credentials in the EOS Configuration window.");
				}
			}
			else if (productConfig.Clients.Count == 1)
			{
				clientCredentials = productConfig.Clients[0].Value;
				Debug.Log("The previous config did not explicitly define client credentials. There was one defined client credential, automatically selecting client credential name '" + productConfig.Clients[0].Name + "' with id '" + clientCredentials.ClientId + "'.");
			}
			else
			{
				Debug.LogWarning("The previous config did not explicitly define client credentials for this platform, and there are more than one available client credentials. You must set the Client Credentials in the EOS Configuration window for this platform.");
			}
		}

		protected virtual void MigratePlatformFlags(EOSConfig overrideValuesFromFieldMember, NonOverrideableConfigValues mainNonOverrideableConfig)
		{
			WrappedPlatformFlags wrappedPlatformFlags = mainNonOverrideableConfig.platformOptionsFlags;
			if (overrideValuesFromFieldMember != null)
			{
				wrappedPlatformFlags |= overrideValuesFromFieldMember.platformOptionsFlags;
			}
			WrappedPlatformFlags wrappedPlatformFlags2 = WrappedPlatformFlags.None;
			foreach (WrappedPlatformFlags item in EnumUtility<WrappedPlatformFlags>.GetEnumerator(wrappedPlatformFlags))
			{
				switch (item)
				{
				case WrappedPlatformFlags.None:
				case WrappedPlatformFlags.LoadingInEditor:
				case WrappedPlatformFlags.DisableOverlay:
				case WrappedPlatformFlags.DisableSocialOverlay:
				case WrappedPlatformFlags.Reserved1:
					wrappedPlatformFlags2 |= item;
					break;
				case WrappedPlatformFlags.WindowsEnableOverlayD3D9:
				case WrappedPlatformFlags.WindowsEnableOverlayD3D10:
				case WrappedPlatformFlags.WindowsEnableOverlayOpengl:
					if (Platform == PlatformManager.Platform.Windows)
					{
						wrappedPlatformFlags2 |= item;
					}
					break;
				case WrappedPlatformFlags.ConsoleEnableOverlayAutomaticUnloading:
					if (Platform == PlatformManager.Platform.Console)
					{
						wrappedPlatformFlags2 |= item;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			platformOptionsFlags = wrappedPlatformFlags2;
		}

		protected override void MigrateConfig()
		{
			if (null != overrideValues)
			{
				OverrideableConfigValues mainOverrideableConfig = Config.Get<OverrideableConfigValues>();
				MigrateOverrideableConfigValues(overrideValues, mainOverrideableConfig);
			}
			NonOverrideableConfigValues mainNonOverrideableConfig = Config.Get<NonOverrideableConfigValues>();
			MigrateNonOverrideableConfigValues(overrideValues, mainNonOverrideableConfig);
			Debug.LogWarning("Configuration values for " + GetType().Name + " have been migrated. Please double check your configuration in EOS Plugin -> EOS Configuration to make sure that the migration was successful.");
		}

		public virtual IntegratedPlatformManagementFlags GetDefaultIntegratedPlatformManagementFlags()
		{
			return IntegratedPlatformManagementFlags.Disabled;
		}
	}
}

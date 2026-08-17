using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	[ConfigGroup("EOS Config", new string[] { "Product Information", "Deployment", "Client Authentication", "Flags", "Thread Affinity & Tick Budgets", "Overlay Options" }, false)]
	[Obsolete("EOSConfig is obsolete. It has been replaced by PlatformConfig and ProductConfig. It remains here for migration purposes.")]
	public class EOSConfig : Config
	{
		[ConfigField("Product Name", ConfigFieldType.Text, "Product name defined in the Development Portal.", 0, null)]
		[NonEmptyStringFieldValidator]
		public string productName;

		[ConfigField("Product Version", ConfigFieldType.Text, "Version of the product.", 0, null)]
		[NonEmptyStringFieldValidator]
		public string productVersion;

		[ConfigField("Product Id", ConfigFieldType.Text, "Product Id defined in the Development Portal.", 0, null)]
		[GUIDFieldValidator]
		public string productID;

		[ConfigField("Sandbox Id", ConfigFieldType.Text, "Sandbox Id to use.", 1, null)]
		[SandboxIDFieldValidator]
		public string sandboxID;

		[ConfigField("Deployment Id", ConfigFieldType.Text, "Deployment Id to use.", 1, null)]
		[GUIDFieldValidator]
		public string deploymentID;

		[ConfigField("Sandbox Deployment Overrides", ConfigFieldType.TextList, "Deployment Id to use.", 1, null)]
		[ExpandField]
		public List<SandboxDeploymentOverride> sandboxDeploymentOverrides;

		[ConfigField("Is Server", ConfigFieldType.Flag, "Indicates whether the application is a dedicated game server.", 1, null)]
		public bool isServer;

		[ConfigField("Client Secret", ConfigFieldType.Text, "Client Secret defined in the Development Portal.", 2, null)]
		public string clientSecret;

		[ConfigField("Client Id", ConfigFieldType.Text, "Client Id defined in the Development Portal.", 2, null)]
		public string clientID;

		[ConfigField("Encryption Key", ConfigFieldType.Text, "Encryption key to use for client authentication.", 2, null)]
		public string encryptionKey;

		[ConfigField("Generate Key", ConfigFieldType.Button, "Click to generate an encryption key.", 2, null)]
		private Action GenerateKeyButtonAction;

		[ConfigField("Platform Options", ConfigFieldType.TextList, "Platform option flags", 3, null)]
		[JsonConverter(typeof(ListOfStringsToPlatformFlags))]
		public WrappedPlatformFlags platformOptionsFlags;

		[ConfigField("Auth Scope Options", ConfigFieldType.TextList, "Platform option flags", 3, null)]
		[JsonConverter(typeof(ListOfStringsToAuthScopeFlags))]
		public AuthScopeFlags authScopeOptionsFlags;

		[ConfigField("Tick Budget (ms)", ConfigFieldType.Uint, "Used to define the maximum amount of execution time the EOS SDK can use each frame.", 3, null)]
		public uint tickBudgetInMilliseconds;

		[ConfigField("Network Timeout Seconds", ConfigFieldType.Double, "Indicates the maximum number of seconds that EOS SDK will allow network calls to run before failing with EOS_TimedOut.", 3, null)]
		public double taskNetworkTimeoutSeconds;

		[ConfigField("Network Work", ConfigFieldType.Ulong, "Specifies affinity for threads that manage network tasks that are not IO related.", 3, null)]
		[JsonConverter(typeof(StringToTypeConverter<ulong>))]
		public ulong? ThreadAffinity_networkWork;

		[ConfigField("Storage IO", ConfigFieldType.Ulong, "Specifies affinity for threads that generate storage IO.", 3, null)]
		[JsonConverter(typeof(StringToTypeConverter<ulong>))]
		public ulong? ThreadAffinity_storageIO;

		[ConfigField("Web Socket IO", ConfigFieldType.Ulong, "Specifies affinity for threads that generate web socket IO.", 3, null)]
		[JsonConverter(typeof(StringToTypeConverter<ulong>))]
		public ulong? ThreadAffinity_webSocketIO;

		[ConfigField("P2P IO", ConfigFieldType.Ulong, "Specifies affinity for any thread that will generate IO related to P2P traffic and management.", 3, null)]
		[JsonConverter(typeof(StringToTypeConverter<ulong>))]
		public ulong? ThreadAffinity_P2PIO;

		[ConfigField("HTTP Request IO", ConfigFieldType.Ulong, "Specifies the affinity for any thread that will generate HTTP request IO.", 3, null)]
		[JsonConverter(typeof(StringToTypeConverter<ulong>))]
		public ulong? ThreadAffinity_HTTPRequestIO;

		[ConfigField("RTC IO", ConfigFieldType.Ulong, "Specifies the affinity for any thread that will generate IO related to RTC traffic and management.", 3, null)]
		[JsonConverter(typeof(StringToTypeConverter<ulong>))]
		public ulong? ThreadAffinity_RTCIO;

		[ConfigField("Always Send Input to Overlay", ConfigFieldType.Flag, "If true, the plugin will always send input to the overlay from the C# side to native, and handle showing the overlay. This doesn't always mean input makes it to the EOS SDK.", 4, null)]
		public bool alwaysSendInputToOverlay;

		[ConfigField("Initial Button Delay", ConfigFieldType.Float, "Initial Button Delay (if not set, whatever the default is will be used).", 4, null)]
		[JsonConverter(typeof(StringToTypeConverter<float>))]
		public float? initialButtonDelayForOverlay;

		[ConfigField("Repeat Button Delay", ConfigFieldType.Text, "Repeat button delay for the overlay. If not set, whatever the default is will be used.", 4, null)]
		[JsonConverter(typeof(StringToTypeConverter<float>))]
		public float? repeatButtonDelayForOverlay;

		[JsonConverter(typeof(ListOfStringsToInputStateButtonFlags))]
		public InputStateButtonFlags toggleFriendsButtonCombination = InputStateButtonFlags.SpecialLeft;

		static EOSConfig()
		{
			Config.RegisterFactory(() => new EOSConfig());
		}

		protected EOSConfig()
			: base("EpicOnlineServicesConfig.json")
		{
		}

		protected override bool NeedsMigration()
		{
			return false;
		}

		protected override Task EnsureConfigFileExistsAsync()
		{
			return Task.CompletedTask;
		}

		public void SetDeployment(string launcherSandboxId)
		{
			if (TryGetDeployment(sandboxDeploymentOverrides, launcherSandboxId, out var deploymentOverride))
			{
				Debug.Log("Sandbox ID overridden to: \"" + deploymentOverride.sandboxID + "\".");
				Debug.Log("Deployment ID overridden to: \"" + deploymentOverride.deploymentID + "\".");
				sandboxID = deploymentOverride.sandboxID;
				deploymentID = deploymentOverride.deploymentID;
			}
			else if (sandboxID != launcherSandboxId)
			{
				throw new Exception("The launcher sandboxId \"" + launcherSandboxId + "\" does not have a corresponding deploymentId configured.");
			}
		}

		private static bool TryGetDeployment(List<SandboxDeploymentOverride> deploymentOverrides, string sandboxId, out SandboxDeploymentOverride deploymentOverride)
		{
			deploymentOverride = null;
			foreach (SandboxDeploymentOverride deploymentOverride2 in deploymentOverrides)
			{
				if (!(deploymentOverride2.sandboxID != sandboxId))
				{
					deploymentOverride = deploymentOverride2;
					return true;
				}
			}
			return false;
		}

		public void ConfigureOverrideThreadAffinity(ref InitializeThreadAffinity affinity)
		{
			if (ThreadAffinity_HTTPRequestIO.HasValue)
			{
				affinity.HttpRequestIo = ThreadAffinity_HTTPRequestIO.Value;
			}
			if (ThreadAffinity_P2PIO.HasValue)
			{
				affinity.P2PIo = ThreadAffinity_P2PIO.Value;
			}
			if (ThreadAffinity_RTCIO.HasValue)
			{
				affinity.RTCIo = ThreadAffinity_RTCIO.Value;
			}
			if (ThreadAffinity_networkWork.HasValue)
			{
				affinity.NetworkWork = ThreadAffinity_networkWork.Value;
			}
			if (ThreadAffinity_storageIO.HasValue)
			{
				affinity.StorageIo = ThreadAffinity_storageIO.Value;
			}
			if (ThreadAffinity_webSocketIO.HasValue)
			{
				affinity.WebSocketIo = ThreadAffinity_webSocketIO.Value;
			}
		}
	}
}

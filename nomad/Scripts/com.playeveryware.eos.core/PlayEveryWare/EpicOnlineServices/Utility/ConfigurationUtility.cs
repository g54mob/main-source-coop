using System;
using Epic.OnlineServices.Platform;
using PlayEveryWare.EpicOnlineServices.Extensions;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices.Utility
{
	public static class ConfigurationUtility
	{
		public static EOSCreateOptions GetEOSCreateOptions()
		{
			PlatformConfig platformConfig = PlatformManager.GetPlatformConfig();
			ProductConfig productConfig = Config.Get<ProductConfig>();
			IPlatformSpecifics instance = EOSManagerPlatformSpecificsSingleton.Instance;
			EOSCreateOptions createOptions = new EOSCreateOptions();
			createOptions.options.CacheDirectory = instance.GetTempDir();
			createOptions.options.IsServer = platformConfig.isServer;
			createOptions.options.Flags = platformConfig.platformOptionsFlags.Unwrap();
			if (platformConfig.clientCredentials == null || !platformConfig.clientCredentials.IsEncryptionKeyValid())
			{
				Debug.LogError("The encryption key used for the selected client credentials is invalid. Please see your platform configuration.");
			}
			else
			{
				createOptions.options.EncryptionKey = platformConfig.clientCredentials.EncryptionKey;
			}
			createOptions.options.OverrideCountryCode = null;
			createOptions.options.OverrideLocaleCode = null;
			createOptions.options.ProductId = productConfig.ProductId.ToString("N").ToLowerInvariant();
			createOptions.options.SandboxId = platformConfig.deployment.SandboxId.ToString();
			createOptions.options.DeploymentId = platformConfig.deployment.DeploymentId.ToString("N").ToLowerInvariant();
			createOptions.options.TickBudgetInMilliseconds = platformConfig.tickBudgetInMilliseconds;
			createOptions.options.TaskNetworkTimeoutSeconds = ((platformConfig.taskNetworkTimeoutSeconds > 0.0) ? new double?(platformConfig.taskNetworkTimeoutSeconds) : ((double?)null));
			createOptions.options.ClientCredentials = new ClientCredentials
			{
				ClientId = platformConfig.clientCredentials.ClientId,
				ClientSecret = platformConfig.clientCredentials.ClientSecret
			};
			instance.ConfigureSystemPlatformCreateOptions(ref createOptions);
			return createOptions;
		}

		public static EOSInitializeOptions GetEOSInitializeOptions()
		{
			EOSInitializeOptions initializeOptions = new EOSInitializeOptions
			{
				options = default(InitializeOptions)
			};
			ProductConfig productConfig = Config.Get<ProductConfig>();
			PlatformConfig platformConfig = PlatformManager.GetPlatformConfig();
			initializeOptions.options.ProductName = productConfig.ProductName;
			initializeOptions.options.ProductVersion = productConfig.ProductVersion;
			initializeOptions.options.OverrideThreadAffinity = platformConfig.threadAffinity?.Unwrap();
			initializeOptions.options.AllocateMemoryFunction = IntPtr.Zero;
			initializeOptions.options.ReallocateMemoryFunction = IntPtr.Zero;
			initializeOptions.options.ReleaseMemoryFunction = IntPtr.Zero;
			EOSManagerPlatformSpecificsSingleton.Instance.ConfigureSystemInitOptions(ref initializeOptions);
			return initializeOptions;
		}
	}
}

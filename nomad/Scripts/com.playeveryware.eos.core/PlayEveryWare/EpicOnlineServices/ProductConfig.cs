using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayEveryWare.Common;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	[ConfigGroup("Product Configuration", new string[] { "", "Deployment Configuration" }, false)]
	public class ProductConfig : Config
	{
		public sealed class PlatformConfigsUpdatedEventArgs : EventArgs
		{
			public readonly IEnumerable<PlatformManager.Platform> PlatformConfigsUpdated;

			public PlatformConfigsUpdatedEventArgs(IEnumerable<PlatformManager.Platform> platformConfigsUpdated)
			{
				PlatformConfigsUpdated = platformConfigsUpdated;
			}
		}

		internal class PreviousEOSConfig : Config
		{
			public string productName;

			public string productVersion;

			public string productID;

			public List<SandboxDeploymentOverride> sandboxDeploymentOverrides;

			public string sandboxID;

			public string deploymentID;

			public string clientSecret;

			public string clientID;

			public string encryptionKey;

			static PreviousEOSConfig()
			{
				Config.RegisterFactory(() => new PreviousEOSConfig());
			}

			protected PreviousEOSConfig()
				: base("EpicOnlineServicesConfig.json")
			{
			}
		}

		[ConfigField("Product Name", ConfigFieldType.Text, "Enter your product name as it appears in the EOS Dev Portal here.", 0, null)]
		public string ProductName;

		[ConfigField("Product Id", ConfigFieldType.Guid, "Enter your Product Id as it appears in the EOS Dev Portal here.", 0, null)]
		public Guid ProductId;

		[ConfigField("Version", ConfigFieldType.Text, "Use this to indicate to the EOS SDK your game version.", 0, null)]
		public string ProductVersion;

		[JsonProperty("imported")]
		private bool _configImported;

		[ConfigField("Client Credentials", ConfigFieldType.SetOfClientCredentials, "Enter the client credentials you have defined in the Epic Dev Portal.", 1, null)]
		public SetOfNamed<EOSClientCredentials> Clients = new SetOfNamed<EOSClientCredentials>("Client");

		[ConfigField("Production Environments", ConfigFieldType.ProductionEnvironments, "Enter the details of your deployment and sandboxes as they exist within the Epic Dev Portal.", 1, null)]
		public ProductionEnvironments Environments = new ProductionEnvironments();

		[JsonIgnore]
		private bool _deploymentDefinedWhenLoaded;

		[JsonIgnore]
		private bool _clientCredentialsDefinedWhenLoaded;

		public static event EventHandler<PlatformConfigsUpdatedEventArgs> DeploymentsUpdatedEvent;

		public static event EventHandler<PlatformConfigsUpdatedEventArgs> ClientCredentialsUpdatedEvent;

		static ProductConfig()
		{
			Config.RegisterFactory(() => new ProductConfig());
		}

		protected override bool NeedsMigration()
		{
			if (!base.NeedsMigration())
			{
				return !_configImported;
			}
			return true;
		}

		protected ProductConfig()
			: base("eos_product_config.json")
		{
		}

		protected override void OnReadCompleted()
		{
			_deploymentDefinedWhenLoaded = Environments.TryGetFirstDefinedNamedDeployment(out var _);
			_clientCredentialsDefinedWhenLoaded = TryGetFirstCompleteNamedClientCredentials(out var _);
		}

		public bool TryGetFirstCompleteNamedClientCredentials(out Named<EOSClientCredentials> credentials)
		{
			credentials = null;
			foreach (Named<EOSClientCredentials> client in Clients)
			{
				if (client.Value.IsComplete)
				{
					credentials = client;
					break;
				}
			}
			return credentials != null;
		}

		private void MigrateProductNameVersionAndId(PreviousEOSConfig config)
		{
			ProductName = config.productName;
			ProductVersion = config.productVersion;
			if (!string.IsNullOrWhiteSpace(config.productID) && !Guid.TryParse(config.productID, out ProductId))
			{
				Debug.LogWarning("Could not parse product ID.");
			}
		}

		private void MigrateClientCredentials(PreviousEOSConfig config)
		{
			if (!string.IsNullOrWhiteSpace(config.clientID) && !string.IsNullOrWhiteSpace(config.clientSecret))
			{
				Clients.Add(new EOSClientCredentials(config.clientID, config.clientSecret, config.encryptionKey));
			}
		}

		private void MigrateSandboxAndDeployment(PreviousEOSConfig config)
		{
			if (!string.IsNullOrWhiteSpace(config.sandboxID) && !string.IsNullOrEmpty(config.deploymentID))
			{
				SandboxId sandboxId = new SandboxId
				{
					Value = config.sandboxID
				};
				Deployment deployment = new Deployment
				{
					DeploymentId = Guid.Parse(config.deploymentID),
					SandboxId = sandboxId
				};
				if (!Environments.AddDeployment(deployment))
				{
					Debug.LogWarning("Could not import deployment details from old config file. Please reach out for support if you need assistance.");
				}
			}
		}

		private void MigrateSandboxAndDeploymentOverrides(PreviousEOSConfig config)
		{
			if (config.sandboxDeploymentOverrides == null)
			{
				return;
			}
			foreach (SandboxDeploymentOverride sandboxDeploymentOverride in config.sandboxDeploymentOverrides)
			{
				SandboxId sandboxId = new SandboxId
				{
					Value = sandboxDeploymentOverride.sandboxID
				};
				Deployment value = new Deployment
				{
					DeploymentId = Guid.Parse(sandboxDeploymentOverride.deploymentID),
					SandboxId = sandboxId
				};
				Environments.Deployments.Add(value);
			}
		}

		protected override void MigrateConfig()
		{
			if (Environments == null)
			{
				Environments = new ProductionEnvironments();
			}
			PreviousEOSConfig config = Config.Get<PreviousEOSConfig>();
			MigrateProductNameVersionAndId(config);
			MigrateClientCredentials(config);
			MigrateSandboxAndDeployment(config);
			MigrateSandboxAndDeploymentOverrides(config);
			_configImported = true;
		}
	}
}

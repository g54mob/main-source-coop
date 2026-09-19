using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fusion
{
	[Serializable]
	public class NetworkProjectConfig : IConfigurationSanityCheck
	{
		public enum PeerModes
		{
			Single = 0,
			Multiple = 1
		}

		public enum ReplicationFeatures
		{
			None = 0,
			InterestManagement = 2
		}

		public enum FusionHubMode
		{
			None = 0,
			Beginner = 1,
			Advanced = 2
		}

		public enum InvokeRenderModes
		{
			NotInBatchMode = 0,
			Always = 1,
			NotInEditorAsServer = 2,
			NotInBatchModeAndEditorAsServer = 3
		}

		public const string DefaultResourceName = "NetworkProjectConfig";

		public const string CurrentTypeId = "NetworkProjectConfig";

		public const int CurrentVersion = 1;

		[HideInInspector]
		public int Version = 1;

		[HideInInspector]
		public string TypeId = "NetworkProjectConfig";

		[HideInInspector]
		public FusionHubMode HubMode = FusionHubMode.None;

		[Header("Scene Settings")]
		[FormerlySerializedAs("InstanceMode")]
		[InlineHelp]
		public PeerModes PeerMode;

		[InlineHelp]
		[DrawInline]
		[Header("Lag Compensation")]
		public LagCompensationSettings LagCompensation = new LagCompensationSettings();

		[InlineHelp]
		[Header("Physics")]
		public bool PhysicsForecast = false;

		[NonSerialized]
		public bool DoNotSetForecastDisabledProxyRigidbodiesToKinematic = false;

		[Space]
		[Header("Miscellaneous")]
		[InlineHelp]
		[ToggleLeft]
		public bool EnqueueIncompleteSynchronousSpawns;

		[InlineHelp]
		[FormerlySerializedAs("InvokeRenderInBatchMode")]
		public InvokeRenderModes InvokeRenderMode = InvokeRenderModes.Always;

		[InlineHelp]
		[ToggleLeft]
		public bool NetworkIdIsObjectName;

		[InlineHelp]
		[ToggleLeft]
		public bool HideNetworkObjectInactivityGuard = false;

		[InlineHelp]
		[ToggleLeft]
		public bool AllowClientServerModesInWebGL = false;

		[InlineHelp]
		[ToggleLeft]
		public bool AllowMasterClientReassignRequest = false;

		[InlineHelp]
		[DrawIf("AllowMasterClientReassignRequest", true, CompareOperator.Equal, DrawIfMode.Hide)]
		public float MasterClientRequestReassignThreshold = 2f;

		[InlineHelp]
		[ToggleLeft]
		public bool ClientsRecordFrameAndPacketTimingTraces = false;

		[NonSerialized]
		public NetworkPrefabTable PrefabTable = new NetworkPrefabTable();

		[DrawInline]
		[Header("Simulation")]
		public SimulationConfig Simulation = new SimulationConfig();

		[DrawInline]
		[Header("Time")]
		public TimeSyncConfiguration TimeSync = new TimeSyncConfiguration();

		[DrawInline]
		[Header("Network")]
		public NetworkConfiguration Network = new NetworkConfiguration();

		[DrawInline]
		[Header("Host Migration")]
		public HostMigrationConfig HostMigration = new HostMigrationConfig();

		[DrawInline]
		[Header("Encryption")]
		public EncryptionConfig EncryptionConfig = new EncryptionConfig();

		[DrawInline]
		[Header("NetworkConditions")]
		public NetworkSimulationConfiguration NetworkConditions = new NetworkSimulationConfiguration();

		[DrawInline]
		[Header("Heap")]
		public HeapConfiguration Heap = new HeapConfiguration();

		[Header("Weaver Settings")]
		[AssemblyName(RequiresUnsafeCode = true)]
		[InlineHelp]
		public string[] AssembliesToWeave = new string[2] { "Assembly-CSharp", "Assembly-CSharp-firstpass" };

		[InlineHelp]
		[ToggleLeft]
		public bool UseSerializableDictionary = true;

		[InlineHelp]
		[ToggleLeft]
		public bool NullChecksForNetworkedProperties = true;

		[InlineHelp]
		[ToggleLeft]
		public bool CheckRpcAttributeUsage = false;

		[InlineHelp]
		[ToggleLeft]
		public bool CheckNetworkedPropertiesBeingEmpty = false;

		[NonSerialized]
		internal readonly Dictionary<Type, int> ExecutionOrderOverrides = new Dictionary<Type, int>();

		public static NetworkProjectConfig Global => NetworkProjectConfigAsset.Global.Config;

		[Obsolete("Use InvokeRenderMode instead")]
		public bool InvokeRenderInBatchMode
		{
			get
			{
				InvokeRenderModes invokeRenderMode = InvokeRenderMode;
				if ((uint)(invokeRenderMode - 1) <= 1u)
				{
					return true;
				}
				return false;
			}
		}

		public static (NetworkRunner.BuildTypes, FileVersionInfo) FusionVersionInfo
		{
			get
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					string fullName = assembly.FullName;
					if (fullName.StartsWith("Fusion.Runtime,"))
					{
						return (NetworkRunner.BuildType, FileVersionInfo.GetVersionInfo(assembly.Location));
					}
				}
				return ((NetworkRunner.BuildTypes)(-1), null);
			}
		}

		public static void UnloadGlobal()
		{
			NetworkProjectConfigAsset.UnloadGlobal();
		}

		public int? GetExecutionOrder(Type type)
		{
			if (ExecutionOrderOverrides.TryGetValue(type, out var value))
			{
				return value;
			}
			while (type != typeof(object))
			{
				DefaultExecutionOrder customAttribute = type.GetCustomAttribute<DefaultExecutionOrder>();
				if (customAttribute != null)
				{
					return customAttribute.order;
				}
				type = type.BaseType;
			}
			return null;
		}

		internal NetworkProjectConfig Init(int globalSize, int? playerCountOverride, int? inputWordCount)
		{
			NetworkProjectConfig networkProjectConfig = Copy();
			networkProjectConfig.Heap = networkProjectConfig.Heap.Init(globalSize);
			networkProjectConfig.Network = networkProjectConfig.Network.Init();
			networkProjectConfig.Simulation = networkProjectConfig.Simulation.Init(playerCountOverride, inputWordCount);
			return networkProjectConfig;
		}

		internal NetworkProjectConfig Copy()
		{
			NetworkProjectConfig networkProjectConfig = (NetworkProjectConfig)MemberwiseClone();
			networkProjectConfig.Simulation = Simulation.Copy();
			return networkProjectConfig;
		}

		public void SanityCheck()
		{
			Simulation.SanityCheck();
			TimeSync.SanityCheck();
			Network.SanityCheck();
			Heap.SanityCheck();
			NetworkConditions.SanityCheck();
		}

		public override string ToString()
		{
			return Serialize(this);
		}

		public static string Serialize(NetworkProjectConfig config)
		{
			return JsonUtility.ToJson(config);
		}

		public static NetworkProjectConfig Deserialize(string data)
		{
			return JsonUtility.FromJson<NetworkProjectConfig>(data);
		}

		internal static string SerializeMinimal(NetworkProjectConfig config)
		{
			return JsonUtils.RemoveExtraReferences(Serialize(config));
		}
	}
}

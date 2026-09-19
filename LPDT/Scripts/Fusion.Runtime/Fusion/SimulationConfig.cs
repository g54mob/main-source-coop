using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fusion
{
	[Serializable]
	public class SimulationConfig : IConfigurationSanityCheck
	{
		public enum InputTransferModes
		{
			Redundancy = 0,
			RedundancyUncompressed = 2,
			LatestState = 1
		}

		public enum DataConsistency
		{
			Full = 0,
			Eventual = 1
		}

		public enum SimulationTimeMode
		{
			UnscaledDeltaTime = 0,
			DeltaTime = 1
		}

		[HideInInspector]
		[InlineHelp]
		public int InputDataWordCount;

		[InlineHelp]
		public NetworkProjectConfig.ReplicationFeatures ReplicationFeatures = NetworkProjectConfig.ReplicationFeatures.None;

		[DrawIf("ReplicationFeatures", 2L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public InterestManagementConfig InterestManagementConfig = new InterestManagementConfig();

		[FormerlySerializedAs("inputTransferMode")]
		[InlineHelp]
		public InputTransferModes InputTransferMode;

		[NonSerialized]
		public DataConsistency ObjectDataConsistency;

		[InlineHelp]
		public SimulationTimeMode SimulationUpdateTimeMode = SimulationTimeMode.UnscaledDeltaTime;

		[FormerlySerializedAs("DefaultPlayerCount")]
		[FormerlySerializedAs("DefaultPlayers")]
		[FormerlySerializedAs("Players")]
		[Unit(Units.None)]
		[InlineHelp]
		[RangeEx(1.0, 255.0)]
		public int PlayerCount = 10;

		[InlineHelp]
		public TickRate.Selection TickRateSelection = TickRate.Zero;

		[NonSerialized]
		public Topologies Topology;

		[NonSerialized]
		public bool HostMigration;

		internal bool EnableSerializers = true;

		public bool AreaOfInterestEnabled => (ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement;

		public int InputTotalWordCount => InputDataWordCount + 4;

		internal SimulationConfig Init(int? playerCountOverride, int? inputWordCount)
		{
			SimulationConfig simulationConfig = Copy();
			if (playerCountOverride.HasValue)
			{
				simulationConfig.PlayerCount = playerCountOverride.Value;
			}
			if (inputWordCount.HasValue)
			{
				simulationConfig.InputDataWordCount = inputWordCount.Value;
			}
			return simulationConfig;
		}

		internal SimulationConfig Copy()
		{
			return (SimulationConfig)MemberwiseClone();
		}

		public void SanityCheck()
		{
		}
	}
}

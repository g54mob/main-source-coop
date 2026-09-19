using System.Collections.Generic;

namespace Fusion
{
	internal readonly struct SimulationBehaviourMeta
	{
		public readonly SimulationStages Stages;

		public readonly SimulationModes Modes;

		public readonly Topologies Topologies;

		public readonly int? StaticWordCount;

		public readonly Dictionary<string, NetworkPropertyMeta> Properties;

		public SimulationBehaviourMeta(SimulationStages stages, SimulationModes modes, Topologies topologies, int? staticWordCount)
		{
			Stages = stages;
			Modes = modes;
			Topologies = topologies;
			StaticWordCount = staticWordCount;
			Properties = new Dictionary<string, NetworkPropertyMeta>();
		}
	}
}

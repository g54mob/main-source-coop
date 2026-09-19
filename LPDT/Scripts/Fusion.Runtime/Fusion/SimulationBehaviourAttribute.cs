using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class SimulationBehaviourAttribute : Attribute
	{
		internal const SimulationStages ALL_STAGES = SimulationStages.Forward | SimulationStages.Resimulate;

		internal const SimulationModes ALL_MODES = SimulationModes.Server | SimulationModes.Host | SimulationModes.Client;

		internal const Topologies ALL_TOPOLOGIES = Topologies.ClientServer | Topologies.Shared;

		public SimulationStages Stages { get; set; } = SimulationStages.Forward | SimulationStages.Resimulate;

		public SimulationModes Modes { get; set; } = SimulationModes.Server | SimulationModes.Host | SimulationModes.Client;

		public Topologies Topologies { get; set; } = Topologies.ClientServer | Topologies.Shared;
	}
}

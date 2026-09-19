using System;

namespace Fusion
{
	internal class SimulationPacketEnvelope
	{
		public Tick Tick;

		public PooledList<NetworkObjectPacketData> Objects;

		public PooledList<SimulationMessagePacketData> Messages;

		public int RefCount;

		internal void PoolInit(Simulation simulation, Tick tick)
		{
			RefCount = 1;
			Tick = tick;
			Objects = simulation.AcquireNetworkObjectPacketDataList(64);
			Messages = simulation.AcquireSimulationMessagePacketDataList(0);
		}

		internal void PoolReset(Simulation simulation)
		{
			Assert.Always(RefCount == 0, "RefCount == 0");
			Tick = default(Tick);
			Span<SimulationMessagePacketData> span = Messages.AsSpan();
			for (int i = 0; i < span.Length; i++)
			{
				SimulationMessagePacketData simulationMessagePacketData = span[i];
				simulation.ReleaseReference(simulationMessagePacketData.Message);
			}
			simulation.Release(ref Objects);
			simulation.Release(ref Messages);
		}
	}
}

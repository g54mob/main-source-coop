using System;

namespace Fusion
{
	[Flags]
	public enum TraceChannels
	{
		Global = 1,
		Stun = 2,
		Object = 4,
		Network = 8,
		Prefab = 0x10,
		SceneInfo = 0x20,
		SceneManager = 0x40,
		SimulationMessage = 0x80,
		HostMigration = 0x100,
		Encryption = 0x200,
		DummyTraffic = 0x400,
		Realtime = 0x800,
		MemoryTrack = 0x1000,
		AreaOfInterest = 0x2000,
		Snapshots = 0x4000,
		Time = 0x8000,
		SendRecv = 0x10000,
		Pools = 0x20000,
		Parent = 0x40000,
		Forecast = 0x80000,
		AutomatedRun = 0x100000,
		PluginVersion = 0x200000
	}
}

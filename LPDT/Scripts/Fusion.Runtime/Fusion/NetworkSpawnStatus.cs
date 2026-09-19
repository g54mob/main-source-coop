namespace Fusion
{
	public enum NetworkSpawnStatus
	{
		Queued = 0,
		Spawned = 1,
		FailedToLoadPrefabSynchronously = 2,
		FailedToCreateInstance = 3,
		FailedClientCantSpawn = 4,
		FailedLocalPlayerNotYetSet = 5,
		FailedSimulationNotRunning = 6
	}
}

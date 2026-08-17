using FishNet.Documenting;

namespace FishNet.Transporting
{
	[APIExclude]
	public enum PacketId : ushort
	{
		Unset = 0,
		Authenticated = 1,
		Split = 2,
		ObjectSpawn = 3,
		ObjectDespawn = 4,
		PredictedSpawnResult = 5,
		SyncVar = 7,
		ServerRpc = 8,
		ObserversRpc = 9,
		TargetRpc = 10,
		OwnershipChange = 11,
		Broadcast = 12,
		SyncObject = 13,
		PingPong = 14,
		Replicate = 15,
		Reconcile = 16,
		Disconnect = 17,
		TimingUpdate = 18,
		NetworkLODUpdate = 19,
		StateUpdate = 20
	}
}

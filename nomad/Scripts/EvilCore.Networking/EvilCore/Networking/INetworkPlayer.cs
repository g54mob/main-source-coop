using UnityEngine;

namespace EvilCore.Networking
{
	public interface INetworkPlayer
	{
		uint NetId { get; }

		string DisplayName { get; }

		int ConnectionId { get; }

		ushort PingMs { get; }

		string EosProductUserId { get; }

		void ServerSetPing(ushort ping);

		void ServerSetEosProductUserId(string id);

		void ServerApplyInitialSpawnNearHost(Vector3 position);
	}
}

using System;

namespace Fusion
{
	public interface INetworkObjectProvider
	{
		NetworkObjectAcquireResult AcquireInstance(NetworkRunner runner, in NetworkObjectAcquireContext context, out NetworkObject result);

		[Obsolete("Use AcquireInstance((NetworkRunner runner, in NetworkObjectAcquireContext context, out NetworkObject result) instead.")]
		NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject result)
		{
			throw new NotSupportedException();
		}

		void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context);

		NetworkPrefabId GetPrefabId(NetworkRunner runner, NetworkObjectGuid prefabGuid)
		{
			return runner.Prefabs.GetId(prefabGuid);
		}

		NetworkPrefabId GetPrefabId(NetworkRunner runner, string prefabName)
		{
			throw new NotImplementedException();
		}

		void Shutdown(NetworkRunner networkRunner)
		{
		}

		void Initialize(NetworkRunner networkRunner)
		{
		}
	}
}

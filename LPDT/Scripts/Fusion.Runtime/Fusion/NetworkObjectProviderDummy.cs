using System;

namespace Fusion
{
	public class NetworkObjectProviderDummy : INetworkObjectProvider
	{
		public NetworkObjectAcquireResult AcquireInstance(NetworkRunner runner, in NetworkObjectAcquireContext context, out NetworkObject instance)
		{
			throw new NotImplementedException();
		}

		public void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
		{
			throw new NotImplementedException();
		}

		public NetworkPrefabId GetPrefabId(NetworkRunner runner, NetworkObjectGuid prefabGuid)
		{
			throw new NotImplementedException();
		}

		NetworkObjectAcquireResult INetworkObjectProvider.AcquireInstance(NetworkRunner runner, in NetworkObjectAcquireContext context, out NetworkObject result)
		{
			return AcquireInstance(runner, in context, out result);
		}

		void INetworkObjectProvider.ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
		{
			ReleaseInstance(runner, in context);
		}
	}
}

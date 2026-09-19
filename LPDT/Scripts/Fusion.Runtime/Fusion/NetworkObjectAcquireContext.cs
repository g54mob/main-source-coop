using System;

namespace Fusion
{
	public readonly struct NetworkObjectAcquireContext
	{
		public readonly NetworkObjectTypeId TypeId;

		public readonly NetworkObjectMeta Meta;

		public readonly bool IsSynchronous;

		public readonly bool DontDestroyOnLoad;

		public readonly NetworkObject AttachableInstance;

		public bool HasHeader => Meta != null;

		public ref NetworkObjectHeader Header => ref Meta.Header;

		public ReadOnlySpan<int> Data => Meta.Data;

		public NetworkPrefabId? PrefabId => TypeId.IsPrefab ? new NetworkPrefabId?(TypeId.AsPrefabId) : ((NetworkPrefabId?)null);

		public NetworkObjectAcquireContext(NetworkObjectTypeId typeId, NetworkObjectMeta meta = null, bool isSynchronous = true, bool dontDestroyOnLoad = false, NetworkObject attachableInstance = null)
		{
			TypeId = typeId;
			Meta = meta;
			IsSynchronous = isSynchronous;
			DontDestroyOnLoad = dontDestroyOnLoad;
			AttachableInstance = attachableInstance;
		}
	}
}

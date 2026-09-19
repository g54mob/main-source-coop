using System;

namespace Fusion
{
	[Obsolete("Use NetworkObjectAcquireContext instead. INetworkObjectProvider.AcquirePrefabInstance needs to be renamed to AcquireInstance as well. NetworkObjectAcquireContext also contains AttachableInstance, which will be set to an instance passed in NetworkRunner.RegisterSceneObjects.")]
	public readonly struct NetworkPrefabAcquireContext
	{
		public readonly NetworkPrefabId PrefabId;

		public readonly NetworkObjectMeta Meta;

		public readonly bool IsSynchronous;

		public readonly bool DontDestroyOnLoad;

		public bool HasHeader => Meta != null;

		public Span<int> Data => (Meta != null) ? Meta.Data : default(Span<int>);

		public NetworkPrefabAcquireContext(in NetworkObjectAcquireContext context)
		{
			PrefabId = context.PrefabId.Value;
			Meta = context.Meta;
			IsSynchronous = context.IsSynchronous;
			DontDestroyOnLoad = context.DontDestroyOnLoad;
		}
	}
}

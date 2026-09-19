using System;

namespace Fusion
{
	public readonly struct NetworkObjectReleaseContext
	{
		public readonly NetworkObject Object;

		public readonly NetworkObjectTypeId TypeId;

		public readonly bool IsBeingDestroyed;

		[Obsolete("Use TypeId instead")]
		public bool IsNestedObject => TypeId.IsPrefabNestedObject;

		public NetworkObjectReleaseContext(NetworkObject obj, NetworkObjectTypeId typeId, bool isBeingDestroyed)
		{
			Object = obj;
			IsBeingDestroyed = isBeingDestroyed;
			TypeId = typeId;
		}

		public override string ToString()
		{
			return $"[{Object}, TypeId={TypeId}, IsBeingDestroyed={IsBeingDestroyed}]";
		}
	}
}

using System.Collections.Generic;
using Mirror;

namespace NomadDrive.Features.SaveSystem
{
	public static class PersistentIdRegistry
	{
		private static readonly Dictionary<string, PersistentId> ByGuid = new Dictionary<string, PersistentId>();

		public static void Register(PersistentId persistentId)
		{
			if (persistentId != null && persistentId.HasGuid)
			{
				ByGuid[persistentId.Guid] = persistentId;
			}
		}

		public static void Unregister(PersistentId persistentId)
		{
			if (persistentId != null && persistentId.HasGuid)
			{
				ByGuid.Remove(persistentId.Guid);
			}
		}

		public static bool TryResolve(string guid, out PersistentId persistentId)
		{
			persistentId = null;
			if (!string.IsNullOrEmpty(guid) && ByGuid.TryGetValue(guid, out persistentId))
			{
				return persistentId != null;
			}
			return false;
		}

		public static bool TryResolveNetId(string guid, out uint netId)
		{
			netId = 0u;
			if (!TryResolve(guid, out var persistentId))
			{
				return false;
			}
			if (!persistentId.TryGetComponent<NetworkIdentity>(out var component))
			{
				return false;
			}
			netId = component.netId;
			return netId != 0;
		}

		public static void Clear()
		{
			ByGuid.Clear();
		}
	}
}

using System;
using Mirror;

namespace NomadDrive.Features.SaveSystem
{
	public sealed class SaveContext : ISaveContext
	{
		public DateTime WorldSaveTimeUtc { get; }

		public SaveContext(DateTime worldSaveTimeUtc)
		{
			WorldSaveTimeUtc = worldSaveTimeUtc;
		}

		public string ToGuid(uint netId)
		{
			if (netId == 0)
			{
				return string.Empty;
			}
			if (NetworkServer.spawned.TryGetValue(netId, out var value) && value != null && value.TryGetComponent<PersistentId>(out var component) && component.HasGuid)
			{
				return component.Guid;
			}
			return string.Empty;
		}

		public bool TryToNetId(string guid, out uint netId)
		{
			return PersistentIdRegistry.TryResolveNetId(guid, out netId);
		}
	}
}

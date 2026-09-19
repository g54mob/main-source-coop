using System.Collections.Generic;
using Features.PlayerIdentityModule;

namespace Features.PlayerPresenceModule
{
	public class SessionRoster
	{
		private readonly List<PersistentPlayerId> _lockedIds = new List<PersistentPlayerId>();

		public bool IsLocked { get; private set; }

		public IReadOnlyList<PersistentPlayerId> LockedIds => _lockedIds;

		public void Lock(IEnumerable<PersistentPlayerId> presentIds)
		{
			if (IsLocked)
			{
				return;
			}
			_lockedIds.Clear();
			foreach (PersistentPlayerId presentId in presentIds)
			{
				if (!presentId.IsNone && !_lockedIds.Contains(presentId))
				{
					_lockedIds.Add(presentId);
				}
			}
			IsLocked = true;
		}

		public bool Contains(PersistentPlayerId id)
		{
			if (!id.IsNone)
			{
				return _lockedIds.Contains(id);
			}
			return false;
		}

		public bool TryFindReclaimable(PersistentPlayerId claimantId, IReadOnlyList<PersistentPlayerId> orphanOwnerIds, out int orphanIndex)
		{
			orphanIndex = -1;
			if (claimantId.IsNone || !Contains(claimantId))
			{
				return false;
			}
			for (int i = 0; i < orphanOwnerIds.Count; i++)
			{
				if (orphanOwnerIds[i].Equals(claimantId))
				{
					orphanIndex = i;
					return true;
				}
			}
			return false;
		}
	}
}

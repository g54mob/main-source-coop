using System.Collections.Generic;
using Features.PlayerIdentityModule;

namespace Features.PlayerPresenceModule
{
	public static class SessionPlayerReconciler
	{
		public static SessionPlayerReconcileResult Reconcile(PersistentPlayerId localId, bool isInScope, IReadOnlyList<SessionPlayerObservation> observations)
		{
			if (localId.IsNone)
			{
				return SessionPlayerReconcileResult.None;
			}
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < observations.Count; i++)
			{
				SessionPlayerObservation sessionPlayerObservation = observations[i];
				if (sessionPlayerObservation.OwnerId.Equals(localId))
				{
					if (sessionPlayerObservation.HasStateAuthority)
					{
						num = i;
						break;
					}
					if (num2 < 0)
					{
						num2 = i;
					}
				}
			}
			if (isInScope)
			{
				if (num >= 0)
				{
					return new SessionPlayerReconcileResult(SessionPlayerReconcileAction.Hold, num);
				}
				if (num2 >= 0)
				{
					return new SessionPlayerReconcileResult(SessionPlayerReconcileAction.Reclaim, num2);
				}
				return new SessionPlayerReconcileResult(SessionPlayerReconcileAction.Spawn, -1);
			}
			if (num >= 0)
			{
				return new SessionPlayerReconcileResult(SessionPlayerReconcileAction.Release, num);
			}
			return SessionPlayerReconcileResult.None;
		}
	}
}

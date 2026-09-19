using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Services
{
	public class EnemyTrackingService : IEnemyTrackingService, ISessionCleanup
	{
		private readonly List<IEnemyTrackable> _targets = new List<IEnemyTrackable>();

		private readonly List<IEnemyTrackable> _threats = new List<IEnemyTrackable>();

		public void RegisterTarget(IEnemyTrackable target)
		{
			Register(_targets, target);
		}

		public void UnregisterTarget(IEnemyTrackable target)
		{
			Unregister(_targets, target);
		}

		public bool TryGetNearestTarget(Vector3 origin, float radius, out IEnemyTrackable target)
		{
			float distance;
			return TryGetNearest(_targets, origin, radius, out target, out distance);
		}

		public void RegisterThreat(IEnemyTrackable threat)
		{
			Register(_threats, threat);
		}

		public void UnregisterThreat(IEnemyTrackable threat)
		{
			Unregister(_threats, threat);
		}

		public bool TryGetNearestThreat(Vector3 origin, float radius, out IEnemyTrackable threat, out float distance)
		{
			return TryGetNearest(_threats, origin, radius, out threat, out distance);
		}

		public void Cleanup()
		{
			_targets.Clear();
			_threats.Clear();
		}

		private static void Register(List<IEnemyTrackable> trackables, IEnemyTrackable trackable)
		{
			if (trackable != null && !trackables.Contains(trackable))
			{
				trackables.Add(trackable);
			}
		}

		private static void Unregister(List<IEnemyTrackable> trackables, IEnemyTrackable trackable)
		{
			if (trackable != null)
			{
				trackables.Remove(trackable);
			}
		}

		private static bool TryGetNearest(List<IEnemyTrackable> trackables, Vector3 origin, float radius, out IEnemyTrackable nearest, out float distance)
		{
			nearest = null;
			float num = radius * radius;
			for (int num2 = trackables.Count - 1; num2 >= 0; num2--)
			{
				IEnemyTrackable enemyTrackable = trackables[num2];
				if (enemyTrackable == null || enemyTrackable.Transform == null)
				{
					trackables.RemoveAt(num2);
				}
				else if (enemyTrackable.IsTrackable)
				{
					float sqrMagnitude = (enemyTrackable.Transform.position - origin).sqrMagnitude;
					if (!(sqrMagnitude > num))
					{
						num = sqrMagnitude;
						nearest = enemyTrackable;
					}
				}
			}
			distance = ((nearest != null) ? Mathf.Sqrt(num) : float.PositiveInfinity);
			return nearest != null;
		}
	}
}

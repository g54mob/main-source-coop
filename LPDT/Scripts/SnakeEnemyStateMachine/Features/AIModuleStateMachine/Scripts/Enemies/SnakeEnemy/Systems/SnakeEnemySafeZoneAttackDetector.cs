using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	public class SnakeEnemySafeZoneAttackDetector : EnemySafeZoneAttackDetector
	{
		private const float InsideVolumeOverlapRadius = 0.25f;

		private readonly Collider[] _insideSafeZoneOverlapHits = new Collider[32];

		public override bool TryFindBlockingSafeZoneUnderPlayer(Vector3 playerPosition, out PlayerSafeZone safeZone)
		{
			if (base.TryFindBlockingSafeZoneUnderPlayer(playerPosition, out safeZone))
			{
				return true;
			}
			int num = Physics.OverlapSphereNonAlloc(playerPosition, 0.25f, _insideSafeZoneOverlapHits, base.PlayerSafeZoneMask, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num; i++)
			{
				Collider collider = _insideSafeZoneOverlapHits[i];
				if (collider != null && TryGetSafeZone(collider, out safeZone))
				{
					return true;
				}
			}
			return false;
		}
	}
}

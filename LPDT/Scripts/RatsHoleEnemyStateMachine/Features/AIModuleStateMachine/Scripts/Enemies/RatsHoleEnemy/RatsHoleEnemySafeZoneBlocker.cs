using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public class RatsHoleEnemySafeZoneBlocker : MonoBehaviour
	{
		[SerializeField]
		private Transform _rayCastPoint;

		[SerializeField]
		private LayerMask _playerSafeZoneMask;

		[Header("Raycast tuning")]
		[SerializeField]
		private float _originYOffset = 0.5f;

		[SerializeField]
		private float _forwardRayLength = 5f;

		[SerializeField]
		private float _underPlayerRayLength = 2f;

		[SerializeField]
		private float _distanceToPlayerMultiplier = 1.1f;

		private readonly RaycastHit[] _blockingSafeZoneRaycastHits = new RaycastHit[32];

		public bool TryFindBlockingSafeZone(Vector3 enemyPosition, Vector3 playerPos, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			float num = playerPos.x - enemyPosition.x;
			float num2 = playerPos.z - enemyPosition.z;
			float num3 = (num * num + num2 * num2) * _distanceToPlayerMultiplier * _distanceToPlayerMultiplier;
			Vector3 vector = playerPos - enemyPosition;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude <= 0.0001f)
			{
				return TryFindBlockingSafeZoneUnderPlayer(playerPos, out safeZone);
			}
			Vector3 origin = ((_rayCastPoint != null) ? _rayCastPoint.position : (enemyPosition + Vector3.up * _originYOffset));
			int num4 = Physics.RaycastNonAlloc(new Ray(origin, vector * (1f / Mathf.Sqrt(sqrMagnitude))), _blockingSafeZoneRaycastHits, _forwardRayLength, _playerSafeZoneMask);
			float num5 = float.MaxValue;
			PlayerSafeZone playerSafeZone = null;
			for (int i = 0; i < num4; i++)
			{
				RaycastHit raycastHit = _blockingSafeZoneRaycastHits[i];
				if (!(raycastHit.collider == null) && raycastHit.collider.TryGetComponent<PlayerSafeZone>(out var component))
				{
					float num6 = raycastHit.point.x - enemyPosition.x;
					float num7 = raycastHit.point.z - enemyPosition.z;
					if (!(num6 * num6 + num7 * num7 > num3) && !(raycastHit.distance >= num5))
					{
						num5 = raycastHit.distance;
						playerSafeZone = component;
					}
				}
			}
			if (playerSafeZone == null)
			{
				return TryFindBlockingSafeZoneUnderPlayer(playerPos, out safeZone);
			}
			safeZone = playerSafeZone;
			return true;
		}

		public bool TryFindBlockingSafeZoneUnderPlayer(Vector3 playerPos, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			int num = Physics.RaycastNonAlloc(playerPos, Vector3.up, _blockingSafeZoneRaycastHits, _underPlayerRayLength, _playerSafeZoneMask);
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = _blockingSafeZoneRaycastHits[i];
				if (raycastHit.collider != null && raycastHit.collider.TryGetComponent<PlayerSafeZone>(out safeZone))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsPlayerInSafeZone(Vector3 enemyPosition, Vector3 playerPosition)
		{
			PlayerSafeZone safeZone;
			return TryFindBlockingSafeZone(enemyPosition, playerPosition, out safeZone);
		}
	}
}

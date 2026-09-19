using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy
{
	public class HeadmanSafeZoneBlocker : MonoBehaviour
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
			float num = Vector3.Distance(playerPos, new Vector3(enemyPosition.x, playerPos.y, enemyPosition.z));
			Vector3 vector = playerPos - enemyPosition;
			Vector3 origin = ((_rayCastPoint != null) ? _rayCastPoint.position : (enemyPosition + Vector3.up * _originYOffset));
			int num2 = Physics.RaycastNonAlloc(new Ray(origin, vector.normalized), _blockingSafeZoneRaycastHits, _forwardRayLength, _playerSafeZoneMask);
			Array.Sort(_blockingSafeZoneRaycastHits, 0, num2, Comparer<RaycastHit>.Create((RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance)));
			for (int num3 = 0; num3 < num2; num3++)
			{
				RaycastHit raycastHit = _blockingSafeZoneRaycastHits[num3];
				if (Vector3.Distance(raycastHit.point, new Vector3(enemyPosition.x, raycastHit.point.y, enemyPosition.z)) <= num * _distanceToPlayerMultiplier && raycastHit.collider != null && raycastHit.collider.TryGetComponent<PlayerSafeZone>(out safeZone))
				{
					return true;
				}
			}
			return TryFindBlockingSafeZoneUnderPlayer(playerPos, out safeZone);
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
	}
}

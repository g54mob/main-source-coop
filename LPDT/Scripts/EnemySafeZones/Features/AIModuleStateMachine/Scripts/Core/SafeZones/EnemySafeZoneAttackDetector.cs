using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.SafeZones
{
	public class EnemySafeZoneAttackDetector : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerSafeZoneMask;

		[Header("Raycast tuning")]
		[SerializeField]
		private float _underPlayerRayLength = 2f;

		[SerializeField]
		private float _playerOverlapRadius = 0.35f;

		private readonly RaycastHit[] _blockingSafeZoneRaycastHits = new RaycastHit[32];

		private readonly Collider[] _blockingSafeZoneOverlapHits = new Collider[16];

		private Vector3 _lastUnderPlayerRayOrigin;

		private bool _hasLastUnderPlayerRayOrigin;

		protected LayerMask PlayerSafeZoneMask => _playerSafeZoneMask;

		protected float UnderPlayerRayLength => _underPlayerRayLength;

		public bool TryGetAttackPosition(Vector3 enemyPosition, Vector3 playerPosition, out Vector3 attackPosition, out PlayerSafeZone safeZone)
		{
			attackPosition = Vector3.zero;
			if (!TryFindBlockingSafeZoneUnderPlayer(playerPosition, out safeZone))
			{
				return false;
			}
			if (!TryGetClosestInteractionPosition(enemyPosition, safeZone, out attackPosition))
			{
				return false;
			}
			return true;
		}

		public bool IsPlayerInSafeZone(Vector3 playerPosition)
		{
			PlayerSafeZone safeZone;
			return TryFindBlockingSafeZoneUnderPlayer(playerPosition, out safeZone);
		}

		public virtual bool TryFindBlockingSafeZoneUnderPlayer(Vector3 playerPosition, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			_lastUnderPlayerRayOrigin = playerPosition;
			_hasLastUnderPlayerRayOrigin = true;
			int num = Physics.RaycastNonAlloc(playerPosition, Vector3.up, _blockingSafeZoneRaycastHits, _underPlayerRayLength, _playerSafeZoneMask, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = _blockingSafeZoneRaycastHits[i];
				if (raycastHit.collider != null && TryGetSafeZone(raycastHit.collider, out safeZone))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryFindPlayerSafeZone(Vector3 playerPosition, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			_lastUnderPlayerRayOrigin = playerPosition;
			_hasLastUnderPlayerRayOrigin = true;
			if (TryFindOverlappingSafeZone(playerPosition, out safeZone))
			{
				return true;
			}
			return TryFindSafeZoneAbovePlayer(playerPosition, out safeZone);
		}

		private bool TryFindOverlappingSafeZone(Vector3 playerPosition, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			int size = Physics.OverlapSphereNonAlloc(playerPosition, _playerOverlapRadius, _blockingSafeZoneOverlapHits, _playerSafeZoneMask, QueryTriggerInteraction.Collide);
			return TryFindFirstSafeZone(_blockingSafeZoneOverlapHits, size, out safeZone);
		}

		private bool TryFindSafeZoneAbovePlayer(Vector3 playerPosition, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			int num = Physics.RaycastNonAlloc(playerPosition, Vector3.up, _blockingSafeZoneRaycastHits, _underPlayerRayLength, _playerSafeZoneMask, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num; i++)
			{
				if (TryGetPlayerSafeZone(_blockingSafeZoneRaycastHits[i].collider, out safeZone))
				{
					return true;
				}
			}
			return false;
		}

		private static bool TryFindFirstSafeZone(Collider[] colliders, int size, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			for (int i = 0; i < size; i++)
			{
				if (TryGetPlayerSafeZone(colliders[i], out safeZone))
				{
					return true;
				}
			}
			return false;
		}

		private static bool TryGetPlayerSafeZone(Collider collider, out PlayerSafeZone safeZone)
		{
			safeZone = null;
			if (collider == null)
			{
				return false;
			}
			if (collider.TryGetComponent<PlayerSafeZone>(out safeZone))
			{
				return true;
			}
			safeZone = collider.GetComponentInParent<PlayerSafeZone>();
			return safeZone != null;
		}

		protected bool TryGetSafeZone(Collider collider, out PlayerSafeZone safeZone)
		{
			if (collider.TryGetComponent<PlayerSafeZone>(out safeZone))
			{
				return true;
			}
			safeZone = collider.GetComponentInParent<PlayerSafeZone>();
			return safeZone != null;
		}

		private void OnDrawGizmos()
		{
			if (_hasLastUnderPlayerRayOrigin)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawRay(_lastUnderPlayerRayOrigin, Vector3.up * _underPlayerRayLength);
				Gizmos.DrawWireSphere(_lastUnderPlayerRayOrigin, 0.08f);
				Gizmos.DrawWireSphere(_lastUnderPlayerRayOrigin + Vector3.up * _underPlayerRayLength, 0.08f);
			}
		}

		private bool TryGetClosestInteractionPosition(Vector3 enemyPosition, PlayerSafeZone safeZone, out Vector3 attackPosition)
		{
			attackPosition = Vector3.zero;
			if (safeZone == null || safeZone.InteractionPoints == null || safeZone.InteractionPoints.Count == 0)
			{
				return false;
			}
			float num = float.PositiveInfinity;
			bool flag = false;
			for (int i = 0; i < safeZone.InteractionPoints.Count; i++)
			{
				Transform transform = safeZone.InteractionPoints[i];
				if (!(transform == null))
				{
					float sqrMagnitude = (transform.position - enemyPosition).sqrMagnitude;
					if (!flag || !(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						attackPosition = transform.position;
						flag = true;
					}
				}
			}
			return flag;
		}
	}
}

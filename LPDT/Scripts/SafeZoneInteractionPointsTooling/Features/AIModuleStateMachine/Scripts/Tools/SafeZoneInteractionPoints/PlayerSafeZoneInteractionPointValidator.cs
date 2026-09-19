using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Tools.SafeZoneInteractionPoints
{
	public static class PlayerSafeZoneInteractionPointValidator
	{
		private const float MIN_FLOOR_RAY_DISTANCE = 0.01f;

		private const float FLOOR_RAY_ORIGIN_OFFSET = 0.5f;

		private const int MAX_OVERLAP_HITS = 32;

		private static readonly Collider[] _overlapHits = new Collider[32];

		public static bool IsValid(Vector3 pointPosition, PlayerSafeZone safeZone, PlayerSafeZoneInteractionPointValidationSettings settings)
		{
			float maxDistance = Mathf.Max(0.01f, settings.FloorRayMaxDistance);
			Vector3 origin = pointPosition + Vector3.up * 0.5f;
			if (!RaycastInSafeZoneScene(safeZone, origin, Vector3.down, maxDistance, GetGroundFloorMask()))
			{
				return false;
			}
			return !HasObstacleSphereBlocker(pointPosition, safeZone, settings);
		}

		private static bool HasObstacleSphereBlocker(Vector3 pointPosition, PlayerSafeZone safeZone, PlayerSafeZoneInteractionPointValidationSettings settings)
		{
			int groundFloorMask = GetGroundFloorMask();
			int num = ResolveObstacleMask(settings.ObstacleMask, groundFloorMask);
			if (num == 0)
			{
				return false;
			}
			float radius = Mathf.Max(0.01f, settings.ObstacleProbeRadius);
			QueryTriggerInteraction triggerInteraction = (settings.IgnoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide);
			int num2 = OverlapSphereInSafeZoneScene(safeZone, pointPosition, radius, _overlapHits, num, triggerInteraction);
			for (int i = 0; i < num2; i++)
			{
				Collider collider = _overlapHits[i];
				_overlapHits[i] = null;
				if (IsRelevantObstacle(collider, safeZone, settings, num))
				{
					return true;
				}
			}
			return false;
		}

		private static bool RaycastInSafeZoneScene(PlayerSafeZone safeZone, Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
		{
			RaycastHit hit;
			return RaycastInSafeZoneScene(safeZone, origin, direction, out hit, maxDistance, layerMask);
		}

		private static bool RaycastInSafeZoneScene(PlayerSafeZone safeZone, Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, int layerMask)
		{
			hit = default(RaycastHit);
			Physics.SyncTransforms();
			if (safeZone != null)
			{
				PhysicsScene physicsScene = safeZone.gameObject.scene.GetPhysicsScene();
				if (physicsScene.IsValid())
				{
					return physicsScene.Raycast(origin, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
				}
			}
			return Physics.Raycast(origin, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
		}

		private static int OverlapSphereInSafeZoneScene(PlayerSafeZone safeZone, Vector3 position, float radius, Collider[] results, int layerMask, QueryTriggerInteraction triggerInteraction)
		{
			Physics.SyncTransforms();
			if (safeZone != null)
			{
				PhysicsScene physicsScene = safeZone.gameObject.scene.GetPhysicsScene();
				if (physicsScene.IsValid())
				{
					return physicsScene.OverlapSphere(position, radius, results, layerMask, triggerInteraction);
				}
			}
			return Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, triggerInteraction);
		}

		private static bool IsRelevantObstacle(Collider collider, PlayerSafeZone safeZone, PlayerSafeZoneInteractionPointValidationSettings settings, int obstacleMask)
		{
			if (collider == null || !collider.enabled || !collider.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (((1 << collider.gameObject.layer) & obstacleMask) == 0)
			{
				return false;
			}
			if (settings.IgnoreTriggers && collider.isTrigger)
			{
				return false;
			}
			if (settings.IgnoreOwnSafeZoneColliders && IsOwnSafeZoneDetectionCollider(collider, safeZone))
			{
				return false;
			}
			return true;
		}

		private static bool IsOwnSafeZoneDetectionCollider(Collider collider, PlayerSafeZone safeZone)
		{
			if (collider == null || safeZone == null)
			{
				return false;
			}
			if (collider.transform == safeZone.transform)
			{
				return collider.isTrigger;
			}
			return false;
		}

		private static int ResolveObstacleMask(LayerMask configuredMask, int groundMask)
		{
			return ((configuredMask.value == -1) ? CreateDefaultObstacleMask() : configuredMask.value) & ~groundMask;
		}

		private static int CreateDefaultObstacleMask()
		{
			return -1 & ~GetLayerBit("Ignore Raycast") & ~GetLayerBit("UI") & ~GetLayerBit("Ground") & ~GetLayerBit("Floor") & ~GetLayerBit("Water") & ~GetLayerBit("Player") & ~GetLayerBit("Enemy") & ~GetLayerBit("Ragdoll");
		}

		private static int GetGroundFloorMask()
		{
			return GetLayerBit("Ground") | GetLayerBit("Floor");
		}

		private static int GetLayerBit(string layerName)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num < 0)
			{
				return 0;
			}
			return 1 << num;
		}
	}
}

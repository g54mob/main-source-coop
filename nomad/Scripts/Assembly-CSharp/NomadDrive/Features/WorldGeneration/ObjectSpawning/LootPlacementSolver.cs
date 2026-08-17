using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootPlacementSolver
	{
		private struct ItemBoundsInfo
		{
			public float BottomOffset;

			public Vector3 BoundsCenterOffset;

			public Vector3 BoundsHalfExtents;

			public bool IsValid;
		}

		private const float RAYCAST_START_OFFSET = 1f;

		private const float MAX_DROP_DISTANCE = 10f;

		private const float SURFACE_EPSILON = 0.002f;

		private const float SAME_LEVEL_THRESHOLD = 0.05f;

		private const float XZ_OVERLAP_MARGIN = 0.01f;

		private const int MAX_OFFSET_ATTEMPTS = 4;

		private const float BASE_OFFSET_RADIUS = 0.1f;

		private const float OFFSET_RADIUS_INCREMENT = 0.05f;

		private const float MIN_SURFACE_NORMAL_Y = 0.3f;

		private const int MAX_RAYCAST_HITS = 16;

		private readonly RaycastHit[] _raycastBuffer = new RaycastHit[16];

		public LootPlacementResult Solve(GameObject instantiatedObject, Vector3 spawnPosition, Quaternion desiredRotation, LootPlacementContext context, System.Random random)
		{
			if (instantiatedObject == null)
			{
				return LootPlacementResult.Failed;
			}
			Collider[] componentsInChildren = instantiatedObject.GetComponentsInChildren<Collider>(includeInactive: true);
			SetCollidersEnabled(componentsInChildren, enabled: false);
			try
			{
				Physics.SyncTransforms();
				ItemBoundsInfo boundsInfo = ComputeItemBounds(instantiatedObject, componentsInChildren);
				if (TryPlaceAtPosition(spawnPosition, boundsInfo, context, out var placementPosition))
				{
					RegisterInContext(context, placementPosition, boundsInfo);
					return MakeResult(placementPosition, desiredRotation, fallback: false);
				}
				for (int i = 0; i < 4; i++)
				{
					float num = (float)(random.NextDouble() * Math.PI * 2.0);
					float num2 = 0.1f + (float)i * 0.05f;
					Vector3 candidateSpawn = spawnPosition + new Vector3((float)Math.Cos(num) * num2, 0f, (float)Math.Sin(num) * num2);
					if (TryPlaceAtPosition(candidateSpawn, boundsInfo, context, out var placementPosition2))
					{
						RegisterInContext(context, placementPosition2, boundsInfo);
						return MakeResult(placementPosition2, desiredRotation, fallback: false);
					}
				}
				Vector3 vector = TryFallbackPlacement(spawnPosition, boundsInfo);
				RegisterInContext(context, vector, boundsInfo);
				return MakeResult(vector, desiredRotation, fallback: true);
			}
			finally
			{
				SetCollidersEnabled(componentsInChildren, enabled: true);
			}
		}

		private bool TryPlaceAtPosition(Vector3 candidateSpawn, ItemBoundsInfo boundsInfo, LootPlacementContext context, out Vector3 placementPosition)
		{
			placementPosition = Vector3.zero;
			float num = -1f / 0f;
			bool flag = false;
			if (TryRaycastSurface(candidateSpawn, out var surfaceY))
			{
				num = surfaceY;
				flag = true;
			}
			if (context != null && boundsInfo.IsValid)
			{
				float num2 = FindHighestPlacedSurface(candidateSpawn, boundsInfo, context);
				if (num2 > num)
				{
					num = num2;
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			float num3 = (boundsInfo.IsValid ? boundsInfo.BottomOffset : 0f);
			placementPosition = new Vector3(candidateSpawn.x, num + num3 + 0.002f, candidateSpawn.z);
			if (context != null && boundsInfo.IsValid && HasSameLevelXZOverlap(placementPosition, boundsInfo, context))
			{
				return false;
			}
			return true;
		}

		private bool TryRaycastSurface(Vector3 spawnPosition, out float surfaceY)
		{
			surfaceY = -1f / 0f;
			Vector3 origin = spawnPosition + Vector3.up * 1f;
			float maxDistance = 11f;
			int num = Physics.RaycastNonAlloc(origin, Vector3.down, _raycastBuffer, maxDistance, -5, QueryTriggerInteraction.Ignore);
			if (num == 0)
			{
				return false;
			}
			bool flag = false;
			float num2 = -1f / 0f;
			for (int i = 0; i < num; i++)
			{
				ref RaycastHit reference = ref _raycastBuffer[i];
				if (!(reference.normal.y < 0.3f) && reference.point.y > num2)
				{
					num2 = reference.point.y;
					flag = true;
				}
			}
			if (flag)
			{
				surfaceY = num2;
				return true;
			}
			return false;
		}

		private static float FindHighestPlacedSurface(Vector3 candidateSpawn, ItemBoundsInfo boundsInfo, LootPlacementContext context)
		{
			float num = -1f / 0f;
			float num2 = boundsInfo.BoundsHalfExtents.x * 0.5f;
			float num3 = boundsInfo.BoundsHalfExtents.z * 0.5f;
			IReadOnlyList<PlacedItemRecord> placedItems = context.PlacedItems;
			for (int i = 0; i < placedItems.Count; i++)
			{
				PlacedItemRecord placedItemRecord = placedItems[i];
				float num4 = Mathf.Abs(candidateSpawn.x - placedItemRecord.Center.x);
				float num5 = Mathf.Abs(candidateSpawn.z - placedItemRecord.Center.z);
				if (num4 < placedItemRecord.HalfExtents.x + num2 && num5 < placedItemRecord.HalfExtents.z + num3)
				{
					float num6 = placedItemRecord.Center.y + placedItemRecord.HalfExtents.y;
					if (num6 > num)
					{
						num = num6;
					}
				}
			}
			return num;
		}

		private static bool HasSameLevelXZOverlap(Vector3 newPosition, ItemBoundsInfo newBounds, LootPlacementContext context)
		{
			Vector3 vector = newPosition + newBounds.BoundsCenterOffset;
			Vector3 boundsHalfExtents = newBounds.BoundsHalfExtents;
			IReadOnlyList<PlacedItemRecord> placedItems = context.PlacedItems;
			for (int i = 0; i < placedItems.Count; i++)
			{
				PlacedItemRecord placedItemRecord = placedItems[i];
				float num = vector.y - boundsHalfExtents.y;
				float num2 = placedItemRecord.Center.y + placedItemRecord.HalfExtents.y;
				float num3 = vector.y + boundsHalfExtents.y;
				float num4 = placedItemRecord.Center.y - placedItemRecord.HalfExtents.y;
				if (!(num >= num2 - 0.05f) && !(num3 <= num4 + 0.05f))
				{
					float num5 = Mathf.Abs(vector.x - placedItemRecord.Center.x);
					float num6 = Mathf.Abs(vector.z - placedItemRecord.Center.z);
					float num7 = boundsHalfExtents.x + placedItemRecord.HalfExtents.x - 0.01f;
					float num8 = boundsHalfExtents.z + placedItemRecord.HalfExtents.z - 0.01f;
					if (num5 < num7 && num6 < num8)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static Vector3 TryFallbackPlacement(Vector3 spawnPosition, ItemBoundsInfo boundsInfo)
		{
			float num = (boundsInfo.IsValid ? boundsInfo.BottomOffset : 0f);
			if (Physics.Raycast(spawnPosition + Vector3.up * 50f, Vector3.down, out var hitInfo, 100f, -5, QueryTriggerInteraction.Ignore))
			{
				return new Vector3(spawnPosition.x, hitInfo.point.y + num + 0.002f, spawnPosition.z);
			}
			return spawnPosition;
		}

		private static void SetCollidersEnabled(Collider[] colliders, bool enabled)
		{
			if (colliders == null)
			{
				return;
			}
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i] != null)
				{
					colliders[i].enabled = enabled;
				}
			}
		}

		private static void RegisterInContext(LootPlacementContext context, Vector3 position, ItemBoundsInfo boundsInfo)
		{
			if (context != null)
			{
				Vector3 boundsCenter = (boundsInfo.IsValid ? (position + boundsInfo.BoundsCenterOffset) : position);
				Vector3 boundsHalfExtents = (boundsInfo.IsValid ? boundsInfo.BoundsHalfExtents : (Vector3.one * 0.05f));
				context.RegisterPlacedItem(boundsCenter, boundsHalfExtents);
			}
		}

		private static LootPlacementResult MakeResult(Vector3 pos, Quaternion rot, bool fallback)
		{
			return new LootPlacementResult
			{
				Success = true,
				FinalPosition = pos,
				FinalRotation = rot,
				UsedFallback = fallback
			};
		}

		private static ItemBoundsInfo ComputeItemBounds(GameObject obj, Collider[] allColliders)
		{
			if (allColliders == null || allColliders.Length == 0)
			{
				return new ItemBoundsInfo
				{
					IsValid = false
				};
			}
			SetCollidersEnabled(allColliders, enabled: true);
			bool flag = false;
			Bounds bounds = default(Bounds);
			foreach (Collider collider in allColliders)
			{
				if (!(collider == null) && !collider.isTrigger && !(collider.bounds.size == Vector3.zero))
				{
					if (!flag)
					{
						bounds = collider.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(collider.bounds);
					}
				}
			}
			SetCollidersEnabled(allColliders, enabled: false);
			if (!flag)
			{
				return new ItemBoundsInfo
				{
					IsValid = false
				};
			}
			Vector3 position = obj.transform.position;
			return new ItemBoundsInfo
			{
				BottomOffset = position.y - bounds.min.y,
				BoundsCenterOffset = bounds.center - position,
				BoundsHalfExtents = bounds.extents,
				IsValid = true
			};
		}
	}
}

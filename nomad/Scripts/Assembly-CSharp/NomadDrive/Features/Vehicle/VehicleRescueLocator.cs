using System;
using System.Collections.Generic;
using EvilCore.DynamicCasting;
using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	public static class VehicleRescueLocator
	{
		private const int AnglesPerRing = 8;

		private const float GroundEpsilon = 0.05f;

		private const float ClearanceScale = 0.9f;

		public static bool TryFindClearGround(Vector3 origin, Vector3 footprintHalfExtents, LayerMask mask, IReadOnlyList<Collider> selfColliders, ICastingManager casting, float searchRadius, float ringStep, float verticalProbe, float maxDrop, out Vector3 groundPoint)
		{
			groundPoint = origin;
			if (casting == null)
			{
				return false;
			}
			if (ringStep <= 0.01f)
			{
				ringStep = 2f;
			}
			int num = Mathf.Max(1, Mathf.CeilToInt(searchRadius / ringStep));
			for (int i = 0; i <= num; i++)
			{
				if (i == 0)
				{
					if (TryCandidate(origin, footprintHalfExtents, mask, selfColliders, casting, verticalProbe, maxDrop, out groundPoint))
					{
						return true;
					}
					continue;
				}
				float num2 = (float)i * ringStep;
				float num3 = (float)i * 45f * 0.5f;
				for (int j = 0; j < 8; j++)
				{
					float f = (num3 + (float)j * 45f) * ((float)Math.PI / 180f);
					if (TryCandidate(origin + new Vector3(Mathf.Cos(f) * num2, 0f, Mathf.Sin(f) * num2), footprintHalfExtents, mask, selfColliders, casting, verticalProbe, maxDrop, out groundPoint))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool TryCandidate(Vector3 column, Vector3 halfExtents, LayerMask mask, IReadOnlyList<Collider> selfColliders, ICastingManager casting, float verticalProbe, float maxDrop, out Vector3 groundPoint)
		{
			groundPoint = column;
			CastRequest request = new CastRequest
			{
				Origin = null,
				Offset = column + Vector3.up * verticalProbe,
				UseTransformForward = false,
				Direction = Vector3.down,
				Type = CastType.Ray,
				Distance = verticalProbe + maxDrop,
				LayerMask = mask,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				MaxHits = 8
			};
			CastResult castResult = casting.CastImmediate(request);
			if (!castResult.DidHit)
			{
				return false;
			}
			Vector3 hitPoint = castResult.HitPoint;
			if (!IsBoxClear(hitPoint + Vector3.up * (halfExtents.y + 0.05f), halfExtents * 0.9f, mask, selfColliders, casting))
			{
				return false;
			}
			groundPoint = hitPoint;
			return true;
		}

		private static bool IsBoxClear(Vector3 center, Vector3 halfExtents, LayerMask mask, IReadOnlyList<Collider> selfColliders, ICastingManager casting)
		{
			CastRequest request = new CastRequest
			{
				Origin = null,
				Offset = center,
				Type = CastType.OverlapBox,
				HalfExtents = halfExtents,
				LayerMask = mask,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				MaxHits = 16
			};
			CastResult castResult = casting.CastImmediate(request);
			if (!castResult.DidHit)
			{
				return true;
			}
			for (int i = 0; i < castResult.HitCount; i++)
			{
				Collider collider = castResult.Colliders[i];
				if (!(collider == null) && !IsSelf(collider, selfColliders))
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsSelf(Collider c, IReadOnlyList<Collider> selfColliders)
		{
			if (selfColliders == null)
			{
				return false;
			}
			for (int i = 0; i < selfColliders.Count; i++)
			{
				if (selfColliders[i] == c)
				{
					return true;
				}
			}
			return false;
		}
	}
}

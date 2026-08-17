using System;
using System.Collections.Generic;
using EvilCore.DynamicCasting;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	public static class PlayerRespawnLocator
	{
		private const int AnglesPerRing = 8;

		private const float GroundEpsilon = 0.05f;

		private const float ClearanceRadiusScale = 0.9f;

		private const float DefaultCapsuleRadius = 0.35f;

		private const float DefaultCapsuleHeight = 1.6f;

		public static bool TryFindSafePosition(Vector3 origin, CapsuleCollider capsule, LayerMask mask, IReadOnlyList<Collider> selfColliders, ICastingManager casting, float searchRadius, float minRadius, float ringStep, float verticalProbe, float maxDrop, float groundClearance, out Vector3 result)
		{
			result = origin;
			if (casting == null)
			{
				return false;
			}
			float radius = ((capsule != null) ? capsule.radius : 0.35f);
			float height = ((capsule != null) ? capsule.height : 1.6f);
			if (ringStep <= 0.01f)
			{
				ringStep = 2f;
			}
			int num = Mathf.Max(1, Mathf.CeilToInt(searchRadius / ringStep));
			for (int i = 0; i <= num; i++)
			{
				if (i == 0)
				{
					if (!(minRadius > 0f) && TryCandidate(origin, radius, height, mask, selfColliders, casting, verticalProbe, maxDrop, groundClearance, out result))
					{
						return true;
					}
					continue;
				}
				float num2 = (float)i * ringStep;
				if (num2 < minRadius)
				{
					continue;
				}
				float num3 = (float)i * 45f * 0.5f;
				for (int j = 0; j < 8; j++)
				{
					float f = (num3 + (float)j * 45f) * ((float)Math.PI / 180f);
					if (TryCandidate(origin + new Vector3(Mathf.Cos(f) * num2, 0f, Mathf.Sin(f) * num2), radius, height, mask, selfColliders, casting, verticalProbe, maxDrop, groundClearance, out result))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool TryCandidate(Vector3 column, float radius, float height, LayerMask mask, IReadOnlyList<Collider> selfColliders, ICastingManager casting, float verticalProbe, float maxDrop, float groundClearance, out Vector3 result)
		{
			result = column;
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
			Vector3 vector = castResult.HitPoint + Vector3.up * 0.05f;
			float radius2 = radius * 0.9f;
			Vector3 center = vector + Vector3.up * radius;
			Vector3 center2 = vector + Vector3.up * Mathf.Max(radius, height - radius);
			if (!IsSphereClear(center, radius2, mask, selfColliders, casting))
			{
				return false;
			}
			if (!IsSphereClear(center2, radius2, mask, selfColliders, casting))
			{
				return false;
			}
			result = vector + Vector3.up * groundClearance;
			return true;
		}

		private static bool IsSphereClear(Vector3 center, float radius, LayerMask mask, IReadOnlyList<Collider> selfColliders, ICastingManager casting)
		{
			CastRequest request = new CastRequest
			{
				Origin = null,
				Offset = center,
				Type = CastType.OverlapSphere,
				Radius = radius,
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

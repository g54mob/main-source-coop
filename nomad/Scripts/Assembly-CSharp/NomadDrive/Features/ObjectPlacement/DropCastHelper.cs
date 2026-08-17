using System;
using System.Collections.Generic;
using EvilCore.DynamicCasting;
using Mirror;
using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public static class DropCastHelper
	{
		private static RaycastHit[] _sortBuffer = new RaycastHit[32];

		public static DropResult ProcessCastResult(CastResult castResult, HeldItem item, DropSettings settings, HashSet<Collider> selfColliders, int blockingMask, Quaternion desiredRotation, Vector3 anchorLocalOffset, MeshRenderer[] renderers = null, HashSet<uint> compoundMemberNetIds = null)
		{
			if (!castResult.DidHit || castResult.HitCount == 0)
			{
				return DropResult.Failed;
			}
			int hitCount = castResult.HitCount;
			if (_sortBuffer.Length < hitCount)
			{
				_sortBuffer = new RaycastHit[hitCount];
			}
			Array.Copy(castResult.Hits, _sortBuffer, hitCount);
			InsertionSortByDistance(_sortBuffer, hitCount);
			DropResult dropResult = DropResult.Failed;
			for (int i = 0; i < hitCount; i++)
			{
				RaycastHit hit = _sortBuffer[i];
				if (!(hit.collider == null) && !IsSelf(hit.collider, selfColliders) && !IsCompoundMember(hit.collider, compoundMemberNetIds) && hit.collider.TryGetComponent<SnappingPlane>(out var component) && IsSnappingPlaneCompatible(component, item))
				{
					dropResult = CreateDropResult(hit, component, item, settings, desiredRotation, anchorLocalOffset, renderers);
					break;
				}
			}
			if (!dropResult.Success)
			{
				int value = settings.GroundLayers.value;
				for (int j = 0; j < hitCount; j++)
				{
					RaycastHit hit2 = _sortBuffer[j];
					if (!(hit2.collider == null) && !IsSelf(hit2.collider, selfColliders) && !IsCompoundMember(hit2.collider, compoundMemberNetIds) && !hit2.collider.TryGetComponent<SnappingPlane>(out var _) && (value & (1 << hit2.collider.gameObject.layer)) != 0)
					{
						dropResult = CreateDropResult(hit2, null, item, settings, desiredRotation, anchorLocalOffset, renderers);
						break;
					}
				}
			}
			if (!dropResult.Success)
			{
				return DropResult.Failed;
			}
			if (IsPathObstructed(_sortBuffer, hitCount, dropResult, selfColliders, blockingMask, compoundMemberNetIds))
			{
				return DropResult.Failed;
			}
			return dropResult;
		}

		private static bool IsSelf(Collider collider, HashSet<Collider> selfColliders)
		{
			return selfColliders?.Contains(collider) ?? false;
		}

		private static bool IsCompoundMember(Collider collider, HashSet<uint> compoundMemberNetIds)
		{
			if (compoundMemberNetIds == null || compoundMemberNetIds.Count == 0)
			{
				return false;
			}
			NetworkIdentity componentInParent = collider.GetComponentInParent<NetworkIdentity>();
			if (componentInParent != null)
			{
				return compoundMemberNetIds.Contains(componentInParent.netId);
			}
			return false;
		}

		private static void InsertionSortByDistance(RaycastHit[] hits, int count)
		{
			for (int i = 1; i < count; i++)
			{
				RaycastHit raycastHit = hits[i];
				int num = i - 1;
				while (num >= 0 && hits[num].distance > raycastHit.distance)
				{
					hits[num + 1] = hits[num];
					num--;
				}
				hits[num + 1] = raycastHit;
			}
		}

		private static bool IsPathObstructed(RaycastHit[] sortedHits, int count, DropResult candidate, HashSet<Collider> selfColliders, int blockingMask, HashSet<uint> compoundMemberNetIds)
		{
			float distance = candidate.HitInfo.distance;
			Collider collider = candidate.HitInfo.collider;
			for (int i = 0; i < count; i++)
			{
				RaycastHit raycastHit = sortedHits[i];
				Collider collider2 = raycastHit.collider;
				if (!(collider2 == null) && !(collider2 == collider) && !IsSelf(collider2, selfColliders) && !IsCompoundMember(collider2, compoundMemberNetIds) && !collider2.isTrigger && !collider2.TryGetComponent<SnappingPlane>(out var _) && (blockingMask & (1 << collider2.gameObject.layer)) != 0 && !(raycastHit.distance >= distance - 0.001f))
				{
					return true;
				}
			}
			return false;
		}

		private static DropResult CreateDropResult(RaycastHit hit, SnappingPlane plane, HeldItem item, DropSettings settings, Quaternion desiredRotation, Vector3 anchorLocalOffset, MeshRenderer[] renderers)
		{
			if (renderers == null)
			{
				renderers = item.GetComponentsInChildren<MeshRenderer>();
			}
			Quaternion quaternion = desiredRotation * Quaternion.Inverse(item.transform.rotation);
			float num = PlacementGeometry.CalculateDynamicNormalOffset(renderers, item.transform.position, hit.normal, quaternion);
			Vector3 vector = quaternion * (item.transform.TransformPoint(anchorLocalOffset) - item.transform.position);
			Vector3 vector2 = vector - Vector3.Dot(vector, hit.normal) * hit.normal;
			Vector3 targetPosition = hit.point + hit.normal * num - vector2;
			targetPosition += hit.normal * settings.SurfaceOffset;
			return new DropResult
			{
				Success = true,
				HitInfo = hit,
				SnappingPlane = plane,
				TargetPosition = targetPosition,
				TargetRotation = desiredRotation
			};
		}

		public static bool IsSnappingPlaneCompatible(SnappingPlane plane, HeldItem item)
		{
			if (plane == null || item == null)
			{
				return false;
			}
			if (plane.transform.IsChildOf(item.transform))
			{
				return false;
			}
			if (!plane.IsPlacementAllowed(item.gameObject))
			{
				return false;
			}
			if (!plane.IsEnabled)
			{
				return false;
			}
			if (plane.IsSingleUseSlot && plane.IsAnyObjectPlaced)
			{
				return false;
			}
			return true;
		}
	}
}

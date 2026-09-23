using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class PlayerHitZones
	{
		private const float HeadRadius = 0.18f;

		private const float TorsoHalfWidth = 0.22f;

		private const float NeckFraction = 0.82f;

		private const float HipsFraction = 0.48f;

		public static HitZone Classify(GameObject victim, Vector3 point)
		{
			if (victim == null)
			{
				return HitZone.Body;
			}
			PlayerVoxelBody component = victim.GetComponent<PlayerVoxelBody>();
			if (component != null && component.HasVoxelBody)
			{
				return HitZone.Body;
			}
			Animator animator = AnimatorOf(victim);
			Transform transform = ((animator != null) ? animator.GetBoneTransform(HumanBodyBones.Head) : null);
			Transform transform2 = ((animator != null) ? animator.GetBoneTransform(HumanBodyBones.Hips) : null);
			if (transform != null && transform2 != null)
			{
				return ClassifyByBones(victim.transform, animator, transform, transform2, point);
			}
			return ClassifyByCapsule(victim, point);
		}

		private static Animator AnimatorOf(GameObject victim)
		{
			PlayerAnimator componentInChildren = victim.GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			Animator animator = ((componentInChildren != null) ? componentInChildren.Animator : null);
			if (!(animator != null) || !animator.isActiveAndEnabled || !animator.isHuman)
			{
				return null;
			}
			return animator;
		}

		private static HitZone ClassifyByBones(Transform root, Animator animator, Transform head, Transform hips, Vector3 point)
		{
			float num = Mathf.Max(0.01f, root.lossyScale.y);
			Vector3 vector = head.position + root.up * (0.09f * num);
			if ((point - vector).sqrMagnitude <= 0.0324f * num * num)
			{
				return HitZone.Head;
			}
			Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Neck);
			float num2 = Vector3.Dot(((boneTransform != null) ? boneTransform.position : head.position) - root.position, root.up);
			float num3 = Vector3.Dot(hips.position - root.position, root.up);
			float num4 = Vector3.Dot(point - root.position, root.up);
			if (num4 >= num2)
			{
				return HitZone.Head;
			}
			if (num4 < num3)
			{
				return HitZone.Limb;
			}
			Transform transform = animator.GetBoneTransform(HumanBodyBones.UpperChest) ?? animator.GetBoneTransform(HumanBodyBones.Chest) ?? animator.GetBoneTransform(HumanBodyBones.Spine) ?? hips;
			if (!(DistanceFromSegment(point, hips.position, transform.position) <= 0.22f * num))
			{
				return HitZone.Limb;
			}
			return HitZone.Body;
		}

		private static HitZone ClassifyByCapsule(GameObject victim, Vector3 point)
		{
			CharacterController component = victim.GetComponent<CharacterController>();
			if (component == null)
			{
				return HitZone.Body;
			}
			Transform transform = victim.transform;
			float num = component.height * Mathf.Max(0.01f, transform.lossyScale.y);
			float num2 = Vector3.Dot(transform.TransformPoint(component.center) - transform.position, transform.up) - num * 0.5f;
			float num3 = ((num > 0.001f) ? ((Vector3.Dot(point - transform.position, transform.up) - num2) / num) : 0.5f);
			if (num3 >= 0.82f)
			{
				return HitZone.Head;
			}
			if (!(num3 >= 0.48f))
			{
				return HitZone.Limb;
			}
			return HitZone.Body;
		}

		private static float DistanceFromSegment(Vector3 point, Vector3 a, Vector3 b)
		{
			Vector3 vector = b - a;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < 0.0001f)
			{
				return Vector3.Distance(point, a);
			}
			float num = Mathf.Clamp01(Vector3.Dot(point - a, vector) / sqrMagnitude);
			return Vector3.Distance(point, a + vector * num);
		}
	}
}

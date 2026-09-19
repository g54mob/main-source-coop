using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	internal static class AnchorPhysicsUtil
	{
		public static void ApplyPose(Rigidbody body, Transform target, Vector3 position, Quaternion rotation)
		{
			if (body != null)
			{
				body.position = position;
				body.rotation = rotation;
			}
			if (target != null)
			{
				target.SetPositionAndRotation(position, rotation);
			}
		}

		public static void ApplyWorldPosition(Rigidbody body, Transform target, Vector3 worldPosition)
		{
			if (body != null)
			{
				body.position = worldPosition;
				body.linearVelocity = Vector3.zero;
				body.angularVelocity = Vector3.zero;
			}
			if (target != null)
			{
				target.position = worldPosition;
			}
		}

		public static void ApplyYankVelocity(Rigidbody body, Vector3 velocity)
		{
			if (!(body == null))
			{
				body.isKinematic = false;
				body.linearVelocity = velocity;
				body.angularVelocity = Vector3.zero;
				body.WakeUp();
			}
		}

		public static void SetRigidbodyThrown(Rigidbody body, bool thrown)
		{
			if (!(body == null))
			{
				body.linearVelocity = Vector3.zero;
				body.angularVelocity = Vector3.zero;
				body.isKinematic = !thrown;
				if (thrown)
				{
					body.WakeUp();
				}
			}
		}

		public static void ZeroVelocity(Rigidbody body)
		{
			if (!(body == null))
			{
				body.linearVelocity = Vector3.zero;
				body.angularVelocity = Vector3.zero;
			}
		}

		public static void SetRenderersEnabled(Renderer[] renderers, bool enabled)
		{
			if (renderers == null)
			{
				return;
			}
			for (int i = 0; i < renderers.Length; i++)
			{
				if (renderers[i] != null)
				{
					renderers[i].enabled = enabled;
				}
			}
		}

		public static void SetCollidersEnabled(Collider[] colliders, bool enabled)
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
	}
}

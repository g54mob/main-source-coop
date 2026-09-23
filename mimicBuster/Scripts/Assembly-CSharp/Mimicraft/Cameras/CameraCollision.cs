using System;
using UnityEngine;

namespace Mimicraft.Cameras
{
	public static class CameraCollision
	{
		public readonly struct Probe
		{
			public readonly Vector3 HalfExtents;

			public float Near => HalfExtents.z * 2f;

			public Probe(Vector3 halfExtents)
			{
				HalfExtents = halfExtents;
			}

			public static Probe For(Camera camera)
			{
				float num = ((camera != null) ? camera.nearClipPlane : 0.3f);
				float num2 = ((camera != null) ? camera.fieldOfView : 60f);
				float num3 = ((camera != null) ? camera.aspect : 1.7777778f);
				float num4 = num * Mathf.Tan(num2 * 0.5f * (MathF.PI / 180f));
				return new Probe(new Vector3(num4 * num3, num4, num * 0.5f));
			}
		}

		public const float MinDistance = 0.15f;

		private const float Buffer = 0.05f;

		private static readonly string[] IgnoredLayerNames = new string[9] { "PlayerController", "MapBlocker", "GarticMapBlocker", "FPSVisual", "Studio", "UI", "Ignore Raycast", "TransparentFX", "Water" };

		private static int mask;

		private static bool maskBuilt;

		private static readonly RaycastHit[] hits = new RaycastHit[24];

		public static int Mask
		{
			get
			{
				if (maskBuilt)
				{
					return mask;
				}
				maskBuilt = true;
				mask = -1;
				string[] ignoredLayerNames = IgnoredLayerNames;
				for (int i = 0; i < ignoredLayerNames.Length; i++)
				{
					int num = LayerMask.NameToLayer(ignoredLayerNames[i]);
					if (num >= 0)
					{
						mask &= ~(1 << num);
					}
				}
				return mask;
			}
		}

		public static float ResolveDistance(Probe probe, Vector3 pivot, Vector3 direction, float desired, Transform ignoreRoot)
		{
			return Mathf.Max(ClearDistance(probe, pivot, direction, desired, ignoreRoot), 0.15f);
		}

		public static float ClearDistance(Probe probe, Vector3 pivot, Vector3 direction, float desired, Transform ignoreRoot)
		{
			if (desired <= 0f || direction.sqrMagnitude < 0.0001f)
			{
				return 0f;
			}
			direction.Normalize();
			float num = desired;
			if (TryNearest(Physics.RaycastNonAlloc(pivot, direction, hits, desired, Mask, QueryTriggerInteraction.Ignore), ignoreRoot, acceptZero: true, out var nearest))
			{
				num = Mathf.Min(num, nearest);
			}
			Quaternion orientation = Quaternion.LookRotation(-direction, UpFor(direction));
			if (TryNearest(Physics.BoxCastNonAlloc(pivot - direction * probe.HalfExtents.z, probe.HalfExtents, direction, hits, orientation, desired, Mask, QueryTriggerInteraction.Ignore), ignoreRoot, acceptZero: false, out var nearest2))
			{
				num = Mathf.Min(num, nearest2);
			}
			return Mathf.Max(num - 0.05f, 0f);
		}

		private static bool TryNearest(int count, Transform ignoreRoot, bool acceptZero, out float nearest)
		{
			nearest = float.MaxValue;
			bool result = false;
			for (int i = 0; i < count; i++)
			{
				RaycastHit raycastHit = hits[i];
				if (acceptZero || !(raycastHit.distance <= 0f))
				{
					Transform transform = raycastHit.transform;
					if ((!(ignoreRoot != null) || (!(transform == ignoreRoot) && !transform.IsChildOf(ignoreRoot))) && raycastHit.distance < nearest)
					{
						nearest = raycastHit.distance;
						result = true;
					}
				}
			}
			return result;
		}

		private static Vector3 UpFor(Vector3 direction)
		{
			if (!(Mathf.Abs(Vector3.Dot(direction, Vector3.up)) > 0.99f))
			{
				return Vector3.up;
			}
			return Vector3.forward;
		}
	}
}

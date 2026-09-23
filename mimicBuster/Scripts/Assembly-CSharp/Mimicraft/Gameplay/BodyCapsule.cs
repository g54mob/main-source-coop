using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class BodyCapsule
	{
		public struct Shape
		{
			public float Radius;

			public float Height;

			public float SkinWidth;

			public Vector3 Center;

			public float WorldRadius;

			public float WorldHeight;
		}

		private const float MinWorldHeight = 0.2f;

		private const float MinWorldRadius = 0.05f;

		private const float SkinFraction = 0.1f;

		private const float MinWorldSkin = 0.001f;

		public static Shape Compute(Bounds boundsLocal, float scale)
		{
			scale = Mathf.Max(scale, 0.0001f);
			float num = Mathf.Max(boundsLocal.size.y * scale, 0.2f);
			float num2 = Mathf.Min(boundsLocal.extents.x, boundsLocal.extents.z) * scale;
			float num3 = Mathf.Max(num2 * 0.1f, 0.001f);
			float num4 = Mathf.Clamp(num3, 0f, (num - 0.2f) * 0.5f);
			float num5 = num - num4 * 2f;
			float num6 = num5 * 0.5f;
			float num7 = Mathf.Clamp(num2 - num3, Mathf.Min(0.05f, num6), num6);
			float height = num5 / scale;
			return new Shape
			{
				Radius = num7 / scale,
				Height = height,
				SkinWidth = num3 / scale,
				Center = new Vector3(boundsLocal.center.x, boundsLocal.min.y + (num4 + num6) / scale, boundsLocal.center.z),
				WorldRadius = num7,
				WorldHeight = num5
			};
		}

		public static bool TryMeasure(Transform root, out Bounds boundsLocal)
		{
			boundsLocal = default(Bounds);
			bool flag = false;
			VoxelModel[] componentsInChildren = root.GetComponentsInChildren<VoxelModel>(includeInactive: false);
			foreach (VoxelModel voxelModel in componentsInChildren)
			{
				if (voxelModel.Grid == null || !GridBounds.TryCompute(voxelModel.Grid, out var min, out var max))
				{
					continue;
				}
				for (int j = 0; j < 8; j++)
				{
					Vector3 position = new Vector3(((j & 1) == 0) ? min.x : (max.x + 1), ((j & 2) == 0) ? min.y : (max.y + 1), ((j & 4) == 0) ? min.z : (max.z + 1));
					Vector3 vector = root.InverseTransformPoint(voxelModel.transform.TransformPoint(position));
					if (!flag)
					{
						boundsLocal = new Bounds(vector, Vector3.zero);
						flag = true;
					}
					else
					{
						boundsLocal.Encapsulate(vector);
					}
				}
			}
			return flag;
		}

		public static Bounds Reframe(Bounds box, Quaternion rotation)
		{
			Vector3 extents = box.extents;
			Bounds result = new Bounds(rotation * box.center, Vector3.zero);
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector = new Vector3(((i & 1) == 0) ? (0f - extents.x) : extents.x, ((i & 2) == 0) ? (0f - extents.y) : extents.y, ((i & 4) == 0) ? (0f - extents.z) : extents.z);
				result.Encapsulate(rotation * (box.center + vector));
			}
			return result;
		}

		public static Quaternion UprightFrame(Transform root)
		{
			Vector3 vector = Vector3.ProjectOnPlane(root.forward, Vector3.up);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.ProjectOnPlane(root.up, Vector3.up);
			}
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.forward;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}
	}
}

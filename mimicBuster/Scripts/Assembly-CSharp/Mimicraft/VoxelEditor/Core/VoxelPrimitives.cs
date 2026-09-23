using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class VoxelPrimitives
	{
		public static readonly VoxelPrimitive[] All = new VoxelPrimitive[8]
		{
			VoxelPrimitive.StartingModel,
			VoxelPrimitive.Cube,
			VoxelPrimitive.Box,
			VoxelPrimitive.Cylinder,
			VoxelPrimitive.Sphere,
			VoxelPrimitive.Cone,
			VoxelPrimitive.Pyramid,
			VoxelPrimitive.Wedge
		};

		public static string NameKey(VoxelPrimitive shape)
		{
			return "Primitive." + shape;
		}

		public static VoxelGrid Build(VoxelPrimitive shape, int size, Color32 color)
		{
			if (shape == VoxelPrimitive.StartingModel)
			{
				return null;
			}
			size = Mathf.Max(size, VoxelEditorSettings.MinBoundExtent);
			VoxelGrid voxelGrid = new VoxelGrid();
			VoxelData data = new VoxelData(color);
			float num = (float)(size - 1) * 0.5f;
			float radius = (float)size * 0.5f;
			for (int i = 0; i < size; i++)
			{
				float up = ((size > 1) ? ((float)i / (float)(size - 1)) : 0f);
				for (int j = 0; j < size; j++)
				{
					for (int k = 0; k < size; k++)
					{
						float dx = (float)j - num;
						float dz = (float)k - num;
						if (Contains(shape, size, radius, num, up, i, k, dx, dz, 1.1f))
						{
							voxelGrid.Set(new Vector3Int(j, i, k), data);
						}
					}
				}
			}
			return voxelGrid;
		}

		private static bool Contains(VoxelPrimitive shape, int size, float radius, float centre, float up, int y, int z, float dx, float dz, float minTip)
		{
			switch (shape)
			{
			case VoxelPrimitive.Cube:
				return true;
			case VoxelPrimitive.Box:
				if (y < Mathf.Max(2, size * 2 / 3))
				{
					return z < Mathf.Max(2, size / 2);
				}
				return false;
			case VoxelPrimitive.Cylinder:
				return dx * dx + dz * dz <= radius * radius;
			case VoxelPrimitive.Sphere:
			{
				float num4 = (float)y - centre;
				return dx * dx + num4 * num4 + dz * dz <= radius * radius;
			}
			case VoxelPrimitive.Cone:
			{
				float num3 = Mathf.Lerp(radius, minTip, up);
				return dx * dx + dz * dz <= num3 * num3;
			}
			case VoxelPrimitive.Pyramid:
			{
				float num2 = Mathf.Lerp(radius, minTip, up);
				if (Mathf.Abs(dx) <= num2)
				{
					return Mathf.Abs(dz) <= num2;
				}
				return false;
			}
			case VoxelPrimitive.Wedge:
			{
				int num = Mathf.Max(2, Mathf.CeilToInt((float)size * (1f - up)));
				return z < num;
			}
			default:
				return false;
			}
		}
	}
}

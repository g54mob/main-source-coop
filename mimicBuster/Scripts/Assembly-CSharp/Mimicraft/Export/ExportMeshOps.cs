using System.Collections.Generic;

namespace Mimicraft.Export
{
	public static class ExportMeshOps
	{
		public static void Transform(ExportMesh mesh, float scale, float qx, float qy, float qz, float qw, float tx, float ty, float tz)
		{
			List<float> positions = mesh.Positions;
			for (int i = 0; i < positions.Count; i += 3)
			{
				Rotate(positions[i] * scale, positions[i + 1] * scale, positions[i + 2] * scale, qx, qy, qz, qw, out var x, out var y, out var z);
				positions[i] = x + tx;
				positions[i + 1] = y + ty;
				positions[i + 2] = z + tz;
			}
			List<float> normals = mesh.Normals;
			for (int j = 0; j < normals.Count; j += 3)
			{
				Rotate(normals[j], normals[j + 1], normals[j + 2], qx, qy, qz, qw, out var x2, out var y2, out var z2);
				normals[j] = x2;
				normals[j + 1] = y2;
				normals[j + 2] = z2;
			}
		}

		private static void Rotate(float vx, float vy, float vz, float qx, float qy, float qz, float qw, out float x, out float y, out float z)
		{
			float num = qy * vz - qz * vy + qw * vx;
			float num2 = qz * vx - qx * vz + qw * vy;
			float num3 = qx * vy - qy * vx + qw * vz;
			x = vx + 2f * (qy * num3 - qz * num2);
			y = vy + 2f * (qz * num - qx * num3);
			z = vz + 2f * (qx * num2 - qy * num);
		}

		public static bool Bounds(IEnumerable<ExportMesh> meshes, float[] min, float[] max)
		{
			bool flag = false;
			foreach (ExportMesh mesh in meshes)
			{
				List<float> positions = mesh.Positions;
				for (int i = 0; i < positions.Count; i += 3)
				{
					for (int j = 0; j < 3; j++)
					{
						float num = positions[i + j];
						if (!flag || num < min[j])
						{
							min[j] = num;
						}
						if (!flag || num > max[j])
						{
							max[j] = num;
						}
					}
					flag = true;
				}
			}
			return flag;
		}

		public static void Translate(ExportMesh mesh, float dx, float dy, float dz)
		{
			List<float> positions = mesh.Positions;
			for (int i = 0; i < positions.Count; i += 3)
			{
				positions[i] += dx;
				positions[i + 1] += dy;
				positions[i + 2] += dz;
			}
		}

		public static void MirrorX(ExportMesh mesh)
		{
			for (int i = 0; i < mesh.Positions.Count; i += 3)
			{
				mesh.Positions[i] = 0f - mesh.Positions[i];
			}
			for (int j = 0; j < mesh.Normals.Count; j += 3)
			{
				mesh.Normals[j] = 0f - mesh.Normals[j];
			}
		}

		public static int FixWinding(ExportMesh mesh)
		{
			int num = 0;
			List<float> positions = mesh.Positions;
			for (int i = 0; i < mesh.QuadCount; i++)
			{
				int num2 = i * 4;
				int num3 = mesh.Corners[num2] * 3;
				int num4 = mesh.Corners[num2 + 1] * 3;
				int num5 = mesh.Corners[num2 + 2] * 3;
				float num6 = positions[num4] - positions[num3];
				float num7 = positions[num4 + 1] - positions[num3 + 1];
				float num8 = positions[num4 + 2] - positions[num3 + 2];
				float num9 = positions[num5] - positions[num3];
				float num10 = positions[num5 + 1] - positions[num3 + 1];
				float num11 = positions[num5 + 2] - positions[num3 + 2];
				float num12 = num7 * num11 - num8 * num10;
				float num13 = num8 * num9 - num6 * num11;
				float num14 = num6 * num10 - num7 * num9;
				if (!(num12 * mesh.Normals[num2 * 3] + num13 * mesh.Normals[num2 * 3 + 1] + num14 * mesh.Normals[num2 * 3 + 2] >= 0f))
				{
					Swap(mesh.Corners, num2 + 1, num2 + 3, 1);
					Swap(mesh.Normals, (num2 + 1) * 3, (num2 + 3) * 3, 3);
					Swap(mesh.Uvs, (num2 + 1) * 2, (num2 + 3) * 2, 2);
					num++;
				}
			}
			return num;
		}

		private static void Swap<T>(List<T> list, int a, int b, int count)
		{
			for (int i = 0; i < count; i++)
			{
				T value = list[a + i];
				list[a + i] = list[b + i];
				list[b + i] = value;
			}
		}

		public static ExportMesh Merge(IReadOnlyList<ExportMesh> meshes, string name)
		{
			ExportMesh exportMesh = new ExportMesh
			{
				Name = (name ?? "")
			};
			foreach (ExportMesh mesh in meshes)
			{
				int pointCount = exportMesh.PointCount;
				exportMesh.Positions.AddRange(mesh.Positions);
				exportMesh.Normals.AddRange(mesh.Normals);
				exportMesh.Uvs.AddRange(mesh.Uvs);
				foreach (int corner in mesh.Corners)
				{
					exportMesh.Corners.Add(corner + pointCount);
				}
			}
			return exportMesh;
		}
	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.VoxelEditor.Core
{
	public static class VoxelMeshBuilder
	{
		private struct FaceDef
		{
			public Vector3Int Normal;

			public Vector3[] Corners;
		}

		private static readonly FaceDef[] Faces = new FaceDef[6]
		{
			new FaceDef
			{
				Normal = new Vector3Int(0, 0, -1),
				Corners = new Vector3[4]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 0f, 0f),
					new Vector3(1f, 1f, 0f)
				}
			},
			new FaceDef
			{
				Normal = new Vector3Int(0, 0, 1),
				Corners = new Vector3[4]
				{
					new Vector3(1f, 0f, 1f),
					new Vector3(1f, 1f, 1f),
					new Vector3(0f, 0f, 1f),
					new Vector3(0f, 1f, 1f)
				}
			},
			new FaceDef
			{
				Normal = new Vector3Int(0, 1, 0),
				Corners = new Vector3[4]
				{
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 1f),
					new Vector3(1f, 1f, 0f),
					new Vector3(1f, 1f, 1f)
				}
			},
			new FaceDef
			{
				Normal = new Vector3Int(0, -1, 0),
				Corners = new Vector3[4]
				{
					new Vector3(1f, 0f, 0f),
					new Vector3(1f, 0f, 1f),
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 0f, 1f)
				}
			},
			new FaceDef
			{
				Normal = new Vector3Int(-1, 0, 0),
				Corners = new Vector3[4]
				{
					new Vector3(0f, 0f, 1f),
					new Vector3(0f, 1f, 1f),
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f)
				}
			},
			new FaceDef
			{
				Normal = new Vector3Int(1, 0, 0),
				Corners = new Vector3[4]
				{
					new Vector3(1f, 0f, 0f),
					new Vector3(1f, 1f, 0f),
					new Vector3(1f, 0f, 1f),
					new Vector3(1f, 1f, 1f)
				}
			}
		};

		private static int[] maskBuffer;

		public static Mesh BuildMesh(VoxelGrid grid)
		{
			List<Vector3> list = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Color32> colors = new List<Color32>();
			List<int> triangles = new List<int>();
			foreach (VoxelGrid.ChunkView chunk in grid.Chunks)
			{
				EmitChunk(grid, chunk, list, normals, colors);
			}
			BuildQuadTriangles(triangles, list.Count / 4);
			Mesh mesh = new Mesh();
			mesh.indexFormat = ((list.Count > 65000) ? IndexFormat.UInt32 : IndexFormat.UInt16);
			mesh.SetVertices(list);
			mesh.SetNormals(normals);
			mesh.SetColors(colors);
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateBounds();
			return mesh;
		}

		internal static void EmitChunk(VoxelGrid grid, VoxelGrid.ChunkView chunk, List<Vector3> vertices, List<Vector3> normals, List<Color32> colors)
		{
			int size = VoxelGrid.Size;
			if (maskBuffer == null || maskBuffer.Length < size * size)
			{
				maskBuffer = new int[size * size];
			}
			for (int i = 0; i < Faces.Length; i++)
			{
				FaceDef face = Faces[i];
				int num = ((face.Normal.x == 0) ? ((face.Normal.y != 0) ? 1 : 2) : 0);
				int axisU = ((num == 0) ? 1 : 0);
				int axisV = ((num == 2) ? 1 : 2);
				for (int j = 0; j < size; j++)
				{
					BuildMask(grid, chunk, face, i, num, axisU, axisV, j, size);
					MergeMask(chunk, face, num, axisU, axisV, j, size, vertices, normals, colors);
				}
			}
		}

		private static void BuildMask(VoxelGrid grid, VoxelGrid.ChunkView chunk, FaceDef face, int faceIndex, int axisN, int axisU, int axisV, int layer, int size)
		{
			int[] array = new int[3];
			bool flag = grid.FaceColorCount > 0;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					array[axisN] = layer;
					array[axisU] = j;
					array[axisV] = i;
					int num = VoxelGrid.LocalIndex(array[0], array[1], array[2]);
					if (!VoxelGrid.Occupied(chunk.Occupancy, num))
					{
						maskBuffer[i * size + j] = 0;
						continue;
					}
					int num2 = array[0] + face.Normal.x;
					int num3 = array[1] + face.Normal.y;
					int num4 = array[2] + face.Normal.z;
					bool flag2 = (((uint)num2 < (uint)size && (uint)num3 < (uint)size && (uint)num4 < (uint)size) ? VoxelGrid.Occupied(chunk.Occupancy, VoxelGrid.LocalIndex(num2, num3, num4)) : grid.Contains(chunk.Origin + new Vector3Int(num2, num3, num4)));
					Color32 color = chunk.Colors[num];
					if (flag && grid.TryGetFaceColor(chunk.Origin + new Vector3Int(array[0], array[1], array[2]), faceIndex, out var color2))
					{
						color = color2;
					}
					maskBuffer[i * size + j] = ((!flag2) ? (((color.r << 16) | (color.g << 8) | color.b) + 1) : 0);
				}
			}
		}

		private static void MergeMask(VoxelGrid.ChunkView chunk, FaceDef face, int axisN, int axisU, int axisV, int layer, int size, List<Vector3> vertices, List<Vector3> normals, List<Color32> colors)
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					int num = maskBuffer[i * size + j];
					if (num == 0)
					{
						continue;
					}
					int k;
					for (k = 1; j + k < size && maskBuffer[i * size + j + k] == num; k++)
					{
					}
					int num2 = 1;
					bool flag = true;
					while (flag && i + num2 < size)
					{
						for (int l = 0; l < k; l++)
						{
							if (maskBuffer[(i + num2) * size + j + l] != num)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							num2++;
						}
					}
					for (int m = 0; m < num2; m++)
					{
						for (int n = 0; n < k; n++)
						{
							maskBuffer[(i + m) * size + j + n] = 0;
						}
					}
					EmitQuad(chunk, face, axisN, axisU, axisV, layer, j, j + k - 1, i, i + num2 - 1, num - 1, vertices, normals, colors);
					j += k - 1;
				}
			}
		}

		private static void EmitQuad(VoxelGrid.ChunkView chunk, FaceDef face, int axisN, int axisU, int axisV, int layer, int u0, int u1, int v0, int v1, int packed, List<Vector3> vertices, List<Vector3> normals, List<Color32> colors)
		{
			Color32 item = new Color32((byte)(packed >> 16), (byte)((packed >> 8) & 0xFF), (byte)(packed & 0xFF), byte.MaxValue);
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector = face.Corners[i];
				Vector3 vector2 = new Vector3
				{
					[axisU] = ((vector[axisU] == 0f) ? u0 : (u1 + 1)),
					[axisV] = ((vector[axisV] == 0f) ? v0 : (v1 + 1)),
					[axisN] = (float)layer + vector[axisN]
				};
				vertices.Add(new Vector3((float)chunk.Origin.x + vector2.x, (float)chunk.Origin.y + vector2.y, (float)chunk.Origin.z + vector2.z));
				normals.Add(face.Normal);
				colors.Add(item);
			}
		}

		internal static void BuildQuadTriangles(List<int> triangles, int quadCount)
		{
			int num = quadCount * 6;
			if (triangles.Count == num)
			{
				return;
			}
			if (triangles.Count > num)
			{
				triangles.RemoveRange(num, triangles.Count - num);
				return;
			}
			for (int i = triangles.Count / 6; i < quadCount; i++)
			{
				int num2 = i * 4;
				triangles.Add(num2);
				triangles.Add(num2 + 1);
				triangles.Add(num2 + 2);
				triangles.Add(num2 + 2);
				triangles.Add(num2 + 1);
				triangles.Add(num2 + 3);
			}
		}
	}
}

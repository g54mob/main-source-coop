using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	[Serializable]
	public class MeshVoxelizer
	{
		[Flags]
		public enum Voxel
		{
			Empty = 0,
			Inside = 1,
			Boundary = 2,
			Outside = 4
		}

		public static readonly Vector3Int[] fullNeighborhood = new Vector3Int[26]
		{
			new Vector3Int(-1, 0, 0),
			new Vector3Int(1, 0, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, 0, -1),
			new Vector3Int(0, 0, 1),
			new Vector3Int(-1, -1, 0),
			new Vector3Int(-1, 0, -1),
			new Vector3Int(-1, 0, 1),
			new Vector3Int(-1, 1, 0),
			new Vector3Int(0, -1, -1),
			new Vector3Int(0, -1, 1),
			new Vector3Int(0, 1, -1),
			new Vector3Int(0, 1, 1),
			new Vector3Int(1, -1, 0),
			new Vector3Int(1, 0, -1),
			new Vector3Int(1, 0, 1),
			new Vector3Int(1, 1, 0),
			new Vector3Int(-1, -1, -1),
			new Vector3Int(-1, -1, 1),
			new Vector3Int(-1, 1, -1),
			new Vector3Int(-1, 1, 1),
			new Vector3Int(1, -1, -1),
			new Vector3Int(1, -1, 1),
			new Vector3Int(1, 1, -1),
			new Vector3Int(1, 1, 1)
		};

		public static readonly Vector3Int[] edgefaceNeighborhood = new Vector3Int[18]
		{
			new Vector3Int(-1, -1, 0),
			new Vector3Int(-1, 0, -1),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(-1, 0, 1),
			new Vector3Int(-1, 1, 0),
			new Vector3Int(0, -1, -1),
			new Vector3Int(0, -1, 0),
			new Vector3Int(0, -1, 1),
			new Vector3Int(0, 0, -1),
			new Vector3Int(0, 0, 1),
			new Vector3Int(0, 1, -1),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, 1, 1),
			new Vector3Int(1, -1, 0),
			new Vector3Int(1, 0, -1),
			new Vector3Int(1, 0, 0),
			new Vector3Int(1, 0, 1),
			new Vector3Int(1, 1, 0)
		};

		public static readonly Vector3Int[] faceNeighborhood = new Vector3Int[6]
		{
			new Vector3Int(-1, 0, 0),
			new Vector3Int(1, 0, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, 0, -1),
			new Vector3Int(0, 0, 1)
		};

		public static readonly Vector3Int[] edgeNeighborhood = new Vector3Int[12]
		{
			new Vector3Int(-1, -1, 0),
			new Vector3Int(-1, 0, -1),
			new Vector3Int(-1, 0, 1),
			new Vector3Int(-1, 1, 0),
			new Vector3Int(0, -1, -1),
			new Vector3Int(0, -1, 1),
			new Vector3Int(0, 1, -1),
			new Vector3Int(0, 1, 1),
			new Vector3Int(1, -1, 0),
			new Vector3Int(1, 0, -1),
			new Vector3Int(1, 0, 1),
			new Vector3Int(1, 1, 0)
		};

		public static readonly Vector3Int[] vertexNeighborhood = new Vector3Int[8]
		{
			new Vector3Int(-1, -1, -1),
			new Vector3Int(-1, -1, 1),
			new Vector3Int(-1, 1, -1),
			new Vector3Int(-1, 1, 1),
			new Vector3Int(1, -1, -1),
			new Vector3Int(1, -1, 1),
			new Vector3Int(1, 1, -1),
			new Vector3Int(1, 1, 1)
		};

		[NonSerialized]
		public Mesh input;

		[HideInInspector]
		[SerializeField]
		private Voxel[] voxels;

		public float voxelSize;

		public Vector3Int resolution;

		private List<int>[] triangleIndices;

		private Vector3Int origin;

		public Vector3Int Origin => origin;

		public int voxelCount => resolution.x * resolution.y * resolution.z;

		public Voxel this[int x, int y, int z]
		{
			get
			{
				return voxels[GetVoxelIndex(x, y, z)];
			}
			set
			{
				voxels[GetVoxelIndex(x, y, z)] = value;
			}
		}

		public MeshVoxelizer(Mesh input, float voxelSize)
		{
			this.input = input;
			this.voxelSize = voxelSize;
		}

		public float GetDistanceToNeighbor(int i)
		{
			if (i > 17)
			{
				return 1.7320508f * voxelSize;
			}
			if (i > 5)
			{
				return 1.4142135f * voxelSize;
			}
			return voxelSize;
		}

		public int GetVoxelIndex(int x, int y, int z)
		{
			return x + resolution.x * (y + resolution.y * z);
		}

		public Vector3 GetVoxelCenter(in Vector3Int coords)
		{
			return new Vector3((float)(Origin.x + coords.x) + 0.5f, (float)(Origin.y + coords.y) + 0.5f, (float)(Origin.z + coords.z) + 0.5f) * voxelSize;
		}

		private Bounds GetTriangleBounds(in Vector3 v1, in Vector3 v2, in Vector3 v3)
		{
			Bounds result = new Bounds(v1, Vector3.zero);
			result.Encapsulate(v2);
			result.Encapsulate(v3);
			return result;
		}

		public List<int> GetTrianglesOverlappingVoxel(int voxelIndex)
		{
			if (voxelIndex >= 0 && voxelIndex < triangleIndices.Length)
			{
				return triangleIndices[voxelIndex];
			}
			return null;
		}

		public Vector3Int GetPointVoxel(in Vector3 point)
		{
			return new Vector3Int(Mathf.FloorToInt(point.x / voxelSize), Mathf.FloorToInt(point.y / voxelSize), Mathf.FloorToInt(point.z / voxelSize));
		}

		public bool VoxelExists(in Vector3Int coords)
		{
			return VoxelExists(coords.x, coords.y, coords.z);
		}

		public bool VoxelExists(int x, int y, int z)
		{
			if (x >= 0 && y >= 0 && z >= 0 && x < resolution.x && y < resolution.y)
			{
				return z < resolution.z;
			}
			return false;
		}

		private void AppendOverlappingVoxels(in Bounds bounds, in Vector3 v1, in Vector3 v2, in Vector3 v3, int triangleIndex)
		{
			Vector3Int pointVoxel = GetPointVoxel(bounds.min);
			Vector3Int pointVoxel2 = GetPointVoxel(bounds.max);
			for (int i = pointVoxel.x; i <= pointVoxel2.x; i++)
			{
				for (int j = pointVoxel.y; j <= pointVoxel2.y; j++)
				{
					for (int k = pointVoxel.z; k <= pointVoxel2.z; k++)
					{
						if (IsIntersecting(new Bounds(new Vector3((float)i + 0.5f, (float)j + 0.5f, (float)k + 0.5f) * voxelSize, Vector3.one * voxelSize), v1, v2, v3))
						{
							int voxelIndex = GetVoxelIndex(i - origin.x, j - origin.y, k - origin.z);
							voxels[voxelIndex] = Voxel.Boundary;
							if (triangleIndices != null)
							{
								triangleIndices[voxelIndex].Add(triangleIndex);
							}
						}
					}
				}
			}
		}

		public IEnumerator Voxelize(Matrix4x4 transform, Vector3Int axisMask, bool generateTriangleIndices = false)
		{
			voxelSize = Mathf.Max(0.0001f, voxelSize);
			Bounds bounds = input.bounds.Transform(transform);
			origin = GetPointVoxel(Vector3.Scale(bounds.min, axisMask)) - axisMask;
			Vector3Int vector3Int = GetPointVoxel(Vector3.Scale(bounds.max, axisMask)) + axisMask;
			resolution = new Vector3Int(vector3Int.x - origin.x + 1, vector3Int.y - origin.y + 1, vector3Int.z - origin.z + 1);
			voxels = new Voxel[resolution.x * resolution.y * resolution.z];
			for (int i = 0; i < resolution.x; i++)
			{
				for (int j = 0; j < resolution.y; j++)
				{
					for (int k = 0; k < resolution.z; k++)
					{
						this[i, j, k] = Voxel.Inside;
					}
				}
			}
			if (generateTriangleIndices)
			{
				triangleIndices = new List<int>[voxels.Length];
				for (int l = 0; l < triangleIndices.Length; l++)
				{
					triangleIndices[l] = new List<int>(4);
				}
			}
			else
			{
				triangleIndices = null;
			}
			int[] triIndices = input.triangles;
			Vector3[] vertices = input.vertices;
			for (int m = 0; m < triIndices.Length; m += 3)
			{
				Vector3 v = Vector3.Scale(transform.MultiplyPoint3x4(vertices[triIndices[m]]), axisMask);
				Vector3 v2 = Vector3.Scale(transform.MultiplyPoint3x4(vertices[triIndices[m + 1]]), axisMask);
				Vector3 v3 = Vector3.Scale(transform.MultiplyPoint3x4(vertices[triIndices[m + 2]]), axisMask);
				AppendOverlappingVoxels(GetTriangleBounds(in v, in v2, in v3), in v, in v2, in v3, m / 3);
				if (m % 150 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("Voxelizing mesh...", (float)m / (float)triIndices.Length);
				}
			}
			IEnumerator fillCoroutine = FloodFill();
			while (fillCoroutine.MoveNext())
			{
				yield return fillCoroutine.Current;
			}
		}

		public void BoundaryThinning()
		{
			for (int i = 0; i < resolution.x; i++)
			{
				for (int j = 0; j < resolution.y; j++)
				{
					for (int k = 0; k < resolution.z; k++)
					{
						if (this[i, j, k] == Voxel.Boundary)
						{
							this[i, j, k] = Voxel.Inside;
						}
					}
				}
			}
			for (int l = 0; l < resolution.x; l++)
			{
				for (int m = 0; m < resolution.y; m++)
				{
					for (int n = 0; n < resolution.z; n++)
					{
						int num = 0;
						for (int num2 = 0; num2 < faceNeighborhood.Length; num2++)
						{
							Vector3Int vector3Int = faceNeighborhood[num2];
							if (VoxelExists(vector3Int.x + l, vector3Int.y + m, vector3Int.z + n) && this[vector3Int.x + l, vector3Int.y + m, vector3Int.z + n] != Voxel.Outside)
							{
								num++;
							}
						}
						if (num % faceNeighborhood.Length != 0 && this[l, m, n] == Voxel.Inside)
						{
							this[l, m, n] = Voxel.Boundary;
						}
					}
				}
			}
		}

		public void MakeBoundary2D()
		{
			for (int i = 0; i < resolution.x; i++)
			{
				for (int j = 0; j < resolution.y; j++)
				{
					for (int k = 0; k < resolution.z; k++)
					{
						if (this[i, j, k] == Voxel.Boundary)
						{
							this[i, j, k] = Voxel.Inside;
						}
					}
				}
			}
			for (int l = 0; l < resolution.x; l++)
			{
				for (int m = 0; m < resolution.y; m++)
				{
					for (int n = 0; n < resolution.z; n++)
					{
						int num = 0;
						for (int num2 = 0; num2 < faceNeighborhood.Length; num2++)
						{
							Vector3Int vector3Int = faceNeighborhood[num2];
							if (VoxelExists(vector3Int.x + l, vector3Int.y + m, vector3Int.z + n) && this[vector3Int.x + l, vector3Int.y + m, vector3Int.z + n] != Voxel.Outside)
							{
								num++;
							}
						}
						if (num <= 3 && this[l, m, n] == Voxel.Inside)
						{
							this[l, m, n] = Voxel.Boundary;
						}
					}
				}
			}
		}

		public void CreateMesh(ref Mesh mesh, int smoothingIterations)
		{
			if (mesh == null)
			{
				mesh = new Mesh();
			}
			mesh.indexFormat = IndexFormat.UInt32;
			mesh.Clear();
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<int> list3 = new List<int>();
			list.Clear();
			list2.Clear();
			list3.Clear();
			int[] array = new int[voxelCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = -1;
			}
			for (int j = 0; j < resolution.x; j++)
			{
				for (int k = 0; k < resolution.y; k++)
				{
					for (int l = 0; l < resolution.z; l++)
					{
						if (this[j, k, l] == Voxel.Boundary)
						{
							array[GetVoxelIndex(j, k, l)] = list.Count;
							Vector3 item = new Vector3((float)(Origin.x + j) + 0.5f, (float)(Origin.y + k) + 0.5f, (float)(Origin.z + l) + 0.5f) * voxelSize;
							list.Add(item);
							list2.Add(item);
						}
					}
				}
			}
			List<Vector3> list4 = list;
			List<Vector3> list5 = list2;
			for (int m = 0; m < smoothingIterations; m++)
			{
				for (int n = 0; n < resolution.x; n++)
				{
					for (int num = 0; num < resolution.y; num++)
					{
						for (int num2 = 0; num2 < resolution.z; num2++)
						{
							if (this[n, num, num2] != Voxel.Boundary)
							{
								continue;
							}
							Vector3 zero = Vector3.zero;
							int num3 = 0;
							for (int num4 = 0; num4 < faceNeighborhood.Length; num4++)
							{
								Vector3Int vector3Int = faceNeighborhood[num4];
								if (VoxelExists(vector3Int.x + n, vector3Int.y + num, vector3Int.z + num2) && this[vector3Int.x + n, vector3Int.y + num, vector3Int.z + num2] == Voxel.Boundary)
								{
									zero += list4[array[GetVoxelIndex(vector3Int.x + n, vector3Int.y + num, vector3Int.z + num2)]];
									num3++;
								}
							}
							if (num3 > 0)
							{
								list5[array[GetVoxelIndex(n, num, num2)]] = zero / num3;
							}
						}
					}
				}
				List<Vector3> list6 = list4;
				list4 = list5;
				list5 = list6;
			}
			for (int num5 = 0; num5 < resolution.x; num5++)
			{
				for (int num6 = 0; num6 < resolution.y; num6++)
				{
					for (int num7 = 0; num7 < resolution.z; num7++)
					{
						if (this[num5, num6, num7] != Voxel.Boundary)
						{
							continue;
						}
						int voxelIndex = GetVoxelIndex(num5, num6, num7);
						int num8 = (VoxelExists(num5 + 1, num6, num7) ? GetVoxelIndex(num5 + 1, num6, num7) : (-1));
						int num9 = (VoxelExists(num5 + 1, num6 + 1, num7) ? GetVoxelIndex(num5 + 1, num6 + 1, num7) : (-1));
						int num10 = (VoxelExists(num5, num6 + 1, num7) ? GetVoxelIndex(num5, num6 + 1, num7) : (-1));
						int num11 = (VoxelExists(num5, num6, num7 + 1) ? GetVoxelIndex(num5, num6, num7 + 1) : (-1));
						int num12 = (VoxelExists(num5, num6 + 1, num7 + 1) ? GetVoxelIndex(num5, num6 + 1, num7 + 1) : (-1));
						int num13 = (VoxelExists(num5 + 1, num6, num7 + 1) ? GetVoxelIndex(num5 + 1, num6, num7 + 1) : (-1));
						int num14 = (VoxelExists(num5 + 1, num6 + 1, num7 + 1) ? GetVoxelIndex(num5 + 1, num6 + 1, num7 + 1) : (-1));
						if (num8 >= 0 && num9 >= 0 && num10 >= 0 && voxels[num8] == Voxel.Boundary && voxels[num9] == Voxel.Boundary && voxels[num10] == Voxel.Boundary)
						{
							if (num12 < 0 || voxels[num12] == Voxel.Outside || num11 < 0 || voxels[num11] == Voxel.Outside || num13 < 0 || voxels[num13] == Voxel.Outside || num14 < 0 || voxels[num14] == Voxel.Outside)
							{
								list3.Add(array[voxelIndex]);
								list3.Add(array[num8]);
								list3.Add(array[num10]);
								list3.Add(array[num10]);
								list3.Add(array[num8]);
								list3.Add(array[num9]);
							}
							else
							{
								list3.Add(array[num8]);
								list3.Add(array[voxelIndex]);
								list3.Add(array[num10]);
								list3.Add(array[num8]);
								list3.Add(array[num10]);
								list3.Add(array[num9]);
							}
						}
						if (num8 >= 0 && num13 >= 0 && num11 >= 0 && voxels[num8] == Voxel.Boundary && voxels[num13] == Voxel.Boundary && voxels[num11] == Voxel.Boundary)
						{
							if (num10 < 0 || voxels[num10] == Voxel.Outside || num12 < 0 || voxels[num12] == Voxel.Outside || num9 < 0 || voxels[num9] == Voxel.Outside || num14 < 0 || voxels[num14] == Voxel.Outside)
							{
								list3.Add(array[num8]);
								list3.Add(array[voxelIndex]);
								list3.Add(array[num11]);
								list3.Add(array[num8]);
								list3.Add(array[num11]);
								list3.Add(array[num13]);
							}
							else
							{
								list3.Add(array[voxelIndex]);
								list3.Add(array[num8]);
								list3.Add(array[num11]);
								list3.Add(array[num11]);
								list3.Add(array[num8]);
								list3.Add(array[num13]);
							}
						}
						if (num11 >= 0 && num12 >= 0 && num10 >= 0 && voxels[num11] == Voxel.Boundary && voxels[num12] == Voxel.Boundary && voxels[num10] == Voxel.Boundary)
						{
							if (num8 < 0 || voxels[num8] == Voxel.Outside || num13 < 0 || voxels[num13] == Voxel.Outside || num9 < 0 || voxels[num9] == Voxel.Outside || num14 < 0 || voxels[num14] == Voxel.Outside)
							{
								list3.Add(array[num11]);
								list3.Add(array[voxelIndex]);
								list3.Add(array[num10]);
								list3.Add(array[num11]);
								list3.Add(array[num10]);
								list3.Add(array[num12]);
							}
							else
							{
								list3.Add(array[voxelIndex]);
								list3.Add(array[num11]);
								list3.Add(array[num10]);
								list3.Add(array[num10]);
								list3.Add(array[num11]);
								list3.Add(array[num12]);
							}
						}
					}
				}
			}
			mesh.SetVertices(list5);
			mesh.SetIndices(list3, MeshTopology.Triangles, 0);
			mesh.RecalculateNormals();
		}

		private IEnumerator FloodFill()
		{
			Queue<Vector3Int> queue = new Queue<Vector3Int>();
			queue.Enqueue(new Vector3Int(0, 0, 0));
			this[0, 0, 0] = Voxel.Outside;
			int i = 0;
			while (queue.Count > 0)
			{
				Vector3Int vector3Int = queue.Dequeue();
				if (vector3Int.x < resolution.x - 1 && this[vector3Int.x + 1, vector3Int.y, vector3Int.z] == Voxel.Inside)
				{
					Vector3Int item = new Vector3Int(vector3Int.x + 1, vector3Int.y, vector3Int.z);
					this[item.x, item.y, item.z] = Voxel.Outside;
					queue.Enqueue(item);
				}
				if (vector3Int.x > 0 && this[vector3Int.x - 1, vector3Int.y, vector3Int.z] == Voxel.Inside)
				{
					Vector3Int item = new Vector3Int(vector3Int.x - 1, vector3Int.y, vector3Int.z);
					this[item.x, item.y, item.z] = Voxel.Outside;
					queue.Enqueue(item);
				}
				if (vector3Int.y < resolution.y - 1 && this[vector3Int.x, vector3Int.y + 1, vector3Int.z] == Voxel.Inside)
				{
					Vector3Int item = new Vector3Int(vector3Int.x, vector3Int.y + 1, vector3Int.z);
					this[item.x, item.y, item.z] = Voxel.Outside;
					queue.Enqueue(item);
				}
				if (vector3Int.y > 0 && this[vector3Int.x, vector3Int.y - 1, vector3Int.z] == Voxel.Inside)
				{
					Vector3Int item = new Vector3Int(vector3Int.x, vector3Int.y - 1, vector3Int.z);
					this[item.x, item.y, item.z] = Voxel.Outside;
					queue.Enqueue(item);
				}
				if (vector3Int.z < resolution.z - 1 && this[vector3Int.x, vector3Int.y, vector3Int.z + 1] == Voxel.Inside)
				{
					Vector3Int item = new Vector3Int(vector3Int.x, vector3Int.y, vector3Int.z + 1);
					this[item.x, item.y, item.z] = Voxel.Outside;
					queue.Enqueue(item);
				}
				if (vector3Int.z > 0 && this[vector3Int.x, vector3Int.y, vector3Int.z - 1] == Voxel.Inside)
				{
					Vector3Int item = new Vector3Int(vector3Int.x, vector3Int.y, vector3Int.z - 1);
					this[item.x, item.y, item.z] = Voxel.Outside;
					queue.Enqueue(item);
				}
				int num = i + 1;
				i = num;
				if (num % 150 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("Filling mesh...", (float)i / (float)voxels.Length);
				}
			}
		}

		public static bool IsIntersecting(in Bounds box, Vector3 v1, Vector3 v2, Vector3 v3)
		{
			v1 -= box.center;
			v2 -= box.center;
			v3 -= box.center;
			Vector3 lhs = v2 - v1;
			Vector3 rhs = v3 - v2;
			Vector3 vector = v1 - v3;
			Vector3 axis = new Vector3(0f, 0f - lhs.z, lhs.y);
			Vector3 axis2 = new Vector3(0f, 0f - rhs.z, rhs.y);
			Vector3 axis3 = new Vector3(0f, 0f - vector.z, vector.y);
			Vector3 axis4 = new Vector3(lhs.z, 0f, 0f - lhs.x);
			Vector3 axis5 = new Vector3(rhs.z, 0f, 0f - rhs.x);
			Vector3 axis6 = new Vector3(vector.z, 0f, 0f - vector.x);
			Vector3 axis7 = new Vector3(0f - lhs.y, lhs.x, 0f);
			Vector3 axis8 = new Vector3(0f - rhs.y, rhs.x, 0f);
			Vector3 axis9 = new Vector3(0f - vector.y, vector.x, 0f);
			if (!TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis2) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis3) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis4) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis5) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis6) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis7) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis8) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, in axis9) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, Vector3.right) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, Vector3.up) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, Vector3.forward) || !TriangleAabbSATTest(in v1, in v2, in v3, box.extents, Vector3.Cross(lhs, rhs)))
			{
				return false;
			}
			return true;
		}

		private static bool TriangleAabbSATTest(in Vector3 v0, in Vector3 v1, in Vector3 v2, in Vector3 aabbExtents, in Vector3 axis)
		{
			float a = Vector3.Dot(v0, axis);
			float a2 = Vector3.Dot(v1, axis);
			float b = Vector3.Dot(v2, axis);
			float num = aabbExtents.x * Mathf.Abs(axis.x) + aabbExtents.y * Mathf.Abs(axis.y) + aabbExtents.z * Mathf.Abs(axis.z);
			float num2 = Mathf.Max(a, Mathf.Max(a2, b));
			float b2 = Mathf.Min(a, Mathf.Min(a2, b));
			return !(Mathf.Max(0f - num2, b2) > num);
		}
	}
}

using System.Collections;
using UnityEngine;

namespace Obi
{
	public class VoxelDistanceField
	{
		public Vector3[,,] distanceField;

		private MeshVoxelizer voxelizer;

		public VoxelDistanceField(MeshVoxelizer voxelizer)
		{
			this.voxelizer = voxelizer;
		}

		public Vector4 SampleUnfiltered(int x, int y, int z)
		{
			x = Mathf.Clamp(x, 0, voxelizer.resolution.x - 1);
			y = Mathf.Clamp(y, 0, voxelizer.resolution.y - 1);
			z = Mathf.Clamp(z, 0, voxelizer.resolution.z - 1);
			Vector3 vector = distanceField[x, y, z];
			float magnitude = vector.magnitude;
			vector.Normalize();
			return new Vector4(vector.x, vector.y, vector.z, 0f - magnitude);
		}

		public Vector4 SampleFiltered(float x, float y, float z, Vector3Int axisMask)
		{
			Vector3 vector = new Vector3(x, y, z);
			Vector3 voxelCenter = voxelizer.GetVoxelCenter(new Vector3Int(0, 0, 0));
			Vector3 voxelCenter2 = voxelizer.GetVoxelCenter(new Vector3Int(voxelizer.resolution.x - 1, voxelizer.resolution.y - 1, voxelizer.resolution.z - 1));
			vector.x = Mathf.Clamp(vector.x, voxelCenter.x, voxelCenter2.x);
			vector.y = Mathf.Clamp(vector.y, voxelCenter.y, voxelCenter2.y);
			vector.z = Mathf.Clamp(vector.z, voxelCenter.z, voxelCenter2.z);
			Vector3Int coords = voxelizer.GetPointVoxel(vector - (Vector3)axisMask * voxelizer.voxelSize * 0.5f) - voxelizer.Origin;
			Vector3 voxelCenter3 = voxelizer.GetVoxelCenter(in coords);
			Vector3 vector2 = Vector3.Scale((vector - voxelCenter3) / voxelizer.voxelSize, axisMask);
			Vector4 a = SampleUnfiltered(coords.x, coords.y, coords.z);
			Vector4 a2 = SampleUnfiltered(coords.x, coords.y, coords.z + 1);
			Vector4 b = SampleUnfiltered(coords.x + 1, coords.y, coords.z);
			Vector4 b2 = SampleUnfiltered(coords.x + 1, coords.y, coords.z + 1);
			Vector4 a3 = SampleUnfiltered(coords.x, coords.y + 1, coords.z);
			Vector4 a4 = SampleUnfiltered(coords.x, coords.y + 1, coords.z + 1);
			Vector4 b3 = SampleUnfiltered(coords.x + 1, coords.y + 1, coords.z);
			Vector4 b4 = SampleUnfiltered(coords.x + 1, coords.y + 1, coords.z + 1);
			Vector4 a5 = Vector4.Lerp(a, b, vector2.x);
			Vector4 b5 = Vector4.Lerp(a2, b2, vector2.x);
			Vector4 a6 = Vector4.Lerp(a3, b3, vector2.x);
			Vector4 b6 = Vector4.Lerp(a4, b4, vector2.x);
			Vector4 a7 = Vector4.Lerp(a5, b5, vector2.z);
			Vector4 b7 = Vector4.Lerp(a6, b6, vector2.z);
			return Vector4.Lerp(a7, b7, vector2.y);
		}

		public void Smooth()
		{
			Vector3[,,] array = new Vector3[voxelizer.resolution.x, voxelizer.resolution.y, voxelizer.resolution.z];
			for (int i = 0; i < distanceField.GetLength(0); i++)
			{
				for (int j = 0; j < distanceField.GetLength(1); j++)
				{
					for (int k = 0; k < distanceField.GetLength(2); k++)
					{
						if (voxelizer[i, j, k] == MeshVoxelizer.Voxel.Outside)
						{
							continue;
						}
						Vector3Int vector3Int = new Vector3Int(i, j, k);
						Vector3 vector = distanceField[i, j, k];
						int num = 1;
						Vector3Int[] faceNeighborhood = MeshVoxelizer.faceNeighborhood;
						foreach (Vector3Int vector3Int2 in faceNeighborhood)
						{
							Vector3Int vector3Int3 = vector3Int + vector3Int2;
							if (voxelizer.VoxelExists(vector3Int3.x, vector3Int3.y, vector3Int3.z) && voxelizer[vector3Int3.x, vector3Int3.y, vector3Int3.z] != MeshVoxelizer.Voxel.Outside)
							{
								vector += distanceField[vector3Int3.x, vector3Int3.y, vector3Int3.z];
								num++;
							}
						}
						vector /= (float)num;
						array[i, j, k] = vector;
					}
				}
			}
			distanceField = array;
		}

		private void CalculateGradientsAndDistances(Vector3Int[,,] buffer1)
		{
			distanceField = new Vector3[voxelizer.resolution.x, voxelizer.resolution.y, voxelizer.resolution.z];
			for (int i = 0; i < buffer1.GetLength(0); i++)
			{
				for (int j = 0; j < buffer1.GetLength(1); j++)
				{
					for (int k = 0; k < buffer1.GetLength(2); k++)
					{
						if (voxelizer[i, j, k] != MeshVoxelizer.Voxel.Outside)
						{
							distanceField[i, j, k] = voxelizer.GetVoxelCenter(in buffer1[i, j, k]) - voxelizer.GetVoxelCenter(new Vector3Int(i, j, k));
						}
						else
						{
							distanceField[i, j, k] = Vector3.zero;
						}
					}
				}
			}
		}

		public IEnumerator JumpFlood()
		{
			Vector3Int[,,] buffer1 = new Vector3Int[voxelizer.resolution.x, voxelizer.resolution.y, voxelizer.resolution.z];
			Vector3Int[,,] buffer2 = new Vector3Int[voxelizer.resolution.x, voxelizer.resolution.y, voxelizer.resolution.z];
			for (int i = 0; i < buffer1.GetLength(0); i++)
			{
				for (int j = 0; j < buffer1.GetLength(1); j++)
				{
					for (int k = 0; k < buffer1.GetLength(2); k++)
					{
						if (voxelizer[i, j, k] == MeshVoxelizer.Voxel.Outside)
						{
							buffer1[i, j, k] = new Vector3Int(i, j, k);
						}
						else
						{
							buffer1[i, j, k] = new Vector3Int(-1, -1, -1);
						}
					}
				}
			}
			int size = Mathf.Max(buffer1.GetLength(0), buffer1.GetLength(1), buffer1.GetLength(2));
			int step = (int)((float)size / 2f);
			yield return new CoroutineJob.ProgressInfo("Generating voxel distance field...", 0f);
			float numPasses = (int)Mathf.Log(size, 2f);
			int i2 = 0;
			while (step >= 1)
			{
				JumpFloodPass(step, buffer1, buffer2);
				step /= 2;
				Vector3Int[,,] array = buffer1;
				buffer1 = buffer2;
				buffer2 = array;
				int num = i2 + 1;
				i2 = num;
				yield return new CoroutineJob.ProgressInfo("Generating voxel distance field...", (float)num / numPasses);
			}
			CalculateGradientsAndDistances(buffer1);
		}

		private void JumpFloodPass(int stride, Vector3Int[,,] input, Vector3Int[,,] output)
		{
			for (int i = 0; i < input.GetLength(0); i++)
			{
				for (int j = 0; j < input.GetLength(1); j++)
				{
					for (int k = 0; k < input.GetLength(2); k++)
					{
						Vector3Int vector3Int = new Vector3Int(i, j, k);
						Vector3Int vector3Int2 = (output[i, j, k] = input[i, j, k]);
						if (vector3Int2.x == i && vector3Int2.y == j && vector3Int2.z == k)
						{
							continue;
						}
						float num = float.MaxValue;
						if (vector3Int2.x >= 0)
						{
							num = (vector3Int2 - vector3Int).sqrMagnitude;
						}
						Vector3Int[] fullNeighborhood = MeshVoxelizer.fullNeighborhood;
						foreach (Vector3Int vector3Int3 in fullNeighborhood)
						{
							Vector3Int vector3Int4 = vector3Int + vector3Int3 * stride;
							if (!voxelizer.VoxelExists(vector3Int4.x, vector3Int4.y, vector3Int4.z))
							{
								continue;
							}
							Vector3Int vector3Int5 = input[vector3Int4.x, vector3Int4.y, vector3Int4.z];
							if (vector3Int5.x >= 0)
							{
								float num2 = (vector3Int5 - vector3Int).sqrMagnitude;
								if (num2 < num)
								{
									output[i, j, k] = vector3Int5;
									num = num2;
								}
							}
						}
					}
				}
			}
		}
	}
}

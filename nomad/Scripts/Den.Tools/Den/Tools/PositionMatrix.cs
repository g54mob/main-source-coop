using System;
using Den.Tools.Matrices;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class PositionMatrix : Matrix2D<Vector3>
	{
		public Vector3 worldPos;

		public Vector3 worldSize;

		public float cellSize;

		public int margins;

		public PositionMatrix(CoordRect rect, Vector3 worldPos, Vector3 worldSize)
		{
			base.rect = rect;
			this.worldPos = worldPos;
			this.worldSize = worldSize;
			cellSize = worldSize.x / (float)rect.size.x;
			count = rect.size.x * rect.size.z;
			arr = new Vector3[count];
		}

		public Coord GetCoord(Vector3 worldPos)
		{
			int num = (int)(worldPos.x / cellSize);
			if (worldPos.x < 0f)
			{
				num--;
			}
			int num2 = (int)(worldPos.z / cellSize);
			if (worldPos.z < 0f)
			{
				num2--;
			}
			return new Coord(num, num2);
		}

		public void SetPosition(Vector3 worldPos)
		{
			int num = (int)(worldPos.x / cellSize);
			if (worldPos.x < 0f)
			{
				num--;
			}
			int num2 = (int)(worldPos.z / cellSize);
			if (worldPos.z < 0f)
			{
				num2--;
			}
			arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x] = worldPos;
		}

		public void SetHeight(int x, int z, float height)
		{
			arr[(z - rect.offset.z) * rect.size.x + x - rect.offset.x].y = height;
		}

		public float GetHeight(int x, int z)
		{
			return arr[(z - rect.offset.z) * rect.size.x + x - rect.offset.x].y;
		}

		public void Scatter(float uniformity, Noise rnd, float maxHeight = 1f)
		{
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					Vector3 vector = new Vector3((float)i * cellSize + cellSize / 2f, 0f, (float)j * cellSize + cellSize / 2f);
					if (uniformity < 1f)
					{
						Vector3 vector2 = new Vector3((float)i * cellSize + rnd.Random(i, j, 0) * cellSize, rnd.Random(i, j, 2) * maxHeight, (float)j * cellSize + rnd.Random(i, j, 1) * cellSize);
						vector = vector * uniformity + vector2 * (1f - uniformity);
					}
					base[i, j] = vector;
				}
			}
		}

		public PositionMatrix Relaxed(float strength = 1f)
		{
			float num = strength * cellSize;
			PositionMatrix positionMatrix = new PositionMatrix(rect, worldPos, worldSize);
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					Vector3 vector = base[i, j];
					Vector3 vector2 = default(Vector3);
					for (int k = -1; k <= 1; k++)
					{
						for (int l = -1; l <= 1; l++)
						{
							if (k != 0 || l != 0)
							{
								int num2 = i + k;
								int num3 = j + l;
								if (num2 >= min.x && num2 < max.x && num3 >= min.z && num3 < max.z)
								{
									Vector3 vector3 = arr[(num3 - rect.offset.z) * rect.size.x + num2 - rect.offset.x];
									Vector3 vector4 = vector - vector3;
									vector2 += vector4.normalized * (1f / vector4.sqrMagnitude);
								}
							}
						}
					}
					vector += vector2 * num;
					if (vector.x < (float)i * cellSize)
					{
						vector.x = (float)i * cellSize;
					}
					if (vector.x > (float)(i + 1) * cellSize)
					{
						vector.x = (float)(i + 1) * cellSize;
					}
					if (vector.z < (float)j * cellSize)
					{
						vector.z = (float)j * cellSize;
					}
					if (vector.z > (float)(j + 1) * cellSize)
					{
						vector.z = (float)(j + 1) * cellSize;
					}
					positionMatrix[i, j] = vector;
				}
			}
			return positionMatrix;
		}

		public void CleanUp(Matrix probMatrix, Noise rnd)
		{
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - rect.offset.z) * rect.size.x + i - rect.offset.x;
					Vector3 vector = arr[num];
					float num2 = (vector.x - worldPos.x) / worldSize.x;
					float num3 = (vector.z - worldPos.z) / worldSize.z;
					float num4 = num2 * (float)probMatrix.rect.size.x + (float)probMatrix.rect.offset.x;
					float num5 = num3 * (float)probMatrix.rect.size.z + (float)probMatrix.rect.offset.z;
					if (num4 < (float)probMatrix.rect.offset.x)
					{
						num4 = probMatrix.rect.offset.x;
					}
					if (num5 < (float)probMatrix.rect.offset.z)
					{
						num5 = probMatrix.rect.offset.z;
					}
					if (num4 >= (float)(probMatrix.rect.offset.x + probMatrix.rect.size.x - 1))
					{
						num4 = probMatrix.rect.offset.x + probMatrix.rect.size.x - 1;
					}
					if (num5 >= (float)(probMatrix.rect.offset.z + probMatrix.rect.size.z - 1))
					{
						num5 = probMatrix.rect.offset.z + probMatrix.rect.size.z - 1;
					}
					float num6 = probMatrix[(int)num4, (int)num5];
					float num7 = rnd.Random(i, j, 0);
					if (num6 < num7)
					{
						arr[num].y = -1f / 0f;
					}
				}
			}
		}

		public void GetTwoClosest(Vector3 worldPos, out Vector3 closest, out Vector3 secondClosest, out float minDist, out float secondMinDist)
		{
			int num = (int)(worldPos.x / cellSize);
			if (worldPos.x < 0f)
			{
				num--;
			}
			int num2 = (int)(worldPos.z / cellSize);
			if (worldPos.z < 0f)
			{
				num2--;
			}
			Vector3 vector = arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x];
			closest = (secondClosest = vector);
			minDist = (secondMinDist = 200000000f);
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					int num3 = num + i;
					int num4 = num2 + j;
					if (num3 >= rect.offset.x && num3 < rect.offset.x + rect.size.x && num4 >= rect.offset.z && num4 < rect.offset.z + rect.size.z)
					{
						Vector3 vector2 = arr[(num4 - rect.offset.z) * rect.size.x + num3 - rect.offset.x];
						float num5 = (worldPos.x - vector2.x) * (worldPos.x - vector2.x) + (worldPos.z - vector2.z) * (worldPos.z - vector2.z);
						if (num5 < minDist)
						{
							secondMinDist = minDist;
							minDist = num5;
							secondClosest = closest;
							closest = vector2;
						}
						else if (num5 < secondMinDist)
						{
							secondMinDist = num5;
							secondClosest = vector2;
						}
					}
				}
			}
		}

		public void FillPosTab(PosTab posTab, float minHeight = -200000000f)
		{
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					Vector3 vector = base[i, j];
					if (!(vector.x < (float)i * cellSize) && !(vector.x > (float)(i + 1) * cellSize) && !(vector.z < (float)j * cellSize) && !(vector.z > (float)(j + 1) * cellSize) && !(vector.x < posTab.pos.x) && !(vector.x > posTab.pos.x + posTab.size.x) && !(vector.z < posTab.pos.z) && !(vector.z > posTab.pos.z + posTab.size.z) && !(vector.y < minHeight))
					{
						Transition trs = new Transition(vector.x, vector.z);
						trs.hash = i * 2000 + j;
						posTab.Add(trs);
					}
				}
			}
		}

		public Vector3[] ToArray()
		{
			Vector3[] array = new Vector3[rect.size.x * rect.size.z];
			int num = 0;
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					array[num] = base[i, j];
					num++;
				}
			}
			return array;
		}

		public void AddTransitionsList(TransitionsList trns)
		{
			for (int i = 0; i < trns.count; i++)
			{
				SetPosition(trns.arr[i].pos);
			}
		}

		public void AddTransitionsList(TransitionsList trns, float customHeight)
		{
			for (int i = 0; i < trns.count; i++)
			{
				if (!(trns.arr[i].pos.x < worldPos.x) && !(trns.arr[i].pos.x > worldPos.x + worldSize.x) && !(trns.arr[i].pos.z < worldPos.z) && !(trns.arr[i].pos.z > worldPos.z + worldSize.z))
				{
					SetPosition(new Vector3(trns.arr[i].pos.x, customHeight, trns.arr[i].pos.z));
				}
			}
		}

		public TransitionsList ToTransitionsList()
		{
			TransitionsList transitionsList = new TransitionsList();
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					Vector3 vector = base[i, j];
					Transition trs = new Transition(vector.x, vector.z);
					trs.hash = i * 2000 + j;
					transitionsList.Add(trs);
				}
			}
			return transitionsList;
		}
	}
}

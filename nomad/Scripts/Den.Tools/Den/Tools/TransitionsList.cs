using System;
using Den.Tools.Matrices;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class TransitionsList : ICloneable
	{
		public Transition[] arr = new Transition[4];

		public int count;

		private int idCounter;

		public TransitionsList()
		{
		}

		public TransitionsList(int count)
		{
			arr = new Transition[count];
			this.count = count;
		}

		public TransitionsList(TransitionsList src)
		{
			arr = new Transition[src.arr.Length];
			Array.Copy(src.arr, arr, src.arr.Length);
			count = src.count;
		}

		public void Add(TransitionsList other)
		{
			for (int i = 0; i < other.count; i++)
			{
				Add(other.arr[i]);
			}
		}

		public void Add(float x, float z)
		{
			Transition trs = new Transition(x, z);
			Add(trs);
		}

		public void Add(Transition trs)
		{
			idCounter++;
			if (idCounter > 2147483646)
			{
				idCounter = 1;
			}
			trs.id = idCounter;
			if (trs.hash == 0)
			{
				trs.hash = trs.id;
			}
			if (arr.Length <= count)
			{
				SetCapacity(arr.Length * 2);
			}
			arr[count] = trs;
			count++;
		}

		private void SetCapacity(int newCapacity)
		{
			Transition[] destinationArray = new Transition[newCapacity];
			Array.Copy(arr, destinationArray, arr.Length);
			arr = destinationArray;
		}

		public Vector3 Min()
		{
			Vector3 result = new Vector3(3.4028235E+38f, 3.4028235E+38f, 3.4028235E+38f);
			for (int i = 0; i < count; i++)
			{
				if (arr[i].pos.x < result.x)
				{
					result.x = arr[i].pos.x;
				}
				if (arr[i].pos.y < result.y)
				{
					result.y = arr[i].pos.y;
				}
				if (arr[i].pos.z < result.z)
				{
					result.z = arr[i].pos.z;
				}
			}
			return result;
		}

		public Vector3 Max()
		{
			Vector3 result = new Vector3(-3.4028235E+38f, -3.4028235E+38f, -3.4028235E+38f);
			for (int i = 0; i < count; i++)
			{
				if (arr[i].pos.x > result.x)
				{
					result.x = arr[i].pos.x;
				}
				if (arr[i].pos.y > result.y)
				{
					result.y = arr[i].pos.y;
				}
				if (arr[i].pos.z > result.z)
				{
					result.z = arr[i].pos.z;
				}
			}
			return result;
		}

		public int CountInRect(Vector2D pos, Vector2D size)
		{
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				if (arr[i].pos.x > pos.x && arr[i].pos.x < pos.x + size.x && arr[i].pos.z > pos.z && arr[i].pos.z < pos.z + size.z)
				{
					num++;
				}
			}
			return num;
		}

		public object Clone()
		{
			return new TransitionsList
			{
				arr = (Transition[])arr.Clone(),
				count = count
			};
		}

		public static void Mask(TransitionsList src, TransitionsList dst, MatrixWorld mask, Noise random, bool invert)
		{
			for (int i = 0; i < src.count; i++)
			{
				Vector3 pos = src.arr[i].pos;
				if (!(pos.x <= mask.worldPos.x) || !(pos.x >= mask.worldPos.x + mask.worldSize.x) || !(pos.z <= mask.worldPos.z) || !(pos.z >= mask.worldPos.x + mask.worldSize.z))
				{
					float worldValue = mask.GetWorldValue(pos.x, pos.z);
					float num = random.Random(src.arr[i].hash);
					if (worldValue < num && invert)
					{
						dst.Add(src.arr[i]);
					}
					if (worldValue >= num && !invert)
					{
						dst.Add(src.arr[i]);
					}
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen.Collision
{
	public class SpatialHashGrid<T>
	{
		private readonly Dictionary<long, List<T>> cells;

		private readonly float cellSize;

		private readonly Func<T, Bounds> getBounds;

		private readonly AxisDirection upAxis;

		private readonly (int, int) primaryAxes;

		public SpatialHashGrid(float cellSize, Func<T, Bounds> getBounds, AxisDirection upDirection = AxisDirection.PosY)
		{
			cells = new Dictionary<long, List<T>>();
			this.cellSize = cellSize;
			this.getBounds = getBounds;
			upAxis = upDirection;
			primaryAxes = GetPrimaryAxes(upDirection);
		}

		private (int, int) GetPrimaryAxes(AxisDirection upDirection)
		{
			switch (upDirection)
			{
			case AxisDirection.PosY:
			case AxisDirection.NegY:
				return (0, 2);
			case AxisDirection.PosX:
			case AxisDirection.NegX:
				return (1, 2);
			case AxisDirection.PosZ:
			case AxisDirection.NegZ:
				return (0, 1);
			default:
				throw new ArgumentException("Invalid axis direction", "upDirection");
			}
		}

		private Vector2 GetGridPosition(Vector3 worldPos)
		{
			float x = worldPos[primaryAxes.Item1];
			float y = worldPos[primaryAxes.Item2];
			return new Vector2(x, y);
		}

		private long GetCellKey(int x, int y)
		{
			return ((long)x << 32) | (y & 0xFFFFFFFFu);
		}

		public void Insert(T obj)
		{
			Bounds bounds = getBounds(obj);
			Vector2 gridPosition = GetGridPosition(bounds.min);
			Vector2 gridPosition2 = GetGridPosition(bounds.max);
			int num = Mathf.FloorToInt(gridPosition.x / cellSize);
			int num2 = Mathf.FloorToInt(gridPosition.y / cellSize);
			int num3 = Mathf.FloorToInt(gridPosition2.x / cellSize);
			int num4 = Mathf.FloorToInt(gridPosition2.y / cellSize);
			for (int i = num2; i <= num4; i++)
			{
				for (int j = num; j <= num3; j++)
				{
					long cellKey = GetCellKey(j, i);
					if (!cells.TryGetValue(cellKey, out var value))
					{
						value = new List<T>();
						cells[cellKey] = value;
					}
					value.Add(obj);
				}
			}
		}

		public bool Remove(T obj)
		{
			bool result = false;
			Bounds bounds = getBounds(obj);
			Vector2 gridPosition = GetGridPosition(bounds.min);
			Vector2 gridPosition2 = GetGridPosition(bounds.max);
			int num = Mathf.FloorToInt(gridPosition.x / cellSize);
			int num2 = Mathf.FloorToInt(gridPosition.y / cellSize);
			int num3 = Mathf.FloorToInt(gridPosition2.x / cellSize);
			int num4 = Mathf.FloorToInt(gridPosition2.y / cellSize);
			for (int i = num2; i <= num4; i++)
			{
				for (int j = num; j <= num3; j++)
				{
					long cellKey = GetCellKey(j, i);
					if (cells.TryGetValue(cellKey, out var value) && value.Remove(obj))
					{
						result = true;
						if (value.Count == 0)
						{
							cells.Remove(cellKey);
						}
					}
				}
			}
			return result;
		}

		public void Query(Bounds queryBounds, ref List<T> results)
		{
			Vector3 min = queryBounds.min;
			Vector3 max = queryBounds.max;
			Vector2 gridPosition = GetGridPosition(min);
			Vector2 gridPosition2 = GetGridPosition(max);
			int num = Mathf.FloorToInt(gridPosition.x / cellSize);
			int num2 = Mathf.FloorToInt(gridPosition.y / cellSize);
			int num3 = Mathf.FloorToInt(gridPosition2.x / cellSize);
			int num4 = Mathf.FloorToInt(gridPosition2.y / cellSize);
			for (int i = num2; i <= num4; i++)
			{
				for (int j = num; j <= num3; j++)
				{
					long cellKey = GetCellKey(j, i);
					if (!cells.TryGetValue(cellKey, out var value))
					{
						continue;
					}
					foreach (T item in value)
					{
						Bounds bounds = getBounds(item);
						Vector3 min2 = bounds.min;
						Vector3 max2 = bounds.max;
						if (min2.x <= max.x && max2.x >= min.x && min2.y <= max.y && max2.y >= min.y && min2.z <= max.z && max2.z >= min.z && !results.Contains(item))
						{
							results.Add(item);
						}
					}
				}
			}
		}

		public void Clear()
		{
			cells.Clear();
		}

		public void DrawDebug(float duration = 0f)
		{
			HashSet<(int, int)> hashSet = new HashSet<(int, int)>();
			foreach (long key in cells.Keys)
			{
				int item = (int)(key >> 32);
				int item2 = (int)(key & 0xFFFFFFFFu);
				hashSet.Add((item, item2));
			}
			foreach (var item3 in hashSet)
			{
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				zero[primaryAxes.Item1] = (float)item3.Item1 * cellSize;
				zero[primaryAxes.Item2] = (float)item3.Item2 * cellSize;
				zero2[primaryAxes.Item1] = (float)(item3.Item1 + 1) * cellSize;
				zero2[primaryAxes.Item2] = (float)(item3.Item2 + 1) * cellSize;
				Vector3 vector = zero;
				Vector3 vector2 = zero;
				vector2[primaryAxes.Item1] = zero2[primaryAxes.Item1];
				Vector3 vector3 = zero2;
				Vector3 vector4 = zero2;
				vector4[primaryAxes.Item1] = zero[primaryAxes.Item1];
				Debug.DrawLine(vector, vector2, Color.white, duration);
				Debug.DrawLine(vector2, vector3, Color.white, duration);
				Debug.DrawLine(vector3, vector4, Color.white, duration);
				Debug.DrawLine(vector4, vector, Color.white, duration);
			}
			HashSet<T> hashSet2 = new HashSet<T>();
			foreach (List<T> value2 in cells.Values)
			{
				foreach (T item4 in value2)
				{
					if (hashSet2.Add(item4))
					{
						Bounds bounds = getBounds(item4);
						Vector3 min = bounds.min;
						Vector3 max = bounds.max;
						Vector3 zero3 = Vector3.zero;
						Vector3 zero4 = Vector3.zero;
						Vector3 zero5 = Vector3.zero;
						Vector3 zero6 = Vector3.zero;
						zero3[primaryAxes.Item1] = min[primaryAxes.Item1];
						zero3[primaryAxes.Item2] = min[primaryAxes.Item2];
						zero4[primaryAxes.Item1] = max[primaryAxes.Item1];
						zero4[primaryAxes.Item2] = min[primaryAxes.Item2];
						zero5[primaryAxes.Item1] = max[primaryAxes.Item1];
						zero5[primaryAxes.Item2] = max[primaryAxes.Item2];
						zero6[primaryAxes.Item1] = min[primaryAxes.Item1];
						zero6[primaryAxes.Item2] = max[primaryAxes.Item2];
						int index = (int)upAxis / 2;
						float value = (zero5[index] = (zero4[index] = (zero3[index] = min[index])));
						zero6[index] = value;
						Debug.DrawLine(zero3, zero4, Color.green, duration);
						Debug.DrawLine(zero4, zero5, Color.green, duration);
						Debug.DrawLine(zero5, zero6, Color.green, duration);
						Debug.DrawLine(zero6, zero3, Color.green, duration);
					}
				}
			}
		}
	}
}

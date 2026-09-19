using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class RegularGrid<T>
	{
		private Dictionary<Vector3Int, List<T>> gridMap = new Dictionary<Vector3Int, List<T>>();

		private float cellSize;

		private Func<T, Vector3> getPosition;

		public RegularGrid(float cellSize, Func<T, Vector3> getPosition)
		{
			this.cellSize = cellSize;
			if (getPosition != null)
			{
				this.getPosition = getPosition;
				return;
			}
			getPosition = (T x) => Vector3.zero;
		}

		public Vector3Int GetCellCoords(Vector3 pos)
		{
			return new Vector3Int(Mathf.FloorToInt(pos.x / cellSize), Mathf.FloorToInt(pos.y / cellSize), Mathf.FloorToInt(pos.z / cellSize));
		}

		public void AddElement(T elm)
		{
			Vector3Int cellCoords = GetCellCoords(getPosition(elm));
			if (gridMap.TryGetValue(cellCoords, out var value))
			{
				value.Add(elm);
				return;
			}
			gridMap[cellCoords] = new List<T> { elm };
		}

		public bool RemoveElement(T elm)
		{
			Vector3Int cellCoords = GetCellCoords(getPosition(elm));
			if (gridMap.TryGetValue(cellCoords, out var value))
			{
				return value.Remove(elm);
			}
			return false;
		}

		public IEnumerable<T> GetNeighborsEnumerator(T elm)
		{
			if (cellSize < 1E-07f)
			{
				yield break;
			}
			Vector3 position = getPosition(elm);
			Vector3Int coords = GetCellCoords(position);
			int x = -1;
			while (x <= 1)
			{
				int num;
				for (int y = -1; y <= 1; y = num)
				{
					for (int z = -1; z <= 1; z = num)
					{
						if (gridMap.TryGetValue(coords + new Vector3Int(x, y, z), out var value))
						{
							foreach (T item in value)
							{
								if (!item.Equals(elm) && Vector3.Distance(position, getPosition(item)) <= cellSize)
								{
									yield return item;
								}
							}
						}
						num = z + 1;
					}
					num = y + 1;
				}
				num = x + 1;
				x = num;
			}
		}

		public IEnumerable<T> GetNeighborsEnumerator(Vector3 position)
		{
			if (cellSize < 1E-07f)
			{
				yield break;
			}
			Vector3Int coords = GetCellCoords(position);
			int x = -1;
			while (x <= 1)
			{
				int num;
				for (int y = -1; y <= 1; y = num)
				{
					for (int z = -1; z <= 1; z = num)
					{
						if (gridMap.TryGetValue(coords + new Vector3Int(x, y, z), out var value))
						{
							foreach (T item in value)
							{
								if (Vector3.Distance(position, getPosition(item)) <= cellSize)
								{
									yield return item;
								}
							}
						}
						num = z + 1;
					}
					num = y + 1;
				}
				num = x + 1;
				x = num;
			}
		}
	}
}

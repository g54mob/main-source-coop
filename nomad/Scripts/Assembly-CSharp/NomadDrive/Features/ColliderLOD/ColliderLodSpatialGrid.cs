using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.ColliderLOD
{
	public sealed class ColliderLodSpatialGrid
	{
		private readonly float _cellSize;

		private readonly Dictionary<long, List<ColliderLodEntry>> _cells = new Dictionary<long, List<ColliderLodEntry>>();

		public float CellSize => _cellSize;

		public ColliderLodSpatialGrid(float cellSize)
		{
			_cellSize = Mathf.Max(1f, cellSize);
		}

		private long KeyFor(Vector3 position)
		{
			int cx = Mathf.FloorToInt(position.x / _cellSize);
			int cz = Mathf.FloorToInt(position.z / _cellSize);
			return PackKey(cx, cz);
		}

		private static long PackKey(int cx, int cz)
		{
			return ((long)cx << 32) ^ (uint)cz;
		}

		public void Insert(ColliderLodEntry entry)
		{
			long key = (entry.GridCellKey = KeyFor(entry.WorldCenter));
			if (!_cells.TryGetValue(key, out var value))
			{
				value = new List<ColliderLodEntry>();
				_cells[key] = value;
			}
			value.Add(entry);
		}

		public void Remove(ColliderLodEntry entry)
		{
			if (_cells.TryGetValue(entry.GridCellKey, out var value))
			{
				value.Remove(entry);
				if (value.Count == 0)
				{
					_cells.Remove(entry.GridCellKey);
				}
			}
		}

		public void Rebucket(ColliderLodEntry entry)
		{
			if (KeyFor(entry.WorldCenter) != entry.GridCellKey)
			{
				Remove(entry);
				Insert(entry);
			}
		}

		public void Clear()
		{
			_cells.Clear();
		}

		public void QueryRing(Vector3 center, float radius, HashSet<ColliderLodEntry> results)
		{
			int num = Mathf.CeilToInt(radius / _cellSize);
			int num2 = Mathf.FloorToInt(center.x / _cellSize);
			int num3 = Mathf.FloorToInt(center.z / _cellSize);
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					long key = PackKey(num2 + i, num3 + j);
					if (_cells.TryGetValue(key, out var value))
					{
						for (int k = 0; k < value.Count; k++)
						{
							results.Add(value[k]);
						}
					}
				}
			}
		}
	}
}

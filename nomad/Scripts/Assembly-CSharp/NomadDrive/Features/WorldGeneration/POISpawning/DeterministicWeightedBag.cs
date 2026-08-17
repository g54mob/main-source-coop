using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public sealed class DeterministicWeightedBag
	{
		private readonly PoiWeightEntry[] _pool;

		private readonly float[] _weights;

		private readonly int[] _lastDrawnOrdinal;

		private int _nextOrdinalBase;

		public int PoolCount => _pool.Length;

		public DeterministicWeightedBag(IEnumerable<PoiWeightEntry> entries)
		{
			List<PoiWeightEntry> list = new List<PoiWeightEntry>();
			if (entries != null)
			{
				foreach (PoiWeightEntry entry in entries)
				{
					if (entry != null && entry.HasValidReference)
					{
						list.Add(entry);
					}
				}
			}
			_pool = list.ToArray();
			_weights = new float[_pool.Length];
			_lastDrawnOrdinal = new int[_pool.Length];
			for (int i = 0; i < _pool.Length; i++)
			{
				_weights[i] = Mathf.Max(0.0001f, _pool[i].weight);
				_lastDrawnOrdinal[i] = -1;
			}
		}

		public List<PoiWeightEntry> DrawCycle(int cycleSeed)
		{
			List<PoiWeightEntry> list = new List<PoiWeightEntry>(_pool.Length);
			if (_pool.Length == 0)
			{
				return list;
			}
			List<int> list2 = new List<int>(_pool.Length);
			for (int i = 0; i < _pool.Length; i++)
			{
				list2.Add(i);
			}
			list2.Sort(delegate(int a, int b)
			{
				int num8 = _lastDrawnOrdinal[a].CompareTo(_lastDrawnOrdinal[b]);
				return (num8 == 0) ? a.CompareTo(b) : num8;
			});
			System.Random random = new System.Random(cycleSeed);
			int num = 0;
			while (list2.Count > 0)
			{
				float num2 = 0f;
				for (int num3 = 0; num3 < list2.Count; num3++)
				{
					num2 += _weights[list2[num3]];
				}
				double num4 = random.NextDouble() * (double)num2;
				float num5 = 0f;
				int index = list2.Count - 1;
				for (int num6 = 0; num6 < list2.Count; num6++)
				{
					num5 += _weights[list2[num6]];
					if (num4 < (double)num5)
					{
						index = num6;
						break;
					}
				}
				int num7 = list2[index];
				list2.RemoveAt(index);
				list.Add(_pool[num7]);
				_lastDrawnOrdinal[num7] = _nextOrdinalBase + num;
				num++;
			}
			_nextOrdinalBase += _pool.Length;
			return list;
		}
	}
}

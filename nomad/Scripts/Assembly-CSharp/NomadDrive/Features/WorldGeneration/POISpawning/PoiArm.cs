using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public sealed class PoiArm
	{
		private readonly int _masterSeed;

		private readonly int _dirSalt;

		private readonly int _sign;

		private readonly PoiSpawnTuning _tuning;

		private readonly DeterministicWeightedBag _minorBag;

		private readonly DeterministicWeightedBag _majorBag;

		private readonly List<PoiPlacement> _placements = new List<PoiPlacement>();

		private readonly Dictionary<int, PoiPlacement> _byChunkZ = new Dictionary<int, PoiPlacement>();

		private readonly Queue<PoiWeightEntry> _minorBuffer = new Queue<PoiWeightEntry>();

		private readonly Queue<PoiWeightEntry> _majorBuffer = new Queue<PoiWeightEntry>();

		private int _cursorZ;

		private int _nextOrdinal;

		private int _minorsEmittedThisCycle;

		private int _targetMinorsThisCycle;

		private int _structureCycleIndex;

		private int _minorDeckIndex;

		private int _majorDeckIndex;

		private bool _exhaustedBothPools;

		public PoiArm(int masterSeed, int dirSalt, int sign, PoiSpawnTuning tuning, DeterministicWeightedBag minorBag, DeterministicWeightedBag majorBag)
		{
			_masterSeed = masterSeed;
			_dirSalt = dirSalt;
			_sign = ((sign >= 0) ? 1 : (-1));
			_tuning = tuning;
			_minorBag = minorBag;
			_majorBag = majorBag;
			System.Random random = new System.Random(Hash(_masterSeed, _dirSalt, "firstOffset", 0));
			int num = Mathf.Max(1, _tuning.MinFirstOffsetChunks);
			int num2 = Mathf.Max(num, _tuning.MaxFirstOffsetChunks);
			_cursorZ = random.Next(num, num2 + 1);
			BeginNewCycle();
		}

		public void EnsureBuiltThrough(int absZ)
		{
			if (_exhaustedBothPools)
			{
				return;
			}
			int num = 0;
			while (_cursorZ <= absZ)
			{
				EmitNext();
				if (_exhaustedBothPools || ++num > 200000)
				{
					break;
				}
			}
		}

		public bool TryGetPlacement(int signedChunkZ, out PoiPlacement placement)
		{
			return _byChunkZ.TryGetValue(signedChunkZ, out placement);
		}

		private void EmitNext()
		{
			bool flag = _minorBag.PoolCount > 0;
			bool flag2 = _majorBag.PoolCount > 0;
			if (!flag && !flag2)
			{
				_exhaustedBothPools = true;
				return;
			}
			int nextOrdinal = _nextOrdinal;
			PoiWeightEntry entry;
			POICategory category;
			bool flag3;
			if (flag && (_minorsEmittedThisCycle < _targetMinorsThisCycle || !flag2))
			{
				entry = NextFromBag(_minorBag, _minorBuffer, "minorDeck", ref _minorDeckIndex);
				category = POICategory.Minor;
				_minorsEmittedThisCycle++;
				flag3 = false;
			}
			else
			{
				entry = NextFromBag(_majorBag, _majorBuffer, "majorDeck", ref _majorDeckIndex);
				category = POICategory.Major;
				flag3 = true;
				BeginNewCycle();
			}
			int num = _sign * _cursorZ;
			PoiPlacement poiPlacement = new PoiPlacement(nextOrdinal, num, category, entry);
			_placements.Add(poiPlacement);
			_byChunkZ[num] = poiPlacement;
			_nextOrdinal++;
			int num2 = NextGap(nextOrdinal);
			if (flag3)
			{
				num2 += Mathf.Max(0, _tuning.ForcedEmptyChunksAfterMajor);
			}
			_cursorZ += Mathf.Max(1, num2);
		}

		private void BeginNewCycle()
		{
			System.Random random = new System.Random(Hash(_masterSeed, _dirSalt, "structure", _structureCycleIndex++));
			int num = Mathf.Max(0, _tuning.MinMinorsBetweenMajors);
			int num2 = Mathf.Max(num, _tuning.MaxMinorsBetweenMajors);
			_targetMinorsThisCycle = random.Next(num, num2 + 1);
			_minorsEmittedThisCycle = 0;
		}

		private PoiWeightEntry NextFromBag(DeterministicWeightedBag bag, Queue<PoiWeightEntry> buffer, string channel, ref int deckIndex)
		{
			if (buffer.Count == 0)
			{
				int cycleSeed = Hash(_masterSeed, _dirSalt, channel, deckIndex++);
				foreach (PoiWeightEntry item in bag.DrawCycle(cycleSeed))
				{
					buffer.Enqueue(item);
				}
			}
			return buffer.Dequeue();
		}

		private int NextGap(int ordinal)
		{
			System.Random random = new System.Random(Hash(_masterSeed, _dirSalt, "gap", ordinal));
			int num = Mathf.Max(1, _tuning.MinChunksBetweenPois);
			int num2 = Mathf.Max(num, _tuning.MaxChunksBetweenPois);
			return random.Next(num, num2 + 1);
		}

		internal static int Hash(int masterSeed, int dirSalt, string channel, int n)
		{
			return (((((masterSeed * 397) ^ dirSalt) * 397) ^ channel.GetHashCode()) * 397) ^ n;
		}
	}
}

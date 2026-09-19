using System.Collections.Generic;

namespace Fusion
{
	internal class DualChannelPriorityQueue<T>
	{
		private readonly List<ulong> _keys = new List<ulong>(256);

		private readonly List<T> _values = new List<T>(256);

		private const ulong UnorderedFlag = 1uL;

		private const ulong PendingRemoval = 0uL;

		public int Count => _keys.Count;

		public (ulong sequence, T value) this[int i] => (sequence: ((_keys[i] & 1) != 0L) ? 0 : (_keys[i] / 2), value: _values[i]);

		public bool TryInsertKey(ulong key, out int position)
		{
			if (key == 0)
			{
				position = _values.Count;
				long num;
				if (_keys.Count <= 0)
				{
					num = 2L;
				}
				else
				{
					List<ulong> keys = _keys;
					num = (long)keys[keys.Count - 1];
				}
				ulong num2 = (ulong)num;
				num2 |= 1;
				_keys.Add(num2);
				_values.Add(default(T));
				return true;
			}
			ulong num3 = key * 2;
			if (num3 <= key)
			{
				position = 0;
				return false;
			}
			int num4 = 0;
			int num5 = _keys.Count - 1;
			while (num4 <= num5)
			{
				int num6 = num4 + (num5 - num4) / 2;
				if (_keys[num6] == num3)
				{
					position = 0;
					return false;
				}
				if (_keys[num6] < num3)
				{
					num4 = num6 + 1;
				}
				else
				{
					num5 = num6 - 1;
				}
			}
			_keys.Insert(num4, num3);
			_values.Insert(num4, default(T));
			position = num4;
			return true;
		}

		public bool TryInsert(ulong key, T value)
		{
			if (!TryInsertKey(key, out var position))
			{
				return false;
			}
			_values[position] = value;
			return true;
		}

		public void InsertValue(int position, T value)
		{
			_values[position] = value;
		}

		public void MarkForCleanup(int position)
		{
			_keys[position] = 0uL;
			_values[position] = default(T);
		}

		public void CleanUp(int start, int end)
		{
			if (start >= end || start >= _keys.Count || end > _keys.Count)
			{
				return;
			}
			for (int num = end - 1; num >= start; num--)
			{
				if (_keys[num] == 0)
				{
					_keys.RemoveAt(num);
					_values.RemoveAt(num);
				}
			}
		}

		public void Clear()
		{
			_keys.Clear();
			_values.Clear();
		}
	}
}

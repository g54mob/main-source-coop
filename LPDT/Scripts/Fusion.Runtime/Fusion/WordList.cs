#define DEBUG
using System;
using System.Runtime.CompilerServices;

namespace Fusion
{
	internal struct WordList
	{
		private (int Offset, int Value)[] _words;

		private int _count;

		public readonly int Count => _count;

		public readonly Span<(int offset, int value)> AsSpan
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Span<(int, int)>(_words, 0, _count);
			}
		}

		public WordList(ReadOnlySpan<(int, int)> source)
		{
			_words = null;
			_count = 0;
			if (source.Length != 0)
			{
				int num;
				for (num = 1024; num < source.Length; num *= 2)
				{
				}
				_words = new(int, int)[num];
				source.CopyTo(_words);
				_count = source.Length;
			}
		}

		public void Add(int offset, int value)
		{
			if (_words == null)
			{
				_words = new(int, int)[1024];
			}
			if (_count >= _words.Length)
			{
				Array.Resize(ref _words, _words.Length * 2);
			}
			if (_count > 0 && _words[_count - 1].Offset >= offset)
			{
				Assert.Fail($"{_words[_count - 1].Offset} < {offset}");
			}
			_words[_count++] = (Offset: offset, Value: value);
		}

		public void Clear()
		{
			_count = 0;
		}

		public bool RemoveAll(OffsetSet offsetSet)
		{
			return RemoveAll(offsetSet.AsArray);
		}

		public bool RemoveAll(int[] sortedOffsets)
		{
			if (sortedOffsets.Length == 0)
			{
				return false;
			}
			int i = 0;
			for (int j = 0; j < sortedOffsets.Length; j++)
			{
				int num = sortedOffsets[j];
				Assert.Check(j == 0 || num >= sortedOffsets[j - 1], "j == 0 || offset >= sortedOffsets[j - 1]");
				for (; i < _count; i++)
				{
					if (_words[i].Offset < num)
					{
						continue;
					}
					if (_words[i].Offset > num)
					{
						break;
					}
					int num2 = i++;
					while (true)
					{
						if (i < _count && _words[i].Offset == num)
						{
							i++;
							continue;
						}
						if (++j == sortedOffsets.Length)
						{
							break;
						}
						num = sortedOffsets[j];
						Assert.Check(j == 0 || num >= sortedOffsets[j - 1], "j == 0 || offset >= sortedOffsets[j - 1]");
						int num3 = i;
						for (; i < _count && _words[i].Offset < num; i++)
						{
						}
						int num4 = i - num3;
						if (num4 > 0)
						{
							Array.Copy(_words, num3, _words, num2, num4);
							num2 += num4;
						}
					}
					int num5 = _count - i;
					if (num5 > 0)
					{
						Array.Copy(_words, i, _words, num2, num5);
						num2 += num5;
					}
					_count = num2;
					return true;
				}
			}
			return false;
		}
	}
}

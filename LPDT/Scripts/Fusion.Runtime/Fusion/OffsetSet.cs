using System;
using System.Runtime.CompilerServices;

namespace Fusion
{
	internal class OffsetSet
	{
		public bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _003CsortedOffsets_003EP.Length == 0;
			}
		}

		public ReadOnlySpan<int> Values => _003CsortedOffsets_003EP;

		public int[] AsArray => _003CsortedOffsets_003EP;

		public OffsetSet(int[] sortedOffsets)
		{
			_003CsortedOffsets_003EP = sortedOffsets;
			base._002Ector();
		}

		public bool TryFind(int offset, ref int index)
		{
			while (index < _003CsortedOffsets_003EP.Length && _003CsortedOffsets_003EP[index] < offset)
			{
				index++;
			}
			return index < _003CsortedOffsets_003EP.Length && _003CsortedOffsets_003EP[index] == offset;
		}
	}
}

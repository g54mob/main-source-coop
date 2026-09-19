using System;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstCellSpan : IEquatable<BurstCellSpan>
	{
		public int4 min;

		public int4 max;

		public int level => min.w;

		public BurstCellSpan(CellSpan span)
		{
			min = new int4(span.min.x, span.min.y, span.min.z, span.min.w);
			max = new int4(span.max.x, span.max.y, span.max.z, span.max.w);
		}

		public BurstCellSpan(int4 min, int4 max)
		{
			this.min = min;
			this.max = max;
		}

		public bool Equals(BurstCellSpan other)
		{
			if (min.Equals(other.min))
			{
				return max.Equals(other.max);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			return Equals((BurstCellSpan)obj);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(BurstCellSpan a, BurstCellSpan b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(BurstCellSpan a, BurstCellSpan b)
		{
			return !a.Equals(b);
		}
	}
}

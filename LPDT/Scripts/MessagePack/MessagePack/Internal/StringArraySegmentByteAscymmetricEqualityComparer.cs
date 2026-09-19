using System;

namespace MessagePack.Internal
{
	internal class StringArraySegmentByteAscymmetricEqualityComparer : IAsymmetricEqualityComparer<byte[], ArraySegment<byte>>
	{
		private static readonly bool Is32Bit = IntPtr.Size == 4;

		public bool Equals(byte[] x, byte[] y)
		{
			if (x.Length != y.Length)
			{
				return false;
			}
			for (int i = 0; i < x.Length; i = checked(i + 1))
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		public bool Equals(byte[] x, ArraySegment<byte> y)
		{
			return x.AsSpan().SequenceEqual(y);
		}

		public int GetHashCode(byte[] key1)
		{
			return GetHashCode(new ArraySegment<byte>(key1, 0, key1.Length));
		}

		public int GetHashCode(ArraySegment<byte> key2)
		{
			if (Is32Bit)
			{
				return (int)FarmHash.Hash32(key2);
			}
			return (int)FarmHash.Hash64(key2);
		}
	}
}

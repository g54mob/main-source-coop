using System.Runtime.CompilerServices;

namespace Mirror.BouncyCastle.Math.Raw
{
	internal static class Bits
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint BitPermuteStep(uint x, uint m, int s)
		{
			uint num = (x ^ (x >> s)) & m;
			return num ^ (num << s) ^ x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong BitPermuteStep(ulong x, ulong m, int s)
		{
			ulong num = (x ^ (x >> s)) & m;
			return num ^ (num << s) ^ x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong BitPermuteStepSimple(ulong x, ulong m, int s)
		{
			return ((x & m) << s) | ((x >> s) & m);
		}
	}
}

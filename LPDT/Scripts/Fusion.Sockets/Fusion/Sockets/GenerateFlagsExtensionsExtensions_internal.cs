using System.Runtime.CompilerServices;

namespace Fusion.Sockets
{
	internal static class GenerateFlagsExtensionsExtensions_internal
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetSocketFlags flag, NetSocketFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetSocketFlags flag, NetSocketFlags value, NetSocketFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetSocketFlags flag, NetSocketFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetSocketFlags flag, NetSocketFlags value, NetSocketFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetSocketFlags flag, NetSocketFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetSocketFlags flag, NetSocketFlags value)
		{
			return (flag & value) == 0;
		}
	}
}

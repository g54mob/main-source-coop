using System.Runtime.CompilerServices;

namespace Fusion.Protocol
{
	internal static class GenerateFlagsExtensionsExtensions_internal
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this JoinRequests flag, JoinRequests value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this JoinRequests flag, JoinRequests value, JoinRequests mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this JoinRequests flag, JoinRequests value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this JoinRequests flag, JoinRequests value, JoinRequests mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this JoinRequests flag, JoinRequests value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this JoinRequests flag, JoinRequests value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this StartRequests flag, StartRequests value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this StartRequests flag, StartRequests value, StartRequests mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this StartRequests flag, StartRequests value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this StartRequests flag, StartRequests value, StartRequests mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this StartRequests flag, StartRequests value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this StartRequests flag, StartRequests value)
		{
			return (flag & value) == 0;
		}
	}
}

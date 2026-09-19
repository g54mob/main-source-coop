using System.Runtime.CompilerServices;

namespace Fusion.Sockets.V2
{
	internal static class GenerateFlagsExtensionsExtensions_internal
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this ChannelFlags flag, ChannelFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this ChannelFlags flag, ChannelFlags value, ChannelFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this ChannelFlags flag, ChannelFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this ChannelFlags flag, ChannelFlags value, ChannelFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this ChannelFlags flag, ChannelFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this ChannelFlags flag, ChannelFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this PacketTypeV2 flag, PacketTypeV2 value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this PacketTypeV2 flag, PacketTypeV2 value, PacketTypeV2 mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this PacketTypeV2 flag, PacketTypeV2 value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this PacketTypeV2 flag, PacketTypeV2 value, PacketTypeV2 mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this PacketTypeV2 flag, PacketTypeV2 value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this PacketTypeV2 flag, PacketTypeV2 value)
		{
			return (flag & value) == 0;
		}
	}
}

using System.Runtime.CompilerServices;

namespace Fusion
{
	public static class NetworkObjectFlagsExtensions
	{
		public const NetworkObjectFlags CurrentVersion = NetworkObjectFlags.V2;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NetworkObjectFlags GetVersion(this NetworkObjectFlags flags)
		{
			return flags & NetworkObjectFlags.MaskVersion;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsVersionCurrent(this NetworkObjectFlags flags)
		{
			return NetworkObjectFlags.V2 == flags.GetVersion();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NetworkObjectFlags SetCurrentVersion(this NetworkObjectFlags flags)
		{
			return SetWithMask(flags, NetworkObjectFlags.V2, NetworkObjectFlags.MaskVersion);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIgnored(this NetworkObjectFlags flags)
		{
			return flags.Has(NetworkObjectFlags.Ignore);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NetworkObjectFlags SetIgnored(this NetworkObjectFlags flags, bool value)
		{
			if (value)
			{
				return flags | NetworkObjectFlags.Ignore;
			}
			return flags & ~NetworkObjectFlags.Ignore;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static NetworkObjectFlags SetWithMask(NetworkObjectFlags flags, NetworkObjectFlags value, NetworkObjectFlags mask)
		{
			flags &= ~mask;
			flags |= value;
			return flags;
		}

		public static NetworkObjectInterestModes GetInterestMode(this NetworkObjectFlags flags)
		{
			if (flags.Has(NetworkObjectFlags.EnableAreaOfInterest))
			{
				return NetworkObjectInterestModes.AreaOfInterest;
			}
			if (flags.Has(NetworkObjectFlags.EnableExplicitObjectInterest))
			{
				return NetworkObjectInterestModes.Explicit;
			}
			return NetworkObjectInterestModes.Global;
		}

		public static NetworkObjectFlags SetInterestMode(this NetworkObjectFlags flags, NetworkObjectInterestModes value)
		{
			flags &= ~NetworkObjectFlags.EnableAreaOfInterest;
			flags &= ~NetworkObjectFlags.EnableExplicitObjectInterest;
			switch (value)
			{
			case NetworkObjectInterestModes.AreaOfInterest:
				flags |= NetworkObjectFlags.EnableAreaOfInterest;
				break;
			case NetworkObjectInterestModes.Explicit:
				flags |= NetworkObjectFlags.EnableExplicitObjectInterest;
				break;
			}
			return flags;
		}
	}
}

using System;

namespace PlayEveryWare.EpicOnlineServices
{
	public static class WrappedPlatformFlagsExtensions
	{
		public static bool IsSupported(this WrappedPlatformFlags platformFlags, PlatformManager.Platform platform)
		{
			switch (platformFlags)
			{
			case WrappedPlatformFlags.None:
			case WrappedPlatformFlags.LoadingInEditor:
			case WrappedPlatformFlags.DisableOverlay:
			case WrappedPlatformFlags.DisableSocialOverlay:
			case WrappedPlatformFlags.Reserved1:
				return true;
			case WrappedPlatformFlags.WindowsEnableOverlayD3D9:
			case WrappedPlatformFlags.WindowsEnableOverlayD3D10:
			case WrappedPlatformFlags.WindowsEnableOverlayOpengl:
				if (platform == PlatformManager.Platform.Windows)
				{
					return true;
				}
				break;
			case WrappedPlatformFlags.ConsoleEnableOverlayAutomaticUnloading:
				if (platform == PlatformManager.Platform.Console)
				{
					return true;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("platformFlags", platformFlags, null);
			}
			return false;
		}
	}
}

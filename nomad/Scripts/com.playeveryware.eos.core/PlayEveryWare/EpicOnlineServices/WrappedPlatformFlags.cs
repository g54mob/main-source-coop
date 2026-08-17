using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[Flags]
	public enum WrappedPlatformFlags
	{
		None = 0,
		LoadingInEditor = 1,
		DisableOverlay = 2,
		DisableSocialOverlay = 4,
		Reserved1 = 8,
		WindowsEnableOverlayD3D9 = 0x10,
		WindowsEnableOverlayD3D10 = 0x20,
		WindowsEnableOverlayOpengl = 0x40,
		ConsoleEnableOverlayAutomaticUnloading = 0x80
	}
}

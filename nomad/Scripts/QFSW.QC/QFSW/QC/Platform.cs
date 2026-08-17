using System;

namespace QFSW.QC
{
	[Flags]
	public enum Platform : long
	{
		OSXEditor = 1L,
		OSXPlayer = 2L,
		WindowsPlayer = 4L,
		OSXWebPlayer = 8L,
		OSXDashboardPlayer = 0x10L,
		WindowsWebPlayer = 0x20L,
		WindowsEditor = 0x80L,
		IPhonePlayer = 0x100L,
		PS3 = 0x200L,
		XBOX360 = 0x400L,
		Android = 0x800L,
		NaCl = 0x1000L,
		LinuxPlayer = 0x2000L,
		FlashPlayer = 0x8000L,
		LinuxEditor = 0x10000L,
		WebGLPlayer = 0x20000L,
		MetroPlayerX86 = 0x40000L,
		WSAPlayerX86 = 0x40000L,
		MetroPlayerX64 = 0x80000L,
		WSAPlayerX64 = 0x80000L,
		MetroPlayerARM = 0x100000L,
		WSAPlayerARM = 0x100000L,
		WP8Player = 0x200000L,
		BlackBerryPlayer = 0x400000L,
		TizenPlayer = 0x800000L,
		PSP2 = 0x1000000L,
		PS4 = 0x2000000L,
		PSM = 0x4000000L,
		XboxOne = 0x8000000L,
		SamsungTVPlayer = 0x10000000L,
		WiiU = 0x40000000L,
		tvOS = 0x80000000L,
		Switch = 0x100000000L,
		Lumin = 0x200000000L,
		Stadia = 0x400000000L,
		None = 0L,
		AllPlatforms = -1L,
		EditorPlatforms = 0x10081L,
		BuildPlatforms = -65666L,
		MobilePlatforms = 0x200900L
	}
}

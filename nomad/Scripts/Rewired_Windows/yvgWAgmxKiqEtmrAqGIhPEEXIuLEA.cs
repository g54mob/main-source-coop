using System;
using System.Runtime.InteropServices;

internal static class yvgWAgmxKiqEtmrAqGIhPEEXIuLEA
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void ItqXeWuZjMDQOQTYoKkLPRPGOUPS(JbFAywBYXfGhagDypdlSSUPUqzcGA pGamepad);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void bKqBUolUkLBbzqLvmXdzAQMoblbD(JbFAywBYXfGhagDypdlSSUPUqzcGA pGamepad);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "IUnknown_Release")]
	private static extern ulong iXVzbBVkGxoVcjBBivqOqEyMKxGJ(IntPtr P_0);

	public static ulong mfgBjUHYfRxnwgUAlqsDkdOKsnCxA(IntPtr P_0)
	{
		if (P_0 == IntPtr.Zero)
		{
			return 0uL;
		}
		return iXVzbBVkGxoVcjBBivqOqEyMKxGJ(P_0);
	}

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "IUnknown_AddRef")]
	private static extern ulong LGbIDjGhYaprbhvasHKvPijbPANV(IntPtr P_0);

	public static ulong OsXDncqJgzKRDbYldLkJiOAUwUkT(IntPtr P_0)
	{
		if (P_0 == IntPtr.Zero)
		{
			return 0uL;
		}
		return LGbIDjGhYaprbhvasHKvPijbPANV(P_0);
	}

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Core_IsAPISupported")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool bIzEGGQGRfdgxHvZhtnVEjYEjUai();

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Core_GetMinimumRequiredWindowsVersionString")]
	private static extern IntPtr lEdgHIpYZVsEZOPgPIgWdIaRthGw();

	public static string FCMJrYAfhPrQiVXaBlwrGCPyAMrjA()
	{
		IntPtr intPtr = lEdgHIpYZVsEZOPgPIgWdIaRthGw();
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return Marshal.PtrToStringUni(intPtr);
	}

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_GetGamepads")]
	public static extern JbFAywBYXfGhagDypdlSSUPUqzcGA BqCDFMDwnJasbOcTuWmiswEOASVV();

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_GetGamepadCount")]
	public static extern uint YIbdzEFxkRDnJJniRdABrcdHzGqU(IntPtr P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_GetGamepad")]
	public static extern JbFAywBYXfGhagDypdlSSUPUqzcGA ZmAfyQAlJrEoBOdwXhSTmsfoPYljA(IntPtr P_0, uint P_1);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_GetCurrentReading")]
	public static extern bool oHyKdoNxKmnYzMprjWhkUXptknQT(IntPtr P_0, ref woIXEaxkDSRQKTJnvfWBciuaAYVB P_1);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_SetVibration")]
	public static extern void FWsFSpeJocOcbUhyFEIffpvJXDOaA(IntPtr P_0, [In][MarshalAs(UnmanagedType.Struct)] nqXEdeCznmdQnPGHYFzpCcMKdXyjb P_1);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_ListenForEvents")]
	public static extern void idmLuMYDAZpjCtJLWKkDFNchNEbY();

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_StopListeningForEvents")]
	public static extern void XKtYMrdEHwSgNyhWWjcUZnfsomdq();

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_SetEventListener_GamepadAdded")]
	public static extern void DCqncaUBsgHVawCCKoMJLEWsgKLI(ItqXeWuZjMDQOQTYoKkLPRPGOUPS P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "Gamepad_SetEventListener_GamepadRemoved")]
	public static extern void wTaVwgqmLUJMIpGjllvCbTspeDGg(bKqBUolUkLBbzqLvmXdzAQMoblbD P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetIsWireless")]
	public static extern bool EqwNsuHEOhqjofrxCXBjGDwfLSXA(IntPtr P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetButtonCount")]
	public static extern int SXqfmYRuiTUNaTkQogGbvhtFcccHA(IntPtr P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetSwitchCount")]
	public static extern int yPYowNwNGNCDSJVcQcyWdYolwICB(IntPtr P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetAxisCount")]
	public static extern int SACOAqkYRkVMWiGPrIyqfOxuLewy(IntPtr P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetDisplayName")]
	private static extern IntPtr VXeWeeBQfCPWHrmbSlLPZFdzTRtu(IntPtr P_0);

	public static string JHhvwqOreiHyGFqqvyBTAgwvWULE(IntPtr P_0)
	{
		IntPtr intPtr = VXeWeeBQfCPWHrmbSlLPZFdzTRtu(P_0);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return Marshal.PtrToStringUni(intPtr);
	}

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetHardwareVendorId")]
	public static extern ushort HbBSVmYKmPWMvwpnQSHrrXjfrxqM(IntPtr P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetHardwareProductId")]
	public static extern ushort dPhLSuOpQFfdibuVLbKebqlUarmfb(IntPtr P_0);

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_GetNonRoamableId")]
	public static extern IntPtr oVhSadYAyWCXhZuhHWAAguLXlTZX(IntPtr P_0);

	public static string qKyRCiBZCOeWcncTYAcVRmnIDfwW(IntPtr P_0)
	{
		IntPtr intPtr = oVhSadYAyWCXhZuhHWAAguLXlTZX(P_0);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return Marshal.PtrToStringUni(intPtr);
	}

	[DllImport("Rewired_WindowsGamingInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "RawGameController_FromGameController")]
	public static extern JbFAywBYXfGhagDypdlSSUPUqzcGA rFuyftGHITbIRezAXOepvHuyTMvcA(IntPtr P_0);
}

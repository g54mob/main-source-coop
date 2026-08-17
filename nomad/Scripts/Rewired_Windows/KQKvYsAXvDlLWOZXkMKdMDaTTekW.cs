using System;
using System.Runtime.InteropServices;
using Rewired.Utils;

internal static class KQKvYsAXvDlLWOZXkMKdMDaTTekW
{
	private static IntPtr fYVxUhGKKXXntMOkVVqtQJOwVWJl = IntPtr.Zero;

	private static int wyvjImUtwnfaMnugOvzWMiIlneGo;

	[DllImport("Kernel32.dll", EntryPoint = "GetCurrentProcess")]
	public static extern IntPtr YpVdpdbqWCnFeBxgMBHHGCRqDpZKA();

	[DllImport("Kernel32.dll", EntryPoint = "IsWow64Process")]
	public static extern bool csyAnBAjkwsbHOpIwOKVUILVjSLyA(IntPtr P_0, out bool P_1);

	[DllImport("kernel32.dll", EntryPoint = "GetOverlappedResult", SetLastError = true)]
	internal static extern bool fkKdEakPxFEwCFvyqBEOjpiEfmHDA(IntPtr P_0, IntPtr P_1, out uint P_2, bool P_3);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetActiveWindow")]
	private static extern IntPtr YGisaiGeNqqRwntxVYjYtjNyNnJF();

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetForegroundWindow")]
	public static extern IntPtr AibyfVejWlavgkstvckwcMQlHaQgb();

	public static IntPtr vxQmmxwilIJheBcwiXxHghIgwKFs(IntPtr P_0, int P_1)
	{
		if (IntPtr.Size == 4)
		{
			return uYHovmjlzoXGaOLWLvugWlyzbyCC(P_0, P_1);
		}
		return uUTSAKUsCovotqVVIdjNbbXeMhRR(P_0, P_1);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongW")]
	private static extern IntPtr uYHovmjlzoXGaOLWLvugWlyzbyCC(IntPtr P_0, int P_1);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtrW")]
	private static extern IntPtr uUTSAKUsCovotqVVIdjNbbXeMhRR(IntPtr P_0, int P_1);

	public static IntPtr ycRDdmfaaIQpstrpIFFQSOTVxbGv(IntPtr P_0, int P_1, IntPtr P_2)
	{
		if (IntPtr.Size == 4)
		{
			return zrlGZOWUOLHjwwpHPdEPwxeVCWNS(P_0, P_1, P_2);
		}
		return CeaJDFvqlOAFpcDQHMuXwCeeWsjBA(P_0, P_1, P_2);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongPtrW")]
	private static extern IntPtr CeaJDFvqlOAFpcDQHMuXwCeeWsjBA(IntPtr P_0, int P_1, IntPtr P_2);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongW")]
	private static extern IntPtr zrlGZOWUOLHjwwpHPdEPwxeVCWNS(IntPtr P_0, int P_1, IntPtr P_2);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputDeviceList")]
	public static extern uint OdRkZKbazOYSHqMXinyOIZyCcEmw(IntPtr P_0, ref uint P_1, uint P_2);

	[DllImport("User32.dll", EntryPoint = "GetRegisteredRawInputDevices")]
	public static extern uint VceligKXAYdHDKRFXbrrICrEnpPsA(IntPtr P_0, ref uint P_1, uint P_2);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputBuffer")]
	public static extern int CgBJZgixLzNdNTzerIFjTEbPozLRA(IntPtr P_0, ref uint P_1, uint P_2);

	[DllImport("User32.dll", EntryPoint = "SystemParametersInfo")]
	public static extern bool NIauLqJqhqWSuJcndqOblvtsshCS(uint P_0, uint P_1, ref int P_2, uint P_3);

	[DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
	public static extern int ajBOvwTePqLBYFJfsEPHxqyplzfk(int P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCursorPos")]
	public static extern bool hHjhOsyLyNAQiawMuPOolvbsLfcf(out yvmnbQjDLoRvQsjOUsVFFNnaiTOB P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "OpenInputDesktop")]
	public static extern IntPtr KOEHysroCvnEYNZMRadPGuReGWG(uint P_0, bool P_1, uint P_2);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetKeyState")]
	public static extern short korvYgNVnDxxHoRUpQvOzdNVcbWi(int P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetAsyncKeyState")]
	public static extern short nTpEmdABblNQonGUcrIYpZSEzkVD(int P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetKeyboardState")]
	public static extern bool NcibMJizVdeFYlChVgkCUcZBGdbOA(IntPtr P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "ClientToScreen")]
	public static extern bool ndStfWCYGrhpSTQJPUkyXjWuBHdQ(IntPtr P_0, out yvmnbQjDLoRvQsjOUsVFFNnaiTOB P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetClientRect")]
	public static extern bool mytFSopRSfKfvGaFVUwryvZAqNbb(IntPtr P_0, out fhzmgqLvPwRrSigefCPuEOcUnAmcb P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetWindowRect")]
	public static extern bool SFUuKbqeTiYalvkzspirFNRgYeht(IntPtr P_0, out fhzmgqLvPwRrSigefCPuEOcUnAmcb P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "MapVirtualKeyW")]
	public static extern uint OMiEatLraEuAUhwUQRfRevbaNPjh(uint P_0, uint P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetKeyboardLayout")]
	public static extern IntPtr huLKYyYmAgYIoGBjbRRPynZxDcSI(int P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetKeyboardLayoutNameW")]
	public static extern bool haLfpQfzqjLqOaeaSCkojNxtjUlb(IntPtr P_0);

	public static IntPtr dYTsadZkMhizYZtWgRTZblGzQsAK()
	{
		if (!UnityTools.isEditor && fYVxUhGKKXXntMOkVVqtQJOwVWJl != IntPtr.Zero)
		{
			return fYVxUhGKKXXntMOkVVqtQJOwVWJl;
		}
		return fYVxUhGKKXXntMOkVVqtQJOwVWJl = YGisaiGeNqqRwntxVYjYtjNyNnJF();
	}

	public static bool RRqeMfExvySlnKgkfsrPMRTdwRPjA()
	{
		try
		{
			if (wyvjImUtwnfaMnugOvzWMiIlneGo == 0)
			{
				bool flag;
				if (IntPtr.Size == 8)
				{
					wyvjImUtwnfaMnugOvzWMiIlneGo = 2;
				}
				else if (csyAnBAjkwsbHOpIwOKVUILVjSLyA(YpVdpdbqWCnFeBxgMBHHGCRqDpZKA(), out flag))
				{
					if (flag)
					{
						wyvjImUtwnfaMnugOvzWMiIlneGo = 2;
					}
					else
					{
						wyvjImUtwnfaMnugOvzWMiIlneGo = 1;
					}
				}
			}
		}
		catch
		{
			wyvjImUtwnfaMnugOvzWMiIlneGo = 1;
		}
		return wyvjImUtwnfaMnugOvzWMiIlneGo == 2;
	}
}

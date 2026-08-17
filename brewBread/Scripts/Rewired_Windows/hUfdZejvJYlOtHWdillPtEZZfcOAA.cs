using System;
using System.Runtime.InteropServices;
using System.Security;
using Rewired.Utils;

internal static class hUfdZejvJYlOtHWdillPtEZZfcOAA
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate bool rxQCQGfiKRnlPlVOzkaJmpXluied(IntPtr hwnd, IntPtr lParam);

	private static IntPtr KwjDKNDVyFfeKbepQfteBUEEsegIA = IntPtr.Zero;

	private static int SFEKRsOQSuYgfaNAlVQmgAWJqqDC;

	[DllImport("Kernel32.dll", EntryPoint = "GetLastError")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int pDahFhKKJBWtYOBaSjVdXTTgLmvX();

	[DllImport("Kernel32.dll", EntryPoint = "GetCurrentProcess")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr PZMVQDtkWyQcCgCSGlPNrcTXCxJD();

	[DllImport("Kernel32.dll", EntryPoint = "GetCurrentProcessId")]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint FYvKIxpcQCVCEiNDtJumzvSToBDP();

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "WaitNamedPipe")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int mxYSRywdJgoBYCXKslYtvbRKziVK(string P_0, int P_1);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "SetNamedPipeHandleState")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int FIhKvJWjplMikZsqQmXbsvLgsrIM(IntPtr P_0, ref int P_1, ref int P_2, ref int P_3);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "SetNamedPipeHandleState")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int FIhKvJWjplMikZsqQmXbsvLgsrIM(IntPtr P_0, ref int P_1, IntPtr P_2, IntPtr P_3);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "PeekNamedPipe")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool eSaGLHZCyHqvFLCaoHybgwmfZQAlA(IntPtr P_0, byte[] P_1, int P_2, out int P_3, out int P_4, out int P_5);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "HeapAlloc")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr epjccCgLZyPKKBgpUVGbYCOLCozS(IntPtr P_0, int P_1, UIntPtr P_2);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "HeapFree")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr OjYSJjjjRcDmtIixJavUicMGnLS(IntPtr P_0, int P_1, IntPtr P_2);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcessHeap")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr IUqyXTkKHMrEUCKfEFsfSTXDLuYJ();

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GlobalAlloc")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr PqtNNlxwUTFVNLFxBlgdyKorFcCx(uint P_0, UIntPtr P_1);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GlobalLock")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr aeFsgZlpDmYnUKMuWnojglCILYFR(IntPtr P_0);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GlobalUnlock")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool hDAHCHFgaXWUHmcVSkZYTnOgPcRJA(IntPtr P_0);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GlobalFree")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr KsofoHBGQLIvrGELgemHUJIAcMSr(IntPtr P_0);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetCurrentThreadId")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int kpDgSGvpkjJXDaTCjDTbmhiJKNju();

	[DllImport("Kernel32.dll", EntryPoint = "IsWow64Process")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool rKSTuPXpdNdwcJXnvCWzkLbZsIPq(IntPtr P_0, out bool P_1);

	[DllImport("user32.dll", CharSet = CharSet.Ansi, EntryPoint = "CreateWindowEx")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr ZjmFBtjidQAvyaPAzYNUjHRLqLTPA(int P_0, string P_1, string P_2, int P_3, int P_4, int P_5, int P_6, int P_7, IntPtr P_8, IntPtr P_9, IntPtr P_10, IntPtr P_11);

	[DllImport("user32.dll", EntryPoint = "DestroyWindow")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr bvNSnpvMuRkgdgaTRmyHkYdWBeNb(IntPtr P_0);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "CallWindowProc")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr ACHfxPraKCfXYMZVbcEyUdfUsKZX(IntPtr P_0, IntPtr P_1, uint P_2, IntPtr P_3, IntPtr P_4);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "IsWindow")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool OiaxAhOcCrpxoTRQQDMjFSbySsem(IntPtr P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetActiveWindow")]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr XasqhpShjwTjhkgYOQqEFaEZrrL();

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetFocus")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr YxhsNzDsAyzBjvZeHBDxPLFVZsnF();

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetForegroundWindow")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr yxZfeNLhHggOFLfUuATbJUJeBghvA();

	public static IntPtr LvjxsRoEpaQBxmxMmNxbVBlGLwBq(IntPtr P_0, int P_1)
	{
		if (IntPtr.Size == 4)
		{
			return vpdwZycqFSjXuHolzctgWndeejiKA(P_0, P_1);
		}
		return ntiVLkBFBIYAdizytqrDBprrGGai(P_0, P_1);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongW")]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr vpdwZycqFSjXuHolzctgWndeejiKA(IntPtr P_0, int P_1);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtrW")]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr ntiVLkBFBIYAdizytqrDBprrGGai(IntPtr P_0, int P_1);

	public static IntPtr sXPoaVBoARfqsHrUgPkAMXtOQWThA(IntPtr P_0, int P_1, IntPtr P_2)
	{
		if (IntPtr.Size == 4)
		{
			return TPvcdsQqfZPjOkiWkbhdQPPkBRUF(P_0, P_1, P_2);
		}
		return FatYSLcEXQcYVXMELwCPBfaepMZE(P_0, P_1, P_2);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongPtrW")]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr FatYSLcEXQcYVXMELwCPBfaepMZE(IntPtr P_0, int P_1, IntPtr P_2);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongW")]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr TPvcdsQqfZPjOkiWkbhdQPPkBRUF(IntPtr P_0, int P_1, IntPtr P_2);

	[DllImport("user32.dll", EntryPoint = "DefWindowProcW", SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr XCEeOpdNZXUgBsKznZZbtbwNBVsSA(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3);

	[DllImport("User32.dll", EntryPoint = "EnumWindows")]
	[SuppressUnmanagedCodeSecurity]
	private static extern bool PnzGFRatjaFRFbQjdmoqUwILrTEhc(IntPtr P_0, IntPtr P_1);

	[DllImport("User32.dll", EntryPoint = "GetWindowThreadProcessId")]
	[SuppressUnmanagedCodeSecurity]
	private static extern uint riLaQpRuycaQTPJgrpIQUzHViUGw(IntPtr P_0, out uint P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputDeviceList")]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint xJntIHwMDcSpfyEtrJoJZqCpxrBp(IntPtr P_0, ref uint P_1, uint P_2);

	[DllImport("User32.dll", EntryPoint = "GetRegisteredRawInputDevices")]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint ZrFXRoJfQbCyPDMySqZOBLTYHRbz(IntPtr P_0, ref uint P_1, uint P_2);

	[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputDeviceInfoW")]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint xquTvKTSrStXSEftQWbKmpoxwpId(IntPtr P_0, uint P_1, IntPtr P_2, out uint P_3);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputData")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int QrtIIeijKaLqQuiEMWMamfcKifAu(IntPtr P_0, uint P_1, IntPtr P_2, out uint P_3, uint P_4);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputBuffer")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int jREMCAOOqKFEdiXCIFUaEhjIOxWhB(IntPtr P_0, ref uint P_1, uint P_2);

	[DllImport("User32.dll", EntryPoint = "SwapMouseButton")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool WtOaYTmsxNhiahMcpQWQfdksVzIs(bool P_0);

	[DllImport("User32.dll", EntryPoint = "SystemParametersInfo")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool XDwFnuwWAuhajDTIKKbdZeygFPShA(uint P_0, uint P_1, ref int P_2, uint P_3);

	[DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int MoJDOljHJxbeLpaYnjiAqKBpcYFvA(int P_0);

	[DllImport("User32.dll", EntryPoint = "GetMessageW")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool WPhSxZiUCJIqMpAZMJWvHfAuPVlT(IntPtr P_0, IntPtr P_1, uint P_2, uint P_3);

	[DllImport("User32.dll", EntryPoint = "GetMessageW")]
	[SuppressUnmanagedCodeSecurity]
	public unsafe static extern bool WPhSxZiUCJIqMpAZMJWvHfAuPVlT(void* P_0, void* P_1, uint P_2, uint P_3);

	[DllImport("User32.dll", EntryPoint = "PeekMessageW")]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public unsafe static extern bool qjqbUcthaAKYqaAxhJPjopVOvLUg(void* P_0, IntPtr P_1, uint P_2, uint P_3, uint P_4);

	[DllImport("User32.dll", EntryPoint = "PeekMessageW")]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool qjqbUcthaAKYqaAxhJPjopVOvLUg(byte[] P_0, IntPtr P_1, uint P_2, uint P_3, uint P_4);

	[DllImport("User32.dll", EntryPoint = "DispatchMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr EUBhJZBQvXcywmfgCfMxlevvhZPO(byte[] P_0);

	[DllImport("User32.dll", EntryPoint = "DispatchMessage")]
	[SuppressUnmanagedCodeSecurity]
	public unsafe static extern IntPtr EUBhJZBQvXcywmfgCfMxlevvhZPO(void* P_0);

	[DllImport("User32.dll", EntryPoint = "TranslateMessage")]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool aOzCyHOyAQhAVZaEVexnHIoASjliA(byte[] P_0);

	[DllImport("User32.dll", EntryPoint = "TranslateMessage")]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	public unsafe static extern bool aOzCyHOyAQhAVZaEVexnHIoASjliA(void* P_0);

	[DllImport("User32.dll", EntryPoint = "SendMessage")]
	[SuppressUnmanagedCodeSecurity]
	public unsafe static extern void* gTWdBpIHgslBwbWtBRmLkkgenhzUb(void* P_0, uint P_1, void* P_2, void* P_3);

	[DllImport("User32.dll", EntryPoint = "SendMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr gTWdBpIHgslBwbWtBRmLkkgenhzUb(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3);

	[DllImport("User32.dll", EntryPoint = "SendMessageTimeout")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr fahsknuVSGUGvtTguoQZasfhROHT(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3, uint P_4, uint P_5, IntPtr P_6);

	[DllImport("User32.dll", EntryPoint = "PostMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool urPaaNeBlJiDPWTuKjKaeNZjlkvc(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3);

	[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "PostThreadMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool aTGykTBGpqYHTkyjVIxoTzEAmkZx(int P_0, uint P_1, IntPtr P_2, IntPtr P_3);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "SetCursorPos")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool uLSGtBWALLbjJKbBeKDqVGEGfcTH(int P_0, int P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCursorPos")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool fEVdYbdhbJnGjBnmxunFKiSVekEB(out TeeEvShNbEAwHqxvHccQNVFhiujR P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "OpenInputDesktop")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr waCzeuZmDRenuGpeuOhpunfqJEOfA(uint P_0, bool P_1, uint P_2);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetKeyState")]
	[SuppressUnmanagedCodeSecurity]
	public static extern short WKSBsWJmlgVnqTuukSAoGjnUBIMe(int P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetAsyncKeyState")]
	[SuppressUnmanagedCodeSecurity]
	public static extern short veqWkKOWloyHtLljMViwvgYSEeJJ(int P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetKeyboardState")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool WBRfNwKGANBNqdomITSeotxcRSyOA(IntPtr P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "ClientToScreen")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool deYrdqkeNPjGdmhlrfEMelPdDsRnA(IntPtr P_0, out TeeEvShNbEAwHqxvHccQNVFhiujR P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetClientRect")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool QLQLhKHtdIxKoJdVkbmitjMBJhYEA(IntPtr P_0, out gDhBjsxtDCXZFOcJIzBWllEGJgSJ P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetWindowRect")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool aAuqFlmzIsPeHaOMPjdAaXUUDyPBA(IntPtr P_0, out gDhBjsxtDCXZFOcJIzBWllEGJgSJ P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "MapVirtualKeyW")]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint dTHamguEmaecjVsZKWWQZXqaYLOh(uint P_0, uint P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "MapVirtualKeyExW")]
	[SuppressUnmanagedCodeSecurity]
	public static extern uint UEQqNzJuGluvSeBAsuRcFDHVgPZu(uint P_0, uint P_1, IntPtr P_2);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetKeyboardLayout")]
	[SuppressUnmanagedCodeSecurity]
	public static extern IntPtr xVinStRXVsoEQAHEqKYJhotMMwUE(int P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetKeyboardLayoutNameW")]
	[SuppressUnmanagedCodeSecurity]
	public static extern bool MWKAPeYxRKQNMHPETsyGSUaYEaph(IntPtr P_0);

	[DllImport("msvcrt.dll", EntryPoint = "memcpy")]
	[SuppressUnmanagedCodeSecurity]
	public unsafe static extern bool FZwJwYXvSsXioalMbVsJdESnycGk(void* P_0, void* P_1, UIntPtr P_2);

	public unsafe static bool FZwJwYXvSsXioalMbVsJdESnycGk(void* P_0, void* P_1, int P_2)
	{
		return FZwJwYXvSsXioalMbVsJdESnycGk(P_0, P_1, new UIntPtr((uint)P_2));
	}

	public static IntPtr rAUvmTfVyKUAaAHERFekLGhGYfNm()
	{
		if (!UnityTools.isEditor && KwjDKNDVyFfeKbepQfteBUEEsegIA != IntPtr.Zero)
		{
			return KwjDKNDVyFfeKbepQfteBUEEsegIA;
		}
		return KwjDKNDVyFfeKbepQfteBUEEsegIA = XasqhpShjwTjhkgYOQqEFaEZrrL();
	}

	public static bool ADSzpqwvSxTBgtWGcBYWKMvKpntb()
	{
		try
		{
			if (SFEKRsOQSuYgfaNAlVQmgAWJqqDC == 0)
			{
				bool flag;
				if (IntPtr.Size == 8)
				{
					SFEKRsOQSuYgfaNAlVQmgAWJqqDC = 2;
				}
				else if (rKSTuPXpdNdwcJXnvCWzkLbZsIPq(PZMVQDtkWyQcCgCSGlPNrcTXCxJD(), out flag))
				{
					if (flag)
					{
						SFEKRsOQSuYgfaNAlVQmgAWJqqDC = 2;
					}
					else
					{
						SFEKRsOQSuYgfaNAlVQmgAWJqqDC = 1;
					}
				}
			}
		}
		catch
		{
			SFEKRsOQSuYgfaNAlVQmgAWJqqDC = 1;
		}
		return SFEKRsOQSuYgfaNAlVQmgAWJqqDC == 2;
	}
}

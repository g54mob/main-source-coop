using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

internal class TGhtjJFdIheEFBuvrEuzmaodtCjb
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class uqPkvBoEPhfzaspnOcFmcNegELlZA
	{
		public int CkDdtUIoSgbxDvfjtOVTqLiGbEQEA;

		public int xPfWAsMgJyBtTNGvswBGzrwEZbpF;

		public int RuCbnUXKgwrbHXguymEBCSAoViRS;

		public int ChRJSRKXLoNDapSRPEynPVOZdUHiA;

		public int AKIdHKUulFmusbquXVdLyaCmlfLO;

		public byte wVcIDiGFOcVbCAsoyEkfjIzLwqxj;

		public byte wbTSqoPByrKqsryKBahRiJXxSwSi;

		public byte jVrHnPSITkbTWuuBNUrbdvZkkkVX;

		public byte tGYXeYfakhRiSNlspsROIdffxfkh;

		public byte gSbJkKynoCVseCnAcFEScEYJnzAaA;

		public byte SnBDggjwhVgONkXDVQUTFYMHdobI;

		public byte dgzwEYiQoiGHoWVFRsxbyacrGnjS;

		public byte FAnPwpLtRzTByhijPmzjOJAcgWFS;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string VpKBAGNpMowqTCETSLzQzdHjbFRJ;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct eyjbInGadmAdXPvSmIavDFesdabDb
	{
		public int CPDamvBMmgZRmlihUtauBoVKnSYGb;

		public int aMFdsOetezuqtIKsihHGfTCEjYnNB;

		public int QgBnLMkuGwkDsPKNWFLRHtyunrdx;

		public int zsHegpAhTpWbpbKAWkxSKMChgskCb;

		public int qUhumwpgGKudPjsCGIHvJcwQNSBf;

		public int UcZZfmooNMPEOdJvGyuWydcrOlbt;

		public int FDkphSjmClDWfUWrazJNJDDlknOC;

		public int kQYdZAEcAtVeAUzUDxvERGcnapdbA;

		public int tlpwYJypGNqYaYZdcBQBYsDJDOjCA;

		public int LehabMMJtWmjFPakMflZnDtBaKCw;

		public int MLKnuITvYccmtdugFbSsCcGRgvzZ;

		public char OEKeZnZwAhYrpbwVmUGlpiSqzFRJ;

		public char YcViQwjMEhuVmJFwamRjpzzlorhx;

		public char zTXCkjgyTywfgUGJzkAcoAGebLMjb;

		public char WuhmNuRSTaiCnpoocbutcVdqxxJdA;

		public byte XiMowyEzvArjlDspJhjkUoMdFUbg;

		public byte QzFyMEUZdfSyBDywZgKdMmPGYIFp;

		public byte JTBCHFrngXaqrRJHdjzYvyvJCOSg;

		public byte xASnCcThLAjMvcHlZjfePLVIVrBNA;

		public byte RdzFmcotBNHJIRpAKVKqEYzcmgui;
	}

	internal enum VsotYxPVhrNWFRZhLebcZIOCPJdt
	{
		WndProc = -4,
		HInstance = -6,
		HwndParent = -8,
		Style = -16,
		ExtendedStyle = -20,
		UserData = -21,
		Id = -12
	}

	internal delegate IntPtr vfEJnndyqcOtrbonChfylNUGxtyc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

	private delegate bool RrcvAsgVSLelkbJcIUacEnFcJyZh(IntPtr hwnd, IntPtr lParam);

	private static IntPtr KwjDKNDVyFfeKbepQfteBUEEsegIA = IntPtr.Zero;

	private static List<IntPtr> rcbsQeiBPptnnQXLPbbKpjLKxrYi;

	[DllImport("user32.dll", EntryPoint = "PeekMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int ZaLQqurYkCrFgTDRZsqyoKLQcjAq(out SWKlQJedidpoVfHLleYoopJddPBD P_0, IntPtr P_1, int P_2, int P_3, int P_4);

	[DllImport("user32.dll", EntryPoint = "GetMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int zCmDUWJUeAyKjyQQtuhyWDmdMzWT(out SWKlQJedidpoVfHLleYoopJddPBD P_0, IntPtr P_1, int P_2, int P_3);

	[DllImport("user32.dll", EntryPoint = "TranslateMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int aOzCyHOyAQhAVZaEVexnHIoASjliA(ref SWKlQJedidpoVfHLleYoopJddPBD P_0);

	[DllImport("user32.dll", EntryPoint = "DispatchMessage")]
	[SuppressUnmanagedCodeSecurity]
	public static extern int EUBhJZBQvXcywmfgCfMxlevvhZPO(ref SWKlQJedidpoVfHLleYoopJddPBD P_0);

	public static IntPtr LvjxsRoEpaQBxmxMmNxbVBlGLwBq(HandleRef P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1)
	{
		if (IntPtr.Size == 4)
		{
			return vpdwZycqFSjXuHolzctgWndeejiKA(P_0, P_1);
		}
		return ntiVLkBFBIYAdizytqrDBprrGGai(P_0, P_1);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetFocus")]
	public static extern IntPtr YxhsNzDsAyzBjvZeHBDxPLFVZsnF();

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLong")]
	private static extern IntPtr vpdwZycqFSjXuHolzctgWndeejiKA(HandleRef P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtr")]
	private static extern IntPtr ntiVLkBFBIYAdizytqrDBprrGGai(HandleRef P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1);

	public static IntPtr sXPoaVBoARfqsHrUgPkAMXtOQWThA(HandleRef P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1, IntPtr P_2)
	{
		if (IntPtr.Size == 4)
		{
			return TPvcdsQqfZPjOkiWkbhdQPPkBRUF(P_0, P_1, P_2);
		}
		return cNaUxkjCRsFPLIvNjjAdFhtKsJLv(P_0, P_1, P_2);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetParent")]
	public static extern IntPtr TQGnxGMKLskVksdjIulJOeVmJZDI(HandleRef P_0, IntPtr P_1);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLong")]
	private static extern IntPtr TPvcdsQqfZPjOkiWkbhdQPPkBRUF(HandleRef P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1, IntPtr P_2);

	public static bool rZVGejTfTtdfLVJJRgzzXqqmFefaA(HandleRef P_0, bool P_1)
	{
		return rZVGejTfTtdfLVJJRgzzXqqmFefaA(P_0, P_1 ? 1 : 0);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "ShowWindow")]
	private static extern bool rZVGejTfTtdfLVJJRgzzXqqmFefaA(HandleRef P_0, int P_1);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongPtr")]
	private static extern IntPtr cNaUxkjCRsFPLIvNjjAdFhtKsJLv(HandleRef P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1, IntPtr P_2);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "CallWindowProc")]
	public static extern IntPtr ACHfxPraKCfXYMZVbcEyUdfUsKZX(IntPtr P_0, IntPtr P_1, int P_2, IntPtr P_3, IntPtr P_4);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandle")]
	public static extern IntPtr jiRVeyAUaSPHpcRKnthczSpKtDOW(string P_0);

	public static IntPtr LvjxsRoEpaQBxmxMmNxbVBlGLwBq(IntPtr P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1)
	{
		if (IntPtr.Size == 4)
		{
			return vpdwZycqFSjXuHolzctgWndeejiKA(P_0, P_1);
		}
		return ntiVLkBFBIYAdizytqrDBprrGGai(P_0, P_1);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLong")]
	private static extern IntPtr vpdwZycqFSjXuHolzctgWndeejiKA(IntPtr P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtr")]
	private static extern IntPtr ntiVLkBFBIYAdizytqrDBprrGGai(IntPtr P_0, VsotYxPVhrNWFRZhLebcZIOCPJdt P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "IsWindow")]
	public static extern bool OiaxAhOcCrpxoTRQQDMjFSbySsem(IntPtr P_0);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetActiveWindow")]
	private static extern IntPtr XasqhpShjwTjhkgYOQqEFaEZrrL();

	[DllImport("Kernel32", EntryPoint = "GetCurrentProcessId")]
	private static extern uint FYvKIxpcQCVCEiNDtJumzvSToBDP();

	[DllImport("User32.dll", EntryPoint = "EnumWindows")]
	private static extern bool PnzGFRatjaFRFbQjdmoqUwILrTEhc(IntPtr P_0, IntPtr P_1);

	private static bool oBCBhdFrHeQVchUnaQrsFUCcPldL(IntPtr P_0, IntPtr P_1)
	{
		lock (rcbsQeiBPptnnQXLPbbKpjLKxrYi)
		{
			rcbsQeiBPptnnQXLPbbKpjLKxrYi.Add(P_0);
		}
		return true;
	}

	[DllImport("User32.dll", EntryPoint = "GetWindowThreadProcessId")]
	private static extern uint riLaQpRuycaQTPJgrpIQUzHViUGw(IntPtr P_0, out uint P_1);

	public static IntPtr rAUvmTfVyKUAaAHERFekLGhGYfNm()
	{
		if (KwjDKNDVyFfeKbepQfteBUEEsegIA != IntPtr.Zero)
		{
			return KwjDKNDVyFfeKbepQfteBUEEsegIA;
		}
		rcbsQeiBPptnnQXLPbbKpjLKxrYi = new List<IntPtr>();
		uint num = FYvKIxpcQCVCEiNDtJumzvSToBDP();
		RrcvAsgVSLelkbJcIUacEnFcJyZh rrcvAsgVSLelkbJcIUacEnFcJyZh = oBCBhdFrHeQVchUnaQrsFUCcPldL;
		IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate((Delegate)rrcvAsgVSLelkbJcIUacEnFcJyZh);
		PnzGFRatjaFRFbQjdmoqUwILrTEhc(functionPointerForDelegate, IntPtr.Zero);
		GC.KeepAlive(rrcvAsgVSLelkbJcIUacEnFcJyZh);
		GC.KeepAlive(functionPointerForDelegate);
		for (int i = 0; i < rcbsQeiBPptnnQXLPbbKpjLKxrYi.Count; i++)
		{
			if (OiaxAhOcCrpxoTRQQDMjFSbySsem(rcbsQeiBPptnnQXLPbbKpjLKxrYi[i]))
			{
				riLaQpRuycaQTPJgrpIQUzHViUGw(rcbsQeiBPptnnQXLPbbKpjLKxrYi[i], out var num2);
				if (num2 == num)
				{
					KwjDKNDVyFfeKbepQfteBUEEsegIA = rcbsQeiBPptnnQXLPbbKpjLKxrYi[i];
					rcbsQeiBPptnnQXLPbbKpjLKxrYi.Clear();
					return KwjDKNDVyFfeKbepQfteBUEEsegIA;
				}
			}
		}
		return XasqhpShjwTjhkgYOQqEFaEZrrL();
	}
}

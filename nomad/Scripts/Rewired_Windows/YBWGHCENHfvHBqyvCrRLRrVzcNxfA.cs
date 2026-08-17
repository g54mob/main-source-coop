using System.Runtime.InteropServices;

internal static class YBWGHCENHfvHBqyvCrRLRrVzcNxfA
{
	public unsafe static int AFfhLxFvMyLtdvUzYjopXhRMaZvx(int P_0, wiMVRiZUpecBrLLXIdvoBrQxZgxk P_1)
	{
		return jZFwMLNbcMoydzYcaoDKyalSdizf(P_0, &P_1);
	}

	private unsafe static int jZFwMLNbcMoydzYcaoDKyalSdizf(int P_0, void* P_1)
	{
		return wHjfVJsrMiRUzqkXLzlmJabeEUxs.SwAUHbwiWlRhjFcSIvOWVHKYyKUJ switch
		{
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_4 => azsTsEuhqlffvDQGaLmYFsWHVgrzA(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_3 => EHrWPuOlbiujjwIzDhSHNReNWFCf(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_2 => TgoreXTdZEMHOqznedzhKamqUlAc(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_1 => uCXexsAGRPagHikXFHZRISxfNwaDB(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_9_1_0 => aWYiwnGAiImmvWghGCUdHBKfxFGtA(P_0, P_1), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	private unsafe static extern int aWYiwnGAiImmvWghGCUdHBKfxFGtA(int P_0, void* P_1);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	private unsafe static extern int uCXexsAGRPagHikXFHZRISxfNwaDB(int P_0, void* P_1);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	private unsafe static extern int TgoreXTdZEMHOqznedzhKamqUlAc(int P_0, void* P_1);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	private unsafe static extern int EHrWPuOlbiujjwIzDhSHNReNWFCf(int P_0, void* P_1);

	[DllImport("xinput1_4.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	private unsafe static extern int azsTsEuhqlffvDQGaLmYFsWHVgrzA(int P_0, void* P_1);

	public unsafe static int uDmJXotUvjydYGfulikILkTfTQnC(int P_0, out pKkCzsEckBaUoBcvMDSUkuZdVCzgA P_1)
	{
		P_1 = default(pKkCzsEckBaUoBcvMDSUkuZdVCzgA);
		int result;
		fixed (pKkCzsEckBaUoBcvMDSUkuZdVCzgA* ptr = &P_1)
		{
			void* ptr2 = ptr;
			result = ZHDfvWrjxMZyhrJSRbONhKywBcNcA(P_0, ptr2);
		}
		return result;
	}

	private unsafe static int ZHDfvWrjxMZyhrJSRbONhKywBcNcA(int P_0, void* P_1)
	{
		if (wHjfVJsrMiRUzqkXLzlmJabeEUxs.qGuinsZyvsIwtWKCiMUujAADchchA && wHjfVJsrMiRUzqkXLzlmJabeEUxs.caSmkZlhbthYMNYMgfgArSjdTIYM != null)
		{
			return wHjfVJsrMiRUzqkXLzlmJabeEUxs.caSmkZlhbthYMNYMgfgArSjdTIYM(P_0, P_1);
		}
		return wHjfVJsrMiRUzqkXLzlmJabeEUxs.SwAUHbwiWlRhjFcSIvOWVHKYyKUJ switch
		{
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_4 => pnvuOEIsmuqnpqjCLElrYdxvIyCO(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_3 => DRBiOwNEdCERtKnnoChjuwtvlVzfA(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_2 => tzbFzreXhZSTBeIsgLVTkImiLfrM(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_1 => xcjiGNIehpoaHyMkhnXhFcfHMMXBb(P_0, P_1), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_9_1_0 => BffBvsMebtdPbOUpWoVhelpRgpzM(P_0, P_1), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int BffBvsMebtdPbOUpWoVhelpRgpzM(int P_0, void* P_1);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int xcjiGNIehpoaHyMkhnXhFcfHMMXBb(int P_0, void* P_1);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int tzbFzreXhZSTBeIsgLVTkImiLfrM(int P_0, void* P_1);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int DRBiOwNEdCERtKnnoChjuwtvlVzfA(int P_0, void* P_1);

	[DllImport("xinput1_4.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	private unsafe static extern int pnvuOEIsmuqnpqjCLElrYdxvIyCO(int P_0, void* P_1);

	public unsafe static int qomjhZagmOFxkhXEItLzssdrXAkU(int P_0, JymHAvrnPSxZRGvgWOgLWQQayzhF P_1, out ahTDkqWAlkFHaatFgCIQfUIUlUbYA P_2)
	{
		P_2 = default(ahTDkqWAlkFHaatFgCIQfUIUlUbYA);
		int result;
		fixed (ahTDkqWAlkFHaatFgCIQfUIUlUbYA* ptr = &P_2)
		{
			void* ptr2 = ptr;
			result = EWaijccnAQzUwwnelayaWFBgdQSD(P_0, (int)P_1, ptr2);
		}
		return result;
	}

	private unsafe static int EWaijccnAQzUwwnelayaWFBgdQSD(int P_0, int P_1, void* P_2)
	{
		return wHjfVJsrMiRUzqkXLzlmJabeEUxs.SwAUHbwiWlRhjFcSIvOWVHKYyKUJ switch
		{
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_4 => nYVsQhuhxgBBRTaMmsAamhePdLLu(P_0, P_1, P_2), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_3 => NDTxYVKaXrnBINZRWHnufCFhekxs(P_0, P_1, P_2), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_2 => KhTGbnZUyuwuJZlokHfmtRazCFxhA(P_0, P_1, P_2), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_1 => AJUqclbrLuyVooIRnEcHCGGhdCiDA(P_0, P_1, P_2), 
			akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_9_1_0 => jmhfTBLOQSVkzqqaVRKbppnBziiK(P_0, P_1, P_2), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	private unsafe static extern int jmhfTBLOQSVkzqqaVRKbppnBziiK(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	private unsafe static extern int AJUqclbrLuyVooIRnEcHCGGhdCiDA(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	private unsafe static extern int KhTGbnZUyuwuJZlokHfmtRazCFxhA(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	private unsafe static extern int NDTxYVKaXrnBINZRWHnufCFhekxs(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_4.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	private unsafe static extern int nYVsQhuhxgBBRTaMmsAamhePdLLu(int P_0, int P_1, void* P_2);
}

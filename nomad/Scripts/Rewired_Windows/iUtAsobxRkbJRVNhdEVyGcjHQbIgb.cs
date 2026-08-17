using System;
using System.Runtime.InteropServices;

internal static class iUtAsobxRkbJRVNhdEVyGcjHQbIgb
{
	public unsafe static void IaRStIzfiUbWMeSpQjkWjwTuRrMeA(IntPtr P_0, int P_1, Guid P_2, out IntPtr P_3, TjLvFIATAwjKUDtcUGvSPgBzGvgS P_4)
	{
		EjRzaRKGYKofIRZTFuJKtdAsBqBO ejRzaRKGYKofIRZTFuJKtdAsBqBO;
		fixed (IntPtr* ptr = &P_3)
		{
			void* ptr2 = ptr;
			ejRzaRKGYKofIRZTFuJKtdAsBqBO = EjRzaRKGYKofIRZTFuJKtdAsBqBO.iQZjciUBNGGHNWpDRHUhgmbGszVTA(NpdoTIOCqJJDaJNjKNhSMxFRtdnU((void*)P_0, P_1, &P_2, ptr2, (void*)(P_4?.fREGeAsscSanGSwlvHwWDQIMIYWO ?? IntPtr.Zero)));
		}
		ejRzaRKGYKofIRZTFuJKtdAsBqBO.IVrhqXVVUiCwtqPLnSGEExdSIomv();
	}

	[DllImport("Rewired_DirectInput", CallingConvention = CallingConvention.StdCall, EntryPoint = "DirectInput8_Create")]
	private unsafe static extern int NpdoTIOCqJJDaJNjKNhSMxFRtdnU(void* P_0, int P_1, void* P_2, void* P_3, void* P_4);
}

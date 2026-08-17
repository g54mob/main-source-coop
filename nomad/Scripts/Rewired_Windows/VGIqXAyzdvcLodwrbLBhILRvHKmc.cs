using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class VGIqXAyzdvcLodwrbLBhILRvHKmc
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private unsafe delegate int VWIMQDKTAIzYlSDibKhjdCjczXDe(void* deviceInstance, IntPtr data);

	private readonly IntPtr HsRklwcWdSpbrexIahaOQLjzCkLCA;

	private readonly VWIMQDKTAIzYlSDibKhjdCjczXDe nGNctuDKuGMVMDkopqkjMZirBjNGb;

	[CompilerGenerated]
	private List<bqkAwqSafotyQAOrTnFIVdkzxoBR> slqGkMGUMTgMzudexSKbcCoxzYqS;

	public IntPtr LVDoIFWkBJmLoIsRnJrPGogduGoU => HsRklwcWdSpbrexIahaOQLjzCkLCA;

	public List<bqkAwqSafotyQAOrTnFIVdkzxoBR> eCQsTQaQVrbzxiMgiozNPKxAJHCH
	{
		[CompilerGenerated]
		get
		{
			return slqGkMGUMTgMzudexSKbcCoxzYqS;
		}
		[CompilerGenerated]
		private set
		{
			slqGkMGUMTgMzudexSKbcCoxzYqS = list;
		}
	}

	public unsafe VGIqXAyzdvcLodwrbLBhILRvHKmc()
	{
		nGNctuDKuGMVMDkopqkjMZirBjNGb = JFsTcghxZoMhVtAejgotjPSnFxCgA;
		HsRklwcWdSpbrexIahaOQLjzCkLCA = Marshal.GetFunctionPointerForDelegate(nGNctuDKuGMVMDkopqkjMZirBjNGb);
		eCQsTQaQVrbzxiMgiozNPKxAJHCH = new List<bqkAwqSafotyQAOrTnFIVdkzxoBR>();
	}

	[MonoPInvokeCallback(typeof(VWIMQDKTAIzYlSDibKhjdCjczXDe))]
	private unsafe static int JFsTcghxZoMhVtAejgotjPSnFxCgA(void* P_0, IntPtr P_1)
	{
		uint instanceId = (uint)P_1.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<VGIqXAyzdvcLodwrbLBhILRvHKmc>(instanceId, out var instance))
		{
			return 1;
		}
		bqkAwqSafotyQAOrTnFIVdkzxoBR bqkAwqSafotyQAOrTnFIVdkzxoBR2 = new bqkAwqSafotyQAOrTnFIVdkzxoBR();
		bqkAwqSafotyQAOrTnFIVdkzxoBR2.qNTAzoWwNsVePMqHDPBuNjHfkTxH(ref *(bqkAwqSafotyQAOrTnFIVdkzxoBR.OurDhvBqJDVMKOcFsAeVpDKvoONw*)P_0);
		instance.eCQsTQaQVrbzxiMgiozNPKxAJHCH.Add(bqkAwqSafotyQAOrTnFIVdkzxoBR2);
		return 1;
	}
}

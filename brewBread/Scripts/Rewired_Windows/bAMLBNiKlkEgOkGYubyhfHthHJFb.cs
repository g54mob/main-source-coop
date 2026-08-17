using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class bAMLBNiKlkEgOkGYubyhfHthHJFb
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private unsafe delegate int iyldZCALRskApdSweCBjttitvDVXB(void* deviceInstance, IntPtr data);

	private readonly IntPtr CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	private readonly iyldZCALRskApdSweCBjttitvDVXB SiXixQSMucyuJvNwhknxJlowTWAA;

	[CompilerGenerated]
	private List<NVwTzNgKkuSLFJbpbcJcHWqWnvPe> WmMFhVmsWgyHgPieKlTdobDNfEXJ;

	public IntPtr KUJrOBoduRcGKIkxgMoKicmgtCeb => CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	public List<NVwTzNgKkuSLFJbpbcJcHWqWnvPe> ycpPAyYUsHLJexCTimmYMDNNBpWaA
	{
		[CompilerGenerated]
		get
		{
			return WmMFhVmsWgyHgPieKlTdobDNfEXJ;
		}
		[CompilerGenerated]
		private set
		{
			WmMFhVmsWgyHgPieKlTdobDNfEXJ = wmMFhVmsWgyHgPieKlTdobDNfEXJ;
		}
	}

	public unsafe bAMLBNiKlkEgOkGYubyhfHthHJFb()
	{
		SiXixQSMucyuJvNwhknxJlowTWAA = XaGwzzKBSYGzrzBlAtUAAoMdwkcw;
		CwTGIOKdRnDIcHksMHqFRvtSvOMs = Marshal.GetFunctionPointerForDelegate((Delegate)SiXixQSMucyuJvNwhknxJlowTWAA);
		ycpPAyYUsHLJexCTimmYMDNNBpWaA = new List<NVwTzNgKkuSLFJbpbcJcHWqWnvPe>();
	}

	[MonoPInvokeCallback(typeof(iyldZCALRskApdSweCBjttitvDVXB))]
	private unsafe static int XaGwzzKBSYGzrzBlAtUAAoMdwkcw(void* P_0, IntPtr P_1)
	{
		uint instanceId = (uint)P_1.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<bAMLBNiKlkEgOkGYubyhfHthHJFb>(instanceId, out var instance))
		{
			return 1;
		}
		NVwTzNgKkuSLFJbpbcJcHWqWnvPe nVwTzNgKkuSLFJbpbcJcHWqWnvPe = new NVwTzNgKkuSLFJbpbcJcHWqWnvPe();
		nVwTzNgKkuSLFJbpbcJcHWqWnvPe.aLBrdGWxOITKFkixRhYnVqCkTJFk(ref *(NVwTzNgKkuSLFJbpbcJcHWqWnvPe.qbTLtnXnDzjBjhKwmSSkkPcAKYboA*)P_0);
		instance.ycpPAyYUsHLJexCTimmYMDNNBpWaA.Add(nVwTzNgKkuSLFJbpbcJcHWqWnvPe);
		return 1;
	}
}

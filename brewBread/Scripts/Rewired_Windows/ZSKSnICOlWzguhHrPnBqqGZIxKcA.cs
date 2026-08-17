using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class ZSKSnICOlWzguhHrPnBqqGZIxKcA
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private unsafe delegate int peNvRUqnjefNpKaXsmbZDRSCgOQfc(void* deviceInstance, IntPtr data);

	private readonly IntPtr CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	private readonly peNvRUqnjefNpKaXsmbZDRSCgOQfc SiXixQSMucyuJvNwhknxJlowTWAA;

	[CompilerGenerated]
	private List<UrQudlfQVOAKZheElVESDyRlnaXO> kITJQbJcZYTtChptjsdCfjoonIFR;

	public IntPtr KUJrOBoduRcGKIkxgMoKicmgtCeb => CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	public List<UrQudlfQVOAKZheElVESDyRlnaXO> cmuFPtCMZJGjQkIGDQltpvfuzLRNA
	{
		[CompilerGenerated]
		get
		{
			return kITJQbJcZYTtChptjsdCfjoonIFR;
		}
		[CompilerGenerated]
		private set
		{
			kITJQbJcZYTtChptjsdCfjoonIFR = list;
		}
	}

	public unsafe ZSKSnICOlWzguhHrPnBqqGZIxKcA()
	{
		SiXixQSMucyuJvNwhknxJlowTWAA = gWqgVPPWiGJDkEcJwbnikhoGbVqGb;
		CwTGIOKdRnDIcHksMHqFRvtSvOMs = Marshal.GetFunctionPointerForDelegate((Delegate)SiXixQSMucyuJvNwhknxJlowTWAA);
		cmuFPtCMZJGjQkIGDQltpvfuzLRNA = new List<UrQudlfQVOAKZheElVESDyRlnaXO>();
	}

	[MonoPInvokeCallback(typeof(peNvRUqnjefNpKaXsmbZDRSCgOQfc))]
	private unsafe static int gWqgVPPWiGJDkEcJwbnikhoGbVqGb(void* P_0, IntPtr P_1)
	{
		uint instanceId = (uint)P_1.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<ZSKSnICOlWzguhHrPnBqqGZIxKcA>(instanceId, out var instance))
		{
			return 1;
		}
		UrQudlfQVOAKZheElVESDyRlnaXO urQudlfQVOAKZheElVESDyRlnaXO = new UrQudlfQVOAKZheElVESDyRlnaXO();
		urQudlfQVOAKZheElVESDyRlnaXO.aLBrdGWxOITKFkixRhYnVqCkTJFk(ref *(UrQudlfQVOAKZheElVESDyRlnaXO.INpozqULrjEEMEIzcVOrSSHoCkMs*)P_0);
		instance.cmuFPtCMZJGjQkIGDQltpvfuzLRNA.Add(urQudlfQVOAKZheElVESDyRlnaXO);
		return 1;
	}
}

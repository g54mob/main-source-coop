using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class YDUBCRqwmtZySxoSSmbDvJJshDRt
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private unsafe delegate int ebNlRzsqTxWcPAQlcXwOiVEuciow(void* deviceInstance, IntPtr data);

	private readonly IntPtr CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	private readonly ebNlRzsqTxWcPAQlcXwOiVEuciow SiXixQSMucyuJvNwhknxJlowTWAA;

	[CompilerGenerated]
	private List<gSsnEkKQgoAoVCLJetnIqykecuEr> JCYtOkgJIMCUgdtrDJjKnqtFsHJTA;

	public IntPtr KUJrOBoduRcGKIkxgMoKicmgtCeb => CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	public List<gSsnEkKQgoAoVCLJetnIqykecuEr> APBGWwAbDVNwYlcfhYtGSjEetvtz
	{
		[CompilerGenerated]
		get
		{
			return JCYtOkgJIMCUgdtrDJjKnqtFsHJTA;
		}
		[CompilerGenerated]
		private set
		{
			JCYtOkgJIMCUgdtrDJjKnqtFsHJTA = jCYtOkgJIMCUgdtrDJjKnqtFsHJTA;
		}
	}

	public unsafe YDUBCRqwmtZySxoSSmbDvJJshDRt()
	{
		SiXixQSMucyuJvNwhknxJlowTWAA = UWxXOePewRkiVywJmUPBnEuJMtvm;
		CwTGIOKdRnDIcHksMHqFRvtSvOMs = Marshal.GetFunctionPointerForDelegate((Delegate)SiXixQSMucyuJvNwhknxJlowTWAA);
		APBGWwAbDVNwYlcfhYtGSjEetvtz = new List<gSsnEkKQgoAoVCLJetnIqykecuEr>();
	}

	[MonoPInvokeCallback(typeof(ebNlRzsqTxWcPAQlcXwOiVEuciow))]
	private unsafe static int UWxXOePewRkiVywJmUPBnEuJMtvm(void* P_0, IntPtr P_1)
	{
		uint instanceId = (uint)P_1.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<YDUBCRqwmtZySxoSSmbDvJJshDRt>(instanceId, out var instance))
		{
			return 1;
		}
		gSsnEkKQgoAoVCLJetnIqykecuEr gSsnEkKQgoAoVCLJetnIqykecuEr2 = new gSsnEkKQgoAoVCLJetnIqykecuEr();
		gSsnEkKQgoAoVCLJetnIqykecuEr2.aLBrdGWxOITKFkixRhYnVqCkTJFk(ref *(gSsnEkKQgoAoVCLJetnIqykecuEr.DnzDjfJJYXxZDKAjBQMXWVKilKAO*)P_0);
		instance.APBGWwAbDVNwYlcfhYtGSjEetvtz.Add(gSsnEkKQgoAoVCLJetnIqykecuEr2);
		return 1;
	}
}

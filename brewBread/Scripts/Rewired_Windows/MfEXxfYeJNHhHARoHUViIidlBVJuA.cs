using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class MfEXxfYeJNHhHARoHUViIidlBVJuA
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private unsafe delegate int LaAigwEQRnpHWamaMTqRBTTdHxIU(void* deviceInstance, IntPtr data);

	private readonly IntPtr CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	private readonly LaAigwEQRnpHWamaMTqRBTTdHxIU SiXixQSMucyuJvNwhknxJlowTWAA;

	[CompilerGenerated]
	private List<JLeeJYfOZUPzNCmqmMheHzTHQFhoB> pKJblDACXDrXJGIQUlzCefcuOfbGb;

	public IntPtr KUJrOBoduRcGKIkxgMoKicmgtCeb => CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	public List<JLeeJYfOZUPzNCmqmMheHzTHQFhoB> cWfGvigaigCtCPRCAUmxGghiBMhc
	{
		[CompilerGenerated]
		get
		{
			return pKJblDACXDrXJGIQUlzCefcuOfbGb;
		}
		[CompilerGenerated]
		private set
		{
			pKJblDACXDrXJGIQUlzCefcuOfbGb = list;
		}
	}

	public unsafe MfEXxfYeJNHhHARoHUViIidlBVJuA()
	{
		SiXixQSMucyuJvNwhknxJlowTWAA = OEnBmrHzcoIoslFxbzrYuoStVgpY;
		CwTGIOKdRnDIcHksMHqFRvtSvOMs = Marshal.GetFunctionPointerForDelegate((Delegate)SiXixQSMucyuJvNwhknxJlowTWAA);
		cWfGvigaigCtCPRCAUmxGghiBMhc = new List<JLeeJYfOZUPzNCmqmMheHzTHQFhoB>();
	}

	[MonoPInvokeCallback(typeof(LaAigwEQRnpHWamaMTqRBTTdHxIU))]
	private unsafe static int OEnBmrHzcoIoslFxbzrYuoStVgpY(void* P_0, IntPtr P_1)
	{
		uint instanceId = (uint)P_1.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<MfEXxfYeJNHhHARoHUViIidlBVJuA>(instanceId, out var instance))
		{
			return 1;
		}
		JLeeJYfOZUPzNCmqmMheHzTHQFhoB item = new JLeeJYfOZUPzNCmqmMheHzTHQFhoB((IntPtr)P_0);
		instance.cWfGvigaigCtCPRCAUmxGghiBMhc.Add(item);
		return 1;
	}
}

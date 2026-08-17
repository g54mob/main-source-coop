using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class VxfOBOJpVYpSrapFdThjtQVDqCxG
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private unsafe delegate int evMEvLIjpJLRTLIFljKUwYRCtbxM(void* deviceInstance, IntPtr data);

	private readonly IntPtr CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	private readonly evMEvLIjpJLRTLIFljKUwYRCtbxM SiXixQSMucyuJvNwhknxJlowTWAA;

	[CompilerGenerated]
	private List<NWgGzQDOZYBvNGVNEGBMOyyPyPYIA> WWCWWMhZvWCovghcVRVKIRzYMmCO;

	public IntPtr KUJrOBoduRcGKIkxgMoKicmgtCeb => CwTGIOKdRnDIcHksMHqFRvtSvOMs;

	public List<NWgGzQDOZYBvNGVNEGBMOyyPyPYIA> xwSoAdvrqbvpbYVSwXBftRUFVIkA
	{
		[CompilerGenerated]
		get
		{
			return WWCWWMhZvWCovghcVRVKIRzYMmCO;
		}
		[CompilerGenerated]
		private set
		{
			WWCWWMhZvWCovghcVRVKIRzYMmCO = wWCWWMhZvWCovghcVRVKIRzYMmCO;
		}
	}

	public unsafe VxfOBOJpVYpSrapFdThjtQVDqCxG()
	{
		SiXixQSMucyuJvNwhknxJlowTWAA = ZnoNrXqbwfnusSYqkItZapyBgELDA;
		CwTGIOKdRnDIcHksMHqFRvtSvOMs = Marshal.GetFunctionPointerForDelegate((Delegate)SiXixQSMucyuJvNwhknxJlowTWAA);
		xwSoAdvrqbvpbYVSwXBftRUFVIkA = new List<NWgGzQDOZYBvNGVNEGBMOyyPyPYIA>();
	}

	[MonoPInvokeCallback(typeof(evMEvLIjpJLRTLIFljKUwYRCtbxM))]
	private unsafe static int ZnoNrXqbwfnusSYqkItZapyBgELDA(void* P_0, IntPtr P_1)
	{
		uint instanceId = (uint)P_1.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<VxfOBOJpVYpSrapFdThjtQVDqCxG>(instanceId, out var instance))
		{
			return 1;
		}
		NWgGzQDOZYBvNGVNEGBMOyyPyPYIA nWgGzQDOZYBvNGVNEGBMOyyPyPYIA = new NWgGzQDOZYBvNGVNEGBMOyyPyPYIA();
		nWgGzQDOZYBvNGVNEGBMOyyPyPYIA.aLBrdGWxOITKFkixRhYnVqCkTJFk(ref *(NWgGzQDOZYBvNGVNEGBMOyyPyPYIA.jwoIHZIFimUefOKyYeclTeEzYcsC*)P_0);
		instance.xwSoAdvrqbvpbYVSwXBftRUFVIkA.Add(nWgGzQDOZYBvNGVNEGBMOyyPyPYIA);
		return 1;
	}
}

using System;
using System.Runtime.CompilerServices;

internal struct wXdOkBsjtVnNnIDvwJcZyTyveGyS : IEquatable<wXdOkBsjtVnNnIDvwJcZyTyveGyS>
{
	public IntPtr HfnJHIiwZKhCLCscEMXBlPUMboob;

	public bool MpSAYmASfEwDrIJnSTQGGdFgjiumA => HfnJHIiwZKhCLCscEMXBlPUMboob != IntPtr.Zero;

	public wXdOkBsjtVnNnIDvwJcZyTyveGyS(JbFAywBYXfGhagDypdlSSUPUqzcGA P_0)
	{
		HfnJHIiwZKhCLCscEMXBlPUMboob = P_0.bnfQTvbjPapCDVeoxjkSbGmDNXmBb;
	}

	public void cKMbqpfkQtxvCcpAfXtCcXQJQnOH()
	{
		if (!(HfnJHIiwZKhCLCscEMXBlPUMboob == IntPtr.Zero))
		{
			yvgWAgmxKiqEtmrAqGIhPEEXIuLEA.mfgBjUHYfRxnwgUAlqsDkdOKsnCxA(HfnJHIiwZKhCLCscEMXBlPUMboob);
			HfnJHIiwZKhCLCscEMXBlPUMboob = IntPtr.Zero;
		}
	}

	[SpecialName]
	public static IntPtr BNBvRTIgKyAGJlbMfdwvVQgsvNTs(wXdOkBsjtVnNnIDvwJcZyTyveGyS P_0)
	{
		return P_0.HfnJHIiwZKhCLCscEMXBlPUMboob;
	}

	public bool JGNrBMLWhlCKBSHzFDWdcvtrjXQC(object P_0)
	{
		if (!(P_0 is wXdOkBsjtVnNnIDvwJcZyTyveGyS))
		{
			return false;
		}
		return ((wXdOkBsjtVnNnIDvwJcZyTyveGyS)P_0).HfnJHIiwZKhCLCscEMXBlPUMboob == HfnJHIiwZKhCLCscEMXBlPUMboob;
	}

	public int xpgYifypdAfQrJHpaXhHsRNWKIDRA()
	{
		return GetHashCode();
	}

	public bool Equals(wXdOkBsjtVnNnIDvwJcZyTyveGyS other)
	{
		return HfnJHIiwZKhCLCscEMXBlPUMboob == other.HfnJHIiwZKhCLCscEMXBlPUMboob;
	}

	bool IEquatable<wXdOkBsjtVnNnIDvwJcZyTyveGyS>.Equals(wXdOkBsjtVnNnIDvwJcZyTyveGyS other)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Equals
		return this.Equals(other);
	}

	[SpecialName]
	public static bool FDyCOpATivCpoFGtDEUeESIszmqRA(wXdOkBsjtVnNnIDvwJcZyTyveGyS P_0, wXdOkBsjtVnNnIDvwJcZyTyveGyS P_1)
	{
		return P_0.Equals(P_1);
	}
}

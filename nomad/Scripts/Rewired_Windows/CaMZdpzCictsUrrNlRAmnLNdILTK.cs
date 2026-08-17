using System;
using System.Runtime.InteropServices;

internal class CaMZdpzCictsUrrNlRAmnLNdILTK : IDisposable
{
	private int PntyRfPWelcQndoYOJaqkMYpcdgcA;

	private uint FISykvvILhJrXUFeuyjKEVcoYww;

	private IntPtr wYMnOZuxOLyzFBRpjKHNeDCEEozR;

	private bool RSDvzuSylHeRxyIFwQIOWkUfdBnHA;

	public CaMZdpzCictsUrrNlRAmnLNdILTK(uint P_0)
	{
		if (P_0 == 0)
		{
			throw new Exception("size must be > 0!");
		}
		FISykvvILhJrXUFeuyjKEVcoYww = P_0;
		PntyRfPWelcQndoYOJaqkMYpcdgcA = 0;
		try
		{
			wYMnOZuxOLyzFBRpjKHNeDCEEozR = Marshal.AllocHGlobal((int)P_0);
			if (wYMnOZuxOLyzFBRpjKHNeDCEEozR == IntPtr.Zero)
			{
				throw new Exception("Could not allocate native memory.");
			}
		}
		catch
		{
			throw;
		}
	}

	public unsafe IntPtr NhMDNCdjtGMaHLOWljAsNMMGedgdb(uint P_0, void* P_1)
	{
		if (RSDvzuSylHeRxyIFwQIOWkUfdBnHA)
		{
			return IntPtr.Zero;
		}
		if (P_0 == 0)
		{
			return IntPtr.Zero;
		}
		if (P_0 > FISykvvILhJrXUFeuyjKEVcoYww)
		{
			return IntPtr.Zero;
		}
		if (PntyRfPWelcQndoYOJaqkMYpcdgcA + P_0 >= FISykvvILhJrXUFeuyjKEVcoYww)
		{
			PntyRfPWelcQndoYOJaqkMYpcdgcA = 0;
		}
		IntPtr intPtr = new IntPtr(wYMnOZuxOLyzFBRpjKHNeDCEEozR.ToInt64() + PntyRfPWelcQndoYOJaqkMYpcdgcA);
		luYaFPaftNInTWGPWfCvgYuDUqDyA.JJGgrpAvfYRUJflNOROgzAByeTGgb(intPtr, (IntPtr)P_1, (int)P_0);
		PntyRfPWelcQndoYOJaqkMYpcdgcA += (int)P_0;
		return intPtr;
	}

	public void Dispose()
	{
		wsUAnxzyQNPVikwyfuIamZGIRKpJ(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void lAUezvmsXfExRHrNeraUKjhMigXv()
	{
		try
		{
			wsUAnxzyQNPVikwyfuIamZGIRKpJ(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void wsUAnxzyQNPVikwyfuIamZGIRKpJ(bool P_0)
	{
		if (!RSDvzuSylHeRxyIFwQIOWkUfdBnHA)
		{
			RSDvzuSylHeRxyIFwQIOWkUfdBnHA = true;
			if (wYMnOZuxOLyzFBRpjKHNeDCEEozR != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(wYMnOZuxOLyzFBRpjKHNeDCEEozR);
			}
		}
	}
}

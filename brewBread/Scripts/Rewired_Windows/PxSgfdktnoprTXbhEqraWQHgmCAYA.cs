using System;
using System.Runtime.InteropServices;

internal class PxSgfdktnoprTXbhEqraWQHgmCAYA : IDisposable
{
	private int IDALqUNXoHNfCVTPfSVHVrDzIYak;

	private uint ZHEQuImTVvNEkKAXcGUIvuGvaloA;

	private IntPtr sGSfvUhTydyIHVCWHlZPDtobHMXR;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public PxSgfdktnoprTXbhEqraWQHgmCAYA(uint P_0)
	{
		if (P_0 == 0)
		{
			throw new Exception("size must be > 0!");
		}
		ZHEQuImTVvNEkKAXcGUIvuGvaloA = P_0;
		IDALqUNXoHNfCVTPfSVHVrDzIYak = 0;
		try
		{
			sGSfvUhTydyIHVCWHlZPDtobHMXR = Marshal.AllocHGlobal((int)P_0);
			if (sGSfvUhTydyIHVCWHlZPDtobHMXR == IntPtr.Zero)
			{
				throw new Exception("Could not allocate native memory.");
			}
		}
		catch
		{
			throw;
		}
	}

	public unsafe IntPtr VKWzxEEjqafsNSMawblfEqtHdvmX(uint P_0, void* P_1)
	{
		if (NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			return IntPtr.Zero;
		}
		if (P_0 == 0)
		{
			return IntPtr.Zero;
		}
		if (P_0 > ZHEQuImTVvNEkKAXcGUIvuGvaloA)
		{
			return IntPtr.Zero;
		}
		if (IDALqUNXoHNfCVTPfSVHVrDzIYak + P_0 >= ZHEQuImTVvNEkKAXcGUIvuGvaloA)
		{
			IDALqUNXoHNfCVTPfSVHVrDzIYak = 0;
		}
		IntPtr intPtr = new IntPtr(sGSfvUhTydyIHVCWHlZPDtobHMXR.ToInt64() + IDALqUNXoHNfCVTPfSVHVrDzIYak);
		aOYtALpBiXtNUYUxrmqpvmqCyvKG.RnAWqRWOrAiuOkNLYFPejoszYblU(intPtr, (IntPtr)P_1, (int)P_0);
		IDALqUNXoHNfCVTPfSVHVrDzIYak += (int)P_0;
		return intPtr;
	}

	public void Dispose()
	{
		lDxnsjCDTQrmresvWgbliNUVruIc(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
	{
		try
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
			if (sGSfvUhTydyIHVCWHlZPDtobHMXR != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(sGSfvUhTydyIHVCWHlZPDtobHMXR);
			}
		}
	}
}

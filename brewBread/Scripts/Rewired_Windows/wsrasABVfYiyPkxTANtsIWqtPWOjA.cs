using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;

internal class wsrasABVfYiyPkxTANtsIWqtPWOjA : IDisposable
{
	private class oRXVwtutBlnsKGJLhLtorAhNaSQp
	{
		public int HFkteAPyFDALCiIfxYSiOcULVTDb;

		public int SHtQBbZniKBdCJZwlwvZKEBlYPmZ;

		public uint JopWpDjwAeJmLiUpEedjkFAPxMVW;

		public object zNGFWAFHtWlodUbeSYlSEhNLErIvA;

		public void RPzvZUnzFHprsOVaYJSDSYKBntfq(int P_0, int P_1, uint P_2, object P_3)
		{
			HFkteAPyFDALCiIfxYSiOcULVTDb = P_0;
			SHtQBbZniKBdCJZwlwvZKEBlYPmZ = P_1;
			JopWpDjwAeJmLiUpEedjkFAPxMVW = P_2;
			zNGFWAFHtWlodUbeSYlSEhNLErIvA = P_3;
		}

		public void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
		{
			zNGFWAFHtWlodUbeSYlSEhNLErIvA = null;
		}
	}

	[Serializable]
	private sealed class YsyMNXxgaqtgxhcZkUWFPuhDYEqD
	{
		public static readonly YsyMNXxgaqtgxhcZkUWFPuhDYEqD _003C_003E9 = new YsyMNXxgaqtgxhcZkUWFPuhDYEqD();

		public static Func<oRXVwtutBlnsKGJLhLtorAhNaSQp> _003C_003E9__6_0;

		public static Action<oRXVwtutBlnsKGJLhLtorAhNaSQp> _003C_003E9__6_1;

		internal oRXVwtutBlnsKGJLhLtorAhNaSQp vQflELWkVYCGmHYoxZjqPSKWuxnqA()
		{
			return new oRXVwtutBlnsKGJLhLtorAhNaSQp();
		}

		internal void ABcbVegDCRlENiwHnXAGXKlhpZVyA(oRXVwtutBlnsKGJLhLtorAhNaSQp P_0)
		{
			P_0.ZrbFhGEbWRbTzVxxQimUntnkwisKA();
		}
	}

	private btTtcDDuGnqMmSyRYKBhUvGLqQwD hqjaEUASFjOXvvoGsKkZpjKAdZUFb;

	private ObjectPool<oRXVwtutBlnsKGJLhLtorAhNaSQp> EbGSFWAVRNbCyIbhAVSyLJGtjxSE;

	private Queue<oRXVwtutBlnsKGJLhLtorAhNaSQp> fWyJSVKuvzkInYpsmxyLNHBfPYRG;

	private Action<object> yqlkRKpZrQRjfKGxmHzXXlerihWn;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public bool SCokzqMHKzjgvkBSMKcbDNPquQxF => GKYQqimLFcIjOlrdwwxrIXVlAuhO();

	public wsrasABVfYiyPkxTANtsIWqtPWOjA(int P_0, int P_1, Action<object> P_2 = null)
	{
		if (P_0 <= 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb = new btTtcDDuGnqMmSyRYKBhUvGLqQwD(P_0);
		EbGSFWAVRNbCyIbhAVSyLJGtjxSE = new ObjectPool<oRXVwtutBlnsKGJLhLtorAhNaSQp>(P_1, YsyMNXxgaqtgxhcZkUWFPuhDYEqD._003C_003E9.vQflELWkVYCGmHYoxZjqPSKWuxnqA, YsyMNXxgaqtgxhcZkUWFPuhDYEqD._003C_003E9.ABcbVegDCRlENiwHnXAGXKlhpZVyA);
		fWyJSVKuvzkInYpsmxyLNHBfPYRG = new Queue<oRXVwtutBlnsKGJLhLtorAhNaSQp>(P_1);
		yqlkRKpZrQRjfKGxmHzXXlerihWn = P_2;
	}

	public unsafe bool uRxHPttoThKrBNCurCuvwPhMcGUfA(byte* P_0, int P_1, object P_2)
	{
		if (P_0 == null || P_1 <= 0)
		{
			return false;
		}
		if (hqjaEUASFjOXvvoGsKkZpjKAdZUFb.KjttXAEzsRtECEzfsLykXNbscFEq(P_0, P_1, P_1, out var num, out var num2) < P_1)
		{
			return false;
		}
		oRXVwtutBlnsKGJLhLtorAhNaSQp oRXVwtutBlnsKGJLhLtorAhNaSQp2 = EbGSFWAVRNbCyIbhAVSyLJGtjxSE.Get();
		oRXVwtutBlnsKGJLhLtorAhNaSQp2.RPzvZUnzFHprsOVaYJSDSYKBntfq(num, P_1, num2, P_2);
		fWyJSVKuvzkInYpsmxyLNHBfPYRG.Enqueue(oRXVwtutBlnsKGJLhLtorAhNaSQp2);
		return true;
	}

	public unsafe bool uRxHPttoThKrBNCurCuvwPhMcGUfA(byte* P_0, int P_1)
	{
		return uRxHPttoThKrBNCurCuvwPhMcGUfA(P_0, P_1, null);
	}

	public unsafe bool uRxHPttoThKrBNCurCuvwPhMcGUfA(IntPtr P_0, int P_1, object P_2)
	{
		if (P_0 == IntPtr.Zero || P_1 <= 0)
		{
			return false;
		}
		return uRxHPttoThKrBNCurCuvwPhMcGUfA((byte*)(void*)P_0, P_1, P_2);
	}

	public bool uRxHPttoThKrBNCurCuvwPhMcGUfA(IntPtr P_0, int P_1)
	{
		return uRxHPttoThKrBNCurCuvwPhMcGUfA(P_0, P_1, null);
	}

	public unsafe bool uRxHPttoThKrBNCurCuvwPhMcGUfA(byte[] P_0, int P_1, object P_2, int P_3 = 0)
	{
		if (P_0 == null || P_1 > P_0.Length)
		{
			return false;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_3 + P_1 > P_0.Length)
		{
			return false;
		}
		fixed (byte* ptr = P_0)
		{
			byte* ptr2 = ptr + P_3;
			return uRxHPttoThKrBNCurCuvwPhMcGUfA(ptr2, P_1, P_2);
		}
	}

	public bool uRxHPttoThKrBNCurCuvwPhMcGUfA(byte[] P_0, int P_1, int P_2 = 0)
	{
		return uRxHPttoThKrBNCurCuvwPhMcGUfA(P_0, P_1, null, P_2);
	}

	public unsafe int ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(byte* P_0, int P_1, out object P_2)
	{
		if (P_0 == null || P_1 <= 0)
		{
			P_2 = null;
			return -1;
		}
		oRXVwtutBlnsKGJLhLtorAhNaSQp oRXVwtutBlnsKGJLhLtorAhNaSQp2 = vSVHRjpmXuMHQxPZvqalaGzvQgmi(false);
		if (oRXVwtutBlnsKGJLhLtorAhNaSQp2 == null)
		{
			P_2 = null;
			return -1;
		}
		if (P_1 < oRXVwtutBlnsKGJLhLtorAhNaSQp2.SHtQBbZniKBdCJZwlwvZKEBlYPmZ)
		{
			Logger.LogError("The buffer is too small to hold the data. Call PeekDataLength before calling Peek to get the data length.", requiredThreadSafety: true);
			P_2 = null;
			return -1;
		}
		int num = hqjaEUASFjOXvvoGsKkZpjKAdZUFb.CLZgDahJjuyUxSWbGURZdNRDDiCOA(P_0, P_1, oRXVwtutBlnsKGJLhLtorAhNaSQp2.SHtQBbZniKBdCJZwlwvZKEBlYPmZ, oRXVwtutBlnsKGJLhLtorAhNaSQp2.HFkteAPyFDALCiIfxYSiOcULVTDb);
		if (num != oRXVwtutBlnsKGJLhLtorAhNaSQp2.SHtQBbZniKBdCJZwlwvZKEBlYPmZ)
		{
			Logger.LogError("Failure reading data from buffer!", requiredThreadSafety: true);
			num = 0;
			P_2 = null;
			return -1;
		}
		P_2 = oRXVwtutBlnsKGJLhLtorAhNaSQp2.zNGFWAFHtWlodUbeSYlSEhNLErIvA;
		return num;
	}

	public unsafe int ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(byte* P_0, int P_1)
	{
		object obj;
		return ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(P_0, P_1, out obj);
	}

	public unsafe int ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(IntPtr P_0, int P_1, out object P_2)
	{
		if (P_0 == IntPtr.Zero || P_1 <= 0)
		{
			P_2 = null;
			return -1;
		}
		return ZaXBZbvfWdsSaHHYgMkyAUUJkFaH((byte*)(void*)P_0, P_1, out P_2);
	}

	public int ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(IntPtr P_0, int P_1)
	{
		object obj;
		return ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(P_0, P_1, out obj);
	}

	public unsafe int ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(byte[] P_0, out object P_1)
	{
		if (P_0 == null || P_0.Length == 0)
		{
			P_1 = null;
			return -1;
		}
		fixed (byte* ptr = P_0)
		{
			return ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(ptr, P_0.Length, out P_1);
		}
	}

	public int ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(byte[] P_0)
	{
		object obj;
		return ZaXBZbvfWdsSaHHYgMkyAUUJkFaH(P_0, out obj);
	}

	public int yCDubrmHOEhzNVgzaNCojLpHGzNz()
	{
		return vSVHRjpmXuMHQxPZvqalaGzvQgmi(false)?.SHtQBbZniKBdCJZwlwvZKEBlYPmZ ?? (-1);
	}

	public unsafe int vsodGafOhgfHKJXnBRDyghIUQAxy(byte* P_0, int P_1, out object P_2)
	{
		if (P_0 == null || P_1 <= 0)
		{
			P_2 = null;
			return -1;
		}
		oRXVwtutBlnsKGJLhLtorAhNaSQp oRXVwtutBlnsKGJLhLtorAhNaSQp2 = vSVHRjpmXuMHQxPZvqalaGzvQgmi(true);
		if (oRXVwtutBlnsKGJLhLtorAhNaSQp2 == null)
		{
			P_2 = null;
			return -1;
		}
		if (P_1 < oRXVwtutBlnsKGJLhLtorAhNaSQp2.SHtQBbZniKBdCJZwlwvZKEBlYPmZ)
		{
			Logger.LogError("The buffer is too small to hold the data. Call PeekDataLength before calling Dequeue to get the data length.", requiredThreadSafety: true);
			P_2 = null;
			IynxniseHqQEalciGPczWjJvIBts(oRXVwtutBlnsKGJLhLtorAhNaSQp2, true);
			return -1;
		}
		int num = hqjaEUASFjOXvvoGsKkZpjKAdZUFb.CLZgDahJjuyUxSWbGURZdNRDDiCOA(P_0, P_1, oRXVwtutBlnsKGJLhLtorAhNaSQp2.SHtQBbZniKBdCJZwlwvZKEBlYPmZ, oRXVwtutBlnsKGJLhLtorAhNaSQp2.HFkteAPyFDALCiIfxYSiOcULVTDb);
		if (num != oRXVwtutBlnsKGJLhLtorAhNaSQp2.SHtQBbZniKBdCJZwlwvZKEBlYPmZ)
		{
			Logger.LogError("Failure reading data from buffer!", requiredThreadSafety: true);
			P_2 = null;
			IynxniseHqQEalciGPczWjJvIBts(oRXVwtutBlnsKGJLhLtorAhNaSQp2, true);
			return -1;
		}
		P_2 = oRXVwtutBlnsKGJLhLtorAhNaSQp2.zNGFWAFHtWlodUbeSYlSEhNLErIvA;
		IynxniseHqQEalciGPczWjJvIBts(oRXVwtutBlnsKGJLhLtorAhNaSQp2, false);
		return num;
	}

	public unsafe int vsodGafOhgfHKJXnBRDyghIUQAxy(byte* P_0, int P_1)
	{
		object obj;
		return vsodGafOhgfHKJXnBRDyghIUQAxy(P_0, P_1, out obj);
	}

	public unsafe int vsodGafOhgfHKJXnBRDyghIUQAxy(IntPtr P_0, int P_1, out object P_2)
	{
		if (P_0 == IntPtr.Zero || P_1 <= 0)
		{
			P_2 = null;
			return -1;
		}
		return vsodGafOhgfHKJXnBRDyghIUQAxy((byte*)(void*)P_0, P_1, out P_2);
	}

	public int vsodGafOhgfHKJXnBRDyghIUQAxy(IntPtr P_0, int P_1)
	{
		object obj;
		return vsodGafOhgfHKJXnBRDyghIUQAxy(P_0, P_1, out obj);
	}

	public unsafe int vsodGafOhgfHKJXnBRDyghIUQAxy(byte[] P_0, out object P_1)
	{
		if (P_0 == null || P_0.Length == 0)
		{
			P_1 = null;
			return -1;
		}
		fixed (byte* ptr = P_0)
		{
			return vsodGafOhgfHKJXnBRDyghIUQAxy(ptr, P_0.Length, out P_1);
		}
	}

	public int vsodGafOhgfHKJXnBRDyghIUQAxy(byte[] P_0)
	{
		object obj;
		return vsodGafOhgfHKJXnBRDyghIUQAxy(P_0, out obj);
	}

	public void qqDwOefaTTLSnfmMBkBUVENbaKtkA()
	{
		hqjaEUASFjOXvvoGsKkZpjKAdZUFb.qqDwOefaTTLSnfmMBkBUVENbaKtkA();
		while (fWyJSVKuvzkInYpsmxyLNHBfPYRG.Count > 0)
		{
			IynxniseHqQEalciGPczWjJvIBts(fWyJSVKuvzkInYpsmxyLNHBfPYRG.Dequeue(), true);
		}
	}

	private oRXVwtutBlnsKGJLhLtorAhNaSQp vSVHRjpmXuMHQxPZvqalaGzvQgmi(bool P_0)
	{
		while (fWyJSVKuvzkInYpsmxyLNHBfPYRG.Count > 0)
		{
			oRXVwtutBlnsKGJLhLtorAhNaSQp oRXVwtutBlnsKGJLhLtorAhNaSQp2 = (P_0 ? fWyJSVKuvzkInYpsmxyLNHBfPYRG.Dequeue() : fWyJSVKuvzkInYpsmxyLNHBfPYRG.Peek());
			if (hqjaEUASFjOXvvoGsKkZpjKAdZUFb.JSayXFTNziEjxHctRbOJQBhdcOwp(oRXVwtutBlnsKGJLhLtorAhNaSQp2.HFkteAPyFDALCiIfxYSiOcULVTDb, oRXVwtutBlnsKGJLhLtorAhNaSQp2.JopWpDjwAeJmLiUpEedjkFAPxMVW))
			{
				return oRXVwtutBlnsKGJLhLtorAhNaSQp2;
			}
			if (!P_0)
			{
				oRXVwtutBlnsKGJLhLtorAhNaSQp2 = fWyJSVKuvzkInYpsmxyLNHBfPYRG.Dequeue();
			}
			IynxniseHqQEalciGPczWjJvIBts(oRXVwtutBlnsKGJLhLtorAhNaSQp2, true);
		}
		return null;
	}

	private bool GKYQqimLFcIjOlrdwwxrIXVlAuhO()
	{
		return vSVHRjpmXuMHQxPZvqalaGzvQgmi(false) != null;
	}

	private void IynxniseHqQEalciGPczWjJvIBts(oRXVwtutBlnsKGJLhLtorAhNaSQp P_0, bool P_1)
	{
		if (P_0 != null)
		{
			if (P_1 && yqlkRKpZrQRjfKGxmHzXXlerihWn != null && P_0.zNGFWAFHtWlodUbeSYlSEhNLErIvA != null)
			{
				yqlkRKpZrQRjfKGxmHzXXlerihWn(P_0.zNGFWAFHtWlodUbeSYlSEhNLErIvA);
			}
			EbGSFWAVRNbCyIbhAVSyLJGtjxSE.Return(P_0);
		}
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

	protected void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		if (NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			return;
		}
		if (P_0)
		{
			qqDwOefaTTLSnfmMBkBUVENbaKtkA();
			if (hqjaEUASFjOXvvoGsKkZpjKAdZUFb != null)
			{
				hqjaEUASFjOXvvoGsKkZpjKAdZUFb.Dispose();
			}
		}
		NchdYNbKzqsssgcQJdenZuGqXgLo = true;
	}

	public static bool SJoHSdCLYmqoPYbgXGOEtWnmAcXk(wsrasABVfYiyPkxTANtsIWqtPWOjA P_0, wsrasABVfYiyPkxTANtsIWqtPWOjA P_1)
	{
		if (P_0 == null || P_1 == null)
		{
			return false;
		}
		MiscTools.Swap(ref P_0.hqjaEUASFjOXvvoGsKkZpjKAdZUFb, ref P_1.hqjaEUASFjOXvvoGsKkZpjKAdZUFb);
		MiscTools.Swap(ref P_0.EbGSFWAVRNbCyIbhAVSyLJGtjxSE, ref P_1.EbGSFWAVRNbCyIbhAVSyLJGtjxSE);
		MiscTools.Swap(ref P_0.fWyJSVKuvzkInYpsmxyLNHBfPYRG, ref P_1.fWyJSVKuvzkInYpsmxyLNHBfPYRG);
		return true;
	}
}

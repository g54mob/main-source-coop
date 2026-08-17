using System;
using System.Globalization;
using System.Runtime.CompilerServices;

internal class ZTlmTFdGVtPQJcuAvEckuKGdUIxv : UTTJNIeEjOgUBSkCqZFvGRXYKUTD<jWgoJXIfqyFgXngIdyGNkdKjrzUh, yyDfEgaXSFZxqadQsPjjnIHPgDIDb>
{
	[CompilerGenerated]
	private int jDxJirybpSJQyuZkHxRayVTUXODI;

	[CompilerGenerated]
	private int uwpBSAJSjyfJzrSIEyQqogzKlWSW;

	[CompilerGenerated]
	private int AIsunFrrOcHEudBfAVbNAwJALPKoA;

	[CompilerGenerated]
	private bool[] mpayENGhbGbrALzMhaCOgrhzgCFe;

	public int LeaAlLlLUVpUZEtsNQKSfcVaXRbL
	{
		[CompilerGenerated]
		get
		{
			return jDxJirybpSJQyuZkHxRayVTUXODI;
		}
		[CompilerGenerated]
		set
		{
			jDxJirybpSJQyuZkHxRayVTUXODI = num;
		}
	}

	public int nYgMBIIMubbuyaBAFyuFsqYaYOAtA
	{
		[CompilerGenerated]
		get
		{
			return uwpBSAJSjyfJzrSIEyQqogzKlWSW;
		}
		[CompilerGenerated]
		set
		{
			uwpBSAJSjyfJzrSIEyQqogzKlWSW = num;
		}
	}

	public int dOuRBCKDBhffYSgvmkclUElpgLap
	{
		[CompilerGenerated]
		get
		{
			return AIsunFrrOcHEudBfAVbNAwJALPKoA;
		}
		[CompilerGenerated]
		set
		{
			AIsunFrrOcHEudBfAVbNAwJALPKoA = aIsunFrrOcHEudBfAVbNAwJALPKoA;
		}
	}

	public bool[] kUBzXDgUpYEZoaPrTtuPPIDjpHZM
	{
		[CompilerGenerated]
		get
		{
			return mpayENGhbGbrALzMhaCOgrhzgCFe;
		}
		[CompilerGenerated]
		private set
		{
			mpayENGhbGbrALzMhaCOgrhzgCFe = array;
		}
	}

	public ZTlmTFdGVtPQJcuAvEckuKGdUIxv()
	{
		kUBzXDgUpYEZoaPrTtuPPIDjpHZM = new bool[8];
	}

	public void Update(yyDfEgaXSFZxqadQsPjjnIHPgDIDb P_0)
	{
		int num = P_0.HdAEtvcarTxFDNDjGWFvAEZcqhab;
		switch (P_0.JqfldkFQUDyRggajChRQUVWxIAKu)
		{
		case ulBRLERfMSXfwcjdMgqohxBYMFbmA.X:
			LeaAlLlLUVpUZEtsNQKSfcVaXRbL = num;
			return;
		case ulBRLERfMSXfwcjdMgqohxBYMFbmA.Y:
			nYgMBIIMubbuyaBAFyuFsqYaYOAtA = num;
			return;
		case ulBRLERfMSXfwcjdMgqohxBYMFbmA.Z:
			dOuRBCKDBhffYSgvmkclUElpgLap = num;
			return;
		}
		int num2 = (int)(P_0.JqfldkFQUDyRggajChRQUVWxIAKu - 12);
		if (num2 >= 0 && num2 < 8)
		{
			kUBzXDgUpYEZoaPrTtuPPIDjpHZM[num2] = (num & 0x80) != 0;
		}
	}

	public unsafe void MarshalFrom(IntPtr P_0)
	{
		jWgoJXIfqyFgXngIdyGNkdKjrzUh* ptr = (jWgoJXIfqyFgXngIdyGNkdKjrzUh*)(void*)P_0;
		LeaAlLlLUVpUZEtsNQKSfcVaXRbL = ptr->LeaAlLlLUVpUZEtsNQKSfcVaXRbL;
		nYgMBIIMubbuyaBAFyuFsqYaYOAtA = ptr->nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		dOuRBCKDBhffYSgvmkclUElpgLap = ptr->dOuRBCKDBhffYSgvmkclUElpgLap;
		void* ptr2 = &ptr->FMjlrUBuwJWUWAgOkGrumeFsKdly;
		fixed (bool* ptr3 = kUBzXDgUpYEZoaPrTtuPPIDjpHZM)
		{
			for (int i = 0; i < 8; i++)
			{
				ptr3[i] = (((byte*)ptr2)[i] & 0x80) != 0;
			}
		}
	}

	public virtual string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return string.Format(CultureInfo.InvariantCulture, "X: {0}, Y: {1}, Z: {2}, Buttons: {3}", LeaAlLlLUVpUZEtsNQKSfcVaXRbL, nYgMBIIMubbuyaBAFyuFsqYaYOAtA, dOuRBCKDBhffYSgvmkclUElpgLap, aOYtALpBiXtNUYUxrmqpvmqCyvKG.beeAFlkTibRtDifoecnQBvpZYYuV(";", kUBzXDgUpYEZoaPrTtuPPIDjpHZM));
	}
}

using System;
using Rewired;
using Rewired.Utils;

internal class XGGQMyZVNBKhRaGnErwNnBmbPgkE : MBHpKnojrUByXgJMzvimCmweJpsGb, MnsFyTzecaNZBeNkLkANtMeQRWvs, yKitnLgjKaXKBXtOvsdBEcqljnIG, IDisposable
{
	public readonly int GaEBKEGSkzRgcHxiSrCPspaILNewA;

	public readonly int okzbdkhxOafeBloAXVXQjlihbdDdb;

	public readonly int zZckucWccGYSrtWZwqSFhDcpcAtiA;

	public readonly int tzfbRUaGGtJXAysVzLeuFTNyUqDPA;

	public readonly short[] NzAGkdPRfArYMUGJSuYwkbaOaVbx;

	private readonly ButtonLoopSet bpJHgiLXyeYuTuzIPuYucAwHrgjL;

	public readonly short[] jsKamTQfyGHvElidohatJFRjLfTWA;

	public readonly short[] ZXZSBGgcitQwnqckgtZXaZvgNCtJ;

	private bool rAgNoUjVPddhslpmbXOMWTyxQoTT;

	int MnsFyTzecaNZBeNkLkANtMeQRWvs.YjNcuscoAVusSfOTOVbvKOQOOXDKA => IfpVnuiAcuCRPvvZrdwkbEinzjysA;

	int MnsFyTzecaNZBeNkLkANtMeQRWvs.WqPKjVPPOTgPceAajhEZiaJOOUZD => GaEBKEGSkzRgcHxiSrCPspaILNewA;

	int MnsFyTzecaNZBeNkLkANtMeQRWvs.eymxebVmptUKyMqnkTUMLZAtIPtV => okzbdkhxOafeBloAXVXQjlihbdDdb;

	int MnsFyTzecaNZBeNkLkANtMeQRWvs.lazczcOAAnApOBVJoqdKAeedhnzpB => zZckucWccGYSrtWZwqSFhDcpcAtiA;

	InputSource MnsFyTzecaNZBeNkLkANtMeQRWvs.WdBQKlEddYOWiqSsIGESbfcFjmtU => InputSource.SDL2;

	bool MnsFyTzecaNZBeNkLkANtMeQRWvs.nKeUqEcMZvFbbNysLlzgzgQizezN => rAgNoUjVPddhslpmbXOMWTyxQoTT;

	public XGGQMyZVNBKhRaGnErwNnBmbPgkE(OfHcchtNNgrqQpaRylQnzBsxMNFD P_0, ettUIVyZApbfOsMrELHoexHuUXcN P_1)
		: this(P_0, P_1, ySXLyVBRFCSduTexmBOntZmkspjP.Joystick)
	{
	}

	protected XGGQMyZVNBKhRaGnErwNnBmbPgkE(OfHcchtNNgrqQpaRylQnzBsxMNFD P_0, ettUIVyZApbfOsMrELHoexHuUXcN P_1, ySXLyVBRFCSduTexmBOntZmkspjP P_2)
		: this(P_0, P_1, P_2, P_1.kruIBIEKmDWLUjnMGjJUQFYlSNFx, P_1.QRGPSUGAgdaBSsLOqeCkDiNnMMdGb, P_1.gARdLCOFuGIqRIoDvTCsYmNSZuAM, P_1.KvDEREhlzoVOVCaKPhQeEuSqmmZU)
	{
	}

	protected XGGQMyZVNBKhRaGnErwNnBmbPgkE(OyffEjCfimFwOhHPMLfTymuffXvKA P_0, ettUIVyZApbfOsMrELHoexHuUXcN P_1, ySXLyVBRFCSduTexmBOntZmkspjP P_2, int P_3, int P_4, int P_5, int P_6)
		: base(P_0, P_1, P_2)
	{
		GaEBKEGSkzRgcHxiSrCPspaILNewA = P_3;
		okzbdkhxOafeBloAXVXQjlihbdDdb = P_4;
		zZckucWccGYSrtWZwqSFhDcpcAtiA = P_5;
		tzfbRUaGGtJXAysVzLeuFTNyUqDPA = P_6;
		if (P_4 > 0)
		{
			NzAGkdPRfArYMUGJSuYwkbaOaVbx = new short[P_4];
		}
		bpJHgiLXyeYuTuzIPuYucAwHrgjL = new ButtonLoopSet(ReInput.UserData.ConfigVars.updateLoop, P_3);
		if (P_5 > 0)
		{
			jsKamTQfyGHvElidohatJFRjLfTWA = new short[P_5];
		}
		if (P_6 > 0)
		{
			ZXZSBGgcitQwnqckgtZXaZvgNCtJ = new short[P_6 * 2];
		}
	}

	public void tfetQISCmyhJuXrFFfsAecThqogsA(MwKsnJyPOGEFvGzQAmSgbJnKNOltA P_0, byte P_1, short P_2, double P_3)
	{
		rAgNoUjVPddhslpmbXOMWTyxQoTT = true;
		switch (P_0)
		{
		case MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Button:
			if (P_1 < GaEBKEGSkzRgcHxiSrCPspaILNewA)
			{
				bpJHgiLXyeYuTuzIPuYucAwHrgjL.SetValue(P_1, P_2 > 0, P_3);
			}
			break;
		case MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Axis:
			if (P_1 < okzbdkhxOafeBloAXVXQjlihbdDdb)
			{
				NzAGkdPRfArYMUGJSuYwkbaOaVbx[P_1] = P_2;
			}
			break;
		case MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Hat:
			if (P_1 < zZckucWccGYSrtWZwqSFhDcpcAtiA)
			{
				jsKamTQfyGHvElidohatJFRjLfTWA[P_1] = P_2;
			}
			break;
		case MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Ball:
			if (P_1 < tzfbRUaGGtJXAysVzLeuFTNyUqDPA)
			{
				ZXZSBGgcitQwnqckgtZXaZvgNCtJ[P_1] = P_2;
			}
			break;
		default:
			throw new NotImplementedException();
		}
	}

	public override void XJnbimdAhduykEEPsKYaCQwEKAahb(UpdateLoopType P_0)
	{
		bpJHgiLXyeYuTuzIPuYucAwHrgjL.SetUpdateLoop(P_0);
	}

	public override void zHGKpsfvCVFaGNhLgaGtFCpMeOjVA()
	{
		bpJHgiLXyeYuTuzIPuYucAwHrgjL.Current.ClearWasTrueThisFrame();
	}

	public float lgwptabYgqwjBJLWwSJQTGtzgorFA(int P_0)
	{
		if (P_0 < 0 || P_0 >= okzbdkhxOafeBloAXVXQjlihbdDdb)
		{
			return 0f;
		}
		return PebEDnKecgJEBNPALdoMZOSJmPtSA(NzAGkdPRfArYMUGJSuYwkbaOaVbx[P_0]);
	}

	float MnsFyTzecaNZBeNkLkANtMeQRWvs.lgwptabYgqwjBJLWwSJQTGtzgorFA(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lgwptabYgqwjBJLWwSJQTGtzgorFA
		return this.lgwptabYgqwjBJLWwSJQTGtzgorFA(P_0);
	}

	public bool rVzNVNpkHCuqmpqvtoGwGXczIcQg(int P_0)
	{
		if (P_0 < 0 || P_0 >= GaEBKEGSkzRgcHxiSrCPspaILNewA)
		{
			return false;
		}
		return bpJHgiLXyeYuTuzIPuYucAwHrgjL.Current.effectiveValue[P_0];
	}

	bool MnsFyTzecaNZBeNkLkANtMeQRWvs.rVzNVNpkHCuqmpqvtoGwGXczIcQg(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rVzNVNpkHCuqmpqvtoGwGXczIcQg
		return this.rVzNVNpkHCuqmpqvtoGwGXczIcQg(P_0);
	}

	public int PpskowCPcwjnjIzOFWAKXQJAlDYb(int P_0)
	{
		if (P_0 < 0 || P_0 >= zZckucWccGYSrtWZwqSFhDcpcAtiA)
		{
			return -1;
		}
		return QsFDLKoPFUqRiAmfSZMfotdNeYss(jsKamTQfyGHvElidohatJFRjLfTWA[P_0]);
	}

	int MnsFyTzecaNZBeNkLkANtMeQRWvs.PpskowCPcwjnjIzOFWAKXQJAlDYb(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in PpskowCPcwjnjIzOFWAKXQJAlDYb
		return this.PpskowCPcwjnjIzOFWAKXQJAlDYb(P_0);
	}

	protected void dXEGyBFqKyARnLLzhvBMUVPDKHPv(OfHcchtNNgrqQpaRylQnzBsxMNFD P_0)
	{
		if (!base.wSKSFDMVyLjVDgLPjDCkFXMYWChOA || qbAgQjRXIPTuMaDgGyMMhTVDcNdG.gbYAfmODAbxJbrYwPswXXPVsFLJi(P_0) <= 0)
		{
			return;
		}
		IntPtr intPtr = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.BMHpUopClddpmgoBWsHSZAHfuaKg(P_0);
		if (intPtr == IntPtr.Zero)
		{
			return;
		}
		if (qbAgQjRXIPTuMaDgGyMMhTVDcNdG.NcFdZjuzKsKkgTHBQssMOESuZzmH(intPtr) != 0)
		{
			qbAgQjRXIPTuMaDgGyMMhTVDcNdG.reXIoXuDmMrfgVLfpRkCGVxDVprG(intPtr);
			return;
		}
		IsSMKDMLZTwDLZBPijqyYousCmRKA = new RaEICuTrbdmJUEFyHrrgAgqjpGzQ(intPtr);
		NuepjONlecgJuAKzxtnNmfNqorEO = true;
		HgaBpdztJEsCfRkeZtOedKmLHfUy = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.KLuCwgHKAohbRtZHBJWskDpZJRjCA(IsSMKDMLZTwDLZBPijqyYousCmRKA) > 0;
		if (HgaBpdztJEsCfRkeZtOedKmLHfUy)
		{
			aIVDMZjyOFvIIMkGQNUQWqEnVaPH = 2;
		}
		lvfqwmZIMikiRpgvxLuDRZghPloA = new float[aIVDMZjyOFvIIMkGQNUQWqEnVaPH];
	}

	protected virtual void HqGoQwXsmhNiAmPUEmXvYcFQDwjT()
	{
		dXEGyBFqKyARnLLzhvBMUVPDKHPv(JdzkTmSxRGTcVQUHsODAeChHmHde as OfHcchtNNgrqQpaRylQnzBsxMNFD);
	}

	protected virtual void LXVyLUuTHRtBppOIAfBeCgRNxbDrA()
	{
		if (JdzkTmSxRGTcVQUHsODAeChHmHde != null && JdzkTmSxRGTcVQUHsODAeChHmHde.IsValid)
		{
			if (!VhjCmqwzxIJOGHLyFKYaEkGENQae())
			{
				JdzkTmSxRGTcVQUHsODAeChHmHde.Clear();
				return;
			}
			qbAgQjRXIPTuMaDgGyMMhTVDcNdG.TdrdxKgHjrwNcweBeXrcFsWTCCNG(JdzkTmSxRGTcVQUHsODAeChHmHde);
			JdzkTmSxRGTcVQUHsODAeChHmHde.Clear();
		}
	}

	private float PebEDnKecgJEBNPALdoMZOSJmPtSA(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.ValueInNewRange(P_0, -32767f, 32768f, -1f, 1f);
	}

	private int QsFDLKoPFUqRiAmfSZMfotdNeYss(short P_0)
	{
		return P_0 switch
		{
			0 => -1, 
			1 => 0, 
			3 => 4500, 
			2 => 9000, 
			6 => 13500, 
			4 => 18000, 
			12 => 22500, 
			8 => 27000, 
			9 => 31500, 
			_ => -1, 
		};
	}
}

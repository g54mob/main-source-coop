using System;
using Rewired;
using Rewired.Utils;
using UnityEngine;

internal class RzpKZbCpWsgubaEUuUNtBGeHcjal : MGqxEsfjqlGIjBQUTfROQxLUbYoZ, IDisposable, iJNhZMfwcNtZpDlvPFwjcjTFYpKk, YnTfoKfozPExlkgidDzpkPNmatxBb
{
	public readonly int tpoonPbJmajQyAErVLnAOoXhqZqEb;

	public readonly int yLLlShtixVkbYMaIfuUMijmLCboKA;

	public readonly int VlOBcOivgRYNVAIyWkzlGxAaDIWac;

	public readonly int ZotJgQDIqTjrsClzSattVdeemzgR;

	public readonly short[] MAxFwAqmsJfpojVMHuTDpUYkEapaA;

	private readonly ButtonLoopSet ipAYrbrQgzLCkiFHhDNuBCmkzLcw;

	public readonly short[] jURseArDXjxqXEEYszGLdejKhGAm;

	public readonly short[] ijfvKuMNZlftAJekrEChUoEhQBMO;

	private bool ulpWNMyBfTTmuiYwnRErPHSaPABK;

	public bool[] PkWNzGQFACjziczRXUCemjiNiygjA
	{
		get
		{
			if (ipAYrbrQgzLCkiFHhDNuBCmkzLcw.Current == null)
			{
				return null;
			}
			return ipAYrbrQgzLCkiFHhDNuBCmkzLcw.Current.effectiveValue;
		}
	}

	int YnTfoKfozPExlkgidDzpkPNmatxBb.SLfCsMJeRjsDOoBdIvygtdngNpevA => kXmAeOVIoefUVTrVJWaLobNGDaae;

	int YnTfoKfozPExlkgidDzpkPNmatxBb.QbWWWqDfCwsrFYIwDENWHYhXUTsBA => tpoonPbJmajQyAErVLnAOoXhqZqEb;

	int YnTfoKfozPExlkgidDzpkPNmatxBb.ZJfUpCttcCvXukzpPLgxkmqGsDX => yLLlShtixVkbYMaIfuUMijmLCboKA;

	int YnTfoKfozPExlkgidDzpkPNmatxBb.wEWJeNudDoxMIBHsPNpLBrUiFwdi => VlOBcOivgRYNVAIyWkzlGxAaDIWac;

	int YnTfoKfozPExlkgidDzpkPNmatxBb.AYySKDpnQMQopJBFyHyoWgNtrvAi => ZotJgQDIqTjrsClzSattVdeemzgR;

	bool YnTfoKfozPExlkgidDzpkPNmatxBb.mroXxBMqkLyxzQpOmFaqNuqIVmEI
	{
		get
		{
			if (tpoonPbJmajQyAErVLnAOoXhqZqEb <= 0 && yLLlShtixVkbYMaIfuUMijmLCboKA <= 0 && VlOBcOivgRYNVAIyWkzlGxAaDIWac <= 0)
			{
				return ZotJgQDIqTjrsClzSattVdeemzgR > 0;
			}
			return true;
		}
	}

	InputSource YnTfoKfozPExlkgidDzpkPNmatxBb.TTLdWdiVtetpvRZZFSbUvyMAQDkcA => InputSource.SDL2;

	bool YnTfoKfozPExlkgidDzpkPNmatxBb.ODfJrIENbPyzdtRhKIDHwOKyrmzn => ulpWNMyBfTTmuiYwnRErPHSaPABK;

	public RzpKZbCpWsgubaEUuUNtBGeHcjal(ADyAksuGELvZwlagWICRBzcTKbTy P_0, wHGpGKjtVGRHisAxykuOCasKYqsT P_1)
		: this(P_0, P_1, yByTLKQQjvsbQHbfOrfBRQNKUZvG.Joystick)
	{
	}

	protected RzpKZbCpWsgubaEUuUNtBGeHcjal(ADyAksuGELvZwlagWICRBzcTKbTy P_0, wHGpGKjtVGRHisAxykuOCasKYqsT P_1, yByTLKQQjvsbQHbfOrfBRQNKUZvG P_2)
		: this(P_0, P_1, P_2, P_1.tpoonPbJmajQyAErVLnAOoXhqZqEb, P_1.yLLlShtixVkbYMaIfuUMijmLCboKA, P_1.VlOBcOivgRYNVAIyWkzlGxAaDIWac, P_1.ZotJgQDIqTjrsClzSattVdeemzgR)
	{
	}

	protected RzpKZbCpWsgubaEUuUNtBGeHcjal(IEUEYmBxhRzGqNjFcDGffARVHobJA P_0, wHGpGKjtVGRHisAxykuOCasKYqsT P_1, yByTLKQQjvsbQHbfOrfBRQNKUZvG P_2, int P_3, int P_4, int P_5, int P_6)
		: base(P_0, P_1, P_2)
	{
		tpoonPbJmajQyAErVLnAOoXhqZqEb = P_3;
		yLLlShtixVkbYMaIfuUMijmLCboKA = P_4;
		VlOBcOivgRYNVAIyWkzlGxAaDIWac = P_5;
		ZotJgQDIqTjrsClzSattVdeemzgR = P_6;
		if (P_4 > 0)
		{
			MAxFwAqmsJfpojVMHuTDpUYkEapaA = new short[P_4];
		}
		ipAYrbrQgzLCkiFHhDNuBCmkzLcw = new ButtonLoopSet(ReInput.UserData.ConfigVars.updateLoop, P_3);
		if (P_5 > 0)
		{
			jURseArDXjxqXEEYszGLdejKhGAm = new short[P_5];
		}
		if (P_6 > 0)
		{
			ijfvKuMNZlftAJekrEChUoEhQBMO = new short[P_6 * 2];
		}
	}

	public void tzfYrQaBdsWHACSbQzIxoLZqqtry(GGbtdUzpXnCzPhpAqObQrrCgdzpo P_0, byte P_1, short P_2, double P_3)
	{
		ulpWNMyBfTTmuiYwnRErPHSaPABK = true;
		switch (P_0)
		{
		case GGbtdUzpXnCzPhpAqObQrrCgdzpo.Button:
			if (P_1 < tpoonPbJmajQyAErVLnAOoXhqZqEb)
			{
				ipAYrbrQgzLCkiFHhDNuBCmkzLcw.SetValue(P_1, P_2 > 0, P_3);
			}
			break;
		case GGbtdUzpXnCzPhpAqObQrrCgdzpo.Axis:
			if (P_1 < yLLlShtixVkbYMaIfuUMijmLCboKA)
			{
				MAxFwAqmsJfpojVMHuTDpUYkEapaA[P_1] = P_2;
			}
			break;
		case GGbtdUzpXnCzPhpAqObQrrCgdzpo.Hat:
			if (P_1 < VlOBcOivgRYNVAIyWkzlGxAaDIWac)
			{
				jURseArDXjxqXEEYszGLdejKhGAm[P_1] = P_2;
			}
			break;
		case GGbtdUzpXnCzPhpAqObQrrCgdzpo.Ball:
			if (P_1 < ZotJgQDIqTjrsClzSattVdeemzgR)
			{
				ijfvKuMNZlftAJekrEChUoEhQBMO[P_1] = P_2;
			}
			break;
		default:
			throw new NotImplementedException();
		}
	}

	public virtual void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
	{
		ipAYrbrQgzLCkiFHhDNuBCmkzLcw.SetUpdateLoop(P_0);
	}

	public virtual void JFPDzpFPVYxyqXpnZhoBGcTRgDUr()
	{
		ipAYrbrQgzLCkiFHhDNuBCmkzLcw.Current.ClearWasTrueThisFrame();
	}

	public float vKvknQyltTIOpMBpDfAHAfgnfcpAA(int P_0)
	{
		if (P_0 < 0 || P_0 >= yLLlShtixVkbYMaIfuUMijmLCboKA)
		{
			return 0f;
		}
		return mGaqRebOaIiLKUijrZfbNliBxbmW(MAxFwAqmsJfpojVMHuTDpUYkEapaA[P_0]);
	}

	float YnTfoKfozPExlkgidDzpkPNmatxBb.vKvknQyltTIOpMBpDfAHAfgnfcpAA(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vKvknQyltTIOpMBpDfAHAfgnfcpAA
		return this.vKvknQyltTIOpMBpDfAHAfgnfcpAA(P_0);
	}

	public int LCVzNlCgyaDNvGUnscGrEYEYcdDcA(int P_0)
	{
		if (P_0 < 0 || P_0 >= yLLlShtixVkbYMaIfuUMijmLCboKA)
		{
			return 0;
		}
		return MAxFwAqmsJfpojVMHuTDpUYkEapaA[P_0];
	}

	int YnTfoKfozPExlkgidDzpkPNmatxBb.LCVzNlCgyaDNvGUnscGrEYEYcdDcA(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in LCVzNlCgyaDNvGUnscGrEYEYcdDcA
		return this.LCVzNlCgyaDNvGUnscGrEYEYcdDcA(P_0);
	}

	public bool HJcRYlOYnieEhWqubAkAbPeBDumaA(int P_0)
	{
		if (P_0 < 0 || P_0 >= tpoonPbJmajQyAErVLnAOoXhqZqEb)
		{
			return false;
		}
		return ipAYrbrQgzLCkiFHhDNuBCmkzLcw.Current.effectiveValue[P_0];
	}

	bool YnTfoKfozPExlkgidDzpkPNmatxBb.HJcRYlOYnieEhWqubAkAbPeBDumaA(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in HJcRYlOYnieEhWqubAkAbPeBDumaA
		return this.HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0);
	}

	public int wdZBeFkvcmdlqMODBpHbBGqBelryb(int P_0)
	{
		if (P_0 < 0 || P_0 >= VlOBcOivgRYNVAIyWkzlGxAaDIWac)
		{
			return -1;
		}
		return iZLaPHQZgcNbbYJpokJhOpynoCHU(jURseArDXjxqXEEYszGLdejKhGAm[P_0]);
	}

	int YnTfoKfozPExlkgidDzpkPNmatxBb.wdZBeFkvcmdlqMODBpHbBGqBelryb(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wdZBeFkvcmdlqMODBpHbBGqBelryb
		return this.wdZBeFkvcmdlqMODBpHbBGqBelryb(P_0);
	}

	public Vector2 FkhkjIwNNewAZWsMlASxXwNBXYIF(int P_0)
	{
		return Vector2.zero;
	}

	Vector2 YnTfoKfozPExlkgidDzpkPNmatxBb.FkhkjIwNNewAZWsMlASxXwNBXYIF(int P_0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in FkhkjIwNNewAZWsMlASxXwNBXYIF
		return this.FkhkjIwNNewAZWsMlASxXwNBXYIF(P_0);
	}

	protected void vAiCaCyRHPCERwuThiIXCsrZBBay(ADyAksuGELvZwlagWICRBzcTKbTy P_0)
	{
		if (!base.MIFGEQkClVgkOHFLeCwtLNUzKzlIB || ikfePoAXHchteyoSqLTiXmtbNjbd.NymlbgHDSTbFRsdOkcewEoRMhTPRA(P_0) <= 0)
		{
			return;
		}
		IntPtr intPtr = ikfePoAXHchteyoSqLTiXmtbNjbd.wIDrIWiIpjznMdnJvohkryqIVnsL(P_0);
		if (intPtr == IntPtr.Zero)
		{
			return;
		}
		if (ikfePoAXHchteyoSqLTiXmtbNjbd.XivLjIODRsZlHBDeafxJABCQcgTib(intPtr) != 0)
		{
			ikfePoAXHchteyoSqLTiXmtbNjbd.BDqfTsrKpJKJnURcwWjQsAzrhJpS(intPtr);
			return;
		}
		nXhploYyPEhhiiEZDcejkfehjTiyA = new PZvbSzEYwSCdiqIihOAUbeDZPhxtA(intPtr);
		RRpiupZILmvDBCSVmHNvOYlPhvde = true;
		ItbRXneKvqEQTVCYuqyBINlwBEwe = ikfePoAXHchteyoSqLTiXmtbNjbd.tjdYkmfMhEzEbIbwpCHCIzPxTnmSA(nXhploYyPEhhiiEZDcejkfehjTiyA) > 0;
		if (ItbRXneKvqEQTVCYuqyBINlwBEwe)
		{
			IvQuNSqDypWdalUGYcleYiqEVPmd = 2;
		}
		FxulhbJFYjHjTDJPFHJLzzvShYjO = new float[IvQuNSqDypWdalUGYcleYiqEVPmd];
	}

	protected override void xDyFZpgroUcdHBizALOoydknwrWBA()
	{
		vAiCaCyRHPCERwuThiIXCsrZBBay(eKFLLxLknskFNCzOLLTydpKBHEYD as ADyAksuGELvZwlagWICRBzcTKbTy);
	}

	protected override void ecfUoJrxWmtZhBiNiPzrbkbYuOtb()
	{
		if (eKFLLxLknskFNCzOLLTydpKBHEYD != null && eKFLLxLknskFNCzOLLTydpKBHEYD.IsValid)
		{
			if (!YfufEGSapqNHkdiETLbVWQWZBEIG())
			{
				eKFLLxLknskFNCzOLLTydpKBHEYD.Clear();
				return;
			}
			ikfePoAXHchteyoSqLTiXmtbNjbd.eOTbDMGXrMfbSvFeOGNoXlMpXSXt(eKFLLxLknskFNCzOLLTydpKBHEYD);
			eKFLLxLknskFNCzOLLTydpKBHEYD.Clear();
		}
	}

	private float mGaqRebOaIiLKUijrZfbNliBxbmW(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.ValueInNewRange(P_0, -32767f, 32768f, -1f, 1f);
	}

	private int iZLaPHQZgcNbbYJpokJhOpynoCHU(short P_0)
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

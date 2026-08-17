using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

internal class rnwFCKLFtKcSQJwPKWBWILhwExdCb : IUnifiedMouseSource, IDisposable, IGetSetEnabled
{
	private class IaAkkWvcrQLAtsWLqGTLzQBZbkNb
	{
		private enum urMRhEiPsMpkXqNmKvhidugoRTeS
		{
			None = 0,
			Down = 1,
			Up = 2
		}

		private const int AihbKYQeEgEJZagcUWxcnmaViEfOA = 120;

		private const int aPpDwhGYcgrdkmvwWcEhbHQDQJmLb = 2048;

		public readonly UpdateLoopType SwvlHouiAxpGNIGJRiRIhvplhZZrA;

		public uint kzTmPfSrTxeFDQorjdSSRWkQvlpr;

		public uint BejYRFXYWCaBKvpnUYEToHQVYGFv;

		public UMGTlVOxBWTnLjjXfUqZUPgGfc fgtuSFOAhIknEPGranEXQgxVUqEu;

		public float BqmjxAfwgseNVDKGAzjtxWWBOPHv;

		public float nWpIIQRjiCIFaiiGnHzTvHDxytFG;

		public float DqhPLfimIKFZuAvGmkVXEnTBFEDiB;

		public float kQaHsjepRTBjFeCXfZxRCcLwtUJMA;

		private bool[] ZWjKYwlRQnaLQOLzxkWTBtgjoYPc;

		private bool[] wTpFNghuQUjzqFFJFhfCDmnjotGKc;

		private mePbBJEVHVtSoNYZdAgNewnfxLIHb piyDtRNAZvewLjDvPxfbRNiEDTFn;

		private uint VsVFvODqNDFPGTMISQZITRVWdOAv;

		private int PNnLhaqhwcIQrPGIGUxrRZTNqJZm;

		private int ZOSUnhxlgoGWLhMwICITfyHajHVNb;

		private bool twChYXDqliuearPrRtGuyFndeaZaA;

		public IaAkkWvcrQLAtsWLqGTLzQBZbkNb(mePbBJEVHVtSoNYZdAgNewnfxLIHb P_0, UpdateLoopType P_1)
		{
			piyDtRNAZvewLjDvPxfbRNiEDTFn = P_0;
			SwvlHouiAxpGNIGJRiRIhvplhZZrA = P_1;
			ZWjKYwlRQnaLQOLzxkWTBtgjoYPc = new bool[5];
			wTpFNghuQUjzqFFJFhfCDmnjotGKc = new bool[5];
		}

		public void zVhDdHImPurLcCtmQAZyuNCpgEwYA(qTkbFMdSAhdoJNKlbrGIIsiJQtCh P_0)
		{
			XqlgzVbKskSdDnWlKCpHwqLikLYAb xqlgzVbKskSdDnWlKCpHwqLikLYAb = P_0.tcXtbXvFnMCjbckPypofhTFVJlYUA;
			if (xqlgzVbKskSdDnWlKCpHwqLikLYAb != XqlgzVbKskSdDnWlKCpHwqLikLYAb.None)
			{
				if ((xqlgzVbKskSdDnWlKCpHwqLikLYAb & XqlgzVbKskSdDnWlKCpHwqLikLYAb.LeftButtonDown) != XqlgzVbKskSdDnWlKCpHwqLikLYAb.None || (xqlgzVbKskSdDnWlKCpHwqLikLYAb & XqlgzVbKskSdDnWlKCpHwqLikLYAb.RightButtonDown) != XqlgzVbKskSdDnWlKCpHwqLikLYAb.None)
				{
					IntPtr intPtr = hUfdZejvJYlOtHWdillPtEZZfcOAA.rAUvmTfVyKUAaAHERFekLGhGYfNm();
					if (hUfdZejvJYlOtHWdillPtEZZfcOAA.yxZfeNLhHggOFLfUuATbJUJeBghvA() == intPtr && izdOxnsJKHCtvTDFIhdpIKkWNlMh(intPtr))
					{
						xqlgzVbKskSdDnWlKCpHwqLikLYAb &= ~XqlgzVbKskSdDnWlKCpHwqLikLYAb.LeftButtonDown;
						xqlgzVbKskSdDnWlKCpHwqLikLYAb &= ~XqlgzVbKskSdDnWlKCpHwqLikLYAb.RightButtonDown;
					}
				}
				int num = (int)xqlgzVbKskSdDnWlKCpHwqLikLYAb;
				if (piyDtRNAZvewLjDvPxfbRNiEDTFn.deuPyhgiCEtDCqcpMQCyxZjDsfSO && piyDtRNAZvewLjDvPxfbRNiEDTFn.MiGoFiBrKiyQYJftEFnRUCoblmsJ)
				{
					surWYVBEPteqFgZRNAsQWoyETsiy(1, num, 1, 2);
					surWYVBEPteqFgZRNAsQWoyETsiy(0, num, 4, 8);
				}
				else
				{
					surWYVBEPteqFgZRNAsQWoyETsiy(0, num, 1, 2);
					surWYVBEPteqFgZRNAsQWoyETsiy(1, num, 4, 8);
				}
				surWYVBEPteqFgZRNAsQWoyETsiy(2, num, 16, 32);
				surWYVBEPteqFgZRNAsQWoyETsiy(3, num, 64, 128);
				surWYVBEPteqFgZRNAsQWoyETsiy(4, num, 256, 512);
			}
			kzTmPfSrTxeFDQorjdSSRWkQvlpr = P_0.kzTmPfSrTxeFDQorjdSSRWkQvlpr;
			BejYRFXYWCaBKvpnUYEToHQVYGFv = P_0.BejYRFXYWCaBKvpnUYEToHQVYGFv;
			UMGTlVOxBWTnLjjXfUqZUPgGfc uMGTlVOxBWTnLjjXfUqZUPgGfc = fgtuSFOAhIknEPGranEXQgxVUqEu;
			fgtuSFOAhIknEPGranEXQgxVUqEu = P_0.fgtuSFOAhIknEPGranEXQgxVUqEu;
			if (fgtuSFOAhIknEPGranEXQgxVUqEu != uMGTlVOxBWTnLjjXfUqZUPgGfc)
			{
				twChYXDqliuearPrRtGuyFndeaZaA = false;
			}
			if (fgtuSFOAhIknEPGranEXQgxVUqEu == UMGTlVOxBWTnLjjXfUqZUPgGfc.MoveRelative)
			{
				BqmjxAfwgseNVDKGAzjtxWWBOPHv += (float)P_0.BqmjxAfwgseNVDKGAzjtxWWBOPHv * 0.5f;
				nWpIIQRjiCIFaiiGnHzTvHDxytFG += (float)P_0.nWpIIQRjiCIFaiiGnHzTvHDxytFG * 0.5f * -1f;
			}
			else if ((fgtuSFOAhIknEPGranEXQgxVUqEu & UMGTlVOxBWTnLjjXfUqZUPgGfc.MoveAbsolute) != UMGTlVOxBWTnLjjXfUqZUPgGfc.MoveRelative)
			{
				bool num2 = (fgtuSFOAhIknEPGranEXQgxVUqEu & UMGTlVOxBWTnLjjXfUqZUPgGfc.VirtualDesktop) != 0;
				int num3 = hUfdZejvJYlOtHWdillPtEZZfcOAA.MoJDOljHJxbeLpaYnjiAqKBpcYFvA(num2 ? vQxfanyYJnehFMFEjGpxIpuLKpqg.jcnNvoyIyYldbledMBSULuRLGiBgA : vQxfanyYJnehFMFEjGpxIpuLKpqg.tfJTchjymxaeRNQgFObxmPCglOyX);
				int num4 = hUfdZejvJYlOtHWdillPtEZZfcOAA.MoJDOljHJxbeLpaYnjiAqKBpcYFvA(num2 ? vQxfanyYJnehFMFEjGpxIpuLKpqg.dicUgtSlHUxsHxHYTllMFigSeZFy : vQxfanyYJnehFMFEjGpxIpuLKpqg.NHuJuCvIlijGhGnIVGULDxSReERQA);
				int num5 = (int)((float)P_0.BqmjxAfwgseNVDKGAzjtxWWBOPHv / 65535f * (float)num3);
				int num6 = (int)((65535f - (float)P_0.nWpIIQRjiCIFaiiGnHzTvHDxytFG) / 65535f * (float)num4);
				if (!twChYXDqliuearPrRtGuyFndeaZaA)
				{
					PNnLhaqhwcIQrPGIGUxrRZTNqJZm = num5;
					ZOSUnhxlgoGWLhMwICITfyHajHVNb = num6;
					twChYXDqliuearPrRtGuyFndeaZaA = true;
				}
				BqmjxAfwgseNVDKGAzjtxWWBOPHv += num5 - PNnLhaqhwcIQrPGIGUxrRZTNqJZm;
				nWpIIQRjiCIFaiiGnHzTvHDxytFG += num6 - ZOSUnhxlgoGWLhMwICITfyHajHVNb;
				PNnLhaqhwcIQrPGIGUxrRZTNqJZm = num5;
				ZOSUnhxlgoGWLhMwICITfyHajHVNb = num6;
			}
			else
			{
				BqmjxAfwgseNVDKGAzjtxWWBOPHv = P_0.BqmjxAfwgseNVDKGAzjtxWWBOPHv;
				nWpIIQRjiCIFaiiGnHzTvHDxytFG = P_0.nWpIIQRjiCIFaiiGnHzTvHDxytFG;
			}
			if (P_0.DKMYJLEugkVTYRCUGNnsNeHbTzQO != 0)
			{
				int num7 = ((MathTools.Abs(P_0.DKMYJLEugkVTYRCUGNnsNeHbTzQO) < 120) ? MathTools.Sign(P_0.DKMYJLEugkVTYRCUGNnsNeHbTzQO) : (P_0.DKMYJLEugkVTYRCUGNnsNeHbTzQO / 120));
				if ((xqlgzVbKskSdDnWlKCpHwqLikLYAb & XqlgzVbKskSdDnWlKCpHwqLikLYAb.MouseWheel) != XqlgzVbKskSdDnWlKCpHwqLikLYAb.None)
				{
					DqhPLfimIKFZuAvGmkVXEnTBFEDiB += num7;
				}
				else if ((xqlgzVbKskSdDnWlKCpHwqLikLYAb & (XqlgzVbKskSdDnWlKCpHwqLikLYAb)2048) != XqlgzVbKskSdDnWlKCpHwqLikLYAb.None)
				{
					kQaHsjepRTBjFeCXfZxRCcLwtUJMA += num7;
				}
			}
		}

		public void ieMLFOneTNvwpyelecSLKzRFbIpQ(ControllerDataUpdater P_0)
		{
			float[] axisValues = P_0.axisValues;
			axisValues[0] = BqmjxAfwgseNVDKGAzjtxWWBOPHv;
			axisValues[1] = nWpIIQRjiCIFaiiGnHzTvHDxytFG;
			axisValues[2] = DqhPLfimIKFZuAvGmkVXEnTBFEDiB;
			axisValues[3] = kQaHsjepRTBjFeCXfZxRCcLwtUJMA;
			bool[] buttonValues = P_0.buttonValues;
			for (int i = 0; i < 5; i++)
			{
				buttonValues[i] = ZWjKYwlRQnaLQOLzxkWTBtgjoYPc[i] || wTpFNghuQUjzqFFJFhfCDmnjotGKc[i];
			}
			PPXvJziBoAcRNKtWRVpDUpMyePREb();
		}

		public void UXsGlgdEPfSLHGRZMVqvnDmuNwHcA()
		{
			PPXvJziBoAcRNKtWRVpDUpMyePREb();
		}

		private void PPXvJziBoAcRNKtWRVpDUpMyePREb()
		{
			if (VsVFvODqNDFPGTMISQZITRVWdOAv != ReInput.absFrame)
			{
				nPCAUmZrQSESVauiyFAWCNzIHDLnB();
				VsVFvODqNDFPGTMISQZITRVWdOAv = ReInput.absFrame;
			}
		}

		public void woKyyTBPSBeTJzbsBHrQfbqfolrf()
		{
			BqmjxAfwgseNVDKGAzjtxWWBOPHv = 0f;
			nWpIIQRjiCIFaiiGnHzTvHDxytFG = 0f;
			BejYRFXYWCaBKvpnUYEToHQVYGFv = 0u;
			fgtuSFOAhIknEPGranEXQgxVUqEu = UMGTlVOxBWTnLjjXfUqZUPgGfc.MoveRelative;
			DqhPLfimIKFZuAvGmkVXEnTBFEDiB = 0f;
			kQaHsjepRTBjFeCXfZxRCcLwtUJMA = 0f;
			Array.Clear(ZWjKYwlRQnaLQOLzxkWTBtgjoYPc, 0, 5);
			Array.Clear(wTpFNghuQUjzqFFJFhfCDmnjotGKc, 0, 5);
			twChYXDqliuearPrRtGuyFndeaZaA = false;
		}

		public void nPCAUmZrQSESVauiyFAWCNzIHDLnB()
		{
			BqmjxAfwgseNVDKGAzjtxWWBOPHv = 0f;
			nWpIIQRjiCIFaiiGnHzTvHDxytFG = 0f;
			DqhPLfimIKFZuAvGmkVXEnTBFEDiB = 0f;
			kQaHsjepRTBjFeCXfZxRCcLwtUJMA = 0f;
			Array.Clear(wTpFNghuQUjzqFFJFhfCDmnjotGKc, 0, 5);
		}

		private bool CTRqAkNbPTeaCEiQihtiarTqVmzG(int P_0, int P_1, int P_2)
		{
			if ((P_0 & P_1) == P_1 && (P_0 & P_2) != P_2)
			{
				return true;
			}
			return false;
		}

		private urMRhEiPsMpkXqNmKvhidugoRTeS RnlRBstnpIaOcnwQYaKSlrTXiaWh(int P_0, int P_1, int P_2)
		{
			if ((P_0 & P_1) == P_1)
			{
				if ((P_0 & P_2) == P_2)
				{
					return urMRhEiPsMpkXqNmKvhidugoRTeS.None;
				}
				return urMRhEiPsMpkXqNmKvhidugoRTeS.Down;
			}
			if ((P_0 & P_2) == P_2)
			{
				return urMRhEiPsMpkXqNmKvhidugoRTeS.Up;
			}
			return urMRhEiPsMpkXqNmKvhidugoRTeS.None;
		}

		private void surWYVBEPteqFgZRNAsQWoyETsiy(int P_0, int P_1, int P_2, int P_3)
		{
			urMRhEiPsMpkXqNmKvhidugoRTeS urMRhEiPsMpkXqNmKvhidugoRTeS2 = RnlRBstnpIaOcnwQYaKSlrTXiaWh(P_1, P_2, P_3);
			if (ZWjKYwlRQnaLQOLzxkWTBtgjoYPc[P_0])
			{
				if (urMRhEiPsMpkXqNmKvhidugoRTeS2 == urMRhEiPsMpkXqNmKvhidugoRTeS.Up)
				{
					ZWjKYwlRQnaLQOLzxkWTBtgjoYPc[P_0] = false;
				}
			}
			else if (urMRhEiPsMpkXqNmKvhidugoRTeS2 == urMRhEiPsMpkXqNmKvhidugoRTeS.Down)
			{
				ZWjKYwlRQnaLQOLzxkWTBtgjoYPc[P_0] = true;
			}
			if (urMRhEiPsMpkXqNmKvhidugoRTeS2 == urMRhEiPsMpkXqNmKvhidugoRTeS.Down)
			{
				wTpFNghuQUjzqFFJFhfCDmnjotGKc[P_0] = true;
			}
		}

		private static bool izdOxnsJKHCtvTDFIhdpIKkWNlMh(IntPtr P_0)
		{
			if (hUfdZejvJYlOtHWdillPtEZZfcOAA.waCzeuZmDRenuGpeuOhpunfqJEOfA(0u, false, 0u) == IntPtr.Zero)
			{
				return false;
			}
			if (!hUfdZejvJYlOtHWdillPtEZZfcOAA.deYrdqkeNPjGdmhlrfEMelPdDsRnA(P_0, out var teeEvShNbEAwHqxvHccQNVFhiujR))
			{
				return false;
			}
			if (!hUfdZejvJYlOtHWdillPtEZZfcOAA.fEVdYbdhbJnGjBnmxunFKiSVekEB(out var teeEvShNbEAwHqxvHccQNVFhiujR2))
			{
				return false;
			}
			if (!hUfdZejvJYlOtHWdillPtEZZfcOAA.QLQLhKHtdIxKoJdVkbmitjMBJhYEA(P_0, out var gDhBjsxtDCXZFOcJIzBWllEGJgSJ2))
			{
				return false;
			}
			int num = teeEvShNbEAwHqxvHccQNVFhiujR2.BqmjxAfwgseNVDKGAzjtxWWBOPHv - teeEvShNbEAwHqxvHccQNVFhiujR.BqmjxAfwgseNVDKGAzjtxWWBOPHv;
			int num2 = teeEvShNbEAwHqxvHccQNVFhiujR2.nWpIIQRjiCIFaiiGnHzTvHDxytFG - teeEvShNbEAwHqxvHccQNVFhiujR.nWpIIQRjiCIFaiiGnHzTvHDxytFG;
			if (num >= 0 && num2 >= 0 && num <= gDhBjsxtDCXZFOcJIzBWllEGJgSJ2.TntMeZzcMmBZOTwtDSHHENXpSOAk && num2 <= gDhBjsxtDCXZFOcJIzBWllEGJgSJ2.NeOKLbAsSOxNvUciUkghCNfdjWPf)
			{
				return false;
			}
			if (!hUfdZejvJYlOtHWdillPtEZZfcOAA.aAuqFlmzIsPeHaOMPjdAaXUUDyPBA(P_0, out var gDhBjsxtDCXZFOcJIzBWllEGJgSJ3))
			{
				return false;
			}
			if (teeEvShNbEAwHqxvHccQNVFhiujR2.BqmjxAfwgseNVDKGAzjtxWWBOPHv >= gDhBjsxtDCXZFOcJIzBWllEGJgSJ3.WneIHUsoKTCLrDjIKFMqhyrLEhNp && teeEvShNbEAwHqxvHccQNVFhiujR2.BqmjxAfwgseNVDKGAzjtxWWBOPHv <= gDhBjsxtDCXZFOcJIzBWllEGJgSJ3.TntMeZzcMmBZOTwtDSHHENXpSOAk && teeEvShNbEAwHqxvHccQNVFhiujR2.nWpIIQRjiCIFaiiGnHzTvHDxytFG >= gDhBjsxtDCXZFOcJIzBWllEGJgSJ3.UmjwrixTJFAlFSfNRbgZvNEsoQdC)
			{
				return teeEvShNbEAwHqxvHccQNVFhiujR2.nWpIIQRjiCIFaiiGnHzTvHDxytFG <= gDhBjsxtDCXZFOcJIzBWllEGJgSJ3.NeOKLbAsSOxNvUciUkghCNfdjWPf;
			}
			return false;
		}
	}

	private class mePbBJEVHVtSoNYZdAgNewnfxLIHb
	{
		private bool bygchBiNKpgguCZJXkLMcmYodbqaA;

		private bool GomcyEbEVfRafEegGzbrQzsyBXdT;

		private bool FDSmKZqklaVIXSpSlvPOaPNgjofM;

		private int bAACSgabiArYInvOfiGiklPqIYIN = 10;

		private readonly float wjYXkChYJPARSBiGVVEEtcoxVasA;

		private double fclkETjsDYfvMLSHrqIgPTUxOKBq;

		public bool deuPyhgiCEtDCqcpMQCyxZjDsfSO
		{
			get
			{
				return bygchBiNKpgguCZJXkLMcmYodbqaA;
			}
			set
			{
				if (flag != bygchBiNKpgguCZJXkLMcmYodbqaA)
				{
					nxJlGxBTGuBwsHbuYNCoZXOTdVOf(true);
				}
			}
		}

		public bool MiGoFiBrKiyQYJftEFnRUCoblmsJ => GomcyEbEVfRafEegGzbrQzsyBXdT;

		public bool fjCczSBZcMaptzcgyNAkKUYeYwogA
		{
			get
			{
				return FDSmKZqklaVIXSpSlvPOaPNgjofM;
			}
			set
			{
				if (FDSmKZqklaVIXSpSlvPOaPNgjofM != flag)
				{
					FDSmKZqklaVIXSpSlvPOaPNgjofM = flag;
					nxJlGxBTGuBwsHbuYNCoZXOTdVOf(true);
				}
			}
		}

		public int TJViixwXcvqZxWhuWBlbvtBiPyZe => bAACSgabiArYInvOfiGiklPqIYIN;

		public mePbBJEVHVtSoNYZdAgNewnfxLIHb(bool P_0, float P_1)
		{
			bygchBiNKpgguCZJXkLMcmYodbqaA = P_0;
			wjYXkChYJPARSBiGVVEEtcoxVasA = P_1;
			nxJlGxBTGuBwsHbuYNCoZXOTdVOf(false);
		}

		public void mPVLsAWcaTKfbIPORRXexWhffTDm()
		{
			if (bygchBiNKpgguCZJXkLMcmYodbqaA && !(ReInput.realTime < fclkETjsDYfvMLSHrqIgPTUxOKBq))
			{
				nxJlGxBTGuBwsHbuYNCoZXOTdVOf(true);
			}
		}

		private void nxJlGxBTGuBwsHbuYNCoZXOTdVOf(bool P_0)
		{
			if (FDSmKZqklaVIXSpSlvPOaPNgjofM)
			{
				hUfdZejvJYlOtHWdillPtEZZfcOAA.XDwFnuwWAuhajDTIKKbdZeygFPShA(112u, 0u, ref bAACSgabiArYInvOfiGiklPqIYIN, 0u);
			}
			GomcyEbEVfRafEegGzbrQzsyBXdT = hUfdZejvJYlOtHWdillPtEZZfcOAA.MoJDOljHJxbeLpaYnjiAqKBpcYFvA(vQxfanyYJnehFMFEjGpxIpuLKpqg.qQoVopgJorblJhkmcnQMnWWglYjc) > 0;
			if (P_0)
			{
				fclkETjsDYfvMLSHrqIgPTUxOKBq = ReInput.realTime + (double)wjYXkChYJPARSBiGVVEEtcoxVasA;
			}
		}
	}

	private const int QpfwGuUFhAlzNgdRjkkqEsyktvho = 5;

	private const int SdseDhPQChIUhkpTFpcZHhGtAzdW = 4;

	private readonly object ifhEwCkIVuTGOnfpubaTrgsqjFiQ = new object();

	private UpdateLoopDataSet<IaAkkWvcrQLAtsWLqGTLzQBZbkNb> QVREeTVFnkMxcshLfAilBnFGqurw;

	private HardwareControllerMap_Game YCOJSLmiUXWAYbZCerZTfDshiiq;

	private mePbBJEVHVtSoNYZdAgNewnfxLIHb piyDtRNAZvewLjDvPxfbRNiEDTFn;

	private bool rtzCYjEVafvMDStvvUswFKRWbCcp;

	private int FObUYObRBEcCoPbRsDouaYSeBopzA;

	private bool bygchBiNKpgguCZJXkLMcmYodbqaA;

	private const bool QpfeqHCdTVLvEucZDvLelRFtZrCJA = true;

	private const float uUlgnYBziGEACBwneMOzjNvHJJldb = 2f;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public bool enabled
	{
		get
		{
			return bygchBiNKpgguCZJXkLMcmYodbqaA;
		}
		set
		{
			if (bygchBiNKpgguCZJXkLMcmYodbqaA != value)
			{
				bygchBiNKpgguCZJXkLMcmYodbqaA = value;
				Clear();
				ThreadSafeUnityInput.mouse.Monitor(value);
			}
		}
	}

	public InputSource inputSource => InputSource.RawInput;

	public HardwareControllerMap_Game hardwareMap
	{
		get
		{
			if (YCOJSLmiUXWAYbZCerZTfDshiiq == null)
			{
				YCOJSLmiUXWAYbZCerZTfDshiiq = MElmyMZScdpAijYFRRwUzvkpPpAv();
			}
			return YCOJSLmiUXWAYbZCerZTfDshiiq;
		}
	}

	public int buttonCount => 5;

	public int axisCount => 4;

	public Vector2 mousePosition
	{
		get
		{
			if (!bygchBiNKpgguCZJXkLMcmYodbqaA)
			{
				return default(Vector2);
			}
			return ThreadSafeUnityInput.mouse.mousePosition;
		}
	}

	public Controller.Extension controllerExtension => null;

	public rnwFCKLFtKcSQJwPKWBWILhwExdCb(UpdateLoopSetting P_0)
	{
		sEBMIzSVtBLWAxLCiMCvGkJwRsBR();
		piyDtRNAZvewLjDvPxfbRNiEDTFn = new mePbBJEVHVtSoNYZdAgNewnfxLIHb(true, 2f);
		QVREeTVFnkMxcshLfAilBnFGqurw = new UpdateLoopDataSet<IaAkkWvcrQLAtsWLqGTLzQBZbkNb>(P_0);
		using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
		{
			List<UpdateLoopType> list = tList.list;
			EnumConverter.ToUpdateLoopTypes(P_0, list);
			for (int i = 0; i < list.Count; i++)
			{
				QVREeTVFnkMxcshLfAilBnFGqurw[i] = new IaAkkWvcrQLAtsWLqGTLzQBZbkNb(piyDtRNAZvewLjDvPxfbRNiEDTFn, list[i]);
			}
		}
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Mouse);
		ReInput.ApplicationFocusChangedEvent += avyeaphbZibEAshhtYkwPMXPQIgq;
		enabled = true;
		ReInput.EditorPauseChangedEvent += pDYfMVFoAOSNVbRGpQChmkUUIjrv;
		ReInput.TimeScalePauseChangedEvent += FgSLXZBjBCJWqEXhRfqbKXTvMlpe;
		ReInput.UpdateEndedEvent += OgmPqENlNCgyBQmzcAocbaOTDmQj;
	}

	public void mPVLsAWcaTKfbIPORRXexWhffTDm(UpdateLoopType P_0)
	{
		QVREeTVFnkMxcshLfAilBnFGqurw.SetUpdateLoop(P_0);
		piyDtRNAZvewLjDvPxfbRNiEDTFn.mPVLsAWcaTKfbIPORRXexWhffTDm();
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Mouse);
	}

	public void QSrdMgzEIZiOTuryCFRIEhNVtPjsA(qTkbFMdSAhdoJNKlbrGIIsiJQtCh P_0)
	{
		if (!rtzCYjEVafvMDStvvUswFKRWbCcp)
		{
			return;
		}
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			int count = QVREeTVFnkMxcshLfAilBnFGqurw.Count;
			for (int i = 0; i < count; i++)
			{
				QVREeTVFnkMxcshLfAilBnFGqurw[i].zVhDdHImPurLcCtmQAZyuNCpgEwYA(P_0);
			}
		}
	}

	public void RHgfvdegRaclghLQGGXSxvRjTVmJA(bool P_0)
	{
		BNERweCtTTGXYCNaMlrNzVmnWymp();
	}

	public void gGXcVSZRUaZCAvwMZVjHsmeVTsJJ(bool P_0)
	{
		if (sEBMIzSVtBLWAxLCiMCvGkJwRsBR() < 0)
		{
			BNERweCtTTGXYCNaMlrNzVmnWymp();
		}
	}

	private int sEBMIzSVtBLWAxLCiMCvGkJwRsBR()
	{
		int fObUYObRBEcCoPbRsDouaYSeBopzA = FObUYObRBEcCoPbRsDouaYSeBopzA;
		if (pYDDNhBibHfSZzyMuFjJSOUJKDWk.UFYWIAYzCAWXNVscTkIIdREHdVye(thGpvMJraEkCGcLLHOTxuArPRrdh.Mouse, out var fObUYObRBEcCoPbRsDouaYSeBopzA2))
		{
			FObUYObRBEcCoPbRsDouaYSeBopzA = fObUYObRBEcCoPbRsDouaYSeBopzA2;
		}
		else
		{
			FObUYObRBEcCoPbRsDouaYSeBopzA = ((hUfdZejvJYlOtHWdillPtEZZfcOAA.MoJDOljHJxbeLpaYnjiAqKBpcYFvA(vQxfanyYJnehFMFEjGpxIpuLKpqg.IkwFAZWulboBmewbiDqBmFiQHGaeA) != 0) ? 1 : 0);
		}
		return FObUYObRBEcCoPbRsDouaYSeBopzA - fObUYObRBEcCoPbRsDouaYSeBopzA;
	}

	private void avyeaphbZibEAshhtYkwPMXPQIgq(bool P_0)
	{
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Mouse);
		if (!P_0 && !rtzCYjEVafvMDStvvUswFKRWbCcp)
		{
			BNERweCtTTGXYCNaMlrNzVmnWymp();
		}
	}

	private void pDYfMVFoAOSNVbRGpQChmkUUIjrv(bool P_0)
	{
	}

	private void FgSLXZBjBCJWqEXhRfqbKXTvMlpe(bool P_0)
	{
		if ((ReInput.configVars.updateLoop & UpdateLoopSetting.FixedUpdate) == 0)
		{
			return;
		}
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Mouse);
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			QVREeTVFnkMxcshLfAilBnFGqurw[QVREeTVFnkMxcshLfAilBnFGqurw.fixedUpdateSetIndex].nPCAUmZrQSESVauiyFAWCNzIHDLnB();
		}
	}

	private void OgmPqENlNCgyBQmzcAocbaOTDmQj(UpdateLoopType P_0)
	{
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			QVREeTVFnkMxcshLfAilBnFGqurw.Get(P_0).UXsGlgdEPfSLHGRZMVqvnDmuNwHcA();
		}
	}

	private void BNERweCtTTGXYCNaMlrNzVmnWymp()
	{
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			int count = QVREeTVFnkMxcshLfAilBnFGqurw.Count;
			for (int i = 0; i < count; i++)
			{
				QVREeTVFnkMxcshLfAilBnFGqurw[i].woKyyTBPSBeTJzbsBHrQfbqfolrf();
			}
		}
	}

	public void UpdateInputData(ControllerDataUpdater dataUpdater)
	{
		QVREeTVFnkMxcshLfAilBnFGqurw.Current.ieMLFOneTNvwpyelecSLKzRFbIpQ(dataUpdater);
	}

	public void Clear()
	{
		BNERweCtTTGXYCNaMlrNzVmnWymp();
	}

	private HardwareControllerMap_Game MElmyMZScdpAijYFRRwUzvkpPpAv()
	{
		ControllerElementIdentifier[] array = new ControllerElementIdentifier[Consts.rawInputUnifiedMouseElementIdentifiers.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new ControllerElementIdentifier(Consts.rawInputUnifiedMouseElementIdentifiers[i]);
		}
		int[] array2 = new int[5];
		int[] array3 = new int[4];
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].elementType == ControllerElementType.Axis)
			{
				array3[num2++] = array[j].id;
			}
			else if (array[j].elementType == ControllerElementType.Button)
			{
				array2[num++] = array[j].id;
			}
		}
		AxisCalibrationData[] array4 = new AxisCalibrationData[4];
		AxisRange[] array5 = new AxisRange[4];
		HardwareAxisInfo[] array6 = new HardwareAxisInfo[4];
		HardwareButtonInfo[] array7 = new HardwareButtonInfo[5];
		for (int k = 0; k < 4; k++)
		{
			array4[k] = AxisCalibrationData.Raw;
			array5[k] = AxisRange.Full;
			float num3 = (((uint)k > 1u) ? 2f : 100f);
			array6[k] = new HardwareAxisInfo(AxisCoordinateMode.Relative, false, num3, SpecialAxisType.None);
		}
		for (int l = 0; l < 5; l++)
		{
			array7[l] = new HardwareButtonInfo();
		}
		return new HardwareControllerMap_Game("Mouse", default(HardwareControllerMapIdentifier), array, array2, array3, array4, array5, array6, array7, null);
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
			ReInput.ApplicationFocusChangedEvent -= avyeaphbZibEAshhtYkwPMXPQIgq;
			ReInput.EditorPauseChangedEvent -= pDYfMVFoAOSNVbRGpQChmkUUIjrv;
			ReInput.TimeScalePauseChangedEvent -= FgSLXZBjBCJWqEXhRfqbKXTvMlpe;
			ReInput.UpdateEndedEvent -= OgmPqENlNCgyBQmzcAocbaOTDmQj;
			if (P_0 && bygchBiNKpgguCZJXkLMcmYodbqaA)
			{
				ThreadSafeUnityInput.mouse.Monitor(state: false);
			}
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}
}

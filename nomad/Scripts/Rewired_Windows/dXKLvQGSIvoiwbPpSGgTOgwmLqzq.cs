using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal class dXKLvQGSIvoiwbPpSGgTOgwmLqzq : IUnifiedMouseSource, IGetSetEnabled, IDisposable
{
	private class aFwiLZbLHZUQheVKOeYsNiyaicgRA
	{
		private enum dKUUFViHIjcmhpmfssagCwcXdjnG
		{
			None = 0,
			Down = 1,
			Up = 2,
			DownAndUp = 3
		}

		public readonly UpdateLoopType ZwpzIbjFlSSiUjcOdxiNvzFelOSv;

		public uint lxBRwTQYNWtejkKsUWfFWpeIygYQ;

		public uint lflBUMPmEwNvkJoxWQkiGghTjVVr;

		public FhAZaChLWjMPTloxUEjbHTLOaeLMA tSMKCMblpTdWIYRFAfaEdfhtALdAA;

		public float mgOUDaaKmMMIGVRKXzrMaNCGbJyb;

		public float PKWflsvMWzXBCVKeefwSArrpFFVDA;

		public float DzztwrrKkniCFZDsqmovzQwMentM;

		public float tlsuGOnVfPEcPsvcHtTqOiVVhehO;

		private bool[] fMpldWGEqXDslzcFqELdjdLkbSXKA;

		private bool[] VAnVUaKYNgtTlgQfUCPULNdLYKzv;

		private hXPLmqkvQTBPfYanxiNCdDvTKtXA RKoTgNIMArQmXAQWxahaEhYaWDoi;

		private uint tFcotkmLiMBlfWOHnWwqFRBZcfVp;

		private int QLGmKRcKFtYqqMjUjMAWIKSOqBnw;

		private int hMFGJVyWKoEluGRClietXSzjpIWlA;

		private bool lNWpqFAFpZhMoiUkVfPAcGhhiyLpc;

		public aFwiLZbLHZUQheVKOeYsNiyaicgRA(hXPLmqkvQTBPfYanxiNCdDvTKtXA P_0, UpdateLoopType P_1)
		{
			RKoTgNIMArQmXAQWxahaEhYaWDoi = P_0;
			ZwpzIbjFlSSiUjcOdxiNvzFelOSv = P_1;
			fMpldWGEqXDslzcFqELdjdLkbSXKA = new bool[5];
			VAnVUaKYNgtTlgQfUCPULNdLYKzv = new bool[5];
		}

		public void PluVROyMoViAefKSURiKvMwXvgxX(ZpILADRRhNkXwxVGHEqPQQWInAbm P_0)
		{
			ANfLcDOJlivBSIwZrLZNHNmfRaZd aNfLcDOJlivBSIwZrLZNHNmfRaZd = P_0.eQQNJerQboovRmFjtFYaIrYZvrce;
			if (aNfLcDOJlivBSIwZrLZNHNmfRaZd != ANfLcDOJlivBSIwZrLZNHNmfRaZd.None)
			{
				if ((aNfLcDOJlivBSIwZrLZNHNmfRaZd & ANfLcDOJlivBSIwZrLZNHNmfRaZd.LeftButtonDown) != ANfLcDOJlivBSIwZrLZNHNmfRaZd.None || (aNfLcDOJlivBSIwZrLZNHNmfRaZd & ANfLcDOJlivBSIwZrLZNHNmfRaZd.RightButtonDown) != ANfLcDOJlivBSIwZrLZNHNmfRaZd.None)
				{
					IntPtr intPtr = KQKvYsAXvDlLWOZXkMKdMDaTTekW.dYTsadZkMhizYZtWgRTZblGzQsAK();
					if (KQKvYsAXvDlLWOZXkMKdMDaTTekW.AibyfVejWlavgkstvckwcMQlHaQgb() == intPtr && spsceRdMKawDBvrYMHjfQNfNHXkdA(intPtr))
					{
						aNfLcDOJlivBSIwZrLZNHNmfRaZd &= ~ANfLcDOJlivBSIwZrLZNHNmfRaZd.LeftButtonDown;
						aNfLcDOJlivBSIwZrLZNHNmfRaZd &= ~ANfLcDOJlivBSIwZrLZNHNmfRaZd.RightButtonDown;
					}
				}
				int num = (int)aNfLcDOJlivBSIwZrLZNHNmfRaZd;
				if (RKoTgNIMArQmXAQWxahaEhYaWDoi.osxuHJLGztVOPPrcWlIKNZhkiYrA && RKoTgNIMArQmXAQWxahaEhYaWDoi.xkNaqdFdEAotuSjjHhSVIfbXkZcS)
				{
					uehXKivMzXIXEfFKLjdCcDEGIBgYA(1, num, 1, 2);
					uehXKivMzXIXEfFKLjdCcDEGIBgYA(0, num, 4, 8);
				}
				else
				{
					uehXKivMzXIXEfFKLjdCcDEGIBgYA(0, num, 1, 2);
					uehXKivMzXIXEfFKLjdCcDEGIBgYA(1, num, 4, 8);
				}
				uehXKivMzXIXEfFKLjdCcDEGIBgYA(2, num, 16, 32);
				uehXKivMzXIXEfFKLjdCcDEGIBgYA(3, num, 64, 128);
				uehXKivMzXIXEfFKLjdCcDEGIBgYA(4, num, 256, 512);
			}
			lxBRwTQYNWtejkKsUWfFWpeIygYQ = P_0.bNjUDAQgnQfQFADUZeXKBFyALBsP;
			lflBUMPmEwNvkJoxWQkiGghTjVVr = P_0.TRaWaDbJPHIMgcsKJrcsgCtbWNuoA;
			FhAZaChLWjMPTloxUEjbHTLOaeLMA fhAZaChLWjMPTloxUEjbHTLOaeLMA = tSMKCMblpTdWIYRFAfaEdfhtALdAA;
			tSMKCMblpTdWIYRFAfaEdfhtALdAA = P_0.YuCXEluQGxCuBTDeGdBuawgUcRSi;
			if (tSMKCMblpTdWIYRFAfaEdfhtALdAA != fhAZaChLWjMPTloxUEjbHTLOaeLMA)
			{
				lNWpqFAFpZhMoiUkVfPAcGhhiyLpc = false;
			}
			if (tSMKCMblpTdWIYRFAfaEdfhtALdAA == FhAZaChLWjMPTloxUEjbHTLOaeLMA.MoveRelative)
			{
				mgOUDaaKmMMIGVRKXzrMaNCGbJyb += (float)P_0.QmtcUthwfQDjlmosjTLYLuCMJLKGb * 0.5f;
				PKWflsvMWzXBCVKeefwSArrpFFVDA += (float)P_0.fwtLHWyxKZlBfAGWWfJSHtaVAlSE * 0.5f * -1f;
			}
			else if ((tSMKCMblpTdWIYRFAfaEdfhtALdAA & FhAZaChLWjMPTloxUEjbHTLOaeLMA.MoveAbsolute) != FhAZaChLWjMPTloxUEjbHTLOaeLMA.MoveRelative)
			{
				bool num2 = (tSMKCMblpTdWIYRFAfaEdfhtALdAA & FhAZaChLWjMPTloxUEjbHTLOaeLMA.VirtualDesktop) != 0;
				int num3 = KQKvYsAXvDlLWOZXkMKdMDaTTekW.ajBOvwTePqLBYFJfsEPHxqyplzfk(num2 ? gbkwoNGXbnemmwRJGohrxZfefZAp.hEvtkdwKZNeOljpWoZJVgHSNCEioA : gbkwoNGXbnemmwRJGohrxZfefZAp.dhxbinqwcCoYdERWjGjzehbQHgicA);
				int num4 = KQKvYsAXvDlLWOZXkMKdMDaTTekW.ajBOvwTePqLBYFJfsEPHxqyplzfk(num2 ? gbkwoNGXbnemmwRJGohrxZfefZAp.oPUYePlVnBmKUOzfoJBDsdQpplEL : gbkwoNGXbnemmwRJGohrxZfefZAp.paTGEtdfNbAMnkkRUmkcNltpwxcp);
				int num5 = (int)((float)P_0.QmtcUthwfQDjlmosjTLYLuCMJLKGb / 65535f * (float)num3);
				int num6 = (int)((65535f - (float)P_0.fwtLHWyxKZlBfAGWWfJSHtaVAlSE) / 65535f * (float)num4);
				if (!lNWpqFAFpZhMoiUkVfPAcGhhiyLpc)
				{
					QLGmKRcKFtYqqMjUjMAWIKSOqBnw = num5;
					hMFGJVyWKoEluGRClietXSzjpIWlA = num6;
					lNWpqFAFpZhMoiUkVfPAcGhhiyLpc = true;
				}
				mgOUDaaKmMMIGVRKXzrMaNCGbJyb += num5 - QLGmKRcKFtYqqMjUjMAWIKSOqBnw;
				PKWflsvMWzXBCVKeefwSArrpFFVDA += num6 - hMFGJVyWKoEluGRClietXSzjpIWlA;
				QLGmKRcKFtYqqMjUjMAWIKSOqBnw = num5;
				hMFGJVyWKoEluGRClietXSzjpIWlA = num6;
			}
			else
			{
				mgOUDaaKmMMIGVRKXzrMaNCGbJyb = P_0.QmtcUthwfQDjlmosjTLYLuCMJLKGb;
				PKWflsvMWzXBCVKeefwSArrpFFVDA = P_0.fwtLHWyxKZlBfAGWWfJSHtaVAlSE;
			}
			if (P_0.rczHsakLdhUTSCpREWTVEJhzIwgz != 0)
			{
				int num7 = ((MathTools.Abs(P_0.rczHsakLdhUTSCpREWTVEJhzIwgz) < 120) ? MathTools.Sign(P_0.rczHsakLdhUTSCpREWTVEJhzIwgz) : (P_0.rczHsakLdhUTSCpREWTVEJhzIwgz / 120));
				if ((aNfLcDOJlivBSIwZrLZNHNmfRaZd & ANfLcDOJlivBSIwZrLZNHNmfRaZd.MouseWheel) != ANfLcDOJlivBSIwZrLZNHNmfRaZd.None)
				{
					DzztwrrKkniCFZDsqmovzQwMentM += num7;
				}
				else if ((aNfLcDOJlivBSIwZrLZNHNmfRaZd & (ANfLcDOJlivBSIwZrLZNHNmfRaZd)2048) != ANfLcDOJlivBSIwZrLZNHNmfRaZd.None)
				{
					tlsuGOnVfPEcPsvcHtTqOiVVhehO += num7;
				}
			}
		}

		public void SojpnaSqvJIbxmYcwhTUUPFDjuDM(ControllerDataUpdater P_0)
		{
			float[] axisValues = P_0.axisValues;
			axisValues[0] = mgOUDaaKmMMIGVRKXzrMaNCGbJyb;
			axisValues[1] = PKWflsvMWzXBCVKeefwSArrpFFVDA;
			axisValues[2] = DzztwrrKkniCFZDsqmovzQwMentM;
			axisValues[3] = tlsuGOnVfPEcPsvcHtTqOiVVhehO;
			bool[] buttonValues = P_0.buttonValues;
			for (int i = 0; i < 5; i++)
			{
				buttonValues[i] = fMpldWGEqXDslzcFqELdjdLkbSXKA[i] || VAnVUaKYNgtTlgQfUCPULNdLYKzv[i];
			}
			WpEtIfVqHSHRnJsLajMiinWkqmZHc();
		}

		public void qQOFxbkqvKSpujWbEMyIxdBRhqdY()
		{
			WpEtIfVqHSHRnJsLajMiinWkqmZHc();
		}

		private void WpEtIfVqHSHRnJsLajMiinWkqmZHc()
		{
			if (tFcotkmLiMBlfWOHnWwqFRBZcfVp != ReInput.absFrame)
			{
				WBdFuLEQlneJudZimZWfOjWJPwGxA();
				tFcotkmLiMBlfWOHnWwqFRBZcfVp = ReInput.absFrame;
			}
		}

		public void ZwoaoGfpCLdZfmktQdhBnWzYiaUU()
		{
			mgOUDaaKmMMIGVRKXzrMaNCGbJyb = 0f;
			PKWflsvMWzXBCVKeefwSArrpFFVDA = 0f;
			lflBUMPmEwNvkJoxWQkiGghTjVVr = 0u;
			tSMKCMblpTdWIYRFAfaEdfhtALdAA = FhAZaChLWjMPTloxUEjbHTLOaeLMA.MoveRelative;
			DzztwrrKkniCFZDsqmovzQwMentM = 0f;
			tlsuGOnVfPEcPsvcHtTqOiVVhehO = 0f;
			Array.Clear(fMpldWGEqXDslzcFqELdjdLkbSXKA, 0, 5);
			Array.Clear(VAnVUaKYNgtTlgQfUCPULNdLYKzv, 0, 5);
			lNWpqFAFpZhMoiUkVfPAcGhhiyLpc = false;
		}

		public void WBdFuLEQlneJudZimZWfOjWJPwGxA()
		{
			mgOUDaaKmMMIGVRKXzrMaNCGbJyb = 0f;
			PKWflsvMWzXBCVKeefwSArrpFFVDA = 0f;
			DzztwrrKkniCFZDsqmovzQwMentM = 0f;
			tlsuGOnVfPEcPsvcHtTqOiVVhehO = 0f;
			Array.Clear(VAnVUaKYNgtTlgQfUCPULNdLYKzv, 0, 5);
		}

		private void uehXKivMzXIXEfFKLjdCcDEGIBgYA(int P_0, int P_1, int P_2, int P_3)
		{
			dKUUFViHIjcmhpmfssagCwcXdjnG dKUUFViHIjcmhpmfssagCwcXdjnG2 = qhuQIGEboDhLdMYCYDAADaosfLUj(P_1, P_2, P_3);
			if (fMpldWGEqXDslzcFqELdjdLkbSXKA[P_0])
			{
				if (dKUUFViHIjcmhpmfssagCwcXdjnG2 == dKUUFViHIjcmhpmfssagCwcXdjnG.Up || dKUUFViHIjcmhpmfssagCwcXdjnG2 == dKUUFViHIjcmhpmfssagCwcXdjnG.DownAndUp)
				{
					fMpldWGEqXDslzcFqELdjdLkbSXKA[P_0] = false;
				}
			}
			else if (dKUUFViHIjcmhpmfssagCwcXdjnG2 == dKUUFViHIjcmhpmfssagCwcXdjnG.Down)
			{
				fMpldWGEqXDslzcFqELdjdLkbSXKA[P_0] = true;
			}
			if (dKUUFViHIjcmhpmfssagCwcXdjnG2 == dKUUFViHIjcmhpmfssagCwcXdjnG.Down || dKUUFViHIjcmhpmfssagCwcXdjnG2 == dKUUFViHIjcmhpmfssagCwcXdjnG.DownAndUp)
			{
				VAnVUaKYNgtTlgQfUCPULNdLYKzv[P_0] = true;
			}
		}

		private static dKUUFViHIjcmhpmfssagCwcXdjnG qhuQIGEboDhLdMYCYDAADaosfLUj(int P_0, int P_1, int P_2)
		{
			if ((P_0 & P_1) == P_1)
			{
				if ((P_0 & P_2) == P_2)
				{
					return dKUUFViHIjcmhpmfssagCwcXdjnG.DownAndUp;
				}
				return dKUUFViHIjcmhpmfssagCwcXdjnG.Down;
			}
			if ((P_0 & P_2) == P_2)
			{
				return dKUUFViHIjcmhpmfssagCwcXdjnG.Up;
			}
			return dKUUFViHIjcmhpmfssagCwcXdjnG.None;
		}

		private static bool spsceRdMKawDBvrYMHjfQNfNHXkdA(IntPtr P_0)
		{
			if (KQKvYsAXvDlLWOZXkMKdMDaTTekW.KOEHysroCvnEYNZMRadPGuReGWG(0u, false, 0u) == IntPtr.Zero)
			{
				return false;
			}
			if (!KQKvYsAXvDlLWOZXkMKdMDaTTekW.ndStfWCYGrhpSTQJPUkyXjWuBHdQ(P_0, out var yvmnbQjDLoRvQsjOUsVFFNnaiTOB2))
			{
				return false;
			}
			if (!KQKvYsAXvDlLWOZXkMKdMDaTTekW.hHjhOsyLyNAQiawMuPOolvbsLfcf(out var yvmnbQjDLoRvQsjOUsVFFNnaiTOB3))
			{
				return false;
			}
			if (!KQKvYsAXvDlLWOZXkMKdMDaTTekW.mytFSopRSfKfvGaFVUwryvZAqNbb(P_0, out var fhzmgqLvPwRrSigefCPuEOcUnAmcb2))
			{
				return false;
			}
			int num = yvmnbQjDLoRvQsjOUsVFFNnaiTOB3.pQUYiDmfqOIIIFDWimiWGFIEDcYBA - yvmnbQjDLoRvQsjOUsVFFNnaiTOB2.pQUYiDmfqOIIIFDWimiWGFIEDcYBA;
			int num2 = yvmnbQjDLoRvQsjOUsVFFNnaiTOB3.IrEWHwkTsoQOYQMjMYfagacSjWSfA - yvmnbQjDLoRvQsjOUsVFFNnaiTOB2.IrEWHwkTsoQOYQMjMYfagacSjWSfA;
			if (num >= 0 && num2 >= 0 && num <= fhzmgqLvPwRrSigefCPuEOcUnAmcb2.VCwSAQIAAZOyEiaiTdjZwkMGEapeA && num2 <= fhzmgqLvPwRrSigefCPuEOcUnAmcb2.IlOFHGYzaFGSHkNVxEtTghwfWbbn)
			{
				return false;
			}
			if (!KQKvYsAXvDlLWOZXkMKdMDaTTekW.SFUuKbqeTiYalvkzspirFNRgYeht(P_0, out var fhzmgqLvPwRrSigefCPuEOcUnAmcb3))
			{
				return false;
			}
			if (yvmnbQjDLoRvQsjOUsVFFNnaiTOB3.pQUYiDmfqOIIIFDWimiWGFIEDcYBA >= fhzmgqLvPwRrSigefCPuEOcUnAmcb3.auXzLUrxBshUUbWkgtjUHdCaHYwZ && yvmnbQjDLoRvQsjOUsVFFNnaiTOB3.pQUYiDmfqOIIIFDWimiWGFIEDcYBA <= fhzmgqLvPwRrSigefCPuEOcUnAmcb3.VCwSAQIAAZOyEiaiTdjZwkMGEapeA && yvmnbQjDLoRvQsjOUsVFFNnaiTOB3.IrEWHwkTsoQOYQMjMYfagacSjWSfA >= fhzmgqLvPwRrSigefCPuEOcUnAmcb3.mvqTDesmPadnzSZIThuCfdynXRDx)
			{
				return yvmnbQjDLoRvQsjOUsVFFNnaiTOB3.IrEWHwkTsoQOYQMjMYfagacSjWSfA <= fhzmgqLvPwRrSigefCPuEOcUnAmcb3.IlOFHGYzaFGSHkNVxEtTghwfWbbn;
			}
			return false;
		}
	}

	private class hXPLmqkvQTBPfYanxiNCdDvTKtXA
	{
		private bool TUlWGCcqoallMGQlFaoezBFHyYBu;

		private bool HtisTKOcCsfrfKQwvqdjSwrppHqv;

		private bool hSrldBAXZBclFAbosAWVBcxnKtwDb;

		private int WkoqljIXSLhkoslkaNYHiosCDzYG = 10;

		private readonly float hfZTxaMwCzMfBMJigxJsbCHAIRTg;

		private double PJVGyuhAkgGGIFkQbiUvwLkSUmxbc;

		public bool osxuHJLGztVOPPrcWlIKNZhkiYrA => TUlWGCcqoallMGQlFaoezBFHyYBu;

		public bool xkNaqdFdEAotuSjjHhSVIfbXkZcS => HtisTKOcCsfrfKQwvqdjSwrppHqv;

		public hXPLmqkvQTBPfYanxiNCdDvTKtXA(bool P_0, float P_1)
		{
			TUlWGCcqoallMGQlFaoezBFHyYBu = P_0;
			hfZTxaMwCzMfBMJigxJsbCHAIRTg = P_1;
			RmVvUpuaGNImJeyJvUtErmXYjMBM(false);
		}

		public void locPUbSDnRLFXCOnpiQHbhVxHmNF()
		{
			if (TUlWGCcqoallMGQlFaoezBFHyYBu && !(ReInput.realTime < PJVGyuhAkgGGIFkQbiUvwLkSUmxbc))
			{
				RmVvUpuaGNImJeyJvUtErmXYjMBM(true);
			}
		}

		private void RmVvUpuaGNImJeyJvUtErmXYjMBM(bool P_0)
		{
			if (hSrldBAXZBclFAbosAWVBcxnKtwDb)
			{
				KQKvYsAXvDlLWOZXkMKdMDaTTekW.NIauLqJqhqWSuJcndqOblvtsshCS(112u, 0u, ref WkoqljIXSLhkoslkaNYHiosCDzYG, 0u);
			}
			HtisTKOcCsfrfKQwvqdjSwrppHqv = KQKvYsAXvDlLWOZXkMKdMDaTTekW.ajBOvwTePqLBYFJfsEPHxqyplzfk(gbkwoNGXbnemmwRJGohrxZfefZAp.sMigHsQNANbMFDXafRWkUhmuACmdA) > 0;
			if (P_0)
			{
				PJVGyuhAkgGGIFkQbiUvwLkSUmxbc = ReInput.realTime + (double)hfZTxaMwCzMfBMJigxJsbCHAIRTg;
			}
		}
	}

	private readonly SpinLock pKWaqpJYeAAHAjTBmCwrWoYpjqSFb = new SpinLock();

	private UpdateLoopDataSet<aFwiLZbLHZUQheVKOeYsNiyaicgRA> ZnEsFZPMDaARrCvKOnIJlAUAmcfAA;

	private HardwareControllerMap_Game DYobFEvFQMMMGYWbZEPiYLdIPVxG;

	private hXPLmqkvQTBPfYanxiNCdDvTKtXA amsaaBGBpaOzMrBqZIlDKkIhbQDR;

	private bool dJwKSQBUnbuEYUOTpCPFHqzEVGfw;

	private int WtKeyXaeskUbYGljzGrutMUWngdHb;

	private bool FsCMBlZvjSWMbRpUOddEakapMEwc;

	private bool xldZRNhXVCCTrSMoGRGgtHoSOwKB;

	bool IGetSetEnabled.enabled
	{
		get
		{
			return FsCMBlZvjSWMbRpUOddEakapMEwc;
		}
		set
		{
			if (FsCMBlZvjSWMbRpUOddEakapMEwc != value)
			{
				FsCMBlZvjSWMbRpUOddEakapMEwc = value;
				Clear();
				ThreadSafeUnityInput.mouse.Monitor(value);
			}
		}
	}

	InputSource IUnifiedMouseSource.inputSource => InputSource.RawInput;

	HardwareControllerMap_Game IUnifiedMouseSource.hardwareMap
	{
		get
		{
			if (DYobFEvFQMMMGYWbZEPiYLdIPVxG == null)
			{
				DYobFEvFQMMMGYWbZEPiYLdIPVxG = taAwuhWsGhmbWaMtPcjVIVFYVvBUA();
			}
			return DYobFEvFQMMMGYWbZEPiYLdIPVxG;
		}
	}

	int IUnifiedMouseSource.buttonCount => 5;

	int IUnifiedMouseSource.axisCount => 4;

	Vector2 IUnifiedMouseSource.mousePosition
	{
		get
		{
			if (!FsCMBlZvjSWMbRpUOddEakapMEwc)
			{
				return default(Vector2);
			}
			return ThreadSafeUnityInput.mouse.mousePosition;
		}
	}

	Controller.Extension IUnifiedMouseSource.controllerExtension => null;

	public dXKLvQGSIvoiwbPpSGgTOgwmLqzq(UpdateLoopSetting P_0)
	{
		KAOMaRNZIbumheFkOhsIAzLBicbB();
		amsaaBGBpaOzMrBqZIlDKkIhbQDR = new hXPLmqkvQTBPfYanxiNCdDvTKtXA(true, 2f);
		ZnEsFZPMDaARrCvKOnIJlAUAmcfAA = new UpdateLoopDataSet<aFwiLZbLHZUQheVKOeYsNiyaicgRA>(P_0);
		using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
		{
			List<UpdateLoopType> list = tList.list;
			EnumConverter.ToUpdateLoopTypes(P_0, list);
			for (int i = 0; i < list.Count; i++)
			{
				ZnEsFZPMDaARrCvKOnIJlAUAmcfAA[i] = new aFwiLZbLHZUQheVKOeYsNiyaicgRA(amsaaBGBpaOzMrBqZIlDKkIhbQDR, list[i]);
			}
		}
		dJwKSQBUnbuEYUOTpCPFHqzEVGfw = ReInput.IsInputAllowed(ControllerType.Mouse);
		ReInput.ApplicationFocusChangedEvent += ejapGPBVPVOAerwwegoreYYgUEJi;
		ReInput.ApplicationPauseChangedEvent += AohEPXSpZBNriMLataMeAffzdjrdA;
		Rewired_002EInterfaces_002EIGetSetEnabled_002Eenabled = true;
		ReInput.EditorPauseChangedEvent += hNsWJcVLlZxDkojhsBswhXXwmSMRA;
		ReInput.TimeScalePauseChangedEvent += VbgFquPjNBUeLAQjzPqWLDLNhUDfA;
		ReInput.UpdateEndedEvent += IIDvTXqvusePWsMcUPwLLFJFGjfT;
	}

	public void tMAcjHTSzHASJIFuUESIMlFqxFZG(UpdateLoopType P_0)
	{
		ZnEsFZPMDaARrCvKOnIJlAUAmcfAA.SetUpdateLoop(P_0);
		amsaaBGBpaOzMrBqZIlDKkIhbQDR.locPUbSDnRLFXCOnpiQHbhVxHmNF();
		dJwKSQBUnbuEYUOTpCPFHqzEVGfw = ReInput.IsInputAllowed(ControllerType.Mouse);
	}

	public void TqlmtfFIKIadmHyzsefIBoShfGsjA(ZpILADRRhNkXwxVGHEqPQQWInAbm P_0)
	{
		if (!dJwKSQBUnbuEYUOTpCPFHqzEVGfw)
		{
			return;
		}
		using (pKWaqpJYeAAHAjTBmCwrWoYpjqSFb.Lock())
		{
			int count = ZnEsFZPMDaARrCvKOnIJlAUAmcfAA.Count;
			for (int i = 0; i < count; i++)
			{
				ZnEsFZPMDaARrCvKOnIJlAUAmcfAA[i].PluVROyMoViAefKSURiKvMwXvgxX(P_0);
			}
		}
	}

	public void rDskzhYJFjDkszhEKpFDFrqwFfNBA(bool P_0)
	{
		oyUINEKLsOCnMElDSIcOPwzAhUyLA();
	}

	public void pAfbVNMeJdAnESsUiBQJRIykXZdP(bool P_0)
	{
		if (KAOMaRNZIbumheFkOhsIAzLBicbB() < 0)
		{
			oyUINEKLsOCnMElDSIcOPwzAhUyLA();
		}
	}

	private int KAOMaRNZIbumheFkOhsIAzLBicbB()
	{
		int wtKeyXaeskUbYGljzGrutMUWngdHb = WtKeyXaeskUbYGljzGrutMUWngdHb;
		if (AuSBfxYAktMaNvbYMEDVwcjrEcXEA.jcUKHrXzwvhrUwFWsunqrhEyggBH(wgGDDSDavYPeDuPlcdwfHLdGaNoSA.Mouse, out var wtKeyXaeskUbYGljzGrutMUWngdHb2))
		{
			WtKeyXaeskUbYGljzGrutMUWngdHb = wtKeyXaeskUbYGljzGrutMUWngdHb2;
		}
		else
		{
			WtKeyXaeskUbYGljzGrutMUWngdHb = ((KQKvYsAXvDlLWOZXkMKdMDaTTekW.ajBOvwTePqLBYFJfsEPHxqyplzfk(gbkwoNGXbnemmwRJGohrxZfefZAp.vcbcOpjbKpSUOlgMXZDTOyJPuyCi) != 0) ? 1 : 0);
		}
		return WtKeyXaeskUbYGljzGrutMUWngdHb - wtKeyXaeskUbYGljzGrutMUWngdHb;
	}

	private void ejapGPBVPVOAerwwegoreYYgUEJi(bool P_0)
	{
		dJwKSQBUnbuEYUOTpCPFHqzEVGfw = ReInput.IsInputAllowed(ControllerType.Mouse);
		if (!P_0 && !dJwKSQBUnbuEYUOTpCPFHqzEVGfw)
		{
			oyUINEKLsOCnMElDSIcOPwzAhUyLA();
		}
	}

	private void AohEPXSpZBNriMLataMeAffzdjrdA(bool P_0)
	{
		dJwKSQBUnbuEYUOTpCPFHqzEVGfw = ReInput.IsInputAllowed(ControllerType.Mouse);
		if (!dJwKSQBUnbuEYUOTpCPFHqzEVGfw)
		{
			oyUINEKLsOCnMElDSIcOPwzAhUyLA();
		}
	}

	private void hNsWJcVLlZxDkojhsBswhXXwmSMRA(bool P_0)
	{
	}

	private void VbgFquPjNBUeLAQjzPqWLDLNhUDfA(bool P_0)
	{
		if ((ReInput.configVars.updateLoop & UpdateLoopSetting.FixedUpdate) == 0)
		{
			return;
		}
		dJwKSQBUnbuEYUOTpCPFHqzEVGfw = ReInput.IsInputAllowed(ControllerType.Mouse);
		using (pKWaqpJYeAAHAjTBmCwrWoYpjqSFb.Lock())
		{
			ZnEsFZPMDaARrCvKOnIJlAUAmcfAA[ZnEsFZPMDaARrCvKOnIJlAUAmcfAA.fixedUpdateSetIndex].WBdFuLEQlneJudZimZWfOjWJPwGxA();
		}
	}

	private void IIDvTXqvusePWsMcUPwLLFJFGjfT(UpdateLoopType P_0)
	{
		using (pKWaqpJYeAAHAjTBmCwrWoYpjqSFb.Lock())
		{
			ZnEsFZPMDaARrCvKOnIJlAUAmcfAA.Get(P_0).qQOFxbkqvKSpujWbEMyIxdBRhqdY();
		}
	}

	private void oyUINEKLsOCnMElDSIcOPwzAhUyLA()
	{
		using (pKWaqpJYeAAHAjTBmCwrWoYpjqSFb.Lock())
		{
			int count = ZnEsFZPMDaARrCvKOnIJlAUAmcfAA.Count;
			for (int i = 0; i < count; i++)
			{
				ZnEsFZPMDaARrCvKOnIJlAUAmcfAA[i].ZwoaoGfpCLdZfmktQdhBnWzYiaUU();
			}
		}
	}

	public void UpdateInputData(ControllerDataUpdater dataUpdater)
	{
		ZnEsFZPMDaARrCvKOnIJlAUAmcfAA.Current.SojpnaSqvJIbxmYcwhTUUPFDjuDM(dataUpdater);
	}

	void IUnifiedMouseSource.UpdateInputData(ControllerDataUpdater dataUpdater)
	{
		//ILSpy generated this explicit interface implementation from .override directive in UpdateInputData
		this.UpdateInputData(dataUpdater);
	}

	public void Clear()
	{
		oyUINEKLsOCnMElDSIcOPwzAhUyLA();
	}

	void IUnifiedMouseSource.Clear()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Clear
		this.Clear();
	}

	private HardwareControllerMap_Game taAwuhWsGhmbWaMtPcjVIVFYVvBUA()
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
		MJgFIGITETlGCGKtnSCBAiftELavA(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void QRPVDAkzjrnnhtRxPRzrycBjlClc()
	{
		try
		{
			MJgFIGITETlGCGKtnSCBAiftELavA(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void MJgFIGITETlGCGKtnSCBAiftELavA(bool P_0)
	{
		if (!xldZRNhXVCCTrSMoGRGgtHoSOwKB)
		{
			ReInput.ApplicationFocusChangedEvent -= ejapGPBVPVOAerwwegoreYYgUEJi;
			ReInput.ApplicationPauseChangedEvent -= AohEPXSpZBNriMLataMeAffzdjrdA;
			ReInput.EditorPauseChangedEvent -= hNsWJcVLlZxDkojhsBswhXXwmSMRA;
			ReInput.TimeScalePauseChangedEvent -= VbgFquPjNBUeLAQjzPqWLDLNhUDfA;
			ReInput.UpdateEndedEvent -= IIDvTXqvusePWsMcUPwLLFJFGjfT;
			if (P_0 && FsCMBlZvjSWMbRpUOddEakapMEwc)
			{
				ThreadSafeUnityInput.mouse.Monitor(state: false);
			}
			xldZRNhXVCCTrSMoGRGgtHoSOwKB = true;
		}
	}
}

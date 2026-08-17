using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal class IgUVpafhmNYDnMTcltiGEUbuIaMm
{
	public class fVxIHACgwelFbfkPEhTOyLPoSJOI
	{
		public readonly Action<InputActionEventData> pzQkhbFpUZlBUwLBtSVoiiHpSsry;

		public readonly UpdateLoopType TVQxfuhRqKwpUmWrMwjRlqhFhvJbA;

		public readonly InputActionEventType FctibpOdqbGoHdZtAXyrxQRmOczv;

		public readonly int cFozRXDWstpmUmPjQCPSELExAPExA;

		public readonly bool hGIIfWEGJabGNAZKrYffJtwonzeQA;

		public float[] pIbjHiOXxbPJQdZnwOUhvcbbxyC;

		public fVxIHACgwelFbfkPEhTOyLPoSJOI(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, int P_3, object[] P_4)
		{
			TVQxfuhRqKwpUmWrMwjRlqhFhvJbA = P_1;
			FctibpOdqbGoHdZtAXyrxQRmOczv = P_2;
			cFozRXDWstpmUmPjQCPSELExAPExA = P_3;
			pzQkhbFpUZlBUwLBtSVoiiHpSsry = P_0;
			CMhLaEpfUbUqGurqVhWenvbJPeRB(P_4);
			switch (P_2)
			{
			case InputActionEventType.Update:
			case InputActionEventType.ButtonUnpressed:
			case InputActionEventType.NegativeButtonUnpressed:
			case InputActionEventType.AxisInactive:
			case InputActionEventType.AxisRawInactive:
				hGIIfWEGJabGNAZKrYffJtwonzeQA = true;
				break;
			}
		}

		public bool jUKQuhBGbzGtcVodtnViEDrMQEBx(int P_0, out float P_1)
		{
			if (pIbjHiOXxbPJQdZnwOUhvcbbxyC == null || pIbjHiOXxbPJQdZnwOUhvcbbxyC.Length <= P_0)
			{
				P_1 = 0f;
				return false;
			}
			P_1 = pIbjHiOXxbPJQdZnwOUhvcbbxyC[P_0];
			return true;
		}

		private void CMhLaEpfUbUqGurqVhWenvbJPeRB(object[] P_0)
		{
			switch (FctibpOdqbGoHdZtAXyrxQRmOczv)
			{
			case InputActionEventType.ButtonPressedForTime:
			case InputActionEventType.ButtonPressedForTimeJustReleased:
			case InputActionEventType.NegativeButtonPressedForTime:
			case InputActionEventType.NegativeButtonPressedForTimeJustReleased:
				if (P_0 == null || P_0.Length < 1)
				{
					throw new Exception("Wrong number of arguments passed for Input event type \"" + FctibpOdqbGoHdZtAXyrxQRmOczv.ToString() + "\". 1 required argument: time [float], 1 optional argument: expireIn [float]");
				}
				pIbjHiOXxbPJQdZnwOUhvcbbxyC = new float[2];
				if (P_0[0] is float)
				{
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[0] = (float)P_0[0];
				}
				else
				{
					if (!(P_0[0] is int))
					{
						throw new Exception("Wrong argument type passed for Input event type \"" + FctibpOdqbGoHdZtAXyrxQRmOczv.ToString() + "\". Argument 0: time [float]");
					}
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[0] = (int)P_0[0];
				}
				if (P_0.Length <= 1)
				{
					break;
				}
				if (P_0[1] is float)
				{
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[1] = (float)P_0[1];
					break;
				}
				if (P_0[1] is int)
				{
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[1] = (int)P_0[1];
					break;
				}
				throw new Exception("Wrong argument type passed for Input event type \"" + FctibpOdqbGoHdZtAXyrxQRmOczv.ToString() + "\". Argument 1 (optional): expireIn [float]");
			case InputActionEventType.ButtonJustPressedForTime:
			case InputActionEventType.NegativeButtonJustPressedForTime:
				if (P_0 == null || P_0.Length < 1)
				{
					throw new Exception("Wrong number of arguments passed for Input event type \"" + FctibpOdqbGoHdZtAXyrxQRmOczv.ToString() + "\". Requires 1 argument: time [float]");
				}
				pIbjHiOXxbPJQdZnwOUhvcbbxyC = new float[1];
				if (P_0[0] is float)
				{
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[0] = (float)P_0[0];
					break;
				}
				if (P_0[0] is int)
				{
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[0] = (int)P_0[0];
					break;
				}
				throw new Exception("Wrong argument type passed for Input event type \"" + FctibpOdqbGoHdZtAXyrxQRmOczv.ToString() + "\". Argument 0: time [float]");
			case InputActionEventType.ButtonDoublePressed:
			case InputActionEventType.ButtonJustDoublePressed:
			case InputActionEventType.NegativeButtonDoublePressed:
			case InputActionEventType.NegativeButtonJustDoublePressed:
			case InputActionEventType.ButtonDoublePressJustReleased:
			case InputActionEventType.NegativeButtonDoublePressJustReleased:
				if (P_0 == null || P_0.Length < 1)
				{
					break;
				}
				pIbjHiOXxbPJQdZnwOUhvcbbxyC = new float[1];
				if (P_0[0] is float)
				{
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[0] = (float)P_0[0];
					break;
				}
				if (P_0[0] is int)
				{
					pIbjHiOXxbPJQdZnwOUhvcbbxyC[0] = (int)P_0[0];
					break;
				}
				throw new Exception("Wrong argument type passed for Input event type \"" + FctibpOdqbGoHdZtAXyrxQRmOczv.ToString() + "\". Argument 0 (optional): time [float]");
			}
		}
	}

	[Serializable]
	private sealed class sZANlzeAYMVvcYIaHMDZQBLHoUxq
	{
		public static readonly sZANlzeAYMVvcYIaHMDZQBLHoUxq _003C_003E9 = new sZANlzeAYMVvcYIaHMDZQBLHoUxq();

		public static Func<AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>> _003C_003E9__8_0;

		internal AList<fVxIHACgwelFbfkPEhTOyLPoSJOI> xHlYKmiskSCMbJPwUnozrIeTrhuf()
		{
			return new AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>();
		}
	}

	private sealed class tEfdZWKpDDnXrnIMlzQHQpVTcfHf
	{
		public Action<InputActionEventData> ikittpeKFLmNjQXxpiAceAVmRevC;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> BYBTtnrhPegZiHjcTJXHDHnwfxYb;

		internal bool XqsftdGaOKhnLJxUKHTpmnCLuPQIA(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			return P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == ikittpeKFLmNjQXxpiAceAVmRevC;
		}
	}

	private sealed class GUugsfLXBONmYchHOhPKjbAPHucOA
	{
		public Action<InputActionEventData> dJedEKvZAYBCgVpLjgWlhbmaKWFYA;

		public int jWpwvVExksnjJmoNvPVrOzGgWdER;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> FruoZGMTTlpQESNlsJiVlylyaEWN;

		internal bool mJZSynxWpelWNlBwUCobAakJGypkA(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			if (P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == dJedEKvZAYBCgVpLjgWlhbmaKWFYA)
			{
				return P_0.cFozRXDWstpmUmPjQCPSELExAPExA == jWpwvVExksnjJmoNvPVrOzGgWdER;
			}
			return false;
		}
	}

	private sealed class pjJxKDXilLompUMjsdrUANexBplxA
	{
		public Action<InputActionEventData> JpWuddCQjgzKTbVZBKHxFZUPifnT;

		public UpdateLoopType NFoGAKrBzbBMGfCWcMsMcFAcKGuTb;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> OcTypHesqdUpIGBitkgvadStBCrb;

		internal bool dNerLKlpMsAdlbgUTEtcPlDLedkeb(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			if (P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == JpWuddCQjgzKTbVZBKHxFZUPifnT)
			{
				return P_0.TVQxfuhRqKwpUmWrMwjRlqhFhvJbA == NFoGAKrBzbBMGfCWcMsMcFAcKGuTb;
			}
			return false;
		}
	}

	private sealed class tRrZpctwIAiStwrCfmiUTYhFQZIJ
	{
		public Action<InputActionEventData> eUNYqorMIrreTilChiQbjkeuDunk;

		public InputActionEventType IgtDFqpVLBIqRIaPGQnhnFpUdqAKA;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> TKZbLTHfVthhdeWDyURnJpsThGPkA;

		internal bool ZqKGyaEcifprzMvLHIymMvioImFfb(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			if (P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == eUNYqorMIrreTilChiQbjkeuDunk)
			{
				return P_0.FctibpOdqbGoHdZtAXyrxQRmOczv == IgtDFqpVLBIqRIaPGQnhnFpUdqAKA;
			}
			return false;
		}
	}

	private sealed class zekAWKDbxgVGPJnciAUbEAdIShczB
	{
		public Action<InputActionEventData> fpDSYrwcJGUuNVHbZGkRDaEXYbdW;

		public UpdateLoopType SgVMNoBJBYroqIWANFJrvGfWhXUbA;

		public int SnOkprltCPmGZIbEzIYvAFDrpmFiA;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> YHFVKMpkZTPLBhPLXHmuWwkeGXjL;

		internal bool OVHOStUNsPAbNeMvyLxkXhQNIMoFA(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			if (P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == fpDSYrwcJGUuNVHbZGkRDaEXYbdW && P_0.TVQxfuhRqKwpUmWrMwjRlqhFhvJbA == SgVMNoBJBYroqIWANFJrvGfWhXUbA)
			{
				return P_0.cFozRXDWstpmUmPjQCPSELExAPExA == SnOkprltCPmGZIbEzIYvAFDrpmFiA;
			}
			return false;
		}
	}

	private sealed class aPhokcXMoXGKjvVBYFdFpQaptlhe
	{
		public Action<InputActionEventData> qghcPiVjsKyuVArJTIoDvpyeVuLR;

		public UpdateLoopType jUnBsXTSJNGSfjlaIKrFDUEQYBSq;

		public int HMTGWeIgOboOfnbBhsMyjNvZjgXw;

		public InputActionEventType gFeFVYQADXEgIcxNwCPOrIzNNJgT;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> GDBGglbWpgGDftFzFFNjVySiCQzx;

		internal bool mckdYFwzPQhKCTkkbjZafEzeaJEfb(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			if (P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == qghcPiVjsKyuVArJTIoDvpyeVuLR && P_0.TVQxfuhRqKwpUmWrMwjRlqhFhvJbA == jUnBsXTSJNGSfjlaIKrFDUEQYBSq && P_0.cFozRXDWstpmUmPjQCPSELExAPExA == HMTGWeIgOboOfnbBhsMyjNvZjgXw)
			{
				return P_0.FctibpOdqbGoHdZtAXyrxQRmOczv == gFeFVYQADXEgIcxNwCPOrIzNNJgT;
			}
			return false;
		}
	}

	private sealed class ZKJbTXBqhdepEyiVmBaqjeAqoajK
	{
		public Action<InputActionEventData> cQpcfvavpsYtyNcCmAKKCJOwvVoH;

		public UpdateLoopType LxIERxAZWurOdzXHKScQrehoNEnU;

		public InputActionEventType eiBvSnUkPcdrUkDyDiyKkMvRBXXFb;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> nGyuxorpTXFSFZglKYZBYFwLAWss;

		internal bool BtTUnvkgEJABVaQfhTBhOCijtpkd(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			if (P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == cQpcfvavpsYtyNcCmAKKCJOwvVoH && P_0.TVQxfuhRqKwpUmWrMwjRlqhFhvJbA == LxIERxAZWurOdzXHKScQrehoNEnU)
			{
				return P_0.FctibpOdqbGoHdZtAXyrxQRmOczv == eiBvSnUkPcdrUkDyDiyKkMvRBXXFb;
			}
			return false;
		}
	}

	private sealed class kGImCLDmOdljFMRVsbFEjdbTRqIX
	{
		public Action<InputActionEventData> FAgNycjPzvYzZcsdSkZXLTgaskJk;

		public int hGzgSkMgYGcDTqBHQdnTJytbldFi;

		public InputActionEventType szIDyrIVDIhEhYVLsqpUeaegGPhm;

		public Predicate<fVxIHACgwelFbfkPEhTOyLPoSJOI> vyEUDAFrboBSrivgXjelXJtAuLuR;

		internal bool pHyVOhtHGpGOiCLrFkIiDAfaobZLA(fVxIHACgwelFbfkPEhTOyLPoSJOI P_0)
		{
			if (P_0.pzQkhbFpUZlBUwLBtSVoiiHpSsry == FAgNycjPzvYzZcsdSkZXLTgaskJk && P_0.cFozRXDWstpmUmPjQCPSELExAPExA == hGzgSkMgYGcDTqBHQdnTJytbldFi)
			{
				return P_0.FctibpOdqbGoHdZtAXyrxQRmOczv == szIDyrIVDIhEhYVLsqpUeaegGPhm;
			}
			return false;
		}
	}

	private static fVxIHACgwelFbfkPEhTOyLPoSJOI[] hEmmHqeOANXKfxKcCtArhCRBzpHJ;

	private bool gZTFZYHVOpEHVZIjCujQUszHUKyVA;

	private AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] ibKLptlPekJIJfmHLaVRPZLSncpv;

	private int[] cKTNQkLtjvSgCrsTHaOGbzANHbqx;

	private int AkzESxItwAkCdByFhhPZuIKSgFqA;

	public int rbDgJJKPZnOfiISIIFHbhZQEItGmD;

	static IgUVpafhmNYDnMTcltiGEUbuIaMm()
	{
		hEmmHqeOANXKfxKcCtArhCRBzpHJ = new fVxIHACgwelFbfkPEhTOyLPoSJOI[100];
	}

	private void SYrvsWkcewIuFoKjBuiJnsmoEurE()
	{
		if (!gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			IList<InputAction> list = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.oNXBuTCpMYRLGCcxGUYgjnxaGNBWA;
			int num = list?.Count ?? 0;
			ibKLptlPekJIJfmHLaVRPZLSncpv = new AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[num + 1];
			cKTNQkLtjvSgCrsTHaOGbzANHbqx = new int[ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.fyMfSUcvzcoChAfDspsaDySQPCgg + 1];
			ArrayTools.Populate(ibKLptlPekJIJfmHLaVRPZLSncpv, 0, ibKLptlPekJIJfmHLaVRPZLSncpv.Length, sZANlzeAYMVvcYIaHMDZQBLHoUxq._003C_003E9.xHlYKmiskSCMbJPwUnozrIeTrhuf);
			for (int i = 0; i < num; i++)
			{
				cKTNQkLtjvSgCrsTHaOGbzANHbqx[list[i].id] = i;
			}
			AkzESxItwAkCdByFhhPZuIKSgFqA = num;
			gZTFZYHVOpEHVZIjCujQUszHUKyVA = true;
		}
	}

	public void exZDkojriUFuDCBsAcnVVOPGfVvM(kBOilrfmQspwwsLlQucgVePHzaAKA P_0, UpdateLoopType P_1)
	{
		AList<fVxIHACgwelFbfkPEhTOyLPoSJOI> aList = ibKLptlPekJIJfmHLaVRPZLSncpv[cKTNQkLtjvSgCrsTHaOGbzANHbqx[P_0.lmTomMKaHBVviiiQJDambxGMGPNi]];
		for (int i = 0; i < 2; i++)
		{
			if (i == 1)
			{
				aList = ibKLptlPekJIJfmHLaVRPZLSncpv[AkzESxItwAkCdByFhhPZuIKSgFqA];
			}
			int count = aList._count;
			if (hEmmHqeOANXKfxKcCtArhCRBzpHJ.Length < count)
			{
				hEmmHqeOANXKfxKcCtArhCRBzpHJ = new fVxIHACgwelFbfkPEhTOyLPoSJOI[count + 50];
			}
			if (count > 0)
			{
				Array.Copy(aList._items, hEmmHqeOANXKfxKcCtArhCRBzpHJ, count);
			}
			for (int j = 0; j < count; j++)
			{
				fVxIHACgwelFbfkPEhTOyLPoSJOI fVxIHACgwelFbfkPEhTOyLPoSJOI2 = hEmmHqeOANXKfxKcCtArhCRBzpHJ[j];
				if (fVxIHACgwelFbfkPEhTOyLPoSJOI2 == null || (!P_0.FbmYYIxnimAqWOuOBHZURWLwKrkn && !fVxIHACgwelFbfkPEhTOyLPoSJOI2.hGIIfWEGJabGNAZKrYffJtwonzeQA) || fVxIHACgwelFbfkPEhTOyLPoSJOI2.TVQxfuhRqKwpUmWrMwjRlqhFhvJbA != P_1 || (fVxIHACgwelFbfkPEhTOyLPoSJOI2.cFozRXDWstpmUmPjQCPSELExAPExA >= 0 && fVxIHACgwelFbfkPEhTOyLPoSJOI2.cFozRXDWstpmUmPjQCPSELExAPExA != P_0.lmTomMKaHBVviiiQJDambxGMGPNi))
				{
					continue;
				}
				bool flag = false;
				switch (fVxIHACgwelFbfkPEhTOyLPoSJOI2.FctibpOdqbGoHdZtAXyrxQRmOczv)
				{
				case InputActionEventType.Update:
					flag = true;
					break;
				case InputActionEventType.ButtonPressed:
					if (P_0.BRgJAcEyxDdRJtEmfVewtjEoYNqt())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonUnpressed:
					if (!P_0.BRgJAcEyxDdRJtEmfVewtjEoYNqt())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonDoublePressed:
				{
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num5);
					if (P_0.eSRbSlRIiNAqSZHieyTWJIkiWXff(num5))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonPressedForTime:
				{
					if (!fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num11))
					{
						continue;
					}
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(1, out var num12);
					if (P_0.jTXtRIsJnPihrseaUxpjqKnCOoPC(num11, num12))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonShortPressed:
					if (P_0.FIsgvKBIeQboRaWNaSyVaektTMzzA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonLongPressed:
					if (P_0.UsNLWxwEABEtvLvDfRUDvXVPnCdx())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustPressed:
					if (P_0.ulNzhMMaXtAXOxcBFAnWyCDHeqoN())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustReleased:
					if (P_0.aOLFHKiGReYtLuVqzyyHLHbfKQYab())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustDoublePressed:
				{
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num9);
					if (P_0.tchZIqxqMcltzqTiihcJYKwuaAei(num9))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonDoublePressJustReleased:
				{
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num6);
					if (P_0.VeFehoBAikgOZbeXFHZCDKEIPZGKC(num6))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonJustPressedForTime:
				{
					if (!fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num4))
					{
						continue;
					}
					if (P_0.kBBCZOnptttxaQSaLliSLAyVIslL(num4))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonJustShortPressed:
					if (P_0.JNUXlbFKdcnOumpOjdJBlERkDRZCA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustLongPressed:
					if (P_0.gOpFTmMabCeWBnCmcrsVKBMyxMQF())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonPressedForTimeJustReleased:
				{
					if (!fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num15))
					{
						continue;
					}
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(1, out var num16);
					if (P_0.RCOCLdDZYLOfkVVZPPcNmYplMPqF(num15, num16))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonShortPressJustReleased:
					if (P_0.lekaYDUvsTYCOiLPOfTfzDVLfDuc())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonLongPressJustReleased:
					if (P_0.jWCwyVMAyGNFusPDQDolSrygEocX())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonRepeating:
					if (P_0.luHCfXeDrbtNBOhpwjjGImBAgXznA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonSinglePressed:
					if (P_0.ZbWxWKzrywKLPNgJCRBudWaWVnQo())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustSinglePressed:
					if (P_0.DzHscFSLxdVERLjLMZVUWfQUTaSF())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonSinglePressJustReleased:
					if (P_0.LZPEdseAeYHhrEFJMyIMBhfZCwvn())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonPressed:
					if (P_0.VRiBjNoimKLqMJcOigBnTGHHrHub())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonUnpressed:
					if (!P_0.VRiBjNoimKLqMJcOigBnTGHHrHub())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonDoublePressed:
				{
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num3);
					if (P_0.VosydDNNmgXAqxlxaeIqKRirlQgdA(num3))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonPressedForTime:
				{
					if (!fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num))
					{
						continue;
					}
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(1, out var num2);
					if (P_0.DpGjKMVCONaAXZmoEjjceWwJoSVj(num, num2))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonShortPressed:
					if (P_0.oMTJrgKTbTdWHoFmJuSgxgShPrm())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonLongPressed:
					if (P_0.QnupUpitKImkVGNVIPtpBxzUcDXf())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustPressed:
					if (P_0.VYSikZzuXqPelkspEaGEPYtFZADJ())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustReleased:
					if (P_0.KOICBttxgehAkePueGwrcVfAlxWCb())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustDoublePressed:
				{
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num14);
					if (P_0.ZnwZnJXiZrPcerRlvjBWaMetofsY(num14))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonDoublePressJustReleased:
				{
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num13);
					if (P_0.xehengRSBDHzLfImHDBStcbEAmlIb(num13))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonJustPressedForTime:
				{
					if (!fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num10))
					{
						continue;
					}
					if (P_0.kEjagsDgZituqfuLKPjZGOHbrQSIA(num10))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonJustShortPressed:
					if (P_0.kslciTTidvoMyIZgtSTdFtuZsUoP())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustLongPressed:
					if (P_0.shokBLMVDXLmddfYxVRvxaLiVrib())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonPressedForTimeJustReleased:
				{
					if (!fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(0, out var num7))
					{
						continue;
					}
					fVxIHACgwelFbfkPEhTOyLPoSJOI2.jUKQuhBGbzGtcVodtnViEDrMQEBx(1, out var num8);
					if (P_0.LVqGPOKOkUJddoPzWyZInGbDACPXA(num7, num8))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonShortPressJustReleased:
					if (P_0.LDMsGESyNgOlMulQqSikAlFjYnii())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonLongPressJustReleased:
					if (P_0.BMXFPjDOFTDTTfHBGMkTmqmNPqEX())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonRepeating:
					if (P_0.uakFyBehSWuTSJarHLAsQDuocrmd())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonSinglePressed:
					if (P_0.ofLgjxbAiLFczqfdeXZQnDffqjaAb())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustSinglePressed:
					if (P_0.WuHTXaSrrQTzEyeAyWMAMFzZWqGR())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonSinglePressJustReleased:
					if (P_0.SyxgdBgGAcWsYgijIiZkjXGkreFJA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisActive:
					if (!MathTools.ApproximatelyZero(P_0.RvRTgrdjWTZRmjoKbsOcGREaiXDi()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisInactive:
					if (MathTools.ApproximatelyZero(P_0.RvRTgrdjWTZRmjoKbsOcGREaiXDi()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisRawActive:
					if (!MathTools.ApproximatelyZero(P_0.IWvqBQApuMVSUeHFsYcOZGosazGeA()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisRawInactive:
					if (MathTools.ApproximatelyZero(P_0.IWvqBQApuMVSUeHFsYcOZGosazGeA()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisActiveOrJustInactive:
					if (!MathTools.ApproximatelyZero(P_0.RvRTgrdjWTZRmjoKbsOcGREaiXDi()) || !MathTools.ApproximatelyZero(P_0.lywVsHFwvRHDyAOYaAHUvBjHQYpD()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisRawActiveOrJustInactive:
					if (!MathTools.ApproximatelyZero(P_0.IWvqBQApuMVSUeHFsYcOZGosazGeA()) || !MathTools.ApproximatelyZero(P_0.RfXcmkCBFuBWUxLdultQzbgNZpmN()))
					{
						flag = true;
					}
					break;
				default:
					throw new NotImplementedException();
				}
				try
				{
					if (flag)
					{
						InputActionEventData obj = P_0.XFBEZqhYseuoBOUaCWIpSgAMfqheb(P_1);
						obj.eventType = fVxIHACgwelFbfkPEhTOyLPoSJOI2.FctibpOdqbGoHdZtAXyrxQRmOczv;
						fVxIHACgwelFbfkPEhTOyLPoSJOI2.pzQkhbFpUZlBUwLBtSVoiiHpSsry(obj);
					}
				}
				catch (Exception exception)
				{
					ReInput.HandleCallbackException("Player input event callback", exception);
				}
			}
		}
	}

	public void VvtznjEucSiQYaSyphkmDjBERHJg(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, int P_3, object[] P_4)
	{
		if (!gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			SYrvsWkcewIuFoKjBuiJnsmoEurE();
		}
		fVxIHACgwelFbfkPEhTOyLPoSJOI item;
		try
		{
			if (P_3 > ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.fyMfSUcvzcoChAfDspsaDySQPCgg)
			{
				throw new ArgumentOutOfRangeException("Invalid Action Id " + P_3);
			}
			item = new fVxIHACgwelFbfkPEhTOyLPoSJOI(P_0, P_1, P_2, P_3, P_4);
		}
		catch (Exception ex)
		{
			Logger.LogWarning("Failed to add Input Event delegate. Reason: " + ex.Message);
			return;
		}
		if (P_3 < 0)
		{
			ibKLptlPekJIJfmHLaVRPZLSncpv[AkzESxItwAkCdByFhhPZuIKSgFqA].Add(item);
		}
		else
		{
			ibKLptlPekJIJfmHLaVRPZLSncpv[cKTNQkLtjvSgCrsTHaOGbzANHbqx[P_3]].Add(item);
		}
		HMttPxgkIisPsIGOoMtEKPHDSKeB();
	}

	public void tPHDsXZajMrnvPPwRkxODRerbYhI(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, object[] P_3)
	{
		if (!gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			SYrvsWkcewIuFoKjBuiJnsmoEurE();
		}
		fVxIHACgwelFbfkPEhTOyLPoSJOI item;
		try
		{
			item = new fVxIHACgwelFbfkPEhTOyLPoSJOI(P_0, P_1, P_2, -1, P_3);
		}
		catch (Exception ex)
		{
			Logger.LogWarning("Failed to add Input Event delegate. Reason: " + ex.Message);
			return;
		}
		ibKLptlPekJIJfmHLaVRPZLSncpv[AkzESxItwAkCdByFhhPZuIKSgFqA].Add(item);
		HMttPxgkIisPsIGOoMtEKPHDSKeB();
	}

	public void oEHjHFaHgVgZEiiPvrBYpoEJRcgM(Action<InputActionEventData> P_0)
	{
		tEfdZWKpDDnXrnIMlzQHQpVTcfHf tEfdZWKpDDnXrnIMlzQHQpVTcfHf2 = new tEfdZWKpDDnXrnIMlzQHQpVTcfHf();
		tEfdZWKpDDnXrnIMlzQHQpVTcfHf2.ikittpeKFLmNjQXxpiAceAVmRevC = P_0;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(tEfdZWKpDDnXrnIMlzQHQpVTcfHf2.XqsftdGaOKhnLJxUKHTpmnCLuPQIA);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void aAssgIcQTwezjASHDINKEhcluwYm(Action<InputActionEventData> P_0, int P_1)
	{
		GUugsfLXBONmYchHOhPKjbAPHucOA gUugsfLXBONmYchHOhPKjbAPHucOA = new GUugsfLXBONmYchHOhPKjbAPHucOA();
		gUugsfLXBONmYchHOhPKjbAPHucOA.dJedEKvZAYBCgVpLjgWlhbmaKWFYA = P_0;
		gUugsfLXBONmYchHOhPKjbAPHucOA.jWpwvVExksnjJmoNvPVrOzGgWdER = P_1;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA && gUugsfLXBONmYchHOhPKjbAPHucOA.jWpwvVExksnjJmoNvPVrOzGgWdER <= ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.fyMfSUcvzcoChAfDspsaDySQPCgg)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(gUugsfLXBONmYchHOhPKjbAPHucOA.mJZSynxWpelWNlBwUCobAakJGypkA);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void mpsatPBiZqhdudDDVpvZnTUipvyV(Action<InputActionEventData> P_0, UpdateLoopType P_1)
	{
		pjJxKDXilLompUMjsdrUANexBplxA pjJxKDXilLompUMjsdrUANexBplxA2 = new pjJxKDXilLompUMjsdrUANexBplxA();
		pjJxKDXilLompUMjsdrUANexBplxA2.JpWuddCQjgzKTbVZBKHxFZUPifnT = P_0;
		pjJxKDXilLompUMjsdrUANexBplxA2.NFoGAKrBzbBMGfCWcMsMcFAcKGuTb = P_1;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(pjJxKDXilLompUMjsdrUANexBplxA2.dNerLKlpMsAdlbgUTEtcPlDLedkeb);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void xjMhVdFmEdfITamEREHhHeVInjHyA(Action<InputActionEventData> P_0, InputActionEventType P_1)
	{
		tRrZpctwIAiStwrCfmiUTYhFQZIJ tRrZpctwIAiStwrCfmiUTYhFQZIJ2 = new tRrZpctwIAiStwrCfmiUTYhFQZIJ();
		tRrZpctwIAiStwrCfmiUTYhFQZIJ2.eUNYqorMIrreTilChiQbjkeuDunk = P_0;
		tRrZpctwIAiStwrCfmiUTYhFQZIJ2.IgtDFqpVLBIqRIaPGQnhnFpUdqAKA = P_1;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(tRrZpctwIAiStwrCfmiUTYhFQZIJ2.ZqKGyaEcifprzMvLHIymMvioImFfb);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void rbAbnRXMZlhoYgOXMyQXMmxDjYQU(Action<InputActionEventData> P_0, UpdateLoopType P_1, int P_2)
	{
		zekAWKDbxgVGPJnciAUbEAdIShczB zekAWKDbxgVGPJnciAUbEAdIShczB2 = new zekAWKDbxgVGPJnciAUbEAdIShczB();
		zekAWKDbxgVGPJnciAUbEAdIShczB2.fpDSYrwcJGUuNVHbZGkRDaEXYbdW = P_0;
		zekAWKDbxgVGPJnciAUbEAdIShczB2.SgVMNoBJBYroqIWANFJrvGfWhXUbA = P_1;
		zekAWKDbxgVGPJnciAUbEAdIShczB2.SnOkprltCPmGZIbEzIYvAFDrpmFiA = P_2;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA && zekAWKDbxgVGPJnciAUbEAdIShczB2.SnOkprltCPmGZIbEzIYvAFDrpmFiA <= ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.fyMfSUcvzcoChAfDspsaDySQPCgg)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(zekAWKDbxgVGPJnciAUbEAdIShczB2.OVHOStUNsPAbNeMvyLxkXhQNIMoFA);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void hJodpAkBUzfITDVxbrWhIwqPPrfvB(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, int P_3)
	{
		aPhokcXMoXGKjvVBYFdFpQaptlhe aPhokcXMoXGKjvVBYFdFpQaptlhe2 = new aPhokcXMoXGKjvVBYFdFpQaptlhe();
		aPhokcXMoXGKjvVBYFdFpQaptlhe2.qghcPiVjsKyuVArJTIoDvpyeVuLR = P_0;
		aPhokcXMoXGKjvVBYFdFpQaptlhe2.jUnBsXTSJNGSfjlaIKrFDUEQYBSq = P_1;
		aPhokcXMoXGKjvVBYFdFpQaptlhe2.HMTGWeIgOboOfnbBhsMyjNvZjgXw = P_3;
		aPhokcXMoXGKjvVBYFdFpQaptlhe2.gFeFVYQADXEgIcxNwCPOrIzNNJgT = P_2;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA && aPhokcXMoXGKjvVBYFdFpQaptlhe2.HMTGWeIgOboOfnbBhsMyjNvZjgXw <= ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.fyMfSUcvzcoChAfDspsaDySQPCgg)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(aPhokcXMoXGKjvVBYFdFpQaptlhe2.mckdYFwzPQhKCTkkbjZafEzeaJEfb);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void HDEpMWesyAgcRghVGTzGaEsbqbAf(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2)
	{
		ZKJbTXBqhdepEyiVmBaqjeAqoajK zKJbTXBqhdepEyiVmBaqjeAqoajK = new ZKJbTXBqhdepEyiVmBaqjeAqoajK();
		zKJbTXBqhdepEyiVmBaqjeAqoajK.cQpcfvavpsYtyNcCmAKKCJOwvVoH = P_0;
		zKJbTXBqhdepEyiVmBaqjeAqoajK.LxIERxAZWurOdzXHKScQrehoNEnU = P_1;
		zKJbTXBqhdepEyiVmBaqjeAqoajK.eiBvSnUkPcdrUkDyDiyKkMvRBXXFb = P_2;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(zKJbTXBqhdepEyiVmBaqjeAqoajK.BtTUnvkgEJABVaQfhTBhOCijtpkd);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void PjrnfjaNJUJGnxbZKkcahMKRIcPs(Action<InputActionEventData> P_0, InputActionEventType P_1, int P_2)
	{
		kGImCLDmOdljFMRVsbFEjdbTRqIX kGImCLDmOdljFMRVsbFEjdbTRqIX2 = new kGImCLDmOdljFMRVsbFEjdbTRqIX();
		kGImCLDmOdljFMRVsbFEjdbTRqIX2.FAgNycjPzvYzZcsdSkZXLTgaskJk = P_0;
		kGImCLDmOdljFMRVsbFEjdbTRqIX2.hGzgSkMgYGcDTqBHQdnTJytbldFi = P_2;
		kGImCLDmOdljFMRVsbFEjdbTRqIX2.szIDyrIVDIhEhYVLsqpUeaegGPhm = P_1;
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA && kGImCLDmOdljFMRVsbFEjdbTRqIX2.hGzgSkMgYGcDTqBHQdnTJytbldFi <= ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.fyMfSUcvzcoChAfDspsaDySQPCgg)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(kGImCLDmOdljFMRVsbFEjdbTRqIX2.pHyVOhtHGpGOiCLrFkIiDAfaobZLA);
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	public void SWXmZDJMjvvJfIqiUJcHmCVSNfXG()
	{
		if (gZTFZYHVOpEHVZIjCujQUszHUKyVA)
		{
			AList<fVxIHACgwelFbfkPEhTOyLPoSJOI>[] array = ibKLptlPekJIJfmHLaVRPZLSncpv;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Clear();
			}
			HMttPxgkIisPsIGOoMtEKPHDSKeB();
		}
	}

	private void HMttPxgkIisPsIGOoMtEKPHDSKeB()
	{
		int num = 0;
		for (int i = 0; i < ibKLptlPekJIJfmHLaVRPZLSncpv.Length; i++)
		{
			num += ibKLptlPekJIJfmHLaVRPZLSncpv[i]._count;
		}
		rbDgJJKPZnOfiISIIFHbhZQEItGmD = num;
	}
}

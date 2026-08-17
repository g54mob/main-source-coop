using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

internal class WiGEESztaQcOlAVSzDYLfidomemMA
{
	private class LkbSSuzcnIghahSKGXROppVsRipFA
	{
		private class dsGahxjTKjCiVsSAvfSxAjnDGfrt
		{
			private int HNrdETOGKUabWLqKrsrJjjjvQQqu;

			private pBlirFMcDvialvBIRTfHatGjBdqH[] rgghiTwPYQtsKoFgKRoUtPldWus;

			private pdhxkcNifRAmZRqxzFpQjkCxjjonA[] cXsJfOPFRUALqnJeHHnUahdxnNZSA;

			public dsGahxjTKjCiVsSAvfSxAjnDGfrt(int P_0)
			{
				HNrdETOGKUabWLqKrsrJjjjvQQqu = P_0;
				rgghiTwPYQtsKoFgKRoUtPldWus = new pBlirFMcDvialvBIRTfHatGjBdqH[20];
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					rgghiTwPYQtsKoFgKRoUtPldWus[i] = new pBlirFMcDvialvBIRTfHatGjBdqH();
				}
				cXsJfOPFRUALqnJeHHnUahdxnNZSA = new pdhxkcNifRAmZRqxzFpQjkCxjjonA[29];
				for (int j = 0; j < cXsJfOPFRUALqnJeHHnUahdxnNZSA.Length; j++)
				{
					cXsJfOPFRUALqnJeHHnUahdxnNZSA[j] = new pdhxkcNifRAmZRqxzFpQjkCxjjonA(j);
				}
			}

			public void iSvbUvOqNFSvgKLMMuWonBnKnWnE()
			{
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					bool joystickButtonValueByJoystickIndex = UnityInputHelper.GetJoystickButtonValueByJoystickIndex(HNrdETOGKUabWLqKrsrJjjjvQQqu, i);
					rgghiTwPYQtsKoFgKRoUtPldWus[i].iSvbUvOqNFSvgKLMMuWonBnKnWnE(joystickButtonValueByJoystickIndex);
				}
				for (int j = 0; j < cXsJfOPFRUALqnJeHHnUahdxnNZSA.Length; j++)
				{
					float joystickAxisRawValueByJoystickIndex = UnityInputHelper.GetJoystickAxisRawValueByJoystickIndex(HNrdETOGKUabWLqKrsrJjjjvQQqu, j);
					cXsJfOPFRUALqnJeHHnUahdxnNZSA[j].iSvbUvOqNFSvgKLMMuWonBnKnWnE(joystickAxisRawValueByJoystickIndex);
				}
			}

			public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					rgghiTwPYQtsKoFgKRoUtPldWus[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln = UnityInputHelper.GetJoystickButtonValueByJoystickIndex(HNrdETOGKUabWLqKrsrJjjjvQQqu, i);
				}
				for (int j = 0; j < cXsJfOPFRUALqnJeHHnUahdxnNZSA.Length; j++)
				{
					cXsJfOPFRUALqnJeHHnUahdxnNZSA[j].yYOUbwIbBcyPAQvjeAEXVsjzLAln = UnityInputHelper.GetJoystickAxisRawValueByJoystickIndex(HNrdETOGKUabWLqKrsrJjjjvQQqu, j);
				}
			}

			public bool IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(int P_0)
			{
				if (P_0 < 0 || P_0 >= rgghiTwPYQtsKoFgKRoUtPldWus.Length)
				{
					return false;
				}
				return rgghiTwPYQtsKoFgKRoUtPldWus[P_0].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
			}

			public bool LbzwmtxTiWvoFmsIMWGeWeOGjmIg(int P_0)
			{
				if (P_0 < 0 || P_0 >= rgghiTwPYQtsKoFgKRoUtPldWus.Length)
				{
					return false;
				}
				return rgghiTwPYQtsKoFgKRoUtPldWus[P_0].zJkMPGtMddueMcJDyMsXdhcqorQl;
			}

			public bool vJpqJdBUhGAMATiWyyWbrqfEhKUg(int P_0)
			{
				if (P_0 < 0 || P_0 >= rgghiTwPYQtsKoFgKRoUtPldWus.Length)
				{
					return false;
				}
				return rgghiTwPYQtsKoFgKRoUtPldWus[P_0].uoMLTbBPDCNDEQdFdJvyeBicfQGhA;
			}

			public float JAUpfGusnmCqpzYFDMsATYuepPVJ(int P_0)
			{
				if (P_0 < 0 || P_0 >= cXsJfOPFRUALqnJeHHnUahdxnNZSA.Length)
				{
					return 0f;
				}
				return cXsJfOPFRUALqnJeHHnUahdxnNZSA[P_0].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
			}

			public bool oPBHjWsEooPaCcBjpRzeVDVxgsdKA(int P_0, bool P_1)
			{
				if (P_0 < 0 || P_0 >= cXsJfOPFRUALqnJeHHnUahdxnNZSA.Length)
				{
					return false;
				}
				return cXsJfOPFRUALqnJeHHnUahdxnNZSA[P_0].jlMJYbxJlzjsBwCBJxvDZPhnPAZF(P_1);
			}

			public void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					rgghiTwPYQtsKoFgKRoUtPldWus[i].SPGTRPyvIslcMdbPTItsewSLRPxx();
				}
				for (int j = 0; j < cXsJfOPFRUALqnJeHHnUahdxnNZSA.Length; j++)
				{
					cXsJfOPFRUALqnJeHHnUahdxnNZSA[j].SPGTRPyvIslcMdbPTItsewSLRPxx();
				}
			}
		}

		private class QsVsvdkUohpuxClMDjASeNadnlJeb
		{
			private pBlirFMcDvialvBIRTfHatGjBdqH[] rgghiTwPYQtsKoFgKRoUtPldWus;

			public QsVsvdkUohpuxClMDjASeNadnlJeb()
			{
				rgghiTwPYQtsKoFgKRoUtPldWus = new pBlirFMcDvialvBIRTfHatGjBdqH[7];
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					rgghiTwPYQtsKoFgKRoUtPldWus[i] = new pBlirFMcDvialvBIRTfHatGjBdqH();
				}
			}

			public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					rgghiTwPYQtsKoFgKRoUtPldWus[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln = Input.GetButton("MouseButton" + i);
				}
			}

			public bool IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(int P_0)
			{
				if (P_0 < 0 || P_0 >= rgghiTwPYQtsKoFgKRoUtPldWus.Length)
				{
					return false;
				}
				return rgghiTwPYQtsKoFgKRoUtPldWus[P_0].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
			}

			public bool LbzwmtxTiWvoFmsIMWGeWeOGjmIg(int P_0)
			{
				if (P_0 < 0 || P_0 >= rgghiTwPYQtsKoFgKRoUtPldWus.Length)
				{
					return false;
				}
				return rgghiTwPYQtsKoFgKRoUtPldWus[P_0].zJkMPGtMddueMcJDyMsXdhcqorQl;
			}

			public bool vJpqJdBUhGAMATiWyyWbrqfEhKUg(int P_0)
			{
				if (P_0 < 0 || P_0 >= rgghiTwPYQtsKoFgKRoUtPldWus.Length)
				{
					return false;
				}
				return rgghiTwPYQtsKoFgKRoUtPldWus[P_0].uoMLTbBPDCNDEQdFdJvyeBicfQGhA;
			}

			public void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					rgghiTwPYQtsKoFgKRoUtPldWus[i].SPGTRPyvIslcMdbPTItsewSLRPxx();
				}
			}
		}

		private class pBlirFMcDvialvBIRTfHatGjBdqH
		{
			private bool ckUkzuSDJGuJonmPceZrDLTKWbwc;

			private bool DLTDFssnQPFYYgComHYKltvlZVzB;

			public bool yYOUbwIbBcyPAQvjeAEXVsjzLAln
			{
				get
				{
					return ckUkzuSDJGuJonmPceZrDLTKWbwc;
				}
				set
				{
					DLTDFssnQPFYYgComHYKltvlZVzB = ckUkzuSDJGuJonmPceZrDLTKWbwc;
					ckUkzuSDJGuJonmPceZrDLTKWbwc = flag;
				}
			}

			public bool zJkMPGtMddueMcJDyMsXdhcqorQl
			{
				get
				{
					if (ckUkzuSDJGuJonmPceZrDLTKWbwc)
					{
						return !DLTDFssnQPFYYgComHYKltvlZVzB;
					}
					return false;
				}
			}

			public bool uoMLTbBPDCNDEQdFdJvyeBicfQGhA
			{
				get
				{
					if (DLTDFssnQPFYYgComHYKltvlZVzB)
					{
						return !ckUkzuSDJGuJonmPceZrDLTKWbwc;
					}
					return false;
				}
			}

			public void iSvbUvOqNFSvgKLMMuWonBnKnWnE(bool P_0)
			{
				ckUkzuSDJGuJonmPceZrDLTKWbwc = P_0;
				DLTDFssnQPFYYgComHYKltvlZVzB = P_0;
			}

			public void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				ckUkzuSDJGuJonmPceZrDLTKWbwc = false;
				DLTDFssnQPFYYgComHYKltvlZVzB = false;
			}
		}

		private class pdhxkcNifRAmZRqxzFpQjkCxjjonA
		{
			private int cAWIsSGMrDbPmTtmurVwXNpHqWOs;

			private float ckUkzuSDJGuJonmPceZrDLTKWbwc;

			private float TUQnXpTipEdknEfkyNtfNzWfjgcH;

			public float yYOUbwIbBcyPAQvjeAEXVsjzLAln
			{
				get
				{
					return ckUkzuSDJGuJonmPceZrDLTKWbwc;
				}
				set
				{
					ckUkzuSDJGuJonmPceZrDLTKWbwc = num;
				}
			}

			public pdhxkcNifRAmZRqxzFpQjkCxjjonA(int P_0)
			{
				cAWIsSGMrDbPmTtmurVwXNpHqWOs = P_0;
			}

			public void iSvbUvOqNFSvgKLMMuWonBnKnWnE(float P_0)
			{
				TUQnXpTipEdknEfkyNtfNzWfjgcH = P_0;
				ckUkzuSDJGuJonmPceZrDLTKWbwc = P_0;
			}

			public bool jlMJYbxJlzjsBwCBJxvDZPhnPAZF(bool P_0)
			{
				float num = ckUkzuSDJGuJonmPceZrDLTKWbwc - TUQnXpTipEdknEfkyNtfNzWfjgcH;
				if (P_0 && num < 0f)
				{
					return false;
				}
				if (MathTools.Abs(num) > 0.7f)
				{
					return true;
				}
				return false;
			}

			public void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				ckUkzuSDJGuJonmPceZrDLTKWbwc = 0f;
				TUQnXpTipEdknEfkyNtfNzWfjgcH = 0f;
			}
		}

		private dsGahxjTKjCiVsSAvfSxAjnDGfrt[] pfvHZfkVJHuFpTIaVPSjIEuhIisl;

		private QsVsvdkUohpuxClMDjASeNadnlJeb dEcTjrSkAbFekEIeQDAfncJOVAhdA;

		public LkbSSuzcnIghahSKGXROppVsRipFA()
		{
			pfvHZfkVJHuFpTIaVPSjIEuhIisl = new dsGahxjTKjCiVsSAvfSxAjnDGfrt[16];
			for (int i = 0; i < pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length; i++)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i] = new dsGahxjTKjCiVsSAvfSxAjnDGfrt(i);
			}
			dEcTjrSkAbFekEIeQDAfncJOVAhdA = new QsVsvdkUohpuxClMDjASeNadnlJeb();
		}

		public void iSvbUvOqNFSvgKLMMuWonBnKnWnE()
		{
			for (int i = 0; i < pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length; i++)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].iSvbUvOqNFSvgKLMMuWonBnKnWnE();
			}
		}

		public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
		{
			for (int i = 0; i < pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length; i++)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
			}
			dEcTjrSkAbFekEIeQDAfncJOVAhdA.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
		}

		public bool QpzPnxGIMMCWBCrxtdiASHIPggWYA(int P_0, int P_1)
		{
			if (P_0 < 0 || P_0 >= pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length)
			{
				return false;
			}
			return pfvHZfkVJHuFpTIaVPSjIEuhIisl[P_0].IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(P_1);
		}

		public bool bmcGOvypZBGMiaYHnsomqOQcWvEoA(int P_0, int P_1)
		{
			if (P_0 < 0 || P_0 >= pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length)
			{
				return false;
			}
			return pfvHZfkVJHuFpTIaVPSjIEuhIisl[P_0].LbzwmtxTiWvoFmsIMWGeWeOGjmIg(P_1);
		}

		public bool IdickmePymjVpAzeitHiSSjkMbCic(int P_0, int P_1)
		{
			if (P_0 < 0 || P_0 >= pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length)
			{
				return false;
			}
			return pfvHZfkVJHuFpTIaVPSjIEuhIisl[P_0].vJpqJdBUhGAMATiWyyWbrqfEhKUg(P_1);
		}

		public bool HLQdiZXhKRobIEOHRYhcAiSwwYrn(int P_0, int P_1, bool P_2)
		{
			if (P_0 < 0 || P_0 >= pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length)
			{
				return false;
			}
			return pfvHZfkVJHuFpTIaVPSjIEuhIisl[P_0].oPBHjWsEooPaCcBjpRzeVDVxgsdKA(P_1, P_2);
		}

		public bool XZHBqxpCatiLLfToyRDwTEzYYuQHA(int P_0)
		{
			return dEcTjrSkAbFekEIeQDAfncJOVAhdA.IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(P_0);
		}

		public bool GOBbNhCzacviCTMMjgLciqMVSHZAA(int P_0)
		{
			return dEcTjrSkAbFekEIeQDAfncJOVAhdA.LbzwmtxTiWvoFmsIMWGeWeOGjmIg(P_0);
		}

		public bool PoWMgjNcAGkRGcpdyqBJLgHRioWQ(int P_0)
		{
			return dEcTjrSkAbFekEIeQDAfncJOVAhdA.vJpqJdBUhGAMATiWyyWbrqfEhKUg(P_0);
		}

		public void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			for (int i = 0; i < pfvHZfkVJHuFpTIaVPSjIEuhIisl.Length; i++)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].SPGTRPyvIslcMdbPTItsewSLRPxx();
			}
			dEcTjrSkAbFekEIeQDAfncJOVAhdA.SPGTRPyvIslcMdbPTItsewSLRPxx();
		}
	}

	private UpdateLoopType IykDTZdiEUdjfaMPasESCgLSFroIc;

	private LkbSSuzcnIghahSKGXROppVsRipFA DUNRrqXaVrxjJldeirhtPEAazJBv;

	private IndexedDictionary<int, LkbSSuzcnIghahSKGXROppVsRipFA> BKofuWBAfBjqNRctqQfPIEsrtHaJA;

	public WiGEESztaQcOlAVSzDYLfidomemMA(UpdateLoopSetting P_0)
	{
		BKofuWBAfBjqNRctqQfPIEsrtHaJA = new IndexedDictionary<int, LkbSSuzcnIghahSKGXROppVsRipFA>();
		using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
		{
			List<UpdateLoopType> list = tList.list;
			EnumConverter.ToUpdateLoopTypes(P_0, list);
			for (int i = 0; i < list.Count; i++)
			{
				BKofuWBAfBjqNRctqQfPIEsrtHaJA.Add((int)list[i], new LkbSSuzcnIghahSKGXROppVsRipFA());
			}
		}
		IykDTZdiEUdjfaMPasESCgLSFroIc = UpdateLoopType.Update;
		DUNRrqXaVrxjJldeirhtPEAazJBv = BKofuWBAfBjqNRctqQfPIEsrtHaJA.GetValue(0);
	}

	public void iSvbUvOqNFSvgKLMMuWonBnKnWnE()
	{
		IEqKbgSmvgUuMwiaBAeKZXIVdJEBA(ReInput.currentUpdateLoop);
		DUNRrqXaVrxjJldeirhtPEAazJBv.iSvbUvOqNFSvgKLMMuWonBnKnWnE();
	}

	public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
	{
		IEqKbgSmvgUuMwiaBAeKZXIVdJEBA(P_0);
		DUNRrqXaVrxjJldeirhtPEAazJBv.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
	}

	public bool QpzPnxGIMMCWBCrxtdiASHIPggWYA(int P_0, int P_1)
	{
		return DUNRrqXaVrxjJldeirhtPEAazJBv.QpzPnxGIMMCWBCrxtdiASHIPggWYA(P_0, P_1);
	}

	public bool bmcGOvypZBGMiaYHnsomqOQcWvEoA(int P_0, int P_1)
	{
		return DUNRrqXaVrxjJldeirhtPEAazJBv.bmcGOvypZBGMiaYHnsomqOQcWvEoA(P_0, P_1);
	}

	public bool IdickmePymjVpAzeitHiSSjkMbCic(int P_0, int P_1)
	{
		return DUNRrqXaVrxjJldeirhtPEAazJBv.IdickmePymjVpAzeitHiSSjkMbCic(P_0, P_1);
	}

	public bool HLQdiZXhKRobIEOHRYhcAiSwwYrn(int P_0, int P_1, bool P_2)
	{
		return DUNRrqXaVrxjJldeirhtPEAazJBv.HLQdiZXhKRobIEOHRYhcAiSwwYrn(P_0, P_1, P_2);
	}

	public bool XZHBqxpCatiLLfToyRDwTEzYYuQHA(int P_0)
	{
		return DUNRrqXaVrxjJldeirhtPEAazJBv.XZHBqxpCatiLLfToyRDwTEzYYuQHA(P_0);
	}

	public bool GOBbNhCzacviCTMMjgLciqMVSHZAA(int P_0)
	{
		return DUNRrqXaVrxjJldeirhtPEAazJBv.GOBbNhCzacviCTMMjgLciqMVSHZAA(P_0);
	}

	public bool PoWMgjNcAGkRGcpdyqBJLgHRioWQ(int P_0)
	{
		return DUNRrqXaVrxjJldeirhtPEAazJBv.PoWMgjNcAGkRGcpdyqBJLgHRioWQ(P_0);
	}

	public void SPGTRPyvIslcMdbPTItsewSLRPxx()
	{
		for (int i = 0; i < BKofuWBAfBjqNRctqQfPIEsrtHaJA.Count; i++)
		{
			BKofuWBAfBjqNRctqQfPIEsrtHaJA[i].SPGTRPyvIslcMdbPTItsewSLRPxx();
		}
	}

	private void IEqKbgSmvgUuMwiaBAeKZXIVdJEBA(UpdateLoopType P_0)
	{
		if (IykDTZdiEUdjfaMPasESCgLSFroIc != P_0)
		{
			IykDTZdiEUdjfaMPasESCgLSFroIc = P_0;
			DUNRrqXaVrxjJldeirhtPEAazJBv = BKofuWBAfBjqNRctqQfPIEsrtHaJA.GetValue((int)P_0);
		}
	}
}

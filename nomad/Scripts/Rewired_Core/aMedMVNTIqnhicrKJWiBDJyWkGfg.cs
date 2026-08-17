using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

internal class aMedMVNTIqnhicrKJWiBDJyWkGfg
{
	private class jJVbWoFWzaCqjilXwLjIGQVEiLuC
	{
		private class zSLmZHEaebfCskKeJBJXkHSgWEMu
		{
			private int TtJcRfiiCsEtUeFmpbuaDsMssPOv;

			private nvNjOCQhaXHZzUKNlbQuJiYYKjEBA[] cFpEGFKUcTaLpsqerYKHeMyaSRCtA;

			private mFSangInLMiwTlcFmxjgCowseDuG[] pwOxTdReXUYpAWyvMUwEDHKAwQQx;

			public zSLmZHEaebfCskKeJBJXkHSgWEMu(int P_0)
			{
				TtJcRfiiCsEtUeFmpbuaDsMssPOv = P_0;
				cFpEGFKUcTaLpsqerYKHeMyaSRCtA = new nvNjOCQhaXHZzUKNlbQuJiYYKjEBA[20];
				for (int i = 0; i < cFpEGFKUcTaLpsqerYKHeMyaSRCtA.Length; i++)
				{
					cFpEGFKUcTaLpsqerYKHeMyaSRCtA[i] = new nvNjOCQhaXHZzUKNlbQuJiYYKjEBA();
				}
				pwOxTdReXUYpAWyvMUwEDHKAwQQx = new mFSangInLMiwTlcFmxjgCowseDuG[29];
				for (int j = 0; j < pwOxTdReXUYpAWyvMUwEDHKAwQQx.Length; j++)
				{
					pwOxTdReXUYpAWyvMUwEDHKAwQQx[j] = new mFSangInLMiwTlcFmxjgCowseDuG(j);
				}
			}

			public void HLDycMdQIBtZSFmFimxqnxmQeaJp()
			{
				for (int i = 0; i < cFpEGFKUcTaLpsqerYKHeMyaSRCtA.Length; i++)
				{
					bool joystickButtonValueByJoystickIndex = UnityInputHelper.GetJoystickButtonValueByJoystickIndex(TtJcRfiiCsEtUeFmpbuaDsMssPOv, i);
					cFpEGFKUcTaLpsqerYKHeMyaSRCtA[i].XzFmADnXJWAZGjhilLBvsHYBNqFc(joystickButtonValueByJoystickIndex);
				}
				for (int j = 0; j < pwOxTdReXUYpAWyvMUwEDHKAwQQx.Length; j++)
				{
					float joystickAxisRawValueByJoystickIndex = UnityInputHelper.GetJoystickAxisRawValueByJoystickIndex(TtJcRfiiCsEtUeFmpbuaDsMssPOv, j);
					pwOxTdReXUYpAWyvMUwEDHKAwQQx[j].lDTozRYlQeiQvCUxdNwPjwGnbOGW(joystickAxisRawValueByJoystickIndex);
				}
			}

			public void tHIbRquRJZfIVBlpNifhTFgjnhwK()
			{
				for (int i = 0; i < cFpEGFKUcTaLpsqerYKHeMyaSRCtA.Length; i++)
				{
					cFpEGFKUcTaLpsqerYKHeMyaSRCtA[i].RleCEJvOVWwrUFVPbmNkFHeVHtbaA = UnityInputHelper.GetJoystickButtonValueByJoystickIndex(TtJcRfiiCsEtUeFmpbuaDsMssPOv, i);
				}
				for (int j = 0; j < pwOxTdReXUYpAWyvMUwEDHKAwQQx.Length; j++)
				{
					pwOxTdReXUYpAWyvMUwEDHKAwQQx[j].TFjiYFHBdfsgVvJZZIFwGYqUDSct = UnityInputHelper.GetJoystickAxisRawValueByJoystickIndex(TtJcRfiiCsEtUeFmpbuaDsMssPOv, j);
				}
			}

			public bool fzVKDgkpgUnWLOKpQoGHcjlTdtjF(int P_0)
			{
				if (P_0 < 0 || P_0 >= cFpEGFKUcTaLpsqerYKHeMyaSRCtA.Length)
				{
					return false;
				}
				return cFpEGFKUcTaLpsqerYKHeMyaSRCtA[P_0].RleCEJvOVWwrUFVPbmNkFHeVHtbaA;
			}

			public bool QSkfOOMlCPQVuvDZGxeyjeiOfXel(int P_0)
			{
				if (P_0 < 0 || P_0 >= cFpEGFKUcTaLpsqerYKHeMyaSRCtA.Length)
				{
					return false;
				}
				return cFpEGFKUcTaLpsqerYKHeMyaSRCtA[P_0].CROrsDrAjZumsEezeFAoXySGhOWw;
			}

			public bool ksRitLcfUtrGqZRAZKKOgKEBEqNBA(int P_0)
			{
				if (P_0 < 0 || P_0 >= cFpEGFKUcTaLpsqerYKHeMyaSRCtA.Length)
				{
					return false;
				}
				return cFpEGFKUcTaLpsqerYKHeMyaSRCtA[P_0].lnbJhiFWJWthdLDiugsDhjHVtNHpA;
			}

			public float LthQeYfUSwmpjGhYYAyYagDUyoWr(int P_0)
			{
				if (P_0 < 0 || P_0 >= pwOxTdReXUYpAWyvMUwEDHKAwQQx.Length)
				{
					return 0f;
				}
				return pwOxTdReXUYpAWyvMUwEDHKAwQQx[P_0].TFjiYFHBdfsgVvJZZIFwGYqUDSct;
			}

			public bool mIQKRarCdgdPGHeQZJdFRByjbqHZA(int P_0, bool P_1)
			{
				if (P_0 < 0 || P_0 >= pwOxTdReXUYpAWyvMUwEDHKAwQQx.Length)
				{
					return false;
				}
				return pwOxTdReXUYpAWyvMUwEDHKAwQQx[P_0].fGyJVqPHGMVRTuGuQVQLruUJivZU(P_1);
			}

			public void kRtSiNUCfVbwMLmAvNBOLQUumaZC()
			{
				for (int i = 0; i < cFpEGFKUcTaLpsqerYKHeMyaSRCtA.Length; i++)
				{
					cFpEGFKUcTaLpsqerYKHeMyaSRCtA[i].ywZLbAhvfqlGXtYmSjSDYHSEftQfA();
				}
				for (int j = 0; j < pwOxTdReXUYpAWyvMUwEDHKAwQQx.Length; j++)
				{
					pwOxTdReXUYpAWyvMUwEDHKAwQQx[j].faMwPNTsFygnRJkNUvJeWMbaXPKi();
				}
			}
		}

		private class qzhKMjIDrRNosclXjWeOybtRfjIC
		{
			private nvNjOCQhaXHZzUKNlbQuJiYYKjEBA[] PVTbvGdXQlMBxSbtLgUewJTlcpmTA;

			public qzhKMjIDrRNosclXjWeOybtRfjIC()
			{
				PVTbvGdXQlMBxSbtLgUewJTlcpmTA = new nvNjOCQhaXHZzUKNlbQuJiYYKjEBA[7];
				for (int i = 0; i < PVTbvGdXQlMBxSbtLgUewJTlcpmTA.Length; i++)
				{
					PVTbvGdXQlMBxSbtLgUewJTlcpmTA[i] = new nvNjOCQhaXHZzUKNlbQuJiYYKjEBA();
				}
			}

			public void xSvXCcmrYBwdkIktvQCgjJYUBxRy()
			{
				for (int i = 0; i < PVTbvGdXQlMBxSbtLgUewJTlcpmTA.Length; i++)
				{
					PVTbvGdXQlMBxSbtLgUewJTlcpmTA[i].RleCEJvOVWwrUFVPbmNkFHeVHtbaA = Input.GetButton("MouseButton" + i);
				}
			}

			public bool rGfEhAJIPiUTfbUuvsWiojcDiOICA(int P_0)
			{
				if (P_0 < 0 || P_0 >= PVTbvGdXQlMBxSbtLgUewJTlcpmTA.Length)
				{
					return false;
				}
				return PVTbvGdXQlMBxSbtLgUewJTlcpmTA[P_0].RleCEJvOVWwrUFVPbmNkFHeVHtbaA;
			}

			public bool TodiYlDJMLKvLOiMAChfemDWfRLfA(int P_0)
			{
				if (P_0 < 0 || P_0 >= PVTbvGdXQlMBxSbtLgUewJTlcpmTA.Length)
				{
					return false;
				}
				return PVTbvGdXQlMBxSbtLgUewJTlcpmTA[P_0].CROrsDrAjZumsEezeFAoXySGhOWw;
			}

			public bool wUoEhkdrODPuJzAttnrrBDIaLoLfc(int P_0)
			{
				if (P_0 < 0 || P_0 >= PVTbvGdXQlMBxSbtLgUewJTlcpmTA.Length)
				{
					return false;
				}
				return PVTbvGdXQlMBxSbtLgUewJTlcpmTA[P_0].lnbJhiFWJWthdLDiugsDhjHVtNHpA;
			}

			public void rirexEKyjywuqPfQgbLnVIHIOXnA()
			{
				for (int i = 0; i < PVTbvGdXQlMBxSbtLgUewJTlcpmTA.Length; i++)
				{
					PVTbvGdXQlMBxSbtLgUewJTlcpmTA[i].ywZLbAhvfqlGXtYmSjSDYHSEftQfA();
				}
			}
		}

		private class nvNjOCQhaXHZzUKNlbQuJiYYKjEBA
		{
			private bool SqEnBVYIQbRxVJAvXEpmYRgyKVAU;

			private bool eubfnkeoQOpmWMVRtQlfwUvHAwRB;

			public bool RleCEJvOVWwrUFVPbmNkFHeVHtbaA
			{
				get
				{
					return SqEnBVYIQbRxVJAvXEpmYRgyKVAU;
				}
				set
				{
					eubfnkeoQOpmWMVRtQlfwUvHAwRB = SqEnBVYIQbRxVJAvXEpmYRgyKVAU;
					SqEnBVYIQbRxVJAvXEpmYRgyKVAU = sqEnBVYIQbRxVJAvXEpmYRgyKVAU;
				}
			}

			public bool CROrsDrAjZumsEezeFAoXySGhOWw
			{
				get
				{
					if (SqEnBVYIQbRxVJAvXEpmYRgyKVAU)
					{
						return !eubfnkeoQOpmWMVRtQlfwUvHAwRB;
					}
					return false;
				}
			}

			public bool lnbJhiFWJWthdLDiugsDhjHVtNHpA
			{
				get
				{
					if (eubfnkeoQOpmWMVRtQlfwUvHAwRB)
					{
						return !SqEnBVYIQbRxVJAvXEpmYRgyKVAU;
					}
					return false;
				}
			}

			public void XzFmADnXJWAZGjhilLBvsHYBNqFc(bool P_0)
			{
				SqEnBVYIQbRxVJAvXEpmYRgyKVAU = P_0;
				eubfnkeoQOpmWMVRtQlfwUvHAwRB = P_0;
			}

			public void ywZLbAhvfqlGXtYmSjSDYHSEftQfA()
			{
				SqEnBVYIQbRxVJAvXEpmYRgyKVAU = false;
				eubfnkeoQOpmWMVRtQlfwUvHAwRB = false;
			}
		}

		private class mFSangInLMiwTlcFmxjgCowseDuG
		{
			private int lIaEjunIipESkDevwvgMerLujJwd;

			private float myBeMtCmPKCtsGXOSmlFxQLcNmxBA;

			private float MNftgeGkMQVlnonbKJIQzquUoEpx;

			public float TFjiYFHBdfsgVvJZZIFwGYqUDSct
			{
				get
				{
					return myBeMtCmPKCtsGXOSmlFxQLcNmxBA;
				}
				set
				{
					myBeMtCmPKCtsGXOSmlFxQLcNmxBA = num;
				}
			}

			public mFSangInLMiwTlcFmxjgCowseDuG(int P_0)
			{
				lIaEjunIipESkDevwvgMerLujJwd = P_0;
			}

			public void lDTozRYlQeiQvCUxdNwPjwGnbOGW(float P_0)
			{
				MNftgeGkMQVlnonbKJIQzquUoEpx = P_0;
				myBeMtCmPKCtsGXOSmlFxQLcNmxBA = P_0;
			}

			public bool fGyJVqPHGMVRTuGuQVQLruUJivZU(bool P_0)
			{
				float num = myBeMtCmPKCtsGXOSmlFxQLcNmxBA - MNftgeGkMQVlnonbKJIQzquUoEpx;
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

			public void faMwPNTsFygnRJkNUvJeWMbaXPKi()
			{
				myBeMtCmPKCtsGXOSmlFxQLcNmxBA = 0f;
				MNftgeGkMQVlnonbKJIQzquUoEpx = 0f;
			}
		}

		private zSLmZHEaebfCskKeJBJXkHSgWEMu[] TUWgeRGTGjnkhlhEPASwCRCDVcuOc;

		private qzhKMjIDrRNosclXjWeOybtRfjIC rDoFkQmAYcbzfZcWDVQYEJTHkysP;

		public jJVbWoFWzaCqjilXwLjIGQVEiLuC()
		{
			TUWgeRGTGjnkhlhEPASwCRCDVcuOc = new zSLmZHEaebfCskKeJBJXkHSgWEMu[16];
			for (int i = 0; i < TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length; i++)
			{
				TUWgeRGTGjnkhlhEPASwCRCDVcuOc[i] = new zSLmZHEaebfCskKeJBJXkHSgWEMu(i);
			}
			rDoFkQmAYcbzfZcWDVQYEJTHkysP = new qzhKMjIDrRNosclXjWeOybtRfjIC();
		}

		public void bRAcQzpjjIJTPQzSeDfZylfruCov()
		{
			for (int i = 0; i < TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length; i++)
			{
				TUWgeRGTGjnkhlhEPASwCRCDVcuOc[i].HLDycMdQIBtZSFmFimxqnxmQeaJp();
			}
		}

		public void QGyjFkoasYqacfOcfxdDrUgaLZIj()
		{
			for (int i = 0; i < TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length; i++)
			{
				TUWgeRGTGjnkhlhEPASwCRCDVcuOc[i].tHIbRquRJZfIVBlpNifhTFgjnhwK();
			}
			rDoFkQmAYcbzfZcWDVQYEJTHkysP.xSvXCcmrYBwdkIktvQCgjJYUBxRy();
		}

		public bool snJUiJuJVTHrbkQCdNTYKPRWufwO(int P_0, int P_1)
		{
			if (P_0 < 0 || P_0 >= TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length)
			{
				return false;
			}
			return TUWgeRGTGjnkhlhEPASwCRCDVcuOc[P_0].fzVKDgkpgUnWLOKpQoGHcjlTdtjF(P_1);
		}

		public bool merQVuklqJDikxPaerjdbKdrbQRJ(int P_0, int P_1)
		{
			if (P_0 < 0 || P_0 >= TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length)
			{
				return false;
			}
			return TUWgeRGTGjnkhlhEPASwCRCDVcuOc[P_0].QSkfOOMlCPQVuvDZGxeyjeiOfXel(P_1);
		}

		public bool fPubFbdPDpsjrOUYcjxMhlpUzAgt(int P_0, int P_1)
		{
			if (P_0 < 0 || P_0 >= TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length)
			{
				return false;
			}
			return TUWgeRGTGjnkhlhEPASwCRCDVcuOc[P_0].ksRitLcfUtrGqZRAZKKOgKEBEqNBA(P_1);
		}

		public bool flqSnXtEPAYqGglBqIYbDeemCFIeb(int P_0, int P_1, bool P_2)
		{
			if (P_0 < 0 || P_0 >= TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length)
			{
				return false;
			}
			return TUWgeRGTGjnkhlhEPASwCRCDVcuOc[P_0].mIQKRarCdgdPGHeQZJdFRByjbqHZA(P_1, P_2);
		}

		public bool CLqmsqkcJinZThNeKjPOxIVYoxYn(int P_0)
		{
			return rDoFkQmAYcbzfZcWDVQYEJTHkysP.rGfEhAJIPiUTfbUuvsWiojcDiOICA(P_0);
		}

		public bool tZqrEQcZAFkwNOABTstfkxCFqyfQ(int P_0)
		{
			return rDoFkQmAYcbzfZcWDVQYEJTHkysP.TodiYlDJMLKvLOiMAChfemDWfRLfA(P_0);
		}

		public bool JVaSDjPjrutEhqDrKikzDacAemRy(int P_0)
		{
			return rDoFkQmAYcbzfZcWDVQYEJTHkysP.wUoEhkdrODPuJzAttnrrBDIaLoLfc(P_0);
		}

		public void KWyTMhTerxoIBzbAyXuCMgenHzAo()
		{
			for (int i = 0; i < TUWgeRGTGjnkhlhEPASwCRCDVcuOc.Length; i++)
			{
				TUWgeRGTGjnkhlhEPASwCRCDVcuOc[i].kRtSiNUCfVbwMLmAvNBOLQUumaZC();
			}
			rDoFkQmAYcbzfZcWDVQYEJTHkysP.rirexEKyjywuqPfQgbLnVIHIOXnA();
		}
	}

	private UpdateLoopType SCesYXruuPfBeYXbxMhygsljsDmX;

	private jJVbWoFWzaCqjilXwLjIGQVEiLuC jykudsNzGBzyKHdvybgoyTmeFDiC;

	private IndexedDictionary<int, jJVbWoFWzaCqjilXwLjIGQVEiLuC> ZoGAwPnyjKVVkVvpJwFrlsrUUwVB;

	public aMedMVNTIqnhicrKJWiBDJyWkGfg(UpdateLoopSetting P_0)
	{
		ZoGAwPnyjKVVkVvpJwFrlsrUUwVB = new IndexedDictionary<int, jJVbWoFWzaCqjilXwLjIGQVEiLuC>();
		using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
		{
			List<UpdateLoopType> list = tList.list;
			EnumConverter.ToUpdateLoopTypes(P_0, list);
			for (int i = 0; i < list.Count; i++)
			{
				ZoGAwPnyjKVVkVvpJwFrlsrUUwVB.Add((int)list[i], new jJVbWoFWzaCqjilXwLjIGQVEiLuC());
			}
		}
		SCesYXruuPfBeYXbxMhygsljsDmX = UpdateLoopType.Update;
		jykudsNzGBzyKHdvybgoyTmeFDiC = ZoGAwPnyjKVVkVvpJwFrlsrUUwVB.GetValue(0);
	}

	public void PiRrpNiZJeRXJSxwNECWWCmjLjLU()
	{
		sJEHakFgcvgIywuvnWtatNaHzdDN(ReInput.currentUpdateLoop);
		jykudsNzGBzyKHdvybgoyTmeFDiC.bRAcQzpjjIJTPQzSeDfZylfruCov();
	}

	public void jLowpfofZEkWfXCClnvBCCrJlgHL(UpdateLoopType P_0)
	{
		sJEHakFgcvgIywuvnWtatNaHzdDN(P_0);
		jykudsNzGBzyKHdvybgoyTmeFDiC.QGyjFkoasYqacfOcfxdDrUgaLZIj();
	}

	public bool akjNMtIbDclBlDySrBzcZWlMiCxO(int P_0, int P_1)
	{
		return jykudsNzGBzyKHdvybgoyTmeFDiC.snJUiJuJVTHrbkQCdNTYKPRWufwO(P_0, P_1);
	}

	public bool eGoPyXCXwZHKhuRJkbyDzEPnOUog(int P_0, int P_1)
	{
		return jykudsNzGBzyKHdvybgoyTmeFDiC.merQVuklqJDikxPaerjdbKdrbQRJ(P_0, P_1);
	}

	public bool PfcrvGvbDEsVNwWKqflKDlPPupOt(int P_0, int P_1)
	{
		return jykudsNzGBzyKHdvybgoyTmeFDiC.fPubFbdPDpsjrOUYcjxMhlpUzAgt(P_0, P_1);
	}

	public bool OhBZiMWrKxEzdKxlOiKmpUxpsenO(int P_0, int P_1, bool P_2)
	{
		return jykudsNzGBzyKHdvybgoyTmeFDiC.flqSnXtEPAYqGglBqIYbDeemCFIeb(P_0, P_1, P_2);
	}

	public bool ftWechozUNSdQdtsAkuhAlywfHLDA(int P_0)
	{
		return jykudsNzGBzyKHdvybgoyTmeFDiC.CLqmsqkcJinZThNeKjPOxIVYoxYn(P_0);
	}

	public bool zqvIokGOcbWSdHndGFdYcyJiicOH(int P_0)
	{
		return jykudsNzGBzyKHdvybgoyTmeFDiC.tZqrEQcZAFkwNOABTstfkxCFqyfQ(P_0);
	}

	public bool mmgNwvCENknItQRsOKOwqESPhLlU(int P_0)
	{
		return jykudsNzGBzyKHdvybgoyTmeFDiC.JVaSDjPjrutEhqDrKikzDacAemRy(P_0);
	}

	public void nDsldkGDaQnVqjfUfNpLkedFdsCV()
	{
		for (int i = 0; i < ZoGAwPnyjKVVkVvpJwFrlsrUUwVB.Count; i++)
		{
			ZoGAwPnyjKVVkVvpJwFrlsrUUwVB[i].KWyTMhTerxoIBzbAyXuCMgenHzAo();
		}
	}

	private void sJEHakFgcvgIywuvnWtatNaHzdDN(UpdateLoopType P_0)
	{
		if (SCesYXruuPfBeYXbxMhygsljsDmX != P_0)
		{
			SCesYXruuPfBeYXbxMhygsljsDmX = P_0;
			jykudsNzGBzyKHdvybgoyTmeFDiC = ZoGAwPnyjKVVkVvpJwFrlsrUUwVB.GetValue((int)P_0);
		}
	}
}

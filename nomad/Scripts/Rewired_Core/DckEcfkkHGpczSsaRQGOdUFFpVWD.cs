using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired;
using Rewired.Utils.Classes.Data;

internal class DckEcfkkHGpczSsaRQGOdUFFpVWD
{
	private class VpVAqSbPzkjZzTBMdoonzIhFSuro
	{
		public readonly InputAction XdMTarZHYAxQeCFQwgTpFmgfncskA;

		public readonly int IDnMzCvhwROiQKQhEeHRxtJGFshP;

		public readonly int TorfMDwXplVCDjsKgnlCNnpCBfJeA;

		public VpVAqSbPzkjZzTBMdoonzIhFSuro(InputAction P_0, int P_1)
		{
			XdMTarZHYAxQeCFQwgTpFmgfncskA = P_0;
			IDnMzCvhwROiQKQhEeHRxtJGFshP = P_0.id;
			TorfMDwXplVCDjsKgnlCNnpCBfJeA = P_1;
		}
	}

	private InputAction[] naVhkIVuwsKOiQwdjFJwFXBnEHmU;

	private ADictionary<string, VpVAqSbPzkjZzTBMdoonzIhFSuro> UMGnJJZPlyPwYiLZKRicukUKKAwU;

	private VpVAqSbPzkjZzTBMdoonzIhFSuro[] brxgbaSYTwdBtVKrieaHsIajMprU;

	private ReadOnlyCollection<InputAction> beDoVtgxMlBWXpLdFITnkcjTvSan;

	private int bMuPRpPwmoJXbxVbFUghMYYjuUfB;

	private int MvBBIdgeIlXleBEYKmVkQjQuBkeDA;

	private List<string> paZehCFfFldgqSVVWEytVDQnWaANA;

	private List<int> WXpCTlVLJyHvYhaQtfenyktNCpoQA;

	public IList<InputAction> oNXBuTCpMYRLGCcxGUYgjnxaGNBWA => beDoVtgxMlBWXpLdFITnkcjTvSan;

	public int mlHXwbZsAMqBDxgLMneFMJrnnMfX => bMuPRpPwmoJXbxVbFUghMYYjuUfB;

	public int fyMfSUcvzcoChAfDspsaDySQPCgg => MvBBIdgeIlXleBEYKmVkQjQuBkeDA;

	public DckEcfkkHGpczSsaRQGOdUFFpVWD(List<InputAction> P_0)
	{
		paZehCFfFldgqSVVWEytVDQnWaANA = new List<string>();
		WXpCTlVLJyHvYhaQtfenyktNCpoQA = new List<int>();
		naVhkIVuwsKOiQwdjFJwFXBnEHmU = P_0.ToArray();
		bMuPRpPwmoJXbxVbFUghMYYjuUfB = naVhkIVuwsKOiQwdjFJwFXBnEHmU.Length;
		int num = -1;
		for (int i = 0; i < bMuPRpPwmoJXbxVbFUghMYYjuUfB; i++)
		{
			int id = naVhkIVuwsKOiQwdjFJwFXBnEHmU[i].id;
			if (id > num)
			{
				num = id;
			}
		}
		MvBBIdgeIlXleBEYKmVkQjQuBkeDA = num;
		brxgbaSYTwdBtVKrieaHsIajMprU = new VpVAqSbPzkjZzTBMdoonzIhFSuro[num + 1];
		for (int j = 0; j < bMuPRpPwmoJXbxVbFUghMYYjuUfB; j++)
		{
			InputAction inputAction = naVhkIVuwsKOiQwdjFJwFXBnEHmU[j];
			brxgbaSYTwdBtVKrieaHsIajMprU[inputAction.id] = new VpVAqSbPzkjZzTBMdoonzIhFSuro(inputAction, j);
		}
		UMGnJJZPlyPwYiLZKRicukUKKAwU = new ADictionary<string, VpVAqSbPzkjZzTBMdoonzIhFSuro>(bMuPRpPwmoJXbxVbFUghMYYjuUfB, StringComparer.OrdinalIgnoreCase);
		for (int k = 0; k < bMuPRpPwmoJXbxVbFUghMYYjuUfB; k++)
		{
			InputAction inputAction2 = naVhkIVuwsKOiQwdjFJwFXBnEHmU[k];
			try
			{
				UMGnJJZPlyPwYiLZKRicukUKKAwU.Add(inputAction2.name, brxgbaSYTwdBtVKrieaHsIajMprU[inputAction2.id]);
			}
			catch
			{
				Logger.LogError("Duplicate Action name \"" + inputAction2.name + "\" found in Action list. Duplicate Action names are not allowed. If you have edited the data manually outside the Rewired Input Manager, remove any duplicate Actions.");
			}
		}
		beDoVtgxMlBWXpLdFITnkcjTvSan = new ReadOnlyCollection<InputAction>(naVhkIVuwsKOiQwdjFJwFXBnEHmU);
	}

	public InputAction NsixmaaSoJxmhDlFhVMoiKUGoKgn(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return null;
		}
		if (!UMGnJJZPlyPwYiLZKRicukUKKAwU.TryGetValue(P_0, out var value))
		{
			if (P_1)
			{
				WpemvjNgjtbzrsDHdrLlXGIHvDdJ(P_0);
			}
			return null;
		}
		return value.XdMTarZHYAxQeCFQwgTpFmgfncskA;
	}

	public InputAction xZfXTcilMeUxJbAzlmlraJwZALAIA(int P_0)
	{
		if (P_0 < 0)
		{
			return null;
		}
		if (P_0 > MvBBIdgeIlXleBEYKmVkQjQuBkeDA)
		{
			return null;
		}
		if (brxgbaSYTwdBtVKrieaHsIajMprU[P_0] == null)
		{
			return null;
		}
		return brxgbaSYTwdBtVKrieaHsIajMprU[P_0].XdMTarZHYAxQeCFQwgTpFmgfncskA;
	}

	public InputAction tOrbWEggKrehwrEPReOQzyywMhaE(int P_0)
	{
		if (P_0 < 0 || P_0 >= bMuPRpPwmoJXbxVbFUghMYYjuUfB)
		{
			return null;
		}
		return naVhkIVuwsKOiQwdjFJwFXBnEHmU[P_0];
	}

	public int XYovkROfWrjUvrhXaljRAUvHHDbU(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return -1;
		}
		if (!UMGnJJZPlyPwYiLZKRicukUKKAwU.TryGetValue(P_0, out var value))
		{
			if (P_1)
			{
				WpemvjNgjtbzrsDHdrLlXGIHvDdJ(P_0);
			}
			return -1;
		}
		return value.TorfMDwXplVCDjsKgnlCNnpCBfJeA;
	}

	public int CKEcLorxcAHKtKQePRXUkBRZbKYWA(int P_0, bool P_1 = false)
	{
		if (P_0 < 0 || P_0 > MvBBIdgeIlXleBEYKmVkQjQuBkeDA)
		{
			if (P_0 >= 0 && P_1)
			{
				XTNjYxcBsedoytfcmXXOGElecnQr(P_0);
			}
			return -1;
		}
		VpVAqSbPzkjZzTBMdoonzIhFSuro vpVAqSbPzkjZzTBMdoonzIhFSuro = brxgbaSYTwdBtVKrieaHsIajMprU[P_0];
		if (vpVAqSbPzkjZzTBMdoonzIhFSuro == null)
		{
			if (P_1)
			{
				XTNjYxcBsedoytfcmXXOGElecnQr(P_0);
			}
			return -1;
		}
		return vpVAqSbPzkjZzTBMdoonzIhFSuro.TorfMDwXplVCDjsKgnlCNnpCBfJeA;
	}

	public bool dnCUOMZmMFZuVmBNOdXsAigoGllK(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return false;
		}
		if (!UMGnJJZPlyPwYiLZKRicukUKKAwU.ContainsKey(P_0))
		{
			if (P_1)
			{
				WpemvjNgjtbzrsDHdrLlXGIHvDdJ(P_0);
			}
			return false;
		}
		return true;
	}

	public bool PkQdWCLFQkmIhaYncbIRDzOLBwYg(int P_0)
	{
		if (P_0 < 0 || P_0 > MvBBIdgeIlXleBEYKmVkQjQuBkeDA)
		{
			return false;
		}
		return brxgbaSYTwdBtVKrieaHsIajMprU[P_0] != null;
	}

	public int gtrjEglTtLNqnGEQlKvLXcvkcVLn(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return -1;
		}
		if (!UMGnJJZPlyPwYiLZKRicukUKKAwU.TryGetValue(P_0, out var value))
		{
			if (P_1)
			{
				WpemvjNgjtbzrsDHdrLlXGIHvDdJ(P_0);
			}
			return -1;
		}
		return value.IDnMzCvhwROiQKQhEeHRxtJGFshP;
	}

	private void WpemvjNgjtbzrsDHdrLlXGIHvDdJ(string P_0)
	{
		if (!paZehCFfFldgqSVVWEytVDQnWaANA.Contains(P_0))
		{
			paZehCFfFldgqSVVWEytVDQnWaANA.Add(P_0);
			Logger.LogWarning("The Action \"" + P_0 + "\" does not exist. You can create Actions in the editor.");
		}
	}

	private void XTNjYxcBsedoytfcmXXOGElecnQr(int P_0)
	{
		if (!WXpCTlVLJyHvYhaQtfenyktNCpoQA.Contains(P_0))
		{
			WXpCTlVLJyHvYhaQtfenyktNCpoQA.Add(P_0);
			Logger.LogWarning("No Action exists for Action Id " + P_0 + ". You can create Actions in the editor.");
		}
	}
}

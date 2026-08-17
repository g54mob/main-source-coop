using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.ControllerExtensions;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

internal class QEyJYCsWIMmSodKZNfAuCCFpSvIJ : ISteamControllerInternal
{
	private static Dictionary<string, ulong> xDeycgXBdnxYrhwzaPqllnLdXYCk;

	private static Dictionary<string, ulong> uCOXbkutTwEeToLlCEhIldHGgrvz;

	private static Dictionary<string, ulong> FkjVzPvCDXBEtBzToVKidVnjjZyOA;

	private static Dictionary<ulong, string> rPubMwEFAMtATANYpomdvRIDBhSsA;

	private static Dictionary<ulong, string> mCOSuzoDtrCleRPbmXWEAYhNyNtk;

	private static Dictionary<ulong, string> LEAyaDbPsctIRiNGUxzHckIrvaaE;

	public readonly ulong gXHpqnjsAHwDeJcgIiosqeVfsaJl;

	private QJIFLNdNEUClCDNvjSBlrTCTDnBU[] FvcQeKUCiPHfMFwvYYJYmEbUWEDC;

	private List<SteamControllerActionOrigin> EKVrcnUsTTIDdFuLXHHGeKDJkZMJ;

	private ReadOnlyCollection<SteamControllerActionOrigin> lNHEAqJnHPwNpwjwTbpRNkdktVhf;

	public int MaxActionSourceCount => 8;

	public bool IsConnected => HEOggPXpZhdGtWpScDgpuGlviEfy.VkKZkHqlEZbCiXtxiFokFSyXEuHJ(gXHpqnjsAHwDeJcgIiosqeVfsaJl);

	public static void oniBTiOipbbDtCjYnSwxUhgqYoNH(Dictionary<string, ulong> P_0)
	{
		if (P_0 != null && P_0.Count != 0)
		{
			xDeycgXBdnxYrhwzaPqllnLdXYCk = P_0;
			rPubMwEFAMtATANYpomdvRIDBhSsA = CollectionTools.CreateInverseDictionary(P_0);
		}
	}

	public static void VMXGtkctkdenFcLCgyHLIuJjAQXtA(Dictionary<string, ulong> P_0)
	{
		if (P_0 != null && P_0.Count != 0)
		{
			uCOXbkutTwEeToLlCEhIldHGgrvz = P_0;
			mCOSuzoDtrCleRPbmXWEAYhNyNtk = CollectionTools.CreateInverseDictionary(P_0);
		}
	}

	public static void tcdJQfVqxAauUjILMhKeKBXbOTWKA(Dictionary<string, ulong> P_0)
	{
		if (P_0 != null && P_0.Count != 0)
		{
			FkjVzPvCDXBEtBzToVKidVnjjZyOA = P_0;
			LEAyaDbPsctIRiNGUxzHckIrvaaE = CollectionTools.CreateInverseDictionary(P_0);
		}
	}

	public QEyJYCsWIMmSodKZNfAuCCFpSvIJ(ulong P_0)
	{
		gXHpqnjsAHwDeJcgIiosqeVfsaJl = P_0;
		FvcQeKUCiPHfMFwvYYJYmEbUWEDC = new QJIFLNdNEUClCDNvjSBlrTCTDnBU[8];
		EKVrcnUsTTIDdFuLXHHGeKDJkZMJ = new List<SteamControllerActionOrigin>(8);
		lNHEAqJnHPwNpwjwTbpRNkdktVhf = new ReadOnlyCollection<SteamControllerActionOrigin>(EKVrcnUsTTIDdFuLXHHGeKDJkZMJ);
	}

	public string GetActionSetName(ulong handle)
	{
		return zixCMQCfOaAMrawRXIHJuyImCJZfb(rPubMwEFAMtATANYpomdvRIDBhSsA, handle);
	}

	public string GetDigitalActionName(ulong handle)
	{
		return zixCMQCfOaAMrawRXIHJuyImCJZfb(LEAyaDbPsctIRiNGUxzHckIrvaaE, handle);
	}

	public string GetAnalogActionName(ulong handle)
	{
		return zixCMQCfOaAMrawRXIHJuyImCJZfb(mCOSuzoDtrCleRPbmXWEAYhNyNtk, handle);
	}

	public ulong GetActionSetHandle(ref string actionSetName)
	{
		return JqQQEFLVlMLtDYKxEAJeRqZeaIdU(xDeycgXBdnxYrhwzaPqllnLdXYCk, ref actionSetName);
	}

	public ulong GetDigitalActionHandle(ref string actionName)
	{
		return JqQQEFLVlMLtDYKxEAJeRqZeaIdU(FkjVzPvCDXBEtBzToVKidVnjjZyOA, ref actionName);
	}

	public ulong GetAnalogActionHandle(ref string actionName)
	{
		return JqQQEFLVlMLtDYKxEAJeRqZeaIdU(uCOXbkutTwEeToLlCEhIldHGgrvz, ref actionName);
	}

	public Vector2 GetAnalogActionValue(ulong actionHandle)
	{
		if (actionHandle == 0L)
		{
			return default(Vector2);
		}
		try
		{
			TwOHAjxfwdEnhgNNaXqwKCOphtGoA twOHAjxfwdEnhgNNaXqwKCOphtGoA = HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.HyuSxVjkhlDTdZujjgpKUFwgnumS(gXHpqnjsAHwDeJcgIiosqeVfsaJl, actionHandle);
			if (!twOHAjxfwdEnhgNNaXqwKCOphtGoA.IZtHOLcfVMxNCIujsFhgXiajpbtN)
			{
				return default(Vector2);
			}
			return new Vector2(twOHAjxfwdEnhgNNaXqwKCOphtGoA.BqmjxAfwgseNVDKGAzjtxWWBOPHv, twOHAjxfwdEnhgNNaXqwKCOphtGoA.nWpIIQRjiCIFaiiGnHzTvHDxytFG);
		}
		catch
		{
			return default(Vector2);
		}
	}

	public Vector2 GetAnalogActionValue(ref string actionName)
	{
		ulong analogActionHandle = GetAnalogActionHandle(ref actionName);
		return GetAnalogActionValue(analogActionHandle);
	}

	public bool GetDigitalActionValue(ulong actionHandle)
	{
		if (actionHandle == 0L)
		{
			return false;
		}
		try
		{
			kjbUmsrauCdNLeEMSIqsEViTeXFw kjbUmsrauCdNLeEMSIqsEViTeXFw2 = HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.thHkuWqmBIMvkmqPaDziiUmoaIaqA(gXHpqnjsAHwDeJcgIiosqeVfsaJl, actionHandle);
			Debug.Log(actionHandle + " state = " + kjbUmsrauCdNLeEMSIqsEViTeXFw2.mGUvIRiAIuEKLstSHZlWpzsYuhcR + " active = " + kjbUmsrauCdNLeEMSIqsEViTeXFw2.IZtHOLcfVMxNCIujsFhgXiajpbtN);
			return kjbUmsrauCdNLeEMSIqsEViTeXFw2.IZtHOLcfVMxNCIujsFhgXiajpbtN && kjbUmsrauCdNLeEMSIqsEViTeXFw2.mGUvIRiAIuEKLstSHZlWpzsYuhcR;
		}
		catch
		{
			return false;
		}
	}

	public bool GetDigitalActionValue(ref string actionName)
	{
		ulong digitalActionHandle = GetDigitalActionHandle(ref actionName);
		return GetDigitalActionValue(digitalActionHandle);
	}

	public bool SetActiveActionSet(ulong actionSetHandle)
	{
		if (actionSetHandle == 0L)
		{
			return false;
		}
		try
		{
			HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.aeZLJqHVBRFLAqUzQQfbLokTVUBN(gXHpqnjsAHwDeJcgIiosqeVfsaJl, actionSetHandle);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public bool SetActiveActionSet(ref string actionSetName)
	{
		ulong actionSetHandle = GetActionSetHandle(ref actionSetName);
		return SetActiveActionSet(actionSetHandle);
	}

	public ulong GetActiveActionSetHandle()
	{
		return HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.XFNeFlgMLOqBUdmsJixSflHqyrlpA(gXHpqnjsAHwDeJcgIiosqeVfsaJl);
	}

	public string GetActiveActionSetName()
	{
		return zixCMQCfOaAMrawRXIHJuyImCJZfb(rPubMwEFAMtATANYpomdvRIDBhSsA, HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.XFNeFlgMLOqBUdmsJixSflHqyrlpA(gXHpqnjsAHwDeJcgIiosqeVfsaJl));
	}

	public void ShowBindingPanel()
	{
		HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.TAplepzXEfRvVaUqADPszJVwXUWM(gXHpqnjsAHwDeJcgIiosqeVfsaJl);
	}

	public void SetHapticPulse(SteamControllerPadType triggerPad, float durationSeconds)
	{
		if (durationSeconds < 0f)
		{
			durationSeconds = 0f;
		}
		HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.KPlGbTsWIwRqEvIQydKwfrYIeEed(gXHpqnjsAHwDeJcgIiosqeVfsaJl, (uint)triggerPad, (ushort)(durationSeconds * 1000000f));
	}

	public void SetHapticPulse(SteamControllerPadType triggerPad, ushort durationMicroSeconds)
	{
		HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.KPlGbTsWIwRqEvIQydKwfrYIeEed(gXHpqnjsAHwDeJcgIiosqeVfsaJl, (uint)triggerPad, durationMicroSeconds);
	}

	public IList<SteamControllerActionOrigin> GetDigitalActionOrigins(ref string actionSetName, ref string actionName)
	{
		return GetDigitalActionOrigins(JqQQEFLVlMLtDYKxEAJeRqZeaIdU(xDeycgXBdnxYrhwzaPqllnLdXYCk, ref actionSetName), JqQQEFLVlMLtDYKxEAJeRqZeaIdU(FkjVzPvCDXBEtBzToVKidVnjjZyOA, ref actionName));
	}

	public IList<SteamControllerActionOrigin> GetDigitalActionOrigins(ulong actionSetHandle, ulong actionHandle)
	{
		EKVrcnUsTTIDdFuLXHHGeKDJkZMJ.Clear();
		if (actionSetHandle == 0L || actionHandle == 0L)
		{
			return lNHEAqJnHPwNpwjwTbpRNkdktVhf;
		}
		int num = HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.miePkyraGLkeqqibokkAIQRzCqGG(gXHpqnjsAHwDeJcgIiosqeVfsaJl, actionSetHandle, actionHandle, FvcQeKUCiPHfMFwvYYJYmEbUWEDC);
		for (int i = 0; i < num; i++)
		{
			EKVrcnUsTTIDdFuLXHHGeKDJkZMJ.Add((SteamControllerActionOrigin)FvcQeKUCiPHfMFwvYYJYmEbUWEDC[i]);
		}
		return lNHEAqJnHPwNpwjwTbpRNkdktVhf;
	}

	public IList<SteamControllerActionOrigin> GetAnalogActionOrigins(ref string actionSetName, ref string actionName)
	{
		return GetAnalogActionOrigins(JqQQEFLVlMLtDYKxEAJeRqZeaIdU(xDeycgXBdnxYrhwzaPqllnLdXYCk, ref actionSetName), JqQQEFLVlMLtDYKxEAJeRqZeaIdU(uCOXbkutTwEeToLlCEhIldHGgrvz, ref actionName));
	}

	public IList<SteamControllerActionOrigin> GetAnalogActionOrigins(ulong actionSetHandle, ulong actionHandle)
	{
		EKVrcnUsTTIDdFuLXHHGeKDJkZMJ.Clear();
		if (actionSetHandle == 0L || actionHandle == 0L)
		{
			return lNHEAqJnHPwNpwjwTbpRNkdktVhf;
		}
		int num = HEOggPXpZhdGtWpScDgpuGlviEfy.pFkdkufpScgMgYvyYASZBSVJwuLT.sRUIjElLUJOzCNgjgvFkrTmxVSFy(gXHpqnjsAHwDeJcgIiosqeVfsaJl, actionSetHandle, actionHandle, FvcQeKUCiPHfMFwvYYJYmEbUWEDC);
		for (int i = 0; i < num; i++)
		{
			EKVrcnUsTTIDdFuLXHHGeKDJkZMJ.Add((SteamControllerActionOrigin)FvcQeKUCiPHfMFwvYYJYmEbUWEDC[i]);
		}
		return lNHEAqJnHPwNpwjwTbpRNkdktVhf;
	}

	private ulong JqQQEFLVlMLtDYKxEAJeRqZeaIdU(Dictionary<string, ulong> P_0, ref string P_1)
	{
		if (P_0 == null || string.IsNullOrEmpty(P_1))
		{
			return 0uL;
		}
		if (!P_0.TryGetValue(P_1, out var value))
		{
			return 0uL;
		}
		return value;
	}

	private string zixCMQCfOaAMrawRXIHJuyImCJZfb(Dictionary<ulong, string> P_0, ulong P_1)
	{
		if (P_0 == null || P_1 == 0L)
		{
			return string.Empty;
		}
		if (!P_0.TryGetValue(P_1, out var value))
		{
			return string.Empty;
		}
		return value;
	}
}

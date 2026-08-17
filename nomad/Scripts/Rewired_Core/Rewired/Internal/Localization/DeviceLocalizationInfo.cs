using System;
using System.Collections.Generic;
using System.Text;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired.Internal.Localization
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal sealed class DeviceLocalizationInfo
	{
		public readonly Guid guid;

		public readonly ControllerType controllerType;

		public readonly bool isControllerTemplate;

		private readonly ReadOnlyList<string> ufWRmwtLNlnvKXDTxDIrbZHXDzoYA;

		private readonly IList<string> EXMAqcpEaWeAnRssaOwHtauHjDQO;

		private readonly ReadOnlyList<Guid> cNiNFUzhxVnSQcAJBhwecEXdRgFq;

		private string okAhHamAePiNzpGVFEbdigmKBSgV;

		private Bytes20 tzgpHkacqdkkEroZaPCpxpTntOex;

		private bool crxAkudDkfyPrgcdoWcsVryinSfzA;

		public ReadOnlyList<string> parentKeys => ufWRmwtLNlnvKXDTxDIrbZHXDzoYA;

		public ReadOnlyList<Guid> controllerTemplateGuids => cNiNFUzhxVnSQcAJBhwecEXdRgFq;

		public string additionalIdentifyingInformation
		{
			get
			{
				return okAhHamAePiNzpGVFEbdigmKBSgV;
			}
			set
			{
				wshLMiUgLijPPAbpXGofFuXsjWPW();
				okAhHamAePiNzpGVFEbdigmKBSgV = value;
			}
		}

		public Bytes20 hash => tzgpHkacqdkkEroZaPCpxpTntOex;

		public DeviceLocalizationInfo()
		{
			EXMAqcpEaWeAnRssaOwHtauHjDQO = new List<string>();
			ufWRmwtLNlnvKXDTxDIrbZHXDzoYA = new ReadOnlyList<string>(EXMAqcpEaWeAnRssaOwHtauHjDQO);
		}

		public DeviceLocalizationInfo(ControllerType P_0, bool P_1, Guid P_2, IList<string> P_3, IList<Guid> P_4)
		{
			controllerType = P_0;
			isControllerTemplate = P_1;
			guid = P_2;
			IList<string> eXMAqcpEaWeAnRssaOwHtauHjDQO;
			if (P_3 == null)
			{
				IList<string> list = new List<string>();
				eXMAqcpEaWeAnRssaOwHtauHjDQO = list;
			}
			else
			{
				eXMAqcpEaWeAnRssaOwHtauHjDQO = P_3;
			}
			EXMAqcpEaWeAnRssaOwHtauHjDQO = eXMAqcpEaWeAnRssaOwHtauHjDQO;
			ufWRmwtLNlnvKXDTxDIrbZHXDzoYA = new ReadOnlyList<string>(EXMAqcpEaWeAnRssaOwHtauHjDQO);
			if (P_4 != null)
			{
				cNiNFUzhxVnSQcAJBhwecEXdRgFq = new ReadOnlyList<Guid>(P_4);
			}
		}

		public DeviceLocalizationInfo(DeviceLocalizationInfo P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			guid = P_0.guid;
			controllerType = P_0.controllerType;
			isControllerTemplate = P_0.isControllerTemplate;
			EXMAqcpEaWeAnRssaOwHtauHjDQO = ((P_0.EXMAqcpEaWeAnRssaOwHtauHjDQO != null) ? new List<string>(P_0.EXMAqcpEaWeAnRssaOwHtauHjDQO) : new List<string>());
			ufWRmwtLNlnvKXDTxDIrbZHXDzoYA = new ReadOnlyList<string>(EXMAqcpEaWeAnRssaOwHtauHjDQO);
			if (P_0.controllerTemplateGuids != null)
			{
				cNiNFUzhxVnSQcAJBhwecEXdRgFq = new ReadOnlyList<Guid>(P_0.controllerTemplateGuids);
			}
			crxAkudDkfyPrgcdoWcsVryinSfzA = P_0.crxAkudDkfyPrgcdoWcsVryinSfzA;
		}

		public void InsertParentKey(int index, string key)
		{
			wshLMiUgLijPPAbpXGofFuXsjWPW();
			if (!string.IsNullOrEmpty(key))
			{
				EXMAqcpEaWeAnRssaOwHtauHjDQO.Insert(index, key);
			}
		}

		public void FinishRuntimeSetup()
		{
			ComputeHash();
			ggydIcSGJKOszeVPSmmuUNcRJGUN();
		}

		public Bytes20 ComputeHash()
		{
			StringBuilder sharedStringBuilder = LocalizationManager.GetSharedStringBuilder();
			sharedStringBuilder.Append(controllerType.ToString());
			bool flag = isControllerTemplate;
			sharedStringBuilder.Append(flag.ToString());
			sharedStringBuilder.Append(guid.ToString());
			int count = EXMAqcpEaWeAnRssaOwHtauHjDQO.Count;
			for (int i = 0; i < count; i++)
			{
				if (!string.IsNullOrEmpty(EXMAqcpEaWeAnRssaOwHtauHjDQO[i]))
				{
					sharedStringBuilder.Append(EXMAqcpEaWeAnRssaOwHtauHjDQO[i]);
				}
			}
			sharedStringBuilder.Append(okAhHamAePiNzpGVFEbdigmKBSgV);
			tzgpHkacqdkkEroZaPCpxpTntOex = MiscTools.HashSHA1(sharedStringBuilder.ToString());
			return tzgpHkacqdkkEroZaPCpxpTntOex;
		}

		private void ggydIcSGJKOszeVPSmmuUNcRJGUN()
		{
			crxAkudDkfyPrgcdoWcsVryinSfzA = true;
		}

		private void wshLMiUgLijPPAbpXGofFuXsjWPW()
		{
			if (crxAkudDkfyPrgcdoWcsVryinSfzA)
			{
				throw new Exception("Cannot modify a read-only object.");
			}
		}

		public static bool DataMatches(DeviceLocalizationInfo a, DeviceLocalizationInfo b)
		{
			if (a == null || b == null)
			{
				return false;
			}
			if (a.controllerType != b.controllerType || a.isControllerTemplate != b.isControllerTemplate || a.guid != b.guid || a.EXMAqcpEaWeAnRssaOwHtauHjDQO.Count != b.EXMAqcpEaWeAnRssaOwHtauHjDQO.Count)
			{
				return false;
			}
			int count = a.EXMAqcpEaWeAnRssaOwHtauHjDQO.Count;
			for (int i = 0; i < count; i++)
			{
				if (!string.Equals(a.EXMAqcpEaWeAnRssaOwHtauHjDQO[i], b.EXMAqcpEaWeAnRssaOwHtauHjDQO[i], StringComparison.Ordinal))
				{
					return false;
				}
			}
			int num = ((a.cNiNFUzhxVnSQcAJBhwecEXdRgFq != null) ? a.cNiNFUzhxVnSQcAJBhwecEXdRgFq.Count : 0);
			int num2 = ((b.cNiNFUzhxVnSQcAJBhwecEXdRgFq != null) ? b.cNiNFUzhxVnSQcAJBhwecEXdRgFq.Count : 0);
			if (num != num2)
			{
				return false;
			}
			for (int j = 0; j < num; j++)
			{
				if (a.cNiNFUzhxVnSQcAJBhwecEXdRgFq[j] != b.cNiNFUzhxVnSQcAJBhwecEXdRgFq[j])
				{
					return false;
				}
			}
			return true;
		}
	}
}

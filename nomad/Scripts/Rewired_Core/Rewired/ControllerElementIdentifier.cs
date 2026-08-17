using System;
using System.Collections.Generic;
using Rewired.Interfaces;
using Rewired.Internal.Glyphs;
using Rewired.Internal.Localization;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class ControllerElementIdentifier : IControllerElementIdentifierCommon_Internal, lOhdpMIGSdyahJLjLKbbeUkHQJxnB, bguKJVtsagJfXPpJQeurpzlOLIYd, vRDWZTvhTxtVFxhZXNeeonctREwv, HQqbZoQigscgVQcdQGCMdxuNvzzS, bOFZUEPNgDgQSavjlJvfJaMptbnQA, qRYbqBqElSaesizKXsECcXURAVVeb
	{
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		internal class lVegNbJXhHfEVYqBGHidPgoEtALWA
		{
			public int id;

			public string name;

			public string positiveName;

			public string negativeName;

			public string key;

			public string positiveKey;

			public string negativeKey;

			public ControllerElementType elementType;

			public CompoundControllerElementType compoundElementType;

			public string role;
		}

		internal sealed class cVMOubmkNChfxInhyRIuBcRKPeHT
		{
			[Serializable]
			private sealed class mcPaXbCUhRYEYeOrjStFsGNzLkbkA
			{
				public static readonly mcPaXbCUhRYEYeOrjStFsGNzLkbkA _003C_003E9 = new mcPaXbCUhRYEYeOrjStFsGNzLkbkA();

				public static Func<ControllerElementIdentifier, ControllerElementIdentifier, bool> _003C_003E9__4_0;

				internal bool FTrqIzNIBlbqDGAgHNOlQOtQnAwp(ControllerElementIdentifier P_0, ControllerElementIdentifier P_1)
				{
					if (P_0 == null || P_1 == null)
					{
						return false;
					}
					if (P_0 != null && P_1 != null && P_0.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == P_1.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid && P_0.elementType == P_1.elementType && P_0.compoundElementType == P_1.compoundElementType)
					{
						return string.Equals(P_0.key, P_1.key, StringComparison.Ordinal);
					}
					return false;
				}
			}

			private static cVMOubmkNChfxInhyRIuBcRKPeHT BNowXWUdNkCIhgcJJgOtqtoULYsV;

			private readonly global::JZpDMkieCQWHnBUBlnLvccwpXKuI<ControllerElementIdentifier> ldDtWzsrPWUbSveAYsoXNnOBcuBz;

			private static cVMOubmkNChfxInhyRIuBcRKPeHT CkcmFnJXIhgKtgAERFXUKmVQVkSo
			{
				get
				{
					if (BNowXWUdNkCIhgcJJgOtqtoULYsV != null)
					{
						return BNowXWUdNkCIhgcJJgOtqtoULYsV;
					}
					BNowXWUdNkCIhgcJJgOtqtoULYsV = new cVMOubmkNChfxInhyRIuBcRKPeHT();
					BNowXWUdNkCIhgcJJgOtqtoULYsV.ycuAsTMhxZSRsQtOBXADIHjuBjaS();
					return BNowXWUdNkCIhgcJJgOtqtoULYsV;
				}
			}

			private cVMOubmkNChfxInhyRIuBcRKPeHT()
			{
				ldDtWzsrPWUbSveAYsoXNnOBcuBz = new global::JZpDMkieCQWHnBUBlnLvccwpXKuI<ControllerElementIdentifier>(mcPaXbCUhRYEYeOrjStFsGNzLkbkA._003C_003E9.FTrqIzNIBlbqDGAgHNOlQOtQnAwp);
			}

			private void ycuAsTMhxZSRsQtOBXADIHjuBjaS()
			{
				ReInput.ShutDownEvent += BNowXWUdNkCIhgcJJgOtqtoULYsV.PQGdXQdpCzOcjhyeFQPbjzYxVVHlc;
			}

			private void PQGdXQdpCzOcjhyeFQPbjzYxVVHlc()
			{
				if (BNowXWUdNkCIhgcJJgOtqtoULYsV == this)
				{
					BNowXWUdNkCIhgcJJgOtqtoULYsV = null;
				}
				ReInput.ShutDownEvent -= PQGdXQdpCzOcjhyeFQPbjzYxVVHlc;
			}

			public static ControllerElementIdentifier evhThOGdKbCcEJrcoicSAAqjKHhzB(DeviceLocalizationInfo P_0, ControllerElementIdentifier P_1)
			{
				return CkcmFnJXIhgKtgAERFXUKmVQVkSo.ldDtWzsrPWUbSveAYsoXNnOBcuBz.gOGhAlpaDWHngyOvcwqBdSVIJQot(P_0.hash, P_1);
			}

			public static bool GWVbPguLsfUJOoFoiCTSEfcLJAReb(DeviceLocalizationInfo P_0, ControllerElementIdentifier P_1, out ControllerElementIdentifier P_2)
			{
				return CkcmFnJXIhgKtgAERFXUKmVQVkSo.ldDtWzsrPWUbSveAYsoXNnOBcuBz.UCVjRYAbAROoXGysMPODoXVTfPnm(P_0.hash, P_1, out P_2);
			}

			public static void EkxzcoIUeaCajdridSIelNeKoqrx(DeviceLocalizationInfo P_0, ControllerElementIdentifier P_1)
			{
				CkcmFnJXIhgKtgAERFXUKmVQVkSo.ldDtWzsrPWUbSveAYsoXNnOBcuBz.jwZfZMJFGsXepvnbcdwNeIsRDvtv(P_0.hash, P_1);
			}
		}

		private class uUywiGAizPqSdAxUNrSksLgDLhAW
		{
			[SerializeField]
			private string AGRwKWlXOYDSpDFiHeAnKILPjFGrA;

			[SerializeField]
			private string GeVtylGmvyeOziajXnpgfozEsBHy;

			public string JMvhdTtcZbxgprakncjapyQeXNxe
			{
				get
				{
					return AGRwKWlXOYDSpDFiHeAnKILPjFGrA;
				}
				set
				{
					AGRwKWlXOYDSpDFiHeAnKILPjFGrA = aGRwKWlXOYDSpDFiHeAnKILPjFGrA;
				}
			}

			public string JwjgBCrVfDPJFXiGRXcbkRlDppXN
			{
				get
				{
					return GeVtylGmvyeOziajXnpgfozEsBHy;
				}
				set
				{
					GeVtylGmvyeOziajXnpgfozEsBHy = geVtylGmvyeOziajXnpgfozEsBHy;
				}
			}

			public uUywiGAizPqSdAxUNrSksLgDLhAW()
			{
			}

			public uUywiGAizPqSdAxUNrSksLgDLhAW(uUywiGAizPqSdAxUNrSksLgDLhAW P_0)
			{
				AGRwKWlXOYDSpDFiHeAnKILPjFGrA = P_0.AGRwKWlXOYDSpDFiHeAnKILPjFGrA;
				GeVtylGmvyeOziajXnpgfozEsBHy = P_0.GeVtylGmvyeOziajXnpgfozEsBHy;
			}
		}

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _id;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _name;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _positiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _negativeName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _key;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _positiveKey;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _negativeKey;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ControllerElementType _elementType;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CompoundControllerElementType _compoundElementType;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _role;

		[NonSerialized]
		private bool ANNDqGGSoxQLhWsOwqlusgHHsExU;

		[NonSerialized]
		private bUlLQaUKfECmSjzpJPefXKFSSdNK CdRZNurhXCElSrCZlBerZAEXBnkO;

		[NonSerialized]
		private QxUxEBAJtBeaHnGkrHvfxKcdhxVz GYbILdZjgQGetrBszduDViYyCzFaA;

		[NonSerialized]
		private UFCQEdfDnGpykVRzRZqGOgJZsGeF NCbONvNdKnWMvfwJINTjvIxnLIeC;

		[NonSerialized]
		private GQKseIhIzoPbpARMcpUcDlfHmRfe wgyAorGeaTdBTCbeQIenkRBJdBkt;

		[NonSerialized]
		private DeviceLocalizationInfo dsZBhmUpjvOCLSRrMEuwpKKZoEIV;

		[NonSerialized]
		private int KAMNpyLpNfVKiMewNWCqQJygPUEB;

		[NonSerialized]
		private List<uUywiGAizPqSdAxUNrSksLgDLhAW> NykokYsHrBCkFLTelQtQCiSiZzlU;

		[NonSerialized]
		private ControllerType ouQfNUIylDcDMjuAbQZwMqnvKKgKA;

		private static ControllerElementIdentifier xZTObFSWgVjlTLYBkSbAJcrLkyou;

		int IControllerElementIdentifierCommon_Internal.id => _id;

		string IControllerElementIdentifierCommon_Internal.name
		{
			get
			{
				if (!ReInput.isReady || CdRZNurhXCElSrCZlBerZAEXBnkO == null || !LocalizationManager.isEnabled)
				{
					return _name;
				}
				return CdRZNurhXCElSrCZlBerZAEXBnkO.MpfwJMTclVnnxEuHhBPCmlxJadkBA;
			}
			internal set
			{
				nonLocalizedName = value;
			}
		}

		string IControllerElementIdentifierCommon_Internal.positiveName
		{
			get
			{
				if (!ReInput.isReady || CdRZNurhXCElSrCZlBerZAEXBnkO == null || !LocalizationManager.isEnabled)
				{
					return _positiveName;
				}
				return CdRZNurhXCElSrCZlBerZAEXBnkO.ypoFNNDaQTVeKqsDkwEcIpgjZwGfA;
			}
			internal set
			{
				nonLocalizedPositiveName = value;
			}
		}

		string IControllerElementIdentifierCommon_Internal.negativeName
		{
			get
			{
				if (!ReInput.isReady || CdRZNurhXCElSrCZlBerZAEXBnkO == null || !LocalizationManager.isEnabled)
				{
					return _negativeName;
				}
				return CdRZNurhXCElSrCZlBerZAEXBnkO.CIYbIIBbIoEXdOMvSfyWNziRLZjc;
			}
			internal set
			{
				nonLocalizedNegativeName = value;
			}
		}

		public ControllerElementType elementType => _elementType;

		public CompoundControllerElementType compoundElementType => _compoundElementType;

		public object glyph
		{
			get
			{
				if (!ReInput.isReady || NCbONvNdKnWMvfwJINTjvIxnLIeC == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return NCbONvNdKnWMvfwJINTjvIxnLIeC.VMAqNtDPmJybBXpPjPFicHThBxQD;
			}
		}

		public object positiveGlyph
		{
			get
			{
				if (!ReInput.isReady || NCbONvNdKnWMvfwJINTjvIxnLIeC == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return NCbONvNdKnWMvfwJINTjvIxnLIeC.ivgGbvjljGLQSbxeeHhSPwvwKzZb;
			}
		}

		public object negativeGlyph
		{
			get
			{
				if (!ReInput.isReady || NCbONvNdKnWMvfwJINTjvIxnLIeC == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return NCbONvNdKnWMvfwJINTjvIxnLIeC.yLUKfKXXayZNlhzpjLtAckROOAOB;
			}
		}

		private string finalGlyphKey
		{
			get
			{
				if (!ReInput.isReady || NCbONvNdKnWMvfwJINTjvIxnLIeC == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return NCbONvNdKnWMvfwJINTjvIxnLIeC.QDOOogoJEZzDwhcfyDMVrQliiZaQ;
			}
		}

		private string finalPositiveGlyphKey
		{
			get
			{
				if (!ReInput.isReady || NCbONvNdKnWMvfwJINTjvIxnLIeC == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return NCbONvNdKnWMvfwJINTjvIxnLIeC.YHLjcmxgqGUnWoepoFwHlpXwGlOQ;
			}
		}

		private string finalNegativeGlyphKey
		{
			get
			{
				if (!ReInput.isReady || NCbONvNdKnWMvfwJINTjvIxnLIeC == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return NCbONvNdKnWMvfwJINTjvIxnLIeC.BOHsCXxxroVnAbuhrSFCqvZoizFN;
			}
		}

		internal string nonLocalizedName
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				if (ReInput.isReady)
				{
					lvNDXDTiggdWQKhNgHvlTXYGDETl();
					if (CdRZNurhXCElSrCZlBerZAEXBnkO != null)
					{
						CdRZNurhXCElSrCZlBerZAEXBnkO.YSgvMmquHVoFhixWnSsVWmcflge();
					}
				}
			}
		}

		internal string nonLocalizedPositiveName
		{
			get
			{
				return _positiveName;
			}
			set
			{
				_positiveName = value;
				if (ReInput.isReady)
				{
					lvNDXDTiggdWQKhNgHvlTXYGDETl();
					if (CdRZNurhXCElSrCZlBerZAEXBnkO != null)
					{
						CdRZNurhXCElSrCZlBerZAEXBnkO.nFzBoFXBNysVIMLFgBueCHpnWLDe();
					}
				}
			}
		}

		internal string nonLocalizedNegativeName
		{
			get
			{
				return _negativeName;
			}
			set
			{
				_negativeName = value;
				if (ReInput.isReady)
				{
					lvNDXDTiggdWQKhNgHvlTXYGDETl();
					if (CdRZNurhXCElSrCZlBerZAEXBnkO != null)
					{
						CdRZNurhXCElSrCZlBerZAEXBnkO.GjiLKqPVUrrLpJzFcrhtOPEtLQgk();
					}
				}
			}
		}

		public string key => _key;

		public string positiveKey => _positiveKey;

		public string negativeKey => _negativeKey;

		public string role => _role;

		internal bool isCompoundElement => _elementType == ControllerElementType.CompoundElement;

		string IControllerElementIdentifierCommon_Internal.nonLocalizedName => nonLocalizedName;

		string IControllerElementIdentifierCommon_Internal.nonLocalizedPositiveName => nonLocalizedPositiveName;

		string IControllerElementIdentifierCommon_Internal.nonLocalizedNegativeName => nonLocalizedNegativeName;

		bool IControllerElementIdentifierCommon_Internal.isNonLocalizedPositiveNameAutoGenerated => (KAMNpyLpNfVKiMewNWCqQJygPUEB & 2) != 0;

		bool IControllerElementIdentifierCommon_Internal.isNonLocalizedNegativeNameAutoGenerated => (KAMNpyLpNfVKiMewNWCqQJygPUEB & 4) != 0;

		bool IControllerElementIdentifierCommon_Internal.isPositiveKeyAutoGenerated => (KAMNpyLpNfVKiMewNWCqQJygPUEB & 8) != 0;

		bool IControllerElementIdentifierCommon_Internal.isNegativeKeyAutoGenerated => (KAMNpyLpNfVKiMewNWCqQJygPUEB & 0x10) != 0;

		string IControllerElementIdentifierCommon_Internal.key => _key;

		string IControllerElementIdentifierCommon_Internal.positiveKey => _positiveKey;

		string IControllerElementIdentifierCommon_Internal.negativeKey => _negativeKey;

		DeviceLocalizationInfo IControllerElementIdentifierCommon_Internal.deviceLocalizationInfo => dsZBhmUpjvOCLSRrMEuwpKKZoEIV;

		object IControllerElementIdentifierCommon_Internal.elementType => _elementType;

		bool IControllerElementIdentifierCommon_Internal.useEditorElementTypeOverride => false;

		ControllerElementType IControllerElementIdentifierCommon_Internal.editorElementTypeOverride => _elementType;

		internal static ControllerElementIdentifier BlankReadOnly
		{
			get
			{
				if (xZTObFSWgVjlTLYBkSbAJcrLkyou == null)
				{
					ControllerElementIdentifier result = new ControllerElementIdentifier
					{
						_id = -1,
						ANNDqGGSoxQLhWsOwqlusgHHsExU = true
					};
					xZTObFSWgVjlTLYBkSbAJcrLkyou = result;
					return result;
				}
				return xZTObFSWgVjlTLYBkSbAJcrLkyou;
			}
		}

		string bguKJVtsagJfXPpJQeurpzlOLIYd.keyCategory => "controller/template";

		string bguKJVtsagJfXPpJQeurpzlOLIYd.scriptingName => _name;

		string bguKJVtsagJfXPpJQeurpzlOLIYd.nonLocalizedDescriptiveName
		{
			get
			{
				return _name;
			}
			set
			{
				lvNDXDTiggdWQKhNgHvlTXYGDETl();
				_name = value;
			}
		}

		string lOhdpMIGSdyahJLjLKbbeUkHQJxnB.nonLocalizedPositiveDescriptiveName
		{
			get
			{
				return _positiveName;
			}
			set
			{
				lvNDXDTiggdWQKhNgHvlTXYGDETl();
				_positiveName = value;
			}
		}

		string lOhdpMIGSdyahJLjLKbbeUkHQJxnB.nonLocalizedNegativeDescriptiveName
		{
			get
			{
				return _negativeName;
			}
			set
			{
				lvNDXDTiggdWQKhNgHvlTXYGDETl();
				_negativeName = value;
			}
		}

		string bguKJVtsagJfXPpJQeurpzlOLIYd.key => _key;

		string lOhdpMIGSdyahJLjLKbbeUkHQJxnB.positiveKey
		{
			get
			{
				return _positiveKey;
			}
			set
			{
				lvNDXDTiggdWQKhNgHvlTXYGDETl();
				_positiveKey = value;
			}
		}

		string lOhdpMIGSdyahJLjLKbbeUkHQJxnB.negativeKey
		{
			get
			{
				return _negativeKey;
			}
			set
			{
				lvNDXDTiggdWQKhNgHvlTXYGDETl();
				_negativeKey = value;
			}
		}

		int bguKJVtsagJfXPpJQeurpzlOLIYd.autoGeneratedValueFlags
		{
			get
			{
				return KAMNpyLpNfVKiMewNWCqQJygPUEB;
			}
			set
			{
				KAMNpyLpNfVKiMewNWCqQJygPUEB = value;
			}
		}

		string HQqbZoQigscgVQcdQGCMdxuNvzzS.keyCategory => fNDBBZXbOAvGiTXVzfEmFadoOOjj.ZjyEVyERnmGwvaLVfGpAagVLJQHN(ouQfNUIylDcDMjuAbQZwMqnvKKgKA);

		string HQqbZoQigscgVQcdQGCMdxuNvzzS.key => _key;

		int HQqbZoQigscgVQcdQGCMdxuNvzzS.autoGeneratedValueFlags
		{
			get
			{
				return KAMNpyLpNfVKiMewNWCqQJygPUEB;
			}
			set
			{
				KAMNpyLpNfVKiMewNWCqQJygPUEB = value;
			}
		}

		string vRDWZTvhTxtVFxhZXNeeonctREwv.positiveKey
		{
			get
			{
				return _positiveKey;
			}
			set
			{
				lvNDXDTiggdWQKhNgHvlTXYGDETl();
				_positiveKey = value;
			}
		}

		string vRDWZTvhTxtVFxhZXNeeonctREwv.negativeKey
		{
			get
			{
				return _negativeKey;
			}
			set
			{
				lvNDXDTiggdWQKhNgHvlTXYGDETl();
				_negativeKey = value;
			}
		}

		internal string GetCompoundElementSpecialName(int index)
		{
			if (!ReInput.isReady || !LocalizationManager.isEnabled || NykokYsHrBCkFLTelQtQCiSiZzlU == null || GYbILdZjgQGetrBszduDViYyCzFaA == null)
			{
				return string.Empty;
			}
			return GYbILdZjgQGetrBszduDViYyCzFaA.UZtpxFUPQZBSvGgnZjNTrWXUAlHF(index);
		}

		internal object GetCompoundElementSpecialGlyph(int index)
		{
			if (!ReInput.isReady || !GlyphManager.isEnabled || NykokYsHrBCkFLTelQtQCiSiZzlU == null || wgyAorGeaTdBTCbeQIenkRBJdBkt == null)
			{
				return null;
			}
			return wgyAorGeaTdBTCbeQIenkRBJdBkt.XmlgGslFtGJifkBoPmYBBdHirZpm(index);
		}

		internal string GetCompoundElementSpecialFinalGlyphKey(int index)
		{
			if (!ReInput.isReady || !GlyphManager.isEnabled || NykokYsHrBCkFLTelQtQCiSiZzlU == null || wgyAorGeaTdBTCbeQIenkRBJdBkt == null)
			{
				return null;
			}
			return wgyAorGeaTdBTCbeQIenkRBJdBkt.LiGFhODBRwkVpOqUDPjgnZGYaAyNA(index);
		}

		string IControllerElementIdentifierCommon_Internal.GetSpecialElementNonLocalizedName(int index)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU == null || (uint)index >= (uint)NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				return null;
			}
			return NykokYsHrBCkFLTelQtQCiSiZzlU[index].JwjgBCrVfDPJFXiGRXcbkRlDppXN;
		}

		string IControllerElementIdentifierCommon_Internal.GetSpecialElementKey(int index)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU == null || (uint)index >= (uint)NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				return null;
			}
			return NykokYsHrBCkFLTelQtQCiSiZzlU[index].JMvhdTtcZbxgprakncjapyQeXNxe;
		}

		public ControllerElementIdentifier()
		{
		}

		public ControllerElementIdentifier(ControllerElementIdentifier P_0)
		{
			_id = P_0._id;
			_name = P_0._name;
			_positiveName = P_0._positiveName;
			_negativeName = P_0._negativeName;
			_key = P_0._key;
			_positiveKey = P_0._positiveKey;
			_negativeKey = P_0._negativeKey;
			_elementType = P_0._elementType;
			_compoundElementType = P_0._compoundElementType;
			_role = P_0._role;
			if (P_0.NykokYsHrBCkFLTelQtQCiSiZzlU != null)
			{
				int count = P_0.NykokYsHrBCkFLTelQtQCiSiZzlU.Count;
				NykokYsHrBCkFLTelQtQCiSiZzlU = new List<uUywiGAizPqSdAxUNrSksLgDLhAW>(count);
				for (int i = 0; i < count; i++)
				{
					if (P_0.NykokYsHrBCkFLTelQtQCiSiZzlU[i] != null)
					{
						NykokYsHrBCkFLTelQtQCiSiZzlU.Add(new uUywiGAizPqSdAxUNrSksLgDLhAW(P_0.NykokYsHrBCkFLTelQtQCiSiZzlU[i]));
					}
				}
			}
			KAMNpyLpNfVKiMewNWCqQJygPUEB = P_0.KAMNpyLpNfVKiMewNWCqQJygPUEB;
			ouQfNUIylDcDMjuAbQZwMqnvKKgKA = P_0.ouQfNUIylDcDMjuAbQZwMqnvKKgKA;
		}

		internal ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initOptions");
			}
			_id = P_0.id;
			_name = P_0.name;
			_positiveName = P_0.positiveName;
			_negativeName = P_0.negativeName;
			_key = P_0.key;
			_positiveKey = P_0.positiveKey;
			_negativeKey = P_0.negativeKey;
			_elementType = P_0.elementType;
			_compoundElementType = P_0.compoundElementType;
			_role = P_0.role;
		}

		[Obsolete("Used by plugins for mouse controllers. Left for plugin compatibility. Do not use.", false)]
		internal ControllerElementIdentifier(int P_0, string P_1, string P_2, string P_3, ControllerElementType P_4, CompoundControllerElementType P_5, bool P_6)
		{
			_id = P_0;
			_name = P_1;
			_positiveName = P_2;
			_negativeName = P_3;
			if (P_0 < Consts.commonMouseElementIdentifierInitOptions.Length && string.Equals(Consts.commonMouseElementIdentifierInitOptions[P_0].name, P_1, StringComparison.Ordinal))
			{
				_key = Consts.commonMouseElementIdentifierInitOptions[P_0].key;
				_positiveKey = Consts.commonMouseElementIdentifierInitOptions[P_0].key;
				_negativeKey = Consts.commonMouseElementIdentifierInitOptions[P_0].key;
			}
			_elementType = P_4;
			_compoundElementType = P_5;
		}

		[Obsolete("Used by UnifiedKeyboardSource. Left for plugin compatibility. Do not use.", false)]
		internal ControllerElementIdentifier(int P_0, string P_1, string P_2, string P_3, ControllerElementType P_4, bool P_5)
		{
			_id = P_0;
			_name = P_1;
			_positiveName = P_2;
			_negativeName = P_3;
			if (P_4 == ControllerElementType.Button && P_0 < Consts.keyboardKeyNames.Count && string.Equals(Consts.keyboardKeyNames[P_0], P_1, StringComparison.Ordinal))
			{
				_key = Consts.keyboardKeyKeys[P_0];
			}
			_elementType = P_4;
			_compoundElementType = CompoundControllerElementType.Axis2D;
		}

		internal ControllerElementIdentifier(ControllerElementIdentifier P_0, bool P_1, ControllerElementType P_2)
			: this(P_0)
		{
			_elementType = P_2;
		}

		public ControllerElementIdentifier Clone()
		{
			return new ControllerElementIdentifier(this);
		}

		public string GetDisplayName(ControllerElementType actualElementType, AxisRange axisRange)
		{
			return actualElementType switch
			{
				ControllerElementType.Axis => axisRange switch
				{
					AxisRange.Full => ((IControllerElementIdentifierCommon_Internal)this).name, 
					AxisRange.Positive => ((IControllerElementIdentifierCommon_Internal)this).positiveName, 
					AxisRange.Negative => ((IControllerElementIdentifierCommon_Internal)this).negativeName, 
					_ => throw new NotImplementedException(), 
				}, 
				ControllerElementType.Button => ((IControllerElementIdentifierCommon_Internal)this).name, 
				ControllerElementType.CompoundElement => ((IControllerElementIdentifierCommon_Internal)this).name, 
				_ => throw new NotImplementedException(), 
			};
		}

		public string GetDisplayName(AxisRange axisRange)
		{
			return GetDisplayName(_elementType, axisRange);
		}

		public object GetGlyph(ControllerElementType actualElementType, AxisRange axisRange)
		{
			return actualElementType switch
			{
				ControllerElementType.Axis => axisRange switch
				{
					AxisRange.Full => glyph, 
					AxisRange.Positive => positiveGlyph, 
					AxisRange.Negative => negativeGlyph, 
					_ => throw new NotImplementedException(), 
				}, 
				ControllerElementType.Button => glyph, 
				ControllerElementType.CompoundElement => glyph, 
				_ => throw new NotImplementedException(), 
			};
		}

		public object GetGlyph(AxisRange axisRange)
		{
			return GetGlyph(_elementType, axisRange);
		}

		public string GetFinalGlyphKey(ControllerElementType actualElementType, AxisRange axisRange)
		{
			return actualElementType switch
			{
				ControllerElementType.Axis => axisRange switch
				{
					AxisRange.Full => finalGlyphKey, 
					AxisRange.Positive => finalPositiveGlyphKey, 
					AxisRange.Negative => finalNegativeGlyphKey, 
					_ => throw new NotImplementedException(), 
				}, 
				ControllerElementType.Button => finalGlyphKey, 
				ControllerElementType.CompoundElement => finalGlyphKey, 
				_ => throw new NotImplementedException(), 
			};
		}

		public string GetFinalGlyphKey(AxisRange axisRange)
		{
			return GetFinalGlyphKey(_elementType, axisRange);
		}

		private void lvNDXDTiggdWQKhNgHvlTXYGDETl()
		{
			if (ANNDqGGSoxQLhWsOwqlusgHHsExU)
			{
				throw new Exception("The object is marked readonly and you are trying to modify its values.");
			}
		}

		internal void FinishRuntimeSetup(DeviceLocalizationInfo deviceLocalizationInfo, ControllerType controllerType)
		{
			ouQfNUIylDcDMjuAbQZwMqnvKKgKA = controllerType;
			ToElementNameLocalizerTypes(_elementType, _compoundElementType, out var resultElementType, out var resultCompoundElementType);
			int num = FDNFDGKMldROgCHjPdSVTnUzAnLgb.FaWRBKagAChKzYIPTHxodNThJXtKA(resultElementType, resultCompoundElementType);
			if (num > 0)
			{
				NykokYsHrBCkFLTelQtQCiSiZzlU = new List<uUywiGAizPqSdAxUNrSksLgDLhAW>(num);
				for (int i = 0; i < num; i++)
				{
					NykokYsHrBCkFLTelQtQCiSiZzlU.Add(new uUywiGAizPqSdAxUNrSksLgDLhAW());
				}
			}
			dsZBhmUpjvOCLSRrMEuwpKKZoEIV = deviceLocalizationInfo;
			CdRZNurhXCElSrCZlBerZAEXBnkO = bUlLQaUKfECmSjzpJPefXKFSSdNK.cCTfXYLFSzPkENyQhWfldKSJqEMn(this, wAIJUtJnRDeoKIthCveEEyAfjePsA.EvNqTyCxDnMyOBrFuMukHLvYIQjU(controllerType), resultElementType, resultCompoundElementType, _id, deviceLocalizationInfo);
			NCbONvNdKnWMvfwJINTjvIxnLIeC = UFCQEdfDnGpykVRzRZqGOgJZsGeF.KZYZHudLuurLkOoeUyaKpOFAazwB(this, wAIJUtJnRDeoKIthCveEEyAfjePsA.EvNqTyCxDnMyOBrFuMukHLvYIQjU(controllerType), resultElementType, resultCompoundElementType, _id, deviceLocalizationInfo);
			if (_elementType == ControllerElementType.CompoundElement)
			{
				GYbILdZjgQGetrBszduDViYyCzFaA = QxUxEBAJtBeaHnGkrHvfxKcdhxVz.lubRiqaUfZAbEgbHLKaegqebcruE(this, wAIJUtJnRDeoKIthCveEEyAfjePsA.EvNqTyCxDnMyOBrFuMukHLvYIQjU(controllerType), resultElementType, resultCompoundElementType, _id, deviceLocalizationInfo);
				wgyAorGeaTdBTCbeQIenkRBJdBkt = GQKseIhIzoPbpARMcpUcDlfHmRfe.SXKbSkgrRVdZMXyXblaFkhuLSYWz(this, wAIJUtJnRDeoKIthCveEEyAfjePsA.EvNqTyCxDnMyOBrFuMukHLvYIQjU(controllerType), resultElementType, resultCompoundElementType, _id, deviceLocalizationInfo);
			}
		}

		internal static void ToElementNameLocalizerTypes(ControllerElementType type, CompoundControllerElementType compoundType, out FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW resultElementType, out FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR resultCompoundElementType)
		{
			resultCompoundElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.None;
			switch (type)
			{
			case ControllerElementType.Axis:
				resultElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Axis;
				break;
			case ControllerElementType.Button:
				resultElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Button;
				break;
			case ControllerElementType.CompoundElement:
				resultElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement;
				switch (compoundType)
				{
				case CompoundControllerElementType.Axis2D:
					resultCompoundElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.Axis2D;
					break;
				case CompoundControllerElementType.Hat:
					resultCompoundElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.Hat;
					break;
				case CompoundControllerElementType.DPad:
					resultCompoundElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.DPad;
					break;
				default:
					resultElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Unknown;
					break;
				}
				break;
			default:
				resultElementType = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Unknown;
				break;
			}
		}

		string bOFZUEPNgDgQSavjlJvfJaMptbnQA.GetSpecialElementNonLocalizedDescriptiveName(int index)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU == null || index >= NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				return null;
			}
			return NykokYsHrBCkFLTelQtQCiSiZzlU[index].JwjgBCrVfDPJFXiGRXcbkRlDppXN;
		}

		void bOFZUEPNgDgQSavjlJvfJaMptbnQA.SetSpecialElementNonLocalizedDescriptiveName(int index, string value)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU != null && index < NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				NykokYsHrBCkFLTelQtQCiSiZzlU[index].JwjgBCrVfDPJFXiGRXcbkRlDppXN = value;
			}
		}

		string bOFZUEPNgDgQSavjlJvfJaMptbnQA.GetSpecialElementKey(int index)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU == null || index >= NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				return null;
			}
			return NykokYsHrBCkFLTelQtQCiSiZzlU[index].JMvhdTtcZbxgprakncjapyQeXNxe;
		}

		void bOFZUEPNgDgQSavjlJvfJaMptbnQA.SetSpecialElementKey(int index, string value)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU != null && index < NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				NykokYsHrBCkFLTelQtQCiSiZzlU[index].JMvhdTtcZbxgprakncjapyQeXNxe = value;
			}
		}

		string qRYbqBqElSaesizKXsECcXURAVVeb.GetSpecialElementKey(int index)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU == null || index >= NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				return null;
			}
			return NykokYsHrBCkFLTelQtQCiSiZzlU[index].JMvhdTtcZbxgprakncjapyQeXNxe;
		}

		void qRYbqBqElSaesizKXsECcXURAVVeb.SetSpecialElementKey(int index, string value)
		{
			if (NykokYsHrBCkFLTelQtQCiSiZzlU != null && index < NykokYsHrBCkFLTelQtQCiSiZzlU.Count)
			{
				NykokYsHrBCkFLTelQtQCiSiZzlU[index].JMvhdTtcZbxgprakncjapyQeXNxe = value;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Glyphs;
using Rewired.Internal.Localization;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public class ControllerTemplateElementIdentifier : IControllerElementIdentifierCommon_Internal, IControllerTemplateElementIdentifier, lOhdpMIGSdyahJLjLKbbeUkHQJxnB, bguKJVtsagJfXPpJQeurpzlOLIYd, vRDWZTvhTxtVFxhZXNeeonctREwv, HQqbZoQigscgVQcdQGCMdxuNvzzS, bOFZUEPNgDgQSavjlJvfJaMptbnQA, qRYbqBqElSaesizKXsECcXURAVVeb
	{
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		internal class jWtiLZTUMxqanbFwvDoMEXpHMAeRA
		{
			public int id;

			public string name;

			public string positiveName;

			public string negativeName;

			public string key;

			public string positiveKey;

			public string negativeKey;

			public ControllerTemplateElementType elementType;
		}

		internal sealed class KJOGhtbCWHYIMNeHPDRgnNjpKzzM
		{
			[Serializable]
			private sealed class YhtBFADsdUQDezNRyczxcsTDnFSwb
			{
				public static readonly YhtBFADsdUQDezNRyczxcsTDnFSwb _003C_003E9 = new YhtBFADsdUQDezNRyczxcsTDnFSwb();

				public static Func<ControllerTemplateElementIdentifier, ControllerTemplateElementIdentifier, bool> _003C_003E9__4_0;

				internal bool iObbkwsALZyvWGuvFaVYIjAQFGhx(ControllerTemplateElementIdentifier P_0, ControllerTemplateElementIdentifier P_1)
				{
					if (P_0 == null || P_1 == null)
					{
						return false;
					}
					if (P_0 != null && P_1 != null && P_0.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == P_1.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid && P_0.Rewired_002EInterfaces_002EIControllerTemplateElementIdentifier_002EelementType == P_1.Rewired_002EInterfaces_002EIControllerTemplateElementIdentifier_002EelementType)
					{
						return string.Equals(P_0.key, P_1.key, StringComparison.Ordinal);
					}
					return false;
				}
			}

			private static KJOGhtbCWHYIMNeHPDRgnNjpKzzM ZwjnARtdpIugojUCsxanIaqoAPBS;

			private readonly global::JZpDMkieCQWHnBUBlnLvccwpXKuI<ControllerTemplateElementIdentifier> rLzbxLnGnQwHcoAjudbpKGAQhvYr;

			private static KJOGhtbCWHYIMNeHPDRgnNjpKzzM WXddxdVHpjwKjjybfaveJnOorhSJ
			{
				get
				{
					if (ZwjnARtdpIugojUCsxanIaqoAPBS != null)
					{
						return ZwjnARtdpIugojUCsxanIaqoAPBS;
					}
					ZwjnARtdpIugojUCsxanIaqoAPBS = new KJOGhtbCWHYIMNeHPDRgnNjpKzzM();
					ZwjnARtdpIugojUCsxanIaqoAPBS.ABvQRPbOrqekdjDIelFWJNqIwGpN();
					return ZwjnARtdpIugojUCsxanIaqoAPBS;
				}
			}

			private KJOGhtbCWHYIMNeHPDRgnNjpKzzM()
			{
				rLzbxLnGnQwHcoAjudbpKGAQhvYr = new global::JZpDMkieCQWHnBUBlnLvccwpXKuI<ControllerTemplateElementIdentifier>(YhtBFADsdUQDezNRyczxcsTDnFSwb._003C_003E9.iObbkwsALZyvWGuvFaVYIjAQFGhx);
			}

			private void ABvQRPbOrqekdjDIelFWJNqIwGpN()
			{
				ReInput.ShutDownEvent += ZwjnARtdpIugojUCsxanIaqoAPBS.MRhcnGUCWGXhszIPWzIXJPiHenYGA;
			}

			private void MRhcnGUCWGXhszIPWzIXJPiHenYGA()
			{
				if (ZwjnARtdpIugojUCsxanIaqoAPBS == this)
				{
					ZwjnARtdpIugojUCsxanIaqoAPBS = null;
				}
				ReInput.ShutDownEvent -= MRhcnGUCWGXhszIPWzIXJPiHenYGA;
			}

			public static ControllerTemplateElementIdentifier QYbHbizNmxWuDEKGQnwdifrygNjZ(DeviceLocalizationInfo P_0, ControllerTemplateElementIdentifier P_1)
			{
				return WXddxdVHpjwKjjybfaveJnOorhSJ.rLzbxLnGnQwHcoAjudbpKGAQhvYr.gOGhAlpaDWHngyOvcwqBdSVIJQot(P_0.hash, P_1);
			}

			public static bool tAoOtQtEpTVwRHgkRThRXyhYPFVF(DeviceLocalizationInfo P_0, ControllerTemplateElementIdentifier P_1, out ControllerTemplateElementIdentifier P_2)
			{
				return WXddxdVHpjwKjjybfaveJnOorhSJ.rLzbxLnGnQwHcoAjudbpKGAQhvYr.UCVjRYAbAROoXGysMPODoXVTfPnm(P_0.hash, P_1, out P_2);
			}

			public static void mnAadYcNJqnJqhQbeWDVcbLdGqdAC(DeviceLocalizationInfo P_0, ControllerTemplateElementIdentifier P_1)
			{
				WXddxdVHpjwKjjybfaveJnOorhSJ.rLzbxLnGnQwHcoAjudbpKGAQhvYr.jwZfZMJFGsXepvnbcdwNeIsRDvtv(P_0.hash, P_1);
			}
		}

		private class IYsGErPHGxQZbPIwMAwULemfWYmP
		{
			[SerializeField]
			private string mnTFgBeixTIhxbEGIDRctjmnRuIeb;

			[SerializeField]
			private string NRwwSrERogeGpPZbofKSWSAEiPme;

			public string yIQEVDFUjFKymXhPRbPSRuaOGuX
			{
				get
				{
					return mnTFgBeixTIhxbEGIDRctjmnRuIeb;
				}
				set
				{
					mnTFgBeixTIhxbEGIDRctjmnRuIeb = text;
				}
			}

			public string JlONRptyvZTrJVuMiJbDxRYfuXnk
			{
				get
				{
					return NRwwSrERogeGpPZbofKSWSAEiPme;
				}
				set
				{
					NRwwSrERogeGpPZbofKSWSAEiPme = nRwwSrERogeGpPZbofKSWSAEiPme;
				}
			}

			public IYsGErPHGxQZbPIwMAwULemfWYmP()
			{
			}

			public IYsGErPHGxQZbPIwMAwULemfWYmP(IYsGErPHGxQZbPIwMAwULemfWYmP P_0)
			{
				mnTFgBeixTIhxbEGIDRctjmnRuIeb = P_0.mnTFgBeixTIhxbEGIDRctjmnRuIeb;
				NRwwSrERogeGpPZbofKSWSAEiPme = P_0.NRwwSrERogeGpPZbofKSWSAEiPme;
			}
		}

		private const string wfOcwgOrXyihUPQeXikoBGJyDOOM = "controller/template";

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
		public string _key;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		public string _positiveKey;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		public string _negativeKey;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ControllerTemplateElementType _elementType;

		[NonSerialized]
		private bUlLQaUKfECmSjzpJPefXKFSSdNK URyGeqknCfuQJavrNWwsqpuKvlBwA;

		[NonSerialized]
		private QxUxEBAJtBeaHnGkrHvfxKcdhxVz OyYLYcwhhdWnhpDRToiMHcLdSZOA;

		[NonSerialized]
		private UFCQEdfDnGpykVRzRZqGOgJZsGeF YtrrAMVPydvccDtgnZRsuTnOYmfK;

		[NonSerialized]
		private GQKseIhIzoPbpARMcpUcDlfHmRfe bqCbDdAGmKauRHlnsAyIaUyHEXQnA;

		[NonSerialized]
		private DeviceLocalizationInfo ahFwEjwlJVFRhhRUNzhfTNtnIPtZA;

		[NonSerialized]
		private int ndjvsrVpKQtPVLEwkvyhkQDNUjdC;

		[NonSerialized]
		private List<IYsGErPHGxQZbPIwMAwULemfWYmP> YvDuIauvEwMRyqiDNcZNiOrvMfRtA;

		int IControllerElementIdentifierCommon_Internal.id => _id;

		string IControllerElementIdentifierCommon_Internal.name
		{
			get
			{
				if (!ReInput.isReady || URyGeqknCfuQJavrNWwsqpuKvlBwA == null || !LocalizationManager.isEnabled)
				{
					return _name;
				}
				return URyGeqknCfuQJavrNWwsqpuKvlBwA.MpfwJMTclVnnxEuHhBPCmlxJadkBA;
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
				if (!ReInput.isReady || URyGeqknCfuQJavrNWwsqpuKvlBwA == null || !LocalizationManager.isEnabled)
				{
					return _positiveName;
				}
				return URyGeqknCfuQJavrNWwsqpuKvlBwA.ypoFNNDaQTVeKqsDkwEcIpgjZwGfA;
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
				if (!ReInput.isReady || URyGeqknCfuQJavrNWwsqpuKvlBwA == null || !LocalizationManager.isEnabled)
				{
					return _negativeName;
				}
				return URyGeqknCfuQJavrNWwsqpuKvlBwA.CIYbIIBbIoEXdOMvSfyWNziRLZjc;
			}
			internal set
			{
				nonLocalizedNegativeName = value;
			}
		}

		ControllerTemplateElementType IControllerTemplateElementIdentifier.elementType => _elementType;

		internal virtual bool useEditorElementTypeOverride => false;

		internal virtual ControllerElementType editorElementTypeOverride
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public object glyph
		{
			get
			{
				if (!ReInput.isReady || YtrrAMVPydvccDtgnZRsuTnOYmfK == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return YtrrAMVPydvccDtgnZRsuTnOYmfK.VMAqNtDPmJybBXpPjPFicHThBxQD;
			}
		}

		public object positiveGlyph
		{
			get
			{
				if (!ReInput.isReady || YtrrAMVPydvccDtgnZRsuTnOYmfK == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return YtrrAMVPydvccDtgnZRsuTnOYmfK.ivgGbvjljGLQSbxeeHhSPwvwKzZb;
			}
		}

		public object negativeGlyph
		{
			get
			{
				if (!ReInput.isReady || YtrrAMVPydvccDtgnZRsuTnOYmfK == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return YtrrAMVPydvccDtgnZRsuTnOYmfK.yLUKfKXXayZNlhzpjLtAckROOAOB;
			}
		}

		private string finalGlyphKey
		{
			get
			{
				if (!ReInput.isReady || YtrrAMVPydvccDtgnZRsuTnOYmfK == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return YtrrAMVPydvccDtgnZRsuTnOYmfK.QDOOogoJEZzDwhcfyDMVrQliiZaQ;
			}
		}

		private string finalPositiveGlyphKey
		{
			get
			{
				if (!ReInput.isReady || YtrrAMVPydvccDtgnZRsuTnOYmfK == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return YtrrAMVPydvccDtgnZRsuTnOYmfK.YHLjcmxgqGUnWoepoFwHlpXwGlOQ;
			}
		}

		private string finalNegativeGlyphKey
		{
			get
			{
				if (!ReInput.isReady || YtrrAMVPydvccDtgnZRsuTnOYmfK == null || !GlyphManager.isEnabled)
				{
					return null;
				}
				return YtrrAMVPydvccDtgnZRsuTnOYmfK.BOHsCXxxroVnAbuhrSFCqvZoizFN;
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
				if (ReInput.isReady && URyGeqknCfuQJavrNWwsqpuKvlBwA != null)
				{
					URyGeqknCfuQJavrNWwsqpuKvlBwA.YSgvMmquHVoFhixWnSsVWmcflge();
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
				if (ReInput.isReady && URyGeqknCfuQJavrNWwsqpuKvlBwA != null)
				{
					URyGeqknCfuQJavrNWwsqpuKvlBwA.nFzBoFXBNysVIMLFgBueCHpnWLDe();
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
				if (ReInput.isReady && URyGeqknCfuQJavrNWwsqpuKvlBwA != null)
				{
					URyGeqknCfuQJavrNWwsqpuKvlBwA.GjiLKqPVUrrLpJzFcrhtOPEtLQgk();
				}
			}
		}

		public string key => _key;

		public string positiveKey => _positiveKey;

		public string negativeKey => _negativeKey;

		string IControllerElementIdentifierCommon_Internal.nonLocalizedName => nonLocalizedName;

		string IControllerElementIdentifierCommon_Internal.nonLocalizedPositiveName => nonLocalizedPositiveName;

		string IControllerElementIdentifierCommon_Internal.nonLocalizedNegativeName => nonLocalizedNegativeName;

		bool IControllerElementIdentifierCommon_Internal.isNonLocalizedPositiveNameAutoGenerated => (ndjvsrVpKQtPVLEwkvyhkQDNUjdC & 2) != 0;

		bool IControllerElementIdentifierCommon_Internal.isNonLocalizedNegativeNameAutoGenerated => (ndjvsrVpKQtPVLEwkvyhkQDNUjdC & 4) != 0;

		bool IControllerElementIdentifierCommon_Internal.isPositiveKeyAutoGenerated => (ndjvsrVpKQtPVLEwkvyhkQDNUjdC & 8) != 0;

		bool IControllerElementIdentifierCommon_Internal.isNegativeKeyAutoGenerated => (ndjvsrVpKQtPVLEwkvyhkQDNUjdC & 0x10) != 0;

		string IControllerElementIdentifierCommon_Internal.key => _key;

		string IControllerElementIdentifierCommon_Internal.positiveKey => _positiveKey;

		string IControllerElementIdentifierCommon_Internal.negativeKey => _negativeKey;

		DeviceLocalizationInfo IControllerElementIdentifierCommon_Internal.deviceLocalizationInfo => ahFwEjwlJVFRhhRUNzhfTNtnIPtZA;

		object IControllerElementIdentifierCommon_Internal.elementType => _elementType;

		bool IControllerElementIdentifierCommon_Internal.useEditorElementTypeOverride => useEditorElementTypeOverride;

		ControllerElementType IControllerElementIdentifierCommon_Internal.editorElementTypeOverride => editorElementTypeOverride;

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
				_negativeKey = value;
			}
		}

		int bguKJVtsagJfXPpJQeurpzlOLIYd.autoGeneratedValueFlags
		{
			get
			{
				return ndjvsrVpKQtPVLEwkvyhkQDNUjdC;
			}
			set
			{
				ndjvsrVpKQtPVLEwkvyhkQDNUjdC = value;
			}
		}

		string HQqbZoQigscgVQcdQGCMdxuNvzzS.keyCategory => "controller/template";

		string HQqbZoQigscgVQcdQGCMdxuNvzzS.key => _key;

		int HQqbZoQigscgVQcdQGCMdxuNvzzS.autoGeneratedValueFlags
		{
			get
			{
				return ndjvsrVpKQtPVLEwkvyhkQDNUjdC;
			}
			set
			{
				ndjvsrVpKQtPVLEwkvyhkQDNUjdC = value;
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
				_negativeKey = value;
			}
		}

		internal string GetCompoundElementSpecialName(int index)
		{
			if (!ReInput.isReady || !LocalizationManager.isEnabled || YvDuIauvEwMRyqiDNcZNiOrvMfRtA == null || OyYLYcwhhdWnhpDRToiMHcLdSZOA == null)
			{
				return string.Empty;
			}
			return OyYLYcwhhdWnhpDRToiMHcLdSZOA.UZtpxFUPQZBSvGgnZjNTrWXUAlHF(index);
		}

		internal object GetCompoundElementSpecialGlyph(int index)
		{
			if (!ReInput.isReady || !GlyphManager.isEnabled || YvDuIauvEwMRyqiDNcZNiOrvMfRtA == null || bqCbDdAGmKauRHlnsAyIaUyHEXQnA == null)
			{
				return null;
			}
			return bqCbDdAGmKauRHlnsAyIaUyHEXQnA.XmlgGslFtGJifkBoPmYBBdHirZpm(index);
		}

		string IControllerElementIdentifierCommon_Internal.GetSpecialElementNonLocalizedName(int index)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA == null || (uint)index >= (uint)YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				return null;
			}
			return YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].JlONRptyvZTrJVuMiJbDxRYfuXnk;
		}

		string IControllerElementIdentifierCommon_Internal.GetSpecialElementKey(int index)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA == null || (uint)index >= (uint)YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				return null;
			}
			return YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].yIQEVDFUjFKymXhPRbPSRuaOGuX;
		}

		public ControllerTemplateElementIdentifier()
		{
		}

		public ControllerTemplateElementIdentifier(ControllerTemplateElementIdentifier P_0)
		{
			_id = P_0._id;
			_name = P_0._name;
			_positiveName = P_0._positiveName;
			_negativeName = P_0._negativeName;
			_key = P_0._key;
			_positiveKey = P_0._positiveKey;
			_negativeKey = P_0._negativeKey;
			_elementType = P_0._elementType;
			if (P_0.YvDuIauvEwMRyqiDNcZNiOrvMfRtA != null)
			{
				int count = P_0.YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count;
				YvDuIauvEwMRyqiDNcZNiOrvMfRtA = new List<IYsGErPHGxQZbPIwMAwULemfWYmP>(count);
				for (int i = 0; i < count; i++)
				{
					if (P_0.YvDuIauvEwMRyqiDNcZNiOrvMfRtA[i] != null)
					{
						YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Add(new IYsGErPHGxQZbPIwMAwULemfWYmP(P_0.YvDuIauvEwMRyqiDNcZNiOrvMfRtA[i]));
					}
				}
			}
			ndjvsrVpKQtPVLEwkvyhkQDNUjdC = P_0.ndjvsrVpKQtPVLEwkvyhkQDNUjdC;
		}

		internal ControllerTemplateElementIdentifier(jWtiLZTUMxqanbFwvDoMEXpHMAeRA P_0)
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
		}

		internal ControllerTemplateElementIdentifier(ControllerTemplateElementIdentifier P_0, ControllerTemplateElementType P_1, bool P_2)
			: this(P_0)
		{
			_elementType = P_1;
		}

		public virtual ControllerTemplateElementIdentifier Clone()
		{
			return new ControllerTemplateElementIdentifier(this);
		}

		public string GetDisplayName(AxisRange axisRange)
		{
			return _elementType switch
			{
				ControllerTemplateElementType.Axis => axisRange switch
				{
					AxisRange.Full => Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename, 
					AxisRange.Positive => Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EpositiveName, 
					AxisRange.Negative => Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EnegativeName, 
					_ => throw new NotImplementedException(), 
				}, 
				_ => Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename, 
			};
		}

		public object GetGlyph(AxisRange axisRange)
		{
			return _elementType switch
			{
				ControllerTemplateElementType.Axis => axisRange switch
				{
					AxisRange.Full => glyph, 
					AxisRange.Positive => positiveGlyph, 
					AxisRange.Negative => negativeGlyph, 
					_ => throw new NotImplementedException(), 
				}, 
				_ => glyph, 
			};
		}

		public string GetFinalGlyphKey(AxisRange axisRange)
		{
			return _elementType switch
			{
				ControllerTemplateElementType.Axis => axisRange switch
				{
					AxisRange.Full => finalGlyphKey, 
					AxisRange.Positive => finalPositiveGlyphKey, 
					AxisRange.Negative => finalNegativeGlyphKey, 
					_ => throw new NotImplementedException(), 
				}, 
				_ => finalGlyphKey, 
			};
		}

		internal ControllerElementIdentifier ToControllerElementIdentifier(IHardwareControllerMap_Internal hardwareControllerMap)
		{
			ControllerElementIdentifier controllerElementIdentifier = new ControllerElementIdentifier(new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
			{
				id = _id,
				name = _name,
				positiveName = _positiveName,
				negativeName = _negativeName,
				key = _key,
				positiveKey = _positiveKey,
				negativeKey = _negativeKey,
				elementType = cVDyIiOsEfJNYzVuZSmuEXqylgT.fGabLRkepWcASbiyYXkBQHQnmBxI(_elementType),
				compoundElementType = CompoundControllerElementType.Axis2D
			});
			if (ReInput.isReady && ahFwEjwlJVFRhhRUNzhfTNtnIPtZA != null && hardwareControllerMap != null)
			{
				DeviceLocalizationInfo deviceLocalizationInfo = new DeviceLocalizationInfo(hardwareControllerMap.controllerType, false, hardwareControllerMap.typeGuid, new List<string> { hardwareControllerMap.typeKey }, null);
				deviceLocalizationInfo.FinishRuntimeSetup();
				controllerElementIdentifier.FinishRuntimeSetup(deviceLocalizationInfo, hardwareControllerMap.controllerType);
			}
			return controllerElementIdentifier;
		}

		internal void FinishRuntimeSetup(DeviceLocalizationInfo deviceLocalizationInfo)
		{
			jCBTuiXYIaoFMZKxJNnVDvsReYcg(_elementType, out var sztzDKprOgaEtSRoFjITTczsHDuW, out var lsWebCorzTdhEUjUrAlgVzPmJJHR);
			int num = FDNFDGKMldROgCHjPdSVTnUzAnLgb.FaWRBKagAChKzYIPTHxodNThJXtKA(sztzDKprOgaEtSRoFjITTczsHDuW, lsWebCorzTdhEUjUrAlgVzPmJJHR);
			if (num > 0)
			{
				YvDuIauvEwMRyqiDNcZNiOrvMfRtA = new List<IYsGErPHGxQZbPIwMAwULemfWYmP>(num);
				for (int i = 0; i < num; i++)
				{
					YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Add(new IYsGErPHGxQZbPIwMAwULemfWYmP());
				}
			}
			ahFwEjwlJVFRhhRUNzhfTNtnIPtZA = deviceLocalizationInfo;
			if (URyGeqknCfuQJavrNWwsqpuKvlBwA == null)
			{
				URyGeqknCfuQJavrNWwsqpuKvlBwA = bUlLQaUKfECmSjzpJPefXKFSSdNK.cCTfXYLFSzPkENyQhWfldKSJqEMn(this, cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate, sztzDKprOgaEtSRoFjITTczsHDuW, lsWebCorzTdhEUjUrAlgVzPmJJHR, _id, deviceLocalizationInfo);
			}
			if (YtrrAMVPydvccDtgnZRsuTnOYmfK == null)
			{
				YtrrAMVPydvccDtgnZRsuTnOYmfK = UFCQEdfDnGpykVRzRZqGOgJZsGeF.KZYZHudLuurLkOoeUyaKpOFAazwB(this, cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate, sztzDKprOgaEtSRoFjITTczsHDuW, lsWebCorzTdhEUjUrAlgVzPmJJHR, _id, deviceLocalizationInfo);
			}
			if (sztzDKprOgaEtSRoFjITTczsHDuW == FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement)
			{
				OyYLYcwhhdWnhpDRToiMHcLdSZOA = QxUxEBAJtBeaHnGkrHvfxKcdhxVz.lubRiqaUfZAbEgbHLKaegqebcruE(this, cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate, sztzDKprOgaEtSRoFjITTczsHDuW, lsWebCorzTdhEUjUrAlgVzPmJJHR, _id, deviceLocalizationInfo);
				bqCbDdAGmKauRHlnsAyIaUyHEXQnA = GQKseIhIzoPbpARMcpUcDlfHmRfe.SXKbSkgrRVdZMXyXblaFkhuLSYWz(this, cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate, sztzDKprOgaEtSRoFjITTczsHDuW, lsWebCorzTdhEUjUrAlgVzPmJJHR, _id, deviceLocalizationInfo);
			}
		}

		string bOFZUEPNgDgQSavjlJvfJaMptbnQA.GetSpecialElementNonLocalizedDescriptiveName(int index)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA == null || index >= YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				return null;
			}
			return YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].JlONRptyvZTrJVuMiJbDxRYfuXnk;
		}

		void bOFZUEPNgDgQSavjlJvfJaMptbnQA.SetSpecialElementNonLocalizedDescriptiveName(int index, string value)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA != null && index < YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].JlONRptyvZTrJVuMiJbDxRYfuXnk = value;
			}
		}

		string bOFZUEPNgDgQSavjlJvfJaMptbnQA.GetSpecialElementKey(int index)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA == null || index >= YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				return null;
			}
			return YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].yIQEVDFUjFKymXhPRbPSRuaOGuX;
		}

		void bOFZUEPNgDgQSavjlJvfJaMptbnQA.SetSpecialElementKey(int index, string value)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA != null && index < YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].yIQEVDFUjFKymXhPRbPSRuaOGuX = value;
			}
		}

		string qRYbqBqElSaesizKXsECcXURAVVeb.GetSpecialElementKey(int index)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA == null || index >= YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				return null;
			}
			return YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].yIQEVDFUjFKymXhPRbPSRuaOGuX;
		}

		void qRYbqBqElSaesizKXsECcXURAVVeb.SetSpecialElementKey(int index, string value)
		{
			if (YvDuIauvEwMRyqiDNcZNiOrvMfRtA != null && index < YvDuIauvEwMRyqiDNcZNiOrvMfRtA.Count)
			{
				YvDuIauvEwMRyqiDNcZNiOrvMfRtA[index].yIQEVDFUjFKymXhPRbPSRuaOGuX = value;
			}
		}

		private static void jCBTuiXYIaoFMZKxJNnVDvsReYcg(ControllerTemplateElementType P_0, out FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW P_1, out FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR P_2)
		{
			P_2 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.None;
			switch (P_0)
			{
			case ControllerTemplateElementType.Axis:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Axis;
				break;
			case ControllerTemplateElementType.Button:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Button;
				break;
			case ControllerTemplateElementType.Hat:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement;
				P_2 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.Hat;
				break;
			case ControllerTemplateElementType.DPad:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement;
				P_2 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.DPad;
				break;
			case ControllerTemplateElementType.ThumbStick:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement;
				break;
			case ControllerTemplateElementType.Yoke:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Unknown;
				break;
			case ControllerTemplateElementType.Throttle:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Unknown;
				break;
			case ControllerTemplateElementType.Stick:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement;
				P_2 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.Stick;
				break;
			case ControllerTemplateElementType.Stick6D:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.CompoundElement;
				P_2 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.Stick6D;
				break;
			default:
				P_1 = FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Unknown;
				break;
			}
		}
	}
}

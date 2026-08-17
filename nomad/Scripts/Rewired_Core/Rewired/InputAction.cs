using System;
using Rewired.Internal.Localization;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public sealed class InputAction : lOhdpMIGSdyahJLjLKbbeUkHQJxnB, bguKJVtsagJfXPpJQeurpzlOLIYd
	{
		private const string keyCategory = "action";

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _id;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _name;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private InputActionType _type;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _descriptiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _positiveDescriptiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _negativeDescriptiveName;

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
		private int _behaviorId;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _userAssignable;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _categoryId;

		[NonSerialized]
		private jHBMHoxKsVfTUeGypnGiuvTLAYrM DFTIsmnQKeBwedapjGfXSqCVSpqQ;

		[NonSerialized]
		private int GBwfccEhzeMlqCJZIJRiYbVbKDhrc;

		public int id
		{
			get
			{
				return _id;
			}
			internal set
			{
				_id = num;
			}
		}

		public string name
		{
			get
			{
				return _name;
			}
			internal set
			{
				_name = text;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.mSjKwPJiyMmQSynHcMWYSOiGAmDB();
				}
			}
		}

		public InputActionType type
		{
			get
			{
				return _type;
			}
			internal set
			{
				_type = inputActionType;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.tnQKiUchqifinDnJuSQdboJqaQgK = cUiNUQrzyzJbULtIwBtgCRFdFSru(_type);
				}
			}
		}

		public string descriptiveName
		{
			get
			{
				if (!ReInput.isReady || !LocalizationManager.isEnabled || DFTIsmnQKeBwedapjGfXSqCVSpqQ == null)
				{
					return _descriptiveName;
				}
				return DFTIsmnQKeBwedapjGfXSqCVSpqQ.MpfwJMTclVnnxEuHhBPCmlxJadkBA;
			}
			internal set
			{
				nonLocalizedDescriptiveName = text;
			}
		}

		public string positiveDescriptiveName
		{
			get
			{
				if (!ReInput.isReady || !LocalizationManager.isEnabled || DFTIsmnQKeBwedapjGfXSqCVSpqQ == null)
				{
					return _positiveDescriptiveName;
				}
				return DFTIsmnQKeBwedapjGfXSqCVSpqQ.ypoFNNDaQTVeKqsDkwEcIpgjZwGfA;
			}
			internal set
			{
				nonLocalizedPositiveDescriptiveName = text;
			}
		}

		public string negativeDescriptiveName
		{
			get
			{
				if (!ReInput.isReady || !LocalizationManager.isEnabled || DFTIsmnQKeBwedapjGfXSqCVSpqQ == null)
				{
					return _negativeDescriptiveName;
				}
				return DFTIsmnQKeBwedapjGfXSqCVSpqQ.CIYbIIBbIoEXdOMvSfyWNziRLZjc;
			}
			internal set
			{
				nonLocalizedNegativeDescriptiveName = text;
			}
		}

		public int behaviorId
		{
			get
			{
				return _behaviorId;
			}
			internal set
			{
				_behaviorId = num;
			}
		}

		public int categoryId
		{
			get
			{
				return _categoryId;
			}
			internal set
			{
				_categoryId = num;
			}
		}

		public bool userAssignable
		{
			get
			{
				return _userAssignable;
			}
			internal set
			{
				_userAssignable = flag;
			}
		}

		[CustomObfuscation(rename = false)]
		internal string key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.hPAqnroLHIcbmjXldArpEMTOSiTdb();
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal string positiveKey
		{
			get
			{
				return _positiveKey;
			}
			set
			{
				_positiveKey = value;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.hbWgZphUlHKneWgRXAGuHIpOkDoyA();
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal string negativeKey
		{
			get
			{
				return _negativeKey;
			}
			set
			{
				_negativeKey = value;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.ERYkzklgqAvssoFtgdVhiVrfrvqpA();
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal string nonLocalizedDescriptiveName
		{
			get
			{
				return _descriptiveName;
			}
			set
			{
				_descriptiveName = value;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.YSgvMmquHVoFhixWnSsVWmcflge();
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal string nonLocalizedPositiveDescriptiveName
		{
			get
			{
				return _positiveDescriptiveName;
			}
			set
			{
				_positiveDescriptiveName = value;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.nFzBoFXBNysVIMLFgBueCHpnWLDe();
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal string nonLocalizedNegativeDescriptiveName
		{
			get
			{
				return _negativeDescriptiveName;
			}
			set
			{
				_negativeDescriptiveName = value;
				if (ReInput.isReady && DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
				{
					DFTIsmnQKeBwedapjGfXSqCVSpqQ.GjiLKqPVUrrLpJzFcrhtOPEtLQgk();
				}
			}
		}

		string bguKJVtsagJfXPpJQeurpzlOLIYd.keyCategory => "action";

		string bguKJVtsagJfXPpJQeurpzlOLIYd.scriptingName => _name;

		string bguKJVtsagJfXPpJQeurpzlOLIYd.nonLocalizedDescriptiveName
		{
			get
			{
				return _descriptiveName;
			}
			set
			{
				_descriptiveName = value;
			}
		}

		string lOhdpMIGSdyahJLjLKbbeUkHQJxnB.nonLocalizedPositiveDescriptiveName
		{
			get
			{
				return _positiveDescriptiveName;
			}
			set
			{
				_positiveDescriptiveName = value;
			}
		}

		string lOhdpMIGSdyahJLjLKbbeUkHQJxnB.nonLocalizedNegativeDescriptiveName
		{
			get
			{
				return _negativeDescriptiveName;
			}
			set
			{
				_negativeDescriptiveName = value;
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
				return GBwfccEhzeMlqCJZIJRiYbVbKDhrc;
			}
			set
			{
				GBwfccEhzeMlqCJZIJRiYbVbKDhrc = value;
			}
		}

		public InputAction()
		{
		}

		public InputAction(InputAction P_0)
		{
			_id = P_0._id;
			_name = P_0._name;
			_type = P_0._type;
			_descriptiveName = P_0._descriptiveName;
			_positiveDescriptiveName = P_0._positiveDescriptiveName;
			_negativeDescriptiveName = P_0._negativeDescriptiveName;
			_key = P_0._key;
			_positiveKey = P_0._positiveKey;
			_negativeKey = P_0._negativeKey;
			_behaviorId = P_0._behaviorId;
			_userAssignable = P_0._userAssignable;
			_categoryId = P_0.categoryId;
			GBwfccEhzeMlqCJZIJRiYbVbKDhrc = P_0.GBwfccEhzeMlqCJZIJRiYbVbKDhrc;
		}

		public InputAction Clone()
		{
			return new InputAction(this);
		}

		public string GetDisplayName(AxisRange axisRange)
		{
			return axisRange switch
			{
				AxisRange.Full => descriptiveName, 
				AxisRange.Positive => positiveDescriptiveName, 
				AxisRange.Negative => negativeDescriptiveName, 
				_ => throw new NotImplementedException(), 
			};
		}

		internal void UyNKzGYQQbcOHOZGyQybTlKHSqIA()
		{
			if (DFTIsmnQKeBwedapjGfXSqCVSpqQ == null)
			{
				DFTIsmnQKeBwedapjGfXSqCVSpqQ = jHBMHoxKsVfTUeGypnGiuvTLAYrM.DDwVoRtrTyixAYluCzsnZREiryUC(this, cUiNUQrzyzJbULtIwBtgCRFdFSru(_type), FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.None);
			}
		}

		internal void xSQboEuJLudqDiLbJTKEFhUJvsxmA()
		{
			if (DFTIsmnQKeBwedapjGfXSqCVSpqQ != null)
			{
				DFTIsmnQKeBwedapjGfXSqCVSpqQ = null;
			}
		}

		private static FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW cUiNUQrzyzJbULtIwBtgCRFdFSru(InputActionType P_0)
		{
			return P_0 switch
			{
				InputActionType.Axis => FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Axis, 
				InputActionType.Button => FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Button, 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}

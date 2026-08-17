using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public sealed class InputAction
	{
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int _id;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _name;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private InputActionType _type;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _descriptiveName;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private string _positiveDescriptiveName;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private string _negativeDescriptiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _behaviorId;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _userAssignable;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int _categoryId;

		[NonSerialized]
		private string uJxdPoFMyIUVGOelCDviThTttvnz;

		[NonSerialized]
		private string gywqCDtuIMyhdXDfRAomvPXuOuKl;

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
			}
		}

		public string descriptiveName
		{
			get
			{
				return _descriptiveName;
			}
			internal set
			{
				_descriptiveName = text;
			}
		}

		public string positiveDescriptiveName
		{
			get
			{
				if (!Application.isPlaying || !string.IsNullOrEmpty(_positiveDescriptiveName))
				{
					return _positiveDescriptiveName;
				}
				if (string.IsNullOrEmpty(uJxdPoFMyIUVGOelCDviThTttvnz))
				{
					uJxdPoFMyIUVGOelCDviThTttvnz = _descriptiveName + " +";
				}
				return uJxdPoFMyIUVGOelCDviThTttvnz;
			}
			internal set
			{
				_positiveDescriptiveName = text;
				uJxdPoFMyIUVGOelCDviThTttvnz = string.Empty;
			}
		}

		public string negativeDescriptiveName
		{
			get
			{
				if (!Application.isPlaying || !string.IsNullOrEmpty(_negativeDescriptiveName))
				{
					return _negativeDescriptiveName;
				}
				if (string.IsNullOrEmpty(gywqCDtuIMyhdXDfRAomvPXuOuKl))
				{
					gywqCDtuIMyhdXDfRAomvPXuOuKl = _descriptiveName + " -";
				}
				return gywqCDtuIMyhdXDfRAomvPXuOuKl;
			}
			internal set
			{
				_negativeDescriptiveName = text;
				gywqCDtuIMyhdXDfRAomvPXuOuKl = string.Empty;
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
			_behaviorId = P_0._behaviorId;
			_userAssignable = P_0._userAssignable;
			_categoryId = P_0.categoryId;
		}

		public InputAction Clone()
		{
			return new InputAction(this);
		}
	}
}

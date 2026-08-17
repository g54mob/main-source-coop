using System;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public class CustomControllerElementTarget
	{
		[CustomObfuscation(rename = false)]
		internal enum ValueRange
		{
			[CustomObfuscation(rename = false)]
			Full = 0,
			[CustomObfuscation(rename = false)]
			Positive = 1,
			[CustomObfuscation(rename = false)]
			Negative = 2
		}

		[Tooltip("The Custom Controller element.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementSelector _element = new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Axis
		};

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ValueRange _valueRange;

		[Tooltip("Should the final value be positive or negative?")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private Pole _valueContribution;

		[Tooltip("Should the final value be inverted?")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _invert;

		public CustomControllerElementSelector element => _element;

		public Pole valueContribution
		{
			get
			{
				return _valueContribution;
			}
			set
			{
				_valueContribution = value;
			}
		}

		internal ValueRange valueRange
		{
			get
			{
				return _valueRange;
			}
			set
			{
				_valueRange = value;
			}
		}

		public bool invert
		{
			get
			{
				return _invert;
			}
			set
			{
				_invert = value;
			}
		}

		internal CustomControllerElementTarget()
		{
		}

		internal CustomControllerElementTarget(CustomControllerElementSelector P_0)
		{
			_element = P_0;
		}

		internal void ClearElementCaches()
		{
			if (_element != null)
			{
				_element.ClearCache();
			}
		}
	}
}

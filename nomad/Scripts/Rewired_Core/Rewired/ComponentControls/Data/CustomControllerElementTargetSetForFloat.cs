using System;
using System.Reflection;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	[DefaultMember("Item")]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public class CustomControllerElementTargetSetForFloat : CustomControllerElementTargetSet
	{
		[Tooltip("Splits the value into positive and negative sides which can be assigned to different Custom Controller elements.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _splitValue;

		[Tooltip("The target element. This is unused if Split Value is enabled.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTarget _target = new CustomControllerElementTarget(new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Axis
		})
		{
			valueRange = CustomControllerElementTarget.ValueRange.Full
		};

		[Tooltip("The positive target element. This is unused if Split Value is not enabled.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTarget _positiveTarget = new CustomControllerElementTarget(new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Button
		})
		{
			valueRange = CustomControllerElementTarget.ValueRange.Positive,
			valueContribution = Pole.Positive
		};

		[Tooltip("The negative target element. This is unused if Split Value is not enabled.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTarget _negativeTarget = new CustomControllerElementTarget(new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Button
		})
		{
			valueRange = CustomControllerElementTarget.ValueRange.Negative,
			valueContribution = Pole.Positive
		};

		public bool splitValue
		{
			get
			{
				return _splitValue;
			}
			set
			{
				_splitValue = value;
			}
		}

		public CustomControllerElementTarget target => _target;

		public CustomControllerElementTarget positiveTarget => _positiveTarget;

		public CustomControllerElementTarget negativeTarget => _negativeTarget;

		int CustomControllerElementTargetSet.targetCount
		{
			get
			{
				if (!_splitValue)
				{
					return 1;
				}
				return 2;
			}
		}

		CustomControllerElementTarget CustomControllerElementTargetSet.this[int index]
		{
			get
			{
				if (_splitValue)
				{
					return index switch
					{
						0 => _positiveTarget, 
						1 => _negativeTarget, 
						_ => throw new IndexOutOfRangeException(), 
					};
				}
				if (index == 0)
				{
					return _target;
				}
				throw new IndexOutOfRangeException();
			}
		}

		internal CustomControllerElementTargetSetForFloat()
		{
		}

		internal CustomControllerElementTargetSetForFloat(CustomControllerElementTarget P_0)
		{
			_splitValue = false;
			_target = P_0;
		}

		internal CustomControllerElementTargetSetForFloat(CustomControllerElementTarget P_0, CustomControllerElementTarget P_1)
		{
			_splitValue = true;
			_positiveTarget = P_0;
			_negativeTarget = P_1;
		}

		internal override void ClearElementCaches()
		{
			if (_target != null)
			{
				_target.ClearElementCaches();
			}
		}
	}
}

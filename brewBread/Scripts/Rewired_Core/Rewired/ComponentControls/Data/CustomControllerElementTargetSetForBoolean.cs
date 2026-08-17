using System;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public class CustomControllerElementTargetSetForBoolean : CustomControllerElementTargetSet
	{
		private const int FEYqngSpWHCylzGRtugricMIeqFB = 1;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The target element.")]
		private CustomControllerElementTarget _target = new CustomControllerElementTarget(new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Button
		})
		{
			valueRange = CustomControllerElementTarget.ValueRange.Positive,
			valueContribution = Pole.Positive
		};

		public CustomControllerElementTarget target => _target;

		internal override int targetCount => 1;

		internal override CustomControllerElementTarget this[int index]
		{
			get
			{
				if (index == 0)
				{
					return _target;
				}
				throw new IndexOutOfRangeException();
			}
		}

		internal CustomControllerElementTargetSetForBoolean()
		{
		}

		internal CustomControllerElementTargetSetForBoolean(CustomControllerElementTarget P_0)
		{
			_target = P_0;
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

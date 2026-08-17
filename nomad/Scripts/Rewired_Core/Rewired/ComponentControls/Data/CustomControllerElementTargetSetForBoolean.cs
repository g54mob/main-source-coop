using System;
using System.Reflection;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	[DefaultMember("Item")]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public class CustomControllerElementTargetSetForBoolean : CustomControllerElementTargetSet
	{
		private const int wzXOoDDSdDnzUHtaKdBoihGUJWak = 1;

		[Tooltip("The target element.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTarget _target = new CustomControllerElementTarget(new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Button
		})
		{
			valueRange = CustomControllerElementTarget.ValueRange.Positive,
			valueContribution = Pole.Positive
		};

		public CustomControllerElementTarget target => _target;

		int CustomControllerElementTargetSet.targetCount => 1;

		CustomControllerElementTarget CustomControllerElementTargetSet.this[int index]
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

using TMPro;
using UnityEngine;

namespace Features.UINavigationModuleRealization.Scripts
{
	public class DropdownBackNavigationCallback : UIBackNavigationCallback
	{
		[SerializeField]
		private TMP_Dropdown _dropdown;

		public override bool CanHandleBack()
		{
			if (IsSelected())
			{
				return _dropdown.IsExpanded;
			}
			return false;
		}

		public override void OnBack()
		{
			_dropdown.Hide();
		}
	}
}

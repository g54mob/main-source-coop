using System;
using TMPro;
using UnityEngine.EventSystems;

namespace Features.CustomUiElementsModule.Scripts
{
	public class DropdownWithShowCallback : TMP_Dropdown
	{
		public Action OnDropdownShown;

		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			OnDropdownShown?.Invoke();
		}
	}
}

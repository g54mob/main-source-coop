using UnityEngine;
using UnityEngine.EventSystems;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class TMPDropdownItemSelectable : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler
	{
		private DropdownContentWithoutSelectingOnPointerEnter _owner;

		private bool _pointerInside;

		public void Initialize(DropdownContentWithoutSelectingOnPointerEnter owner)
		{
			_owner = owner;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_pointerInside = true;
			_owner?.OnItemHover();
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (!_pointerInside)
			{
				_owner?.OnItemHover();
			}
		}

		private void OnDisable()
		{
			_pointerInside = false;
		}
	}
}

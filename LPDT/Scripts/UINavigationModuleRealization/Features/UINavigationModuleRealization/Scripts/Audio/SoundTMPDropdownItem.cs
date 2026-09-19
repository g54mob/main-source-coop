using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UINavigationModuleRealization.Scripts.Audio
{
	public class SoundTMPDropdownItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler
	{
		private SoundTMPDropdown _owner;

		private bool _pointerInside;

		public void Initialize(SoundTMPDropdown owner)
		{
			_owner = owner;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_pointerInside = true;
			_owner?.PlayItemHover();
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (!_pointerInside)
			{
				_owner?.PlayItemHover();
			}
		}

		private void OnDisable()
		{
			_pointerInside = false;
		}
	}
}

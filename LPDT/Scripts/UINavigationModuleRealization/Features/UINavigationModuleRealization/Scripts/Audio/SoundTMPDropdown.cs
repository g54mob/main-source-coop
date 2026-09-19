using FMODUnity;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UINavigationModuleRealization.Scripts.Audio
{
	public class SoundTMPDropdown : DropdownContentWithoutSelectingOnPointerEnter
	{
		[Header("FMOD")]
		[SerializeField]
		private EventReference _selectEvent;

		[SerializeField]
		private EventReference _openEvent;

		[SerializeField]
		private EventReference _changeEvent;

		[SerializeField]
		private EventReference _itemHoverEvent;

		private bool _isPointerInside;

		protected override void Start()
		{
			base.Start();
			if (Application.isPlaying)
			{
				base.onValueChanged.AddListener(OnValueChangedSound);
			}
		}

		protected override void OnDestroy()
		{
			if (Application.isPlaying)
			{
				base.onValueChanged.RemoveListener(OnValueChangedSound);
			}
			base.OnDestroy();
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			_isPointerInside = true;
			Play(_selectEvent);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			_isPointerInside = false;
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (!_isPointerInside)
			{
				Play(_selectEvent);
			}
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			Play(_openEvent);
		}

		public override void OnSubmit(BaseEventData eventData)
		{
			base.OnSubmit(eventData);
			Play(_openEvent);
		}

		protected override DropdownItem CreateItem(DropdownItem itemTemplate)
		{
			DropdownItem dropdownItem = base.CreateItem(itemTemplate);
			if (dropdownItem != null)
			{
				SoundTMPDropdownItem soundTMPDropdownItem = dropdownItem.gameObject.GetComponent<SoundTMPDropdownItem>();
				if (soundTMPDropdownItem == null)
				{
					soundTMPDropdownItem = dropdownItem.gameObject.AddComponent<SoundTMPDropdownItem>();
				}
				soundTMPDropdownItem.Initialize(this);
			}
			return dropdownItem;
		}

		private void OnValueChangedSound(int value)
		{
			Play(_changeEvent);
		}

		internal void PlayItemHover()
		{
			Play(_itemHoverEvent);
		}

		private void Play(EventReference eventReference)
		{
			if (Application.isPlaying && IsActive() && IsInteractable() && !eventReference.IsNull)
			{
				RuntimeManager.PlayOneShot(eventReference);
			}
		}
	}
}

using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CustomUiModule.Scripts
{
	[Serializable]
	public class SoundButton : Button
	{
		[Header("FMOD")]
		[SerializeField]
		private EventReference _onPointerClickEvent;

		[SerializeField]
		private EventReference _onPointerEnterEvent;

		[SerializeField]
		private EventReference _onSelectEvent;

		[SerializeField]
		private EventReference _onSubmitEvent;

		public override void OnPointerClick(PointerEventData eventData)
		{
			UiSound(_onPointerClickEvent);
			base.OnPointerClick(eventData);
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			UiSound(_onPointerEnterEvent);
			base.OnPointerEnter(eventData);
		}

		public override void OnSelect(BaseEventData eventData)
		{
			UiSound(_onSelectEvent);
			base.OnSelect(eventData);
		}

		public override void OnSubmit(BaseEventData eventData)
		{
			UiSound(_onSubmitEvent);
			base.OnSubmit(eventData);
		}

		private void UiSound(EventReference uiEvent)
		{
			if (Application.isPlaying && IsActive() && IsInteractable() && !uiEvent.IsNull)
			{
				RuntimeManager.PlayOneShot(uiEvent);
			}
		}
	}
}

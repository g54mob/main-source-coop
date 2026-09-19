using FMODUnity;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CustomUiModule.Scripts
{
	public class SoundToggle : ToggleWithNavigationCallbacks
	{
		[Header("FMOD")]
		[SerializeField]
		private EventReference _selectEvent;

		[SerializeField]
		private EventReference _changeEvent;

		private bool _isPointerInside;

		protected override void Start()
		{
			base.Start();
			if (Application.isPlaying)
			{
				onValueChanged.AddListener(OnValueChangedSound);
			}
		}

		protected override void OnDestroy()
		{
			if (Application.isPlaying)
			{
				onValueChanged.RemoveListener(OnValueChangedSound);
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

		private void OnValueChangedSound(bool value)
		{
			Play(_changeEvent);
		}

		private void Play(EventReference eventReference)
		{
			if (Application.isPlaying && IsActive() && IsInteractable() && !eventReference.IsNull)
			{
				RuntimeManager.PlayOneShot(eventReference);
			}
		}

		public void SetIsOnWithoutSound(bool value)
		{
			if (!Application.isPlaying)
			{
				SetIsOnWithoutNotify(value);
				return;
			}
			onValueChanged.RemoveListener(OnValueChangedSound);
			SetIsOnWithoutNotify(value);
			onValueChanged.AddListener(OnValueChangedSound);
		}
	}
}

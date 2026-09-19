using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CustomUiModule.Scripts
{
	public class SoundSlider : Slider
	{
		[Header("FMOD")]
		[SerializeField]
		private EventReference _changeEvent;

		[SerializeField]
		private EventReference _selectEvent;

		private bool _isPointerInside;

		protected override void Start()
		{
			base.Start();
			base.onValueChanged.AddListener(OnValueChangedSound);
		}

		protected override void OnDestroy()
		{
			base.onValueChanged.RemoveListener(OnValueChangedSound);
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

		private void OnValueChangedSound(float value)
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

		public void SetValueWithoutSound(float value)
		{
			base.onValueChanged.RemoveListener(OnValueChangedSound);
			SetValueWithoutNotify(value);
			base.onValueChanged.AddListener(OnValueChangedSound);
		}
	}
}

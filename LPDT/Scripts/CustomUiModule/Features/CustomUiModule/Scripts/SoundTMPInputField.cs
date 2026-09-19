using FMODUnity;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CustomUiModule.Scripts
{
	public class SoundTMPInputField : InputFieldExpanded
	{
		[Header("FMOD")]
		[SerializeField]
		private EventReference _typingEvent;

		[SerializeField]
		private EventReference _selectEvent;

		private string _lastText;

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

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			Play(_selectEvent);
		}

		private void OnValueChangedSound(string value)
		{
			if (!(value == _lastText))
			{
				_lastText = value;
				Play(_typingEvent);
			}
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

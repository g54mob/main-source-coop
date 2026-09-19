using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class MicrophoneEnabledSettingsView : MicrophoneEnabledSettingsViewBase
	{
		[SerializeField]
		private Toggle _enabledToggle;

		protected override void OnEnable()
		{
			base.OnEnable();
			_enabledToggle.onValueChanged.AddListener(InvokeOnMicrophoneEnabledChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_enabledToggle.onValueChanged.RemoveListener(InvokeOnMicrophoneEnabledChanged);
		}

		private void InvokeOnMicrophoneEnabledChanged(bool microphoneEnabled)
		{
			OnMicrophoneEnabledChanged?.Invoke(microphoneEnabled);
		}

		public override void UpdateToggleVisual(bool microphoneEnabled)
		{
			_enabledToggle.SetIsOnWithoutNotify(microphoneEnabled);
		}
	}
}

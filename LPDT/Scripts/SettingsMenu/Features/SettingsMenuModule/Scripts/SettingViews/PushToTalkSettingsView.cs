using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class PushToTalkSettingsView : PushToTalkSettingsViewBase
	{
		[SerializeField]
		private Toggle _enabledToggle;

		protected override void OnEnable()
		{
			base.OnEnable();
			_enabledToggle.onValueChanged.AddListener(InvokeOnPushToTalkEnabledChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_enabledToggle.onValueChanged.RemoveListener(InvokeOnPushToTalkEnabledChanged);
		}

		private void InvokeOnPushToTalkEnabledChanged(bool pushToTalkEnabled)
		{
			OnPushToTalkEnabledChanged?.Invoke(pushToTalkEnabled);
		}

		public override void UpdateToggleVisual(bool pushToTalkEnabled)
		{
			_enabledToggle.SetIsOnWithoutNotify(pushToTalkEnabled);
		}
	}
}

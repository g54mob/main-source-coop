using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class StreamerModeSettingsViewBase : ViewBehaviour
	{
		[SerializeField]
		private Toggle _enabledToggle;

		public event Action<bool> OnStreamerModeEnabledChanged;

		protected override void OnEnable()
		{
			base.OnEnable();
			_enabledToggle.onValueChanged.AddListener(InvokeOnStreamerModeEnabledChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_enabledToggle.onValueChanged.RemoveListener(InvokeOnStreamerModeEnabledChanged);
		}

		private void InvokeOnStreamerModeEnabledChanged(bool streamerModeEnabled)
		{
			this.OnStreamerModeEnabledChanged?.Invoke(streamerModeEnabled);
		}

		public void UpdateToggleVisual(bool streamerModeEnabled)
		{
			_enabledToggle.SetIsOnWithoutNotify(streamerModeEnabled);
		}
	}
}

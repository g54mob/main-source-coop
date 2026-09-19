using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class VsyncSettingsViewBase : ViewBehaviour
	{
		[SerializeField]
		private Toggle _enabledToggle;

		public event Action<bool> OnVSyncEnabledChanged;

		protected override void OnEnable()
		{
			base.OnEnable();
			_enabledToggle.onValueChanged.AddListener(InvokeOnVSyncEnabledChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_enabledToggle.onValueChanged.RemoveListener(InvokeOnVSyncEnabledChanged);
		}

		private void InvokeOnVSyncEnabledChanged(bool microphoneEnabled)
		{
			this.OnVSyncEnabledChanged?.Invoke(microphoneEnabled);
		}

		public void UpdateToggleVisual(bool vSyncEnabled)
		{
			_enabledToggle.SetIsOnWithoutNotify(vSyncEnabled);
		}
	}
}

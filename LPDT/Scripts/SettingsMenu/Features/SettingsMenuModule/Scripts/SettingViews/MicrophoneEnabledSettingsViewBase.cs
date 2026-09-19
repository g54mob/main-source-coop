using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class MicrophoneEnabledSettingsViewBase : ViewBehaviour
	{
		public Action<bool> OnMicrophoneEnabledChanged;

		public abstract void UpdateToggleVisual(bool microphoneEnabled);
	}
}

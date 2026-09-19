using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class PushToTalkSettingsViewBase : ViewBehaviour
	{
		public Action<bool> OnPushToTalkEnabledChanged;

		public abstract void UpdateToggleVisual(bool pushToTalkEnabled);
	}
}

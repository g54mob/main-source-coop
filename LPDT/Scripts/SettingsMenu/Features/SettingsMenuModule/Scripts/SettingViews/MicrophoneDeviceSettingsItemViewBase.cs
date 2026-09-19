using System;
using System.Collections.Generic;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class MicrophoneDeviceSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<string> OnMicrophoneChanged;

		public abstract void SetStartIndex(int index);

		public abstract void SetMicrophoneSettingsOptions(List<string> values, string currentMicrophone);
	}
}

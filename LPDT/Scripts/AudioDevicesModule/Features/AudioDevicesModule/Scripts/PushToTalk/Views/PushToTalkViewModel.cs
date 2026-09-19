using System;

namespace Features.AudioDevicesModule.Scripts.PushToTalk.Views
{
	public class PushToTalkViewModel
	{
		public event Action<bool> PushToTalkVisibilityChanged;

		public void ChangePushToTalkVisibility(bool visible)
		{
			this.PushToTalkVisibilityChanged?.Invoke(visible);
		}
	}
}

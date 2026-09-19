using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.AudioDevicesModule.Scripts.PushToTalk.Views
{
	public abstract class PushToTalkHintViewBase : ViewBehaviour
	{
		public abstract bool IsVisibleOnDeadState { get; }

		public abstract void SetVisible(bool isVisible);

		public abstract void SetPressedVisual(bool isPressed);

		public abstract void SetHintText(string text);
	}
}

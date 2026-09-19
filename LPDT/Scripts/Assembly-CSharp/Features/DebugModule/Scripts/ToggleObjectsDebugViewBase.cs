using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public abstract class ToggleObjectsDebugViewBase : ViewBehaviour
	{
		public Action<bool> OnToggleAdditionalLights;

		public Action<bool> OnToggleObiRopes;

		public Action<bool> OnToggleGeometry;

		public Action<bool> OnTogglePostProcessing;

		public Action<bool> OnToggleLatchGrab;

		public abstract void RefreshLatchGrabToggle(bool isEnabled);
	}
}

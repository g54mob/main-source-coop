using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public abstract class ToggleDebugWindowViewBase : ViewBehaviour
	{
		public Action OnEnableDebugButtonClick;
	}
}

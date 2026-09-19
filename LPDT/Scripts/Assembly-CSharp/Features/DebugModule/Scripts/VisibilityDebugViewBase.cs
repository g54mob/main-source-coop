using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public abstract class VisibilityDebugViewBase : ViewBehaviour
	{
		public abstract event Action<bool> OnArmsActiveSwitched;

		public abstract event Action<bool> OnUIActiveSwitched;

		public abstract void RefreshUiToggle(bool isActive);

		public abstract void RefreshArmsToggle(bool isActive);
	}
}

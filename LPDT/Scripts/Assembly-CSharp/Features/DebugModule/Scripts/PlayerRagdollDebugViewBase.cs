using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public abstract class PlayerRagdollDebugViewBase : ViewBehaviour
	{
		public abstract event Action<bool> OnRagdollSimulationChanged;

		public abstract void RefreshToggle(bool isEnabled);
	}
}

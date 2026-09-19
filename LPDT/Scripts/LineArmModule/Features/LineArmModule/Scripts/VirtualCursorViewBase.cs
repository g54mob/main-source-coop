using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.LineArmModule.Scripts
{
	public abstract class VirtualCursorViewBase : ViewBehaviour
	{
		public abstract void SetVirtualCursorVisibility(bool isVisible);

		public abstract void SetVirtualCursorScreenPosition(Vector2 screenPosition);
	}
}

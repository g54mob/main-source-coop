using System;
using System.Collections.Generic;
using Features.LevelLightModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public abstract class FlashlightSwitchDebugViewBase : ViewBehaviour
	{
		public event Action<FlashlightType> OnFlashlightTypeApplyRequested;

		public abstract void SetFlashlightTypes(IReadOnlyList<FlashlightType> flashlightTypes);

		protected void InvokeFlashlightTypeApplyRequested(FlashlightType flashlightType)
		{
			this.OnFlashlightTypeApplyRequested?.Invoke(flashlightType);
		}
	}
}

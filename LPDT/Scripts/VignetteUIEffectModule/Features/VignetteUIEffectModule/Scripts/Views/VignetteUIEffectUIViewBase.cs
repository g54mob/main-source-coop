using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.VignetteUIEffectModule.Scripts.Views
{
	public abstract class VignetteUIEffectUIViewBase : ViewBehaviour
	{
		public abstract void PlayFocusAnimation();

		public abstract void ApplyIntensity(float intensity);
	}
}

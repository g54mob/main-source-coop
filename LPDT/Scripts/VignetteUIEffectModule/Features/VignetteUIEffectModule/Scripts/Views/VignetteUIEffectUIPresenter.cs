using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.VignetteUIEffectModule.Scripts.Views
{
	public class VignetteUIEffectUIPresenter : PresenterBehaviour<VignetteUIEffectUIViewBase>, IVignetteUIEffectVisual
	{
		protected override void OnViewSet()
		{
			ApplyIntensity(0f);
		}

		public void ApplyIntensity(float intensity)
		{
			base.View.ApplyIntensity(intensity);
		}

		public void PlayFocusAnimation()
		{
			base.View.PlayFocusAnimation();
		}
	}
}

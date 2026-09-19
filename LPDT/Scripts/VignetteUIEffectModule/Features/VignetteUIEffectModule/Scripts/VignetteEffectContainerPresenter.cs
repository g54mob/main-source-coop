using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.VignetteUIEffectModule.Scripts
{
	public class VignetteEffectContainerPresenter : PresenterBehaviour<VignetteEffectContainerViewBase>
	{
		private readonly IVignetteUILayersBootstrapService _layersBootstrapService;

		public VignetteEffectContainerPresenter(IVignetteUILayersBootstrapService layersBootstrapService)
		{
			_layersBootstrapService = layersBootstrapService;
		}

		protected override void OnViewSet()
		{
			_layersBootstrapService.BootstrapLayers(base.View);
		}
	}
}

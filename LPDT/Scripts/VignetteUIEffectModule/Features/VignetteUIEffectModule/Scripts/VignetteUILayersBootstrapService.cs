using UnityEngine;

namespace Features.VignetteUIEffectModule.Scripts
{
	public class VignetteUILayersBootstrapService : IVignetteUILayersBootstrapService
	{
		private readonly VignetteUIEffectModel _vignetteUIEffectModel;

		private readonly IVignetteUIEffectViewFactory _vignetteUIEffectViewFactory;

		private readonly VignetteUIEffectViewsConfiguration _viewsConfiguration;

		public VignetteUILayersBootstrapService(VignetteUIEffectModel vignetteUIEffectModel, IVignetteUIEffectViewFactory vignetteUIEffectViewFactory, VignetteUIEffectViewsConfiguration viewsConfiguration)
		{
			_vignetteUIEffectModel = vignetteUIEffectModel;
			_vignetteUIEffectViewFactory = vignetteUIEffectViewFactory;
			_viewsConfiguration = viewsConfiguration;
		}

		public void BootstrapLayers(VignetteEffectContainerViewBase containerView)
		{
			Transform effectsRoot = containerView.GetEffectsRoot();
			foreach (VignetteUIEffectType item in _viewsConfiguration.EnumerateConfiguredEffectTypes())
			{
				IVignetteUIEffectVisual vignetteUIEffectVisual = _vignetteUIEffectViewFactory.CreateLayer(item, effectsRoot);
				if (vignetteUIEffectVisual != null)
				{
					_vignetteUIEffectModel.RegisterLayer(item, vignetteUIEffectVisual);
				}
			}
		}
	}
}

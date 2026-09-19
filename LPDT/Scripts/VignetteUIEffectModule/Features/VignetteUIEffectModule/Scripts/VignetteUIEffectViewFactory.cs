using Features.VignetteUIEffectModule.Scripts.Views;
using UnityEngine;
using Zenject;

namespace Features.VignetteUIEffectModule.Scripts
{
	public class VignetteUIEffectViewFactory : IVignetteUIEffectViewFactory
	{
		private readonly VignetteUIEffectViewsConfiguration _viewsConfiguration;

		private readonly DiContainer _diContainer;

		private readonly IVignetteUIService _vignetteUIService;

		public VignetteUIEffectViewFactory(VignetteUIEffectViewsConfiguration viewsConfiguration, DiContainer diContainer, IVignetteUIService vignetteUIService)
		{
			_viewsConfiguration = viewsConfiguration;
			_diContainer = diContainer;
			_vignetteUIService = vignetteUIService;
		}

		public IVignetteUIEffectVisual CreateLayer(VignetteUIEffectType effectType, Transform root)
		{
			if (!_viewsConfiguration.TryGetViewPrefab(effectType, out var viewPrefab))
			{
				return null;
			}
			VignetteUIEffectUIViewBase view = _diContainer.InstantiatePrefabForComponent<VignetteUIEffectUIViewBase>(viewPrefab, root);
			return _vignetteUIService.AddViewToVignetteWindow<VignetteUIEffectUIPresenter>(view, root);
		}
	}
}

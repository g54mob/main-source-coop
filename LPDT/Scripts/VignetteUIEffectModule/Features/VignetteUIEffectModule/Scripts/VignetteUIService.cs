using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.VignetteUIEffectModule.Scripts
{
	public class VignetteUIService : IVignetteUIService
	{
		private readonly VignetteEffectWindow _vignetteEffectWindow;

		public VignetteUIService(VignetteEffectWindow vignetteEffectWindow)
		{
			_vignetteEffectWindow = vignetteEffectWindow;
		}

		public TPresenter AddViewToVignetteWindow<TPresenter>(ViewBehaviour view, Transform root) where TPresenter : PresenterBehaviour
		{
			_vignetteEffectWindow.AddView(view.transform, worldPositionStays: false);
			view.transform.SetParent(root, worldPositionStays: false);
			view.transform.localPosition = Vector3.zero;
			return _vignetteEffectWindow.GetPresenterForView<TPresenter>(view);
		}
	}
}

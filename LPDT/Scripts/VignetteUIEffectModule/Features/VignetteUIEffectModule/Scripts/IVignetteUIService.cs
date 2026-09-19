using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.VignetteUIEffectModule.Scripts
{
	public interface IVignetteUIService
	{
		TPresenter AddViewToVignetteWindow<TPresenter>(ViewBehaviour view, Transform root) where TPresenter : PresenterBehaviour;
	}
}

using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public interface ILoadingScreenUIService
	{
		TPresenter BindPresenter<TPresenter>(ViewBehaviour view) where TPresenter : PresenterBehaviour;
	}
}

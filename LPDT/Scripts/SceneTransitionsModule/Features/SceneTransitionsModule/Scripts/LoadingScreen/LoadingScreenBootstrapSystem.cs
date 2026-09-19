using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenBootstrapSystem : IInitializable
	{
		private readonly LoadingScreenWindow _loadingScreenWindow;

		public LoadingScreenBootstrapSystem(LoadingScreenWindow loadingScreenWindow)
		{
			_loadingScreenWindow = loadingScreenWindow;
		}

		public void Initialize()
		{
			if (_loadingScreenWindow.WindowStatus == WindowStatus.Closed)
			{
				_loadingScreenWindow.Open();
			}
			_loadingScreenWindow.GetPresenter<LoadingScreenPresenter>();
		}
	}
}

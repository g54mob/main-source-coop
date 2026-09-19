using System;
using Global.Modules.Localization_Module.Scripts;
using Zenject;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class FontsLoadingScreenSystem : IInitializable, IDisposable
	{
		private readonly ILanguageService _languageService;

		private readonly ILoadingScreenService _loadingScreenService;

		private bool _isFontsLoadingOverlayActive;

		public FontsLoadingScreenSystem(ILanguageService languageService, ILoadingScreenService loadingScreenService)
		{
			_languageService = languageService;
			_loadingScreenService = loadingScreenService;
		}

		public void Initialize()
		{
			ILanguageService languageService = _languageService;
			languageService.OnPreLanguageChanged = (Action)Delegate.Combine(languageService.OnPreLanguageChanged, new Action(ShowFontsLoadingScreen));
			ILanguageService languageService2 = _languageService;
			languageService2.OnLanguageChanged = (Action)Delegate.Combine(languageService2.OnLanguageChanged, new Action(HideFontsLoadingScreen));
		}

		public void Dispose()
		{
			ILanguageService languageService = _languageService;
			languageService.OnPreLanguageChanged = (Action)Delegate.Remove(languageService.OnPreLanguageChanged, new Action(ShowFontsLoadingScreen));
			ILanguageService languageService2 = _languageService;
			languageService2.OnLanguageChanged = (Action)Delegate.Remove(languageService2.OnLanguageChanged, new Action(HideFontsLoadingScreen));
		}

		private void ShowFontsLoadingScreen()
		{
			if (!_isFontsLoadingOverlayActive && _loadingScreenService.CanShow(LoadingScreenShowType.FontsLoading))
			{
				_isFontsLoadingOverlayActive = true;
				_loadingScreenService.Show(LoadingScreenShowType.FontsLoading);
			}
		}

		private void HideFontsLoadingScreen()
		{
			if (_isFontsLoadingOverlayActive)
			{
				_isFontsLoadingOverlayActive = false;
				if (_loadingScreenService.ActiveShowType == LoadingScreenShowType.FontsLoading)
				{
					_loadingScreenService.Hide(LoadingScreenShowType.FontsLoading);
				}
			}
		}
	}
}

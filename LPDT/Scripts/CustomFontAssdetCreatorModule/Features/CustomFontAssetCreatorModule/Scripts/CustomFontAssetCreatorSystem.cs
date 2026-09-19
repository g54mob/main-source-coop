using System;
using Global.Modules.Localization_Module.Scripts;
using Zenject;

namespace Features.CustomFontAssetCreatorModule.Scripts
{
	public class CustomFontAssetCreatorSystem : IInitializable, IDisposable
	{
		private readonly ILanguageService _languageService;

		private readonly ICustomFontAssetCreatorService _customFontAssetCreatorService;

		public CustomFontAssetCreatorSystem(ILanguageService languageService, ICustomFontAssetCreatorService customFontAssetCreatorService)
		{
			_languageService = languageService;
			_customFontAssetCreatorService = customFontAssetCreatorService;
		}

		public void Initialize()
		{
			ILanguageService languageService = _languageService;
			languageService.OnPreLanguageChanged = (Action)Delegate.Combine(languageService.OnPreLanguageChanged, new Action(GenerateFontsWithWindow));
		}

		public void Dispose()
		{
			ILanguageService languageService = _languageService;
			languageService.OnPreLanguageChanged = (Action)Delegate.Remove(languageService.OnPreLanguageChanged, new Action(GenerateFontsWithWindow));
		}

		private void GenerateFontsWithWindow()
		{
			_customFontAssetCreatorService.GenerateFontsWithWindow();
		}
	}
}

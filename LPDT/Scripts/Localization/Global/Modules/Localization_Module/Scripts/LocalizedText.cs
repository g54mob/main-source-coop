using System;
using Global.Modules.LocalizationModule.Scripts.Generated;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Global.Modules.Localization_Module.Scripts
{
	public class LocalizedText : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _text;

		[SerializeField]
		private LocalizationKey _localizationKey;

		[SerializeField]
		private RectTransform _layoutRoot;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		[Inject]
		private void InjectDependencies(ILocalizationService localizationService, ILanguageService languageService)
		{
			_localizationService = localizationService;
			_languageService = languageService;
		}

		private void OnEnable()
		{
			UpdateText();
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(UpdateText));
		}

		private void OnDisable()
		{
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(UpdateText));
		}

		private void UpdateText()
		{
			_text.SetText(_localizationService.GetLocalizedString(_localizationKey));
			if (!(_layoutRoot == null))
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutRoot);
			}
		}
	}
}

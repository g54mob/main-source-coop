using System;
using Global.Modules.LocalizationModule.Scripts.Generated;
using TMPro;
using UnityEngine;
using Zenject;

namespace Global.Modules.Localization_Module.Scripts.MonoBehaviours
{
	public class LocalizedTMP : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _tmpText;

		[SerializeField]
		private bool _isWithAutomaticUpdate = true;

		[SerializeField]
		private bool _isWithUpdateOnLanguageChange = true;

		[SerializeField]
		private LocalizationKey _localizationKey;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		public LocalizationKey LocalizationKey
		{
			get
			{
				return _localizationKey;
			}
			set
			{
				_localizationKey = value;
				SetLocalizedText();
			}
		}

		[Inject]
		private void InjectDependencies(ILocalizationService localizationService, ILanguageService languageService)
		{
			_languageService = languageService;
			_localizationService = localizationService;
		}

		private void OnEnable()
		{
			if (_isWithAutomaticUpdate)
			{
				SetLocalizedText();
			}
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(SetLocalizedText));
		}

		private void OnDisable()
		{
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(SetLocalizedText));
		}

		private void SetLocalizedText()
		{
			if (_isWithUpdateOnLanguageChange)
			{
				_tmpText.SetText(_localizationService.GetLocalizedString(LocalizationKey));
			}
		}
	}
}

using System;
using DG.Tweening;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public class ChapterTypeOptionView : ChapterTypeOptionViewBase
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private Color _selectedColor = Color.white;

		[SerializeField]
		private Color _unselectedColor = Color.gray;

		[SerializeField]
		private float _selectedAlpha = 1f;

		[SerializeField]
		private float _unselectedAlpha = 0.4f;

		[SerializeField]
		private float _colorTweenDuration = 0.2f;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		private LocalizationKey _nameLocalizationKey;

		[field: SerializeField]
		public TMP_Text NameText { get; private set; }

		[Inject]
		public void InjectDependencies(ILocalizationService localizationService, ILanguageService languageService)
		{
			_localizationService = localizationService;
			_languageService = languageService;
		}

		private new void OnEnable()
		{
			UpdateNameText();
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(UpdateNameText));
		}

		private new void OnDisable()
		{
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(UpdateNameText));
			NameText.DOKill();
			if (_canvasGroup != null)
			{
				_canvasGroup.DOKill();
			}
		}

		public override void SetNameLocalizationKey(LocalizationKey localizationKey)
		{
			_nameLocalizationKey = localizationKey;
			UpdateNameText();
		}

		public override void SetSelected(bool isSelected)
		{
			NameText.DOKill();
			NameText.DOColor(isSelected ? _selectedColor : _unselectedColor, _colorTweenDuration).SetEase(Ease.OutSine);
			if (!(_canvasGroup == null))
			{
				_canvasGroup.DOKill();
				_canvasGroup.DOFade(isSelected ? _selectedAlpha : _unselectedAlpha, _colorTweenDuration).SetEase(Ease.OutSine);
			}
		}

		private void UpdateNameText()
		{
			NameText.SetText(_localizationService.GetLocalizedString(_nameLocalizationKey));
		}
	}
}

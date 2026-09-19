using System;
using System.Collections.Generic;
using DG.Tweening;
using Features.MainMenuModule.Scripts.Views.Chapters.SupView;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public class FinalChapterOptionView : ChapterOptionViewBase
	{
		private static readonly int IsLockedHash = Animator.StringToHash("IsLocked");

		[SerializeField]
		private List<Image> _bordersImage;

		[SerializeField]
		private ChapterProgressListSupView _chapterProgressList;

		[SerializeField]
		private CanvasGroup _selectedCanvasGroup;

		[SerializeField]
		private Animator _stateAnimator;

		[SerializeField]
		private Image _frameDefaultImage;

		[SerializeField]
		private Image _frameSelectedImage;

		[SerializeField]
		private Image _frameLowerImage;

		[SerializeField]
		private Image _frameFrontImage;

		[SerializeField]
		private LocalizationKey _chapterNumberLocalizationKey;

		[SerializeField]
		private Color _backgroundSelectedColor;

		[SerializeField]
		private Color _backgroundUnselectedColor;

		[SerializeField]
		private float _borderColorTweenDuration = 0.2f;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		private int _chapterNumber;

		[field: SerializeField]
		public TMP_Text ChapterNumberText { get; private set; }

		[Inject]
		public void InjectDependencies(ILocalizationService localizationService, ILanguageService languageService)
		{
			_localizationService = localizationService;
			_languageService = languageService;
		}

		private new void OnEnable()
		{
			UpdateChapterNumberText();
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(UpdateChapterNumberText));
		}

		private new void OnDisable()
		{
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(UpdateChapterNumberText));
			KillBorderColorTweens();
		}

		public override void SetChapterNumber(int chapterNumber)
		{
			_chapterNumber = chapterNumber;
			UpdateChapterNumberText();
		}

		public override void SetLevelsCount(int levelsCount)
		{
			_chapterProgressList.SetTotalCount(levelsCount);
		}

		public override void SetSelected(bool isSelected)
		{
			Color targetColor = (isSelected ? _backgroundSelectedColor : _backgroundUnselectedColor);
			_frameSelectedImage.gameObject.SetActive(isSelected);
			_frameDefaultImage.gameObject.SetActive(!isSelected);
			foreach (Image item in _bordersImage)
			{
				SetColorAnimated(item, targetColor);
			}
		}

		private void SetColorAnimated(Image image, Color targetColor)
		{
			image.DOKill();
			image.DOColor(targetColor, _borderColorTweenDuration).SetEase(Ease.OutSine);
		}

		private void KillBorderColorTweens()
		{
			foreach (Image item in _bordersImage)
			{
				item.DOKill();
			}
		}

		public override void SetLocked(bool isLocked)
		{
			_stateAnimator.SetBool(IsLockedHash, isLocked);
		}

		public override void SetChapterData(ChapterPreviewData chapterPreviewData)
		{
		}

		public override void SetProgress(int levelsCount, int reachedSubLevelCount, bool isChapterPassed)
		{
			_chapterProgressList.SetProgress(levelsCount, reachedSubLevelCount, isChapterPassed);
		}

		private void UpdateChapterNumberText()
		{
			ChapterNumberText.SetText($"{_localizationService.GetLocalizedString(_chapterNumberLocalizationKey)} {_chapterNumber}+");
		}
	}
}

using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts.View
{
	public class DayBannerView : DayBannerViewBase
	{
		private static readonly int ShowHash = Animator.StringToHash("Show");

		[SerializeField]
		private TMP_Text _dayDetailText;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private LocalizationKey _dayLocalizationKey;

		[SerializeField]
		private LocalizationKey _chapterLocalizationKey;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		[Inject]
		public void InjectDependencies(ILocalizationService localizationService, ILanguageService languageService)
		{
			_localizationService = localizationService;
			_languageService = languageService;
		}

		public override void SetDay(int chapterNumber, int dayInChapter)
		{
			if ((Object)(object)_dayDetailText != null)
			{
				_dayDetailText.SetText($"{_localizationService.GetLocalizedString(_chapterLocalizationKey)} {chapterNumber}  {_localizationService.GetLocalizedString(_dayLocalizationKey)} {dayInChapter}");
			}
		}

		public override void Show()
		{
			if (_animator != null)
			{
				_animator.SetTrigger(ShowHash);
			}
		}
	}
}

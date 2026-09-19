using Features.GameUpdaterModule;
using Features.LevelModule.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.PlayersStatisticsModule.Scripts.Views.StatisticsDay
{
	public class StatisticsDayPresenter : PresenterBehaviour<StatisticsDayViewBase>
	{
		private readonly IGameUpdater _gameUpdater;

		private readonly LevelModel _levelModel;

		private readonly ILocalizationService _localizationService;

		public StatisticsDayPresenter(LevelModel levelModel, ILocalizationService localizationService, IGameUpdater gameUpdater)
		{
			_levelModel = levelModel;
			_localizationService = localizationService;
			_gameUpdater = gameUpdater;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			UpdateView();
			_levelModel.OnCurrentChapterIndexChanged += ProcessLevelDataChanced;
			_levelModel.OnCurrentLevelNumberChanged += ProcessLevelDataChanced;
			_gameUpdater.OnUpdate += OnUpdateView;
		}

		protected override void OnViewDisabled()
		{
			_levelModel.OnCurrentChapterIndexChanged -= ProcessLevelDataChanced;
			_levelModel.OnCurrentLevelNumberChanged -= ProcessLevelDataChanced;
			_gameUpdater.OnUpdate -= OnUpdateView;
			base.OnViewDisabled();
		}

		private void ProcessLevelDataChanced(int obj)
		{
			UpdateView();
		}

		private void OnUpdateView()
		{
			UpdateView();
		}

		private void UpdateView()
		{
			base.View.SetDayText($"{_localizationService.GetLocalizedString(LocalizationKey.UI_Session_Day)} {_levelModel.CurrentLevelNumber}");
			if (!TryGetCompletedChapterIndex(out var completedChapterIndex))
			{
				base.View.SetChapterCompletedVisible(visible: false);
				return;
			}
			int num = completedChapterIndex + 1;
			string chapterCompletedText = _localizationService.GetLocalizedString(LocalizationKey.UI_Statistics_ChapterCompleted).Replace("[0]", num.ToString());
			base.View.SetChapterCompletedText(chapterCompletedText);
			base.View.SetChapterCompletedVisible(visible: true);
		}

		private bool TryGetCompletedChapterIndex(out int completedChapterIndex)
		{
			completedChapterIndex = -1;
			if (_levelModel.CurrentLevelInChapterIndex != 1)
			{
				return false;
			}
			if (_levelModel.CurrentChapterIndex <= 0)
			{
				return false;
			}
			completedChapterIndex = _levelModel.CurrentChapterIndex - 1;
			return true;
		}
	}
}

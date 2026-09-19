using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.StoreModule.Scripts.MovedToBeachLocal;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using JetBrains.Annotations;
using NetworkServices.NetworkEvents;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.LevelModule.Scripts.View
{
	[PublicAPI]
	public class DayBannerPresenter : PresenterBehaviour<DayBannerViewBase>
	{
		private readonly LevelModel _levelModel;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly MovedToBeachEventClass _movedToBeachEvent;

		private readonly LoadingScreenModel _loadingScreenModel;

		private readonly TutorialModel _tutorialModel;

		private bool _pendingShow;

		public DayBannerPresenter(LevelModel levelModel, NetworkRunnerEventBus networkRunnerEventBus, MovedToBeachEventClass movedToBeachEvent, LoadingScreenModel loadingScreenModel, TutorialModel tutorialModel)
		{
			_levelModel = levelModel;
			_networkRunnerEventBus = networkRunnerEventBus;
			_movedToBeachEvent = movedToBeachEvent;
			_loadingScreenModel = loadingScreenModel;
			_tutorialModel = tutorialModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_movedToBeachEvent.OnLocalPlayerMovedToBeach += OnLocalPlayerMovedToBeach;
			_loadingScreenModel.OnEndedFadeOut += OnFadeOutEnded;
			_networkRunnerEventBus.Subscribe<OnStartGamePhaseEvent>(OnStartGamePhase);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_movedToBeachEvent.OnLocalPlayerMovedToBeach -= OnLocalPlayerMovedToBeach;
			_loadingScreenModel.OnEndedFadeOut -= OnFadeOutEnded;
			_networkRunnerEventBus.Unsubscribe<OnStartGamePhaseEvent>(OnStartGamePhase);
			_pendingShow = false;
		}

		private void OnLocalPlayerMovedToBeach()
		{
			_pendingShow = true;
		}

		private void OnStartGamePhase(OnStartGamePhaseEvent _)
		{
			_pendingShow = true;
		}

		private void OnFadeOutEnded()
		{
			if (_pendingShow && !_tutorialModel.IsTutorialInProgress)
			{
				_pendingShow = false;
				base.View.SetDay(_levelModel.CurrentChapterIndex + 1, _levelModel.CurrentLevelInChapterIndex);
				base.View.Show();
			}
		}
	}
}

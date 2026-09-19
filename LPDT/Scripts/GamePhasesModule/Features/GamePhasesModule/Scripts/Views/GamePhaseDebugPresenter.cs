using System.Collections.Generic;
using Features.EnemyFearingModule.Scripts;
using Features.GamePhasesModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using JetBrains.Annotations;
using NetworkServices.ObjectsProvider;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.GamePhasesModule.Scripts.Views
{
	[PublicAPI]
	public class GamePhaseDebugPresenter : PresenterBehaviour<GamePhaseDebugViewBase>
	{
		private readonly GamePhasesModel _gamePhasesModel;

		private readonly IGamePhaseService _gamePhaseService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly EnemyFearListenerModel _enemiesFearListenerModel;

		private bool _destroyImmediately;

		public GamePhaseDebugPresenter(GamePhasesModel gamePhasesModel, IGamePhaseService gamePhaseService, MultiplayerModel multiplayerModel, EnemyFearListenerModel enemiesFearListenerModel)
		{
			_gamePhasesModel = gamePhasesModel;
			_gamePhaseService = gamePhaseService;
			_multiplayerModel = multiplayerModel;
			_enemiesFearListenerModel = enemiesFearListenerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_gamePhasesModel.OnCurrentPhaseTimeChanged += ChangeTimeText;
			base.View.SkipGamePhaseButton.onClick.AddListener(SkipGamePhase);
			ChangeTimeText(_gamePhasesModel.CurrentPhaseTime);
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				HideView();
			}
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_gamePhasesModel.OnCurrentPhaseTimeChanged -= ChangeTimeText;
			base.View.SkipGamePhaseButton.onClick.RemoveListener(SkipGamePhase);
		}

		private void OnDestroyImmediatelyValueChanged(bool destroyImmediately)
		{
			_destroyImmediately = destroyImmediately;
		}

		private void ChangeTimeText(float time)
		{
			base.View.CurrentMultiplierText.SetText($"(*{_gamePhasesModel.TimeMultiplier})");
			base.View.CurrentPhaseCountText.SetText("Current phase: " + _gamePhasesModel.CurrentPhaseCount);
			base.View.CurrentPhaseTimeText.SetText(time.ToString("F1"));
		}

		private void SkipGamePhase()
		{
			if (_destroyImmediately)
			{
				foreach (IEnemyFearCallbackListener item in new List<IEnemyFearCallbackListener>(_enemiesFearListenerModel.GetAllEnemyFearCallbackListeners()))
				{
					(item as NetworkBehaviour).Object.DespawnHierarchy();
				}
			}
			_gamePhaseService.ActivateNextGamePhaseInSequence();
		}
	}
}

using Features.MultiplayerSessionServices.Scripts;
using Features.QuotaModule.Scripts;
using Features.SessionManagementModule.Models;
using Fusion;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	[PublicAPI]
	public class LevelTransitionDebugPresenter : PresenterBehaviour<LevelTransitionDebugViewBase>
	{
		private const int CountdownAlreadyElapsedTicks = 100000;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly LevelModel _sessionLevelModel;

		private readonly SessionStateMachine _sessionStateMachine;

		public LevelTransitionDebugPresenter(MultiplayerModel multiplayerModel, QuotaCompletionModel quotaCompletionModel, LevelModel sessionLevelModel, SessionStateMachine sessionStateMachine)
		{
			_multiplayerModel = multiplayerModel;
			_quotaCompletionModel = quotaCompletionModel;
			_sessionLevelModel = sessionLevelModel;
			_sessionStateMachine = sessionStateMachine;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.GoToTheNextLevelButton.onClick.AddListener(GoToTheNextLevel);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.GoToTheNextLevelButton.onClick.RemoveListener(GoToTheNextLevel);
		}

		private void GoToTheNextLevel()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				Debug.LogWarning("GoToTheNextLevel debug action requires an active network session.");
				return;
			}
			if (!networkRunner.IsSharedModeMasterClient)
			{
				Debug.LogWarning("GoToTheNextLevel debug action is host-only because level completion is state-authoritative.");
				return;
			}
			if (_sessionStateMachine.Current != SessionState.Level || !_sessionStateMachine.IsActive)
			{
				Debug.LogWarning("GoToTheNextLevel debug action requires an active Level state.");
				return;
			}
			_quotaCompletionModel.IsQuotaCompleted.Value = true;
			_sessionLevelModel.CountdownStartTick.Value = (int)networkRunner.Tick - 100000;
			_quotaCompletionModel.IsBellActivated.Value = true;
		}
	}
}

using System;
using Global.StateMachinesModule.Scripts;
using Zenject;

namespace Features.PlatformStatusRealizationModule.Scripts.Systems
{
	public class MainMenuStatusSyncSystem : IInitializable, IDisposable
	{
		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly IGameStatusService _gameStatusService;

		public MainMenuStatusSyncSystem(GameFlowStateMachine gameFlowStateMachine, IGameStatusService gameStatusService)
		{
			_gameFlowStateMachine = gameFlowStateMachine;
			_gameStatusService = gameStatusService;
		}

		public void Initialize()
		{
			_gameFlowStateMachine.OnStateEnter += OnGameFlowStateEntered;
			RefreshStatus();
		}

		public void Dispose()
		{
			_gameFlowStateMachine.OnStateEnter -= OnGameFlowStateEntered;
		}

		private void OnGameFlowStateEntered(GameFlowState state)
		{
			RefreshStatus();
		}

		private void RefreshStatus()
		{
			if (_gameFlowStateMachine.CurrentState != GameFlowState.SessionGameState)
			{
				_gameStatusService.SetGameStatus(GameStatusId.MainMenu);
			}
		}
	}
}

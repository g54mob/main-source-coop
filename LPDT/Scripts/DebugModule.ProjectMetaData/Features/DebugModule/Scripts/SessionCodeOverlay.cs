using Features.MultiplayerSessionServices.Scripts;
using Features.RunningSessionModule.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion;
using Global.StateMachinesModule.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.DebugModule.Scripts
{
	public class SessionCodeOverlay : MonoBehaviour
	{
		private const string SessionCodePrefix = "Code: ";

		[SerializeField]
		private TMP_Text _sessionCodeText;

		private GameFlowStateMachine _gameFlowStateMachine;

		private RunningSessionModel _runningSessionModel;

		private MultiplayerModel _multiplayerModel;

		private CurrentSettingsModel _currentSettingsModel;

		[Inject]
		public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine, RunningSessionModel runningSessionModel, MultiplayerModel multiplayerModel, CurrentSettingsModel currentSettingsModel)
		{
			_gameFlowStateMachine = gameFlowStateMachine;
			_runningSessionModel = runningSessionModel;
			_multiplayerModel = multiplayerModel;
			_currentSettingsModel = currentSettingsModel;
		}

		private void Start()
		{
			_gameFlowStateMachine.OnStateEnter += OnGameFlowStateChanged;
			_runningSessionModel.OnRunningSessionCodeChanged += OnSessionCodeChanged;
			Refresh();
		}

		private void OnDestroy()
		{
			if (_gameFlowStateMachine != null)
			{
				_gameFlowStateMachine.OnStateEnter -= OnGameFlowStateChanged;
			}
			if (_runningSessionModel != null)
			{
				_runningSessionModel.OnRunningSessionCodeChanged -= OnSessionCodeChanged;
			}
		}

		private void Update()
		{
			if (_gameFlowStateMachine != null && IsLobbyOrSessionState(_gameFlowStateMachine.CurrentState))
			{
				Refresh();
			}
		}

		private void OnGameFlowStateChanged(GameFlowState _)
		{
			Refresh();
		}

		private void OnSessionCodeChanged(string _)
		{
			Refresh();
		}

		private void Refresh()
		{
			if (_gameFlowStateMachine != null && _multiplayerModel != null && _runningSessionModel != null && _currentSettingsModel != null && !((Object)(object)_sessionCodeText == null))
			{
				if (!IsLobbyOrSessionState(_gameFlowStateMachine.CurrentState))
				{
					Apply(null);
				}
				else
				{
					Apply(ResolveSessionCode());
				}
			}
		}

		private void Apply(string sessionCode)
		{
			bool flag = !string.IsNullOrEmpty(sessionCode);
			((Component)(object)_sessionCodeText).gameObject.SetActive(flag);
			if (flag)
			{
				string text = (_currentSettingsModel.StreamerModeEnabled ? new string('?', sessionCode.Length) : sessionCode);
				_sessionCodeText.text = "Code: " + text;
			}
		}

		private static bool IsLobbyOrSessionState(GameFlowState state)
		{
			if (state != GameFlowState.LobbyGameState)
			{
				return state == GameFlowState.SessionGameState;
			}
			return true;
		}

		private string ResolveSessionCode()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner != null && networkRunner.IsRunning)
			{
				return networkRunner.SessionInfo.Name;
			}
			return _runningSessionModel.SessionCode;
		}
	}
}

using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.ExtendedLogger.Scripts;
using Features.GamePhasesModule.Scripts.Data;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.GamePhasesModule.Scripts
{
	public class GamePhaseStartTrigger : MonoBehaviour
	{
		private GamePhasesModel _gamePhasesModel;

		private SessionAnalyticsModel _sessionAnalyticsModel;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		private void InjectDependencies(GamePhasesModel gamePhasesModel, MultiplayerModel multiplayerModel, SessionAnalyticsModel sessionAnalyticsModel)
		{
			_gamePhasesModel = gamePhasesModel;
			_multiplayerModel = multiplayerModel;
			_sessionAnalyticsModel = sessionAnalyticsModel;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<CharacterMovableBase>(out var component))
			{
				TrySendAnalytics(component);
				if (!_gamePhasesModel.IsSomePlayerMovedFromSpawn)
				{
					ExtendedDebug.LogFiltered(DebugFilterType.EnemiesSpawn, $"\ud83c\udfdd\ufe0f⏲\ufe0f {component.Object.StateAuthority} leaved from spawn, so timer is on.");
					_gamePhasesModel.IsSomePlayerMovedFromSpawn = true;
					_gamePhasesModel.InvokeSomePlayerMovedFromSpawn();
				}
			}
		}

		private void TrySendAnalytics(CharacterMovableBase characterMovableBase)
		{
			if (characterMovableBase.Object.InputAuthority == _multiplayerModel.NetworkRunner.LocalPlayer)
			{
				_sessionAnalyticsModel.SetLocationEnteredStatus(isEntered: true);
			}
		}
	}
}

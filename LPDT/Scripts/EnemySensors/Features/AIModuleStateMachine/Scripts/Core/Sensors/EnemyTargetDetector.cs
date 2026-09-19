using System;
using System.Collections.Generic;
using System.Linq;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.Extensions;
using Features.LevelGatesModule.Data;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Sensors
{
	public class EnemyTargetDetector : EnemyTargetDetectorBase
	{
		private SessionAnalyticsModel _sessionAnalyticsModel;

		private PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private bool _targetWasDetected;

		public override event Action<PlayerRef> OnTargetDetectedInRange;

		public override event Action OnTargetDetecting;

		[Inject]
		private void InjectDependencies(SessionAnalyticsModel sessionAnalyticsModel, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel)
		{
			_sessionAnalyticsModel = sessionAnalyticsModel;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
		}

		protected override void DetectPlayers()
		{
			_targetWasDetected = false;
			base.DetectTargets.Clear();
			if (MultiplayerModel.NetworkRunner == null)
			{
				return;
			}
			List<PlayerRef> list = MultiplayerModel.NetworkRunner.ActivePlayers.ToList();
			list.Shuffle();
			foreach (PlayerRef item in list)
			{
				TryDetectAndNotify(item);
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in SpawnedPlayersModel.Players)
			{
				PlayerRef key = player.Key;
				if (!list.Contains(key))
				{
					NetworkObject networkObject = player.Value?.NetworkObject;
					if (!(networkObject == null) && networkObject.IsValid && !(networkObject.InputAuthority != PlayerRef.None))
					{
						TryDetectAndNotify(key);
					}
				}
			}
			OnTargetDetecting?.Invoke();
		}

		private void TryDetectAndNotify(PlayerRef player)
		{
			if (IsPlayerDetected(player, out var detectionType))
			{
				base.DetectTargets.Add(player);
				_sessionAnalyticsModel.RegisterEnemyTarget(player.PlayerId);
				base.DetectTargetsType[player] = detectionType;
				if (!_targetWasDetected || !_soloDetecting)
				{
					OnTargetDetectedInRange?.Invoke(player);
				}
				_targetWasDetected = true;
			}
		}

		public override bool IsPlayerDetected(PlayerRef player, out DetectionType detectionType)
		{
			detectionType = DetectionType.Default;
			if (!CanSelectPlayerAsTarget(player))
			{
				return false;
			}
			if (!SpawnedPlayersModel.Players.TryGetValue(player, out var value))
			{
				return false;
			}
			if (GetDistanceToPlayer(player) > _detectionRadius)
			{
				return false;
			}
			NetworkObject networkObject = value.NetworkObject;
			if (networkObject != null && networkObject.IsValid && networkObject.InputAuthority == PlayerRef.None)
			{
				return true;
			}
			if (_playersGatesModelSynchronizedModel.TryGetPlayerState(player.PlayerId, out var state))
			{
				return state.PlayerInsideGate;
			}
			return false;
		}
	}
}

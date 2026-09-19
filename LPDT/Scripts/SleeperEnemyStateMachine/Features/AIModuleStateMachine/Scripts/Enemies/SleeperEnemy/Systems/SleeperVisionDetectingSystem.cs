using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.LevelGatesModule.Data;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperVisionDetectingSystem : MonoSystem
	{
		[SerializeField]
		private EnemyVisibilityTargetDetector _targetDetector;

		[SerializeField]
		private float _timeToDetect = 1f;

		[SerializeField]
		private float _timeToLoseTarget = 1f;

		private IDetectionContext _detectionContext;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private PlayersGatesModelSynchronizedModel _playersGatesModel;

		private readonly Dictionary<PlayerRef, float> _visibleTime = new Dictionary<PlayerRef, float>();

		private readonly Dictionary<PlayerRef, float> _lostTime = new Dictionary<PlayerRef, float>();

		private readonly HashSet<PlayerRef> _confirmedTargets = new HashSet<PlayerRef>();

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IDetectionContext detectionContext, SpawnedPlayersModel spawnedPlayersModel, PlayersGatesModelSynchronizedModel playersGatesModel)
		{
			_detectionContext = detectionContext;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playersGatesModel = playersGatesModel;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_targetDetector.Init(_detectionContext.TargetSearchRange);
			_targetDetector.EnableDetecting();
		}

		public override void Disable()
		{
			_isEnabled = false;
			_targetDetector.DisableDetecting();
		}

		public override void Clear()
		{
			ResetTracking();
			_detectionContext.ClearDetectedPlayersDistance();
			_detectionContext.SetDetectedPlayers(new List<PlayerDataHolder>());
		}

		private void Update()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			List<(PlayerDataHolder, float)> list = new List<(PlayerDataHolder, float)>();
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player2 in _spawnedPlayersModel.Players)
			{
				PlayerRef key = player2.Key;
				PlayerDataHolder value = player2.Value;
				if (!(value.NetworkObject == null))
				{
					bool isVisibleNow = IsInsideGate(key) && _targetDetector.DetectTargets.Contains(key);
					UpdateVisibilityTimers(key, isVisibleNow, deltaTime);
					if (_confirmedTargets.Contains(key))
					{
						list.Add((value, _targetDetector.GetDistanceToPlayer(key)));
					}
				}
			}
			foreach (PlayerDataHolder key2 in _detectionContext.DetectedPlayersDistance.Keys.ToList())
			{
				if (!list.Exists(((PlayerDataHolder holder, float distance) tuple2) => tuple2.holder == key2))
				{
					_detectionContext.RemoveDetectedPlayerDistance(key2);
				}
			}
			foreach (var (player, distance) in list)
			{
				_detectionContext.SetDetectedPlayerDistance(player, distance);
			}
			_detectionContext.SetDetectedPlayers(list.Select<(PlayerDataHolder, float), PlayerDataHolder>(((PlayerDataHolder holder, float distance) tuple2) => tuple2.holder).ToList());
		}

		private void UpdateVisibilityTimers(PlayerRef player, bool isVisibleNow, float deltaTime)
		{
			if (isVisibleNow)
			{
				_visibleTime[player] = GetTimer(_visibleTime, player) + deltaTime;
				_lostTime[player] = 0f;
				if (GetTimer(_visibleTime, player) >= _timeToDetect)
				{
					_confirmedTargets.Add(player);
				}
			}
			else
			{
				_lostTime[player] = GetTimer(_lostTime, player) + deltaTime;
				_visibleTime[player] = 0f;
				if (GetTimer(_lostTime, player) >= _timeToLoseTarget)
				{
					_confirmedTargets.Remove(player);
				}
			}
		}

		private bool IsInsideGate(PlayerRef player)
		{
			if (_playersGatesModel.TryGetPlayerState(player.PlayerId, out var state))
			{
				return state.PlayerInsideGate;
			}
			return true;
		}

		private static float GetTimer(Dictionary<PlayerRef, float> timers, PlayerRef player)
		{
			if (!timers.TryGetValue(player, out var value))
			{
				return 0f;
			}
			return value;
		}

		private void ResetTracking()
		{
			_visibleTime.Clear();
			_lostTime.Clear();
			_confirmedTargets.Clear();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}

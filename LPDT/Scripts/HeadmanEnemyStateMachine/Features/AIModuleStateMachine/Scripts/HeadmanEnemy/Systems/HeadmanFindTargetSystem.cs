using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.DamageableTrackModule.Scripts;
using Features.LevelGatesModule.Data;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Systems
{
	public class HeadmanFindTargetSystem : MonoBehaviour
	{
		[SerializeField]
		private HeadmanEnemy _headManEnemyBehaviour;

		[SerializeField]
		private SimpleEnemyDamageable _simpleMonoDamageable;

		[SerializeField]
		private EnemyTargetDetectorBase _detector;

		[SerializeField]
		private HeadManTargetsModel _headManTargetsModel;

		[SerializeField]
		private float _baseHeadManDetectionTargetTime = 0.7f;

		[SerializeField]
		private float _headManDetectionAdditionalTargetTime = 0.5f;

		[SerializeField]
		private float _headManLostTargetTime = 5f;

		[SerializeField]
		private bool _hardDetectImmediately = true;

		[SerializeField]
		private bool _respectsCauldronStealth = true;

		private readonly Dictionary<PlayerRef, float> _detectionTimers = new Dictionary<PlayerRef, float>();

		private readonly Dictionary<PlayerRef, float> _lostTargetTimers = new Dictionary<PlayerRef, float>();

		private readonly HashSet<PlayerRef> _detectedTargetsSet = new HashSet<PlayerRef>();

		private readonly List<PlayerRef> _timersToRemove = new List<PlayerRef>();

		private readonly List<PlayerRef> _targetsToRemove = new List<PlayerRef>();

		private PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private IPlayerStateService _playerStateService;

		private IMultiplayerService _multiplayerService;

		private CauldronStealthModel _cauldronStealthModel;

		[Inject]
		private void InjectDependencies(IPlayerStateService playerStateService, IMultiplayerService multiplayerService, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel, CauldronStealthModel cauldronStealthModel)
		{
			_playerStateService = playerStateService;
			_multiplayerService = multiplayerService;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
			_cauldronStealthModel = cauldronStealthModel;
		}

		private void Awake()
		{
			if (_simpleMonoDamageable != null)
			{
				_simpleMonoDamageable.OnDamaged += ProcessEnemyDamage;
			}
		}

		private void OnDestroy()
		{
			if (_simpleMonoDamageable != null)
			{
				_simpleMonoDamageable.OnDamaged -= ProcessEnemyDamage;
			}
		}

		private void ProcessEnemyDamage(DamageData damageData)
		{
			if (_multiplayerService.TryGetPlayerRefById(damageData.DamageDealerPlayerID, out var playerRef))
			{
				_headManTargetsModel.RegisterTarget(new TargetDetectData(playerRef, TargetDetectionReason.Attacked));
			}
		}

		private void Update()
		{
			UpdateDetectionTimers();
			UpdateLostTargetTimers();
		}

		private void UpdateDetectionTimers()
		{
			List<PlayerRef> detectTargets = _detector.DetectTargets;
			_detectedTargetsSet.Clear();
			foreach (PlayerRef item2 in detectTargets)
			{
				_detectedTargetsSet.Add(item2);
			}
			PlayerRef key;
			foreach (PlayerRef item3 in detectTargets)
			{
				_detectionTimers.TryAdd(item3, 0f);
				if (_headManTargetsModel.HeadManTargetsList.Contains(item3))
				{
					continue;
				}
				DetectionType value;
				bool flag = _hardDetectImmediately && _detector.DetectTargetsType.TryGetValue(item3, out value) && value == DetectionType.HardDetection;
				if (_respectsCauldronStealth && _cauldronStealthModel.IsStealthed(item3.PlayerId))
				{
					_detectionTimers[item3] = 0f;
					continue;
				}
				float num = ((_headManTargetsModel.HeadManTargets.Count > 0) ? _headManDetectionAdditionalTargetTime : _baseHeadManDetectionTargetTime);
				Dictionary<PlayerRef, float> detectionTimers = _detectionTimers;
				key = item3;
				detectionTimers[key] += Time.deltaTime;
				if (_detectionTimers[item3] >= num || flag)
				{
					TargetDetectionReason detectionReason = ((!flag) ? TargetDetectionReason.Detected : TargetDetectionReason.HardDetected);
					_headManTargetsModel.RegisterTarget(new TargetDetectData(item3, detectionReason));
					_detectionTimers.Remove(item3);
					_lostTargetTimers[item3] = 0f;
				}
				else if (_headManEnemyBehaviour != null && _headManEnemyBehaviour.HasStateAuthority && _headManEnemyBehaviour.StateMachineForDebug != null && _headManEnemyBehaviour.StateMachineForDebug.ActiveStateName == HeadmanStateId.Rage && _headManEnemyBehaviour.Context != null && _headManEnemyBehaviour.Context.ActiveRageSubstate == HeadmanRageStateId.Interacted && _headManEnemyBehaviour.Context.LastPlayerToChase == item3)
				{
					_headManTargetsModel.RegisterTarget(new TargetDetectData(item3, TargetDetectionReason.Detected));
					_detectionTimers.Remove(item3);
					_lostTargetTimers[item3] = 0f;
				}
			}
			_timersToRemove.Clear();
			foreach (KeyValuePair<PlayerRef, float> detectionTimer in _detectionTimers)
			{
				detectionTimer.Deconstruct(out key, out var _);
				PlayerRef item = key;
				if (!_detectedTargetsSet.Contains(item))
				{
					_timersToRemove.Add(item);
				}
			}
			foreach (PlayerRef item4 in _timersToRemove)
			{
				_detectionTimers.Remove(item4);
			}
		}

		private void UpdateLostTargetTimers()
		{
			_detectedTargetsSet.Clear();
			foreach (PlayerRef detectTarget in _detector.DetectTargets)
			{
				_detectedTargetsSet.Add(detectTarget);
			}
			_targetsToRemove.Clear();
			foreach (PlayerRef item in _headManTargetsModel.HeadManTargetsList.ToList())
			{
				if (_detectedTargetsSet.Contains(item))
				{
					_lostTargetTimers[item] = 0f;
					continue;
				}
				_lostTargetTimers.TryAdd(item, 0f);
				_lostTargetTimers[item] += Time.deltaTime;
				if (_playersGatesModelSynchronizedModel.TryGetPlayerState(item.PlayerId, out var state) && !state.PlayerInsideGate)
				{
					_targetsToRemove.Add(item);
				}
				else if (_lostTargetTimers[item] >= _headManLostTargetTime || !_playerStateService.IsPlayerAlive(item.PlayerId))
				{
					_targetsToRemove.Add(item);
				}
			}
			foreach (PlayerRef item2 in _targetsToRemove)
			{
				_headManTargetsModel.RemoveTarget(item2);
				_lostTargetTimers.Remove(item2);
			}
		}
	}
}

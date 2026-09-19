using System;
using FMOD.Studio;
using FMODUnity;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy
{
	public class SirenEnemyContext : MonoBehaviour
	{
		public EventInstance ScreamSoundInstance;

		public EventInstance SongSoundInstance;

		public EventInstance HitSoundInstance;

		private SpawnedPlayersModel _spawnedPlayers;

		private IPlayerStateService _playerStateService;

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private IAudioService _audioService;

		[field: SerializeField]
		public EnemyStatHealthController StatHealthController { get; private set; }

		[field: SerializeField]
		public SimpleEnemyDeadProcessor SimpleEnemyDeadProcessor { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public EnemyTargetDetector TargetDetector { get; private set; }

		[field: SerializeField]
		public SimpleEnemyDamageable Damageable { get; private set; }

		[field: SerializeField]
		public ParticleSystem ScreamParticle { get; private set; }

		[field: SerializeField]
		public Transform LookAtTarget { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public EventReference SirenScream { get; private set; }

		[field: SerializeField]
		public EventReference SirenSong { get; private set; }

		[field: SerializeField]
		public EventReference SirenHit { get; private set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		public PlayerRef TargetPlayer { get; set; } = PlayerRef.None;

		public float ChaseElapsed { get; set; }

		public float LostTargetElapsed { get; set; }

		public bool IsLosingTarget { get; set; }

		public bool IsDead { get; set; }

		public bool IsFearing { get; set; }

		public bool IsDespawnAfterFear { get; set; } = true;

		public bool HasTarget => TargetPlayer != PlayerRef.None;

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayers, IPlayerStateService playerStateService, PlayersStatesSynchronizer playersStatesSynchronizer, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, IAudioService audioService)
		{
			_spawnedPlayers = spawnedPlayers;
			_playerStateService = playerStateService;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_audioService = audioService;
		}

		public bool CanEnemyInteractWithTargetPlayer()
		{
			if (TargetPlayer != PlayerRef.None)
			{
				return _enemyPlayerAttackabilityService.CanEnemyAttackPlayer(TargetPlayer.PlayerId);
			}
			return false;
		}

		public void ValidateRequiredReferences()
		{
			AssertAssigned(StatHealthController, "StatHealthController");
			AssertAssigned(SimpleEnemyDeadProcessor, "SimpleEnemyDeadProcessor");
			AssertAssigned(StatEntity, "StatEntity");
			AssertAssigned(TargetDetector, "TargetDetector");
			AssertAssigned(Damageable, "Damageable");
			AssertAssigned(ScreamParticle, "ScreamParticle");
			AssertAssigned(LookAtTarget, "LookAtTarget");
			AssertAssigned(Animator, "Animator");
		}

		public void CreateFmodInstances()
		{
			ScreamSoundInstance = _audioService.CreateInstance(SirenScream);
			SongSoundInstance = _audioService.CreateInstance(SirenSong);
			HitSoundInstance = _audioService.CreateInstance(SirenHit);
		}

		public void ReleaseFmodInstances()
		{
			_audioService.StopInstance(ScreamSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(ScreamSoundInstance);
			_audioService.StopInstance(SongSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(SongSoundInstance);
			_audioService.StopInstance(HitSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(HitSoundInstance);
		}

		public void UpdateFmod3DAttributes()
		{
			ScreamSoundInstance.set3DAttributes(SoundSourceBehaviour.SoundSourceTransform.To3DAttributes());
			SongSoundInstance.set3DAttributes(SoundSourceBehaviour.SoundSourceTransform.To3DAttributes());
			HitSoundInstance.set3DAttributes(SoundSourceBehaviour.SoundSourceTransform.To3DAttributes());
		}

		public float GetDistanceToTarget()
		{
			if (!HasTarget)
			{
				return float.PositiveInfinity;
			}
			if (!_spawnedPlayers.Players.TryGetValue(TargetPlayer, out var value))
			{
				return float.PositiveInfinity;
			}
			return Vector3.Distance(value.NetworkObject.transform.position, base.transform.position);
		}

		public bool IsTargetLookingAtMe(float hardDetectionDistance, float raycastThreshold, bool angleCullEnabled)
		{
			if (!HasTarget)
			{
				return false;
			}
			if (!_spawnedPlayers.Players.TryGetValue(TargetPlayer, out var value))
			{
				return false;
			}
			PlayerLookDetection component = value.NetworkObject.GetComponent<PlayerLookDetection>();
			if (component != null)
			{
				return component.IsLookingAtObject(LookAtTarget, angleCullEnabled, hardDetectionDistance, raycastThreshold);
			}
			return false;
		}

		public bool IsPlayerEligibleForDetection(int playerId, PlayerState[] ignoredStates)
		{
			if (_playersStatesSynchronizer.TryGetState(playerId, out var state) && ignoredStates != null && Array.IndexOf(ignoredStates, state) >= 0)
			{
				return false;
			}
			return _playerStateService.IsPlayerAlive(playerId);
		}

		public float GetStatValue(EntityStatType statType, float fallbackValue = 0f)
		{
			return StatEntity.GetStat(statType)?.Value ?? fallbackValue;
		}

		private void AssertAssigned(UnityEngine.Object reference, string fieldName)
		{
			if (reference != null)
			{
				return;
			}
			throw new MissingReferenceException("SirenEnemyContext on '" + base.name + "' requires '" + fieldName + "' to be assigned.");
		}
	}
}

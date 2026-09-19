using System;
using FMOD.Studio;
using FMODUnity;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Shared;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy
{
	public class HeadcrabEnemyContext : MonoBehaviour
	{
		public EventInstance SittingOnPlayerLoopedSoundInstance;

		private SpawnedPlayersModel _spawnedPlayers;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private IAudioService _audioService;

		[field: Header("Scene refs")]
		[field: SerializeField]
		public Rigidbody Rigidbody { get; private set; }

		[field: SerializeField]
		public NetworkedAnimator NetworkedAnimator { get; private set; }

		[field: SerializeField]
		public VisibilityHandlerBase VisibilityHandler { get; private set; }

		[field: SerializeField]
		public SimpleEnemyHealthController LegacyHealthController { get; private set; }

		[field: SerializeField]
		public Transform NoParentGrabPoint { get; private set; }

		[field: SerializeField]
		public HeadCrabHome HomePrefab { get; private set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: Header("Sensors")]
		[field: SerializeField]
		public EnemyTargetDetector TargetDetector { get; private set; }

		[field: SerializeField]
		public EnemyTargetDetector ResnapTargetDetector { get; private set; }

		[field: Header("Raycasts")]
		[field: SerializeField]
		public LayerMask CeilingLayerMask { get; private set; }

		[field: SerializeField]
		public LayerMask VisibilityLayerMask { get; private set; }

		[field: Header("Audio")]
		[field: SerializeField]
		public EventReference FoundPlayerSound { get; private set; }

		[field: SerializeField]
		public EventReference DeattachSound { get; private set; }

		[field: SerializeField]
		public EventReference AttachToPlayerSound { get; private set; }

		[field: SerializeField]
		public EventReference SittingOnPlayerLoopedSound { get; private set; }

		[field: SerializeField]
		public EventReference KillPlayerSound { get; private set; }

		[field: SerializeField]
		public EventReference GoHomeSound { get; private set; }

		public bool IsFearing { get; set; }

		public bool IsDespawnAfterFear { get; set; } = true;

		public bool IsAttached { get; set; }

		public PlayerRef TargetPlayer { get; set; } = PlayerRef.None;

		public NetworkObject TargetObject { get; set; }

		public HeadCrabHome HomeInstance { get; set; }

		public ParentWithPosition CurrentParentWithPosition { get; set; }

		public float ChaseElapsed { get; set; }

		public float SnapElapsed { get; set; }

		public float ResnapCooldownElapsed { get; set; }

		public bool WithVisibilityChange { get; set; }

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayers, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, IAudioService audioService)
		{
			_spawnedPlayers = spawnedPlayers;
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

		public void CreateFmodInstances()
		{
			SittingOnPlayerLoopedSoundInstance = _audioService.CreateInstance(SittingOnPlayerLoopedSound);
		}

		public void ReleaseFmodInstances()
		{
			if (SittingOnPlayerLoopedSoundInstance.isValid())
			{
				_audioService.StopInstance(SittingOnPlayerLoopedSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(SittingOnPlayerLoopedSoundInstance);
			}
		}

		public void UpdateFmod3DAttributes()
		{
			if (SittingOnPlayerLoopedSoundInstance.isValid())
			{
				SittingOnPlayerLoopedSoundInstance.set3DAttributes(SoundSourceBehaviour.SoundSourceTransform.To3DAttributes());
			}
		}

		public bool TryGetPlayerObject(PlayerRef player, out NetworkObject playerObject)
		{
			playerObject = null;
			if (_spawnedPlayers != null && _spawnedPlayers.Players.TryGetValue(player, out var value))
			{
				return (playerObject = value.NetworkObject) != null;
			}
			return false;
		}

		public bool RaycastToTarget(NetworkObject target)
		{
			if (target == null)
			{
				return false;
			}
			Vector3 position = base.transform.position;
			Vector3 normalized = (target.transform.position - position).normalized;
			float maxDistance = Vector3.Distance(position, target.transform.position);
			if (Physics.Raycast(position, normalized, out var hitInfo, maxDistance, VisibilityLayerMask))
			{
				return hitInfo.transform == target.transform;
			}
			return true;
		}

		public RaycastHit? RaycastUpward(float distance)
		{
			RaycastHit[] array = Physics.RaycastAll(base.transform.position, Vector3.up, distance, CeilingLayerMask);
			if (array == null || array.Length == 0)
			{
				return null;
			}
			Array.Sort(array, (RaycastHit a, RaycastHit b) => a.transform.position.y.CompareTo(b.transform.position.y));
			return array[^1];
		}
	}
}

using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy
{
	public class SharkEnemyContext : MonoBehaviour
	{
		public EventInstance IdleLoopSoundInstance;

		private int _lastPlayerHuntLostId = -1;

		private float _lastPlayerHuntLostUnscaledTime = -1f;

		private float _lastAttackStartSoundUnscaledTime = -999f;

		private bool _waitingForRetreatAfterStartHuntSound;

		private SpawnedPlayersModel _spawnedPlayers;

		private INavigationService _navigationService;

		private EnemyTargetPositionsModel _targetPositionsModel;

		private int _originalAreaMask;

		private PlayerMovableModel _playerMovableModel;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private SharkAudioSettings _audioSettings;

		private IAudioService _audioService;

		[field: Header("Scene")]
		[field: SerializeField]
		public Rigidbody FinBody { get; private set; }

		[field: SerializeField]
		public Transform FinVisual { get; private set; }

		[field: SerializeField]
		public Collider HuntBounds { get; private set; }

		[field: SerializeField]
		public SimpleEnemyDamageable FinDamageable { get; private set; }

		[field: SerializeField]
		public EnemyStatHealthController StatHealthController { get; private set; }

		[field: SerializeField]
		public SimpleEnemyDeadProcessor DeadProcessor { get; private set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: Header("Presentation")]
		[field: SerializeField]
		public GameObject FinRoot { get; private set; }

		[field: SerializeField]
		public GameObject SharkRoot { get; private set; }

		[field: Header("Navigation")]
		[field: SerializeField]
		public NavMeshAgent Agent { get; private set; }

		[field: Header("Targeting")]
		[field: SerializeField]
		public EnemySafeZoneAttackDetector SafeZoneDetector { get; private set; }

		[field: Header("Sounds")]
		[field: SerializeField]
		public EventReference AttackStartReference { get; private set; }

		[field: SerializeField]
		public EventReference StartHuntReference { get; private set; }

		[field: SerializeField]
		public EventReference IdleLoopReference { get; private set; }

		[field: SerializeField]
		public string VoiceOcclusionLowPassParameterName { get; private set; } = "VoiceOcclusionLowPass";

		[field: SerializeField]
		public LayerMask OcclusionLayerMask { get; private set; }

		[field: SerializeField]
		public float OcclusionMaxDistance { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalDistanceMultiplier { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalFalloffExponent { get; private set; } = 2f;

		[field: SerializeField]
		public float LowPassMinValue { get; private set; } = 0.3f;

		public bool IsFearing { get; set; }

		public bool IsDespawnAfterFear { get; set; } = true;

		public PlayerRef TargetPlayer { get; set; } = PlayerRef.None;

		public NetworkObject TargetObject { get; set; }

		public bool IsMimicTarget
		{
			get
			{
				if (TargetPlayer == PlayerRef.None && TargetObject != null)
				{
					return TargetObject.IsValid;
				}
				return false;
			}
		}

		public float AttackCooldownElapsed { get; set; }

		public float BackoffElapsed { get; set; }

		public float AttackStateElapsed { get; set; }

		public bool AttackDamageApplied { get; set; }

		public bool AttackExitSent { get; set; }

		public Vector3 BackoffDirection { get; set; }

		public float WanderTimer { get; set; }

		public float WanderUpdateFrequency { get; private set; }

		public float IdleChangeAreaTimer { get; set; }

		public float LineOfSightLostElapsed { get; set; }

		public Vector3 AreaPosition { get; private set; }

		private bool IsNavMeshMovementReady
		{
			get
			{
				if (Agent.enabled)
				{
					return Agent.isOnNavMesh;
				}
				return false;
			}
		}

		public AttractionZoneData PendingAttractionZone { get; private set; }

		public bool CanEnemyInteractWithTargetPlayer()
		{
			if (TargetPlayer != PlayerRef.None)
			{
				return _enemyPlayerAttackabilityService.CanEnemyAttackPlayer(TargetPlayer.PlayerId);
			}
			return false;
		}

		public bool ShouldDebounceAttackStartSound()
		{
			if (_audioSettings.AttackSoundReacquireDebounce <= 0f)
			{
				return false;
			}
			return Time.unscaledTime - _lastAttackStartSoundUnscaledTime < _audioSettings.AttackSoundReacquireDebounce;
		}

		public void RecordAttackStartSoundPlayed()
		{
			_lastAttackStartSoundUnscaledTime = Time.unscaledTime;
		}

		public void ClearAttackStartSoundDebounce()
		{
			_lastAttackStartSoundUnscaledTime = -999f;
		}

		public void RecordPlayerHuntLost(PlayerRef player)
		{
			if (!(player == PlayerRef.None))
			{
				_lastPlayerHuntLostId = player.PlayerId;
				_lastPlayerHuntLostUnscaledTime = Time.unscaledTime;
			}
		}

		public bool ShouldDebounceStartHuntSoundForPlayer(PlayerRef player)
		{
			if (player == PlayerRef.None || _audioSettings.HuntSoundReacquireDebounce <= 0f)
			{
				return false;
			}
			if (player.PlayerId != _lastPlayerHuntLostId)
			{
				return false;
			}
			return Time.unscaledTime - _lastPlayerHuntLostUnscaledTime < _audioSettings.HuntSoundReacquireDebounce;
		}

		public void ClearPlayerHuntLostRecord()
		{
			_lastPlayerHuntLostId = -1;
			_lastPlayerHuntLostUnscaledTime = -1f;
		}

		public bool IsStartHuntSoundBlockedByRetreat()
		{
			if (!_waitingForRetreatAfterStartHuntSound)
			{
				return false;
			}
			float startHuntSoundMinRetreatDistance = _audioSettings.StartHuntSoundMinRetreatDistance;
			if (startHuntSoundMinRetreatDistance <= 0f)
			{
				return false;
			}
			return HasAnyPlayerWithinDistance(startHuntSoundMinRetreatDistance);
		}

		public void OnStartHuntSoundPlayedOnHost()
		{
			if (!(_audioSettings.StartHuntSoundMinRetreatDistance <= 0f))
			{
				_waitingForRetreatAfterStartHuntSound = true;
			}
		}

		public void TickStartHuntSoundRetreatCheckOnHost()
		{
			if (_waitingForRetreatAfterStartHuntSound)
			{
				float startHuntSoundMinRetreatDistance = _audioSettings.StartHuntSoundMinRetreatDistance;
				if (startHuntSoundMinRetreatDistance <= 0f)
				{
					_waitingForRetreatAfterStartHuntSound = false;
				}
				else if (!HasAnyPlayerWithinDistance(startHuntSoundMinRetreatDistance))
				{
					_waitingForRetreatAfterStartHuntSound = false;
				}
			}
		}

		public void ClearStartHuntSoundRetreatState()
		{
			_waitingForRetreatAfterStartHuntSound = false;
		}

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayers, INavigationService navigationService, EnemyTargetPositionsModel targetPositionsModel, PlayerMovableModel playerMovableModel, SharkAudioSettings audioSettings, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, IAudioService audioService)
		{
			_spawnedPlayers = spawnedPlayers;
			_navigationService = navigationService;
			_targetPositionsModel = targetPositionsModel;
			_playerMovableModel = playerMovableModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_audioSettings = audioSettings;
			_audioService = audioService;
		}

		public void ValidateRequiredReferences()
		{
			AssertAssigned(Agent, "Agent");
			AssertAssigned(FinBody, "FinBody");
			AssertAssigned(FinDamageable, "FinDamageable");
			AssertAssigned(StatHealthController, "StatHealthController");
			AssertAssigned(DeadProcessor, "DeadProcessor");
			AssertAssigned(FinRoot, "FinRoot");
			AssertAssigned(SharkRoot, "SharkRoot");
			AssertAssigned(HuntBounds, "HuntBounds");
			if (_spawnedPlayers == null)
			{
				throw new InvalidOperationException("SharkEnemyContext on '" + base.name + "' requires injected SpawnedPlayersModel.");
			}
			if (_navigationService == null)
			{
				throw new InvalidOperationException("SharkEnemyContext on '" + base.name + "' requires injected INavigationService.");
			}
			if (_targetPositionsModel == null)
			{
				throw new InvalidOperationException("SharkEnemyContext on '" + base.name + "' requires injected EnemyTargetPositionsModel.");
			}
			if (_audioSettings == null)
			{
				throw new InvalidOperationException("SharkEnemyContext on '" + base.name + "' requires injected SharkAudioSettings.");
			}
		}

		public void CreateFmodInstances()
		{
			if (!IdleLoopReference.Guid.IsNull)
			{
				IdleLoopSoundInstance = _audioService.CreateInstance(IdleLoopReference);
			}
			_audioService.StartInstanceWith3DAttributes(IdleLoopSoundInstance, SoundSourceBehaviour);
		}

		public void ReleaseFmodInstances()
		{
			ReleaseInstance(ref IdleLoopSoundInstance);
		}

		public void UpdateFmod3DAttributes()
		{
			ATTRIBUTES_3D attributes = GetSoundEmitterPosition().To3DAttributes();
			if (IdleLoopSoundInstance.isValid())
			{
				IdleLoopSoundInstance.set3DAttributes(attributes);
			}
		}

		public void PlayAttackStartSound()
		{
			PlayOneShot(AttackStartReference);
		}

		public void PlayStartHuntSoundLocal()
		{
			if (CanLocalPlayerHearStartHuntSound())
			{
				PlayOneShot(StartHuntReference);
			}
		}

		private bool CanLocalPlayerHearStartHuntSound()
		{
			if (!TryGetLocalPlayerSoundPosition(out var position))
			{
				return false;
			}
			return IsWithinAudibleDistance(position);
		}

		private bool HasAnyPlayerWithinAudibleDistance()
		{
			return HasAnyPlayerWithinDistance(_audioSettings.StartHuntSoundAudibleDistance);
		}

		private bool HasAnyPlayerWithinDistance(float distance)
		{
			if (distance <= 0f)
			{
				return false;
			}
			if (_playerMovableModel?.AllCharacterMovables == null)
			{
				return false;
			}
			Vector3 soundEmitterPosition = GetSoundEmitterPosition();
			foreach (PlayerCharacterMovableBase value in _playerMovableModel.AllCharacterMovables.Values)
			{
				if (!(value == null) && value.Object.IsValid && IsWithinHorizontalDistance(((value.CameraPositionTransform != null) ? value.CameraPositionTransform : value.transform).position, soundEmitterPosition, distance))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsWithinAudibleDistance(Vector3 playerPosition)
		{
			float startHuntSoundAudibleDistance = _audioSettings.StartHuntSoundAudibleDistance;
			if (startHuntSoundAudibleDistance <= 0f)
			{
				return true;
			}
			return IsWithinHorizontalDistance(playerPosition, GetSoundEmitterPosition(), startHuntSoundAudibleDistance);
		}

		private bool TryGetLocalPlayerSoundPosition(out Vector3 position)
		{
			position = default(Vector3);
			if (_playerMovableModel.LocalMovable == null || _playerMovableModel.LocalMovable.CameraPositionTransform == null)
			{
				return false;
			}
			position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
			return true;
		}

		private static bool IsWithinHorizontalDistance(Vector3 from, Vector3 to, float distance)
		{
			float num = from.x - to.x;
			float num2 = from.z - to.z;
			return num * num + num2 * num2 <= distance * distance;
		}

		private void PlayOneShot(EventReference reference)
		{
			if (!reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, SoundSourceBehaviour);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * VerticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(VoiceOcclusionLowPassParameterName, out var _);
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, OcclusionMaxDistance);
					float value2 = Mathf.Lerp(1f, LowPassMinValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, value2);
				}
				else
				{
					eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, 1f);
				}
			}
		}

		public Vector3 GetSoundEmitterPosition()
		{
			return FinBody.position;
		}

		public void InitNavAgent(SharkEnemy shark, SharkMovementSettings movement)
		{
			Agent.enabled = shark.HasStateAuthority;
			if (shark.HasStateAuthority)
			{
				ApplyNavAgentSettings(movement);
				TrySnapNavAgentOntoNavMesh(movement);
			}
		}

		public void EnableNavAgentForStateAuthority(SharkMovementSettings movement)
		{
			if (!(Agent == null))
			{
				Agent.enabled = true;
				ApplyNavAgentSettings(movement);
			}
		}

		private void ApplyNavAgentSettings(SharkMovementSettings movement)
		{
			Agent.speed = movement.IdleMovementSpeed;
			Agent.stoppingDistance = movement.NavMeshStoppingDistance;
			Agent.acceleration = movement.NavMeshAcceleration;
			Agent.angularSpeed = movement.NavMeshAngularSpeed;
			Agent.updateUpAxis = true;
			_originalAreaMask = Agent.areaMask;
		}

		public bool TrySnapNavAgentOntoNavMesh(SharkMovementSettings movement)
		{
			if (!Agent.enabled)
			{
				return false;
			}
			return _navigationService.TryWarpAgentOntoNavMesh(Agent, base.transform.position, movement.NavMeshSpawnSampleRadius);
		}

		public void DisposeNavAgent(SharkEnemy shark)
		{
			if (shark.HasStateAuthority && Agent.isOnNavMesh)
			{
				Agent.ResetPath();
			}
			_targetPositionsModel.RemoveEnemyTargetPosition(EnemyType.Shark, base.gameObject.GetHashCode());
			Agent.enabled = false;
			ClearPlayerHuntLostRecord();
			ClearStartHuntSoundRetreatState();
			_lastAttackStartSoundUnscaledTime = -999f;
		}

		public void SetNavMovementEnabled(bool enabled)
		{
			if (!enabled && Agent.isOnNavMesh)
			{
				Agent.ResetPath();
			}
		}

		public bool TrySetChaseDestinationNear(Vector3 playerWorldPos, float sampleRange)
		{
			if (!IsNavMeshMovementReady)
			{
				return false;
			}
			if (!_navigationService.HasAvailablePointInRange(Agent, playerWorldPos, sampleRange, out var availablePosition))
			{
				return false;
			}
			Agent.SetDestination(availablePosition);
			RegisterTargetPosition(availablePosition);
			return true;
		}

		public void UpdateWanderUpdateFrequency(SharkMovementSettings movement)
		{
			float num = Mathf.Max(0f, movement.MinWanderUpdateTime);
			float maxInclusive = Mathf.Max(num, movement.MaxWanderUpdateTime);
			WanderUpdateFrequency = UnityEngine.Random.Range(num, maxInclusive);
		}

		public bool TrySetWanderDestination(SharkMovementSettings movement)
		{
			if (!IsNavMeshMovementReady)
			{
				return false;
			}
			Vector3 center = HuntBounds.bounds.center;
			int attempts = Mathf.Max(1, movement.WanderNavRandomAttempts);
			if (!_navigationService.TryGetRandomSafeNavmeshPosition(center, movement.WanderRadius, attempts, null, Agent.agentTypeID, Agent.areaMask, out var bestPosition))
			{
				return false;
			}
			Agent.SetDestination(bestPosition);
			RegisterTargetPosition(bestPosition);
			return true;
		}

		public bool TrySetBackoffDestination(Vector3 fromWorld, Vector3 directionXZNormalized, float distance, SharkMovementSettings movement)
		{
			if (!IsNavMeshMovementReady)
			{
				return false;
			}
			Vector3 vector = directionXZNormalized;
			vector.y = 0f;
			if (vector.sqrMagnitude < 1E-06f)
			{
				return false;
			}
			vector.Normalize();
			Vector3 vector2 = fromWorld + vector * distance;
			float backoffNavSearchRadius = movement.BackoffNavSearchRadius;
			if (_navigationService.HasAvailablePointInRange(Agent, vector2, backoffNavSearchRadius * 0.5f, out var availablePosition))
			{
				Agent.SetDestination(availablePosition);
				RegisterTargetPosition(availablePosition);
				return true;
			}
			if (_navigationService.TryGetRandomSafeNavmeshPosition(vector2, backoffNavSearchRadius, movement.BackoffNavRandomAttempts, out var position) && _navigationService.HasAvailablePointInRange(Agent, position, backoffNavSearchRadius * 0.5f, out var availablePosition2))
			{
				Agent.SetDestination(availablePosition2);
				RegisterTargetPosition(availablePosition2);
				return true;
			}
			if (_navigationService.TryGetRandomNavmeshPosition(vector2, backoffNavSearchRadius, out var position2) && _navigationService.HasAvailablePointInRange(Agent, position2, backoffNavSearchRadius * 0.5f, out var availablePosition3))
			{
				Agent.SetDestination(availablePosition3);
				RegisterTargetPosition(availablePosition3);
				return true;
			}
			return false;
		}

		public void RestoreDefaultNavAreaMask()
		{
			Agent.areaMask = _originalAreaMask;
		}

		private void RegisterTargetPosition(Vector3 position)
		{
			_targetPositionsModel.UpdateEnemyTargetPosition(EnemyType.Shark, base.gameObject.GetHashCode(), position);
		}

		public bool TryGetPlayerObject(PlayerRef player, out NetworkObject playerObject)
		{
			playerObject = null;
			if (_spawnedPlayers.Players.TryGetValue(player, out var value))
			{
				return (playerObject = value.NetworkObject) != null;
			}
			return false;
		}

		public Vector3 ClampToHuntBounds(Vector3 worldPos, float padding)
		{
			Bounds bounds = HuntBounds.bounds;
			bounds.Expand((0f - padding) * 2f);
			return new Vector3(Mathf.Clamp(worldPos.x, bounds.min.x, bounds.max.x), worldPos.y, Mathf.Clamp(worldPos.z, bounds.min.z, bounds.max.z));
		}

		public bool IsChaseTargetReachableOnNavMesh(Vector3 playerWorldPos, float maxNavMeshPathLength, float sampleRadius)
		{
			if (!IsNavMeshMovementReady)
			{
				return true;
			}
			if (!_navigationService.TryGetCompletePath(Agent, playerWorldPos, sampleRadius, out var path))
			{
				return false;
			}
			if (maxNavMeshPathLength > 0f && path.GetLength() > maxNavMeshPathLength)
			{
				return false;
			}
			return true;
		}

		private void ReleaseInstance(ref EventInstance instance)
		{
			if (instance.isValid())
			{
				_audioService.StopInstance(instance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(instance);
			}
		}

		private void AssertAssigned(UnityEngine.Object reference, string fieldName)
		{
			if (reference != null)
			{
				return;
			}
			throw new MissingReferenceException("SharkEnemyContext on '" + base.name + "' requires '" + fieldName + "' to be assigned.");
		}

		public void SetAreaPosition(Vector3 areaPosition)
		{
			AreaPosition = areaPosition;
			_targetPositionsModel.UpdateEnemyAreaPosition(EnemyType.Shark, base.gameObject.GetHashCode(), AreaPosition);
		}

		public void SetPendingAttractionZone(AttractionZoneData zone)
		{
			PendingAttractionZone = zone;
		}

		public bool CanReachZoneOrigin(SharkMovementSettings movement)
		{
			if (!IsNavMeshMovementReady)
			{
				return false;
			}
			Vector3 destination;
			return TrySampleZoneWaterPoint(movement, out destination);
		}

		public bool TrySetZoneApproachDestination(SharkMovementSettings movement)
		{
			if (!IsNavMeshMovementReady)
			{
				return false;
			}
			if (!TrySampleZoneWaterPoint(movement, out var destination))
			{
				return false;
			}
			Agent.SetDestination(destination);
			RegisterTargetPosition(destination);
			return true;
		}

		private bool TrySampleZoneWaterPoint(SharkMovementSettings movement, out Vector3 destination)
		{
			int attempts = Mathf.Max(1, movement.WanderNavRandomAttempts);
			float searchRadius = Mathf.Max(0.5f, PendingAttractionZone.ApproachRadius);
			return _navigationService.TryGetRandomSafeNavmeshPosition(PendingAttractionZone.Origin, searchRadius, attempts, null, Agent.agentTypeID, Agent.areaMask, out destination);
		}
	}
}

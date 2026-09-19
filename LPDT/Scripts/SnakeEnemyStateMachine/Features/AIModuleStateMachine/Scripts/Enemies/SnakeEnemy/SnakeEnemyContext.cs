using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems;
using Features.AnimationModule.Scripts;
using Features.LevelGatesModule.Data;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.SnakeModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	[NetworkBehaviourWeaved(12)]
	public class SnakeEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
	{
		private const float SMOOTHED_VELOCITY_QUANTIZE_FACTOR = 10f;

		private const float MOVE_SPEED_CHANGE_THRESHOLD = 0.01f;

		private PlayersGatesModelSynchronizedModel _playersGatesModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private MultiplayerModel _multiplayerModel;

		private INavigationService _navigationService;

		private IPlayerStateService _playerStateService;

		private EnemySafeZoneInteractionPointsModel _safeZoneInteractionPointsModel;

		private PlayerMovableModel _playerMovableModel;

		[SerializeField]
		private float _moveSpeed = 4f;

		[SerializeField]
		private float _smoothedVelocityLerpSpeed = 5f;

		[SerializeField]
		private float _stepAggroMoveSpeed = 8f;

		[Tooltip("SnakeVisualSlither frequency while StepAggro. Idle keeps the slither's base _frequency.")]
		[SerializeField]
		private float _chaseSlitherFrequency = 10f;

		[Tooltip("SnakeController Visual yaw slerp rate while StepAggro (like Wrap, without suppressing velocity yaw).")]
		[SerializeField]
		private float _chaseVisualRotationSpeed = 20f;

		[SerializeField]
		private float _stepAggroStandDistance = 2.5f;

		[SerializeField]
		private float _stepAggroLoseDistance = 12f;

		[SerializeField]
		private float _stepAggroWrapOrbitRadius = 1.8f;

		[SerializeField]
		private float _stepAggroWrapAngularSpeed = 720f;

		[SerializeField]
		private float _stepAggroWrapMoveSpeed = 14f;

		[Tooltip("Time to ease from wrap angular speed down to strangle speed. Walk lock stays on until Wrap exits (fear/death).")]
		[SerializeField]
		private float _stepAggroWrapCoilDuration = 3f;

		[SerializeField]
		private float _stepAggroStrangleAngularSpeed = 30f;

		[Tooltip("How far below the player transform the wrap coil sits. 0 = coil at player root; ~1 when root is body center (waist-level wrap). Follows player Y up and down.")]
		[SerializeField]
		private float _wrapOrbitPlayerPivotHeight = 1f;

		[Tooltip("Max |playerY - snake root Y| allowed to start Wrap Visual. Beyond this → Idle wander (cannot reach).")]
		[SerializeField]
		private float _wrapMaxVerticalReach = 5f;

		[Tooltip("After a player escapes Wrap via StruggleBar or ally body-pull, block step-aggro wrap for this many seconds.")]
		[SerializeField]
		private float _wrapEscapeCooldownDuration = 8f;

		[Tooltip("StruggleBar drain while this snake strangling. Overrides global StruggleBarConfiguration.")]
		[SerializeField]
		private float _wrapStruggleDrainPerSecond = 0.25f;

		[Tooltip("StruggleBar fill per Jump press while strangling.")]
		[SerializeField]
		private float _wrapStruggleBoostPerPress = 0.08f;

		[Tooltip("StruggleBar starting fill (0–1) when strangle begins.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _wrapStruggleStartNormalized = 0.35f;

		[Tooltip("Damage dealt to the wrap victim every interval while fully strangling.")]
		[SerializeField]
		private float _wrapStrangleDamage = 20f;

		[Tooltip("Seconds between strangle damage ticks while fully wrapped.")]
		[SerializeField]
		private float _wrapStrangleDamageInterval = 2f;

		[Tooltip("Burst damage when StruggleBar drains to empty (snake finishes the wrap), then release.")]
		[SerializeField]
		private float _wrapFinishDamage = 100f;

		[SerializeField]
		private float _targetSearchRange = 25f;

		[SerializeField]
		private float _completePointMinDistance = 1.5f;

		[SerializeField]
		private float _distanceToAttack = 2f;

		[SerializeField]
		private float _attackCooldownTime = 3f;

		[Tooltip("NavMesh sample radius when picking a fear flee point away from players.")]
		[SerializeField]
		private float _fearFleeRadius = 25f;

		[Tooltip("Players still \"see\" the snake within this distance (look / close range / reachable).")]
		[SerializeField]
		private float _fearVisibilityDistance = 25f;

		[Tooltip("How often to re-pick a flee destination while players can still see the snake.")]
		[SerializeField]
		private float _fearDestinationUpdateInterval = 0.5f;

		[Tooltip("Treat path end as reached when closer than this to agent pathEndPosition.")]
		[SerializeField]
		private float _fearDestinationReachedDistance = 1.5f;

		[Tooltip("Seconds to wait at each safe-zone InteractionPoint before picking the next one.")]
		[SerializeField]
		private float _safeZoneWaitDuration = 5f;

		[Tooltip("How many approach→wait cycles to run before returning to Idle wander.")]
		[SerializeField]
		private int _safeZoneApproachVisitCount = 3;

		[Tooltip("Animator trigger fired for under-table wait and wrap start (SnakeAnimationController Attack).")]
		[SerializeField]
		private string _attackTriggerName = "Attack";

		[Tooltip("XZ distance at which the safe-zone InteractionPoint is considered reached.")]
		[SerializeField]
		private float _safeZoneApproachReachedDistance = 0.75f;

		[Tooltip("XZ distance to the approach point at which the head starts facing the player (while moving).")]
		[SerializeField]
		private float _safeZoneApproachLookStartDistance = 2.5f;

		[Tooltip("Reject InteractionPoints that have body segments within this XZ radius (avoids crawl-over head lift). 0 = use SnakeController.CrawlOverRadius.")]
		[SerializeField]
		private float _safeZoneBodyClearance;

		[Tooltip("Body chord segments near the head ignored when testing InteractionPoint clearance.")]
		[SerializeField]
		private int _safeZoneIgnoredHeadBodySegments = 2;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SmoothedVelocityQuantized", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _SmoothedVelocityQuantized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("MoveSpeed", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _MoveSpeed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsCrouching", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsCrouching;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SnakeVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsWrapOrbitActive", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsWrapOrbitActive;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsWrapStrangling", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsWrapStrangling;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("WrapOrbitAngleRadians", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _WrapOrbitAngleRadians;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("WrapOrbitHeight", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _WrapOrbitHeight;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("WrapTargetPlayerId", 8, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _WrapTargetPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("WrapOrbitCenter", 9, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _WrapOrbitCenter;

		private PlayerDataHolder _pinnedPriorityPlayer;

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public Transform LookTargetTransform { get; private set; }

		[field: SerializeField]
		public EnemySafeZoneAttackDetector SafeZoneDetector { get; private set; }

		[field: SerializeField]
		public SnakeController SnakeController { get; private set; }

		[field: SerializeField]
		public SnakeVisualSlither SnakeVisualSlither { get; private set; }

		[field: SerializeField]
		public NetworkedAnimationController AnimationController { get; private set; }

		[field: SerializeField]
		public MoveSystem MoveSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedSystem TargetPositionCompletedSystem { get; private set; }

		[field: SerializeField]
		public AreaTypeTrackSystem AreaTypeTrackSystem { get; private set; }

		[field: SerializeField]
		public FindSnakeRandomPositionSystem FindSnakeRandomPositionSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedResetSystem TargetPositionCompletedResetSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionSystem FindTargetPlayerPositionSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionResetSystem FindTargetPlayerPositionResetSystem { get; private set; }

		[field: SerializeField]
		public PlayerDetectingSystem PlayerDetectingSystem { get; private set; }

		[field: SerializeField]
		public SnakeVisionDetectingSystem SnakeVisionDetectingSystem { get; private set; }

		[field: SerializeField]
		public EnemyDetectionAnalyticsSystem EnemyDetectionAnalyticsSystem { get; private set; }

		[field: SerializeField]
		public TargetPlayerPrioritizeSystem TargetPlayerPrioritizeSystem { get; private set; }

		[field: SerializeField]
		public DetectedPlayersTimeSystem DetectedPlayersTimeSystem { get; private set; }

		[field: SerializeField]
		public StateDurationTimeSystem StateDurationTimeSystem { get; private set; }

		[field: SerializeField]
		public SnakeWrapOrbitSystem WrapOrbitSystem { get; private set; }

		[field: SerializeField]
		public SnakeDamageReactionSystem SnakeDamageReactionSystem { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(short*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe float MoveSpeed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe bool IsCrouching
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe SnakeVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (SnakeVisualState)Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe NetworkBool IsWrapOrbitActive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.IsWrapOrbitActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 4);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.IsWrapOrbitActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe NetworkBool IsWrapStrangling
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.IsWrapStrangling. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 5);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.IsWrapStrangling. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 5) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 1)]
		public unsafe float WrapOrbitAngleRadians
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapOrbitAngleRadians. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 6);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapOrbitAngleRadians. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 6) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(7, 1)]
		public unsafe float WrapOrbitHeight
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapOrbitHeight. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 7);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapOrbitHeight. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 7) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(8, 1)]
		public unsafe int WrapTargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[8];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[8] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(9, 3)]
		public unsafe Vector3 WrapOrbitCenter
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapOrbitCenter. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 9);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SnakeEnemyContext.WrapOrbitCenter. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 9) = value;
			}
		}

		public int CurrentAreaType { get; private set; }

		public Vector3 TargetPosition { get; private set; }

		public bool TargetPositionCompleted { get; private set; }

		public bool NeedToFindTargetPosition { get; set; }

		public float CompletePointMinDistance { get; set; }

		public float TargetSearchRange { get; set; }

		public float AvailablePointRange { get; set; } = 0.5f;

		public float TimeToAttack { get; set; }

		public float DistanceToAttack { get; set; }

		public float AttackCooldown { get; set; }

		public bool IsAttackOnCooldown { get; set; }

		public float CurrentStateTime { get; set; }

		public float StepAggroStandDistance { get; private set; }

		public float StepAggroLoseDistance { get; private set; }

		public float StepAggroWrapOrbitRadius { get; private set; }

		public float StepAggroWrapAngularSpeed { get; private set; }

		public float StepAggroWrapMoveSpeed { get; private set; }

		public float StepAggroWrapCoilDuration { get; private set; }

		public float StepAggroStrangleAngularSpeed { get; private set; }

		public float WrapOrbitPlayerPivotHeight { get; private set; }

		public float WrapMaxVerticalReach { get; private set; }

		public float WrapEscapeCooldownDuration { get; private set; }

		public float WrapEscapeCooldownRemaining { get; private set; }

		public float WrapStruggleDrainPerSecond { get; private set; }

		public float WrapStruggleBoostPerPress { get; private set; }

		public float WrapStruggleStartNormalized { get; private set; }

		public float WrapStrangleDamage { get; private set; }

		public float WrapStrangleDamageInterval { get; private set; }

		public float WrapFinishDamage { get; private set; }

		public float FearFleeRadius { get; private set; }

		public float FearVisibilityDistance { get; private set; }

		public float FearDestinationUpdateInterval { get; private set; }

		public float FearDestinationReachedDistance { get; private set; }

		public float FearDestinationTimer { get; set; }

		public float SafeZoneWaitDuration { get; private set; }

		public int SafeZoneApproachVisitCount { get; private set; }

		public float SafeZoneApproachReachedDistance { get; private set; }

		public float SafeZoneApproachLookStartDistance { get; private set; }

		public float SafeZoneBodyClearance { get; private set; }

		public int SafeZoneIgnoredHeadBodySegments { get; private set; }

		public Vector3 SafeZoneApproachPosition { get; private set; }

		public bool HasSafeZoneApproachPosition { get; private set; }

		public bool IsWrapEscapeCooldownActive => WrapEscapeCooldownRemaining > 0f;

		public float PriorityPinRemaining { get; private set; }

		public bool IsPriorityPinned
		{
			get
			{
				if (PriorityPinRemaining > 0f && _pinnedPriorityPlayer != null)
				{
					return _pinnedPriorityPlayer.NetworkObject != null;
				}
				return false;
			}
		}

		public Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; } = new Dictionary<PlayerDataHolder, float>();

		public Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; } = new Dictionary<PlayerDataHolder, float>();

		public List<PlayerDataHolder> DetectedPlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<PlayerDataHolder> VisiblePlayers { get; private set; } = new List<PlayerDataHolder>();

		public PlayerDataHolder PriorityPlayer { get; private set; }

		public float SmoothedVelocity => (float)SmoothedVelocityQuantized / 10f;

		public float SmoothedVelocityLerpSpeed => _smoothedVelocityLerpSpeed;

		public event Action OnDetectedPlayersChanged;

		public event Action OnVisiblePlayersChanged;

		public event Action OnPriorityPlayerChanged;

		[Inject]
		private void InjectDependencies(PlayersGatesModelSynchronizedModel playersGatesModel, SpawnedPlayersModel spawnedPlayersModel, MultiplayerModel multiplayerModel, INavigationService navigationService, IPlayerStateService playerStateService, EnemySafeZoneInteractionPointsModel safeZoneInteractionPointsModel, PlayerMovableModel playerMovableModel)
		{
			_playersGatesModel = playersGatesModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_multiplayerModel = multiplayerModel;
			_navigationService = navigationService;
			_playerStateService = playerStateService;
			_safeZoneInteractionPointsModel = safeZoneInteractionPointsModel;
			_playerMovableModel = playerMovableModel;
		}

		public bool IsPlayerOutsideGate(int playerId)
		{
			if (_playersGatesModel != null && _playersGatesModel.TryGetPlayerState(playerId, out var state))
			{
				return !state.PlayerInsideGate;
			}
			return false;
		}

		public bool CanTargetPlayerForWrap(int playerId)
		{
			if (_playerStateService != null)
			{
				return _playerStateService.IsPlayerAlive(playerId);
			}
			return false;
		}

		public float EvaluateWrapOrbitAngularSpeed(float coilElapsedSeconds)
		{
			if ((bool)IsWrapStrangling || StepAggroWrapCoilDuration <= 0f)
			{
				return StepAggroStrangleAngularSpeed;
			}
			float t = Mathf.Clamp01(coilElapsedSeconds / StepAggroWrapCoilDuration);
			float t2 = Mathf.SmoothStep(0f, 1f, t);
			return Mathf.Lerp(StepAggroWrapAngularSpeed, StepAggroStrangleAngularSpeed, t2);
		}

		public virtual void Initialize()
		{
			SetMoveSpeed(_moveSpeed);
			TargetSearchRange = _targetSearchRange;
			CompletePointMinDistance = _completePointMinDistance;
			DistanceToAttack = _distanceToAttack;
			StepAggroStandDistance = _stepAggroStandDistance;
			StepAggroLoseDistance = _stepAggroLoseDistance;
			StepAggroWrapOrbitRadius = _stepAggroWrapOrbitRadius;
			StepAggroWrapAngularSpeed = _stepAggroWrapAngularSpeed;
			StepAggroWrapMoveSpeed = _stepAggroWrapMoveSpeed;
			StepAggroWrapCoilDuration = _stepAggroWrapCoilDuration;
			StepAggroStrangleAngularSpeed = _stepAggroStrangleAngularSpeed;
			WrapOrbitPlayerPivotHeight = _wrapOrbitPlayerPivotHeight;
			WrapMaxVerticalReach = _wrapMaxVerticalReach;
			WrapEscapeCooldownDuration = _wrapEscapeCooldownDuration;
			WrapEscapeCooldownRemaining = 0f;
			WrapStruggleDrainPerSecond = _wrapStruggleDrainPerSecond;
			WrapStruggleBoostPerPress = _wrapStruggleBoostPerPress;
			WrapStruggleStartNormalized = _wrapStruggleStartNormalized;
			WrapStrangleDamage = _wrapStrangleDamage;
			WrapStrangleDamageInterval = _wrapStrangleDamageInterval;
			WrapFinishDamage = _wrapFinishDamage;
			FearFleeRadius = Mathf.Max(1f, _fearFleeRadius);
			FearVisibilityDistance = Mathf.Max(1f, _fearVisibilityDistance);
			FearDestinationUpdateInterval = Mathf.Max(0f, _fearDestinationUpdateInterval);
			FearDestinationReachedDistance = Mathf.Max(0.1f, _fearDestinationReachedDistance);
			FearDestinationTimer = 0f;
			SafeZoneWaitDuration = Mathf.Max(0f, _safeZoneWaitDuration);
			SafeZoneApproachVisitCount = Mathf.Max(1, _safeZoneApproachVisitCount);
			SafeZoneApproachReachedDistance = Mathf.Max(0.1f, _safeZoneApproachReachedDistance);
			SafeZoneApproachLookStartDistance = Mathf.Max(SafeZoneApproachReachedDistance, _safeZoneApproachLookStartDistance);
			SafeZoneBodyClearance = ((_safeZoneBodyClearance > 0f) ? _safeZoneBodyClearance : ((SnakeController != null) ? SnakeController.CrawlOverRadius : 1f));
			SafeZoneIgnoredHeadBodySegments = Mathf.Max(0, _safeZoneIgnoredHeadBodySegments);
			TimeToAttack = _attackCooldownTime;
			AttackCooldown = 0f;
		}

		public bool IsPriorityPlayerInSafeZone()
		{
			if (!TryGetPriorityPlayerSafeZoneProbePosition(out var playerPosition))
			{
				return false;
			}
			return IsPositionInSafeZone(playerPosition);
		}

		public bool IsPositionInSafeZone(Vector3 position)
		{
			if (SafeZoneDetector != null)
			{
				return SafeZoneDetector.IsPlayerInSafeZone(position);
			}
			return false;
		}

		public bool IsPlayerDataInSafeZone(PlayerDataHolder playerData)
		{
			if (playerData?.NetworkObject == null)
			{
				return false;
			}
			if (!TryGetSafeZoneProbePosition(playerData, out var playerPosition))
			{
				return false;
			}
			return IsPositionInSafeZone(playerPosition);
		}

		public bool TryPrepareSafeZoneApproachPosition()
		{
			return TryPrepareSafeZoneApproachPosition(excludePrevious: false);
		}

		public bool TryPrepareNextSafeZoneApproachPosition()
		{
			return TryPrepareSafeZoneApproachPosition(excludePrevious: true);
		}

		private bool TryPrepareSafeZoneApproachPosition(bool excludePrevious)
		{
			Vector3 safeZoneApproachPosition = SafeZoneApproachPosition;
			bool hasSafeZoneApproachPosition = HasSafeZoneApproachPosition;
			ReleaseSafeZoneApproachPoint();
			if (SafeZoneDetector == null)
			{
				return false;
			}
			if (!TryGetPriorityPlayerSafeZoneProbePosition(out var playerPosition))
			{
				return false;
			}
			if (!SafeZoneDetector.TryFindBlockingSafeZoneUnderPlayer(playerPosition, out var safeZone))
			{
				return false;
			}
			Vector3? excludedPosition = ((excludePrevious && hasSafeZoneApproachPosition) ? new Vector3?(safeZoneApproachPosition) : ((Vector3?)null));
			if (!TryOccupyFarthestFromBodySafeZoneApproachPoint(safeZone, excludedPosition, out var approachPosition))
			{
				return false;
			}
			SafeZoneApproachPosition = approachPosition;
			HasSafeZoneApproachPosition = true;
			return true;
		}

		public void ReleaseSafeZoneApproachPoint()
		{
			_safeZoneInteractionPointsModel?.ReleaseOccupiedByOwner(GetSafeZoneApproachOwnerType(), GetSafeZoneApproachOwnerInstanceId());
			SafeZoneApproachPosition = Vector3.zero;
			HasSafeZoneApproachPosition = false;
		}

		public void TriggerSafeZoneWaitAnimation()
		{
			TriggerAttackAnimation();
		}

		public void TriggerAttackAnimation()
		{
			if (!(AnimationController == null))
			{
				AnimationController.SetTrigger(GetAttackTriggerName());
			}
		}

		public void TriggerAttackAnimationLocal()
		{
			if (!(AnimationController == null))
			{
				AnimationController.SetTriggerLocal(GetAttackTriggerName());
			}
		}

		private string GetAttackTriggerName()
		{
			if (!string.IsNullOrEmpty(_attackTriggerName))
			{
				return _attackTriggerName;
			}
			return "Attack";
		}

		public bool TryGetPriorityPlayerLookPosition(out Vector3 playerPosition)
		{
			return TryGetPriorityPlayerSafeZoneProbePosition(out playerPosition);
		}

		private bool TryGetPriorityPlayerSafeZoneProbePosition(out Vector3 playerPosition)
		{
			playerPosition = Vector3.zero;
			if (PriorityPlayer == null)
			{
				return false;
			}
			return TryGetSafeZoneProbePosition(PriorityPlayer, out playerPosition);
		}

		private bool TryGetSafeZoneProbePosition(PlayerDataHolder playerData, out Vector3 playerPosition)
		{
			playerPosition = Vector3.zero;
			if (playerData?.NetworkObject == null)
			{
				return false;
			}
			PlayerRef playerRef = playerData.NetworkObject.InputAuthority;
			if (playerRef == PlayerRef.None)
			{
				playerRef = playerData.NetworkObject.StateAuthority;
			}
			if (TryGetCharacterMovable(playerRef, out var movable) && movable.CameraPositionTransform != null)
			{
				playerPosition = movable.CameraPositionTransform.position;
				return true;
			}
			if (_navigationService != null && playerRef != PlayerRef.None && _navigationService.TryGetPlayerTrackingPosition(playerRef, out playerPosition))
			{
				return true;
			}
			playerPosition = playerData.NetworkObject.transform.position;
			return true;
		}

		private bool TryGetCharacterMovable(PlayerRef playerRef, out PlayerCharacterMovableBase movable)
		{
			movable = null;
			if (_playerMovableModel == null || playerRef == PlayerRef.None)
			{
				return false;
			}
			if (_playerMovableModel.AllCharacterMovables.TryGetValue(playerRef, out movable) && movable != null)
			{
				return true;
			}
			return false;
		}

		private bool TryOccupyFarthestFromBodySafeZoneApproachPoint(PlayerSafeZone safeZone, Vector3? excludedPosition, out Vector3 approachPosition)
		{
			approachPosition = Vector3.zero;
			if (safeZone == null || safeZone.InteractionPoints == null || safeZone.InteractionPoints.Count == 0)
			{
				return false;
			}
			NavMeshAgent navMeshAgent = NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				return false;
			}
			int safeZoneApproachOwnerType = GetSafeZoneApproachOwnerType();
			int safeZoneApproachOwnerInstanceId = GetSafeZoneApproachOwnerInstanceId();
			Transform transform = null;
			float num = float.NegativeInfinity;
			float safeZoneApproachReachedDistance = SafeZoneApproachReachedDistance;
			for (int i = 0; i < safeZone.InteractionPoints.Count; i++)
			{
				Transform transform2 = safeZone.InteractionPoints[i];
				if (transform2 == null || (_safeZoneInteractionPointsModel != null && _safeZoneInteractionPointsModel.IsOccupiedByAnother(transform2, safeZoneApproachOwnerType, safeZoneApproachOwnerInstanceId)))
				{
					continue;
				}
				Vector3 position = transform2.position;
				if ((!excludedPosition.HasValue || !(HorizontalDistance(position, excludedPosition.Value) <= safeZoneApproachReachedDistance)) && HasCompletePath(navMeshAgent, position))
				{
					float minDistanceToBodyXZ = GetMinDistanceToBodyXZ(position);
					if (!(minDistanceToBodyXZ < SafeZoneBodyClearance) && !(minDistanceToBodyXZ <= num))
					{
						num = minDistanceToBodyXZ;
						transform = transform2;
					}
				}
			}
			if (transform == null)
			{
				return false;
			}
			if (_safeZoneInteractionPointsModel != null && !_safeZoneInteractionPointsModel.TryOccupy(transform, safeZoneApproachOwnerType, safeZoneApproachOwnerInstanceId))
			{
				return false;
			}
			approachPosition = transform.position;
			return true;
		}

		private static float HorizontalDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		private float GetMinDistanceToBodyXZ(Vector3 pointPosition)
		{
			if (SnakeController == null)
			{
				return float.PositiveInfinity;
			}
			return SnakeController.GetMinDistanceToBodyXZ(pointPosition, SafeZoneIgnoredHeadBodySegments);
		}

		private static bool HasCompletePath(NavMeshAgent agent, Vector3 destination)
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!agent.CalculatePath(destination, navMeshPath))
			{
				return false;
			}
			return navMeshPath.status == NavMeshPathStatus.PathComplete;
		}

		private int GetSafeZoneApproachOwnerType()
		{
			return 19;
		}

		private int GetSafeZoneApproachOwnerInstanceId()
		{
			return base.gameObject.GetInstanceID();
		}

		public bool IsEnemyVisibleByPlayers()
		{
			if (_multiplayerModel?.NetworkRunner == null)
			{
				return false;
			}
			Transform targetTransform = ((LookTargetTransform != null) ? LookTargetTransform : base.transform);
			float fearVisibilityDistance = FearVisibilityDistance;
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (_spawnedPlayersModel == null || !_spawnedPlayersModel.Players.TryGetValue(activePlayer, out var value))
				{
					continue;
				}
				NetworkObject networkObject = value.NetworkObject;
				if (!(networkObject == null) && networkObject.TryGetComponent<PlayerLookDetection>(out var component))
				{
					if (component.IsLookingAtObject(targetTransform, angleCullEnabled: false, fearVisibilityDistance / 2f))
					{
						return true;
					}
					if (_navigationService.IsPlayerOnReachablePoint(activePlayer, NavMeshAgent, fearVisibilityDistance, out var reachableData))
					{
						return true;
					}
					if (Vector3.Distance(base.transform.position, reachableData.NavMeshProjectedHit.position) < fearVisibilityDistance / 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void BeginWrapEscapeCooldown()
		{
			WrapEscapeCooldownRemaining = WrapEscapeCooldownDuration;
		}

		public void ClearWrapEscapeCooldown()
		{
			WrapEscapeCooldownRemaining = 0f;
		}

		public void TickWrapEscapeCooldown(float deltaTime)
		{
			if (!(WrapEscapeCooldownRemaining <= 0f))
			{
				WrapEscapeCooldownRemaining = Mathf.Max(0f, WrapEscapeCooldownRemaining - deltaTime);
			}
		}

		public void SetSmoothedVelocity(float value)
		{
			short num = (short)Mathf.Clamp(Mathf.RoundToInt(value * 10f), -32768, 32767);
			if (SmoothedVelocityQuantized != num)
			{
				SmoothedVelocityQuantized = num;
			}
		}

		public void SetMoveSpeed(float value)
		{
			if (!(Mathf.Abs(MoveSpeed - value) < 0.01f))
			{
				MoveSpeed = value;
			}
		}

		public void ApplyBaseMoveSpeed()
		{
			SetMoveSpeed(_moveSpeed);
		}

		public void ApplyStepAggroMoveSpeed()
		{
			SetMoveSpeed(_stepAggroMoveSpeed);
		}

		public void ApplyWrapMoveSpeed()
		{
			SetMoveSpeed(_stepAggroWrapMoveSpeed);
		}

		public void ApplyChaseSlitherFrequency()
		{
			SnakeVisualSlither?.SetFrequencyOverride(_chaseSlitherFrequency);
		}

		public void ClearChaseSlitherFrequency()
		{
			SnakeVisualSlither?.ClearFrequencyOverride();
		}

		public void ApplyChaseVisualRotationSpeed()
		{
			SnakeController?.SetVisualRotationSpeedOverride(_chaseVisualRotationSpeed);
		}

		public void ClearChaseVisualRotationSpeed()
		{
			SnakeController?.ClearVisualRotationSpeedOverride();
		}

		public void SetIsCrouching(bool value)
		{
			if (IsCrouching != value)
			{
				IsCrouching = value;
			}
		}

		public void SetVisualState(SnakeVisualState value)
		{
			if (VisualState != value)
			{
				VisualState = value;
			}
		}

		public void SetWrapOrbitState(bool isActive, float angleRadians, float height, int targetPlayerId, Vector3 orbitCenter)
		{
			IsWrapOrbitActive = isActive;
			WrapOrbitAngleRadians = angleRadians;
			WrapOrbitHeight = height;
			WrapTargetPlayerId = targetPlayerId;
			WrapOrbitCenter = orbitCenter;
			if (!isActive)
			{
				IsWrapStrangling = false;
			}
		}

		public void SetWrapOrbitAngle(float angleRadians)
		{
			WrapOrbitAngleRadians = angleRadians;
		}

		public void SetWrapOrbitCenter(Vector3 orbitCenter)
		{
			if (!((WrapOrbitCenter - orbitCenter).sqrMagnitude < 0.0001f))
			{
				WrapOrbitCenter = orbitCenter;
			}
		}

		public void SetWrapStrangling(bool isStrangling)
		{
			if (!(IsWrapStrangling == isStrangling))
			{
				IsWrapStrangling = isStrangling;
			}
		}

		public void SetTargetPosition(Vector3 position)
		{
			TargetPosition = position;
		}

		public void SetTargetPositionCompleted(bool isCompleted)
		{
			TargetPositionCompleted = isCompleted;
		}

		public void SetCurrentAreaType(int areaType)
		{
			CurrentAreaType = areaType;
		}

		public void SetDetectedPlayers(List<PlayerDataHolder> players)
		{
			DetectedPlayers = players;
			this.OnDetectedPlayersChanged?.Invoke();
		}

		public void SetVisiblePlayers(List<PlayerDataHolder> players)
		{
			VisiblePlayers = players;
			this.OnVisiblePlayersChanged?.Invoke();
		}

		public void PinPriorityPlayer(PlayerDataHolder player, float durationSeconds)
		{
			if (player != null)
			{
				_pinnedPriorityPlayer = player;
				PriorityPinRemaining = Mathf.Max(0f, durationSeconds);
				SetPriorityPlayer(player);
			}
		}

		public void ClearPinnedPriorityPlayer()
		{
			_pinnedPriorityPlayer = null;
			PriorityPinRemaining = 0f;
		}

		public void TickPriorityPin(float deltaTime)
		{
			if (!(PriorityPinRemaining <= 0f))
			{
				PriorityPinRemaining = Mathf.Max(0f, PriorityPinRemaining - deltaTime);
				if (PriorityPinRemaining <= 0f)
				{
					_pinnedPriorityPlayer = null;
				}
			}
		}

		public void SetPriorityPlayer(PlayerDataHolder priorityPlayer)
		{
			if ((!IsPriorityPinned || priorityPlayer == _pinnedPriorityPlayer) && priorityPlayer != PriorityPlayer)
			{
				PriorityPlayer = priorityPlayer;
				this.OnPriorityPlayerChanged?.Invoke();
			}
		}

		public void SetDetectedPlayerDistance(PlayerDataHolder player, float distance)
		{
			DetectedPlayersDistance[player] = distance;
		}

		public void RemoveDetectedPlayerDistance(PlayerDataHolder player)
		{
			DetectedPlayersDistance.Remove(player);
		}

		public void ClearDetectedPlayersDistance()
		{
			DetectedPlayersDistance.Clear();
		}

		public void SetDetectedPlayerFirstSeenTime(PlayerDataHolder player, float time)
		{
			DetectedPlayerFirstDetectedTime[player] = time;
		}

		public void RemoveDetectedPlayerFirstSeenTime(PlayerDataHolder player)
		{
			DetectedPlayerFirstDetectedTime.Remove(player);
		}

		public void ClearDetectedPlayersFirstSeenTime()
		{
			DetectedPlayerFirstDetectedTime.Clear();
		}

		public float GetStatValue(EntityStatType statType, float fallbackValue = 0f)
		{
			if (StatEntity == null)
			{
				return fallbackValue;
			}
			return StatEntity.GetStat(statType)?.Value ?? fallbackValue;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
			MoveSpeed = _MoveSpeed;
			IsCrouching = _IsCrouching;
			VisualState = _VisualState;
			IsWrapOrbitActive = _IsWrapOrbitActive;
			IsWrapStrangling = _IsWrapStrangling;
			WrapOrbitAngleRadians = _WrapOrbitAngleRadians;
			WrapOrbitHeight = _WrapOrbitHeight;
			WrapTargetPlayerId = _WrapTargetPlayerId;
			WrapOrbitCenter = _WrapOrbitCenter;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_VisualState = VisualState;
			_IsWrapOrbitActive = IsWrapOrbitActive;
			_IsWrapStrangling = IsWrapStrangling;
			_WrapOrbitAngleRadians = WrapOrbitAngleRadians;
			_WrapOrbitHeight = WrapOrbitHeight;
			_WrapTargetPlayerId = WrapTargetPlayerId;
			_WrapOrbitCenter = WrapOrbitCenter;
		}
	}
}

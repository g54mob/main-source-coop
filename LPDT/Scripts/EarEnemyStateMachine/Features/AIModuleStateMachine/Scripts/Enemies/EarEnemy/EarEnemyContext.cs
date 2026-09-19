using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Attack;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Flee;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Systems;
using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	[NetworkBehaviourWeaved(4)]
	public class EarEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext, IEnemySoundOcclusionModel
	{
		private const float SMOOTHED_VELOCITY_QUANTIZE_FACTOR = 10f;

		private const float MOVE_SPEED_CHANGE_THRESHOLD = 0.01f;

		[SerializeField]
		private float _moveSpeed = 4f;

		[SerializeField]
		private float _smoothedVelocityLerpSpeed = 5f;

		[SerializeField]
		private float _targetSearchRange = 25f;

		[SerializeField]
		private float _completePointMinDistance = 1.5f;

		[SerializeField]
		private float _distanceToAttack = 2f;

		[SerializeField]
		private float _attackCooldownTime = 3f;

		private EarSoundDestinationResolver _soundDestinationResolver;

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
		private EarVisualState _VisualState;

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public MoveSystem MoveSystem { get; private set; }

		[field: SerializeField]
		public RotateTowardsDirectionSystem RotateTowardsDirectionSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedSystem TargetPositionCompletedSystem { get; private set; }

		[field: SerializeField]
		public AreaTypeTrackSystem AreaTypeTrackSystem { get; private set; }

		[field: SerializeField]
		public FindRandomPositionSystem FindRandomPositionSystem { get; private set; }

		[field: SerializeField]
		public FindRandomSafePositionSystem FindRandomSafePositionSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedResetSystem TargetPositionCompletedResetSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionSystem FindTargetPlayerPositionSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionResetSystem FindTargetPlayerPositionResetSystem { get; private set; }

		[field: SerializeField]
		public PlayerDetectingSystem PlayerDetectingSystem { get; private set; }

		[field: SerializeField]
		public EnemyDetectionAnalyticsSystem EnemyDetectionAnalyticsSystem { get; private set; }

		[field: SerializeField]
		public TargetPlayerPrioritizeSystem TargetPlayerPrioritizeSystem { get; private set; }

		[field: SerializeField]
		public DetectedPlayersTimeSystem DetectedPlayersTimeSystem { get; private set; }

		[field: SerializeField]
		public AttackCooldownSystem AttackCooldownSystem { get; private set; }

		[field: SerializeField]
		public StateDurationTimeSystem StateDurationTimeSystem { get; private set; }

		[field: SerializeField]
		public TargetAttackSystem TargetAttackSystem { get; private set; }

		[field: SerializeField]
		public SoundOcclusionMonoSystem SoundOcclusionMonoSystem { get; private set; }

		[field: SerializeField]
		public EarSoundOcclusionHearingStrengthSetupSystem EarSoundOcclusionHearingStrengthSetupSystem { get; private set; }

		[field: SerializeField]
		public EarWanderingSoundAggroSystem EarWanderingSoundAggroSystem { get; private set; }

		[field: SerializeField]
		public EarAggroSoundAggroSystem EarAggroSoundAggroSystem { get; private set; }

		[field: SerializeField]
		public EarDamageReactionSystem EarDamageReactionSystem { get; private set; }

		[field: SerializeField]
		public EarAttackSystem EarAttackSystem { get; private set; }

		[field: SerializeField]
		public EnemyFearFleeSystem EnemyFearFleeSystem { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EarEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EarEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing EarEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EarEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing EarEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EarEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe EarVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EarEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (EarVisualState)Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EarEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = (int)value;
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

		public Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; } = new Dictionary<PlayerDataHolder, float>();

		public Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; } = new Dictionary<PlayerDataHolder, float>();

		public List<PlayerDataHolder> DetectedPlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<PlayerDataHolder> VisiblePlayers { get; private set; } = new List<PlayerDataHolder>();

		public PlayerDataHolder PriorityPlayer { get; private set; }

		public float SmoothedVelocity => (float)SmoothedVelocityQuantized / 10f;

		public float SmoothedVelocityLerpSpeed => _smoothedVelocityLerpSpeed;

		public float HearingStrength { get; set; } = 1f;

		public HeardSound TargetHeardSound { get; set; }

		public Vector3 SoundTargetPosition => TargetHeardSound.Position;

		public float SoundTargetLoudness => TargetHeardSound.Loudness;

		public int SoundTargetSourceId => TargetHeardSound.SourceId;

		public int SoundTargetPlayerId => TargetHeardSound.PlayerId;

		public event Action OnDetectedPlayersChanged;

		public event Action OnVisiblePlayersChanged;

		public event Action OnPriorityPlayerChanged;

		public event Action<HeardSound> OnEnemyTriggeredBySound;

		[Inject]
		public void InjectDependencies(EarSoundDestinationResolver soundDestinationResolver)
		{
			_soundDestinationResolver = soundDestinationResolver;
		}

		public virtual void Initialize()
		{
			SetMoveSpeed(_moveSpeed);
			TargetSearchRange = _targetSearchRange;
			CompletePointMinDistance = _completePointMinDistance;
			DistanceToAttack = _distanceToAttack;
			TimeToAttack = _attackCooldownTime;
			AttackCooldown = 0f;
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

		public void SetIsCrouching(bool value)
		{
			if (IsCrouching != value)
			{
				IsCrouching = value;
			}
		}

		public void SetVisualState(EarVisualState value)
		{
			if (VisualState != value)
			{
				VisualState = value;
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

		public void StopAgentAndResetVelocity()
		{
			NavMeshAgent.isStopped = true;
			SetSmoothedVelocity(0f);
		}

		public void SetDestinationToReachablePoint(Vector3 desired)
		{
			SetTargetPosition(ResolveReachablePoint(desired));
			SetTargetPositionCompleted(isCompleted: false);
		}

		private Vector3 ResolveReachablePoint(Vector3 desired)
		{
			if (_soundDestinationResolver != null && _soundDestinationResolver.TryResolve(NavMeshAgent, desired, out var resolved))
			{
				return resolved;
			}
			return desired;
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

		public void SetPriorityPlayer(PlayerDataHolder priorityPlayer)
		{
			if (priorityPlayer != PriorityPlayer)
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

		public void TriggerEnemyBySound(HeardSound heardSound)
		{
			this.OnEnemyTriggeredBySound?.Invoke(heardSound);
		}

		public void SetSoundTarget(HeardSound heardSound)
		{
			TargetHeardSound = heardSound;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
			MoveSpeed = _MoveSpeed;
			IsCrouching = _IsCrouching;
			VisualState = _VisualState;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_VisualState = VisualState;
		}
	}
}

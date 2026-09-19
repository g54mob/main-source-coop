using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Attack;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Systems;
using Features.AnimationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	[NetworkBehaviourWeaved(14)]
	public class RatsHoleEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
	{
		private const float SMOOTHED_VELOCITY_QUANTIZE_FACTOR = 10f;

		private const float MOVE_SPEED_CHANGE_THRESHOLD = 0.01f;

		[SerializeField]
		private float _smoothedVelocityLerpSpeed = 5f;

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
		private RatsHoleEnemyVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsAttackPerforming", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsAttackPerforming;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("AttackTriggerCount", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _AttackTriggerCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HomePosition", 6, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _HomePosition;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasHomePosition", 9, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasHomePosition;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HomeAbsorbWorldPosition", 10, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _HomeAbsorbWorldPosition;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasHomeAbsorbWorldPosition", 13, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasHomeAbsorbWorldPosition;

		private RatsHoleEnemyAttackSettings _attackSettings;

		private RatsHoleEnemyMovementSettings _movementSettings;

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
		public RatsHoleEnemyMoveSpeedSetupSystem MoveSpeedSetupSystem { get; private set; }

		[field: SerializeField]
		public EnemySafeZoneAttackDetector SafeZoneAttackDetector { get; private set; }

		[field: SerializeField]
		public RatsHoleEnemySafeZoneBlocker SafeZoneBlocker { get; private set; }

		[field: SerializeField]
		public DamageableAnimationFunctionReactor AttackReactor { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe RatsHoleEnemyVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (RatsHoleEnemyVisualState)Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe bool IsAttackPerforming
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.IsAttackPerforming. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 4);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.IsAttackPerforming. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe int AttackTriggerCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.AttackTriggerCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[5];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.AttackTriggerCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[5] = value;
			}
		}

		public int CurrentAreaType { get; private set; }

		public Vector3 TargetPosition { get; private set; }

		public Vector3 HoleAbsorbWorldPosition { get; private set; }

		public bool TargetPositionCompleted { get; private set; }

		public bool HasHoleAbsorbWorldPosition { get; private set; }

		public bool NeedToFindTargetPosition { get; set; }

		public float CompletePointMinDistance { get; set; }

		public float TargetSearchRange { get; set; }

		public float AvailablePointRange { get; set; } = 0.5f;

		public float AggroRange { get; private set; }

		public float DespawnPlayerCheckRadius { get; private set; }

		public float TimeToAttack { get; set; }

		public float DistanceToAttack { get; set; }

		public float AttackCooldown { get; set; }

		public bool IsAttackOnCooldown { get; set; }

		public float CurrentStateTime { get; set; }

		public float RunVelocityThreshold { get; private set; }

		public float NextAttackAllowedTime { get; set; }

		[Networked]
		[NetworkedWeaved(6, 3)]
		public unsafe Vector3 HomePosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 6);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 6) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(9, 1)]
		public unsafe NetworkBool HasHomePosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HasHomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 9);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HasHomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 9) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(10, 3)]
		public unsafe Vector3 HomeAbsorbWorldPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HomeAbsorbWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 10);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HomeAbsorbWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 10) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(13, 1)]
		public unsafe NetworkBool HasHomeAbsorbWorldPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HasHomeAbsorbWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 13);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHoleEnemyContext.HasHomeAbsorbWorldPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 13) = value;
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
		public void InjectDependencies(RatsHoleEnemyAttackSettings attackSettings, RatsHoleEnemyMovementSettings movementSettings)
		{
			_attackSettings = attackSettings;
			_movementSettings = movementSettings;
		}

		public virtual void Initialize()
		{
			if (_attackSettings == null || _movementSettings == null)
			{
				throw new InvalidOperationException("RatsHoleEnemy settings are not bound in RatsHoleEnemyInstaller.");
			}
			SetMoveSpeed(_movementSettings.DefaultMoveSpeed);
			AggroRange = _movementSettings.AggroRange;
			DespawnPlayerCheckRadius = _movementSettings.DespawnPlayerCheckRadius;
			TargetSearchRange = _movementSettings.AggroRange;
			CompletePointMinDistance = _movementSettings.CompletePointMinDistance;
			RunVelocityThreshold = _movementSettings.RunVelocityThreshold;
			DistanceToAttack = _attackSettings.AttackRange;
			TimeToAttack = _attackSettings.AttackFrequency;
			AttackCooldown = 0f;
			NextAttackAllowedTime = 0f;
		}

		public void SetHomePosition(Vector3 position)
		{
			SetHomePosition(position, position);
		}

		public void SetHomePosition(Vector3 position, Vector3 absorbWorldPosition)
		{
			if (base.HasStateAuthority)
			{
				HomePosition = position;
				HasHomePosition = true;
				HomeAbsorbWorldPosition = absorbWorldPosition;
				HasHomeAbsorbWorldPosition = true;
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

		public void SetIsCrouching(bool value)
		{
			if (IsCrouching != value)
			{
				IsCrouching = value;
			}
		}

		public void SetVisualState(RatsHoleEnemyVisualState value)
		{
			if (VisualState != value)
			{
				VisualState = value;
			}
		}

		public void SetIsAttackPerforming(bool value)
		{
			if (base.HasStateAuthority && IsAttackPerforming != value)
			{
				IsAttackPerforming = value;
			}
		}

		public void RequestAttackPresentation()
		{
			if (base.HasStateAuthority)
			{
				AttackTriggerCount++;
			}
		}

		public void StopMovement()
		{
			if (!(NavMeshAgent == null) && NavMeshAgent.isOnNavMesh)
			{
				NavMeshAgent.isStopped = true;
				NavMeshAgent.ResetPath();
				NavMeshAgent.velocity = Vector3.zero;
			}
		}

		public void ResumeMovement()
		{
			if (!(NavMeshAgent == null) && NavMeshAgent.isOnNavMesh)
			{
				NavMeshAgent.isStopped = false;
			}
		}

		public void SetTargetPosition(Vector3 position)
		{
			TargetPosition = position;
		}

		public void SetHoleAbsorbWorldPosition(Vector3 position)
		{
			HoleAbsorbWorldPosition = position;
			HasHoleAbsorbWorldPosition = true;
		}

		public void ClearHoleAbsorbWorldPosition()
		{
			HoleAbsorbWorldPosition = default(Vector3);
			HasHoleAbsorbWorldPosition = false;
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

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
			MoveSpeed = _MoveSpeed;
			IsCrouching = _IsCrouching;
			VisualState = _VisualState;
			IsAttackPerforming = _IsAttackPerforming;
			AttackTriggerCount = _AttackTriggerCount;
			HomePosition = _HomePosition;
			HasHomePosition = _HasHomePosition;
			HomeAbsorbWorldPosition = _HomeAbsorbWorldPosition;
			HasHomeAbsorbWorldPosition = _HasHomeAbsorbWorldPosition;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_VisualState = VisualState;
			_IsAttackPerforming = IsAttackPerforming;
			_AttackTriggerCount = AttackTriggerCount;
			_HomePosition = HomePosition;
			_HasHomePosition = HasHomePosition;
			_HomeAbsorbWorldPosition = HomeAbsorbWorldPosition;
			_HasHomeAbsorbWorldPosition = HasHomeAbsorbWorldPosition;
		}
	}
}

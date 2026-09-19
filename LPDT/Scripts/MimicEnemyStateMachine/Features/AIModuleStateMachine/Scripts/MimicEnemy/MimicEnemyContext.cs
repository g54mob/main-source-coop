using System;
using System.Collections.Generic;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems;
using Features.AudioServiceModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	[NetworkBehaviourWeaved(6)]
	public class MimicEnemyContext : NetworkBehaviour, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext, IFacingSpeedLimitContext
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
		[DefaultForProperty("IsAggressive", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsAggressive;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsSprinting", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsSprinting;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ReplicateTarget", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ReplicateTarget;

		private PlayerDataHolder _pinnedPriorityPlayer;

		[field: SerializeField]
		public FindInterestPointPositionResetSystem FindInterestPointPositionResetSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionResetSystem FindTargetPlayerPositionResetSystem { get; private set; }

		[field: SerializeField]
		public FindInterestPointPositionSystem FindInterestPointPositionSystem { get; private set; }

		[field: SerializeField]
		public MimicInterestPointsDetectSystem MimicInterestPointsDetectSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionSystem FindTargetPlayerPositionSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedSystem TargetPositionCompletedSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedResetSystem TargetPositionCompletedResetSystem { get; private set; }

		[field: SerializeField]
		public TargetPlayerPrioritizeSystem TargetPlayerPrioritizeSystem { get; private set; }

		[field: SerializeField]
		public RotateTowardsDirectionSystem RotateTowardsDirectionSystem { get; private set; }

		[field: SerializeField]
		public TargetSearchRangeSetupSystem TargetSearchRangeSetupSystem { get; private set; }

		[field: SerializeField]
		public FindRandomSafePositionSystem FindRandomSafePositionSystem { get; private set; }

		[field: SerializeField]
		public CompleteDistanceSetupSystem CompleteDistanceSetupSystem { get; private set; }

		[field: SerializeField]
		public DistanceToAttackSetupSystem DistanceToAttackSetupSystem { get; private set; }

		[field: SerializeField]
		public MimicMoveStatesSetupSystem MimicMoveStatesSetupSystem { get; private set; }

		[field: SerializeField]
		public DetectedPlayersTimeSystem DetectedPlayersTimeSystem { get; private set; }

		[field: SerializeField]
		public MimicEnemyAnimationSystem MimicEnemyAnimationSystem { get; private set; }

		[field: SerializeField]
		public ReadyForAttackTrackSystem ReadyForAttackTrackSystem { get; private set; }

		[field: SerializeField]
		public FindRandomPositionSystem FindRandomPositionSystem { get; private set; }

		[field: SerializeField]
		public StateDurationTimeSystem StateDurationTimeSystem { get; private set; }

		[field: SerializeField]
		public MimicTargetAttackSystem MimicTargetAttackSystem { get; private set; }

		[field: SerializeField]
		public TimeToAttackSetupSystem TimeToAttackSetupSystem { get; private set; }

		[field: SerializeField]
		public MimicFearDestroySystem MimicFearDestroySystem { get; private set; }

		[field: SerializeField]
		public MimicMoveStatesSystem MimicMoveStatesSystem { get; private set; }

		[field: SerializeField]
		public PlayerDetectingSystem PlayerDetectingSystem { get; private set; }

		[field: SerializeField]
		public ReplicateTargetSystem ReplicateTargetSystem { get; private set; }

		[field: SerializeField]
		public AttackCooldownSystem AttackCooldownSystem { get; private set; }

		[field: SerializeField]
		public MimicAnalyticsSystem MimicAnalyticsSystem { get; private set; }

		[field: SerializeField]
		public AreaTypeTrackSystem AreaTypeTrackSystem { get; private set; }

		[field: SerializeField]
		public PlayerVisibleSystem PlayerVisibleSystem { get; private set; }

		[field: SerializeField]
		public MoveSystem MoveSystem { get; private set; }

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: SerializeField]
		public float AttackCooldownTime { get; private set; }

		[field: SerializeField]
		public float BehaviorSimulationTime { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe bool IsAggressive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.IsAggressive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.IsAggressive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe bool IsSprinting
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.IsSprinting. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 4);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.IsSprinting. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe int ReplicateTarget
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.ReplicateTarget. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[5];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemyContext.ReplicateTarget. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[5] = value;
			}
		}

		public bool IsReadyForAttack { get; set; }

		public bool IsAttackInProcess { get; set; }

		public bool NeedReplicateTarget { get; set; }

		public bool NeedFindReplicateTarget { get; set; }

		public int CurrentAreaType { get; private set; }

		public Vector3 TargetPosition { get; private set; }

		public bool TargetPositionCompleted { get; private set; }

		public bool NeedToFindTargetPosition { get; set; }

		public Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; } = new Dictionary<PlayerDataHolder, float>();

		public Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; } = new Dictionary<PlayerDataHolder, float>();

		public List<PlayerDataHolder> DetectedPlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<PlayerDataHolder> VisiblePlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<MimicInterestPoint> MimicInterestPoints { get; private set; } = new List<MimicInterestPoint>();

		public int CurrentTargetInterestPriority { get; set; }

		public PlayerDataHolder PriorityPlayer { get; private set; }

		public float PriorityPinRemaining { get; private set; }

		public float TimeToAttack { get; set; }

		public float CompletePointMinDistance { get; set; }

		public float DistanceToAttack { get; set; }

		public float TargetSearchRange { get; set; }

		public float AvailablePointRange { get; set; } = 0.5f;

		public float AttackCooldown { get; set; }

		public bool IsAttackOnCooldown { get; set; }

		public float CurrentStateTime { get; set; }

		public float WalkSpeed { get; set; }

		public float SprintSpeed { get; set; }

		public float CrouchSpeed { get; set; }

		public bool IsDespawnAfterFear { get; set; }

		public float FacingMoveSpeedMultiplier { get; private set; } = 1f;

		public float FacingMoveAngle { get; private set; }

		public float SmoothedVelocity => (float)SmoothedVelocityQuantized / 10f;

		public bool IsPriorityPinned
		{
			get
			{
				if (PriorityPinRemaining > 0f && _pinnedPriorityPlayer != null && _pinnedPriorityPlayer.NetworkObject != null)
				{
					return _pinnedPriorityPlayer.NetworkObject.IsValid;
				}
				return false;
			}
		}

		public float SmoothedVelocityLerpSpeed => _smoothedVelocityLerpSpeed;

		public AttractionZoneData PendingAttractionZone { get; private set; }

		public event Action OnTargetPositionCompletedChanged;

		public event Action OnDetectedPlayersChanged;

		public event Action OnVisiblePlayersChanged;

		public event Action OnPriorityPlayerChanged;

		public event Action OnAreaTypeChanged;

		public event Action OnMimicInterestPointsChanged;

		public event Action<Vector3> OnTargetChanged;

		public void SetPendingAttractionZone(AttractionZoneData zone)
		{
			PendingAttractionZone = zone;
		}

		public void Initialize()
		{
			AttackCooldown = AttackCooldownTime;
			NeedReplicateTarget = true;
			NeedFindReplicateTarget = true;
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

		public void SetFacingMoveSpeedMultiplier(float multiplier, float angle)
		{
			FacingMoveSpeedMultiplier = Mathf.Clamp01(multiplier);
			FacingMoveAngle = Mathf.Max(0f, angle);
		}

		public void SetIsCrouching(bool value)
		{
			if (IsCrouching != value)
			{
				IsCrouching = value;
			}
		}

		public void SetIsAggressive(bool value)
		{
			if (IsAggressive != value)
			{
				IsAggressive = value;
			}
		}

		public void SetIsSprinting(bool value)
		{
			if (IsSprinting != value)
			{
				IsSprinting = value;
			}
		}

		public void SetReplicateTarget(int value)
		{
			if (ReplicateTarget != value)
			{
				ReplicateTarget = value;
			}
		}

		public void SetTargetPosition(Vector3 position)
		{
			TargetPosition = position;
			this.OnTargetChanged?.Invoke(TargetPosition);
		}

		public void SetMimicInterestPoints(List<MimicInterestPoint> mimicInterestPoints)
		{
			MimicInterestPoints = mimicInterestPoints;
			this.OnMimicInterestPointsChanged?.Invoke();
		}

		public void SetCurrentAreaType(int areaType)
		{
			CurrentAreaType = areaType;
			this.OnAreaTypeChanged?.Invoke();
		}

		public void SetTargetPositionCompleted(bool isCompleted)
		{
			TargetPositionCompleted = isCompleted;
			this.OnTargetPositionCompletedChanged?.Invoke();
		}

		public void SetDetectedPlayers(List<PlayerDataHolder> visiblePlayers)
		{
			DetectedPlayers = visiblePlayers;
			this.OnDetectedPlayersChanged?.Invoke();
		}

		public void SetVisiblePlayers(List<PlayerDataHolder> visiblePlayers)
		{
			VisiblePlayers = visiblePlayers;
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
			IsAggressive = _IsAggressive;
			IsSprinting = _IsSprinting;
			ReplicateTarget = _ReplicateTarget;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_IsAggressive = IsAggressive;
			_IsSprinting = IsSprinting;
			_ReplicateTarget = ReplicateTarget;
		}
	}
}

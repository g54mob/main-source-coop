using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.DeadPartsModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemSpawnerModule;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	[NetworkBehaviourWeaved(8)]
	public class PlayerTutorialGuideEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
	{
		private const float SmoothedVelocityQuantizeFactor = 10f;

		private const float MoveSpeedChangeThreshold = 0.01f;

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
		[DefaultForProperty("AppearanceSeed", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _AppearanceSeed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsDead", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsDead;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsDancing", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsDancing;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BottomPartUsageCount", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BottomPartUsageCount;

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
		public TargetPlayerPrioritizeSystem TargetPlayerPrioritizeSystem { get; private set; }

		[field: SerializeField]
		public DetectedPlayersTimeSystem DetectedPlayersTimeSystem { get; private set; }

		[field: SerializeField]
		public AttackCooldownSystem AttackCooldownSystem { get; private set; }

		[field: SerializeField]
		public StateDurationTimeSystem StateDurationTimeSystem { get; private set; }

		[field: SerializeField]
		public PlayerTutorialGuideReplicateTargetSystem ReplicateTargetSystem { get; private set; }

		[field: SerializeField]
		public PlayerTutorialGuideAnimationSystem PlayerTutorialGuideAnimationSystem { get; private set; }

		[field: SerializeField]
		public EnemyStatHealthController HealthController { get; private set; }

		[field: SerializeField]
		public VisibilityHandlerBase BottomPartVisibilityHandler { get; private set; }

		[field: SerializeField]
		public Transform DownPos { get; private set; }

		[field: SerializeField]
		public Transform UpperPos { get; private set; }

		[field: SerializeField]
		public DeadPartType DeadPartType { get; private set; } = DeadPartType.DefaultButt;

		[field: SerializeField]
		public float ReviveConnectDistance { get; private set; } = 0.6f;

		[field: SerializeField]
		public float ReviveConnectAngle { get; private set; } = 75f;

		[field: SerializeField]
		public float ReviveMinTime { get; private set; } = 2f;

		[field: SerializeField]
		public Transform RightArmTransform { get; private set; }

		[field: SerializeField]
		public ItemSpawnerBase CarryItemSpawner { get; private set; }

		[field: SerializeField]
		public Transform ShootTarget { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsAggressive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsAggressive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe int AppearanceSeed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.AppearanceSeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.AppearanceSeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe bool IsDead
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsDead. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 5);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsDead. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 5) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(6, 1)]
		public unsafe bool IsDancing
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsDancing. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.IsDancing. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 6) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(7, 1)]
		public unsafe int BottomPartUsageCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.BottomPartUsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[7];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerTutorialGuideEnemyContext.BottomPartUsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[7] = value;
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

		public bool NeedGenerateAppearance { get; set; }

		public Vector3 PendingDestination { get; private set; }

		public bool HasPendingDestination { get; private set; }

		public bool CanBeRevived { get; set; }

		public bool HasCarryItem { get; set; } = true;

		public Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; } = new Dictionary<PlayerDataHolder, float>();

		public Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; } = new Dictionary<PlayerDataHolder, float>();

		public List<PlayerDataHolder> DetectedPlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<PlayerDataHolder> VisiblePlayers { get; private set; } = new List<PlayerDataHolder>();

		public PlayerDataHolder PriorityPlayer { get; private set; }

		public PlayerDeadPart LastSpawnedDeadPart { get; set; }

		public SimplePointGrabable CarryItemGrabbable { get; set; }

		public TransformReplicator CarryItemTransformReplicator { get; set; }

		public float SmoothedVelocity => (float)SmoothedVelocityQuantized / 10f;

		public float SmoothedVelocityLerpSpeed => _smoothedVelocityLerpSpeed;

		public event Action OnDetectedPlayersChanged;

		public event Action OnVisiblePlayersChanged;

		public event Action OnPriorityPlayerChanged;

		public event Action<bool> OnIsDeadChanged;

		public void Initialize()
		{
			SetMoveSpeed(_moveSpeed);
			SetIsAggressive(value: false);
			SetIsCrouching(value: false);
			TargetSearchRange = _targetSearchRange;
			CompletePointMinDistance = _completePointMinDistance;
			DistanceToAttack = _distanceToAttack;
			TimeToAttack = _attackCooldownTime;
			AttackCooldown = 0f;
			NeedGenerateAppearance = true;
			HasPendingDestination = false;
			SetAppearanceSeed(0);
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

		public void SetIsAggressive(bool value)
		{
			if (IsAggressive != value)
			{
				IsAggressive = value;
			}
		}

		public void SetAppearanceSeed(int value)
		{
			if (AppearanceSeed != value)
			{
				AppearanceSeed = value;
			}
		}

		public void SetIsDead(bool value)
		{
			if (IsDead != value)
			{
				IsDead = value;
				this.OnIsDeadChanged?.Invoke(value);
			}
		}

		public void SetIsDancing(bool value)
		{
			if (IsDancing != value)
			{
				IsDancing = value;
			}
		}

		public void SetBottomPartUsageCount(int value)
		{
			if (BottomPartUsageCount != value)
			{
				BottomPartUsageCount = value;
			}
		}

		public void SetDestination(Vector3 position)
		{
			PendingDestination = position;
			HasPendingDestination = true;
		}

		public void ConsumeDestination()
		{
			SetTargetPosition(PendingDestination);
			SetTargetPositionCompleted(isCompleted: false);
			NeedToFindTargetPosition = false;
			HasPendingDestination = false;
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

		public void UpdateBottomPartVisibility(bool enabledVis)
		{
			ReplicateTargetSystem.SetBottomSkinVisible(enabledVis);
			if (enabledVis)
			{
				BottomPartVisibilityHandler.EnableRenderObject();
			}
			else
			{
				BottomPartVisibilityHandler.DisableRenderObject();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
			MoveSpeed = _MoveSpeed;
			IsCrouching = _IsCrouching;
			IsAggressive = _IsAggressive;
			AppearanceSeed = _AppearanceSeed;
			IsDead = _IsDead;
			IsDancing = _IsDancing;
			BottomPartUsageCount = _BottomPartUsageCount;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_IsAggressive = IsAggressive;
			_AppearanceSeed = AppearanceSeed;
			_IsDead = IsDead;
			_IsDancing = IsDancing;
			_BottomPartUsageCount = BottomPartUsageCount;
		}
	}
}

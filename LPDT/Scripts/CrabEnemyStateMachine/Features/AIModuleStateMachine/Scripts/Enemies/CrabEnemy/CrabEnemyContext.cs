using System;
using System.Collections.Generic;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Attack;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.ItemsModule.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy
{
	[NetworkBehaviourWeaved(6)]
	public class CrabEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
	{
		private const float SMOOTHED_VELOCITY_QUANTIZE_FACTOR = 10f;

		private const float DIRECTION_QUANTIZE_FACTOR = 100f;

		private const float MOVE_SPEED_CHANGE_THRESHOLD = 0.01f;

		private const float DIRECTION_CHANGE_THRESHOLD = 0.01f;

		private IEnemyAttractionZoneService _attractionZoneService;

		private INavigationService _navigationService;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

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
		[DefaultForProperty("DirectionQuantized", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _DirectionQuantized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("MoveSpeed", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _MoveSpeed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsCrouching", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsCrouching;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private CrabVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ActiveClaw", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private CrabClawSide _ActiveClaw;

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
		public FindRandomSafePositionSystem FindRandomSafePositionSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionSystem FindTargetPlayerPositionSystem { get; private set; }

		[field: SerializeField]
		public FindTargetPlayerPositionResetSystem FindTargetPlayerPositionResetSystem { get; private set; }

		[field: SerializeField]
		public CrabVisionDetectingSystem CrabVisionDetectingSystem { get; private set; }

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
		public PlayerGrabSystem PlayerGrabSystem { get; private set; }

		[field: SerializeField]
		public CrabBurstMovementSystem CrabBurstMovementSystem { get; private set; }

		[field: SerializeField]
		public CrabTurnSpeedSystem CrabTurnSpeedSystem { get; private set; }

		[field: SerializeField]
		public CrabHomeRoamSystem CrabHomeRoamSystem { get; private set; }

		[field: SerializeField]
		public CrabSearchRangeSetupSystem CrabSearchRangeSetupSystem { get; private set; }

		[field: SerializeField]
		public CrabItemCarrySystem CrabItemCarrySystem { get; private set; }

		[field: SerializeField]
		public CrabGrabbedByPlayerTrackSystem CrabGrabbedByPlayerTrackSystem { get; private set; }

		[field: SerializeField]
		public CrabMoveSpeedSetupSystem CrabMoveSpeedSetupSystem { get; private set; }

		[field: SerializeField]
		public CrabDamageAggrSystem CrabDamageAggrSystem { get; private set; }

		[field: SerializeField]
		public CrabClawDetachLootSystem CrabClawDetachLootSystem { get; private set; }

		[field: SerializeField]
		public float GrabCooldown { get; private set; } = 2f;

		[field: SerializeField]
		public float GrabWanderDuration { get; private set; } = 8f;

		[field: SerializeField]
		public float PlayerGrabWanderDuration { get; private set; } = 6f;

		[field: SerializeField]
		public float GrabWanderRadius { get; private set; } = 6f;

		[field: SerializeField]
		public float RestDuration { get; private set; } = 5f;

		[field: SerializeField]
		public float DamageAggroDuration { get; private set; } = 5f;

		[field: SerializeField]
		public float DetachDuration { get; private set; } = 7f;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(short*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe short DirectionQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.DirectionQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((short*)Ptr)[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.DirectionQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe float MoveSpeed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe bool IsCrouching
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe CrabVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (CrabVisualState)Ptr[4];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe CrabClawSide ActiveClaw
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.ActiveClaw. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (CrabClawSide)Ptr[5];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrabEnemyContext.ActiveClaw. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[5] = (int)value;
			}
		}

		public bool DetachCompletedRequested { get; private set; }

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

		public bool IsGrabbedByPlayer { get; set; }

		public IItem CarriedItem { get; set; }

		public IItem LastGrabbedItem { get; set; }

		public float TargetMoveSpeed { get; private set; }

		public Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; } = new Dictionary<PlayerDataHolder, float>();

		public Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; } = new Dictionary<PlayerDataHolder, float>();

		public List<PlayerDataHolder> DetectedPlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<PlayerDataHolder> VisiblePlayers { get; private set; } = new List<PlayerDataHolder>();

		public PlayerDataHolder PriorityPlayer { get; private set; }

		public AttractionZoneData PendingAttractionZone { get; private set; }

		public float SmoothedVelocity => (float)SmoothedVelocityQuantized / 10f;

		public float SmoothedVelocityLerpSpeed => _smoothedVelocityLerpSpeed;

		public float Direction => (float)DirectionQuantized / 100f;

		public event Action OnDetectedPlayersChanged;

		public event Action OnVisiblePlayersChanged;

		public event Action OnPriorityPlayerChanged;

		public void SetTargetMoveSpeed(float value)
		{
			TargetMoveSpeed = value;
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

		[Inject]
		public void InjectDependencies(INavigationService navigationService, IEnemyAttractionZoneService attractionZoneService, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_navigationService = navigationService;
			_attractionZoneService = attractionZoneService;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public bool CanEnemyTargetPlayer(int playerId)
		{
			return _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(playerId);
		}

		public bool CanEnemyTargetPriorityPlayer()
		{
			if (PriorityPlayer == null || PriorityPlayer.NetworkObject == null)
			{
				return false;
			}
			return CanEnemyTargetPlayer(PriorityPlayer.NetworkObject.InputAuthority.PlayerId);
		}

		public void SetPendingAttractionZone(AttractionZoneData zone)
		{
			PendingAttractionZone = zone;
		}

		public bool TryGetAttractionApproachPoint(int attempts, out Vector3 point)
		{
			return _attractionZoneService.TrySampleApproachPoint(_navigationService, PendingAttractionZone.Origin, PendingAttractionZone.ApproachRadius, attempts, out point);
		}

		public void SetSmoothedVelocity(float value)
		{
			short num = (short)Mathf.Clamp(Mathf.RoundToInt(value * 10f), -32768, 32767);
			if (SmoothedVelocityQuantized != num)
			{
				SmoothedVelocityQuantized = num;
			}
		}

		public void SetDirection(float value)
		{
			float num = Mathf.Clamp(value, -1f, 1f);
			if (!(Mathf.Abs(Direction - num) < 0.01f))
			{
				int value2 = Mathf.RoundToInt(num * 100f);
				DirectionQuantized = (short)Mathf.Clamp(value2, -32768, 32767);
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

		public void SetVisualState(CrabVisualState value)
		{
			if (VisualState != value)
			{
				VisualState = value;
			}
		}

		public void SetActiveClaw(CrabClawSide value)
		{
			if (ActiveClaw != value)
			{
				ActiveClaw = value;
			}
		}

		public void RequestDetachCompleted()
		{
			DetachCompletedRequested = true;
		}

		public void ClearDetachCompleted()
		{
			DetachCompletedRequested = false;
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

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
			DirectionQuantized = _DirectionQuantized;
			MoveSpeed = _MoveSpeed;
			IsCrouching = _IsCrouching;
			VisualState = _VisualState;
			ActiveClaw = _ActiveClaw;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_DirectionQuantized = DirectionQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_VisualState = VisualState;
			_ActiveClaw = ActiveClaw;
		}
	}
}

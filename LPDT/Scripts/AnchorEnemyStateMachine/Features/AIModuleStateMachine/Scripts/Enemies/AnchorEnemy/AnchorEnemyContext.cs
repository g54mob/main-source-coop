using System;
using System.Collections.Generic;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.DamageableTrackModule.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[NetworkBehaviourWeaved(5)]
	public class AnchorEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
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
		private float _completePointMinDistance = 2.5f;

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
		[DefaultForProperty("VisualState", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private AnchorVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsAnchorThrown", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsAnchorThrown;

		private INavigationService _navigationService;

		private IEnemyAttractionZoneService _attractionZoneService;

		private IEnemyTrackingService _enemyTrackingService;

		private IEnemyTrackable _auxTarget;

		private IDamageable _auxTargetDamageable;

		private float _auxTargetSearchTimer;

		private static int _auxSightMask = -1;

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
		public AnchorDamageReactionSystem AnchorDamageReactionSystem { get; private set; }

		[field: SerializeField]
		public AnchorMoveSpeedSetupSystem AnchorMoveSpeedSetupSystem { get; private set; }

		[field: SerializeField]
		public AnchorFaceTargetSystem AnchorFaceTargetSystem { get; private set; }

		[field: SerializeField]
		public AnchorMeleeAttackSystem AnchorMeleeAttackSystem { get; private set; }

		[field: SerializeField]
		public AnchorMeleeDecisionSystem AnchorMeleeDecisionSystem { get; private set; }

		[field: SerializeField]
		public AnchorAreaRoamSystem AnchorAreaRoamSystem { get; private set; }

		[field: SerializeField]
		public AnchorAreaRelocateSystem AnchorAreaRelocateSystem { get; private set; }

		[field: SerializeField]
		public AnchorVisionDetectingSystem AnchorVisionDetectingSystem { get; private set; }

		[field: SerializeField]
		public AnchorThrowDecisionSystem AnchorThrowDecisionSystem { get; private set; }

		[field: SerializeField]
		public AnchorPlayerGrabSystem AnchorPlayerGrabSystem { get; private set; }

		[field: SerializeField]
		public AnchorHookControlSystem AnchorHookControlSystem { get; private set; }

		[field: SerializeField]
		public float AreaRoamRadius { get; private set; } = 8f;

		[field: SerializeField]
		public float AreaRelocateRadius { get; private set; } = 8f;

		[field: SerializeField]
		public float ChangeAreaFrequency { get; private set; } = 30f;

		[field: SerializeField]
		public float MinHookDistance { get; private set; } = 5f;

		[field: SerializeField]
		public float RecommendedHookDistance { get; private set; } = 8f;

		[field: SerializeField]
		public float MaxHookDistance { get; private set; } = 12f;

		[field: SerializeField]
		public float ThrowAimHeightOffset { get; private set; }

		[field: SerializeField]
		public float MeleeRange { get; private set; } = 2f;

		[field: SerializeField]
		public float MeleeHitRange { get; private set; } = 3f;

		[field: SerializeField]
		public float MeleeCooldownTime { get; private set; } = 2f;

		[field: SerializeField]
		public float ThrowWindupDuration { get; private set; } = 1f;

		[field: SerializeField]
		public float ThrowMissTimeout { get; private set; } = 4f;

		[field: SerializeField]
		public float ThrowCooldown { get; private set; } = 3f;

		[field: SerializeField]
		public float ThrowFlightTime { get; private set; } = 0.25f;

		[field: SerializeField]
		public float ThrowSpeedMultiplier { get; private set; } = 1.75f;

		[field: SerializeField]
		public float ReelNearSpeed { get; private set; } = 28f;

		[field: SerializeField]
		public float ReelFarSpeed { get; private set; } = 6f;

		[field: SerializeField]
		public float ReelPlayerNearSpeed { get; private set; } = 18f;

		[field: SerializeField]
		public float ReelPlayerFarSpeed { get; private set; } = 4f;

		[field: SerializeField]
		public float ReelArriveDistance { get; private set; } = 2f;

		[field: SerializeField]
		public float ReelEmptyTimeout { get; private set; } = 8f;

		[field: SerializeField]
		public float ReelPlayerTimeout { get; private set; } = 10f;

		[field: SerializeField]
		public float HookReleaseDistance { get; private set; } = 3f;

		[field: SerializeField]
		public float HandGrabRadius { get; private set; } = 4f;

		[field: SerializeField]
		public float HoldPlayerDuration { get; private set; } = 4f;

		[field: SerializeField]
		public float ReleaseDuration { get; private set; } = 5f;

		[field: SerializeField]
		public float ThrowDamage { get; private set; } = 10f;

		[field: SerializeField]
		public float ThrowForce { get; private set; } = 15f;

		[field: SerializeField]
		public float ThrowUpBias { get; private set; } = 0.35f;

		[field: SerializeField]
		public StunDurationPreset ThrowStunDurationPreset { get; private set; } = StunDurationPreset.Short;

		[field: SerializeField]
		public float DamageAggroDuration { get; private set; } = 5f;

		[field: SerializeField]
		public float FearDespawnRadius { get; private set; } = 25f;

		[field: SerializeField]
		public bool IsAuxTargetingEnabled { get; private set; } = true;

		[field: SerializeField]
		public float AuxTargetDetectionRadius { get; private set; } = 12f;

		[field: SerializeField]
		public float AuxTargetSearchInterval { get; private set; } = 0.3f;

		[field: SerializeField]
		public float AuxTargetAimHeight { get; private set; } = 1f;

		[field: SerializeField]
		public float AuxTargetHardDetectDistance { get; private set; } = 3f;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe AnchorVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (AnchorVisualState)Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe bool IsAnchorThrown
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.IsAnchorThrown. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 4);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorEnemyContext.IsAnchorThrown. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = new NetworkBool(value);
			}
		}

		public int CurrentAreaType { get; private set; }

		public Vector3 AreaPosition { get; private set; }

		public bool HasAreaPosition { get; private set; }

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

		public float MeleeCooldown { get; set; }

		public bool ThrowReleaseRequested { get; private set; }

		public bool PlayerThrowReleaseRequested { get; private set; }

		public bool MeleeHitRequested { get; private set; }

		public Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; } = new Dictionary<PlayerDataHolder, float>();

		public Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; } = new Dictionary<PlayerDataHolder, float>();

		public List<PlayerDataHolder> DetectedPlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<PlayerDataHolder> VisiblePlayers { get; private set; } = new List<PlayerDataHolder>();

		public PlayerDataHolder PriorityPlayer { get; private set; }

		public AttractionZoneData PendingAttractionZone { get; private set; }

		public float SmoothedVelocity => (float)SmoothedVelocityQuantized / 10f;

		public float SmoothedVelocityLerpSpeed => _smoothedVelocityLerpSpeed;

		private static int AuxSightMask
		{
			get
			{
				if (_auxSightMask < 0)
				{
					_auxSightMask = LayerMask.GetMask("Default", "Ground", "Floor", "Wall");
				}
				return _auxSightMask;
			}
		}

		public bool HasAuxTarget
		{
			get
			{
				PruneDeadAuxTarget();
				if (_auxTarget != null)
				{
					return _auxTarget.IsTrackable;
				}
				return false;
			}
		}

		public Transform AuxTargetTransform
		{
			get
			{
				if (!HasAuxTarget)
				{
					return null;
				}
				return _auxTarget.Transform;
			}
		}

		public event Action OnDetectedPlayersChanged;

		public event Action OnVisiblePlayersChanged;

		public event Action OnPriorityPlayerChanged;

		public void RequestThrowRelease()
		{
			ThrowReleaseRequested = true;
		}

		public void ClearThrowRelease()
		{
			ThrowReleaseRequested = false;
		}

		public void RequestPlayerThrowRelease()
		{
			PlayerThrowReleaseRequested = true;
		}

		public void ClearPlayerThrowRelease()
		{
			PlayerThrowReleaseRequested = false;
		}

		public void RequestMeleeHit()
		{
			MeleeHitRequested = true;
		}

		public void ClearMeleeHit()
		{
			MeleeHitRequested = false;
		}

		[Inject]
		public void InjectDependencies(INavigationService navigationService, IEnemyAttractionZoneService attractionZoneService, IEnemyTrackingService enemyTrackingService)
		{
			_navigationService = navigationService;
			_attractionZoneService = attractionZoneService;
			_enemyTrackingService = enemyTrackingService;
		}

		public bool TryAcquireAuxTarget(float deltaTime)
		{
			if (!IsAuxTargetingEnabled || HasLivePriorityPlayer())
			{
				return false;
			}
			_auxTargetSearchTimer -= deltaTime;
			if (_auxTargetSearchTimer > 0f)
			{
				return false;
			}
			_auxTargetSearchTimer = AuxTargetSearchInterval;
			if (!_enemyTrackingService.TryGetNearestTarget(base.transform.position, AuxTargetDetectionRadius, out var target))
			{
				return false;
			}
			if (!IsAuxTargetVisible(target))
			{
				return false;
			}
			SetAuxTarget(target);
			return true;
		}

		public void SetAuxTarget(IEnemyTrackable auxTarget)
		{
			_auxTarget = auxTarget;
			_auxTargetDamageable = null;
			if (auxTarget != null && auxTarget.Transform != null)
			{
				auxTarget.Transform.TryGetComponent<IDamageable>(out _auxTargetDamageable);
			}
		}

		public void ClearAuxTarget()
		{
			_auxTarget = null;
			_auxTargetDamageable = null;
		}

		public bool TryGetAuxTargetDamageable(out IDamageable damageable)
		{
			damageable = _auxTargetDamageable;
			if (HasAuxTarget)
			{
				return damageable != null;
			}
			return false;
		}

		public bool TryGetAttackTargetPosition(out Vector3 position)
		{
			if (HasLivePriorityPlayer())
			{
				position = PriorityPlayer.NetworkObject.transform.position;
				return true;
			}
			if (HasAuxTarget)
			{
				position = _auxTarget.Transform.position;
				return true;
			}
			position = default(Vector3);
			return false;
		}

		private void PruneDeadAuxTarget()
		{
			if (_auxTarget != null && _auxTarget.Transform == null)
			{
				ClearAuxTarget();
			}
		}

		private bool IsAuxTargetVisible(IEnemyTrackable auxTarget)
		{
			if (auxTarget == null || auxTarget.Transform == null || !auxTarget.IsTrackable)
			{
				return false;
			}
			Vector3 vector = base.transform.position + Vector3.up * AuxTargetAimHeight;
			Vector3 vector2 = auxTarget.Transform.position + Vector3.up * AuxTargetAimHeight;
			if ((vector2 - vector).sqrMagnitude <= AuxTargetHardDetectDistance * AuxTargetHardDetectDistance)
			{
				return true;
			}
			if (!Physics.Linecast(vector, vector2, out var hitInfo, AuxSightMask, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			return hitInfo.transform.root == auxTarget.Transform.root;
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

		public void SetPendingAttractionZone(AttractionZoneData zone)
		{
			PendingAttractionZone = zone;
		}

		public bool TryGetAttractionApproachPoint(int attempts, out Vector3 point)
		{
			return _attractionZoneService.TrySampleApproachPoint(_navigationService, PendingAttractionZone.Origin, PendingAttractionZone.ApproachRadius, attempts, out point);
		}

		public bool TryGetPriorityPlayerTrackingHorizontalDistance(out float distance)
		{
			distance = -1f;
			if (PriorityPlayer?.NetworkObject == null)
			{
				return false;
			}
			PlayerRef inputAuthority = PriorityPlayer.NetworkObject.InputAuthority;
			if (!_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
			{
				return false;
			}
			Vector3 vector = position - base.transform.position;
			vector.y = 0f;
			distance = vector.magnitude;
			return true;
		}

		public bool HasLivePriorityPlayer()
		{
			return IsLiveSessionPlayer(PriorityPlayer);
		}

		public bool IsLiveSessionPlayer(PlayerDataHolder player)
		{
			if (player?.NetworkObject == null || !player.NetworkObject.IsValid)
			{
				return false;
			}
			return IsPlayerSessionActive(player.NetworkObject.InputAuthority);
		}

		public bool IsPlayerSessionActive(int playerId)
		{
			if (base.Runner == null || playerId < 0)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsPlayerSessionActive(PlayerRef playerRef)
		{
			if (base.Runner == null || playerRef == PlayerRef.None)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer == playerRef)
				{
					return true;
				}
			}
			return false;
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

		public void SetVisualState(AnchorVisualState value)
		{
			if (VisualState != value)
			{
				VisualState = value;
			}
		}

		public void SetAnchorThrown(bool value)
		{
			if (IsAnchorThrown != value)
			{
				IsAnchorThrown = value;
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

		public void SetAreaPosition(Vector3 position)
		{
			AreaPosition = position;
			HasAreaPosition = true;
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
			IsAnchorThrown = _IsAnchorThrown;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_VisualState = VisualState;
			_IsAnchorThrown = IsAnchorThrown;
		}
	}
}

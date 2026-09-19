using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(3)]
	public class ParrotMobEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
	{
		private const float SMOOTHED_VELOCITY_QUANTIZE_FACTOR = 10f;

		private const float MOVE_SPEED_CHANGE_THRESHOLD = 0.01f;

		[SerializeField]
		private float _smoothedVelocityLerpSpeed = 5f;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CurrentTargetPlayerId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CurrentTargetPlayerId = -1;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsLookAtPresentationActive", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsLookAtPresentationActive;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SmoothedVelocityQuantized", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _SmoothedVelocityQuantized;

		private ParrotMobEnemySettings _enemySettings;

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public Transform BodyTransform { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public ParrotMobAnimationEvent AnimationEvent { get; private set; }

		[field: SerializeField]
		public MoveSystem MoveSystem { get; private set; }

		[field: SerializeField]
		public RotateTowardsDirectionSystem RotateTowardsDirectionSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedSystem TargetPositionCompletedSystem { get; private set; }

		[field: SerializeField]
		public FindRandomPositionSystem FindRandomPositionSystem { get; private set; }

		[field: SerializeField]
		public TargetPositionCompletedResetSystem TargetPositionCompletedResetSystem { get; private set; }

		[field: SerializeField]
		public StateDurationTimeSystem StateDurationTimeSystem { get; private set; }

		[field: SerializeField]
		public ParrotMobPlayerTriggerSensor PlayerTriggerSensor { get; private set; }

		[field: SerializeField]
		public ParrotMobPlayerDetectionSyncSystem PlayerDetectionSyncSystem { get; private set; }

		[field: SerializeField]
		public EnemyDetectionAnalyticsSystem EnemyDetectionAnalyticsSystem { get; private set; }

		[field: SerializeField]
		public SimpleEnemyDamageable Damageable { get; private set; }

		[field: SerializeField]
		public EnemyStatHealthController StatHealthController { get; private set; }

		[field: SerializeField]
		public ParrotMobDamageProcessSystem DamageProcessSystem { get; private set; }

		public bool IsDead { get; set; }

		public bool IsFearing { get; set; }

		public bool IsDespawnAfterFear { get; set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe int CurrentTargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemyContext.CurrentTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemyContext.CurrentTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe bool IsLookAtPresentationActive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemyContext.IsLookAtPresentationActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemyContext.IsLookAtPresentationActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((short*)Ptr)[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[4] = value;
			}
		}

		public float MoveSpeed { get; set; }

		public bool IsCrouching { get; set; }

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

		public float ScreamCooldownRemaining { get; set; }

		public bool IsZoneEngagementActive { get; set; }

		public Vector3 HomePosition { get; private set; }

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
		public void InjectDependencies(ParrotMobEnemySettings enemySettings)
		{
			_enemySettings = enemySettings;
		}

		public virtual void Initialize()
		{
			if (_enemySettings == null)
			{
				throw new InvalidOperationException("ParrotMobEnemySettings is not bound in ParrotMobEnemyInstaller.");
			}
			SetMoveSpeed(_enemySettings.MoveSpeed);
			CompletePointMinDistance = _enemySettings.CompletePointMinDistance;
			TargetSearchRange = _enemySettings.DetectionRange;
			DistanceToAttack = 0f;
			TimeToAttack = 0f;
			AttackCooldown = 0f;
			IsDead = false;
			IsFearing = false;
			IsZoneEngagementActive = false;
			ScreamCooldownRemaining = 0f;
			if (base.HasStateAuthority)
			{
				CurrentTargetPlayerId = -1;
				IsLookAtPresentationActive = false;
			}
			HomePosition = base.transform.position;
			PlayerTriggerSensor.Configure(TargetSearchRange);
		}

		public void SetCurrentTargetPlayerId(int playerId)
		{
			if (base.HasStateAuthority && CurrentTargetPlayerId != playerId)
			{
				CurrentTargetPlayerId = playerId;
			}
		}

		public void SetCurrentTargetFromPlayer(PlayerDataHolder player)
		{
			if (player?.NetworkObject == null)
			{
				SetCurrentTargetPlayerId(-1);
				return;
			}
			PlayerRef inputAuthority = player.NetworkObject.InputAuthority;
			SetCurrentTargetPlayerId((inputAuthority != PlayerRef.None) ? inputAuthority.PlayerId : (-1));
		}

		public void SetLookAtPresentationActive(bool isActive)
		{
			if (base.HasStateAuthority && IsLookAtPresentationActive != isActive)
			{
				IsLookAtPresentationActive = isActive;
			}
		}

		public void TickScreamCooldown(float deltaTime)
		{
			if (!(ScreamCooldownRemaining <= 0f))
			{
				ScreamCooldownRemaining = Mathf.Max(0f, ScreamCooldownRemaining - deltaTime);
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
			return StatEntity.GetStat(statType)?.Value ?? fallbackValue;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CurrentTargetPlayerId = _CurrentTargetPlayerId;
			IsLookAtPresentationActive = _IsLookAtPresentationActive;
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CurrentTargetPlayerId = CurrentTargetPlayerId;
			_IsLookAtPresentationActive = IsLookAtPresentationActive;
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
		}
	}
}

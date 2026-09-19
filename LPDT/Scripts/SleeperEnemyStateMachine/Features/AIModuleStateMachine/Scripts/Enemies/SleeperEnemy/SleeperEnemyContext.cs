using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Detection;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Movement;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Timing;
using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems;
using Features.AudioServiceModule.Scripts;
using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	[NetworkBehaviourWeaved(8)]
	public class SleeperEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext, IEnemySoundOcclusionModel
	{
		private const float SMOOTHED_VELOCITY_QUANTIZE_FACTOR = 10f;

		private const float MOVE_SPEED_CHANGE_THRESHOLD = 0.01f;

		private const float NAVMESH_SAMPLE_RADIUS = 2f;

		[SerializeField]
		private float _smoothedVelocityLerpSpeed = 5f;

		[SerializeField]
		private float _targetSearchRange = 25f;

		[SerializeField]
		private float _completePointMinDistance = 1.5f;

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
		private SleeperVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HomePosition", 4, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _HomePosition;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasHomePosition", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasHomePosition;

		private float _distanceToAttack;

		private bool _initialized;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

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
		public SleeperVisionDetectingSystem SleeperVisionDetectingSystem { get; private set; }

		[field: SerializeField]
		public SleeperSearchRangeSetupSystem SleeperSearchRangeSetupSystem { get; private set; }

		[field: SerializeField]
		public SleeperMoveSpeedSetupSystem SleeperMoveSpeedSetupSystem { get; private set; }

		[field: SerializeField]
		public SleeperAggroSoundReactDelaySetupSystem SleeperAggroSoundReactDelaySetupSystem { get; private set; }

		[field: SerializeField]
		public TargetPlayerPrioritizeSystem TargetPlayerPrioritizeSystem { get; private set; }

		[field: SerializeField]
		public DetectedPlayersTimeSystem DetectedPlayersTimeSystem { get; private set; }

		[field: SerializeField]
		public EnemyDetectionAnalyticsSystem EnemyDetectionAnalyticsSystem { get; private set; }

		[field: SerializeField]
		public AttackCooldownSystem AttackCooldownSystem { get; private set; }

		[field: SerializeField]
		public StateDurationTimeSystem StateDurationTimeSystem { get; private set; }

		[field: SerializeField]
		public SoundOcclusionMonoSystem SoundOcclusionMonoSystem { get; private set; }

		[field: SerializeField]
		public SleeperSoundAggroSystem SleeperSoundAggroSystem { get; private set; }

		[field: SerializeField]
		public SleeperAggressiveSoundAggroSystem SleeperAggressiveSoundAggroSystem { get; private set; }

		[field: SerializeField]
		public SleeperSoundOcclusionHearingStrengthSetupSystem SleeperSoundOcclusionHearingStrengthSetupSystem { get; private set; }

		[field: SerializeField]
		public SleeperDamageAggrSystem SleeperDamageAggrSystem { get; private set; }

		[field: SerializeField]
		public SleeperFearSystem SleeperFearSystem { get; private set; }

		[field: SerializeField]
		public SleeperChaseGiveUpSystem SleeperChaseGiveUpSystem { get; private set; }

		[field: SerializeField]
		public SleeperAttackSystem SleeperAttackSystem { get; private set; }

		[field: SerializeField]
		public SleeperAttackRotationSystem SleeperAttackRotationSystem { get; private set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: Header("Audio Occlusion")]
		[field: SerializeField]
		public LayerMask OcclusionLayerMask { get; private set; }

		[field: SerializeField]
		public float OcclusionMaxDistance { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalDistanceMultiplier { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalFalloffExponent { get; private set; } = 2f;

		[field: SerializeField]
		public float LowPassMinValue { get; private set; } = 0.35f;

		[field: SerializeField]
		public string VoiceOcclusionLowPassParameterName { get; private set; } = "VoiceOcclusionLowPass";

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe SleeperVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (SleeperVisualState)Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 3)]
		public unsafe Vector3 HomePosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.HomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 4);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.HomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 4) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(7, 1)]
		public unsafe NetworkBool HasHomePosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.HasHomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 7);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SleeperEnemyContext.HasHomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 7) = value;
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

		public float DistanceToAttack
		{
			get
			{
				return _distanceToAttack;
			}
			set
			{
				_distanceToAttack = value;
				AvailablePointRange = value;
			}
		}

		public float AttackCooldown { get; set; }

		public bool IsAttackOnCooldown { get; set; }

		public float CurrentStateTime { get; set; }

		public Vector3 LastHeardSoundPosition { get; set; }

		public float HearingStrength { get; set; } = 1f;

		public float AggroSoundReactDelay { get; set; }

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

		public event Action<HeardSound> OnEnemyTriggeredBySound;

		[Inject]
		public void InjectDependencies(IAudioService audioService, PlayerMovableModel playerMovableModel)
		{
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			_initialized = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_initialized = false;
		}

		public virtual void Initialize()
		{
			SetMoveSpeed(GetStatValue(EntityStatType.WalkSpeed));
			TargetSearchRange = _targetSearchRange;
			CompletePointMinDistance = _completePointMinDistance;
			TimeToAttack = _attackCooldownTime;
			AttackCooldown = 0f;
			if (base.HasStateAuthority && !HasHomePosition)
			{
				HomePosition = NavMeshAgent.transform.position;
				HasHomePosition = true;
			}
		}

		public void SetVisualState(SleeperVisualState value)
		{
			if (_initialized && VisualState != value)
			{
				VisualState = value;
			}
		}

		public void SetSmoothedVelocity(float value)
		{
			if (_initialized)
			{
				short num = (short)Mathf.Clamp(Mathf.RoundToInt(value * 10f), -32768, 32767);
				if (SmoothedVelocityQuantized != num)
				{
					SmoothedVelocityQuantized = num;
				}
			}
		}

		public void SetMoveSpeed(float value)
		{
			if (_initialized && !(Mathf.Abs(MoveSpeed - value) < 0.01f))
			{
				MoveSpeed = value;
			}
		}

		public void SetIsCrouching(bool value)
		{
			if (_initialized && IsCrouching != value)
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

		public void SetDestinationToReachablePoint(Vector3 desired)
		{
			SetTargetPosition(ResolveReachablePoint(desired));
			SetTargetPositionCompleted(isCompleted: false);
		}

		private Vector3 ResolveReachablePoint(Vector3 desired)
		{
			if (!NavMeshAgent.isOnNavMesh)
			{
				return NavMeshAgent.transform.position;
			}
			if (!NavMesh.SamplePosition(desired, out var hit, 2f, -1))
			{
				return NavMeshAgent.transform.position;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!NavMeshAgent.CalculatePath(hit.position, navMeshPath) || navMeshPath.corners.Length == 0)
			{
				return NavMeshAgent.transform.position;
			}
			if (navMeshPath.status == NavMeshPathStatus.PathComplete)
			{
				return hit.position;
			}
			return navMeshPath.corners[navMeshPath.corners.Length - 1];
		}

		public void SetCurrentAreaType(int areaType)
		{
			CurrentAreaType = areaType;
		}

		public void SetDetectedPlayers(List<PlayerDataHolder> players)
		{
			DetectedPlayers = players ?? new List<PlayerDataHolder>();
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

		public void PlayOccludedOneShot(EventReference reference)
		{
			if (_audioService != null && !reference.IsNull && !(SoundSourceBehaviour == null))
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				_audioService.StartInstanceWith3DAttributes(eventInstance, SoundSourceBehaviour);
				ApplyOcclusion(eventInstance, smooth: false);
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

		public void ApplyOcclusion(EventInstance eventInstance, bool smooth)
		{
			if (!(_playerMovableModel?.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(VoiceOcclusionLowPassParameterName, out var value);
				float num = 1f;
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, OcclusionLayerMask, QueryTriggerInteraction.Ignore))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, OcclusionMaxDistance);
					num = Mathf.Lerp(1f, LowPassMinValue, normalizedOcclusionDistance);
				}
				if (!smooth)
				{
					eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, num, ignoreseekspeed: true);
					return;
				}
				float value2 = Mathf.Lerp(value, num, Time.deltaTime * 5f);
				eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, value2, ignoreseekspeed: true);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
			MoveSpeed = _MoveSpeed;
			IsCrouching = _IsCrouching;
			VisualState = _VisualState;
			HomePosition = _HomePosition;
			HasHomePosition = _HasHomePosition;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_VisualState = VisualState;
			_HomePosition = HomePosition;
			_HasHomePosition = HasHomePosition;
		}
	}
}

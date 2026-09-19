using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Animation;
using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Systems;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy
{
	public class HeadmanEnemyContext : MonoBehaviour
	{
		public EventInstance ChasingSoundInstance;

		public EventInstance IdleSoundInstance;

		private IAudioService _audioService;

		private static int _auxSightMask = -1;

		private IEnemyTrackable _auxTarget;

		private IDamageable _auxTargetDamageable;

		private float _auxTargetSearchTimer;

		private IEnemyTrackingService _enemyTrackingService;

		private SpawnedPlayersModel _spawnedPlayers;

		private PlayerDamageablesTrackModel _playerDamageTrackModel;

		private IPlayerStateService _playerStateService;

		private INavigationService _navigationService;

		private IEnemyAttractionZoneService _attractionZoneService;

		private EnemyTargetPositionsModel _targetPositionsModel;

		private PlayerMovableModel _playerMovableModel;

		private MultiplayerModel _multiplayerModel;

		private IScreenShakeService _screenShakeService;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private HeadmanChasingSettings _chasingSettings;

		private HeadmanWanderingSettings _wanderingSettings;

		private HeadmanFearSettings _fearSettings;

		private HeadmanNavmeshPositionSettings _navmeshPositionSettings;

		private HeadmanEnemy _headmanEnemy;

		private int _originalAreaMask;

		[field: Header("Scene refs")]
		[field: SerializeField]
		public EnemyStatHealthController StatHealthController { get; private set; }

		[field: SerializeField]
		public DamageableAnimationFunctionReactor AttackReactor { get; private set; }

		[field: SerializeField]
		public RageAnimationFunctionReactor RageReactor { get; private set; }

		[field: SerializeField]
		public HeadManVisibilityTargetDetector TargetDetector { get; private set; }

		[field: SerializeField]
		public HeadManTargetsModel HeadManTargetsModel { get; private set; }

		[field: SerializeField]
		public NavMeshAgent Agent { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public CinemachineImpulseSource CinemachineImpulseSource { get; private set; }

		[field: SerializeField]
		public Transform SafeZoneRayCastPoint { get; private set; }

		[field: SerializeField]
		public HeadmanSafeZoneBlocker SafeZoneBlocker { get; private set; }

		[field: SerializeField]
		public Transform LookTargetTransform { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: SerializeField]
		public HeadmanLowAttackInterestPointSystem HeadmanLowAttackInterestPointSystem { get; private set; }

		[field: Header("Audio")]
		[field: SerializeField]
		public EventReference ChasingLoopReference { get; private set; }

		[field: SerializeField]
		public EventReference AttackReference { get; private set; }

		[field: SerializeField]
		public EventReference IdleLoopReference { get; private set; }

		[field: SerializeField]
		public EventReference RoarReference { get; private set; }

		[field: SerializeField]
		public EventReference NoticePlayerReference { get; private set; }

		[field: SerializeField]
		public EventReference BiteReference { get; private set; }

		[field: SerializeField]
		public LayerMask OcclusionLayerMask { get; private set; }

		[field: SerializeField]
		public float OcclusionMaxDistance { get; private set; }

		[field: SerializeField]
		public float VerticalDistanceMultiplier { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalFalloffExponent { get; private set; } = 2f;

		[field: SerializeField]
		public float LowPassMinValue { get; private set; }

		[field: SerializeField]
		public string VoiceOcclusionLowPassParameterName { get; private set; } = "VoiceOcclusionLowPass";

		[field: Header("VFX")]
		[field: SerializeField]
		public ScreenShakeData RageStartScreenShakeData { get; private set; }

		[field: SerializeField]
		public ScreenShakeData RageInteractionScreenShakeData { get; private set; }

		public PlayerRef TargetPlayerToChase { get; set; } = PlayerRef.None;

		public PlayerRef LastPlayerToChase { get; set; } = PlayerRef.None;

		public int DamageDealerPlayerId { get; set; }

		public HeadmanRageStateId ActiveRageSubstate { get; set; }

		public HeadmanChasingStateId ActiveChasingSubstate { get; set; }

		public bool IsDead { get; set; }

		public bool IsFearing { get; set; }

		public bool IsDespawnAfterFear { get; set; } = true;

		public float WanderingTimer { get; set; }

		public float ChangeLocationTimer { get; set; }

		public float AttackTimer { get; set; }

		public float SlowedTimer { get; set; }

		public float FearDestinationTimer { get; set; }

		public float CurrentAttackElapsed { get; set; }

		public bool AttackDamageApplied { get; set; }

		public bool AttackExitSent { get; set; }

		public Vector3 AreaPosition { get; private set; }

		public float TargetPositionUpdateFrequency { get; private set; }

		public bool HasLastSeenPlayerPosition { get; set; }

		public Vector3 LastSeenPlayerPosition { get; set; }

		public float RageTimeLeft { get; set; }

		public float RageWanderTimer { get; set; }

		public float RageApproachTimer { get; set; }

		public bool RageSessionInitialized { get; set; }

		public bool RageIsApproachingCenter { get; set; }

		public float RageInteractedTimer { get; set; }

		public int RoarsPerformed { get; set; }

		public bool HasPrioritizedTarget
		{
			get
			{
				if (HeadManTargetsModel != null && HeadManTargetsModel.PrioritizedTargets != null)
				{
					return HeadManTargetsModel.PrioritizedTargets.Count > 0;
				}
				return false;
			}
		}

		public bool HasPlayerChaseTarget => TargetPlayerToChase != PlayerRef.None;

		public bool HasAuxChaseTarget
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

		public bool HasChaseTarget
		{
			get
			{
				if (!HasPlayerChaseTarget)
				{
					return HasAuxChaseTarget;
				}
				return true;
			}
		}

		public AttractionZoneData PendingAttractionZone { get; private set; }

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

		public bool CanKeepChasing()
		{
			if (HasPlayerChaseTarget)
			{
				if (HasPrioritizedTarget)
				{
					return _playerStateService.IsPlayerAlive(TargetPlayerToChase.PlayerId);
				}
				return false;
			}
			return HasAuxChaseTarget;
		}

		public void SetPendingAttractionZone(AttractionZoneData zone)
		{
			PendingAttractionZone = zone;
		}

		public bool TryGetAttractionApproachPoint(int attempts, out Vector3 point)
		{
			return _attractionZoneService.TrySampleApproachPoint(_navigationService, PendingAttractionZone.Origin, PendingAttractionZone.ApproachRadius, attempts, out point);
		}

		public bool CanEnemyInteractWithChaseTarget()
		{
			if (!HasPlayerChaseTarget)
			{
				return HasAuxChaseTarget;
			}
			return _enemyPlayerAttackabilityService.CanEnemyAttackPlayer(TargetPlayerToChase.PlayerId);
		}

		public bool TryAcquireAuxTarget(float deltaTime)
		{
			if (HasPlayerChaseTarget || _chasingSettings == null || !_chasingSettings.IsAuxTargetingEnabled || _enemyTrackingService == null)
			{
				return false;
			}
			_auxTargetSearchTimer -= deltaTime;
			if (_auxTargetSearchTimer > 0f)
			{
				return false;
			}
			_auxTargetSearchTimer = _chasingSettings.AuxTargetSearchInterval;
			if (!_enemyTrackingService.TryGetNearestTarget(base.transform.position, _chasingSettings.AuxTargetDetectionRadius, out var target))
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
			Vector3 vector = ((SafeZoneRayCastPoint != null) ? SafeZoneRayCastPoint.position : base.transform.position);
			Vector3 vector2 = auxTarget.Transform.position + Vector3.up * _chasingSettings.AuxTargetAimHeight;
			if ((vector2 - vector).sqrMagnitude <= _chasingSettings.AuxTargetHardDetectDistance * _chasingSettings.AuxTargetHardDetectDistance)
			{
				return true;
			}
			if (!Physics.Linecast(vector, vector2, out var hitInfo, AuxSightMask, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			return hitInfo.transform.root == auxTarget.Transform.root;
		}

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayers, PlayerDamageablesTrackModel playerDamageTrackModel, IPlayerStateService playerStateService, INavigationService navigationService, IEnemyAttractionZoneService attractionZoneService, EnemyTargetPositionsModel targetPositionsModel, PlayerMovableModel playerMovableModel, MultiplayerModel multiplayerModel, IScreenShakeService screenShakeService, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, HeadmanEnemy headmanEnemy, IAudioService audioService, IEnemyTrackingService enemyTrackingService)
		{
			_enemyTrackingService = enemyTrackingService;
			_spawnedPlayers = spawnedPlayers;
			_playerDamageTrackModel = playerDamageTrackModel;
			_playerStateService = playerStateService;
			_navigationService = navigationService;
			_attractionZoneService = attractionZoneService;
			_targetPositionsModel = targetPositionsModel;
			_playerMovableModel = playerMovableModel;
			_multiplayerModel = multiplayerModel;
			_screenShakeService = screenShakeService;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_headmanEnemy = headmanEnemy;
			_audioService = audioService;
		}

		public void Init(HeadmanChasingSettings chasing, HeadmanWanderingSettings wandering, HeadmanFearSettings fear, HeadmanNavmeshPositionSettings navmeshPositionSettings)
		{
			_chasingSettings = chasing;
			_wanderingSettings = wandering;
			_fearSettings = fear;
			_navmeshPositionSettings = navmeshPositionSettings;
			if (Agent != null)
			{
				_originalAreaMask = Agent.areaMask;
				if (_headmanEnemy.HasStateAuthority)
				{
					Agent.stoppingDistance = chasing.StoppingDistance;
					Agent.Warp(base.transform.position);
				}
			}
			if (TargetDetector != null)
			{
				TargetDetector.Init(chasing.DistanceToAttack);
			}
			UpdatePositionUpdateFrequency();
			WanderingTimer = TargetPositionUpdateFrequency;
			ChangeLocationTimer = 0f;
		}

		public void CreateFmodInstances()
		{
			ChasingSoundInstance = _audioService.CreateInstance(ChasingLoopReference);
			IdleSoundInstance = _audioService.CreateInstance(IdleLoopReference);
		}

		public void ReleaseFmodInstances()
		{
			_audioService.StopInstance(ChasingSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(ChasingSoundInstance);
			_audioService.StopInstance(IdleSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(IdleSoundInstance);
		}

		public void SetAreaPosition(Vector3 areaPosition)
		{
			AreaPosition = areaPosition;
			_targetPositionsModel.UpdateEnemyAreaPosition(EnemyType.HeadMan, base.gameObject.GetHashCode(), AreaPosition);
		}

		public bool TrySyncTargetFromPriority()
		{
			if (!HasPrioritizedTarget)
			{
				return false;
			}
			PlayerRef playerRef = HeadManTargetsModel.PrioritizedTargets[0];
			ClearAuxTarget();
			TargetPlayerToChase = playerRef;
			LastPlayerToChase = playerRef;
			return true;
		}

		public void ClearTarget()
		{
			TargetPlayerToChase = PlayerRef.None;
		}

		public bool ShouldEnterRageWhenNoTargets()
		{
			if (HasPrioritizedTarget)
			{
				return false;
			}
			if (LastPlayerToChase == PlayerRef.None)
			{
				return false;
			}
			if (!_playerStateService.IsPlayerAlive(LastPlayerToChase.PlayerId))
			{
				return false;
			}
			if (!_navigationService.TryGetPlayerTrackingPosition(LastPlayerToChase, out var position))
			{
				return false;
			}
			NavMeshHit hit;
			return !_navigationService.IsPointOnNavMeshProjected(position, Agent, out hit);
		}

		public bool TryGetTargetTrackingWorldPosition(out Vector3 worldPosition)
		{
			worldPosition = Vector3.zero;
			if (!HasPlayerChaseTarget)
			{
				if (!HasAuxChaseTarget)
				{
					return false;
				}
				worldPosition = _auxTarget.Transform.position;
				return true;
			}
			return _navigationService.TryGetPlayerTrackingPosition(TargetPlayerToChase, out worldPosition);
		}

		public bool TryGetPlayerTrackingWorldPosition(PlayerRef player, out Vector3 worldPosition)
		{
			return _navigationService.TryGetPlayerTrackingPosition(player, out worldPosition);
		}

		public float GetDistanceToTarget()
		{
			if (!HasPlayerChaseTarget)
			{
				if (!HasAuxChaseTarget)
				{
					return float.PositiveInfinity;
				}
				Vector3 vector = _auxTarget.Transform.position - base.transform.position;
				vector.y = 0f;
				return vector.magnitude;
			}
			if (!(TargetDetector != null))
			{
				return float.PositiveInfinity;
			}
			return TargetDetector.GetDistanceToPlayer(TargetPlayerToChase);
		}

		private bool IsObstacleOnPathToTarget()
		{
			if (HasPlayerChaseTarget && TargetDetector != null)
			{
				return TargetDetector.IsObstacleOnPath(TargetPlayerToChase);
			}
			return false;
		}

		public bool CanStartAttack()
		{
			if (!CanEnemyInteractWithChaseTarget())
			{
				return false;
			}
			if (!HasChaseTarget)
			{
				return false;
			}
			if (Agent == null || !Agent.enabled || Agent.isOnOffMeshLink)
			{
				return false;
			}
			if (AttackTimer < _chasingSettings.AttackTime)
			{
				return false;
			}
			if (IsObstacleOnPathToTarget())
			{
				return false;
			}
			if (!TryGetTargetTrackingWorldPosition(out var worldPosition))
			{
				return false;
			}
			Vector3 availablePosition;
			if (CheckDistanceEnoughToAttack())
			{
				return IsTargetPositionAvailable(worldPosition, out availablePosition);
			}
			return false;
		}

		public bool ShouldUseLowAttack()
		{
			if (!HasPlayerChaseTarget)
			{
				return false;
			}
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(TargetPlayerToChase, out var value))
			{
				return false;
			}
			if (!TryGetTargetTrackingWorldPosition(out var worldPosition))
			{
				return false;
			}
			float num = worldPosition.y - base.transform.position.y;
			if (value.IsHardCrouch())
			{
				return num > 0f;
			}
			return false;
		}

		public bool CheckDistanceEnoughToAttack(float coefficient = 1f)
		{
			return GetDistanceToTarget() <= _chasingSettings.DistanceToAttack * coefficient;
		}

		public bool IsTargetPositionAvailable(Vector3 targetRaycastPoint, out Vector3 availablePosition)
		{
			availablePosition = Vector3.zero;
			if (Agent == null)
			{
				return false;
			}
			if (!_navigationService.HasAvailablePointInRange(Agent, targetRaycastPoint, _chasingSettings.DistanceToAttack, out availablePosition))
			{
				return false;
			}
			if (SafeZoneBlocker == null)
			{
				return true;
			}
			PlayerSafeZone safeZone;
			return !SafeZoneBlocker.TryFindBlockingSafeZone(availablePosition, targetRaycastPoint, out safeZone);
		}

		public bool MoveToTargetPlayer()
		{
			if (!HasChaseTarget)
			{
				return false;
			}
			if (TryGetTargetTrackingWorldPosition(out var worldPosition) && _navigationService.HasAvailablePointInRange(Agent, worldPosition, _chasingSettings.DistanceToAttack, out var availablePosition))
			{
				Agent.SetDestination(availablePosition);
				return true;
			}
			return false;
		}

		public void MoveToPosition(Vector3 position)
		{
			if (!(Agent == null) && Agent.isOnNavMesh)
			{
				Agent.SetDestination(position);
				_targetPositionsModel.UpdateEnemyTargetPosition(EnemyType.HeadMan, base.gameObject.GetHashCode(), position);
			}
		}

		public Vector3 GetRandomNavmeshPosition(float radius)
		{
			Vector3 result = base.transform.position;
			if (_targetPositionsModel.TargetPositions.TryGetValue(EnemyType.HeadMan, out var value) && _navigationService.TryGetRandomSafeNavmeshPosition(AreaPosition, radius, _navmeshPositionSettings.Attempts, new List<Vector3>(value.Positions.Values), Agent.agentTypeID, -1, out var bestPosition, _navmeshPositionSettings.MinDistanceFromCenter, _navmeshPositionSettings.CenterDistanceWeight, _navmeshPositionSettings.AverageAvoidDistanceWeight, _navmeshPositionSettings.TooClosePenaltyRadius, _navmeshPositionSettings.TooClosePenaltyWeight))
			{
				result = bestPosition;
			}
			return result;
		}

		public void UpdatePositionUpdateFrequency()
		{
			TargetPositionUpdateFrequency = Random.Range(_wanderingSettings.MinWanderPositionUpdateTime, _wanderingSettings.MaxWanderPositionUpdateTime);
		}

		public void SetMovementEnabled(bool enabled)
		{
			if (!(Agent == null) && !enabled && Agent.isOnNavMesh)
			{
				Agent.ResetPath();
			}
		}

		public void RestoreDefaultAreaMask()
		{
			if (Agent != null)
			{
				Agent.areaMask = _originalAreaMask;
			}
		}

		public void UseDefaultAreaMaskOnly()
		{
			if (Agent != null)
			{
				Agent.areaMask = 1 << NavMesh.GetAreaFromName(_chasingSettings.DefaultAreaName);
			}
		}

		public void RotateTowards(Vector3 worldPosition, float minSqrMagnitude = 0.01f)
		{
			Vector3 vector = worldPosition - base.transform.position;
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
			if (vector2.sqrMagnitude > minSqrMagnitude)
			{
				base.transform.rotation = Quaternion.LookRotation(vector2.normalized);
			}
		}

		public bool TryDealBaseAttackDamage()
		{
			if (!CanEnemyInteractWithChaseTarget())
			{
				return false;
			}
			if (!HasChaseTarget)
			{
				return false;
			}
			if (!TryGetTargetDamageable(out var _))
			{
				return false;
			}
			bool flag = true;
			if (TryGetTargetTrackingWorldPosition(out var worldPosition) && SafeZoneBlocker != null && SafeZoneBlocker.TryFindBlockingSafeZoneUnderPlayer(worldPosition, out var _))
			{
				flag = false;
			}
			if (CheckDistanceEnoughToAttack(_chasingSettings.BaseAttackRangeCoefficient) && !IsObstacleOnPathToTarget() && flag)
			{
				DealDamageToTarget();
				return true;
			}
			return false;
		}

		public bool TryDealLowAttackDamage()
		{
			if (!CanEnemyInteractWithChaseTarget())
			{
				return false;
			}
			if (!HasChaseTarget)
			{
				return false;
			}
			if (!TryGetTargetDamageable(out var _))
			{
				return false;
			}
			if (!TryGetTargetTrackingWorldPosition(out var worldPosition))
			{
				return false;
			}
			if (SafeZoneBlocker != null && SafeZoneBlocker.TryFindBlockingSafeZoneUnderPlayer(worldPosition, out var _))
			{
				return false;
			}
			if (CheckDistanceEnoughToAttack() && !IsObstacleOnPathToTarget())
			{
				DealDamageToTarget();
				return true;
			}
			return false;
		}

		public bool TryGetTargetDamageable(out IDamageable damageable)
		{
			if (!HasPlayerChaseTarget)
			{
				damageable = _auxTargetDamageable;
				if (HasAuxChaseTarget)
				{
					return damageable != null;
				}
				return false;
			}
			if (_playerDamageTrackModel.AllPlayerDamageables.TryGetValue(TargetPlayerToChase.PlayerId, out damageable))
			{
				return damageable != null;
			}
			return false;
		}

		private void DealDamageToTarget()
		{
			if (TryGetTargetDamageable(out var damageable) && TryGetTargetDirectionPosition(out var directionTarget))
			{
				damageable.Damage(new DamageData
				{
					Damage = GetStatValue(EntityStatType.Damage, _chasingSettings.Damage),
					Position = base.transform.position,
					Direction = directionTarget - base.transform.position,
					DamageDealerPlayerID = DamageDealerPlayerId,
					Force = _chasingSettings.DamageImpulseStrength,
					ForceMode = ForceMode.Impulse,
					Source = DamageDataSourceExtensions.ForEnemyAttack(base.transform, EnemyType.HeadMan.ToString(), DamageType.Melee)
				});
			}
		}

		private bool TryGetTargetDirectionPosition(out Vector3 directionTarget)
		{
			directionTarget = Vector3.zero;
			if (!HasPlayerChaseTarget)
			{
				return TryGetTargetTrackingWorldPosition(out directionTarget);
			}
			if (!_spawnedPlayers.Players.TryGetValue(TargetPlayerToChase, out var value))
			{
				return false;
			}
			directionTarget = value.NetworkObject.transform.position;
			if (_navigationService.TryGetPlayerTrackingPosition(TargetPlayerToChase, out var position))
			{
				directionTarget = position;
			}
			return true;
		}

		public bool IsEnemyVisibleByPlayers()
		{
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				NavMeshPath path = new NavMeshPath();
				if (_navigationService.TryGetPlayerTrackingPosition(activePlayer, out var position))
				{
					if (!_spawnedPlayers.Players.TryGetValue(activePlayer, out var value))
					{
						return false;
					}
					NetworkObject networkObject = value.NetworkObject;
					if (networkObject == null || !networkObject.TryGetComponent<PlayerLookDetection>(out var component) || component == null)
					{
						return false;
					}
					if (component.IsLookingAtObject(LookTargetTransform, angleCullEnabled: false, _fearSettings.FearDetectionDistance / 2f))
					{
						return true;
					}
					if (_navigationService.IsPointOnNavMeshProjected(position, Agent.agentTypeID, out var hit) && Agent.CalculatePath(hit.position, path) && path.GetLength() < _fearSettings.FearDetectionDistance)
					{
						return true;
					}
					if (Vector3.Distance(base.transform.position, position) < _fearSettings.FearDetectionDistance / 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void UpdateLongEventOcclusion()
		{
			if (ChasingSoundInstance.isValid())
			{
				ChasingSoundInstance.set3DAttributes(SoundSourceBehaviour.SoundSourceTransform.To3DAttributes());
			}
			if (IdleSoundInstance.isValid())
			{
				IdleSoundInstance.set3DAttributes(SoundSourceBehaviour.SoundSourceTransform.To3DAttributes());
			}
			ProcessSoundOcclusion(ChasingSoundInstance);
			ProcessSoundOcclusion(IdleSoundInstance);
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

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (eventInstance.isValid() && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(VoiceOcclusionLowPassParameterName, out var value);
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, OcclusionMaxDistance);
					float b = Mathf.Lerp(1f, LowPassMinValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(VoiceOcclusionLowPassParameterName, value3);
				}
			}
		}

		public void PlayNoticeSound()
		{
			PlayOneShot(NoticePlayerReference);
		}

		public void PlayAttackSound()
		{
			PlayOneShot(AttackReference);
		}

		public void PlayRoarSound()
		{
			PlayOneShot(RoarReference);
		}

		public void PlayBiteSound()
		{
			PlayOneShot(BiteReference);
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

		public void TriggerRageStartScreenShake()
		{
			if (_screenShakeService != null && CinemachineImpulseSource != null)
			{
				_screenShakeService.TriggerScreenShake(CinemachineImpulseSource, RageStartScreenShakeData);
			}
		}

		public void TriggerRageInteractScreenShake()
		{
			if (_screenShakeService != null && CinemachineImpulseSource != null)
			{
				_screenShakeService.TriggerScreenShake(CinemachineImpulseSource, RageInteractionScreenShakeData);
			}
		}

		public float GetStatValue(EntityStatType statType, float fallbackValue = 0f)
		{
			if (StatEntity == null)
			{
				return fallbackValue;
			}
			return StatEntity.GetStat(statType)?.Value ?? fallbackValue;
		}

		public bool TryGetBlockingSafeZoneOnRage(out PlayerSafeZone safeZone)
		{
			safeZone = null;
			if (!HasLastSeenPlayerPosition)
			{
				return false;
			}
			Vector3 enemyPosition = ((SafeZoneRayCastPoint != null) ? SafeZoneRayCastPoint.position : base.transform.position);
			Vector3 lastSeenPlayerPosition = LastSeenPlayerPosition;
			if (SafeZoneBlocker != null)
			{
				return SafeZoneBlocker.TryFindBlockingSafeZone(enemyPosition, lastSeenPlayerPosition, out safeZone);
			}
			return false;
		}

		public bool IsChaseTargetUnderSafeZone()
		{
			if (!TryGetTargetTrackingWorldPosition(out var worldPosition))
			{
				return false;
			}
			PlayerSafeZone safeZone;
			if (SafeZoneBlocker != null)
			{
				return SafeZoneBlocker.TryFindBlockingSafeZoneUnderPlayer(worldPosition, out safeZone);
			}
			return false;
		}
	}
}

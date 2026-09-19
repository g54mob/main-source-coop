using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.LevelGatesModule.Data;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateEnemyContext : MonoBehaviour
	{
		private IPlayerStateService _playerStateService;

		private PlayersStatesSynchronizer _playerStateModel;

		private PlayerMovableModel _playerMovableModel;

		private PlayersGatesModelSynchronizedModel _playersGatesModel;

		private INavigationService _navigationService;

		private IEnemyAttractionZoneService _attractionZoneService;

		private EnemyTargetPositionsModel _targetPositionsModel;

		private EnemySafeZoneInteractionPointsModel _safeZoneInteractionPointsModel;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private IEnemyTrackingService _enemyTrackingService;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private CauldronStealthModel _cauldronStealthModel;

		private readonly List<Transform> _safeZoneInteractionPointCandidates = new List<Transform>();

		private PlayerRef _targetPlayer = PlayerRef.None;

		private IEnemyTrackable _auxTarget;

		private IDamageable _auxTargetDamageable;

		private float _auxTargetSearchTimer;

		private EnemyType _enemyType = EnemyType.PiratePistol;

		private bool _hasAreaPosition;

		private bool _hasObservedTargetSafeZoneState;

		private bool _wasObservedTargetInSafeZone;

		private bool _hasRecentOutsideSafeZoneObservation;

		private float _lastOutsideSafeZoneObservationTime;

		private Vector3 _lastOutsideSafeZoneObservationPosition;

		[field: SerializeField]
		public EnemyTargetDetectorBase TargetDetector { get; set; }

		[field: SerializeField]
		public EnemyMovableBase EnemyMovableBase { get; set; }

		[field: SerializeField]
		public PirateConfiguration PirateConfiguration { get; set; }

		[field: SerializeField]
		public PirateAnimationController PirateAnimationController { get; set; }

		[field: SerializeField]
		public EnemyWeaponBase PirateAttackController { get; set; }

		[field: SerializeField]
		public PirateAttackRotationComponent PirateAttackRotationComponent { get; set; }

		[field: SerializeField]
		public EnemySafeZoneAttackDetector PirateSafeZoneDetector { get; set; }

		[field: SerializeField]
		public Transform RaycastShootPoint { get; set; }

		[field: SerializeField]
		public PirateAimController PirateAimController { get; set; }

		[field: SerializeField]
		public SimpleEnemyDamageable Damageable { get; set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: SerializeField]
		public LayerMask ObstacleLayerMask { get; set; }

		[field: SerializeField]
		public PirateAudioController PirateAudioController { get; set; }

		[field: SerializeField]
		public bool IsDespawnAfterFear { get; set; }

		public PlayerRef TargetPlayer
		{
			get
			{
				return _targetPlayer;
			}
			set
			{
				if (_targetPlayer != value)
				{
					ClearTargetObservation();
				}
				ClearAuxTarget();
				_targetPlayer = value;
				RefreshTargetBeachState();
			}
		}

		public PirateTargetKind TargetKind
		{
			get
			{
				if (_targetPlayer != PlayerRef.None)
				{
					return PirateTargetKind.Player;
				}
				PruneDeadAuxTarget();
				if (_auxTarget == null)
				{
					return PirateTargetKind.None;
				}
				return PirateTargetKind.Aux;
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

		public bool IsFearing { get; set; }

		public bool IsDead { get; set; }

		public Vector3 SafeZoneAttackPosition { get; private set; }

		public Vector3 AreaPosition { get; private set; }

		public float IdleWanderTimer { get; set; }

		public float IdleWanderUpdateInterval { get; private set; }

		public float IdleChangeAreaTimer { get; set; }

		public float ChaseTargetLostTimer { get; private set; }

		public Vector3 LastKnownTargetPosition { get; private set; }

		public Vector3 LastKnownTargetDirection { get; private set; }

		public bool HasLastKnownTargetPosition { get; private set; }

		public bool HasLastKnownTargetDirection { get; private set; }

		public bool HasObservedTargetEnterSafeZone { get; private set; }

		public bool HasSafeZoneAttackPosition { get; private set; }

		public bool IsSafeZoneAttackActive { get; set; }

		public bool IsTargetOnBeach { get; private set; }

		public PlayersGatesModelSynchronizedModel PlayersGatesModel => _playersGatesModel;

		public AttractionZoneData PendingAttractionZone { get; private set; }

		public void ApplyWalkingSpeed()
		{
			if (!(PirateConfiguration == null) && !(EnemyMovableBase == null))
			{
				EnemyMovableBase.SetMovementSpeed(PirateConfiguration.WalkingSpeed);
			}
		}

		public void ApplyChaseSpeed()
		{
			if (!(PirateConfiguration == null) && !(EnemyMovableBase == null))
			{
				EnemyMovableBase.SetMovementSpeed(PirateConfiguration.ChaseSpeed);
			}
		}

		[Inject]
		private void InjectDependencies(IPlayerStateService playerStateService, PlayersStatesSynchronizer playerStateModel, PlayerMovableModel playerMovableModel, PlayersGatesModelSynchronizedModel playersGatesModel, INavigationService navigationService, EnemyTargetPositionsModel targetPositionsModel, EnemySafeZoneInteractionPointsModel safeZoneInteractionPointsModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, IEnemyAttractionZoneService attractionZoneService, CauldronStealthModel cauldronStealthModel, IEnemyTrackingService enemyTrackingService, PlayerDamageablesTrackModel playerDamageablesTrackModel)
		{
			_enemyTrackingService = enemyTrackingService;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_cauldronStealthModel = cauldronStealthModel;
			_playerStateService = playerStateService;
			_playerStateModel = playerStateModel;
			_playerMovableModel = playerMovableModel;
			_playersGatesModel = playersGatesModel;
			_navigationService = navigationService;
			_targetPositionsModel = targetPositionsModel;
			_safeZoneInteractionPointsModel = safeZoneInteractionPointsModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_attractionZoneService = attractionZoneService;
			RefreshTargetBeachState();
		}

		public bool CanEnemyInteractWithTarget()
		{
			if (TargetKind == PirateTargetKind.Aux)
			{
				return HasAuxTarget;
			}
			if (TargetPlayer != PlayerRef.None)
			{
				return _enemyPlayerAttackabilityService.CanEnemyAttackPlayer(TargetPlayer.PlayerId);
			}
			return false;
		}

		public void SetAuxTarget(IEnemyTrackable auxTarget)
		{
			if (_auxTarget != auxTarget)
			{
				TargetPlayer = PlayerRef.None;
				_auxTarget = auxTarget;
				_auxTargetDamageable = null;
				if (auxTarget != null && auxTarget.Transform != null)
				{
					auxTarget.Transform.TryGetComponent<IDamageable>(out _auxTargetDamageable);
				}
				ClearTargetObservation();
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

		public bool TryAcquireAuxTarget(float deltaTime)
		{
			if (PirateConfiguration == null || !PirateConfiguration.IsAuxTargetingEnabled || _enemyTrackingService == null)
			{
				return false;
			}
			_auxTargetSearchTimer -= deltaTime;
			if (_auxTargetSearchTimer > 0f)
			{
				return false;
			}
			_auxTargetSearchTimer = PirateConfiguration.AuxTargetSearchInterval;
			if (!_enemyTrackingService.TryGetNearestTarget(base.transform.position, PirateConfiguration.AuxTargetDetectionRadius, out var target))
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

		public bool TryStealTargetToPlayer()
		{
			if (TargetKind != PirateTargetKind.Aux || TargetDetector == null)
			{
				return false;
			}
			foreach (PlayerRef detectTarget in TargetDetector.DetectTargets)
			{
				if (IsPlayerAlive(detectTarget.PlayerId) && !IsPlayerStealthedByHeadwear(detectTarget.PlayerId) && IsPlayerVisible(detectTarget) && (!_playerMovableModel.AllCharacterMovables.TryGetValue(detectTarget, out var value) || !(value != null) || !IsPositionInSafeZone(value.CameraPositionTransform.position)))
				{
					TargetPlayer = detectTarget;
					return true;
				}
			}
			return false;
		}

		public bool IsTargetVisible()
		{
			if (TargetKind != PirateTargetKind.Aux)
			{
				return IsPlayerVisible(TargetPlayer);
			}
			return IsAuxTargetVisible(_auxTarget);
		}

		public bool TryGetTargetMovePosition(out Vector3 targetPosition)
		{
			targetPosition = Vector3.zero;
			if (TargetKind == PirateTargetKind.Aux)
			{
				if (!HasAuxTarget)
				{
					return false;
				}
				targetPosition = _auxTarget.Transform.position;
				return true;
			}
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(TargetPlayer, out var value) || value == null)
			{
				return false;
			}
			targetPosition = value.transform.position;
			return true;
		}

		public bool TryGetTargetDamageable(out IDamageable damageable)
		{
			if (TargetKind == PirateTargetKind.Aux)
			{
				damageable = _auxTargetDamageable;
				return damageable != null;
			}
			if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(TargetPlayer.PlayerId, out damageable))
			{
				return damageable != null;
			}
			return false;
		}

		private bool IsAuxTargetVisible(IEnemyTrackable auxTarget)
		{
			if (auxTarget == null || auxTarget.Transform == null || !auxTarget.IsTrackable)
			{
				return false;
			}
			if (RaycastShootPoint == null)
			{
				return true;
			}
			Vector3 position = RaycastShootPoint.position;
			Vector3 vector = auxTarget.Transform.position + Vector3.up * ((PirateConfiguration != null) ? PirateConfiguration.AuxTargetAimHeight : 0f);
			if (PirateConfiguration != null && (vector - position).sqrMagnitude <= PirateConfiguration.AuxTargetHardDetectDistance * PirateConfiguration.AuxTargetHardDetectDistance)
			{
				return true;
			}
			if (!Physics.Linecast(position, vector, out var hitInfo, ObstacleLayerMask, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			return hitInfo.transform.root == auxTarget.Transform.root;
		}

		public bool IsPlayerStealthedByHeadwear(int playerId)
		{
			return _cauldronStealthModel.IsStealthed(playerId);
		}

		public void SetPendingAttractionZone(AttractionZoneData zone)
		{
			PendingAttractionZone = zone;
		}

		public bool TryGetAttractionApproachPoint(int attempts, out Vector3 point)
		{
			return _attractionZoneService.TrySampleApproachPoint(_navigationService, PendingAttractionZone.Origin, PendingAttractionZone.ApproachRadius, attempts, out point);
		}

		public void SetAreaPosition(EnemyType enemyType, Vector3 areaPosition)
		{
			_enemyType = enemyType;
			AreaPosition = areaPosition;
			_hasAreaPosition = true;
			_targetPositionsModel?.UpdateEnemyAreaPosition(_enemyType, base.gameObject.GetHashCode(), AreaPosition);
		}

		public void ResetIdleWanderTimer(bool moveImmediately = true)
		{
			UpdateIdleWanderUpdateInterval();
			IdleWanderTimer = (moveImmediately ? IdleWanderUpdateInterval : 0f);
		}

		public void UpdateIdleWanderUpdateInterval()
		{
			if (PirateConfiguration == null)
			{
				IdleWanderUpdateInterval = 0f;
				return;
			}
			float minInclusive = Mathf.Min(PirateConfiguration.IdleMinWanderPositionUpdateTime, PirateConfiguration.IdleMaxWanderPositionUpdateTime);
			float maxInclusive = Mathf.Max(PirateConfiguration.IdleMinWanderPositionUpdateTime, PirateConfiguration.IdleMaxWanderPositionUpdateTime);
			IdleWanderUpdateInterval = Random.Range(minInclusive, maxInclusive);
		}

		public bool TryGetIdleWanderPosition(out Vector3 position)
		{
			position = Vector3.zero;
			if (PirateConfiguration == null || _navigationService == null)
			{
				return false;
			}
			Vector3 center = (_hasAreaPosition ? AreaPosition : base.transform.position);
			int attempts = Mathf.Max(1, PirateConfiguration.IdleWanderSearchAttempts);
			return _navigationService.TryGetRandomSafeNavmeshPosition(center, PirateConfiguration.IdleWanderRadius, attempts, out position);
		}

		public void RegisterIdleWanderTargetPosition(Vector3 position)
		{
			_targetPositionsModel?.UpdateEnemyTargetPosition(_enemyType, base.gameObject.GetHashCode(), position);
		}

		public bool IsTargetAlive()
		{
			if (TargetKind == PirateTargetKind.Aux)
			{
				return HasAuxTarget;
			}
			if (TargetPlayer == PlayerRef.None)
			{
				return false;
			}
			return IsPlayerAlive(TargetPlayer.PlayerId);
		}

		public bool IsPlayerVisible(PlayerRef player)
		{
			DetectionType detectionType;
			if (TargetDetector != null)
			{
				return TargetDetector.IsPlayerDetected(player, out detectionType);
			}
			return false;
		}

		public bool ObserveTargetIfVisible()
		{
			if (TargetKind == PirateTargetKind.None || !IsTargetVisible())
			{
				return false;
			}
			Vector3 targetPosition;
			bool flag = TryGetTargetTrackingPosition(out targetPosition);
			if (flag)
			{
				UpdateLastKnownTargetDirection(targetPosition);
				LastKnownTargetPosition = targetPosition;
				HasLastKnownTargetPosition = true;
			}
			bool flag2 = IsTargetInSafeZone();
			if (_hasObservedTargetSafeZoneState && !_wasObservedTargetInSafeZone && flag2)
			{
				HasObservedTargetEnterSafeZone = true;
			}
			else if (!flag2)
			{
				HasObservedTargetEnterSafeZone = false;
				_hasRecentOutsideSafeZoneObservation = true;
				_lastOutsideSafeZoneObservationTime = Time.time;
				_lastOutsideSafeZoneObservationPosition = (flag ? targetPosition : LastKnownTargetPosition);
			}
			_wasObservedTargetInSafeZone = flag2;
			_hasObservedTargetSafeZoneState = true;
			return true;
		}

		public bool TryMarkTargetSafeZoneEntryFromRecentObservation()
		{
			if (!IsTargetInSafeZone())
			{
				return false;
			}
			if (HasObservedTargetEnterSafeZone)
			{
				return false;
			}
			if (PirateConfiguration == null || !_hasRecentOutsideSafeZoneObservation)
			{
				return false;
			}
			if (Time.time - _lastOutsideSafeZoneObservationTime > PirateConfiguration.SafeZoneObservedEntryGraceTime)
			{
				return false;
			}
			LastKnownTargetPosition = _lastOutsideSafeZoneObservationPosition;
			HasLastKnownTargetPosition = true;
			HasObservedTargetEnterSafeZone = true;
			_hasObservedTargetSafeZoneState = true;
			_wasObservedTargetInSafeZone = true;
			return true;
		}

		public void ClearTargetObservation()
		{
			ReleaseSafeZoneAttackPoint();
			HasLastKnownTargetPosition = false;
			LastKnownTargetPosition = Vector3.zero;
			HasLastKnownTargetDirection = false;
			LastKnownTargetDirection = Vector3.zero;
			HasObservedTargetEnterSafeZone = false;
			_hasObservedTargetSafeZoneState = false;
			_wasObservedTargetInSafeZone = false;
			_hasRecentOutsideSafeZoneObservation = false;
			_lastOutsideSafeZoneObservationTime = 0f;
			_lastOutsideSafeZoneObservationPosition = Vector3.zero;
			ResetChaseTargetLostTimer();
		}

		public void ResetChaseTargetLostTimer()
		{
			ChaseTargetLostTimer = 0f;
		}

		public bool IncreaseChaseTargetLostTimer(float deltaTime, float lostTargetTime)
		{
			ChaseTargetLostTimer += deltaTime;
			return ChaseTargetLostTimer >= lostTargetTime;
		}

		public bool IsPlayerAlive(int playerId)
		{
			if (_playerStateService.IsPlayerDead(playerId))
			{
				return false;
			}
			if (_playerStateModel.TryGetState(playerId, out var state) && state == PlayerState.PreDeadCrouch)
			{
				return false;
			}
			return true;
		}

		public void RefreshTargetBeachState()
		{
			IsTargetOnBeach = TargetPlayer != PlayerRef.None && !IsPlayerInsideGate(TargetPlayer.PlayerId);
		}

		public bool HasOtherAlivePlayersInsideGate()
		{
			foreach (PlayerRef key in _playerMovableModel.AllCharacterMovables.Keys)
			{
				if (!(key == TargetPlayer) && IsPlayerAlive(key.PlayerId) && IsPlayerInsideGate(key.PlayerId))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryFindClosestAlivePlayerInsideGate(Vector3 position, out PlayerRef closestPlayer)
		{
			closestPlayer = PlayerRef.None;
			float num = float.PositiveInfinity;
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				PlayerRef key = allCharacterMovable.Key;
				PlayerCharacterMovableBase value = allCharacterMovable.Value;
				if (!(value == null) && IsPlayerAlive(key.PlayerId) && IsPlayerInsideGate(key.PlayerId))
				{
					float sqrMagnitude = (value.transform.position - position).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						closestPlayer = key;
					}
				}
			}
			return closestPlayer != PlayerRef.None;
		}

		public bool TryGetInvestigationSearchPosition(out Vector3 position)
		{
			position = Vector3.zero;
			if (!HasLastKnownTargetPosition || PirateConfiguration == null || _navigationService == null)
			{
				return false;
			}
			int attempts = Mathf.Max(1, PirateConfiguration.InvestigationSearchAttempts);
			if (HasLastKnownTargetDirection && PirateConfiguration.InvestigationDirectionSearchDistance > 0f)
			{
				Vector3 center = LastKnownTargetPosition + LastKnownTargetDirection * PirateConfiguration.InvestigationDirectionSearchDistance;
				if (_navigationService.TryGetRandomSafeNavmeshPosition(center, PirateConfiguration.InvestigationDirectionSearchRadius, attempts, out position))
				{
					return true;
				}
			}
			return _navigationService.TryGetRandomSafeNavmeshPosition(LastKnownTargetPosition, PirateConfiguration.InvestigationSearchRadius, attempts, out position);
		}

		private void UpdateLastKnownTargetDirection(Vector3 trackingPosition)
		{
			if (!(PirateConfiguration == null))
			{
				float num = Mathf.Max(0f, PirateConfiguration.InvestigationDirectionMinSpeed);
				float minMagnitudeSqr = num * num;
				if (TryGetTargetVelocityDirection(minMagnitudeSqr, out var direction) || TryGetTargetPositionDeltaDirection(trackingPosition, minMagnitudeSqr, out direction))
				{
					LastKnownTargetDirection = direction;
					HasLastKnownTargetDirection = true;
				}
			}
		}

		private bool TryGetTargetVelocityDirection(float minMagnitudeSqr, out Vector3 direction)
		{
			direction = Vector3.zero;
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(TargetPlayer, out var value) || value == null)
			{
				return false;
			}
			Vector3 velocity = value.GetVelocity();
			velocity.y = 0f;
			if (velocity.sqrMagnitude < minMagnitudeSqr)
			{
				return false;
			}
			direction = velocity.normalized;
			return true;
		}

		private bool TryGetTargetPositionDeltaDirection(Vector3 trackingPosition, float minMagnitudeSqr, out Vector3 direction)
		{
			direction = Vector3.zero;
			if (!HasLastKnownTargetPosition)
			{
				return false;
			}
			Vector3 vector = trackingPosition - LastKnownTargetPosition;
			vector.y = 0f;
			if (vector.sqrMagnitude < minMagnitudeSqr)
			{
				return false;
			}
			direction = vector.normalized;
			return true;
		}

		private bool IsPlayerInsideGate(int playerId)
		{
			if (_playersGatesModel != null && _playersGatesModel.TryGetPlayerState(playerId, out var state))
			{
				return state.PlayerInsideGate;
			}
			return false;
		}

		public bool IsObstacleInShootDirection()
		{
			if (!TryGetTargetPosition(out var targetPosition))
			{
				return true;
			}
			Vector3 vector = targetPosition - PirateAttackController.SphereCastPosition.position;
			float magnitude = vector.magnitude;
			vector = vector.normalized;
			if (Physics.SphereCast(PirateAttackController.SphereCastPosition.position, PirateAttackController.SphereCastRadius, vector.normalized, out var _, magnitude, ObstacleLayerMask))
			{
				return true;
			}
			return false;
		}

		public bool TryPrepareSafeZoneAttackPosition()
		{
			ReleaseSafeZoneAttackPoint();
			if (TargetKind != PirateTargetKind.Player)
			{
				return false;
			}
			if (PirateConfiguration == null)
			{
				return false;
			}
			if (!PirateConfiguration.IsSafeZoneAttackEnabled)
			{
				return false;
			}
			if (!HasObservedTargetEnterSafeZone)
			{
				return false;
			}
			if (PirateSafeZoneDetector == null)
			{
				return false;
			}
			if (!TryGetTargetPosition(out var targetPosition))
			{
				return false;
			}
			if (!PirateSafeZoneDetector.TryFindBlockingSafeZoneUnderPlayer(targetPosition, out var safeZone))
			{
				return false;
			}
			if (!CanUseBottomSafeZoneAttack(safeZone))
			{
				return false;
			}
			if (EnemyMovableBase == null)
			{
				return false;
			}
			if (!TryOccupyClosestSafeZoneAttackPoint(safeZone, out var attackPosition))
			{
				return false;
			}
			SafeZoneAttackPosition = attackPosition;
			HasSafeZoneAttackPosition = true;
			return true;
		}

		public void ReleaseSafeZoneAttackPoint()
		{
			_safeZoneInteractionPointsModel?.ReleaseOccupiedByOwner(GetSafeZoneAttackOwnerType(), GetSafeZoneAttackOwnerInstanceId());
			SafeZoneAttackPosition = Vector3.zero;
			HasSafeZoneAttackPosition = false;
		}

		private bool TryOccupyClosestSafeZoneAttackPoint(PlayerSafeZone safeZone, out Vector3 attackPosition)
		{
			attackPosition = Vector3.zero;
			if (safeZone == null || safeZone.InteractionPoints == null || safeZone.InteractionPoints.Count == 0)
			{
				return false;
			}
			_safeZoneInteractionPointCandidates.Clear();
			for (int i = 0; i < safeZone.InteractionPoints.Count; i++)
			{
				Transform transform = safeZone.InteractionPoints[i];
				if (transform != null)
				{
					_safeZoneInteractionPointCandidates.Add(transform);
				}
			}
			_safeZoneInteractionPointCandidates.Sort(CompareSafeZoneInteractionPointsByDistance);
			int safeZoneAttackOwnerType = GetSafeZoneAttackOwnerType();
			int safeZoneAttackOwnerInstanceId = GetSafeZoneAttackOwnerInstanceId();
			for (int j = 0; j < _safeZoneInteractionPointCandidates.Count; j++)
			{
				Transform transform2 = _safeZoneInteractionPointCandidates[j];
				if (_safeZoneInteractionPointsModel == null || !_safeZoneInteractionPointsModel.IsOccupiedByAnother(transform2, safeZoneAttackOwnerType, safeZoneAttackOwnerInstanceId))
				{
					Vector3 position = transform2.position;
					if (EnemyMovableBase.IsCanMoveToPoint(position) && (_safeZoneInteractionPointsModel == null || _safeZoneInteractionPointsModel.TryOccupy(transform2, safeZoneAttackOwnerType, safeZoneAttackOwnerInstanceId)))
					{
						attackPosition = position;
						return true;
					}
				}
			}
			return false;
		}

		private int CompareSafeZoneInteractionPointsByDistance(Transform left, Transform right)
		{
			float sqrMagnitude = (left.position - base.transform.position).sqrMagnitude;
			float sqrMagnitude2 = (right.position - base.transform.position).sqrMagnitude;
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		}

		private int GetSafeZoneAttackOwnerType()
		{
			return (int)_enemyType;
		}

		private int GetSafeZoneAttackOwnerInstanceId()
		{
			return base.gameObject.GetInstanceID();
		}

		public bool IsTargetInSafeZone()
		{
			if (!TryGetTargetPosition(out var targetPosition))
			{
				return false;
			}
			return IsPositionInSafeZone(targetPosition);
		}

		public bool IsPositionInSafeZone(Vector3 position)
		{
			if (PirateSafeZoneDetector != null)
			{
				return PirateSafeZoneDetector.IsPlayerInSafeZone(position);
			}
			return false;
		}

		public bool CanUseBottomSafeZoneAttack(PlayerSafeZone safeZone)
		{
			if (safeZone != null)
			{
				return safeZone.SafeZoneType != SafeZoneType.Closet;
			}
			return false;
		}

		public bool TryGetTargetBlockingSafeZone(out PlayerSafeZone safeZone)
		{
			safeZone = null;
			if (!TryGetTargetPosition(out var targetPosition) || PirateSafeZoneDetector == null)
			{
				return false;
			}
			return PirateSafeZoneDetector.TryFindBlockingSafeZoneUnderPlayer(targetPosition, out safeZone);
		}

		public bool ShouldPreserveCombatForObservedSafeZoneEntry()
		{
			if (!HasObservedTargetEnterSafeZone || !IsTargetInSafeZone())
			{
				return false;
			}
			if (TryGetTargetBlockingSafeZone(out var safeZone) && safeZone.SafeZoneType == SafeZoneType.Closet)
			{
				return false;
			}
			return true;
		}

		public bool CanAttackTargetWithLineOfSight()
		{
			if (IsTargetVisible())
			{
				return !IsObstacleInShootDirection();
			}
			return false;
		}

		public bool TryGetTargetPosition(out Vector3 targetPosition)
		{
			targetPosition = Vector3.zero;
			if (TargetKind == PirateTargetKind.Aux)
			{
				if (!HasAuxTarget)
				{
					return false;
				}
				targetPosition = _auxTarget.Transform.position + Vector3.up * ((PirateConfiguration != null) ? PirateConfiguration.AuxTargetAimHeight : 0f);
				return true;
			}
			if (TargetPlayer == PlayerRef.None)
			{
				return false;
			}
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(TargetPlayer, out var value) || value == null)
			{
				return false;
			}
			targetPosition = value.CameraPositionTransform.position;
			return true;
		}

		public bool TryGetTargetTrackingPosition(out Vector3 targetPosition)
		{
			targetPosition = Vector3.zero;
			if (TargetKind == PirateTargetKind.Aux)
			{
				return TryGetTargetMovePosition(out targetPosition);
			}
			if (TargetPlayer == PlayerRef.None)
			{
				return false;
			}
			if (_navigationService != null && _navigationService.TryGetPlayerTrackingPosition(TargetPlayer, out targetPosition))
			{
				return true;
			}
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(TargetPlayer, out var value) || value == null)
			{
				return false;
			}
			targetPosition = value.transform.position;
			return true;
		}
	}
}

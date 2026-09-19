using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Features.AIModule.Scripts.HeadCrab;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.CustomSynchronizersModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.Extensions;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.LevelGatesModule.Data;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.TeleportModule.Scripts.TeleportCommon;
using Fusion;
using NetworkServices.ObjectsProvider;
using PlayerCustomization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting;
using UnityEngine.VFX;
using Zenject;

namespace Features.AIModule.Scripts.ManInShadows
{
	[NetworkBehaviourWeaved(3)]
	public class ManInShadowsBehaviour : EnemyBehaviourBase<EnemyState>, ITeleportable, IEnemyFearCallbackListener, IEnemyStateProvider
	{
		[SerializeField]
		private VisualEffect _soulAttract;

		[SerializeField]
		private Transform _visionPoint;

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[SerializeField]
		private float _thresholdAngle = 10f;

		[SerializeField]
		private Transform _visual;

		[SerializeField]
		private float _chaseSpeed = 4f;

		[SerializeField]
		private float _wanderingChangePointTimerValue = 0.5f;

		[SerializeField]
		private LayerMask _obstacleMask;

		[SerializeField]
		private float _raycastLateralOffset = 0.4f;

		[SerializeField]
		private PhysGrabber _physGrabber;

		[SerializeField]
		private float _attackDuration = 3f;

		[SerializeField]
		private float _afterAttackDuration = 4f;

		[SerializeField]
		private float _attackRange = 1.5f;

		[SerializeField]
		private float _despawnTimeValue = 30f;

		[SerializeField]
		private float _attackDamage = 100f;

		[SerializeField]
		private float _distanceVisibility = 30f;

		[SerializeField]
		private float _maxAttackDistance = 3f;

		[SerializeField]
		private SimpleEnemyHealthController _simpleEnemyHealthController;

		[SerializeField]
		private SimpleEnemyDamageable _simpleEnemyDamageable;

		[SerializeField]
		private float _teleportBehindDistance = 4f;

		[SerializeField]
		private float _teleportBehindSampleRadius = 2f;

		[SerializeField]
		private float _afterDamageTeleportIdleDuration = 2f;

		[SerializeField]
		private float _teleportBehindMinJumpDistance = 2f;

		[SerializeField]
		private NetworkedAnimationControllerBase _networkedAnimationControllerBase;

		[SerializeField]
		private EventReference _moveReference;

		[SerializeField]
		private EventReference _localVisibilitySoundReference;

		[SerializeField]
		private EventReference _attackReference;

		[SerializeField]
		private EventReference _teleportReference;

		[SerializeField]
		private EventReference _hitReference;

		[SerializeField]
		private string _voiceOcclusionLowPassParameterName = "VoiceOcclusionLowPass";

		[SerializeField]
		private LayerMask _occlusionLayerMask;

		[SerializeField]
		private float _occlusionMaxDistance;

		[SerializeField]
		private float _verticalDistanceMultiplier = 3f;

		[SerializeField]
		private float _verticalFalloffExponent = 2f;

		[SerializeField]
		private float _lowPassMinValue;

		[SerializeField]
		private EnemySafeZoneAttackDetector _enemySafeZoneAttackDetector;

		[SerializeField]
		private bool _isFlickerEnabled = true;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private PhysicsSynchronizer _physicsSynchronizer;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private IPlayerStateService _playerStateService;

		private PlayerMovableModel _playerMovableModel;

		private ManInShadowsPointsModel _manInShadowsPointsModel;

		private Vector3 _localVisualPosition;

		private bool _isVisualPositionStored;

		private bool _isLocalCloseEnough;

		private bool _isSpawned;

		private float _wanderingChangePointTimer;

		private bool _isFeared;

		private float _despawnTimer;

		private bool _screamerFinishedOnAuthority;

		private bool _isAttackRoutineRunning;

		private Coroutine _attackCoroutine;

		private int _blockedPlayerId = -1;

		private float _afterDamageTeleportIdleTimer;

		private float _localVisionSuppressTimer;

		private const float AFTER_TELEPORT_VISION_SUPPRESS_DURATION = 0.5f;

		private EnemyState _lastAppliedAnimState;

		private PlayerGrabSimplePointGrabableModel _playerGrabSimplePointGrabableModel;

		private PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private PlayerCustomizationModel _playerCustomizationModel;

		private LineArmsModel _lineArmsModel;

		private EventInstance _moveSoundInstance;

		private EventInstance _localVisibilitySoundInstance;

		private IAudioService _audioService;

		private IManInShadowFlickerAdjustingService _manInShadowFlickerService;

		private PlayerEnemyInteractionBlocksModel _playerEnemyInteractionBlocksModel;

		private CauldronStealthModel _cauldronStealthModel;

		private LocalBodyHideSuspensionModel _localBodyHideSuspensionModel;

		private PlayersRagdollModel _playersRagdollModel;

		private bool _isSuspendingLocalBodyHide;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedCurrentEnemyState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private EnemyState _NetworkedCurrentEnemyState;

		[WeaverGenerated]
		[DefaultForProperty("IsPlayerGrabbed", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsPlayerGrabbed;

		[WeaverGenerated]
		[DefaultForProperty("TargetPlayerRef", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private PlayerRef _TargetPlayerRef;

		public override EnemyType EnemyType => EnemyType.ManInShadows;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe EnemyState NetworkedCurrentEnemyState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ManInShadowsBehaviour.NetworkedCurrentEnemyState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(EnemyState*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ManInShadowsBehaviour.NetworkedCurrentEnemyState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(EnemyState*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe bool IsPlayerGrabbed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ManInShadowsBehaviour.IsPlayerGrabbed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ManInShadowsBehaviour.IsPlayerGrabbed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe PlayerRef TargetPlayerRef
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ManInShadowsBehaviour.TargetPlayerRef. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(PlayerRef*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ManInShadowsBehaviour.TargetPlayerRef. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(PlayerRef*)(Ptr + 2) = value;
			}
		}

		EnemyState IEnemyStateProvider.CurrentState => base.CurrentEnemyState;

		protected override EnemyState ReactivationState => EnemyState.Wandering;

		[Inject]
		private void InjectDependencies(EnemyFearListenerModel enemyFearListenerModel, IPlayerStateService playerStateService, PlayerMovableModel playerMovableModel, ManInShadowsPointsModel manInShadowsPointsModel, PlayerGrabSimplePointGrabableModel playerGrabSimplePointGrabableModel, PlayerDamageablesTrackModel playerDamageablesTrackModel, LineArmsModel lineArmsModel, PlayerCustomizationModel playerCustomizationModel, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel, IManInShadowFlickerAdjustingService manInShadowIManInShadowFlickerAdjustingService, PlayerEnemyInteractionBlocksModel playerEnemyInteractionBlocksModel, IAudioService audioService, CauldronStealthModel cauldronStealthModel, LocalBodyHideSuspensionModel localBodyHideSuspensionModel, PlayersRagdollModel playersRagdollModel)
		{
			_enemyFearListenerModel = enemyFearListenerModel;
			_playerStateService = playerStateService;
			_playerMovableModel = playerMovableModel;
			_manInShadowsPointsModel = manInShadowsPointsModel;
			_playerGrabSimplePointGrabableModel = playerGrabSimplePointGrabableModel;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_lineArmsModel = lineArmsModel;
			_playerCustomizationModel = playerCustomizationModel;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
			_manInShadowFlickerService = manInShadowIManInShadowFlickerAdjustingService;
			_playerEnemyInteractionBlocksModel = playerEnemyInteractionBlocksModel;
			_audioService = audioService;
			_cauldronStealthModel = cauldronStealthModel;
			_localBodyHideSuspensionModel = localBodyHideSuspensionModel;
			_playersRagdollModel = playersRagdollModel;
		}

		public override void Spawned()
		{
			_moveSoundInstance = _audioService.CreateInstance(_moveReference);
			_audioService.StartInstanceWith3DAttributes(_moveSoundInstance, _soundSourceBehaviour);
			if (!_localVisibilitySoundReference.IsNull)
			{
				_localVisibilitySoundInstance = _audioService.CreateInstance(_localVisibilitySoundReference);
				_audioService.StartInstanceWith3DAttributes(_localVisibilitySoundInstance, _soundSourceBehaviour);
			}
			_soulAttract.Stop();
			_simpleEnemyDamageable.OnDamaged += OnEnemyDamaged;
			_enemyFearListenerModel.RegisterFearListener(EnemyType.HeadCrab, base.gameObject.GetHashCode(), this);
			if (_isFlickerEnabled)
			{
				_manInShadowFlickerService.RegisterManInShadow(base.gameObject);
			}
			base.Spawned();
			_isSpawned = true;
			if (!base.HasStateAuthority)
			{
				_navMeshAgent.enabled = false;
				return;
			}
			_isSpawned = true;
			_navMeshAgent.Warp(base.transform.position);
			ChangeEnemyState(EnemyState.Wandering);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_audioService.StopInstance(_moveSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(_moveSoundInstance);
			if (!_localVisibilitySoundReference.IsNull)
			{
				_audioService.StopInstance(_localVisibilitySoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(_localVisibilitySoundInstance);
			}
			_simpleEnemyDamageable.OnDamaged -= OnEnemyDamaged;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType.HeadCrab, base.gameObject.GetHashCode());
			if (_isFlickerEnabled)
			{
				_manInShadowFlickerService.UnregisterManInShadow(base.gameObject);
			}
			base.Despawned(runner, hasState);
			_isSpawned = false;
			if (_isSuspendingLocalBodyHide)
			{
				_isSuspendingLocalBodyHide = false;
				_localBodyHideSuspensionModel.IsSuspended = false;
			}
			if (base.HasStateAuthority)
			{
				if (IsPlayerGrabbed)
				{
					UngrabPlayer();
				}
				else if (TargetPlayerRef != PlayerRef.None)
				{
					ReleaseHeldPlayerState();
				}
				PositionOnFearEnd = base.transform.position;
				FearCompleted = true;
				TargetPlayerRef = PlayerRef.None;
				_isVisualPositionStored = false;
				_isLocalCloseEnough = false;
				_wanderingChangePointTimer = 0f;
				_isFeared = false;
				IsPlayerGrabbed = false;
				_despawnTimer = 0f;
				_afterDamageTeleportIdleTimer = 0f;
				_localVisionSuppressTimer = 0f;
			}
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();
			if (base.HasStateAuthority)
			{
				ReconcileInterruptedAttack();
			}
			if (_isFeared && base.CurrentEnemyState != EnemyState.Fear && base.CurrentEnemyState != EnemyState.PlayerAttacking)
			{
				ChangeEnemyState(EnemyState.Fear);
			}
		}

		protected override void OnAuthorityGained()
		{
			_navMeshAgent.enabled = true;
			_navMeshAgent.Warp(base.transform.position);
			RecoverFromInterruptedAttack();
		}

		public void FixedUpdate()
		{
			if (_isSpawned)
			{
				ProcessSoundOcclusion(_moveSoundInstance);
				_moveSoundInstance.set3DAttributes(_soundSourceBehaviour.SoundSourceTransform.To3DAttributes());
				ProcessLocalVisibilityEffects();
				ProcessAttackEffect();
				if (base.HasStateAuthority)
				{
					NetworkedCurrentEnemyState = base.CurrentEnemyState;
				}
				ReconcileAttackAnimation();
				ProcessLocalVision();
			}
		}

		private void ReconcileAttackAnimation()
		{
			if (NetworkedCurrentEnemyState != _lastAppliedAnimState)
			{
				bool flag = _lastAppliedAnimState == EnemyState.PlayerAttacking;
				bool flag2 = NetworkedCurrentEnemyState == EnemyState.PlayerAttacking;
				_lastAppliedAnimState = NetworkedCurrentEnemyState;
				if (flag2 && !flag)
				{
					_networkedAnimationControllerBase.PlayAnimationLocal(AnimationType.Grab);
				}
				else if (!flag2 && flag)
				{
					_networkedAnimationControllerBase.ResetAnimationLocal(AnimationType.Grab);
					_networkedAnimationControllerBase.PlayAnimationLocal(AnimationType.UnGrab);
				}
			}
		}

		private void ProcessAttackEffect()
		{
			if (!IsPlayerGrabbed)
			{
				_soulAttract.Stop();
				return;
			}
			if (!_soulAttract.HasAnySystemAwake())
			{
				PlaySound(_attackReference);
				_soulAttract.Play();
			}
			Vector3 position = base.transform.position;
			if (_playerMovableModel.AllCharacterMovables.TryGetValue(TargetPlayerRef, out var value) && value != null && value.CameraPositionTransform != null)
			{
				position = value.CameraPositionTransform.position;
			}
			_soulAttract.SetVector3("PlayerPosition", position);
			foreach (PlayerCustomizationSlotData slot in _playerCustomizationModel.Slots)
			{
				if (slot.PlayerId == TargetPlayerRef.PlayerId)
				{
					_soulAttract.SetVector4("PlayerColor", slot.PrimaryColor);
				}
			}
		}

		private void PlaySound(EventReference eventReference)
		{
			EventInstance eventInstance = _audioService.CreateInstance(eventReference);
			ProcessSoundOcclusion(eventInstance);
			_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSourceBehaviour);
			_audioService.ReleaseInstance(eventInstance);
		}

		private void PlaySound(EventReference eventReference, Vector3 position)
		{
			EventInstance eventInstance = _audioService.CreateInstance(eventReference);
			ProcessSoundOcclusion(eventInstance);
			_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSourceBehaviour);
			_audioService.ReleaseInstance(eventInstance);
		}

		private void LateUpdate()
		{
			if (_isSpawned)
			{
				ProcessLocalVisionPosition();
				ProcessLocalHeldBodyVisibility();
			}
		}

		private void ProcessLocalHeldBodyVisibility()
		{
			bool flag = IsPlayerGrabbed && TargetPlayerRef == base.Runner.LocalPlayer;
			if (!flag && _isSuspendingLocalBodyHide)
			{
				flag = IsLocalRagdollSimulated();
			}
			if (flag != _isSuspendingLocalBodyHide)
			{
				_isSuspendingLocalBodyHide = flag;
				_localBodyHideSuspensionModel.IsSuspended = flag;
			}
		}

		private bool IsLocalRagdollSimulated()
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(base.Runner.LocalPlayer.PlayerId, out var ragdoll))
			{
				return ragdoll.IsSimulated;
			}
			return false;
		}

		private void ProcessLocalVisionPosition()
		{
			if (_isLocalCloseEnough && !IsPlayerDead(base.Runner.LocalPlayer.PlayerId) && !IsPlayerGrabbed)
			{
				_visual.position = _localVisualPosition;
			}
			else
			{
				_visual.localPosition = Vector3.zero;
			}
		}

		private void ProcessLocalVision()
		{
			PlayerCharacterMovableBase value;
			if (_localVisionSuppressTimer > 0f)
			{
				_localVisionSuppressTimer -= Time.deltaTime;
				_isLocalCloseEnough = false;
				_isVisualPositionStored = false;
			}
			else if (NetworkedCurrentEnemyState != EnemyState.Chasing)
			{
				_isLocalCloseEnough = false;
			}
			else if (_playerMovableModel.AllCharacterMovables.TryGetValue(base.Runner.LocalPlayer, out value))
			{
				_isLocalCloseEnough = CheckPlayerPointVisibility(_visionPoint.position, value);
				if (!_isLocalCloseEnough)
				{
					_isVisualPositionStored = false;
				}
				else if (!_isVisualPositionStored)
				{
					_isVisualPositionStored = true;
					_localVisualPosition = _visual.position;
				}
			}
		}

		private void ProcessLocalVisibilityEffects()
		{
			float b = (CheckAllPlayersPointVisibility(_visionPoint.position) ? 0f : 1f);
			if (_moveSoundInstance.getVolume(out var volume) == RESULT.OK)
			{
				float num = Mathf.Lerp(volume, b, Time.deltaTime * 10f);
				_moveSoundInstance.setVolume(num);
				if (!_localVisibilitySoundReference.IsNull)
				{
					_localVisibilitySoundInstance.set3DAttributes(base.transform.To3DAttributes());
					_localVisibilitySoundInstance.setVolume(num);
				}
				if (_isFlickerEnabled)
				{
					_manInShadowFlickerService.SetInstanceBlend(base.gameObject, num);
				}
			}
		}

		public void Fear()
		{
			_isFeared = true;
		}

		protected override void HandleCurrentEnemyState(EnemyState currentEnemyState)
		{
			switch (currentEnemyState)
			{
			case EnemyState.Wandering:
				HandleWanderingState();
				break;
			case EnemyState.Chasing:
				HandleStateChase();
				break;
			case EnemyState.RunAway:
				TeleportToRandomPoint(checkPlayersVisibility: false);
				ChangeEnemyState(EnemyState.Wandering);
				break;
			case EnemyState.Stun:
				HandleAfterTeleportIdle();
				break;
			case EnemyState.Fear:
				HandleFear();
				break;
			}
		}

		private void HandleFear()
		{
			if (_despawnTimer < _despawnTimeValue)
			{
				_despawnTimer += Time.deltaTime;
				if (!CheckAllPlayersPointVisibility(_visionPoint.position))
				{
					_simpleEnemyHealthController.IsNeedToSpawnItem = false;
					if (IsDespawnAfterFear)
					{
						base.Object.DespawnHierarchy();
						return;
					}
					_isFeared = false;
					FearCompleted = true;
					ChangeEnemyState(EnemyState.Wandering);
				}
			}
			else
			{
				_simpleEnemyHealthController.IsNeedToSpawnItem = false;
				if (IsDespawnAfterFear)
				{
					base.Object.DespawnHierarchy();
					return;
				}
				_isFeared = false;
				FearCompleted = true;
				ChangeEnemyState(EnemyState.Wandering);
			}
		}

		protected override void ReactOnEnemyStateChanged(EnemyState newState)
		{
			switch (newState)
			{
			case EnemyState.Wandering:
				HandleWanderingStateEntered();
				break;
			case EnemyState.PlayerAttacking:
				_attackCoroutine = StartCoroutine(PlayerAttack());
				break;
			case EnemyState.Chasing:
			case EnemyState.Attacking:
			case EnemyState.RunAway:
				break;
			}
		}

		protected override void ReactOnPlayerDetected(PlayerRef player, EnemyState currentEnemyState)
		{
			if (currentEnemyState == EnemyState.Wandering && player.IsActiveInSession(base.Runner) && !IsPlayerDead(player.PlayerId) && !_cauldronStealthModel.IsStealthed(player.PlayerId))
			{
				TargetPlayerRef = player;
				ChangeEnemyState(EnemyState.Chasing);
			}
		}

		protected override void ReactOnPlayerStateChanged(PlayerStateData playerStateData, EnemyState currentEnemyState)
		{
		}

		public void Teleport(Vector3 position)
		{
			PlatTeleportSoundRpc(base.transform.position);
			if (_navMeshAgent != null && _navMeshAgent.enabled && _navMeshAgent.Warp(position))
			{
				ResetPath();
				_navMeshAgent.velocity = Vector3.zero;
				_navMeshAgent.isStopped = true;
			}
			else
			{
				base.transform.position = position;
			}
		}

		private bool CheckAllPlayersPointVisibility(Vector3 point)
		{
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (CheckPlayerPointVisibility(point, allCharacterMovable.Value))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheckPlayerPointVisibility(Vector3 point, PlayerCharacterMovableBase movable)
		{
			if (movable == null || movable.Object == null || movable.CameraPositionTransform == null)
			{
				return false;
			}
			if (IsPlayerDead(movable.Object.InputAuthority.PlayerId))
			{
				return false;
			}
			Vector3 position = movable.CameraPositionTransform.position;
			Vector3 forward = movable.RotatePoint.forward;
			forward.y = 0f;
			Vector3 vector = point - position;
			vector.y = 0f;
			if (vector.magnitude > _distanceVisibility)
			{
				return false;
			}
			if (Vector3.Angle(forward.normalized, vector.normalized) > _thresholdAngle)
			{
				return false;
			}
			if (!HasClearLineOfSightToPoint(position, point))
			{
				return false;
			}
			return true;
		}

		private bool HasClearLineOfSightToPoint(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.001f)
			{
				return true;
			}
			Vector3 direction = vector / magnitude;
			if (!Physics.Raycast(from, direction, magnitude, _obstacleMask, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			Vector3 vector2 = vector;
			vector2.y = 0f;
			if (vector2.sqrMagnitude <= 0.0001f)
			{
				return false;
			}
			if (_raycastLateralOffset <= 0f)
			{
				return false;
			}
			Vector3 vector3 = Vector3.Cross(Vector3.up, vector2.normalized) * _raycastLateralOffset;
			if (!Physics.Raycast(from - vector3, direction, magnitude, _obstacleMask, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			if (!Physics.Raycast(from + vector3, direction, magnitude, _obstacleMask, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			return false;
		}

		private void OnEnemyDamaged(DamageData damageData)
		{
			if (base.HasStateAuthority && !(_simpleEnemyHealthController.CurrentHealth <= 0f) && !_isFeared && base.CurrentEnemyState != EnemyState.Fear)
			{
				if (_isAttackRoutineRunning || IsPlayerGrabbed || base.CurrentEnemyState == EnemyState.PlayerAttacking)
				{
					RecoverFromInterruptedAttack();
				}
				else
				{
					TargetPlayerRef = PlayerRef.None;
					ResetPath();
				}
				TeleportAfterDamage(damageData.DamageDealerPlayerID);
				_afterDamageTeleportIdleTimer = 0f;
				ChangeEnemyState(EnemyState.Stun);
			}
		}

		private void HandleAfterTeleportIdle()
		{
			if (_navMeshAgent.enabled)
			{
				_navMeshAgent.velocity = Vector3.zero;
				_navMeshAgent.isStopped = true;
			}
			_afterDamageTeleportIdleTimer += Time.deltaTime;
			if (!(_afterDamageTeleportIdleTimer < _afterDamageTeleportIdleDuration))
			{
				_afterDamageTeleportIdleTimer = 0f;
				if (TryGetNearestAlivePlayer(out var nearestPlayerRef))
				{
					TargetPlayerRef = nearestPlayerRef;
					ChangeEnemyState(EnemyState.Chasing);
				}
				else
				{
					ChangeEnemyState(EnemyState.Wandering);
				}
			}
		}

		private bool TryGetNearestAlivePlayer(out PlayerRef nearestPlayerRef)
		{
			nearestPlayerRef = PlayerRef.None;
			float num = float.MaxValue;
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				PlayerCharacterMovableBase value = allCharacterMovable.Value;
				if (!(value == null) && !(value.Object == null) && allCharacterMovable.Key.IsActiveInSession(base.Runner) && !IsPlayerDead(allCharacterMovable.Key.PlayerId) && !_playerEnemyInteractionBlocksModel.IsBlocked(allCharacterMovable.Key.PlayerId))
				{
					float sqrMagnitude = (value.transform.position - base.transform.position).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						nearestPlayerRef = allCharacterMovable.Key;
					}
				}
			}
			return nearestPlayerRef != PlayerRef.None;
		}

		private void TeleportAfterDamage(int damageDealerPlayerId)
		{
			if (TryTeleportBehindPlayer(damageDealerPlayerId) || TryTeleportBehindPlayer(-1))
			{
				return;
			}
			List<Transform> list = new List<Transform>(_manInShadowsPointsModel.Points);
			list.Shuffle();
			foreach (Transform item in list)
			{
				if (!CheckAllPlayersPointVisibility(item.position))
				{
					TeleportAfterDamageTo(item.position);
					break;
				}
			}
		}

		private void TeleportAfterDamageTo(Vector3 position, Quaternion? rotation = null)
		{
			Teleport(position);
			if (rotation.HasValue)
			{
				base.transform.rotation = rotation.Value;
			}
			_physicsSynchronizer.Teleport(position, base.transform.rotation);
			SuppressLocalVisionAfterTeleportRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2999425250u)]
		private void SuppressLocalVisionAfterTeleportRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2999425250u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModule.Scripts.ManInShadows.ManInShadowsBehaviour::SuppressLocalVisionAfterTeleportRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_localVisionSuppressTimer = 0.5f;
			_isLocalCloseEnough = false;
			_isVisualPositionStored = false;
			_visual.localPosition = Vector3.zero;
		}

		private bool TryTeleportBehindPlayer(int excludedPlayerId)
		{
			List<PlayerCharacterMovableBase> list = new List<PlayerCharacterMovableBase>();
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (allCharacterMovable.Key.PlayerId != excludedPlayerId)
				{
					PlayerCharacterMovableBase value = allCharacterMovable.Value;
					if (!(value == null) && !(value.Object == null) && allCharacterMovable.Key.IsActiveInSession(base.Runner) && !IsPlayerDead(allCharacterMovable.Key.PlayerId))
					{
						list.Add(value);
					}
				}
			}
			list.Shuffle();
			foreach (PlayerCharacterMovableBase item in list)
			{
				Vector3 vector = ((item.RotatePoint != null) ? item.RotatePoint.forward : item.transform.forward);
				vector.y = 0f;
				if (!(vector.sqrMagnitude < 0.0001f) && NavMesh.SamplePosition(item.transform.position - vector.normalized * _teleportBehindDistance, out var hit, _teleportBehindSampleRadius, -1) && !(Vector3.Distance(hit.position, base.transform.position) < _teleportBehindMinJumpDistance))
				{
					Vector3 vector2 = item.transform.position - hit.position;
					vector2.y = 0f;
					Quaternion? rotation = ((vector2.sqrMagnitude > 0.0001f) ? new Quaternion?(Quaternion.LookRotation(vector2.normalized)) : ((Quaternion?)null));
					TeleportAfterDamageTo(hit.position, rotation);
					return true;
				}
			}
			return false;
		}

		private void TeleportToRandomPoint(bool checkPlayersVisibility = true)
		{
			if (CheckAllPlayersPointVisibility(_visionPoint.position) && checkPlayersVisibility)
			{
				return;
			}
			List<Transform> list = new List<Transform>(_manInShadowsPointsModel.Points);
			list.Shuffle();
			for (int i = 0; i < list.Count; i++)
			{
				Transform transform = list[i];
				if (!CheckAllPlayersPointVisibility(transform.position))
				{
					Teleport(transform.position);
					break;
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1863879925u)]
		private void PlatTeleportSoundRpc([RpcPayload(12)] Vector3 position)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1863879925u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModule.Scripts.ManInShadows.ManInShadowsBehaviour::PlatTeleportSoundRpc(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(position, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			PlaySound(_teleportReference, position);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 766474609u)]
		private void PlatHitSoundRpc([RpcPayload(12)] Vector3 position)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(766474609u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModule.Scripts.ManInShadows.ManInShadowsBehaviour::PlatHitSoundRpc(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(position, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			PlaySound(_hitReference, position);
		}

		private void HandleStateChase()
		{
			if (TargetPlayerRef == PlayerRef.None || !TargetPlayerRef.IsActiveInSession(base.Runner))
			{
				TargetPlayerRef = PlayerRef.None;
				ResetPath();
				ChangeEnemyState(EnemyState.Wandering);
				return;
			}
			if (IsPlayerStayOutBeach())
			{
				TargetPlayerRef = PlayerRef.None;
				ChangeEnemyState(EnemyState.RunAway);
				ResetPath();
				return;
			}
			if (IsPlayerDead())
			{
				TargetPlayerRef = PlayerRef.None;
				ChangeEnemyState(EnemyState.Wandering);
				ResetPath();
				return;
			}
			bool flag = CheckAllPlayersPointVisibility(_visionPoint.position);
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(TargetPlayerRef, out var value))
			{
				return;
			}
			if (flag)
			{
				_navMeshAgent.speed = 0f;
				if (_navMeshAgent.isOnNavMesh)
				{
					_navMeshAgent.velocity = Vector3.zero;
					_navMeshAgent.isStopped = true;
				}
				return;
			}
			_navMeshAgent.speed = _chaseSpeed;
			if (_navMeshAgent.isOnNavMesh)
			{
				_navMeshAgent.isStopped = false;
				_navMeshAgent.SetDestination(value.transform.position);
			}
			Vector3 position = base.transform.position;
			position.y = 0f;
			Vector3 position2 = value.transform.position;
			position2.y = 0f;
			if (CanAttack(value, position, position2))
			{
				ChangeEnemyState(EnemyState.PlayerAttacking);
			}
		}

		private bool CanAttack(PlayerCharacterMovableBase targetPlayerMovable, Vector3 ownPosition, Vector3 targetPosition)
		{
			if (!_enemySafeZoneAttackDetector.IsPlayerInSafeZone(targetPlayerMovable.CameraPositionTransform.position) && Vector3.Distance(ownPosition, targetPosition) < _attackRange && Mathf.Abs(base.transform.position.y - targetPlayerMovable.transform.position.y) < _maxAttackDistance)
			{
				return HasLineOfSightToTarget(targetPlayerMovable);
			}
			return false;
		}

		private bool HasLineOfSightToTarget(PlayerCharacterMovableBase targetPlayerMovable)
		{
			if (targetPlayerMovable == null || targetPlayerMovable.CameraPositionTransform == null)
			{
				return false;
			}
			Vector3 vector = ((_visionPoint != null) ? _visionPoint.position : base.transform.position);
			Vector3 vector2 = targetPlayerMovable.CameraPositionTransform.position - vector;
			float magnitude = vector2.magnitude;
			if (magnitude <= 0.001f)
			{
				return true;
			}
			return !Physics.Raycast(vector, vector2 / magnitude, magnitude, _obstacleMask, QueryTriggerInteraction.Ignore);
		}

		private void GrabPlayer()
		{
			if (TargetPlayerRef == PlayerRef.None || !_playerGrabSimplePointGrabableModel.PlayerGrabables.TryGetValue(TargetPlayerRef.PlayerId, out var value) || value == null || value.NetworkObject == null)
			{
				return;
			}
			foreach (int item in new List<int>(value.GrabbedByPlayers))
			{
				_lineArmsModel.GetLineArmForPlayer(item).UnJoinAll(throwItem: false);
			}
			value.BlockGrabRPC();
			value.GrabbedByPlayer(base.Runner.LocalPlayer.PlayerId);
			value.InvokeOnGrab();
			value.GrabObject.Grabbers.Add(_physGrabber);
			value.BlockGrabRPC();
			_physGrabber.physGrabPoints.Add(value.GrabObject, value.GetNearestHandle(_physGrabber.physGrabPointPullerPosition.position));
			IsPlayerGrabbed = true;
			MarkPlayerHeldByManInShadow();
		}

		private void UngrabPlayer()
		{
			if (TargetPlayerRef != PlayerRef.None && _playerGrabSimplePointGrabableModel.PlayerGrabables.TryGetValue(TargetPlayerRef.PlayerId, out var value) && value != null && value.NetworkObject != null)
			{
				value.EnableGrabRPC();
				value.UnGrabbedByPlayer(base.Runner.LocalPlayer.PlayerId);
				value.InvokeOnUnGrab();
				value.GrabObject.Grabbers.Remove(_physGrabber);
				value.EnableGrabRPC();
				_physGrabber.physGrabPoints.Remove(value.GrabObject);
			}
			IsPlayerGrabbed = false;
			ReleaseHeldPlayerState();
		}

		private void MarkPlayerHeldByManInShadow()
		{
			if (base.HasStateAuthority && !(TargetPlayerRef == PlayerRef.None))
			{
				_blockedPlayerId = TargetPlayerRef.PlayerId;
				_playerEnemyInteractionBlocksModel.SetBlock(_blockedPlayerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
			}
		}

		private void ReleaseHeldPlayerState()
		{
			if (base.HasStateAuthority && _blockedPlayerId >= 0)
			{
				_playerEnemyInteractionBlocksModel.ClearBlock(_blockedPlayerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
				_blockedPlayerId = -1;
			}
		}

		private void HandleWanderingStateEntered()
		{
			_wanderingChangePointTimer = 0f;
			TeleportToRandomPoint();
		}

		private void HandleWanderingState()
		{
			if (_isFeared)
			{
				ChangeEnemyState(EnemyState.Fear);
			}
			_wanderingChangePointTimer += Time.deltaTime;
			if (!(_wanderingChangePointTimer < _wanderingChangePointTimerValue))
			{
				_wanderingChangePointTimer = 0f;
				TeleportToRandomPoint();
			}
		}

		private IEnumerator PlayerAttack()
		{
			_isAttackRoutineRunning = true;
			try
			{
				GrabPlayer();
				yield return new WaitForSeconds(_attackDuration);
				TryApplyAttackDamage();
				yield return new WaitForSeconds(_afterAttackDuration);
			}
			finally
			{
				ManInShadowsBehaviour manInShadowsBehaviour = this;
				manInShadowsBehaviour.FinishPlayerAttack();
				manInShadowsBehaviour._isAttackRoutineRunning = false;
			}
		}

		private void TryApplyAttackDamage()
		{
			if (TryGetValidTargetDamageable(out var targetDamageable))
			{
				PlatHitSoundRpc(base.transform.position);
				targetDamageable.Damage(new DamageData
				{
					Damage = _attackDamage,
					Force = 0f,
					IsStunning = false,
					Source = DamageDataSourceExtensions.ForEnemyAttack(base.transform, EnemyType.ManInShadows.ToString(), DamageType.Melee)
				});
			}
		}

		private bool TryGetValidTargetDamageable(out IDamageable targetDamageable)
		{
			targetDamageable = null;
			if (TargetPlayerRef == PlayerRef.None || !TargetPlayerRef.IsActiveInSession(base.Runner))
			{
				return false;
			}
			if (!_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(TargetPlayerRef.PlayerId, out targetDamageable))
			{
				return false;
			}
			return targetDamageable.IsValidDamageTarget();
		}

		private void FinishPlayerAttack()
		{
			_attackCoroutine = null;
			int num;
			if (base.Object != null)
			{
				num = (base.Object.IsValid ? 1 : 0);
				if (num != 0 && IsPlayerGrabbed)
				{
					UngrabPlayer();
				}
			}
			else
			{
				num = 0;
			}
			ReleaseHeldPlayerState();
			if (num != 0)
			{
				TargetPlayerRef = PlayerRef.None;
			}
			ResetPath();
			if (base.CurrentEnemyState != EnemyState.Fear)
			{
				EnterWanderingFromAttack();
			}
		}

		private void ReconcileInterruptedAttack()
		{
			if ((_isAttackRoutineRunning || IsPlayerGrabbed || base.CurrentEnemyState == EnemyState.PlayerAttacking || NetworkedCurrentEnemyState == EnemyState.PlayerAttacking) && (!(TargetPlayerRef != PlayerRef.None) || !TargetPlayerRef.IsActiveInSession(base.Runner)))
			{
				RecoverFromInterruptedAttack();
			}
		}

		private void RecoverFromInterruptedAttack()
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				if (_attackCoroutine != null)
				{
					StopCoroutine(_attackCoroutine);
					_attackCoroutine = null;
				}
				_isAttackRoutineRunning = false;
				if (IsPlayerGrabbed)
				{
					UngrabPlayer();
				}
				ReleaseHeldPlayerState();
				IsPlayerGrabbed = false;
				TargetPlayerRef = PlayerRef.None;
				ResetPath();
				EnterWanderingFromAttack();
			}
		}

		private void EnterWanderingFromAttack()
		{
			ChangeEnemyState(EnemyState.Wandering);
			if (base.HasStateAuthority)
			{
				NetworkedCurrentEnemyState = EnemyState.Wandering;
			}
		}

		private void ResetPath()
		{
			if (_navMeshAgent != null && _navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh)
			{
				_navMeshAgent.ResetPath();
			}
		}

		private bool IsPlayerDead()
		{
			if (!_playerStateService.IsPlayerDead(TargetPlayerRef.PlayerId))
			{
				return _playerStateService.GetPlayerState(TargetPlayerRef.PlayerId) == PlayerState.PreDeadCrouch;
			}
			return true;
		}

		private bool IsPlayerStayOutBeach()
		{
			if (_playersGatesModelSynchronizedModel.TryGetPlayerState(TargetPlayerRef.PlayerId, out var state))
			{
				return !state.PlayerInsideGate;
			}
			return false;
		}

		private bool IsPlayerDead(int playerId)
		{
			if (!_playerStateService.IsPlayerDead(playerId))
			{
				return _playerStateService.GetPlayerState(playerId) == PlayerState.PreDeadCrouch;
			}
			return true;
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _verticalDistanceMultiplier;
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
			float t = Mathf.Abs(offset.y) * _verticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _verticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (!(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(_voiceOcclusionLowPassParameterName, out var value);
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, _occlusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					float b = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value3);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			NetworkedCurrentEnemyState = _NetworkedCurrentEnemyState;
			IsPlayerGrabbed = _IsPlayerGrabbed;
			TargetPlayerRef = _TargetPlayerRef;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_NetworkedCurrentEnemyState = NetworkedCurrentEnemyState;
			_IsPlayerGrabbed = IsPlayerGrabbed;
			_TargetPlayerRef = TargetPlayerRef;
		}

		[NetworkRpcWeavedInvoker(2999425250u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SuppressLocalVisionAfterTeleportRpc_0040Invoker2999425250([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ManInShadowsBehaviour)context.TargetBehaviour).SuppressLocalVisionAfterTeleportRpc();
		}

		[NetworkRpcWeavedInvoker(1863879925u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlatTeleportSoundRpc_0040Invoker1863879925([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ManInShadowsBehaviour)context.TargetBehaviour).PlatTeleportSoundRpc(value);
		}

		[NetworkRpcWeavedInvoker(766474609u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlatHitSoundRpc_0040Invoker766474609([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ManInShadowsBehaviour)context.TargetBehaviour).PlatHitSoundRpc(value);
		}
	}
}

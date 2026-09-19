using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.CameraModelModule;
using Features.EmotesModule.Scripts;
using Features.GamePauseModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.VoiceControlModule.Scripts;
using Fusion;
using GameplayEvents;
using UnityEngine;
using Zenject;

namespace Features.PlayerGrabModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class PlayerGrabController : NetworkBehaviour
	{
		private const int INVALID_PLAYER_ID = -1;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private float _authorityDesyncTimeout = 5f;

		[SerializeField]
		private float _ungrabDelay = 3f;

		[SerializeField]
		private float _cooldownTimerValue = 2f;

		[SerializeField]
		private float _stuckGrabRecoveryTimeout = 7f;

		[SerializeField]
		private float _grabBlockedRecoveryTimeout = 10f;

		[SerializeField]
		private string _pitchEffectName = "Pitch";

		[SerializeField]
		private float _pitchEffectValueOnGrab = 0.3f;

		[SerializeField]
		private float _pitchFadeDuration = 2f;

		[SerializeField]
		private EventReference _grabbedEvent;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private PlayerRef _originStateAuthority;

		private PlayerMovableModel _playerMovableModel;

		private float _authorityDesyncTimer;

		private LineArmsModel _lineArmsModel;

		private Coroutine _delayCoroutine;

		private CameraModel _cameraModel;

		private GameGlobalNetworkingPause _gameGlobalNetworkingPause;

		private MultiplayerModel _multiplayerModel;

		private PlayersRagdollModel _playersRagdollModel;

		private IVoiceService _voiceService;

		private GameplayEventBus _gameplayEventBus;

		private IPlayerStateService _playerStateService;

		private EmotesTriggerModel _emotesTriggerModel;

		private IAudioService _audioService;

		private float _stuckGrabRecoveryTimer;

		private float _localGrabReasonRecoveryTimer;

		private float _grabBlockedRecoveryTimer;

		private PlayerRef _lastObservedStateAuthority;

		[WeaverGenerated]
		[DefaultForProperty("_isGrabbed", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool __isGrabbed;

		[WeaverGenerated]
		[DefaultForProperty("_cooldownTimer", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float __cooldownTimer;

		[WeaverGenerated]
		[DefaultForProperty("_grabberPlayerId", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int __grabberPlayerId;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool _isGrabbed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabController._isGrabbed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabController._isGrabbed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe float _cooldownTimer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabController._cooldownTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabController._cooldownTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe int _grabberPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabController._grabberPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabController._grabberPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel, LineArmsModel lineArmsModel, CameraModel cameraModel, GameGlobalNetworkingPause gameGlobalNetworkingPause, MultiplayerModel multiplayerModel, PlayersRagdollModel playersRagdollModel, IVoiceService voiceService, GameplayEventBus gameplayEventBus, IPlayerStateService playerStateService, EmotesTriggerModel emotesTriggerModel, IAudioService audioService)
		{
			_playerMovableModel = playerMovableModel;
			_lineArmsModel = lineArmsModel;
			_cameraModel = cameraModel;
			_gameGlobalNetworkingPause = gameGlobalNetworkingPause;
			_multiplayerModel = multiplayerModel;
			_playersRagdollModel = playersRagdollModel;
			_voiceService = voiceService;
			_gameplayEventBus = gameplayEventBus;
			_playerStateService = playerStateService;
			_emotesTriggerModel = emotesTriggerModel;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			_originStateAuthority = base.Object.StateAuthority;
			_lastObservedStateAuthority = base.Object.StateAuthority;
			PublishDiagnostic("spawned");
			if (!(base.Object.StateAuthority != base.Runner.LocalPlayer))
			{
				_simplePointGrabable.LocalGrabBlocked = true;
			}
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_simplePointGrabable.LocalOnGrab += OnGrab;
			_simplePointGrabable.LocalOnUnGrab += OnUnGrab;
			_simplePointGrabable.LocalOnExternalGrab += OnExternalGrab;
			_simplePointGrabable.LocalOnExternalUnGrab += OnExternalUnGrab;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_simplePointGrabable.LocalOnGrab -= OnGrab;
			_simplePointGrabable.LocalOnUnGrab -= OnUnGrab;
			_simplePointGrabable.LocalOnExternalGrab -= OnExternalGrab;
			_simplePointGrabable.LocalOnExternalUnGrab -= OnExternalUnGrab;
		}

		private void OnGrab(int i)
		{
			if (base.Runner == null || base.Object == null)
			{
				return;
			}
			_audioService.PlayOneShotAttached(_grabbedEvent, _soundSourceBehaviour);
			PublishDiagnostic("on_grab_received");
			PurgeDepartedGrabbers();
			if (GetActiveGrabberCount() > 1)
			{
				return;
			}
			CancelDelayedUnGrab("on_grab_cancelled_delayed_ungrab");
			ApplySharedHoldEnterEffects();
			if (base.Object.InputAuthority == base.Runner.LocalPlayer)
			{
				AddLocalRagdollReason(RagdollSimulationReasonEnum.Grab);
				_emotesTriggerModel.RequestBodyEmoteInterrupt();
			}
			if (!base.HasStateAuthority)
			{
				return;
			}
			int firstActiveGrabberId = GetFirstActiveGrabberId();
			if (firstActiveGrabberId != -1)
			{
				if (GetActiveGrabberCount() <= 1)
				{
					_playerMovableModel.LocalMovable.Rigidbody.linearVelocity = Vector3.zero;
				}
				if (_lineArmsModel.GetLineArmForPlayer(_originStateAuthority.PlayerId).CurrentGrabbables.Count > 0)
				{
					_lineArmsModel.GetLineArmForPlayer(_originStateAuthority.PlayerId).UnJoinAll(throwItem: false);
				}
				if (!IsGrabableDespawned())
				{
					_simplePointGrabable.AssignStateAuthorityRpc(firstActiveGrabberId);
					_isGrabbed = true;
					_grabberPlayerId = firstActiveGrabberId;
					PublishDiagnostic("on_grab_state_authority_applied");
				}
			}
		}

		private void OnExternalGrab(int holderId, int authorityPlayerId)
		{
			if (base.Runner == null || base.Object == null)
			{
				return;
			}
			_audioService.PlayOneShotAttached(_grabbedEvent, _soundSourceBehaviour);
			PublishDiagnostic("on_external_grab_received");
			PurgeDepartedGrabbers();
			CancelDelayedUnGrab("on_external_grab_cancelled_delayed_ungrab");
			ApplySharedHoldEnterEffects();
			if (_isGrabbed)
			{
				PublishDiagnostic("on_external_grab_already_held");
			}
			else if (base.HasStateAuthority && authorityPlayerId != -1)
			{
				_playerMovableModel.LocalMovable.Rigidbody.linearVelocity = Vector3.zero;
				if (_lineArmsModel.GetLineArmForPlayer(_originStateAuthority.PlayerId).CurrentGrabbables.Count > 0)
				{
					_lineArmsModel.GetLineArmForPlayer(_originStateAuthority.PlayerId).UnJoinAll(throwItem: false);
				}
				if (base.Object.InputAuthority == base.Runner.LocalPlayer)
				{
					_emotesTriggerModel.RequestBodyEmoteInterrupt();
				}
				if (!IsGrabableDespawned())
				{
					_simplePointGrabable.AssignStateAuthorityRpc(authorityPlayerId);
					_isGrabbed = true;
					_grabberPlayerId = authorityPlayerId;
					PublishDiagnostic("on_external_grab_state_authority_applied");
				}
			}
		}

		private void ApplySharedHoldEnterEffects()
		{
			if (base.Runner.LocalPlayer != base.Object.InputAuthority)
			{
				_voiceService.ProcessFadeEffectForSpeaker(base.Object.InputAuthority.PlayerId, _pitchEffectValueOnGrab, _pitchFadeDuration, _pitchEffectName);
			}
			ResetRecoveryTimers();
		}

		private void CancelDelayedUnGrab(string diagnosticStage)
		{
			if (_delayCoroutine != null)
			{
				StopCoroutine(_delayCoroutine);
				_delayCoroutine = null;
				PublishDiagnostic(diagnosticStage);
			}
		}

		private void AddLocalRagdollReason(RagdollSimulationReasonEnum reason)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				ragdoll.AddSimulationReason(reason);
			}
		}

		private void RemoveLocalRagdollReason(RagdollSimulationReasonEnum reason)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				ragdoll.RemoveSimulationReason(reason);
			}
		}

		private void OnUnGrab()
		{
			PublishDiagnostic("on_ungrab_received");
			if (base.HasStateAuthority && !IsGrabableDespawned() && _simplePointGrabable.GrabbedByPlayers.Count != 0 && _simplePointGrabable.GrabbedByPlayers[0] != base.Object.StateAuthority.PlayerId)
			{
				_simplePointGrabable.AssignStateAuthorityRpc(_simplePointGrabable.GrabbedByPlayers[0]);
			}
			if (HasAnyHoldSource())
			{
				PublishDiagnostic("on_ungrab_hold_source_remains");
				return;
			}
			if (base.Runner.LocalPlayer != base.Object.InputAuthority)
			{
				_voiceService.ProcessFadeEffectForSpeaker(base.Object.InputAuthority.PlayerId, 0f, _pitchFadeDuration, _pitchEffectName);
			}
			CancelDelayedUnGrab("on_ungrab_replaced_delayed_cleanup");
			_delayCoroutine = StartCoroutine(DelayedUnGrab());
			PublishDiagnostic("on_ungrab_delayed_cleanup_scheduled");
		}

		private void OnExternalUnGrab()
		{
			PublishDiagnostic("on_external_ungrab_received");
			if (HasAnyHoldSource())
			{
				if (base.HasStateAuthority && !IsGrabableDespawned() && _simplePointGrabable.GrabbedByPlayers.Count != 0 && _simplePointGrabable.GrabbedByPlayers[0] != base.Object.StateAuthority.PlayerId)
				{
					_simplePointGrabable.AssignStateAuthorityRpc(_simplePointGrabable.GrabbedByPlayers[0]);
				}
				PublishDiagnostic("on_external_ungrab_hold_source_remains");
				return;
			}
			if (base.Runner.LocalPlayer != base.Object.InputAuthority)
			{
				_voiceService.ProcessFadeEffectForSpeaker(base.Object.InputAuthority.PlayerId, 0f, _pitchFadeDuration, _pitchEffectName);
			}
			CancelDelayedUnGrab("on_external_ungrab_replaced_delayed_cleanup");
			_delayCoroutine = StartCoroutine(DelayedUnGrab());
			PublishDiagnostic("on_external_ungrab_delayed_cleanup_scheduled");
		}

		private IEnumerator DelayedUnGrab()
		{
			float timer = 0f;
			while (timer < _ungrabDelay)
			{
				timer += Time.deltaTime * _gameGlobalNetworkingPause.NetworkTimeScale;
				yield return null;
			}
			if (HasAnyHoldSource())
			{
				PublishDiagnostic("delayed_ungrab_cancelled_hold_source_remains");
			}
			else
			{
				if (IsGrabableDespawned())
				{
					yield break;
				}
				if (!IsPlayerActive(_originStateAuthority.PlayerId))
				{
					PublishDiagnostic("delayed_ungrab_owner_departed");
					yield break;
				}
				if (base.HasStateAuthority)
				{
					_cooldownTimer = 0f;
					ReturnAuthorityToOriginalOwner();
					_isGrabbed = false;
					PublishDiagnostic("delayed_ungrab_returning_authority");
				}
				while (_simplePointGrabable.NetworkObject.StateAuthority != _originStateAuthority)
				{
					yield return null;
					if (IsGrabableDespawned())
					{
						yield break;
					}
				}
				if (base.HasStateAuthority)
				{
					ProcessLocalAfterGrab();
					PublishDiagnostic("delayed_ungrab_cleanup_completed");
				}
			}
		}

		private bool IsGrabableDespawned()
		{
			if (!(base.Runner == null) && !(base.Object == null) && !(_simplePointGrabable == null) && !(_simplePointGrabable.NetworkObject == null))
			{
				return !_simplePointGrabable.NetworkObject.IsValid;
			}
			return true;
		}

		private void ReturnAuthorityToOriginalOwner()
		{
			if (!IsGrabableDespawned() && IsPlayerActive(_originStateAuthority.PlayerId))
			{
				_simplePointGrabable.AssignStateAuthorityRpc(_originStateAuthority.PlayerId);
			}
		}

		public void ForceUnGrabRecovery()
		{
			if (!(base.Runner == null) && !(base.Object == null) && !(base.Object.InputAuthority != base.Runner.LocalPlayer))
			{
				if (_delayCoroutine != null)
				{
					StopCoroutine(_delayCoroutine);
					_delayCoroutine = null;
				}
				if (base.HasStateAuthority)
				{
					_cooldownTimer = 0f;
					_isGrabbed = false;
				}
				else if (base.Object.StateAuthority != _originStateAuthority)
				{
					ReturnAuthorityToOriginalOwner();
				}
				ProcessLocalAfterGrab();
				ResetRecoveryTimers();
				PublishDiagnostic("force_ungrab_recovery");
			}
		}

		public void ReturnGrabbedAuthorityToOwner()
		{
			if (!(base.Runner == null) && !(base.Object == null) && !(base.Object.InputAuthority == base.Runner.LocalPlayer) && !(base.Object.InputAuthority == PlayerRef.None) && !(base.Object.StateAuthority != base.Runner.LocalPlayer))
			{
				ReturnAuthorityToOriginalOwner();
			}
		}

		private void ProcessLocalAfterGrab()
		{
			RestoreLocalCameraTargetAfterGrab();
			if (base.Object.InputAuthority == base.Runner.LocalPlayer)
			{
				RemoveLocalRagdollReason(RagdollSimulationReasonEnum.Grab);
			}
		}

		private void RestoreLocalCameraTargetAfterGrab()
		{
			if (!(base.Object.InputAuthority != base.Runner.LocalPlayer) && !_playerStateService.IsPlayerDead(base.Runner.LocalPlayer.PlayerId))
			{
				Transform cameraPositionTransform = _playerMovableModel.LocalMovable.CameraPositionTransform;
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
			}
		}

		private void Update()
		{
			if (base.Runner == null)
			{
				return;
			}
			PublishStateAuthorityChangeDiagnostic();
			ProcessDepartedGrabberRecovery();
			if (base.Runner.LocalPlayer.PlayerId == _originStateAuthority.PlayerId && !_playerStateService.IsPlayerDead(base.Runner.LocalPlayer.PlayerId) && !IsGrabberDeparted() && _simplePointGrabable.GrabObject.Grabbers.Count > 0 && _isGrabbed && _simplePointGrabable.GrabObject.Grabbers[0].physGrabPoints.ContainsKey(_simplePointGrabable.GrabObject) && _cameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].GetTrackingTarget() != _simplePointGrabable.GrabObject.Grabbers[0].physGrabPoints[_simplePointGrabable.GrabObject].transform)
			{
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(_simplePointGrabable.GrabObject.Grabbers[0].physGrabPoints[_simplePointGrabable.GrabObject].transform);
			}
			ProcessStuckGrabRecovery();
			ProcessLocalGrabReasonRecovery();
			ProcessGrabValidation();
			if (!base.HasStateAuthority)
			{
				return;
			}
			ProcessGrabBlockedRecovery();
			if (!HasAnyHoldSource())
			{
				SetGrabBlocked(_cooldownTimer < _cooldownTimerValue, "cooldown_grab_block_changed");
				if (_cooldownTimer >= _cooldownTimerValue)
				{
					_cooldownTimer = _cooldownTimerValue;
				}
				else
				{
					_cooldownTimer += Time.deltaTime * _gameGlobalNetworkingPause.NetworkTimeScale;
				}
			}
		}

		private void ProcessStuckGrabRecovery()
		{
			if (!_isGrabbed || HasEffectiveGrab())
			{
				_stuckGrabRecoveryTimer = 0f;
				return;
			}
			_stuckGrabRecoveryTimer += Time.deltaTime * _gameGlobalNetworkingPause.NetworkTimeScale;
			if (_stuckGrabRecoveryTimer < _stuckGrabRecoveryTimeout)
			{
				return;
			}
			if (base.HasStateAuthority)
			{
				_cooldownTimer = 0f;
				_isGrabbed = false;
				if (base.Object.StateAuthority != _originStateAuthority)
				{
					ReturnAuthorityToOriginalOwner();
				}
				PublishDiagnostic("stuck_grab_authority_recovery");
			}
			_stuckGrabRecoveryTimer = 0f;
		}

		private void ProcessLocalGrabReasonRecovery()
		{
			if (base.Object.InputAuthority != base.Runner.LocalPlayer || HasEffectiveGrab() || !_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll) || !ragdoll.HasSimulationReason(RagdollSimulationReasonEnum.Grab))
			{
				_localGrabReasonRecoveryTimer = 0f;
				return;
			}
			_localGrabReasonRecoveryTimer += Time.deltaTime * _gameGlobalNetworkingPause.NetworkTimeScale;
			if (!(_localGrabReasonRecoveryTimer < _stuckGrabRecoveryTimeout))
			{
				ProcessLocalAfterGrab();
				PublishDiagnostic("stuck_grab_local_ragdoll_recovery");
				_localGrabReasonRecoveryTimer = 0f;
			}
		}

		private void ProcessGrabBlockedRecovery()
		{
			if (!_simplePointGrabable.GrabBlocked || HasEffectiveGrab())
			{
				_grabBlockedRecoveryTimer = 0f;
				return;
			}
			_grabBlockedRecoveryTimer += Time.deltaTime * _gameGlobalNetworkingPause.NetworkTimeScale;
			if (!(_grabBlockedRecoveryTimer < _grabBlockedRecoveryTimeout))
			{
				_cooldownTimer = _cooldownTimerValue;
				SetGrabBlocked(blocked: false, "grab_blocked_timeout_recovery");
				_grabBlockedRecoveryTimer = 0f;
			}
		}

		private void ProcessGrabValidation()
		{
			if (_isGrabbed)
			{
				_authorityDesyncTimer = 0f;
			}
			else
			{
				if (!base.HasStateAuthority)
				{
					return;
				}
				bool num = !HasEffectiveGrab();
				bool flag = base.Object.StateAuthority != _originStateAuthority;
				if (num && flag)
				{
					_authorityDesyncTimer += Time.deltaTime * _gameGlobalNetworkingPause.NetworkTimeScale;
					if (_authorityDesyncTimer >= _authorityDesyncTimeout)
					{
						ReturnAuthorityToOriginalOwner();
						ProcessLocalAfterGrab();
						PublishDiagnostic("authority_desync_recovery");
						_authorityDesyncTimer = 0f;
					}
				}
				else
				{
					_authorityDesyncTimer = 0f;
				}
			}
		}

		private void ProcessDepartedGrabberRecovery()
		{
			if (!IsGrabberDeparted())
			{
				return;
			}
			bool flag = GetActiveGrabberCount() > 0;
			if (flag)
			{
				int firstActiveGrabberId = GetFirstActiveGrabberId();
				if (firstActiveGrabberId != -1)
				{
					_grabberPlayerId = firstActiveGrabberId;
				}
			}
			else
			{
				ProcessLocalAfterGrab();
			}
			if (!base.HasStateAuthority)
			{
				return;
			}
			for (int num = _simplePointGrabable.GrabbedByPlayers.Count - 1; num >= 0; num--)
			{
				int num2 = _simplePointGrabable.GrabbedByPlayers[num];
				if (num2 != _originStateAuthority.PlayerId && !IsPlayerActive(num2))
				{
					_simplePointGrabable.UnGrabbedByPlayer(num2);
				}
			}
			if (!flag && _simplePointGrabable.GrabbedByExternals.Count <= 0)
			{
				_isGrabbed = false;
				_grabberPlayerId = _originStateAuthority.PlayerId;
				_cooldownTimer = 0f;
				if (_playersRagdollModel.TryGetPlayerRagdoll(base.Object.InputAuthority.PlayerId, out var ragdoll))
				{
					ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.Grab);
				}
				if (base.Object.StateAuthority != _originStateAuthority)
				{
					ReturnAuthorityToOriginalOwner();
				}
				ResetRecoveryTimers();
			}
		}

		private bool IsGrabberDeparted()
		{
			if (_isGrabbed && _grabberPlayerId != _originStateAuthority.PlayerId && !IsPlayerActive(_grabberPlayerId))
			{
				return true;
			}
			foreach (int grabbedByPlayer in _simplePointGrabable.GrabbedByPlayers)
			{
				if (grabbedByPlayer != _originStateAuthority.PlayerId && !IsPlayerActive(grabbedByPlayer))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsPlayerActive(int playerId)
		{
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
		}

		private void PurgeDepartedGrabbers()
		{
			PurgeStalePhysGrabbers();
			if (!base.HasStateAuthority)
			{
				return;
			}
			for (int num = _simplePointGrabable.GrabbedByPlayers.Count - 1; num >= 0; num--)
			{
				int num2 = _simplePointGrabable.GrabbedByPlayers[num];
				if (num2 != _originStateAuthority.PlayerId && !IsPlayerActive(num2))
				{
					_simplePointGrabable.UnGrabbedByPlayer(num2);
				}
			}
		}

		private void PurgeStalePhysGrabbers()
		{
			List<PhysGrabber> grabbers = _simplePointGrabable.GrabObject.Grabbers;
			for (int num = grabbers.Count - 1; num >= 0; num--)
			{
				PhysGrabber physGrabber = grabbers[num];
				if (physGrabber == null || physGrabber.Object == null || !physGrabber.Object.IsValid || !IsPlayerActive(physGrabber.Object.StateAuthority.PlayerId))
				{
					grabbers.RemoveAt(num);
				}
			}
		}

		private int GetActiveGrabberCount()
		{
			int num = 0;
			foreach (int grabbedByPlayer in _simplePointGrabable.GrabbedByPlayers)
			{
				if (IsPlayerActive(grabbedByPlayer))
				{
					num++;
				}
			}
			return num;
		}

		private int GetFirstActiveGrabberId()
		{
			foreach (int grabbedByPlayer in _simplePointGrabable.GrabbedByPlayers)
			{
				if (IsPlayerActive(grabbedByPlayer))
				{
					return grabbedByPlayer;
				}
			}
			return -1;
		}

		private bool HasAnyHoldSource()
		{
			if (GetActiveGrabberCount() <= 0)
			{
				return _simplePointGrabable.GrabbedByExternals.Count > 0;
			}
			return true;
		}

		private bool HasEffectiveGrab()
		{
			foreach (PhysGrabber grabber in _simplePointGrabable.GrabObject.Grabbers)
			{
				if (!(grabber == null) && !(grabber.Object == null) && grabber.Object.IsValid && IsPlayerActive(grabber.Object.StateAuthority.PlayerId) && grabber.physGrabPoints.ContainsKey(_simplePointGrabable.GrabObject))
				{
					return true;
				}
			}
			return false;
		}

		private void ResetRecoveryTimers()
		{
			_stuckGrabRecoveryTimer = 0f;
			_localGrabReasonRecoveryTimer = 0f;
			_grabBlockedRecoveryTimer = 0f;
		}

		private void SetGrabBlocked(bool blocked, string stage)
		{
			if (_simplePointGrabable.GrabBlocked != blocked)
			{
				_simplePointGrabable.GrabBlocked = blocked;
				PublishDiagnostic(stage);
			}
		}

		private void PublishStateAuthorityChangeDiagnostic()
		{
			if (!(_lastObservedStateAuthority == base.Object.StateAuthority))
			{
				PublishDiagnostic("state_authority_changed");
				_lastObservedStateAuthority = base.Object.StateAuthority;
			}
		}

		private void PublishDiagnostic(string stage)
		{
			if (!(base.Runner == null) && !(base.Object == null))
			{
				PlayerRagdollEntity ragdoll;
				bool flag = _playersRagdollModel.TryGetPlayerRagdoll(base.Object.InputAuthority.PlayerId, out ragdoll);
				Dictionary<string, string> extras = new Dictionary<string, string>
				{
					["stage"] = stage,
					["origin_state_authority_player_id"] = _originStateAuthority.PlayerId.ToString(),
					["current_state_authority_player_id"] = base.Object.StateAuthority.PlayerId.ToString(),
					["has_state_authority"] = base.HasStateAuthority.ToString(),
					["is_grabbed"] = _isGrabbed.ToString(),
					["grab_blocked"] = _simplePointGrabable.GrabBlocked.ToString(),
					["effective_grab"] = HasEffectiveGrab().ToString(),
					["grabbed_by_players_count"] = _simplePointGrabable.GrabbedByPlayers.Count.ToString(),
					["grabbed_by_externals_count"] = _simplePointGrabable.GrabbedByExternals.Count.ToString(),
					["cooldown_timer"] = _cooldownTimer.ToString(CultureInfo.InvariantCulture),
					["authority_desync_timer"] = _authorityDesyncTimer.ToString(CultureInfo.InvariantCulture),
					["stuck_grab_recovery_timer"] = _stuckGrabRecoveryTimer.ToString(CultureInfo.InvariantCulture),
					["local_grab_reason_recovery_timer"] = _localGrabReasonRecoveryTimer.ToString(CultureInfo.InvariantCulture),
					["grab_blocked_recovery_timer"] = _grabBlockedRecoveryTimer.ToString(CultureInfo.InvariantCulture),
					["delayed_ungrab_coroutine_active"] = (_delayCoroutine != null).ToString(),
					["ragdoll_has_grab_reason"] = (flag && ragdoll.HasSimulationReason(RagdollSimulationReasonEnum.Grab)).ToString()
				};
				if (Application.isEditor)
				{
					Debug.Log("[PlayerGrabDiagnostic] " + FormatDiagnostic(extras), this);
				}
				_gameplayEventBus.Publish(new OnPlayerGrabDiagnosticGameplayEvent(extras));
			}
		}

		private static string FormatDiagnostic(Dictionary<string, string> extras)
		{
			List<string> list = new List<string>(extras.Count);
			foreach (KeyValuePair<string, string> extra in extras)
			{
				list.Add(extra.Key + "=" + extra.Value);
			}
			return string.Join(" | ", list);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			_isGrabbed = __isGrabbed;
			_cooldownTimer = __cooldownTimer;
			_grabberPlayerId = __grabberPlayerId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			__isGrabbed = _isGrabbed;
			__cooldownTimer = _cooldownTimer;
			__grabberPlayerId = _grabberPlayerId;
		}
	}
}

using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Locking;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Interaction
{
	public class InteractableHinge : Interactable
	{
		[Header("Hinge Settings")]
		[SerializeField]
		[Tooltip("Optional. If set, this transform is rotated instead of the component's own transform. Useful when InteractableHinge sits on a parent GameObject and the actual rotating pivot is elsewhere.")]
		private Transform connectedTransform;

		[SerializeField]
		private Vector3 targetOpeningRotation;

		[SerializeField]
		private Vector3 targetClosingRotation;

		[Header("Animation Settings")]
		[SerializeField]
		private float openingDuration = 0.5f;

		[SerializeField]
		private float closingDuration = 0.5f;

		[SerializeField]
		private AnimationEaseConfig openingEaseConfig = new AnimationEaseConfig();

		[SerializeField]
		private AnimationEaseConfig closingEaseConfig = new AnimationEaseConfig();

		[Header("Sound Settings")]
		[SerializeField]
		private SoundID openingSound;

		[SerializeField]
		private SoundID closingSound;

		[Header("Lockable (Optional)")]
		[SerializeField]
		private bool startsLocked;

		[SerializeField]
		private Lock attachedLock;

		[SerializeField]
		private SoundID lockedFeedbackSound;

		[SerializeField]
		private Transform lockedShakeTarget;

		[SerializeField]
		private float lockedShakeStrength = 0.02f;

		[SerializeField]
		private float lockedShakeDuration = 0.25f;

		[SerializeField]
		private float lockedShakeFrequency = 18f;

		[Header("Stuck (Optional)")]
		[SerializeField]
		private bool startsStuck;

		[SerializeField]
		private SoundID stuckFeedbackSound;

		[SerializeField]
		private Transform stuckShakeTarget;

		[SerializeField]
		private float stuckShakeStrength = 0.02f;

		[SerializeField]
		private float stuckShakeDuration = 0.25f;

		[SerializeField]
		private float stuckShakeFrequency = 18f;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		private Sequence _currentSequence;

		private Tween _lockedShakeTween;

		private Tween _stuckShakeTween;

		private double _lastLockedAttemptServerTime;

		private double _lastStuckAttemptServerTime;

		[SyncVar(hook = "OnIsOpenChanged")]
		private bool _isOpen;

		[SyncVar(hook = "OnIsLockedChanged")]
		private bool _isLocked;

		[SyncVar(hook = "OnIsStuckChanged")]
		private bool _isStuck;

		private InteractionStateMachine<HingeState> _stateMachine;

		private Action _onAnimateComplete;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isOpen;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isLocked;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isStuck;

		public UnityEvent OnOpened { get; } = new UnityEvent();

		public UnityEvent OnClosed { get; } = new UnityEvent();

		private Transform RotatingTransform
		{
			get
			{
				if (!(connectedTransform != null))
				{
					return base.transform;
				}
				return connectedTransform;
			}
		}

		public bool IsOpen
		{
			get
			{
				return _isOpen;
			}
			private set
			{
				Network_isOpen = value;
			}
		}

		public bool IsLocked => _isLocked;

		public bool IsStuck => _isStuck;

		public UnityEvent OnLockedAttempt { get; } = new UnityEvent();

		public UnityEvent OnLocked { get; } = new UnityEvent();

		public UnityEvent OnUnlocked { get; } = new UnityEvent();

		public UnityEvent OnStuckAttempt { get; } = new UnityEvent();

		public UnityEvent OnUnstuck { get; } = new UnityEvent();

		protected override bool UseStateMachine => true;

		public bool Network_isOpen
		{
			get
			{
				return _isOpen;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isOpen, 64uL, _Mirror_SyncVarHookDelegate__isOpen);
			}
		}

		public bool Network_isLocked
		{
			get
			{
				return _isLocked;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isLocked, 128uL, _Mirror_SyncVarHookDelegate__isLocked);
			}
		}

		public bool Network_isStuck
		{
			get
			{
				return _isStuck;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isStuck, 256uL, _Mirror_SyncVarHookDelegate__isStuck);
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<HingeState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(HingeState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleOpenAttempt).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(HingeState.Opened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", Close).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		private HingeState DetermineState()
		{
			if (!_isOpen)
			{
				return HingeState.Closed;
			}
			return HingeState.Opened;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (startsLocked)
			{
				Network_isLocked = true;
			}
			if (startsStuck)
			{
				Network_isStuck = true;
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				SetForLateJoiners();
			}
		}

		private void OnDestroy()
		{
			_currentSequence.Stop();
			_lockedShakeTween.Stop();
			_stuckShakeTween.Stop();
		}

		private void SetForLateJoiners()
		{
			if (IsOpen)
			{
				RotatingTransform.localRotation = Quaternion.Euler(targetOpeningRotation);
			}
			else
			{
				RotatingTransform.localRotation = Quaternion.Euler(targetClosingRotation);
			}
		}

		public void SetDurations(float openingDuration, float closingDuration)
		{
			this.openingDuration = openingDuration;
			this.closingDuration = closingDuration;
		}

		public void SetSounds(SoundID openingSound, SoundID closingSound)
		{
			this.openingSound = openingSound;
			this.closingSound = closingSound;
		}

		private void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue)
				{
					HandleOpening();
				}
				else
				{
					HandleClosing();
				}
			}
		}

		private void HandleOpening()
		{
			PlaySoundNetworked(openingSound);
			OnOpened.Invoke();
			AnimateHinge(targetOpeningRotation, openingDuration, openingEaseConfig, delegate
			{
				UpdateState();
			});
		}

		private void HandleClosing()
		{
			PlaySoundNetworked(closingSound);
			AnimateHinge(targetClosingRotation, closingDuration, closingEaseConfig, delegate
			{
				OnClosed.Invoke();
				UpdateState();
			});
		}

		private void AnimateHinge(Vector3 targetRotation, float duration, AnimationEaseConfig easeConfig, Action onComplete)
		{
			SetInteractionAvailability(newValue: false);
			_onAnimateComplete = onComplete;
			_currentSequence.Stop();
			_currentSequence = Sequence.Create().Chain(easeConfig.CreateLocalRotationTween(RotatingTransform, Quaternion.Euler(targetRotation), duration)).ChainCallback(this, delegate(InteractableHinge target)
			{
				target.SetInteractionAvailability(newValue: true);
				target._onAnimateComplete?.Invoke();
			});
		}

		private void PlaySoundNetworked(SoundID sound)
		{
			if (sound.IsValid() && base.isServer)
			{
				NetworkAudioRelay?.PlayOneShot(sound, base.transform.position);
			}
		}

		private void HandleOpenAttempt()
		{
			if (_isLocked)
			{
				_uiFeedbackManager?.CreateFloatingMessage("@interaction.is_locked", FeedbackType.Warning);
				CmdReportLockedAttempt();
			}
			else if (_isStuck)
			{
				if (HasCrowbar())
				{
					CmdUnstickAndOpen();
					return;
				}
				_uiFeedbackManager?.CreateFloatingMessage("@interaction.is_stuck", FeedbackType.Warning);
				CmdReportStuckAttempt();
			}
			else
			{
				Open();
			}
		}

		private bool HasCrowbar()
		{
			HeldItem heldItem = _playerService?.EquipmentManager?.EquippedEntity;
			if (heldItem == null)
			{
				return false;
			}
			Crowbar component;
			return heldItem.TryGetComponent<Crowbar>(out component);
		}

		[Server]
		public void ServerSetLocked(bool locked)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Interaction.InteractableHinge::ServerSetLocked(System.Boolean)' called when server was not active");
			}
			else
			{
				Network_isLocked = locked;
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdRequestUnlock()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdRequestUnlock()", -533884767, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdReportLockedAttempt()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdReportLockedAttempt()", -404289151, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayLockedShake()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::RpcPlayLockedShake()", 444780327, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void PlayLockedShake()
		{
			if (!_lockedShakeTween.isAlive && !(lockedShakeStrength <= 0f) && !(lockedShakeDuration <= 0f))
			{
				Transform target = ((lockedShakeTarget != null) ? lockedShakeTarget : base.transform);
				_lockedShakeTween = Tween.ShakeLocalPosition(target, Vector3.one * lockedShakeStrength, lockedShakeDuration, lockedShakeFrequency);
			}
		}

		private void OnIsLockedChanged(bool oldValue, bool newValue)
		{
			if (!IsLateJoinCompleted)
			{
				return;
			}
			if (newValue)
			{
				OnLocked.Invoke();
				return;
			}
			OnUnlocked.Invoke();
			if (attachedLock != null)
			{
				attachedLock.PlayUnlockingAnimation();
			}
		}

		[Server]
		public void ServerSetStuck(bool stuck)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Interaction.InteractableHinge::ServerSetStuck(System.Boolean)' called when server was not active");
			}
			else
			{
				Network_isStuck = stuck;
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdUnstickAndOpen()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdUnstickAndOpen()", 1018173236, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdReportStuckAttempt()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdReportStuckAttempt()", -428397507, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayStuckShake()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::RpcPlayStuckShake()", 252984273, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void PlayStuckShake()
		{
			if (!_stuckShakeTween.isAlive && !(stuckShakeStrength <= 0f) && !(stuckShakeDuration <= 0f))
			{
				Transform target = ((stuckShakeTarget != null) ? stuckShakeTarget : base.transform);
				_stuckShakeTween = Tween.ShakeLocalPosition(target, Vector3.one * stuckShakeStrength, stuckShakeDuration, stuckShakeFrequency);
			}
		}

		private void OnIsStuckChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted && !newValue)
			{
				OnUnstuck.Invoke();
			}
		}

		public void Open()
		{
			if (!IsOpen)
			{
				CmdOpen();
			}
		}

		public void Close()
		{
			if (IsOpen)
			{
				CmdClose();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdOpen()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdOpen()", 576337374, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdClose()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdClose()", -1210642568, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public InteractableHinge()
		{
			_Mirror_SyncVarHookDelegate__isOpen = OnIsOpenChanged;
			_Mirror_SyncVarHookDelegate__isLocked = OnIsLockedChanged;
			_Mirror_SyncVarHookDelegate__isStuck = OnIsStuckChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRequestUnlock()
		{
			if (_isLocked)
			{
				Network_isLocked = false;
			}
		}

		protected static void InvokeUserCode_CmdRequestUnlock(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestUnlock called on client.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_CmdRequestUnlock();
			}
		}

		protected void UserCode_CmdReportLockedAttempt()
		{
			float num = Mathf.Max(lockedShakeDuration, 0.2f);
			if (!(NetworkTime.time - _lastLockedAttemptServerTime < (double)num))
			{
				_lastLockedAttemptServerTime = NetworkTime.time;
				if (lockedFeedbackSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShot(lockedFeedbackSound, base.transform.position);
				}
				RpcPlayLockedShake();
			}
		}

		protected static void InvokeUserCode_CmdReportLockedAttempt(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReportLockedAttempt called on client.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_CmdReportLockedAttempt();
			}
		}

		protected void UserCode_RpcPlayLockedShake()
		{
			OnLockedAttempt.Invoke();
			PlayLockedShake();
			if (attachedLock != null)
			{
				attachedLock.PlayLockedShake();
			}
		}

		protected static void InvokeUserCode_RpcPlayLockedShake(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayLockedShake called on server.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_RpcPlayLockedShake();
			}
		}

		protected void UserCode_CmdUnstickAndOpen()
		{
			if (_isStuck)
			{
				Network_isStuck = false;
				IsOpen = true;
			}
		}

		protected static void InvokeUserCode_CmdUnstickAndOpen(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdUnstickAndOpen called on client.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_CmdUnstickAndOpen();
			}
		}

		protected void UserCode_CmdReportStuckAttempt()
		{
			float num = Mathf.Max(stuckShakeDuration, 0.2f);
			if (!(NetworkTime.time - _lastStuckAttemptServerTime < (double)num))
			{
				_lastStuckAttemptServerTime = NetworkTime.time;
				if (stuckFeedbackSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShot(stuckFeedbackSound, base.transform.position);
				}
				RpcPlayStuckShake();
			}
		}

		protected static void InvokeUserCode_CmdReportStuckAttempt(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReportStuckAttempt called on client.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_CmdReportStuckAttempt();
			}
		}

		protected void UserCode_RpcPlayStuckShake()
		{
			OnStuckAttempt.Invoke();
			PlayStuckShake();
		}

		protected static void InvokeUserCode_RpcPlayStuckShake(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayStuckShake called on server.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_RpcPlayStuckShake();
			}
		}

		protected void UserCode_CmdOpen()
		{
			IsOpen = true;
		}

		protected static void InvokeUserCode_CmdOpen(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdOpen called on client.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_CmdOpen();
			}
		}

		protected void UserCode_CmdClose()
		{
			IsOpen = false;
		}

		protected static void InvokeUserCode_CmdClose(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdClose called on client.");
			}
			else
			{
				((InteractableHinge)obj).UserCode_CmdClose();
			}
		}

		static InteractableHinge()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdRequestUnlock()", InvokeUserCode_CmdRequestUnlock, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdReportLockedAttempt()", InvokeUserCode_CmdReportLockedAttempt, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdUnstickAndOpen()", InvokeUserCode_CmdUnstickAndOpen, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdReportStuckAttempt()", InvokeUserCode_CmdReportStuckAttempt, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdOpen()", InvokeUserCode_CmdOpen, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::CmdClose()", InvokeUserCode_CmdClose, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::RpcPlayLockedShake()", InvokeUserCode_RpcPlayLockedShake);
			RemoteProcedureCalls.RegisterRpc(typeof(InteractableHinge), "System.Void NomadDrive.Features.Interaction.InteractableHinge::RpcPlayStuckShake()", InvokeUserCode_RpcPlayStuckShake);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isOpen);
				writer.WriteBool(_isLocked);
				writer.WriteBool(_isStuck);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				writer.WriteBool(_isOpen);
			}
			if ((syncVarDirtyBits & 0x80L) != 0L)
			{
				writer.WriteBool(_isLocked);
			}
			if ((syncVarDirtyBits & 0x100L) != 0L)
			{
				writer.WriteBool(_isStuck);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isOpen, _Mirror_SyncVarHookDelegate__isOpen, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isLocked, _Mirror_SyncVarHookDelegate__isLocked, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isStuck, _Mirror_SyncVarHookDelegate__isStuck, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isOpen, _Mirror_SyncVarHookDelegate__isOpen, reader.ReadBool());
			}
			if ((num & 0x80L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isLocked, _Mirror_SyncVarHookDelegate__isLocked, reader.ReadBool());
			}
			if ((num & 0x100L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isStuck, _Mirror_SyncVarHookDelegate__isStuck, reader.ReadBool());
			}
		}
	}
}

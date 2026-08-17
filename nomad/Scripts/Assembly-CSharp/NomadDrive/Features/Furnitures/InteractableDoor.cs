using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Locking;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Furnitures
{
	public class InteractableDoor : Interactable
	{
		[Header("Door Settings")]
		[SerializeField]
		private float openAngle = 90f;

		[SerializeField]
		private float animationDuration = 0.4f;

		[SerializeField]
		private Ease openEase = Ease.OutQuart;

		[SerializeField]
		private Ease closeEase = Ease.OutQuart;

		[Header("Rotation Axis")]
		[SerializeField]
		private Vector3 rotationAxis = Vector3.up;

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

		[Header("Events")]
		[SerializeField]
		private UnityEvent onDoorOpen;

		[SerializeField]
		private UnityEvent onDoorClose;

		[SyncVar(hook = "OnDoorStateChanged")]
		private byte _doorStateByte;

		[SyncVar(hook = "OnIsLockedChanged")]
		private bool _isLocked;

		[SyncVar(hook = "OnIsStuckChanged")]
		private bool _isStuck;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		private InteractionStateMachine<DoorState> _stateMachine;

		private Quaternion _closedRotation;

		private Quaternion _openRotation;

		private Tween _lockedShakeTween;

		private Tween _stuckShakeTween;

		private double _lastLockedAttemptServerTime;

		private double _lastStuckAttemptServerTime;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__doorStateByte;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isLocked;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isStuck;

		public DoorState CurrentDoorState
		{
			get
			{
				return (DoorState)_doorStateByte;
			}
			private set
			{
				Network_doorStateByte = (byte)value;
			}
		}

		public bool IsOpen => CurrentDoorState == DoorState.Open;

		public bool IsLocked => _isLocked;

		public bool IsStuck => _isStuck;

		public UnityEvent OnLockedAttempt { get; } = new UnityEvent();

		public UnityEvent OnLocked { get; } = new UnityEvent();

		public UnityEvent OnUnlocked { get; } = new UnityEvent();

		public UnityEvent OnStuckAttempt { get; } = new UnityEvent();

		public UnityEvent OnUnstuck { get; } = new UnityEvent();

		protected override bool UseStateMachine => true;

		public byte Network_doorStateByte
		{
			get
			{
				return _doorStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _doorStateByte, 64uL, _Mirror_SyncVarHookDelegate__doorStateByte);
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

		protected override void Awake()
		{
			base.Awake();
			Transform transform = ((base.ModelTransform != null) ? base.ModelTransform : base.transform);
			_closedRotation = transform.localRotation;
			_openRotation = _closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis);
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

		private void OnDestroy()
		{
			_lockedShakeTween.Stop();
			_stuckShakeTween.Stop();
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<DoorState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(DoorState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(DoorState.Open, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", HandleClose).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false));
		}

		private DoorState DetermineState()
		{
			return CurrentDoorState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		private void OnDoorStateChanged(byte oldState, byte newState)
		{
			if (!IsLateJoinCompleted)
			{
				return;
			}
			SetInteractionAvailability(newValue: false);
			Transform target = ((base.ModelTransform != null) ? base.ModelTransform : base.transform);
			if (newState == 1)
			{
				Tween.LocalRotation(target, _openRotation, animationDuration, openEase).OnComplete(this, delegate(InteractableDoor interactableDoor)
				{
					interactableDoor.onDoorOpen?.Invoke();
					interactableDoor.SetInteractionAvailability(newValue: true);
					interactableDoor.UpdateState();
				});
			}
			else
			{
				Tween.LocalRotation(target, _closedRotation, animationDuration, closeEase).OnComplete(this, delegate(InteractableDoor interactableDoor)
				{
					interactableDoor.onDoorClose?.Invoke();
					interactableDoor.SetInteractionAvailability(newValue: true);
					interactableDoor.UpdateState();
				});
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			Transform transform = ((base.ModelTransform != null) ? base.ModelTransform : base.transform);
			if (CurrentDoorState == DoorState.Open)
			{
				transform.localRotation = _openRotation;
			}
			else
			{
				transform.localRotation = _closedRotation;
			}
			UpdateState();
		}

		private void HandleOpen()
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
				CmdSetDoorState(DoorState.Open);
			}
		}

		private void HandleClose()
		{
			CmdSetDoorState(DoorState.Closed);
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

		[Command(requiresAuthority = false)]
		private void CmdSetDoorState(DoorState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EFurnitures_002EDoorState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdSetDoorState(NomadDrive.Features.Furnitures.DoorState)", 968838121, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerSetLocked(bool locked)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Furnitures.InteractableDoor::ServerSetLocked(System.Boolean)' called when server was not active");
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
			SendCommandInternal("System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdRequestUnlock()", 1493799499, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdReportLockedAttempt()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdReportLockedAttempt()", 1443698139, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayLockedShake()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Furnitures.InteractableDoor::RpcPlayLockedShake()", -1600893311, writer, 0, includeOwner: true);
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
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Furnitures.InteractableDoor::ServerSetStuck(System.Boolean)' called when server was not active");
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
			SendCommandInternal("System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdUnstickAndOpen()", -596755666, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdReportStuckAttempt()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdReportStuckAttempt()", -1861926621, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayStuckShake()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Furnitures.InteractableDoor::RpcPlayStuckShake()", 364496927, writer, 0, includeOwner: true);
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

		public InteractableDoor()
		{
			_Mirror_SyncVarHookDelegate__doorStateByte = OnDoorStateChanged;
			_Mirror_SyncVarHookDelegate__isLocked = OnIsLockedChanged;
			_Mirror_SyncVarHookDelegate__isStuck = OnIsStuckChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetDoorState__DoorState(DoorState state)
		{
			CurrentDoorState = state;
		}

		protected static void InvokeUserCode_CmdSetDoorState__DoorState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetDoorState called on client.");
			}
			else
			{
				((InteractableDoor)obj).UserCode_CmdSetDoorState__DoorState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EFurnitures_002EDoorState(reader));
			}
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
				((InteractableDoor)obj).UserCode_CmdRequestUnlock();
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
				((InteractableDoor)obj).UserCode_CmdReportLockedAttempt();
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
				((InteractableDoor)obj).UserCode_RpcPlayLockedShake();
			}
		}

		protected void UserCode_CmdUnstickAndOpen()
		{
			if (_isStuck)
			{
				Network_isStuck = false;
				CurrentDoorState = DoorState.Open;
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
				((InteractableDoor)obj).UserCode_CmdUnstickAndOpen();
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
				((InteractableDoor)obj).UserCode_CmdReportStuckAttempt();
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
				((InteractableDoor)obj).UserCode_RpcPlayStuckShake();
			}
		}

		static InteractableDoor()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableDoor), "System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdSetDoorState(NomadDrive.Features.Furnitures.DoorState)", InvokeUserCode_CmdSetDoorState__DoorState, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableDoor), "System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdRequestUnlock()", InvokeUserCode_CmdRequestUnlock, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableDoor), "System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdReportLockedAttempt()", InvokeUserCode_CmdReportLockedAttempt, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableDoor), "System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdUnstickAndOpen()", InvokeUserCode_CmdUnstickAndOpen, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableDoor), "System.Void NomadDrive.Features.Furnitures.InteractableDoor::CmdReportStuckAttempt()", InvokeUserCode_CmdReportStuckAttempt, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(InteractableDoor), "System.Void NomadDrive.Features.Furnitures.InteractableDoor::RpcPlayLockedShake()", InvokeUserCode_RpcPlayLockedShake);
			RemoteProcedureCalls.RegisterRpc(typeof(InteractableDoor), "System.Void NomadDrive.Features.Furnitures.InteractableDoor::RpcPlayStuckShake()", InvokeUserCode_RpcPlayStuckShake);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _doorStateByte);
				writer.WriteBool(_isLocked);
				writer.WriteBool(_isStuck);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _doorStateByte);
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
				GeneratedSyncVarDeserialize(ref _doorStateByte, _Mirror_SyncVarHookDelegate__doorStateByte, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _isLocked, _Mirror_SyncVarHookDelegate__isLocked, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isStuck, _Mirror_SyncVarHookDelegate__isStuck, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _doorStateByte, _Mirror_SyncVarHookDelegate__doorStateByte, NetworkReaderExtensions.ReadByte(reader));
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

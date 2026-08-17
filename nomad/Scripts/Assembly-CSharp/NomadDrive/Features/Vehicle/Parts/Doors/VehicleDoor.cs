using System;
using System.Runtime.InteropServices;
using System.Threading;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore.EvilSave;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Interactables;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Doors
{
	public class VehicleDoor : AttachableObject, INetworkSaveable
	{
		[SyncVar(hook = "OnDoorStateChanged")]
		private byte _doorStateByte;

		[SerializeField]
		private DoorHandle handleIn;

		[SerializeField]
		private DoorHandle handleOut;

		[SerializeField]
		private Transform glassTransform;

		[SerializeField]
		private Transform glassClosedPoint;

		[SerializeField]
		private Transform glassOpenPoint;

		[SerializeField]
		private float windowOpeningDuration;

		[SerializeField]
		private float windowClosingDuration;

		[Header("Audio")]
		[SerializeField]
		private SoundID openSound;

		[SerializeField]
		private SoundID closeSound;

		[SyncVar(hook = "OnWindowStateChanged")]
		private bool _isWindowOpen;

		private Vector3 _initialGlassLocalPosition;

		private Tween _doorTween;

		private InteractionStateMachine<VehicleDoorInteractionState> _doorInteractionStateMachine;

		private Tween _windowTween;

		private byte _restoredDoorState;

		private bool _restoredWindowOpen;

		private bool _hasDoorRestore;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__doorStateByte;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isWindowOpen;

		public UnityEvent OnDoorOpened { get; } = new UnityEvent();

		public UnityEvent OnDoorClosed { get; } = new UnityEvent();

		[field: SerializeField]
		public WindowControlType WindowControlType { get; private set; }

		[field: SerializeField]
		public DoorWindowButton WindowUpButton { get; set; }

		[field: SerializeField]
		public DoorWindowButton WindowDownButton { get; set; }

		[field: SerializeField]
		public VehicleDoorWindowCrank WindowCrank { get; set; }

		public VehicleDoorState VehicleDoorState
		{
			get
			{
				return (VehicleDoorState)_doorStateByte;
			}
			set
			{
				Network_doorStateByte = (byte)value;
			}
		}

		[field: SerializeField]
		public float OpeningSpeed { get; set; } = 1.5f;

		[field: SerializeField]
		public float ClosingSpeed { get; set; } = 0.5f;

		[field: SerializeField]
		public Vector3 ClosedRotation { get; set; } = Vector3.zero;

		[field: SerializeField]
		public Vector3 OpenedRotation { get; set; } = Vector3.zero;

		private Vector3 GlassClosedLocalPosition
		{
			get
			{
				if (!(glassClosedPoint != null))
				{
					return _initialGlassLocalPosition;
				}
				return glassClosedPoint.localPosition;
			}
		}

		private Vector3 GlassOpenLocalPosition
		{
			get
			{
				if (!(glassOpenPoint != null))
				{
					return _initialGlassLocalPosition;
				}
				return glassOpenPoint.localPosition;
			}
		}

		public string ContributorKey => "door";

		public byte Network_doorStateByte
		{
			get
			{
				return _doorStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _doorStateByte, 1024uL, _Mirror_SyncVarHookDelegate__doorStateByte);
			}
		}

		public bool Network_isWindowOpen
		{
			get
			{
				return _isWindowOpen;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isWindowOpen, 2048uL, _Mirror_SyncVarHookDelegate__isWindowOpen);
			}
		}

		protected override void InitializeStateMachine()
		{
			_doorInteractionStateMachine = new InteractionStateMachine<VehicleDoorInteractionState>(this);
			base.BaseStateMachine = _doorInteractionStateMachine;
			ConfigureDoorStates();
			_doorInteractionStateMachine.Initialize(DetermineDoorInteractionState());
		}

		private void ConfigureDoorStates()
		{
			_doorInteractionStateMachine.RegisterState(VehicleDoorInteractionState.Detached, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(VehicleDoorInteractionState.AttachedClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", Interact).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(VehicleDoorInteractionState.AttachedOpened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", Interact).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true));
		}

		private VehicleDoorInteractionState DetermineDoorInteractionState()
		{
			if (!base.IsAttached)
			{
				return VehicleDoorInteractionState.Detached;
			}
			return VehicleDoorState switch
			{
				VehicleDoorState.Opened => VehicleDoorInteractionState.AttachedOpened, 
				VehicleDoorState.Closing => VehicleDoorInteractionState.AttachedOpened, 
				_ => VehicleDoorInteractionState.AttachedClosed, 
			};
		}

		public override void UpdateState()
		{
			_doorInteractionStateMachine?.TransitionTo(DetermineDoorInteractionState());
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			OnAttached.AddListener(OnAttachedActions);
			handleIn.onHandlePulled.AddListener(Interact);
			handleOut.onHandlePulled.AddListener(Interact);
		}

		protected override void Start()
		{
			base.Start();
			handleIn?.SetInteractionAvailability(base.IsAttached);
			handleOut?.SetInteractionAvailability(base.IsAttached);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			OnAttached.RemoveListener(OnAttachedActions);
			handleIn.onHandlePulled.RemoveListener(Interact);
			handleOut.onHandlePulled.RemoveListener(Interact);
		}

		protected override void CaptureInitialAnimatedPoses()
		{
			if (glassTransform != null)
			{
				_initialGlassLocalPosition = glassTransform.localPosition;
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			VehicleDoorState = VehicleDoorState.Detached;
		}

		private void OnAttachedActions()
		{
			if (_doorTween.isAlive)
			{
				_doorTween.Stop();
			}
			if (_windowTween.isAlive)
			{
				_windowTween.Stop();
			}
			if (base.ModelTransform != null)
			{
				base.ModelTransform.localRotation = Quaternion.Euler(ClosedRotation);
			}
			if (glassTransform != null)
			{
				glassTransform.localPosition = GlassClosedLocalPosition;
			}
			if (WindowControlType == WindowControlType.Mechanical && WindowCrank != null)
			{
				WindowCrank.SetCrankPosition(isOpen: false);
			}
			handleIn?.SetInteractionAvailability(newValue: true);
			handleOut?.SetInteractionAvailability(newValue: true);
			CmdSetDoorState(3);
		}

		protected override void ResetAnimatedPoseOnDetach()
		{
			handleIn?.SetInteractionAvailability(newValue: false);
			handleOut?.SetInteractionAvailability(newValue: false);
			if (_doorTween.isAlive)
			{
				_doorTween.Stop();
			}
			if (_windowTween.isAlive)
			{
				_windowTween.Stop();
			}
			if (base.ModelTransform != null)
			{
				base.ModelTransform.localRotation = Quaternion.Euler(ClosedRotation);
			}
			if (glassTransform != null)
			{
				glassTransform.localPosition = GlassClosedLocalPosition;
			}
			if (WindowControlType == WindowControlType.Mechanical && WindowCrank != null)
			{
				WindowCrank.SetCrankPosition(isOpen: false);
			}
			if (base.isServer)
			{
				VehicleDoorState = VehicleDoorState.Closed;
				Network_isWindowOpen = false;
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			handleIn?.SetInteractionAvailability(base.IsAttached);
			handleOut?.SetInteractionAvailability(base.IsAttached);
			if (VehicleDoorState == VehicleDoorState.Opened)
			{
				base.ModelTransform.localRotation = Quaternion.Euler(OpenedRotation);
			}
			else if (VehicleDoorState == VehicleDoorState.Closed)
			{
				base.ModelTransform.localRotation = Quaternion.Euler(ClosedRotation);
			}
			if (glassTransform != null)
			{
				glassTransform.localPosition = (_isWindowOpen ? GlassOpenLocalPosition : GlassClosedLocalPosition);
			}
			if (WindowControlType == WindowControlType.Mechanical && WindowCrank != null)
			{
				WindowCrank.SetCrankPosition(_isWindowOpen);
			}
		}

		private void Interact()
		{
			if (base.IsAttached && VehicleDoorState != VehicleDoorState.Opening && VehicleDoorState != VehicleDoorState.Closing)
			{
				if (VehicleDoorState == VehicleDoorState.Closed)
				{
					Open();
				}
				else if (VehicleDoorState == VehicleDoorState.Opened)
				{
					Close();
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetDoorState(byte newState)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, newState);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.Doors.VehicleDoor::CmdSetDoorState(System.Byte)", 912938784, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnDoorStateChanged(byte oldState, byte newState)
		{
			if (IsLateJoinCompleted)
			{
				switch (newState)
				{
				case 0:
					OpenAnimations();
					break;
				case 1:
					CloseAnimations();
					break;
				}
				UpdateState();
			}
		}

		private void Open()
		{
			CmdSetDoorState(0);
		}

		private void Close()
		{
			CmdSetDoorState(1);
		}

		private void OpenAnimations()
		{
			PlayHandleAnimations();
			if (openSound.IsValid())
			{
				AudioManager?.PlayOneShot(openSound, base.transform.position);
			}
			_doorTween = Tween.LocalRotation(base.ModelTransform, Quaternion.Euler(OpenedRotation), 1f / OpeningSpeed).OnComplete(delegate
			{
				OnDoorOpened.Invoke();
				if (base.isServer)
				{
					CmdSetDoorState(2);
				}
			});
		}

		private void CloseAnimations()
		{
			PlayHandleAnimations();
			_doorTween = Tween.LocalRotation(base.ModelTransform, Quaternion.Euler(ClosedRotation), 1f / ClosingSpeed).OnComplete(delegate
			{
				if (closeSound.IsValid())
				{
					AudioManager?.PlayOneShot(closeSound, base.transform.position);
				}
				OnDoorClosed.Invoke();
				if (base.isServer)
				{
					CmdSetDoorState(3);
				}
			});
		}

		private void PlayHandleAnimations()
		{
			handleIn?.PlayPullAnimation();
			handleOut?.PlayPullAnimation();
		}

		public void CloseWindowElectric(bool isBatteryInstalled, bool isBatteryBroken)
		{
			if (!(!_isWindowOpen || !isBatteryInstalled || isBatteryBroken))
			{
				CmdSetWindowOpen(isOpen: false);
			}
		}

		public void OpenWindowElectric(bool isBatteryInstalled, bool isBatteryBroken)
		{
			if (!(_isWindowOpen || !isBatteryInstalled || isBatteryBroken))
			{
				CmdSetWindowOpen(isOpen: true);
			}
		}

		public void ToggleWindowMechanical()
		{
			CmdSetWindowOpen(!_isWindowOpen);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetWindowOpen(bool isOpen)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(isOpen);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.Doors.VehicleDoor::CmdSetWindowOpen(System.Boolean)", 315473579, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnWindowStateChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue)
				{
					OpenWindowAnimation();
				}
				else
				{
					CloseWindowAnimation();
				}
			}
		}

		private void OpenWindowAnimation()
		{
			if (!(glassTransform == null))
			{
				_windowTween = Tween.LocalPosition(glassTransform, GlassOpenLocalPosition, windowOpeningDuration, Ease.Linear);
			}
		}

		private void CloseWindowAnimation()
		{
			if (!(glassTransform == null))
			{
				_windowTween = Tween.LocalPosition(glassTransform, GlassClosedLocalPosition, windowClosingDuration, Ease.Linear);
			}
		}

		public void PauseWindowAnimation()
		{
			if (_windowTween.isAlive)
			{
				_windowTween.isPaused = true;
			}
		}

		public void ResumeWindowAnimation()
		{
			if (_windowTween.isAlive)
			{
				_windowTween.isPaused = false;
			}
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			VehicleDoorState vehicleDoorState = VehicleDoorState;
			bool flag = vehicleDoorState == VehicleDoorState.Opened || vehicleDoorState == VehicleDoorState.Opening;
			writer.Write((byte)(flag ? 2u : 3u));
			writer.Write(_isWindowOpen);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			_restoredDoorState = reader.ReadByte();
			_restoredWindowOpen = reader.ReadBool();
			_hasDoorRestore = true;
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
			if (_hasDoorRestore && NetworkServer.active)
			{
				ApplyRestoredDoorStateAsync().Forget();
			}
		}

		private async UniTaskVoid ApplyRestoredDoorStateAsync()
		{
			CancellationToken token = this.GetCancellationTokenOnDestroy();
			for (int i = 0; i < 120; i++)
			{
				if (base.IsAttached)
				{
					break;
				}
				await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
			}
			await UniTask.DelayFrame(2, PlayerLoopTiming.Update, token);
			if (NetworkServer.active)
			{
				VehicleDoorState = (VehicleDoorState)_restoredDoorState;
				Network_isWindowOpen = _restoredWindowOpen;
				SetForLateJoiner();
			}
		}

		public VehicleDoor()
		{
			_Mirror_SyncVarHookDelegate__doorStateByte = OnDoorStateChanged;
			_Mirror_SyncVarHookDelegate__isWindowOpen = OnWindowStateChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetDoorState__Byte(byte newState)
		{
			VehicleDoorState = (VehicleDoorState)newState;
		}

		protected static void InvokeUserCode_CmdSetDoorState__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetDoorState called on client.");
			}
			else
			{
				((VehicleDoor)obj).UserCode_CmdSetDoorState__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdSetWindowOpen__Boolean(bool isOpen)
		{
			Network_isWindowOpen = isOpen;
		}

		protected static void InvokeUserCode_CmdSetWindowOpen__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetWindowOpen called on client.");
			}
			else
			{
				((VehicleDoor)obj).UserCode_CmdSetWindowOpen__Boolean(reader.ReadBool());
			}
		}

		static VehicleDoor()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleDoor), "System.Void NomadDrive.Features.Vehicle.Parts.Doors.VehicleDoor::CmdSetDoorState(System.Byte)", InvokeUserCode_CmdSetDoorState__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleDoor), "System.Void NomadDrive.Features.Vehicle.Parts.Doors.VehicleDoor::CmdSetWindowOpen(System.Boolean)", InvokeUserCode_CmdSetWindowOpen__Boolean, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _doorStateByte);
				writer.WriteBool(_isWindowOpen);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _doorStateByte);
			}
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				writer.WriteBool(_isWindowOpen);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _doorStateByte, _Mirror_SyncVarHookDelegate__doorStateByte, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _isWindowOpen, _Mirror_SyncVarHookDelegate__isWindowOpen, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _doorStateByte, _Mirror_SyncVarHookDelegate__doorStateByte, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isWindowOpen, _Mirror_SyncVarHookDelegate__isWindowOpen, reader.ReadBool());
			}
		}
	}
}

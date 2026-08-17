using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Interactables;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.GenericDoor
{
	public class VehicleGenericDoor : AttachableObject
	{
		[Header("Door Settings")]
		[SerializeField]
		private GenericDoorType doorType;

		[SerializeField]
		private ControlRotationAxis rotationAxis = ControlRotationAxis.Z;

		[Header("Handles (optional — if assigned, door opens/closes via handle pulls; handles also animate on any open/close)")]
		[SerializeField]
		private List<DoorHandle> handles = new List<DoorHandle>();

		[Header("Audio")]
		[SerializeField]
		private SoundID openSound;

		[SerializeField]
		private SoundID closeSound;

		[SyncVar(hook = "OnIsOpenChanged")]
		private bool _isOpen;

		private InteractionStateMachine<GenericDoorCombinedState> _doorStateMachine;

		private Quaternion _initialClosedRotation;

		private Tween _doorTween;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isOpen;

		public UnityEvent OnDoorOpened { get; } = new UnityEvent();

		public UnityEvent OnDoorClosed { get; } = new UnityEvent();

		[field: SerializeField]
		public float OpeningSpeed { get; set; } = 1f;

		[field: SerializeField]
		public float ClosingSpeed { get; set; } = 1f;

		[field: SerializeField]
		public float OpeningTargetAngle { get; set; } = 90f;

		private bool UseInteractiveHandles
		{
			get
			{
				if (handles != null)
				{
					return handles.Count > 0;
				}
				return false;
			}
		}

		public GenericDoorType DoorType => doorType;

		public VehicleGenericDoorSlot GenericDoorSlot { get; set; }

		protected override bool UseDefaultStateMachine => false;

		public bool Network_isOpen
		{
			get
			{
				return _isOpen;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isOpen, 1024uL, _Mirror_SyncVarHookDelegate__isOpen);
			}
		}

		protected override void CaptureInitialAnimatedPoses()
		{
			if (base.ModelTransform != null)
			{
				_initialClosedRotation = base.ModelTransform.localRotation;
			}
		}

		protected override void InitializeStateMachine()
		{
			_doorStateMachine = new InteractionStateMachine<GenericDoorCombinedState>(this);
			base.BaseStateMachine = _doorStateMachine;
			ConfigureDoorStates();
			_doorStateMachine.Initialize(DetermineDoorState());
		}

		private void ConfigureDoorStates()
		{
			_doorStateMachine.RegisterState(GenericDoorCombinedState.Detached, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false));
			if (UseInteractiveHandles)
			{
				_doorStateMachine.RegisterState(GenericDoorCombinedState.AttachedClosed, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.35f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach()).WithCrosshair(CrosshairType.Interact)
					.WithNameLabelVisibility(visible: false)
					.WithInteractionLabelVisibility(visible: true)).RegisterState(GenericDoorCombinedState.AttachedOpened, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.35f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach()).WithCrosshair(CrosshairType.Interact)
					.WithNameLabelVisibility(visible: false)
					.WithInteractionLabelVisibility(visible: true));
				return;
			}
			_doorStateMachine.RegisterState(GenericDoorCombinedState.AttachedClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", OpenDoor).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.35f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(GenericDoorCombinedState.AttachedOpened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", CloseDoor).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.35f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		private GenericDoorCombinedState DetermineDoorState()
		{
			if (!base.IsAttached)
			{
				return GenericDoorCombinedState.Detached;
			}
			if (!_isOpen)
			{
				return GenericDoorCombinedState.AttachedClosed;
			}
			return GenericDoorCombinedState.AttachedOpened;
		}

		public override void UpdateState()
		{
			_doorStateMachine?.TransitionTo(DetermineDoorState());
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			ForEachHandle(delegate(DoorHandle h)
			{
				h.onHandlePulled.AddListener(OnHandlePulled);
			});
		}

		protected override void Start()
		{
			base.Start();
			SetHandlesInteractable(base.IsAttached);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			ForEachHandle(delegate(DoorHandle h)
			{
				h.onHandlePulled.RemoveListener(OnHandlePulled);
			});
		}

		protected override void OnAttach()
		{
			base.OnAttach();
			SetHandlesInteractable(value: true);
		}

		protected override void OnDetach()
		{
			base.OnDetach();
			SetHandlesInteractable(value: false);
		}

		private void OnHandlePulled()
		{
			if (base.IsAttached)
			{
				if (_isOpen)
				{
					CloseDoor();
				}
				else
				{
					OpenDoor();
				}
			}
		}

		private void SetHandlesInteractable(bool value)
		{
			ForEachHandle(delegate(DoorHandle h)
			{
				h.SetInteractionAvailability(value);
			});
		}

		private void PlayHandleAnimations()
		{
			ForEachHandle(delegate(DoorHandle h)
			{
				h.PlayPullAnimation();
			});
		}

		private void ForEachHandle(Action<DoorHandle> action)
		{
			if (handles == null)
			{
				return;
			}
			for (int i = 0; i < handles.Count; i++)
			{
				DoorHandle doorHandle = handles[i];
				if (doorHandle != null)
				{
					action(doorHandle);
				}
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			SetHandlesInteractable(base.IsAttached);
			if (base.IsAttached)
			{
				if (_isOpen)
				{
					base.ModelTransform.localRotation = _initialClosedRotation * GetOpenRotation();
				}
				else
				{
					base.ModelTransform.localRotation = _initialClosedRotation;
				}
				UpdateState();
			}
		}

		protected override void ResetAnimatedPoseOnDetach()
		{
			SetHandlesInteractable(value: false);
			if (_doorTween.isAlive)
			{
				_doorTween.Stop();
			}
			if (base.ModelTransform != null)
			{
				base.ModelTransform.localRotation = _initialClosedRotation;
			}
			if (base.isServer)
			{
				Network_isOpen = false;
			}
		}

		private void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue)
				{
					OpenDoorActions();
				}
				else
				{
					CloseDoorActions();
				}
			}
		}

		private void OpenDoor()
		{
			if (!_isOpen)
			{
				CmdSetOpen(value: true);
			}
		}

		private void CloseDoor()
		{
			if (_isOpen)
			{
				CmdSetOpen(value: false);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetOpen(bool value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(value);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.GenericDoor.VehicleGenericDoor::CmdSetOpen(System.Boolean)", -1495074092, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OpenDoorActions()
		{
			PlayHandleAnimations();
			if (openSound.IsValid())
			{
				AudioManager?.PlayOneShot(openSound, base.transform.position);
			}
			_doorTween = Tween.LocalRotation(base.ModelTransform, _initialClosedRotation * GetOpenRotation(), 1f / OpeningSpeed, Ease.InQuad).OnComplete(this, delegate(VehicleGenericDoor target)
			{
				target.OnDoorOpened.Invoke();
				target.UpdateState();
			});
		}

		private void CloseDoorActions()
		{
			PlayHandleAnimations();
			_doorTween = Tween.LocalRotation(base.ModelTransform, _initialClosedRotation, 1f / ClosingSpeed, Ease.InQuart).OnComplete(this, delegate(VehicleGenericDoor target)
			{
				if (target.closeSound.IsValid())
				{
					target.AudioManager?.PlayOneShot(target.closeSound, target.transform.position);
				}
				target.OnDoorClosed.Invoke();
				target.UpdateState();
			});
		}

		private Quaternion GetOpenRotation()
		{
			return rotationAxis switch
			{
				ControlRotationAxis.X => Quaternion.Euler(OpeningTargetAngle, 0f, 0f), 
				ControlRotationAxis.Y => Quaternion.Euler(0f, OpeningTargetAngle, 0f), 
				ControlRotationAxis.Z => Quaternion.Euler(0f, 0f, OpeningTargetAngle), 
				_ => Quaternion.Euler(0f, 0f, OpeningTargetAngle), 
			};
		}

		public VehicleGenericDoor()
		{
			_Mirror_SyncVarHookDelegate__isOpen = OnIsOpenChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetOpen__Boolean(bool value)
		{
			Network_isOpen = value;
		}

		protected static void InvokeUserCode_CmdSetOpen__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetOpen called on client.");
			}
			else
			{
				((VehicleGenericDoor)obj).UserCode_CmdSetOpen__Boolean(reader.ReadBool());
			}
		}

		static VehicleGenericDoor()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleGenericDoor), "System.Void NomadDrive.Features.Vehicle.Parts.GenericDoor.VehicleGenericDoor::CmdSetOpen(System.Boolean)", InvokeUserCode_CmdSetOpen__Boolean, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isOpen);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteBool(_isOpen);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isOpen, _Mirror_SyncVarHookDelegate__isOpen, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isOpen, _Mirror_SyncVarHookDelegate__isOpen, reader.ReadBool());
			}
		}
	}
}

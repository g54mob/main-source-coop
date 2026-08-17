using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public class CustomHatch : Interactable
	{
		[SerializeField]
		private float openingAnnimationDuration = 0.4f;

		[SerializeField]
		private float closingAnnimationDuration = 0.4f;

		[SerializeField]
		private HatchRotationAxis rotationAxis;

		[SerializeField]
		private bool invertDirection;

		[SerializeField]
		private float openingAngleDegrees = 90f;

		[SerializeField]
		private float closingAngleDegrees;

		[SerializeField]
		private Ease openingEase;

		[SerializeField]
		private Ease closingEase;

		[Header("Sound Settings")]
		[SerializeField]
		private SoundID openSound;

		[SerializeField]
		private SoundID closeSound;

		[SerializeField]
		private MonoBehaviour _relatedSlotBehaviour;

		private IObjectSlot _relatedObjectSlot;

		private Quaternion _initialRotation;

		public UnityEvent onHatchOpened;

		public UnityEvent onHatchClosed;

		[SyncVar(hook = "OnIsOpenChanged")]
		private bool _isOpen;

		private InteractionStateMachine<HatchState> _stateMachine;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isOpen;

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

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<HatchState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(HatchState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", Open).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(HatchState.Opened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", Close).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false));
		}

		private HatchState DetermineState()
		{
			if (!_isOpen)
			{
				return HatchState.Closed;
			}
			return HatchState.Opened;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		protected override void Awake()
		{
			base.Awake();
			_initialRotation = base.ModelTransform.localRotation;
			_relatedObjectSlot = _relatedSlotBehaviour as IObjectSlot;
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_relatedObjectSlot != null)
			{
				onHatchOpened.AddListener(_relatedObjectSlot.Activate);
				onHatchClosed.AddListener(_relatedObjectSlot.Deactivate);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (_relatedObjectSlot != null)
			{
				onHatchOpened.RemoveListener(_relatedObjectSlot.Activate);
				onHatchClosed.RemoveListener(_relatedObjectSlot.Deactivate);
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (IsOpen)
			{
				base.ModelTransform.localRotation = GetTargetRotation(openingAngleDegrees);
				_relatedObjectSlot?.Activate();
			}
			else
			{
				base.ModelTransform.localRotation = GetTargetRotation(closingAngleDegrees);
				_relatedObjectSlot?.Deactivate();
			}
			UpdateState();
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
			if (openSound.IsValid())
			{
				AudioManager?.PlayOneShot(openSound, base.InteractionDisplayPoint.position);
			}
			Quaternion targetRotation = GetTargetRotation(openingAngleDegrees);
			Tween.LocalRotation(base.ModelTransform, targetRotation, openingAnnimationDuration, openingEase).OnComplete(this, OnOpeningComplete);
		}

		private void HandleClosing()
		{
			if (closeSound.IsValid())
			{
				AudioManager?.PlayOneShot(closeSound, base.InteractionDisplayPoint.position);
			}
			Quaternion targetRotation = GetTargetRotation(closingAngleDegrees);
			Tween.LocalRotation(base.ModelTransform, targetRotation, closingAnnimationDuration, closingEase).OnComplete(this, OnClosingComplete);
		}

		private Quaternion GetTargetRotation(float angleDegrees)
		{
			Vector3 axis = rotationAxis switch
			{
				HatchRotationAxis.X => Vector3.right, 
				HatchRotationAxis.Y => Vector3.up, 
				HatchRotationAxis.Z => Vector3.forward, 
				_ => Vector3.right, 
			};
			float angle = (invertDirection ? (0f - angleDegrees) : angleDegrees);
			return _initialRotation * Quaternion.AngleAxis(angle, axis);
		}

		private static void OnOpeningComplete(CustomHatch target)
		{
			target.SetInteractionAvailability(newValue: true);
			target.UpdateState();
			target.onHatchOpened.Invoke();
		}

		private static void OnClosingComplete(CustomHatch target)
		{
			target.UpdateState();
			target.onHatchClosed.Invoke();
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
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.CustomHatch::CmdOpen()", 1611355448, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdClose()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.CustomHatch::CmdClose()", 1744717898, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public CustomHatch()
		{
			_Mirror_SyncVarHookDelegate__isOpen = OnIsOpenChanged;
		}

		public override bool Weaved()
		{
			return true;
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
				((CustomHatch)obj).UserCode_CmdOpen();
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
				((CustomHatch)obj).UserCode_CmdClose();
			}
		}

		static CustomHatch()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(CustomHatch), "System.Void NomadDrive.Features.Interaction.CustomHatch::CmdOpen()", InvokeUserCode_CmdOpen, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(CustomHatch), "System.Void NomadDrive.Features.Interaction.CustomHatch::CmdClose()", InvokeUserCode_CmdClose, requiresAuthority: false);
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
			if ((syncVarDirtyBits & 0x40L) != 0L)
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
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isOpen, _Mirror_SyncVarHookDelegate__isOpen, reader.ReadBool());
			}
		}
	}
}

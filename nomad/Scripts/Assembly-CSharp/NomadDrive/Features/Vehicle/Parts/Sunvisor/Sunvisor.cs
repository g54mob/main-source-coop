using System;
using System.Runtime.InteropServices;
using EvilCore.EvilSave;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.SaveSystem;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Sunvisor
{
	public class Sunvisor : AttachableObject, INetworkSaveable
	{
		[SyncVar(hook = "OnSunvisorStateChanged")]
		private byte _sunvisorStateByte;

		private InteractionStateMachine<SunvisorInteractionState> _sunvisorStateMachine;

		private Quaternion _initialSunvisorLocalRotation;

		private Tween _sunvisorTween;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__sunvisorStateByte;

		[field: SerializeField]
		public float OpeningSpeed { get; set; } = 1.5f;

		[field: SerializeField]
		public float ClosingSpeed { get; set; } = 1.5f;

		[field: SerializeField]
		public float OpeningTargetAngle { get; set; } = 50f;

		[field: SerializeField]
		public float ClosingTargetAngle { get; set; }

		public SunvisorState SunvisorState
		{
			get
			{
				return (SunvisorState)_sunvisorStateByte;
			}
			private set
			{
				Network_sunvisorStateByte = (byte)value;
			}
		}

		public SunvisorSlot SunvisorSlot { get; set; }

		protected override bool UseDefaultStateMachine => false;

		public string ContributorKey => "sunvisor";

		public byte Network_sunvisorStateByte
		{
			get
			{
				return _sunvisorStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _sunvisorStateByte, 1024uL, _Mirror_SyncVarHookDelegate__sunvisorStateByte);
			}
		}

		protected override void CaptureInitialAnimatedPoses()
		{
			if (base.ModelTransform != null)
			{
				_initialSunvisorLocalRotation = base.ModelTransform.localRotation;
			}
		}

		protected override void InitializeStateMachine()
		{
			_sunvisorStateMachine = new InteractionStateMachine<SunvisorInteractionState>(this);
			base.BaseStateMachine = _sunvisorStateMachine;
			ConfigureStates();
			_sunvisorStateMachine.Initialize(DetermineSunvisorInteractionState());
		}

		protected override void ConfigureStates()
		{
			_sunvisorStateMachine.RegisterState(SunvisorInteractionState.Detached, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(SunvisorInteractionState.AttachedClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.push", Push).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(SunvisorInteractionState.AttachedOpen, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.pull", Pull).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		private SunvisorInteractionState DetermineSunvisorInteractionState()
		{
			if (!base.IsAttached)
			{
				return SunvisorInteractionState.Detached;
			}
			if (SunvisorState != SunvisorState.Open)
			{
				return SunvisorInteractionState.AttachedClosed;
			}
			return SunvisorInteractionState.AttachedOpen;
		}

		public override void UpdateState()
		{
			_sunvisorStateMachine?.TransitionTo(DetermineSunvisorInteractionState());
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (!base.IsAttached)
			{
				base.ModelTransform.localRotation = _initialSunvisorLocalRotation;
			}
			else if (SunvisorState == SunvisorState.Open)
			{
				base.ModelTransform.localRotation = Quaternion.Euler(new Vector3(OpeningTargetAngle, 0f, 0f));
			}
			else
			{
				base.ModelTransform.localRotation = Quaternion.Euler(new Vector3(ClosingTargetAngle, 0f, 0f));
			}
			UpdateState();
		}

		protected override void ResetAnimatedPoseOnDetach()
		{
			if (_sunvisorTween.isAlive)
			{
				_sunvisorTween.Stop();
			}
			if (base.ModelTransform != null)
			{
				base.ModelTransform.localRotation = _initialSunvisorLocalRotation;
			}
			if (base.isServer)
			{
				SunvisorState = SunvisorState.None;
			}
		}

		private void OnSunvisorStateChanged(byte oldValue, byte newValue)
		{
			if (IsLateJoinCompleted)
			{
				switch (newValue)
				{
				case 2:
					OnPushActions();
					break;
				case 1:
					OnPullActions();
					break;
				}
				UpdateState();
			}
		}

		private void OnPullActions()
		{
			_sunvisorTween = Tween.LocalRotation(endValue: Quaternion.Euler(new Vector3(ClosingTargetAngle, 0f, 0f)), target: base.ModelTransform, duration: 1f / ClosingSpeed, ease: Ease.InOutSine);
		}

		private void OnPushActions()
		{
			_sunvisorTween = Tween.LocalRotation(endValue: Quaternion.Euler(new Vector3(OpeningTargetAngle, 0f, 0f)), target: base.ModelTransform, duration: 1f / OpeningSpeed, ease: Ease.InOutSine);
		}

		private void Pull()
		{
			CmdPull();
		}

		private void Push()
		{
			CmdPush();
		}

		[Command(requiresAuthority = false)]
		private void CmdPull()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.Sunvisor.Sunvisor::CmdPull()", -101563474, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPush()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.Sunvisor.Sunvisor::CmdPush()", -1593155387, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_sunvisorStateByte);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			byte network_sunvisorStateByte = reader.ReadByte();
			if (NetworkServer.active)
			{
				Network_sunvisorStateByte = network_sunvisorStateByte;
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public Sunvisor()
		{
			_Mirror_SyncVarHookDelegate__sunvisorStateByte = OnSunvisorStateChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdPull()
		{
			SunvisorState = SunvisorState.Closed;
		}

		protected static void InvokeUserCode_CmdPull(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPull called on client.");
			}
			else
			{
				((Sunvisor)obj).UserCode_CmdPull();
			}
		}

		protected void UserCode_CmdPush()
		{
			SunvisorState = SunvisorState.Open;
		}

		protected static void InvokeUserCode_CmdPush(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPush called on client.");
			}
			else
			{
				((Sunvisor)obj).UserCode_CmdPush();
			}
		}

		static Sunvisor()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Sunvisor), "System.Void NomadDrive.Features.Vehicle.Parts.Sunvisor.Sunvisor::CmdPull()", InvokeUserCode_CmdPull, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Sunvisor), "System.Void NomadDrive.Features.Vehicle.Parts.Sunvisor.Sunvisor::CmdPush()", InvokeUserCode_CmdPush, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _sunvisorStateByte);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _sunvisorStateByte);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _sunvisorStateByte, _Mirror_SyncVarHookDelegate__sunvisorStateByte, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _sunvisorStateByte, _Mirror_SyncVarHookDelegate__sunvisorStateByte, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}

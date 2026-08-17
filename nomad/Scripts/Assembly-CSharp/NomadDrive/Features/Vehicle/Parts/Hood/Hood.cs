using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.EvilSave;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.Vehicle.Enums;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Hood
{
	public class Hood : AttachableObject, INetworkSaveable
	{
		[SyncVar(hook = "OnHoodStateChanged")]
		private byte _hoodStateByte;

		[Header("Audio")]
		[SerializeField]
		private SoundID openSound;

		[SerializeField]
		private SoundID closeSound;

		private InteractionStateMachine<HoodCombinedState> _hoodStateMachine;

		private Quaternion _initialClosedRotation;

		private Tween _hoodTween;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__hoodStateByte;

		public UnityEvent OnHoodOpened { get; } = new UnityEvent();

		public UnityEvent OnHoodClosed { get; } = new UnityEvent();

		public HoodState HoodState
		{
			get
			{
				return (HoodState)_hoodStateByte;
			}
			private set
			{
				Network_hoodStateByte = (byte)value;
			}
		}

		[field: SerializeField]
		public float OpeningSpeed { get; set; } = 1f;

		[field: SerializeField]
		public float ClosingSpeed { get; set; } = 1f;

		[field: SerializeField]
		public float OpeningTargetAngle { get; set; } = 75f;

		public HoodSlot HoodSlot { get; set; }

		public string ContributorKey => "hood";

		public byte Network_hoodStateByte
		{
			get
			{
				return _hoodStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _hoodStateByte, 1024uL, _Mirror_SyncVarHookDelegate__hoodStateByte);
			}
		}

		protected override void InitializeStateMachine()
		{
			_hoodStateMachine = new InteractionStateMachine<HoodCombinedState>(this);
			base.BaseStateMachine = _hoodStateMachine;
			ConfigureHoodStates();
			_hoodStateMachine.Initialize(DetermineHoodState());
		}

		private void ConfigureHoodStates()
		{
			_hoodStateMachine.RegisterState(HoodCombinedState.Detached, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(HoodCombinedState.AttachedClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", OpenHood).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.35f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(HoodCombinedState.AttachedOpened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", CloseHood).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.35f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		private HoodCombinedState DetermineHoodState()
		{
			if (!base.IsAttached)
			{
				return HoodCombinedState.Detached;
			}
			return HoodState switch
			{
				HoodState.None => HoodCombinedState.AttachedClosed, 
				HoodState.Closed => HoodCombinedState.AttachedClosed, 
				HoodState.Opened => HoodCombinedState.AttachedOpened, 
				_ => HoodCombinedState.AttachedClosed, 
			};
		}

		public override void UpdateState()
		{
			_hoodStateMachine?.TransitionTo(DetermineHoodState());
		}

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void CaptureInitialAnimatedPoses()
		{
			if (base.ModelTransform != null)
			{
				_initialClosedRotation = base.ModelTransform.localRotation;
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (base.isServer)
			{
				HoodState = HoodState.None;
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (base.IsAttached)
			{
				if (HoodState == HoodState.Opened)
				{
					base.ModelTransform.localRotation = _initialClosedRotation * Quaternion.Euler(OpeningTargetAngle, 0f, 0f);
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
			if (_hoodTween.isAlive)
			{
				_hoodTween.Stop();
			}
			if (base.ModelTransform != null)
			{
				base.ModelTransform.localRotation = _initialClosedRotation;
			}
			if (base.isServer)
			{
				HoodState = HoodState.None;
			}
		}

		private void OnHoodStateChanged(byte oldState, byte newState)
		{
			if (IsLateJoinCompleted)
			{
				switch (newState)
				{
				case 3:
					OpenHoodActions();
					break;
				case 1:
					CloseHoodActions();
					break;
				}
			}
		}

		private void CloseHood()
		{
			if (HoodState != HoodState.Closed)
			{
				CmdCloseHood();
			}
		}

		private void OpenHood()
		{
			if (HoodState != HoodState.Opened)
			{
				CmdOpenHood();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdCloseHood()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.Hood.Hood::CmdCloseHood()", 542585221, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdOpenHood()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.Hood.Hood::CmdOpenHood()", 859022481, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OpenHoodActions()
		{
			if (openSound.IsValid())
			{
				AudioManager?.PlayOneShot(openSound, base.transform.position);
			}
			_hoodTween = Tween.LocalRotation(base.ModelTransform, _initialClosedRotation * Quaternion.Euler(OpeningTargetAngle, 0f, 0f), 1f / OpeningSpeed, Ease.InQuad).OnComplete(this, delegate(Hood target)
			{
				target.OnHoodOpened.Invoke();
				target.UpdateState();
			});
		}

		private void CloseHoodActions()
		{
			_hoodTween = Tween.LocalRotation(base.ModelTransform, _initialClosedRotation, 1f / ClosingSpeed, Ease.InQuart).OnComplete(this, delegate(Hood target)
			{
				if (target.closeSound.IsValid())
				{
					target.AudioManager?.PlayOneShot(target.closeSound, target.transform.position);
				}
				target.OnHoodClosed.Invoke();
				target.UpdateState();
			});
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_hoodStateByte);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			byte network_hoodStateByte = reader.ReadByte();
			if (NetworkServer.active)
			{
				Network_hoodStateByte = network_hoodStateByte;
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public Hood()
		{
			_Mirror_SyncVarHookDelegate__hoodStateByte = OnHoodStateChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdCloseHood()
		{
			HoodState = HoodState.Closed;
		}

		protected static void InvokeUserCode_CmdCloseHood(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdCloseHood called on client.");
			}
			else
			{
				((Hood)obj).UserCode_CmdCloseHood();
			}
		}

		protected void UserCode_CmdOpenHood()
		{
			HoodState = HoodState.Opened;
		}

		protected static void InvokeUserCode_CmdOpenHood(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdOpenHood called on client.");
			}
			else
			{
				((Hood)obj).UserCode_CmdOpenHood();
			}
		}

		static Hood()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Hood), "System.Void NomadDrive.Features.Vehicle.Parts.Hood.Hood::CmdCloseHood()", InvokeUserCode_CmdCloseHood, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Hood), "System.Void NomadDrive.Features.Vehicle.Parts.Hood.Hood::CmdOpenHood()", InvokeUserCode_CmdOpenHood, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _hoodStateByte);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _hoodStateByte);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _hoodStateByte, _Mirror_SyncVarHookDelegate__hoodStateByte, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _hoodStateByte, _Mirror_SyncVarHookDelegate__hoodStateByte, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}

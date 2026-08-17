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
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Handbrake
{
	public class Handbrake : AttachableObject, INetworkSaveable
	{
		[SyncVar(hook = "OnHandbrakeStateChanged")]
		private bool _isHandbrakeOn;

		[SerializeField]
		private Transform bodyTransform;

		[SerializeField]
		private Vector3 _offRotation;

		[SerializeField]
		private Vector3 _onRotation;

		[Header("Audio")]
		[SerializeField]
		private SoundID handbrakeOnSound;

		[SerializeField]
		private SoundID handbrakeOffSound;

		private Quaternion _initialBodyLocalRotation;

		private Tween _bodyTween;

		private InteractionStateMachine<HandbrakeInteractionState> _handbrakeStateMachine;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isHandbrakeOn;

		public UnityEvent OnHandbrakeButtonOn { get; } = new UnityEvent();

		public UnityEvent OnHandbrakeButtonOff { get; } = new UnityEvent();

		public bool IsHandbrakeOn => _isHandbrakeOn;

		public HandbrakeSlot HandbrakeSlot { get; set; }

		protected new InteractionStateMachine<HandbrakeInteractionState> StateMachine => _handbrakeStateMachine;

		public new HandbrakeInteractionState CurrentState => _handbrakeStateMachine?.CurrentState ?? HandbrakeInteractionState.Detached;

		protected override bool UseDefaultStateMachine => false;

		public string ContributorKey => "handbrake";

		public bool Network_isHandbrakeOn
		{
			get
			{
				return _isHandbrakeOn;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isHandbrakeOn, 1024uL, _Mirror_SyncVarHookDelegate__isHandbrakeOn);
			}
		}

		protected override void InitializeStateMachine()
		{
			_handbrakeStateMachine = new InteractionStateMachine<HandbrakeInteractionState>(this);
			base.BaseStateMachine = _handbrakeStateMachine;
			ConfigureStates();
			_handbrakeStateMachine.Initialize(DetermineHandbrakeState());
		}

		protected override void ConfigureStates()
		{
			_handbrakeStateMachine.RegisterState(HandbrakeInteractionState.Detached, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithInteractionLabelVisibility(visible: false)
				.WithNameLabelVisibility(visible: true)).RegisterState(HandbrakeInteractionState.AttachedOff, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach()).WithBasicInteraction(InteractionKey.Primary, "@interaction.on", HandleHandbrakeOn)
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(HandbrakeInteractionState.AttachedOn, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach()).WithBasicInteraction(InteractionKey.Primary, "@interaction.off", HandleHandbrakeOff)
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		private HandbrakeInteractionState DetermineHandbrakeState()
		{
			if (!base.IsAttached)
			{
				return HandbrakeInteractionState.Detached;
			}
			if (!_isHandbrakeOn)
			{
				return HandbrakeInteractionState.AttachedOff;
			}
			return HandbrakeInteractionState.AttachedOn;
		}

		public override void UpdateState()
		{
			if (_handbrakeStateMachine != null)
			{
				_handbrakeStateMachine.TransitionTo(DetermineHandbrakeState());
			}
		}

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void CaptureInitialAnimatedPoses()
		{
			if (bodyTransform != null)
			{
				_initialBodyLocalRotation = bodyTransform.localRotation;
			}
		}

		protected override void ResetAnimatedPoseOnDetach()
		{
			if (_bodyTween.isAlive)
			{
				_bodyTween.Stop();
			}
			if (bodyTransform != null)
			{
				bodyTransform.localRotation = Quaternion.Euler(_offRotation);
			}
			if (base.isServer)
			{
				Network_isHandbrakeOn = false;
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (_isHandbrakeOn)
			{
				bodyTransform.localRotation = Quaternion.Euler(_onRotation);
				OnHandbrakeButtonOn.Invoke();
			}
			else
			{
				bodyTransform.localRotation = Quaternion.Euler(_offRotation);
			}
			UpdateState();
		}

		private void HandleHandbrakeOn()
		{
			CmdSetHandbrakeState(isOn: true);
		}

		private void HandleHandbrakeOff()
		{
			CmdSetHandbrakeState(isOn: false);
		}

		public void ToggleFromInput()
		{
			if (base.IsAttached)
			{
				CmdSetHandbrakeState(!_isHandbrakeOn);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetHandbrakeState(bool isOn)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(isOn);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Parts.Handbrake.Handbrake::CmdSetHandbrakeState(System.Boolean)", 1270827107, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnHandbrakeStateChanged(bool oldState, bool newState)
		{
			if (IsLateJoinCompleted)
			{
				if (newState)
				{
					ActivateHandbrakeActions();
					AudioManager?.PlayOneShotAttached(handbrakeOnSound, base.gameObject);
				}
				else
				{
					DeactivateHandbrakeActions();
					AudioManager?.PlayOneShotAttached(handbrakeOffSound, base.gameObject);
				}
				UpdateState();
			}
		}

		private void ActivateHandbrakeActions()
		{
			_bodyTween = Tween.LocalRotation(bodyTransform, Quaternion.Euler(_onRotation), 1f, Ease.OutCubic).OnComplete(delegate
			{
				OnHandbrakeButtonOn.Invoke();
			});
		}

		private void DeactivateHandbrakeActions()
		{
			_bodyTween = Tween.LocalRotation(bodyTransform, Quaternion.Euler(_offRotation), 0.5f, Ease.OutExpo).OnComplete(delegate
			{
				OnHandbrakeButtonOff.Invoke();
			});
		}

		public void OnAttachedActions()
		{
			UpdateState();
		}

		public void OnDetachedActions()
		{
			if (_isHandbrakeOn)
			{
				CmdSetHandbrakeState(isOn: false);
			}
			UpdateState();
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_isHandbrakeOn);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			bool network_isHandbrakeOn = reader.ReadBool();
			if (NetworkServer.active)
			{
				Network_isHandbrakeOn = network_isHandbrakeOn;
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public Handbrake()
		{
			_Mirror_SyncVarHookDelegate__isHandbrakeOn = OnHandbrakeStateChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetHandbrakeState__Boolean(bool isOn)
		{
			Network_isHandbrakeOn = isOn;
		}

		protected static void InvokeUserCode_CmdSetHandbrakeState__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetHandbrakeState called on client.");
			}
			else
			{
				((Handbrake)obj).UserCode_CmdSetHandbrakeState__Boolean(reader.ReadBool());
			}
		}

		static Handbrake()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Handbrake), "System.Void NomadDrive.Features.Vehicle.Parts.Handbrake.Handbrake::CmdSetHandbrakeState(System.Boolean)", InvokeUserCode_CmdSetHandbrakeState__Boolean, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isHandbrakeOn);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteBool(_isHandbrakeOn);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isHandbrakeOn, _Mirror_SyncVarHookDelegate__isHandbrakeOn, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isHandbrakeOn, _Mirror_SyncVarHookDelegate__isHandbrakeOn, reader.ReadBool());
			}
		}
	}
}

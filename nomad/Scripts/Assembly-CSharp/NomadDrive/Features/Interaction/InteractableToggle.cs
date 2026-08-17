using System;
using System.Runtime.InteropServices;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public class InteractableToggle : Interactable
	{
		[SyncVar(hook = "OnToggleStateChange")]
		private byte _toggleStateByte;

		public UnityEvent onToggleOn;

		public UnityEvent onToggleOff;

		[SerializeField]
		private float toggleDuration = 0.2f;

		[SerializeField]
		private float toggleAngle = 40f;

		private Vector3 _defaultRotation;

		private Vector3 _targetRotation;

		private InteractionStateMachine<ToggleState> _stateMachine;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__toggleStateByte;

		public ToggleState ToggleState
		{
			get
			{
				return _toggleStateByte switch
				{
					0 => ToggleState.Off, 
					1 => ToggleState.On, 
					_ => ToggleState.Off, 
				};
			}
			private set
			{
				Network_toggleStateByte = value switch
				{
					ToggleState.Off => 0, 
					ToggleState.On => 1, 
					_ => 0, 
				};
			}
		}

		protected override bool UseStateMachine => true;

		public byte Network_toggleStateByte
		{
			get
			{
				return _toggleStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _toggleStateByte, 64uL, _Mirror_SyncVarHookDelegate__toggleStateByte);
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<ToggleState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(ToggleState.Off, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.on", ToggleOn).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(ToggleState.On, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.off", ToggleOff).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		private ToggleState DetermineState()
		{
			return ToggleState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		protected override void Start()
		{
			base.Start();
			_defaultRotation = base.transform.localEulerAngles;
			_targetRotation = base.transform.localEulerAngles + new Vector3(0f, toggleAngle, 0f);
		}

		private void OnToggleStateChange(byte _, byte newState)
		{
			SetInteractionAvailability(newValue: false);
			if (newState == 1)
			{
				Tween.LocalRotation(base.transform, Quaternion.Euler(_targetRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(InteractableToggle target)
				{
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
				return;
			}
			Tween.LocalRotation(base.transform, Quaternion.Euler(_defaultRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(InteractableToggle target)
			{
				target.onToggleOff.Invoke();
				target.SetInteractionAvailability(newValue: true);
				target.UpdateState();
			});
		}

		[Command(requiresAuthority = false)]
		public void CmdSetToggleState(ToggleState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EInteraction_002EToggleState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableToggle::CmdSetToggleState(NomadDrive.Features.Interaction.ToggleState)", 1327381621, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void ToggleOn()
		{
			CmdSetToggleState(ToggleState.On);
			onToggleOn.Invoke();
		}

		public void ToggleOff()
		{
			CmdSetToggleState(ToggleState.Off);
		}

		public InteractableToggle()
		{
			_Mirror_SyncVarHookDelegate__toggleStateByte = OnToggleStateChange;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetToggleState__ToggleState(ToggleState state)
		{
			ToggleState = state;
		}

		protected static void InvokeUserCode_CmdSetToggleState__ToggleState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetToggleState called on client.");
			}
			else
			{
				((InteractableToggle)obj).UserCode_CmdSetToggleState__ToggleState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EInteraction_002EToggleState(reader));
			}
		}

		static InteractableToggle()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableToggle), "System.Void NomadDrive.Features.Interaction.InteractableToggle::CmdSetToggleState(NomadDrive.Features.Interaction.ToggleState)", InvokeUserCode_CmdSetToggleState__ToggleState, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _toggleStateByte);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _toggleStateByte);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _toggleStateByte, _Mirror_SyncVarHookDelegate__toggleStateByte, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _toggleStateByte, _Mirror_SyncVarHookDelegate__toggleStateByte, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}

using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.Extensions;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public class InteractableHandle : Interactable
	{
		public UnityEvent onHandleRotateTarget;

		public UnityEvent onHandleRotateInitial;

		public CustomDirection customDirection;

		public float handleRotateDuration = 0.4f;

		public float handleRotateAngle = 90f;

		[Header("Sound Settings")]
		[SerializeField]
		private SoundID openingSound;

		[SerializeField]
		private SoundID closingSound;

		[SyncVar(hook = "OnHandleStateChange")]
		private byte _handleStateByte;

		private Vector3 _initialRotation;

		private InteractionStateMachine<HandleState> _stateMachine;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__handleStateByte;

		public HandleState HandleState
		{
			get
			{
				return _handleStateByte switch
				{
					0 => HandleState.Closed, 
					1 => HandleState.Opened, 
					_ => HandleState.Closed, 
				};
			}
			private set
			{
				Network_handleStateByte = value switch
				{
					HandleState.Closed => 0, 
					HandleState.Opened => 1, 
					_ => 0, 
				};
			}
		}

		protected override bool UseStateMachine => true;

		public byte Network_handleStateByte
		{
			get
			{
				return _handleStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _handleStateByte, 64uL, _Mirror_SyncVarHookDelegate__handleStateByte);
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<HandleState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(HandleState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", RotateHandleToTarget).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(HandleState.Opened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", RotateHandleToInitial).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		private HandleState DetermineState()
		{
			return HandleState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		protected override void Awake()
		{
			base.Awake();
			_initialRotation = base.transform.localEulerAngles;
			onHandleRotateTarget.AddListener(OnHandleRotateTarget);
			onHandleRotateInitial.AddListener(OnHandleRotateInitial);
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SetForLateJoiners();
		}

		private void SetForLateJoiners()
		{
			if (HandleState == HandleState.Opened)
			{
				Vector3 globalDirectionVector = customDirection.GetGlobalDirectionVector();
				base.transform.localRotation = Quaternion.Euler(_initialRotation + globalDirectionVector * handleRotateAngle);
			}
			else
			{
				base.transform.localRotation = Quaternion.Euler(_initialRotation);
			}
		}

		private void RotateHandleToTarget()
		{
			SetInteractionAvailability(newValue: false);
			CmdChangeHandleState(HandleState.Opened);
		}

		private void RotateHandleToInitial()
		{
			SetInteractionAvailability(newValue: false);
			CmdChangeHandleState(HandleState.Closed);
		}

		private void OnHandleStateChange(byte _, byte newState)
		{
			if (!IsLateJoinCompleted)
			{
				return;
			}
			if (newState == 0)
			{
				PlaySoundNetworked(closingSound);
				Tween.LocalRotation(base.transform, Quaternion.Euler(_initialRotation), handleRotateDuration).OnComplete(this, delegate(InteractableHandle target)
				{
					target.onHandleRotateInitial.Invoke();
				});
				return;
			}
			PlaySoundNetworked(openingSound);
			Vector3 globalDirectionVector = customDirection.GetGlobalDirectionVector();
			Tween.LocalRotation(base.transform, Quaternion.Euler(_initialRotation + globalDirectionVector * handleRotateAngle), handleRotateDuration).OnComplete(this, delegate(InteractableHandle target)
			{
				target.onHandleRotateTarget.Invoke();
			});
		}

		private void PlaySoundNetworked(SoundID sound)
		{
			if (sound.IsValid() && base.isServer)
			{
				NetworkAudioRelay?.PlayOneShot(sound, base.transform.position);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdChangeHandleState(HandleState newState)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EInteraction_002EHandleState(writer, newState);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.InteractableHandle::CmdChangeHandleState(NomadDrive.Features.Interaction.HandleState)", 215794859, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnHandleRotateTarget()
		{
			SetInteractionAvailability(newValue: true);
			UpdateState();
		}

		private void OnHandleRotateInitial()
		{
			SetInteractionAvailability(newValue: true);
			UpdateState();
		}

		public InteractableHandle()
		{
			_Mirror_SyncVarHookDelegate__handleStateByte = OnHandleStateChange;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdChangeHandleState__HandleState(HandleState newState)
		{
			HandleState = newState;
		}

		protected static void InvokeUserCode_CmdChangeHandleState__HandleState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdChangeHandleState called on client.");
			}
			else
			{
				((InteractableHandle)obj).UserCode_CmdChangeHandleState__HandleState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EInteraction_002EHandleState(reader));
			}
		}

		static InteractableHandle()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(InteractableHandle), "System.Void NomadDrive.Features.Interaction.InteractableHandle::CmdChangeHandleState(NomadDrive.Features.Interaction.HandleState)", InvokeUserCode_CmdChangeHandleState__HandleState, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _handleStateByte);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _handleStateByte);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _handleStateByte, _Mirror_SyncVarHookDelegate__handleStateByte, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _handleStateByte, _Mirror_SyncVarHookDelegate__handleStateByte, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}

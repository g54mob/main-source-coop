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
	public class NetworkedInteractableLightSwitch : Interactable
	{
		[Header("Toggle Settings")]
		[SerializeField]
		private Transform toggleTransform;

		[SerializeField]
		private SwitchRotationDirection rotationDirection;

		[SerializeField]
		private float toggleDuration = 0.2f;

		[SerializeField]
		private float toggleAngle = 40f;

		[Header("Lights")]
		[SerializeField]
		private bool initiallyOn;

		[SerializeField]
		private Light[] lights;

		[Header("Emission")]
		[SerializeField]
		private Renderer[] emissionRenderers;

		[SerializeField]
		private int materialIndex;

		[SerializeField]
		private string emissionPropertyName = "_EmissiveExposureWeight";

		[SerializeField]
		private float emissionDuration = 0.3f;

		[Header("Events")]
		public UnityEvent onToggleOn = new UnityEvent();

		public UnityEvent onToggleOff = new UnityEvent();

		[SyncVar(hook = "OnStateChanged")]
		private byte _stateByte;

		private InteractionStateMachine<LightSwitchState> _stateMachine;

		private Vector3 _defaultRotation;

		private Vector3 _targetRotation;

		private Material[] _emissionMaterials;

		private int _emissionPropertyId;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__stateByte;

		public LightSwitchState SwitchState => (LightSwitchState)_stateByte;

		public bool IsOn => _stateByte == 1;

		protected override bool UseStateMachine => true;

		public byte Network_stateByte
		{
			get
			{
				return _stateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _stateByte, 64uL, _Mirror_SyncVarHookDelegate__stateByte);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (toggleTransform == null)
			{
				toggleTransform = base.transform;
			}
			_defaultRotation = toggleTransform.localEulerAngles;
			Vector3 vector = ((rotationDirection == SwitchRotationDirection.Vertical) ? new Vector3(toggleAngle, 0f, 0f) : new Vector3(0f, toggleAngle, 0f));
			_targetRotation = _defaultRotation + vector;
			CacheEmissionMaterials();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (initiallyOn)
			{
				Network_stateByte = 1;
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<LightSwitchState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(LightSwitchState.Off, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.turn_on", TurnOn).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(LightSwitchState.On, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.turn_off", TurnOff).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		private LightSwitchState DetermineState()
		{
			return (LightSwitchState)_stateByte;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		private void OnStateChanged(byte oldValue, byte newValue)
		{
			if (IsLateJoinCompleted)
			{
				LightSwitchState state = (LightSwitchState)newValue;
				AnimateToState(state);
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (_stateByte == 1)
			{
				toggleTransform.localRotation = Quaternion.Euler(_targetRotation);
				SetLights(on: true);
				SetEmission(0f);
			}
			else
			{
				toggleTransform.localRotation = Quaternion.Euler(_defaultRotation);
				SetLights(on: false);
				SetEmission(1f);
			}
			UpdateState();
		}

		private void AnimateToState(LightSwitchState state)
		{
			SetInteractionAvailability(newValue: false);
			bool flag = state == LightSwitchState.On;
			AnimateEmission(flag);
			if (flag)
			{
				Tween.LocalRotation(toggleTransform, Quaternion.Euler(_targetRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(NetworkedInteractableLightSwitch target)
				{
					target.SetLights(on: true);
					target.onToggleOn.Invoke();
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
			}
			else
			{
				Tween.LocalRotation(toggleTransform, Quaternion.Euler(_defaultRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(NetworkedInteractableLightSwitch target)
				{
					target.SetLights(on: false);
					target.onToggleOff.Invoke();
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
			}
		}

		private void CacheEmissionMaterials()
		{
			_emissionPropertyId = Shader.PropertyToID(emissionPropertyName);
			if (emissionRenderers == null || emissionRenderers.Length == 0)
			{
				return;
			}
			_emissionMaterials = new Material[emissionRenderers.Length];
			for (int i = 0; i < emissionRenderers.Length; i++)
			{
				if (emissionRenderers[i] != null)
				{
					_emissionMaterials[i] = emissionRenderers[i].materials[materialIndex];
				}
			}
		}

		private void SetEmission(float value)
		{
			if (_emissionMaterials != null)
			{
				Material[] emissionMaterials = _emissionMaterials;
				for (int i = 0; i < emissionMaterials.Length; i++)
				{
					emissionMaterials[i]?.SetFloat(_emissionPropertyId, value);
				}
			}
		}

		private void AnimateEmission(bool on)
		{
			if (_emissionMaterials != null)
			{
				float startValue = (on ? 1f : 0f);
				float endValue = (on ? 0f : 1f);
				Tween.Custom(this, startValue, endValue, emissionDuration, delegate(NetworkedInteractableLightSwitch target, float val)
				{
					target.SetEmission(val);
				});
			}
		}

		private void SetLights(bool on)
		{
			if (lights == null)
			{
				return;
			}
			Light[] array = lights;
			foreach (Light light in array)
			{
				if (light != null)
				{
					light.enabled = on;
				}
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdSetState(LightSwitchState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EInteraction_002ELightSwitchState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.NetworkedInteractableLightSwitch::CmdSetState(NomadDrive.Features.Interaction.LightSwitchState)", -463905584, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void TurnOn()
		{
			if (!IsOn)
			{
				CmdSetState(LightSwitchState.On);
			}
		}

		public void TurnOff()
		{
			if (IsOn)
			{
				CmdSetState(LightSwitchState.Off);
			}
		}

		public void Toggle()
		{
			if (IsOn)
			{
				TurnOff();
			}
			else
			{
				TurnOn();
			}
		}

		public NetworkedInteractableLightSwitch()
		{
			_Mirror_SyncVarHookDelegate__stateByte = OnStateChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetState__LightSwitchState(LightSwitchState state)
		{
			Network_stateByte = (byte)state;
		}

		protected static void InvokeUserCode_CmdSetState__LightSwitchState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetState called on client.");
			}
			else
			{
				((NetworkedInteractableLightSwitch)obj).UserCode_CmdSetState__LightSwitchState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EInteraction_002ELightSwitchState(reader));
			}
		}

		static NetworkedInteractableLightSwitch()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedInteractableLightSwitch), "System.Void NomadDrive.Features.Interaction.NetworkedInteractableLightSwitch::CmdSetState(NomadDrive.Features.Interaction.LightSwitchState)", InvokeUserCode_CmdSetState__LightSwitchState, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _stateByte);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _stateByte);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _stateByte, _Mirror_SyncVarHookDelegate__stateByte, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _stateByte, _Mirror_SyncVarHookDelegate__stateByte, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}

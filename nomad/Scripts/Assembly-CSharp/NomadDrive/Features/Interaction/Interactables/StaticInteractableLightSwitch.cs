using Ami.BroAudio;
using EvilCore.UI.Scripts;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction.Interactables
{
	public class StaticInteractableLightSwitch : StaticInteractable
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

		[Header("Sound Settings")]
		[SerializeField]
		private SoundID turnOnSound;

		[SerializeField]
		private SoundID turnOffSound;

		[Header("Events")]
		public UnityEvent onToggleOn = new UnityEvent();

		public UnityEvent onToggleOff = new UnityEvent();

		private StaticInteractionStateMachine<LightSwitchState> _stateMachine;

		private Vector3 _defaultRotation;

		private Vector3 _targetRotation;

		private LightSwitchState _currentState;

		private Material[] _emissionMaterials;

		private int _emissionPropertyId;

		public LightSwitchState SwitchState => _currentState;

		public bool IsOn => _currentState == LightSwitchState.On;

		protected override bool UseStateMachine => true;

		protected override void Awake()
		{
			_currentState = (initiallyOn ? LightSwitchState.On : LightSwitchState.Off);
			base.Awake();
		}

		protected override void Start()
		{
			if (toggleTransform == null)
			{
				toggleTransform = base.transform;
			}
			_defaultRotation = toggleTransform.localEulerAngles;
			Vector3 vector = ((rotationDirection == SwitchRotationDirection.Vertical) ? new Vector3(toggleAngle, 0f, 0f) : new Vector3(0f, toggleAngle, 0f));
			_targetRotation = _defaultRotation + vector;
			CacheEmissionMaterials();
			base.Start();
			bool flag = _currentState == LightSwitchState.On;
			SetLights(flag);
			SetEmission(flag ? 0f : 1f);
			toggleTransform.localRotation = Quaternion.Euler(flag ? _targetRotation : _defaultRotation);
			UpdateState();
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<LightSwitchState>(this);
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
			return _currentState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void ApplyStateFromManager(byte stateData, bool skipAnimation)
		{
			LightSwitchState lightSwitchState = (LightSwitchState)stateData;
			if (lightSwitchState == _currentState)
			{
				return;
			}
			_currentState = lightSwitchState;
			if (skipAnimation)
			{
				if (lightSwitchState == LightSwitchState.On)
				{
					toggleTransform.localRotation = Quaternion.Euler(_targetRotation);
					SetLights(on: true);
					SetEmission(0f);
					onToggleOn.Invoke();
				}
				else
				{
					toggleTransform.localRotation = Quaternion.Euler(_defaultRotation);
					SetLights(on: false);
					SetEmission(1f);
					onToggleOff.Invoke();
				}
				UpdateState();
			}
			else
			{
				AnimateToState(lightSwitchState);
			}
		}

		private void AnimateToState(LightSwitchState state)
		{
			SetInteractionAvailability(newValue: false);
			bool flag = state == LightSwitchState.On;
			AnimateEmission(flag);
			PlaySound(flag ? turnOnSound : turnOffSound);
			if (flag)
			{
				Tween.LocalRotation(toggleTransform, Quaternion.Euler(_targetRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(StaticInteractableLightSwitch target)
				{
					target.SetLights(on: true);
					target.onToggleOn.Invoke();
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
			}
			else
			{
				Tween.LocalRotation(toggleTransform, Quaternion.Euler(_defaultRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(StaticInteractableLightSwitch target)
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
				Tween.Custom(this, startValue, endValue, emissionDuration, delegate(StaticInteractableLightSwitch target, float val)
				{
					target.SetEmission(val);
				});
			}
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				AudioManager?.PlayOneShot(sound, base.transform.position);
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

		private void TurnOn()
		{
			if (!IsOn)
			{
				RequestStateChange(1);
			}
		}

		private void TurnOff()
		{
			if (IsOn)
			{
				RequestStateChange(0);
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
	}
}

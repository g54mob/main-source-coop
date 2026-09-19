using UnityEngine;
using VolumetricLightBeam.Scripts.HD;

namespace Features.JacuzziBeachInteractableModule.Scripts.VisualMusic
{
	public abstract class VisualMusicBehaviour : MonoBehaviour
	{
		private const float BLEND_EPSILON = 0.001f;

		[SerializeField]
		protected Transform LightsRoot;

		[SerializeField]
		private Light[] _lights;

		[Tooltip("Beams attached to the lights above; HD beams never re-read their light at runtime, so every change has to be pushed manually.")]
		[SerializeField]
		private VolumetricLightBeamHd[] _lightBeams;

		[SerializeField]
		private float _appearDuration = 0.75f;

		[SerializeField]
		private float _disappearDuration = 0.75f;

		private float[] _defaultRanges;

		private float[] _defaultIntensities;

		private Color[] _defaultColors;

		private Quaternion[] _defaultLocalRotations;

		private Quaternion _defaultRootLocalRotation;

		private float _blend;

		private bool _isTurnedOn;

		protected Light[] Lights => _lights;

		protected float[] DefaultIntensities => _defaultIntensities;

		protected Color[] DefaultColors => _defaultColors;

		protected Quaternion[] DefaultLocalRotations => _defaultLocalRotations;

		protected float Blend => _blend;

		private void Awake()
		{
			_defaultRanges = new float[_lights.Length];
			_defaultIntensities = new float[_lights.Length];
			_defaultColors = new Color[_lights.Length];
			_defaultLocalRotations = new Quaternion[_lights.Length];
			for (int i = 0; i < _lights.Length; i++)
			{
				_defaultRanges[i] = _lights[i].range;
				_defaultIntensities[i] = _lights[i].intensity;
				_defaultColors[i] = _lights[i].color;
				_defaultLocalRotations[i] = _lights[i].transform.localRotation;
				_lights[i].gameObject.SetActive(value: false);
			}
			_defaultRootLocalRotation = LightsRoot.localRotation;
			base.enabled = false;
		}

		public void InvokeEffect(bool isTurnedOn)
		{
			if (_isTurnedOn == isTurnedOn && base.enabled)
			{
				return;
			}
			_isTurnedOn = isTurnedOn;
			if (!isTurnedOn)
			{
				base.enabled = _lights.Length != 0;
				return;
			}
			for (int i = 0; i < _lights.Length; i++)
			{
				_lights[i].range = 0f;
				PushLightPropertiesToBeams();
				_lights[i].gameObject.SetActive(value: true);
			}
			base.enabled = true;
			OnEffectStarted();
		}

		protected abstract void OnEffectStarted();

		protected abstract void OnEffectTick(float deltaTime);

		protected abstract void OnEffectStopped();

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			float target = (_isTurnedOn ? 1f : 0f);
			float num = (_isTurnedOn ? _appearDuration : _disappearDuration);
			_blend = Mathf.MoveTowards(_blend, target, deltaTime / num);
			OnEffectTick(deltaTime);
			for (int i = 0; i < _lights.Length; i++)
			{
				_lights[i].range = Mathf.Lerp(0f, _defaultRanges[i], _blend);
			}
			PushLightPropertiesToBeams();
			if (!_isTurnedOn && !(_blend > 0.001f))
			{
				_blend = 0f;
				OnEffectStopped();
				RestoreDefaults();
				base.enabled = false;
			}
		}

		private void RestoreDefaults()
		{
			for (int i = 0; i < _lights.Length; i++)
			{
				_lights[i].range = _defaultRanges[i];
				_lights[i].intensity = _defaultIntensities[i];
				_lights[i].color = _defaultColors[i];
				_lights[i].transform.localRotation = _defaultLocalRotations[i];
				_lights[i].gameObject.SetActive(value: false);
			}
			LightsRoot.localRotation = _defaultRootLocalRotation;
			PushLightPropertiesToBeams();
		}

		private void PushLightPropertiesToBeams()
		{
			for (int i = 0; i < _lightBeams.Length; i++)
			{
				_lightBeams[i].AssignPropertiesFromAttachedSpotLight();
			}
		}
	}
}

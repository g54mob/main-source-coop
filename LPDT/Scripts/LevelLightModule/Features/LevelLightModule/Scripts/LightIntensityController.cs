using System.Collections;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	public class LightIntensityController : MonoBehaviour
	{
		[SerializeField]
		private Light _affectedLight;

		[SerializeField]
		private FlashlightRotator _flashlightRotator;

		[FormerlySerializedAs("_intensityInCrouch")]
		[SerializeField]
		private float _intensityMultiplierInCrouch = 0.5f;

		[SerializeField]
		private float _intensityChangeDuration;

		[SerializeField]
		private NetworkObject _networkObject;

		private PlayerMovableModel _playerMovableModel;

		private float _initialLightIntensity;

		private float _currentIntensityMultiplier = 1f;

		private MovementState _prevMovementState;

		private Coroutine _lightFadeRoutine;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		private void Start()
		{
			if (_affectedLight != null)
			{
				_initialLightIntensity = _affectedLight.intensity;
			}
			if (_flashlightRotator != null)
			{
				_flashlightRotator.SetIntensityMultiplier(_currentIntensityMultiplier);
			}
		}

		private void Update()
		{
			if ((_flashlightRotator == null && _affectedLight == null) || !_playerMovableModel.AllCharacterMovables.TryGetValue(_networkObject.InputAuthority, out var value) || value == null || value.Object == null)
			{
				return;
			}
			MovementState movementState = value.MovementState;
			if (movementState == _prevMovementState)
			{
				return;
			}
			if (movementState == MovementState.Crouching)
			{
				if (_lightFadeRoutine != null)
				{
					StopCoroutine(_lightFadeRoutine);
				}
				_lightFadeRoutine = StartCoroutine(LightFadeRoutine(_intensityMultiplierInCrouch, _intensityChangeDuration));
			}
			else if (_prevMovementState == MovementState.Crouching)
			{
				if (_lightFadeRoutine != null)
				{
					StopCoroutine(_lightFadeRoutine);
				}
				_lightFadeRoutine = StartCoroutine(LightFadeRoutine(1f, _intensityChangeDuration));
			}
			_prevMovementState = movementState;
		}

		private IEnumerator LightFadeRoutine(float endIntensityMultiplier, float duration)
		{
			float timer = 0f;
			float startIntensityMultiplier = _currentIntensityMultiplier;
			float startLightIntensity = ((_affectedLight != null) ? _affectedLight.intensity : 0f);
			float endLightIntensity = _initialLightIntensity * endIntensityMultiplier;
			while (timer < duration)
			{
				timer += Time.deltaTime;
				float t = timer / duration;
				_currentIntensityMultiplier = Mathf.Lerp(startIntensityMultiplier, endIntensityMultiplier, t);
				if (_flashlightRotator != null)
				{
					_flashlightRotator.SetIntensityMultiplier(_currentIntensityMultiplier);
				}
				else if (_affectedLight != null)
				{
					_affectedLight.intensity = Mathf.Lerp(startLightIntensity, endLightIntensity, t);
				}
				yield return null;
			}
			_currentIntensityMultiplier = endIntensityMultiplier;
			if (_flashlightRotator != null)
			{
				_flashlightRotator.SetIntensityMultiplier(_currentIntensityMultiplier);
			}
			else if (_affectedLight != null)
			{
				_affectedLight.intensity = endLightIntensity;
			}
		}
	}
}

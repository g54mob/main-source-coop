using Features.Movement.Scripts;
using UnityEngine;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	public class LightAttenuatorByDistance : MonoBehaviour
	{
		[SerializeField]
		private Light _light;

		[SerializeField]
		private float _minIntensity;

		[SerializeField]
		private float _distanceToMaxGain;

		private PlayerMovableModel _playerMovableModel;

		private float _originalIntensity;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		private void Start()
		{
			_originalIntensity = _light.intensity;
		}

		private void Update()
		{
			_light.intensity = CalculateIntensityByDistance(_playerMovableModel.LocalMovable.GetPosition());
		}

		private float CalculateIntensityByDistance(Vector3 playerPosition)
		{
			return Mathf.Lerp(_minIntensity, _originalIntensity, Mathf.Clamp01(Vector3.Distance(playerPosition, base.transform.position) / _distanceToMaxGain));
		}
	}
}

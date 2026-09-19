using Features.CameraModelModule;
using Features.LevelGatesModule.Data;
using Features.Movement.Scripts;
using UnityEngine;
using Zenject;

namespace Features.LevelGatesModule.Scripts
{
	public class GateCorridorLightBehaviour : MonoBehaviour, IGateCorridorLightPresetTarget
	{
		[SerializeField]
		private int _gateIndex;

		[SerializeField]
		private Light _light;

		[SerializeField]
		private float _distanceForMaxIntensity = 7.5f;

		[SerializeField]
		private float _attenuatedIntensity;

		[SerializeField]
		private float _exitBias = 0.5f;

		private LevelGatesModel _levelGateModel;

		private PlayerMovableModel _playerMovableModel;

		private IGateNavigationService _gateNavigationService;

		private CameraModel _cameraModel;

		private float _originalIntensity;

		private bool _isAttenuatedApplied;

		public int GateIndex => _gateIndex;

		public Transform Transform => base.transform;

		[Inject]
		public void InjectDependencies(LevelGatesModel levelGateModel, PlayerMovableModel playerMovableModel, IGateNavigationService gateNavigationService, CameraModel cameraModel)
		{
			_levelGateModel = levelGateModel;
			_playerMovableModel = playerMovableModel;
			_gateNavigationService = gateNavigationService;
			_cameraModel = cameraModel;
		}

		private void Start()
		{
			_originalIntensity = _light.intensity;
		}

		public void ApplyPreset(LightType type, float originalIntensity, Color color, float range, float spotAngle, float innerSpotAngle, float distanceForMaxIntensity, float attenuatedIntensity, float exitBias)
		{
			_originalIntensity = originalIntensity;
			_distanceForMaxIntensity = distanceForMaxIntensity;
			_attenuatedIntensity = attenuatedIntensity;
			_exitBias = exitBias;
			if (!(_light == null))
			{
				_light.type = type;
				_light.intensity = originalIntensity;
				_light.color = color;
				_light.range = range;
				_light.spotAngle = spotAngle;
				_light.innerSpotAngle = innerSpotAngle;
				_isAttenuatedApplied = false;
				if (!_levelGateModel.IsLocalPlayerInsideGate)
				{
					ApplyAttenuatedIntensityIfNeeded();
				}
			}
		}

		private void Update()
		{
			if (!_levelGateModel.IsLocalPlayerInsideGate)
			{
				ApplyAttenuatedIntensityIfNeeded();
				return;
			}
			_isAttenuatedApplied = false;
			Camera cameraObject = _cameraModel.CameraObject;
			if (!(cameraObject == null))
			{
				Vector3 position = cameraObject.transform.position;
				if (_gateNavigationService.TryGetClosestExitGate(position, out var closestGate))
				{
					float num = Mathf.Max((position - closestGate.Position).magnitude - _exitBias, 0f);
					_light.intensity = Mathf.Lerp(_attenuatedIntensity, _originalIntensity, Mathf.Clamp01(num / _distanceForMaxIntensity));
				}
			}
		}

		private void ApplyAttenuatedIntensityIfNeeded()
		{
			if (!_isAttenuatedApplied && !(_light == null))
			{
				_light.intensity = _attenuatedIntensity;
				_isAttenuatedApplied = true;
			}
		}
	}
}

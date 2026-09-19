using UnityEngine;
using VLB;

namespace VolumetricLightBeam.Scripts.HD
{
	public class LightFadeOnApproach : MonoBehaviour
	{
		[SerializeField]
		private float _startFadeDistance = 5f;

		[SerializeField]
		private float _endFadeDistance = 1f;

		[SerializeField]
		private float _minIntensity;

		[SerializeField]
		private VolumetricLightBeamSD _volumetricLightBeamSD;

		[SerializeField]
		private float _originalIntensityInside;

		[SerializeField]
		private float _originalIntensityOutside;

		private Transform _target;

		private void Start()
		{
			if (_volumetricLightBeamSD != null)
			{
				_originalIntensityInside = _volumetricLightBeamSD.intensityInside;
				_originalIntensityOutside = _volumetricLightBeamSD.intensityOutside;
			}
			if (Camera.main != null)
			{
				_target = Camera.main.transform;
			}
		}

		private void Update()
		{
			InitializeCameraValue();
			if (!(_target == null) && !(_volumetricLightBeamSD == null))
			{
				float num = Vector3.Distance(_target.position, base.transform.position);
				if (num >= _startFadeDistance)
				{
					_volumetricLightBeamSD.intensityInside = _originalIntensityInside;
					_volumetricLightBeamSD.intensityOutside = _originalIntensityOutside;
				}
				else if (num <= _endFadeDistance)
				{
					_volumetricLightBeamSD.intensityInside = _minIntensity;
					_volumetricLightBeamSD.intensityOutside = _minIntensity;
				}
				else
				{
					float t = Mathf.InverseLerp(_endFadeDistance, _startFadeDistance, num);
					_volumetricLightBeamSD.intensityInside = Mathf.Lerp(_minIntensity, _originalIntensityInside, t);
					_volumetricLightBeamSD.intensityOutside = Mathf.Lerp(_minIntensity, _originalIntensityOutside, t);
				}
				_volumetricLightBeamSD.UpdateAfterManualPropertyChange();
			}
		}

		private void InitializeCameraValue()
		{
			if (!(Camera.main == null) && (_target == null || _target != Camera.main.transform))
			{
				_target = Camera.main.transform;
			}
		}
	}
}

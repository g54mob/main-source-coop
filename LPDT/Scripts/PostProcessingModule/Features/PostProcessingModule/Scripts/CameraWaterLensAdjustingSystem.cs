using System;
using Features.PostProcessingModule.Scripts.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Features.PostProcessingModule.Scripts
{
	public class CameraWaterLensAdjustingSystem : IInitializable, IDisposable, ITickable
	{
		private const float DefaultWetnessFullDecaySeconds = 3.5f;

		private readonly PostProcessingModel _postProcessingModel;

		private readonly CameraWaterLensModel _cameraWaterLensModel;

		private readonly float _decayPerSecond;

		public CameraWaterLensAdjustingSystem(PostProcessingModel postProcessingModel, CameraWaterLensModel cameraWaterLensModel, CameraWaterLensConfiguration configuration)
		{
			_postProcessingModel = postProcessingModel;
			_cameraWaterLensModel = cameraWaterLensModel;
			_decayPerSecond = 1f / Mathf.Max(0.1f, (configuration != null) ? configuration.WetnessFullDecaySeconds : 3.5f);
		}

		public void Initialize()
		{
			_cameraWaterLensModel.OnWetnessChanged += ApplyWetnessToProfiles;
			ApplyWetnessToProfiles(_cameraWaterLensModel.Wetness);
		}

		public void Dispose()
		{
			_cameraWaterLensModel.OnWetnessChanged -= ApplyWetnessToProfiles;
		}

		public void Tick()
		{
			_cameraWaterLensModel.Decay(Time.deltaTime, _decayPerSecond);
		}

		private void ApplyWetnessToProfiles(float wetness)
		{
			foreach (Volume value in _postProcessingModel.ActiveVolumes.Values)
			{
				if (value.profile.TryGet<CameraWaterLensVolume>(out var component))
				{
					component.intensity.Override(wetness);
				}
			}
		}
	}
}

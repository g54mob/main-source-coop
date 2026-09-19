using Features.PostProcessingModule.Scripts;
using Features.WeatherModule.Scripts;
using Fusion;
using INab.BetterFog.URP;
using UnityEngine;
using Zenject;

namespace Features.FogModule.Scripts
{
	public class FogTransitionBehaviour : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		private PostProcessingType _postProcessingVolumeKey = PostProcessingType.SessionSceneMain;

		private FogRegionsModel _fogRegionsModel;

		private WeatherModel _weatherModel;

		private PostProcessingModel _postProcessingModel;

		[Inject]
		public void InjectDependencies(FogRegionsModel fogRegionsModel, WeatherModel weatherModel, PostProcessingModel postProcessingModel)
		{
			_fogRegionsModel = fogRegionsModel;
			_weatherModel = weatherModel;
			_postProcessingModel = postProcessingModel;
		}

		private void Update()
		{
			if (!_networkObject.HasInputAuthority || !_fogRegionsModel.DebugFogTransitionEnabled || _weatherModel.DebugUsedWeather == null)
			{
				return;
			}
			FogBlend currentBlend = _fogRegionsModel.CurrentBlend;
			if (currentBlend.IsValid)
			{
				RenderSettings.ambientSkyColor = Color.Lerp(GetAmbientColor(currentBlend.StartLocation), GetAmbientColor(currentBlend.EndLocation), currentBlend.Ratio);
				RenderSettings.fogColor = Color.Lerp(GetFogColor(currentBlend.StartLocation), GetFogColor(currentBlend.EndLocation), currentBlend.Ratio);
				if (TryGetBetterFog(out var betterFog))
				{
					betterFog._FogColor.value = Color.Lerp(GetVolumetricFogColor(currentBlend.StartLocation), GetVolumetricFogColor(currentBlend.EndLocation), currentBlend.Ratio);
					betterFog._FogColor.overrideState = true;
				}
			}
		}

		private Color GetAmbientColor(FogLocationType fogLocationType)
		{
			if (fogLocationType != FogLocationType.Beach)
			{
				return _weatherModel.DebugUsedWeather.DebugLocationAmbientColor;
			}
			return _weatherModel.DebugUsedWeather.DebugBeachAmbientColor;
		}

		private Color GetFogColor(FogLocationType fogLocationType)
		{
			if (fogLocationType != FogLocationType.Beach)
			{
				return _weatherModel.DebugUsedWeather.DebugLocationFogColor;
			}
			return _weatherModel.DebugUsedWeather.DebugBeachFogColor;
		}

		private Color GetVolumetricFogColor(FogLocationType fogLocationType)
		{
			if (fogLocationType != FogLocationType.Beach)
			{
				return _weatherModel.DebugUsedWeather.DebugVolumetricLocationFogColor;
			}
			return _weatherModel.DebugUsedWeather.DebugVolumetricBeachFogColor;
		}

		private bool TryGetBetterFog(out BetterFogVolumeComponent betterFog)
		{
			betterFog = null;
			if (_postProcessingModel == null || _postProcessingVolumeKey == PostProcessingType.None)
			{
				return false;
			}
			if (!_postProcessingModel.ActiveVolumes.TryGetValue(_postProcessingVolumeKey, out var value) || value == null || value.profile == null)
			{
				return false;
			}
			return value.profile.TryGet<BetterFogVolumeComponent>(out betterFog);
		}
	}
}

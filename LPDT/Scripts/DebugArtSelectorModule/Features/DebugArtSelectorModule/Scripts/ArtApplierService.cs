using Features.FogModule.Scripts;
using Features.WeatherModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.DebugArtSelectorModule.Scripts
{
	public class ArtApplierService : IArtApplierService
	{
		private readonly WeatherModel _weatherModel;

		private readonly DiContainer _diContainer;

		private readonly FogRegionsModel _fogRegionsModel;

		public ArtApplierService(WeatherModel weatherModel, DiContainer diContainer, FogRegionsModel fogRegionsModel)
		{
			_weatherModel = weatherModel;
			_diContainer = diContainer;
			_fogRegionsModel = fogRegionsModel;
		}

		public void ApplyArtConfiguration(ArtConfiguration artConfiguration)
		{
			if (artConfiguration.UseDebugAmbientFromLevel)
			{
				RenderSettings.ambientSkyColor = _weatherModel.DebugUsedWeather?.DebugAdaptiveAmbientColor ?? RenderSettings.ambientSkyColor;
				RenderSettings.fogColor = _weatherModel.DebugUsedWeather?.DebugAdaptiveFogColor ?? artConfiguration.FogColor;
			}
			else
			{
				RenderSettings.ambientSkyColor = artConfiguration.AmbientColor;
				RenderSettings.fogColor = artConfiguration.FogColor;
			}
			RenderSettings.subtractiveShadowColor = artConfiguration.RealtimeShadowColor;
			RenderSettings.fog = artConfiguration.Fog;
			RenderSettings.fogMode = artConfiguration.FogMode;
			RenderSettings.fogStartDistance = artConfiguration.FogStartDistance;
			RenderSettings.fogEndDistance = artConfiguration.FogEndDistance;
			if (artConfiguration.ReApplyWeather)
			{
				_weatherModel.DebugUsedWeather?.Apply(null, _diContainer);
			}
			_fogRegionsModel.DebugFogTransitionEnabled = artConfiguration.FogTransitionEnabled;
		}
	}
}

using UnityEngine;

namespace Enviro
{
	public class EnviroConfiguration : ScriptableObject
	{
		public string version = "";

		public EnviroTimeModule timeModule;

		public EnviroLightingModule lightingModule;

		public EnviroReflectionsModule reflectionsModule;

		public EnviroSkyModule Sky;

		public EnviroFogModule fogModule;

		public EnviroVolumetricCloudsModule volumetricCloudModule;

		public EnviroFlatCloudsModule flatCloudModule;

		public EnviroWeatherModule Weather;

		public EnviroAuroraModule Aurora;

		public EnviroAudioModule Audio;

		public EnviroEffectsModule Effects;

		public EnviroLightningModule Lightning;

		public EnviroQualityModule Quality;

		public EnviroEnvironmentModule Environment;
	}
}

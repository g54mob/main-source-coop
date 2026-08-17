using UnityEngine;

namespace Enviro
{
	public class EnviroConfigurationCreation
	{
		public static EnviroConfiguration CreateMyAsset()
		{
			EnviroConfiguration enviroConfiguration = ScriptableObject.CreateInstance<EnviroConfiguration>();
			enviroConfiguration.version = "3.3.0";
			return enviroConfiguration;
		}
	}
}

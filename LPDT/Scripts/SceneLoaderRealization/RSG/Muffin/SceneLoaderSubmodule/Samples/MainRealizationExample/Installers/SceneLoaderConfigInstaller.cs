using System.Linq;
using RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts;
using UnityEngine;
using Zenject;

namespace RSG.Muffin.SceneLoaderSubmodule.Samples.MainRealizationExample.Installers
{
	public class SceneLoaderConfigInstaller : Installer<SceneLoaderConfigInstaller>
	{
		private const string CONFIGURATIONS_PATH = "SceneLoaderConfigurations";

		public override void InstallBindings()
		{
			SceneLoaderConfig[] array = Resources.LoadAll<SceneLoaderConfig>("SceneLoaderConfigurations");
			if (array.Length == 0)
			{
				Debug.LogWarning("No SceneLoaderConfig resources found in the Resources folder.");
				return;
			}
			if (array.Length > 1)
			{
				Debug.LogError($"Multiple SceneLoaderConfig resources found: {array.Length} configurations found.");
				for (int i = 0; i < array.Length; i++)
				{
					Debug.LogError($"Configuration {i + 1}: {array[i].name})");
				}
			}
			base.Container.Bind<SceneLoaderConfig>().FromScriptableObject(array.First()).AsSingle();
		}
	}
}

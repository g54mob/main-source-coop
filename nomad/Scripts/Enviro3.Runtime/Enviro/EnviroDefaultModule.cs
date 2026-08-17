using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroDefaultModule : EnviroModule
	{
		public EnviroDefault settings;

		public EnviroDefaultModule preset;

		public bool showDefaultControls;

		public override void UpdateModule()
		{
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				settings = JsonUtility.FromJson<EnviroDefault>(JsonUtility.ToJson(preset.settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroDefaultModule module)
		{
			module.settings = JsonUtility.FromJson<EnviroDefault>(JsonUtility.ToJson(settings));
		}
	}
}

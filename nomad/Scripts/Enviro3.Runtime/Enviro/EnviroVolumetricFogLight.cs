using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
	[ExecuteInEditMode]
	[AddComponentMenu("Enviro 3/Volumetric Light")]
	public class EnviroVolumetricFogLight : MonoBehaviour
	{
		[Range(0f, 2f)]
		public float intensity = 1f;

		[Range(0f, 2f)]
		public float range = 1f;

		private Light myLight;

		private bool initialized;

		private CommandBuffer cascadeShadowCB;

		public bool isOn
		{
			get
			{
				if (!base.isActiveAndEnabled)
				{
					return false;
				}
				Init();
				return myLight.enabled;
			}
			private set
			{
			}
		}

		public Light light
		{
			get
			{
				Init();
				return myLight;
			}
			private set
			{
			}
		}

		private void OnEnable()
		{
			Init();
			if (EnviroManager.instance != null && EnviroManager.instance.Fog != null)
			{
				AddToLightManager();
			}
		}

		private void OnDisable()
		{
			if (EnviroManager.instance != null && EnviroManager.instance.Fog != null)
			{
				RemoveFromLightManager();
			}
		}

		private void AddToLightManager()
		{
			bool flag = false;
			for (int i = 0; i < EnviroManager.instance.Fog.fogLights.Count; i++)
			{
				if (EnviroManager.instance.Fog.fogLights[i] == this)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				EnviroManager.instance.Fog.AddLight(this);
			}
		}

		private void RemoveFromLightManager()
		{
			for (int i = 0; i < EnviroManager.instance.Fog.fogLights.Count; i++)
			{
				if (EnviroManager.instance.Fog.fogLights[i] == this)
				{
					EnviroManager.instance.Fog.RemoveLight(this);
					initialized = false;
				}
			}
		}

		private void Init()
		{
			if (!initialized)
			{
				myLight = GetComponent<Light>();
				initialized = true;
			}
		}
	}
}

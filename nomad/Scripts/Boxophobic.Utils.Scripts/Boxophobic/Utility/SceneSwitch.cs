using Boxophobic.StyledGUI;
using UnityEngine;
using UnityEngine.Rendering;

namespace Boxophobic.Utility
{
	[ExecuteInEditMode]
	public class SceneSwitch : StyledMonoBehaviour
	{
		[StyledBanner("Switch")]
		public bool styledBanner;

		public GameObject setupStandard;

		public GameObject setupUniversal;

		public GameObject setupHD;

		[HideInInspector]
		public GameObject objectStandard;

		[HideInInspector]
		public GameObject objectUniversal;

		[HideInInspector]
		public GameObject objectHD;

		[Space(10f)]
		public bool setRenderSettings;

		[Space(10f)]
		public Material skyboxMaterial;

		[Range(0f, 8f)]
		public float skyboxAmbient = 1f;

		[Range(0f, 1f)]
		public float skyboxReflection = 1f;

		[StyledSpace(5)]
		public bool styledSpace;

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				return;
			}
			int num = 0;
			if (GraphicsSettings.defaultRenderPipeline != null)
			{
				if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("Universal"))
				{
					num = 1;
				}
				if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("HD"))
				{
					num = 2;
				}
			}
			if (QualitySettings.renderPipeline != null)
			{
				if (QualitySettings.renderPipeline.GetType().ToString().Contains("Universal"))
				{
					num = 1;
				}
				if (QualitySettings.renderPipeline.GetType().ToString().Contains("HD"))
				{
					num = 2;
				}
			}
			if (num == 0 && setupStandard != null && objectStandard == null)
			{
				objectStandard = Object.Instantiate(setupStandard, base.gameObject.transform);
				objectStandard.name = objectStandard.name.Replace("(Clone)", "");
			}
			if (num == 1 && setupUniversal != null && objectUniversal == null)
			{
				objectUniversal = Object.Instantiate(setupUniversal, base.gameObject.transform);
				objectUniversal.name = objectUniversal.name.Replace("(Clone)", "");
			}
			if (num == 2 && setupHD != null && objectHD == null)
			{
				objectHD = Object.Instantiate(setupHD, base.gameObject.transform);
				objectHD.name = objectHD.name.Replace("(Clone)", "");
			}
			if (setRenderSettings)
			{
				RenderSettings.skybox = skyboxMaterial;
				RenderSettings.ambientIntensity = skyboxAmbient;
				RenderSettings.reflectionIntensity = skyboxReflection;
				DynamicGI.UpdateEnvironment();
			}
		}
	}
}

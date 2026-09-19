using System;
using System.Collections.Generic;
using INab.BetterFog.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	public class BetterFog : ScriptableRendererFeature
	{
		[Serializable]
		public class BetterFogSettings
		{
			public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;

			public bool _UseCustomDepthTexture;

			public bool _UseFogOffsetTexture;

			public bool _DisableSceneView;
		}

		public BetterFogSettings m_Settings = new BetterFogSettings();

		private Material m_FogFactorMaterial;

		private Material m_FogBlendMaterial;

		private Material m_SMSSMaterial;

		private Material m_CustomDepthPassMaterial;

		[SerializeField]
		[HideInInspector]
		private Shader m_FogFactorShader;

		[SerializeField]
		[HideInInspector]
		private Shader m_FogBlendShader;

		[SerializeField]
		[HideInInspector]
		private Shader m_SMSSShader;

		[SerializeField]
		[HideInInspector]
		private Shader m_CustomDepthPassShader;

		private TemporaryBlitPass temporaryBlitPass;

		private FogBlendBlit fogBlendBlit;

		private FogFactorBlit fogFactorBlit;

		private SMSSPass smssPass;

		private CustomDepthPass customDepthPass;

		private FogOffsetPass fogOffsetPass;

		private BetterFogVolumeComponent m_BetterFogVolume;

		public static int kMaxIterations = 16;

		private List<CustomRenderer> m_DepthRenderers = new List<CustomRenderer>();

		private List<CustomRenderer> m_FogOffsetRenderers = new List<CustomRenderer>();

		public bool m_UseSMSS => m_BetterFogVolume._UseSSMS.value;

		public override void Create()
		{
			temporaryBlitPass = new TemporaryBlitPass();
			fogBlendBlit = new FogBlendBlit();
			fogFactorBlit = new FogFactorBlit();
			smssPass = new SMSSPass();
			customDepthPass = new CustomDepthPass();
			fogOffsetPass = new FogOffsetPass();
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if ((renderingData.cameraData.cameraType == CameraType.SceneView && m_Settings._DisableSceneView) || renderingData.cameraData.cameraType == CameraType.Preview || renderingData.cameraData.cameraType == CameraType.Reflection || !renderingData.cameraData.postProcessEnabled)
			{
				return;
			}
			m_BetterFogVolume = VolumeManager.instance.stack?.GetComponent<BetterFogVolumeComponent>();
			if (m_BetterFogVolume == null || !m_BetterFogVolume.IsActive())
			{
				return;
			}
			if (m_FogFactorMaterial == null)
			{
				m_FogFactorMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Shader Graphs/FogFactor"));
			}
			if (m_FogBlendMaterial == null)
			{
				m_FogBlendMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Shader Graphs/FogBlend"));
			}
			if (m_SMSSMaterial == null)
			{
				m_SMSSMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/INabStudio/SSMS_URP"));
			}
			if (m_CustomDepthPassMaterial == null)
			{
				m_CustomDepthPassMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Shader Graphs/DepthBlit"));
			}
			if (renderingData.cameraData.cameraType == CameraType.Game)
			{
				BetterFogRenderers component = UnityEngine.Object.FindFirstObjectByType<Camera>().GetComponent<BetterFogRenderers>();
				if (component != null)
				{
					m_DepthRenderers = component.depthRenderers;
					m_FogOffsetRenderers = component.fogOffsetRenderers;
				}
			}
			if ((bool)m_FogFactorMaterial)
			{
				FogFactorMaterialProperties();
			}
			if ((bool)m_FogBlendMaterial)
			{
				FogBlendMaterialProperties();
			}
			temporaryBlitPass.renderPassEvent = m_Settings.Event;
			customDepthPass.renderPassEvent = m_Settings.Event;
			fogOffsetPass.renderPassEvent = m_Settings.Event;
			fogFactorBlit.renderPassEvent = m_Settings.Event;
			fogBlendBlit.renderPassEvent = m_Settings.Event;
			smssPass.renderPassEvent = m_Settings.Event;
			temporaryBlitPass.Setup(m_UseSMSS);
			if (m_Settings._UseCustomDepthTexture)
			{
				customDepthPass.Setup(m_CustomDepthPassMaterial, m_DepthRenderers);
			}
			if (m_Settings._UseFogOffsetTexture)
			{
				fogOffsetPass.Setup(m_FogOffsetRenderers);
			}
			fogFactorBlit.Setup(m_FogFactorMaterial);
			renderer.EnqueuePass(temporaryBlitPass);
			if (m_Settings._UseCustomDepthTexture)
			{
				renderer.EnqueuePass(customDepthPass);
			}
			if (m_Settings._UseFogOffsetTexture)
			{
				renderer.EnqueuePass(fogOffsetPass);
			}
			renderer.EnqueuePass(fogFactorBlit);
			fogBlendBlit.Setup(m_FogBlendMaterial, m_UseSMSS);
			renderer.EnqueuePass(fogBlendBlit);
			if (m_UseSMSS)
			{
				int width = renderingData.cameraData.cameraTargetDescriptor.width;
				int height = renderingData.cameraData.cameraTargetDescriptor.height;
				width /= 2;
				height /= 2;
				int iterations = -1;
				if ((bool)m_SMSSMaterial)
				{
					iterations = SSMSProperties(width, height);
				}
				smssPass.Setup(m_SMSSMaterial, iterations);
				renderer.EnqueuePass(smssPass);
			}
		}

		private void FogFactorMaterialProperties()
		{
			if (m_Settings._UseCustomDepthTexture)
			{
				m_FogFactorMaterial.EnableKeyword("_USECUSTOMDEPTH_ON");
			}
			else
			{
				m_FogFactorMaterial.DisableKeyword("_USECUSTOMDEPTH_ON");
			}
			if (m_Settings._UseFogOffsetTexture)
			{
				m_FogFactorMaterial.SetInt("_UseFogOffset", 1);
			}
			else
			{
				m_FogFactorMaterial.SetInt("_UseFogOffset", 0);
			}
			if (m_BetterFogVolume._UseDistanceFog.value)
			{
				m_FogFactorMaterial.EnableKeyword("_USEDISTANCEFOG_ON");
			}
			else
			{
				m_FogFactorMaterial.DisableKeyword("_USEDISTANCEFOG_ON");
			}
			if (m_BetterFogVolume._UseSkyboxHeightFog.value)
			{
				m_FogFactorMaterial.EnableKeyword("_USESKYBOXHEIGHTFOG_ON");
			}
			else
			{
				m_FogFactorMaterial.DisableKeyword("_USESKYBOXHEIGHTFOG_ON");
			}
			if (m_BetterFogVolume._UseHeightFog.value)
			{
				m_FogFactorMaterial.EnableKeyword("_USEHEIGHTFOG_ON");
			}
			else
			{
				m_FogFactorMaterial.DisableKeyword("_USEHEIGHTFOG_ON");
			}
			if (m_BetterFogVolume._UseNoise.value)
			{
				m_FogFactorMaterial.EnableKeyword("_USENOISE_ON");
			}
			else
			{
				m_FogFactorMaterial.DisableKeyword("_USENOISE_ON");
			}
			m_FogFactorMaterial.SetFloat("_FogIntensity", m_BetterFogVolume._FogIntensity.value);
			m_FogFactorMaterial.SetInt("_UseRadialDistance", m_BetterFogVolume._UseRadialDistance.value ? 1 : 0);
			switch (m_BetterFogVolume._FogType.value)
			{
			case FogMode.Linear:
				m_FogFactorMaterial.EnableKeyword("_FOGTYPE_LINEAR");
				m_FogFactorMaterial.DisableKeyword("_FOGTYPE_EXP");
				m_FogFactorMaterial.DisableKeyword("_FOGTYPE_EXP2");
				break;
			case FogMode.Exponential:
				m_FogFactorMaterial.DisableKeyword("_FOGTYPE_LINEAR");
				m_FogFactorMaterial.EnableKeyword("_FOGTYPE_EXP");
				m_FogFactorMaterial.DisableKeyword("_FOGTYPE_EXP2");
				break;
			case FogMode.ExponentialSquared:
				m_FogFactorMaterial.DisableKeyword("_FOGTYPE_LINEAR");
				m_FogFactorMaterial.DisableKeyword("_FOGTYPE_EXP");
				m_FogFactorMaterial.EnableKeyword("_FOGTYPE_EXP2");
				break;
			}
			m_FogFactorMaterial.SetFloat("_DistanceFogOffset", m_BetterFogVolume._DistanceFogOffset.value);
			m_FogFactorMaterial.SetFloat("_SkyboxFogIntensity", m_BetterFogVolume._SkyboxFogIntensity.value);
			m_FogFactorMaterial.SetFloat("_SkyboxFogHardness", m_BetterFogVolume._SkyboxFogHardness.value);
			m_FogFactorMaterial.SetFloat("_SkyboxFogOffset", m_BetterFogVolume._SkyboxFogOffset.value);
			m_FogFactorMaterial.SetFloat("_SkyboxFill", m_BetterFogVolume._SkyboxFill.value);
			m_FogFactorMaterial.SetFloat("_HeightDensity", Mathf.Pow(m_BetterFogVolume._HeightDensity.value, 4f));
			m_FogFactorMaterial.SetFloat("_Height", m_BetterFogVolume._Height.value);
			m_FogFactorMaterial.SetInt("_HeightFogTypeExp", (m_BetterFogVolume._HeightFogType.value == HeightFogType.ExponentialSquared) ? 1 : 0);
			m_FogFactorMaterial.SetFloat("_Scale1", m_BetterFogVolume._Scale1.value);
			m_FogFactorMaterial.SetFloat("_NoiseTimeScale1", m_BetterFogVolume._NoiseTimeScale1.value);
			m_FogFactorMaterial.SetFloat("_Lerp1", m_BetterFogVolume._Lerp1.value);
			m_FogFactorMaterial.SetFloat("_NoiseDistanceEnd", m_BetterFogVolume._NoiseDistanceEnd.value);
			m_FogFactorMaterial.SetFloat("_NoiseIntensity", m_BetterFogVolume._NoiseIntensity.value);
			m_FogFactorMaterial.SetFloat("_NoiseEndHardness", m_BetterFogVolume._NoiseEndHardness.value);
			m_FogFactorMaterial.SetVector("_NoiseSpeed1", m_BetterFogVolume._NoiseSpeed1.value);
			int value = 0;
			int value2 = 0;
			switch (m_BetterFogVolume._NoiseAffect.value)
			{
			case NoiseAffect.DistanceOnly:
				value = 1;
				value2 = 0;
				break;
			case NoiseAffect.HeightOnly:
				value = 0;
				value2 = 1;
				break;
			case NoiseAffect.Both:
				value = 1;
				value2 = 1;
				break;
			}
			if (!m_BetterFogVolume._UseDistanceFog.value)
			{
				value = 0;
			}
			if (!m_BetterFogVolume._UseHeightFog.value)
			{
				value2 = 0;
			}
			m_FogFactorMaterial.SetInt("_UseNoiseDistance", value);
			m_FogFactorMaterial.SetInt("_UseNoiseHeight", value2);
			float num = m_BetterFogVolume._SceneEnd.value - m_BetterFogVolume._SceneStart.value;
			float num2 = ((Mathf.Abs(num) > 0.0001f) ? (1f / num) : 0f);
			Vector4 value3 = default(Vector4);
			value3.x = m_BetterFogVolume._FogDensity.value * 1.2011224f;
			value3.y = m_BetterFogVolume._FogDensity.value * 1.442695f;
			value3.z = 0f - num2;
			value3.w = m_BetterFogVolume._SceneEnd.value * num2;
			m_FogFactorMaterial.SetVector("_SceneFogParams", value3);
		}

		private void FogBlendMaterialProperties()
		{
			if (m_Settings._UseCustomDepthTexture)
			{
				m_FogFactorMaterial.EnableKeyword("_USECUSTOMDEPTH_ON");
			}
			else
			{
				m_FogFactorMaterial.DisableKeyword("_USECUSTOMDEPTH_ON");
			}
			if (m_BetterFogVolume._UseSunLight.value)
			{
				m_FogBlendMaterial.EnableKeyword("_USESUNLIGHT_ON");
			}
			else
			{
				m_FogBlendMaterial.DisableKeyword("_USESUNLIGHT_ON");
			}
			if (m_BetterFogVolume._UseGradient.value)
			{
				m_FogBlendMaterial.EnableKeyword("_USEGRADIENT_ON");
			}
			else
			{
				m_FogBlendMaterial.DisableKeyword("_USEGRADIENT_ON");
			}
			m_FogBlendMaterial.SetColor("_SunColor", m_BetterFogVolume._SunColor.value);
			m_FogBlendMaterial.SetFloat("_SunPower", m_BetterFogVolume._SunPower.value);
			m_FogBlendMaterial.SetFloat("_SunIntensity", m_BetterFogVolume._SunIntensity.value);
			m_FogBlendMaterial.SetColor("_FogColor", m_BetterFogVolume._FogColor.value);
			m_FogBlendMaterial.SetFloat("_GradientStart", m_BetterFogVolume._GradientStart.value);
			m_FogBlendMaterial.SetFloat("_GradientEnd", m_BetterFogVolume._GradientEnd.value);
			if (m_BetterFogVolume._GradientTexture.value != null)
			{
				m_FogBlendMaterial.SetFloat("_GradientLerp", m_BetterFogVolume._GradientTexture.LerpValue);
				m_FogBlendMaterial.SetTexture("_GradientTextureFrom", m_BetterFogVolume._GradientTexture.FromTexture);
				m_FogBlendMaterial.SetTexture("_GradientTexture", m_BetterFogVolume._GradientTexture.value);
			}
			m_FogBlendMaterial.SetFloat("_EnergyLoss", m_BetterFogVolume._EnergyLoss.value);
		}

		private int SSMSProperties(float tw, float th)
		{
			float num = Mathf.Log(th, 2f) + m_BetterFogVolume._Radius.value - 8f;
			int num2 = (int)num;
			int result = Mathf.Clamp(num2, 1, kMaxIterations);
			float value = m_BetterFogVolume._Threshold.value;
			m_SMSSMaterial.SetFloat("_Threshold", value);
			float num3 = value * m_BetterFogVolume._SoftKnee.value + 1E-05f;
			Vector3 vector = new Vector3(value - num3, num3 * 2f, 0.25f / num3);
			m_SMSSMaterial.SetVector("_Curve", vector);
			bool flag = !m_BetterFogVolume._HighQuality.value && m_BetterFogVolume._AntiFlicker.value;
			m_SMSSMaterial.SetFloat("_PrefilterOffs", flag ? (-0.5f) : 0f);
			m_SMSSMaterial.SetFloat("_SampleScale", 0.5f + num - (float)num2);
			m_SMSSMaterial.SetFloat("_Intensity", m_BetterFogVolume._Intensity.value);
			Texture value2 = m_BetterFogVolume._FadeRamp.value;
			if (value2 != null)
			{
				m_SMSSMaterial.SetTexture("_FadeTex", value2);
			}
			m_SMSSMaterial.SetFloat("_BlurWeight", m_BetterFogVolume._BlurWeight.value);
			m_SMSSMaterial.SetFloat("_Radius", m_BetterFogVolume._Radius.value);
			if (m_BetterFogVolume._AntiFlicker.value)
			{
				m_SMSSMaterial.EnableKeyword("ANTI_FLICKER_ON");
			}
			else
			{
				m_SMSSMaterial.DisableKeyword("ANTI_FLICKER_ON");
			}
			if (m_BetterFogVolume._HighQuality.value)
			{
				m_SMSSMaterial.EnableKeyword("_HIGH_QUALITY_ON");
				return result;
			}
			m_SMSSMaterial.DisableKeyword("_HIGH_QUALITY_ON");
			return result;
		}

		protected override void Dispose(bool disposing)
		{
			CoreUtils.Destroy(m_FogFactorMaterial);
			CoreUtils.Destroy(m_FogBlendMaterial);
			CoreUtils.Destroy(m_SMSSMaterial);
			CoreUtils.Destroy(m_CustomDepthPassMaterial);
		}
	}
}

using Enviro;

namespace UnityEngine.Rendering.HighDefinition
{
	internal class EnviroHDRPSkyRenderer : SkyRenderer
	{
		private Material skyMat;

		private MaterialPropertyBlock m_PropertyBlock = new MaterialPropertyBlock();

		public override void Build()
		{
			if (skyMat == null)
			{
				skyMat = CoreUtils.CreateEngineMaterial(Shader.Find("Enviro/HDRP/Sky"));
			}
		}

		public override void Cleanup()
		{
			CoreUtils.Destroy(skyMat);
		}

		protected override bool Update(BuiltinSkyParameters builtinParams)
		{
			return false;
		}

		public override void RenderSky(BuiltinSkyParameters builtinParams, bool renderForCubemap, bool renderSunDisk)
		{
			if (!(EnviroManager.instance == null) && !(EnviroManager.instance.Sky == null))
			{
				if (skyMat == null)
				{
					Build();
				}
				EnviroManager.instance.Sky.UpdateSkybox(skyMat);
				if (EnviroManager.instance.Sky != null && EnviroManager.instance.Lighting != null)
				{
					Shader.SetGlobalColor("_AmbientColorTintHDRP", EnviroManager.instance.Lighting.Settings.ambientColorTintHDRP.Evaluate(EnviroManager.instance.solarTime));
				}
				EnviroHDRPSky skySettings = builtinParams.skySettings as EnviroHDRPSky;
				m_PropertyBlock.SetMatrix("_PixelCoordToViewDirWS", builtinParams.pixelCoordToViewDirMatrix);
				Shader.SetGlobalMatrix("_PixelCoordToViewDirWS", builtinParams.pixelCoordToViewDirMatrix);
				Shader.SetGlobalFloat("_EnviroSkyIntensity", SkyRenderer.GetSkyIntensity(skySettings, builtinParams.debugSettings));
				if (EnviroManager.instance.Objects.directionalLight != null)
				{
					EnviroManager.instance.Objects.directionalLight.transform.position = Vector3.zero;
				}
				if (EnviroManager.instance.Objects.additionalDirectionalLight != null)
				{
					EnviroManager.instance.Objects.additionalDirectionalLight.transform.position = Vector3.zero;
				}
				_ = builtinParams.hdCamera.camera.cameraType == CameraType.Reflection && renderForCubemap;
				CoreUtils.DrawFullScreen(builtinParams.commandBuffer, skyMat, m_PropertyBlock, (!renderForCubemap) ? 1 : 0);
			}
		}
	}
}

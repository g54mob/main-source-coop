using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Portningsbolaget
{
	[RequireComponent(typeof(Camera))]
	public class CameraPlatformSettings : MonoBehaviour
	{
		[Serializable]
		public struct CameraSettings
		{
			public AntialiasingMode antialiasing;

			public bool postProcessing;

			public bool shadows;
		}

		public CameraSettings standalone;

		private void Awake()
		{
			Camera component = GetComponent<Camera>();
			CameraSettings settings = GetSettings();
			UniversalAdditionalCameraData universalAdditionalCameraData = component.GetUniversalAdditionalCameraData();
			universalAdditionalCameraData.renderPostProcessing = settings.postProcessing;
			universalAdditionalCameraData.antialiasing = settings.antialiasing;
			universalAdditionalCameraData.renderShadows = settings.shadows;
		}

		private CameraSettings GetSettings()
		{
			return standalone;
		}
	}
}

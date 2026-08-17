using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CW.Common
{
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class CwDemo : MonoBehaviour
	{
		[SerializeField]
		private bool upgradeInputModule = true;

		[SerializeField]
		private bool changeExposureInHDRP = true;

		[SerializeField]
		private bool changeVisualEnvironmentInHDRP = true;

		[SerializeField]
		private bool changeFogInHDRP = true;

		[SerializeField]
		private bool changeCloudsInHDRP = true;

		[SerializeField]
		private bool changeMotionBlurInHDRP = true;

		[SerializeField]
		private bool upgradeLightsInHDRP = true;

		[SerializeField]
		private bool upgradeCamerasInHDRP = true;

		public bool UpgradeInputModule
		{
			get
			{
				return upgradeInputModule;
			}
			set
			{
				upgradeInputModule = value;
			}
		}

		public bool ChangeExposureInHDRP
		{
			get
			{
				return changeExposureInHDRP;
			}
			set
			{
				changeExposureInHDRP = value;
			}
		}

		public bool ChangeVisualEnvironmentInHDRP
		{
			get
			{
				return changeVisualEnvironmentInHDRP;
			}
			set
			{
				changeVisualEnvironmentInHDRP = value;
			}
		}

		public bool ChangeFogInHDRP
		{
			get
			{
				return changeFogInHDRP;
			}
			set
			{
				changeFogInHDRP = value;
			}
		}

		public bool ChangeCloudsInHDRP
		{
			get
			{
				return changeCloudsInHDRP;
			}
			set
			{
				changeCloudsInHDRP = value;
			}
		}

		public bool ChangeMotionBlurInHDRP
		{
			get
			{
				return changeMotionBlurInHDRP;
			}
			set
			{
				changeMotionBlurInHDRP = value;
			}
		}

		public bool UpgradeLightsInHDRP
		{
			get
			{
				return upgradeLightsInHDRP;
			}
			set
			{
				upgradeLightsInHDRP = value;
			}
		}

		public bool UpgradeCamerasInHDRP
		{
			get
			{
				return upgradeCamerasInHDRP;
			}
			set
			{
				upgradeCamerasInHDRP = value;
			}
		}

		protected virtual void OnEnable()
		{
			if (upgradeInputModule)
			{
				TryUpgradeEventSystem();
			}
			if (CwHelper.IsURP)
			{
				TryApplyURP();
			}
			if (CwHelper.IsHDRP)
			{
				TryApplyHDRP();
			}
		}

		protected virtual void TryApplyURP()
		{
		}

		protected virtual void TryApplyHDRP()
		{
			if (changeExposureInHDRP || changeVisualEnvironmentInHDRP || changeFogInHDRP)
			{
				TryCreateVolume();
			}
			if (upgradeLightsInHDRP)
			{
				TryUpgradeLights();
			}
			if (upgradeCamerasInHDRP)
			{
				TryUpgradeCameras();
			}
		}

		private void TryCreateVolume()
		{
			Volume volume = GetComponent<Volume>();
			if (volume == null)
			{
				volume = base.gameObject.AddComponent<Volume>();
			}
			VolumeProfile volumeProfile = volume.profile;
			if (volumeProfile == null)
			{
				volumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
				volumeProfile.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
			}
			if (volumeProfile.components.Count == 0)
			{
				base.name = "Demo (Volume Added)";
				if (changeExposureInHDRP)
				{
					volumeProfile.Add<Exposure>(overrides: true).fixedExposure.value = 14f;
				}
				if (changeVisualEnvironmentInHDRP)
				{
					volumeProfile.Add<VisualEnvironment>(overrides: true).skyType.value = 0;
				}
				if (changeFogInHDRP)
				{
					volumeProfile.Add<Fog>(overrides: true).enabled.value = false;
				}
				if (changeCloudsInHDRP)
				{
					volumeProfile.Add<VolumetricClouds>(overrides: true).enable.value = false;
				}
				if (changeMotionBlurInHDRP)
				{
					volumeProfile.Add<MotionBlur>(overrides: true).intensity.value = 0f;
				}
			}
			volume.profile = volumeProfile;
		}

		private void TryUpgradeLights()
		{
			Light[] array = CwHelper.FindObjectsByType<Light>();
			foreach (Light light in array)
			{
				if (light.GetComponent<HDAdditionalLightData>() == null)
				{
					light.gameObject.AddComponent<HDAdditionalLightData>();
				}
			}
		}

		private void TryUpgradeCameras()
		{
			Camera[] array = CwHelper.FindObjectsByType<Camera>();
			foreach (Camera camera in array)
			{
				if (camera.GetComponent<HDAdditionalCameraData>() == null)
				{
					camera.gameObject.AddComponent<HDAdditionalCameraData>().backgroundColorHDR = Color.black;
				}
			}
		}

		private void TryUpgradeEventSystem()
		{
		}
	}
}

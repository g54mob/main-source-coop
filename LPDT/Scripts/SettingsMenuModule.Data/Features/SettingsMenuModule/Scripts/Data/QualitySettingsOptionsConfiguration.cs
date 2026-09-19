using System.Collections.Generic;
using System.Linq;
using Features.DeviceModule.Scripts.DeviceData;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[CreateAssetMenu(fileName = "QualitySettingsOptionsConfiguration_Default", menuName = "Configurations/SettingsMenu/QualitySettingsOptionsConfiguration")]
	public class QualitySettingsOptionsConfiguration : ScriptableObject
	{
		[SerializeField]
		public List<int> AllFpsLimits;

		[field: SerializeField]
		public List<QualitySetting> QualitySettings { get; private set; } = new List<QualitySetting>();

		[field: SerializeField]
		public PerformanceLevelSettings LowPerformanceSettings { get; private set; }

		[field: SerializeField]
		public PerformanceLevelSettings MediumPerformanceSettings { get; private set; }

		[field: SerializeField]
		public PerformanceLevelSettings HighPerformanceSettings { get; private set; }

		[field: SerializeField]
		public List<PlatformPerformanceProfile> PlatformProfiles { get; private set; } = new List<PlatformPerformanceProfile>();

		public PlatformPerformanceProfile GetProfileForDevice(Features.DeviceModule.Scripts.DeviceData.DeviceType device)
		{
			return PlatformProfiles.FirstOrDefault((PlatformPerformanceProfile profile) => profile.Platform == device);
		}

		public IReadOnlyList<QualitySetting> GetQualitySettings(Features.DeviceModule.Scripts.DeviceData.DeviceType device)
		{
			PlatformPerformanceProfile profileForDevice = GetProfileForDevice(device);
			if (profileForDevice != null)
			{
				List<QualitySetting> qualitySettings = profileForDevice.QualitySettings;
				if (qualitySettings != null && qualitySettings.Count > 0)
				{
					return profileForDevice.QualitySettings;
				}
			}
			return QualitySettings;
		}

		public QualitySetting GetQualitySettingByPerformanceLevel(PerformanceLevel performanceLevel, Features.DeviceModule.Scripts.DeviceData.DeviceType device)
		{
			return GetQualitySettings(device).FirstOrDefault((QualitySetting setting) => setting.PerformanceLevel == performanceLevel);
		}
	}
}

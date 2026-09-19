using System;
using System.Collections.Generic;
using Features.DeviceModule.Scripts.DeviceData;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[Serializable]
	public class PlatformPerformanceProfile
	{
		[field: SerializeField]
		public Features.DeviceModule.Scripts.DeviceData.DeviceType Platform { get; private set; }

		[field: SerializeField]
		public bool UseForcedLevel { get; private set; }

		[field: SerializeField]
		public PerformanceLevel ForcedLevel { get; private set; }

		[field: SerializeField]
		public PerformanceLevelSettings LowPerformanceSettings { get; private set; }

		[field: SerializeField]
		public PerformanceLevelSettings MediumPerformanceSettings { get; private set; }

		[field: SerializeField]
		public PerformanceLevelSettings HighPerformanceSettings { get; private set; }

		[field: SerializeField]
		public List<QualitySetting> QualitySettings { get; private set; } = new List<QualitySetting>();
	}
}

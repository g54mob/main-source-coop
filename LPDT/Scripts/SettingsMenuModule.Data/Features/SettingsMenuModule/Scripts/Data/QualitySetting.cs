using System;
using Global.Modules.LocalizationModule.Scripts.Generated;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[Serializable]
	public class QualitySetting
	{
		[field: SerializeField]
		public int QualityOption { get; private set; }

		[field: SerializeField]
		public LocalizationKey LocalizedKey { get; private set; }

		[field: SerializeField]
		public PerformanceLevel PerformanceLevel { get; private set; }
	}
}

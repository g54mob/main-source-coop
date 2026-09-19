using Features.SettingsMenuModule.Scripts.Data;
using Global.Modules.LocalizationModule.Scripts.Generated;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts
{
	[CreateAssetMenu(fileName = "InitialSettingsConfiguration_Default", menuName = "Configurations/SettingsMenu/InitialSettingsConfiguration")]
	public class InitialSettingsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float AllVolume { get; internal set; }

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float MusicVolume { get; internal set; }

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float EffectsVolume { get; internal set; }

		[field: SerializeField]
		[field: Range(0f, 100f)]
		public float MicrophoneSensitivity { get; internal set; }

		[field: SerializeField]
		public bool MicrophoneEnabled { get; internal set; }

		[field: SerializeField]
		public bool NoiseSuppressionEnabled { get; internal set; } = true;

		[field: SerializeField]
		public bool PushToTalkEnabled { get; internal set; }

		[field: SerializeField]
		public bool YAxisInvertEnabled { get; internal set; }

		[field: SerializeField]
		public bool VSyncEnabled { get; internal set; }

		[field: SerializeField]
		public int QualityLevel { get; internal set; }

		[field: SerializeField]
		public ScreenMode ScreenMode { get; internal set; }

		[field: SerializeField]
		public int FPSLimitLevel { get; internal set; }

		[field: SerializeField]
		public float Brightness { get; internal set; }

		[field: SerializeField]
		[field: Range(0f, 100f)]
		public float MouseSensitivity { get; internal set; }

		[field: SerializeField]
		[field: Range(60f, 90f)]
		public float FieldOfView { get; internal set; }

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float ScreenShakeIntensityNormalized { get; internal set; }

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float HeadBobbingIntensityNormalized { get; internal set; }

		[field: SerializeField]
		public Language Language { get; internal set; }
	}
}

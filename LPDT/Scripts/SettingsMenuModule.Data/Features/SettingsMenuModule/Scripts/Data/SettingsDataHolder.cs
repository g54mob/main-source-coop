using System;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[Serializable]
	public class SettingsDataHolder
	{
		public float AllVolume;

		public float MusicVolume;

		public float EffectsVolume;

		public bool MicrophoneEnabled;

		public bool NoiseSuppressionEnabled = true;

		public bool PushToTalkEnabled;

		public bool YAxisInvertEnabled;

		public bool VSyncEnabled;

		public bool StreamerModeEnabled;

		public string MicrophoneDevice;

		public float MicrophoneSensitivity;

		public int QualityLevel;

		public float MouseSensitivity;

		public float FieldOfView;

		public float Brightness;

		public int ScreenMode;

		public Vector2Int Resolution;

		public int FPSLimit;

		public float ScreenShakeIntensityNormalized;

		public float HeadBobbingIntensityNormalized;

		public int Language;

		public string FixedRegion;
	}
}

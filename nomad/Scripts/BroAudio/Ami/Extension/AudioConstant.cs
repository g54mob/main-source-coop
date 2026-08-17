using UnityEngine;

namespace Ami.Extension
{
	public static class AudioConstant
	{
		public const float MinVolume = 0.0001f;

		public const float FullVolume = 1f;

		public const float MaxVolume = 10f;

		public const float DefaultDecibelVolumeScale = 20f;

		public const float MinDecibelVolume = -80f;

		public const float FullDecibelVolume = 0f;

		public const float MaxDecibelVolume = 20f;

		public const float MaxFrequency = 22000f;

		public const float MinFrequency = 10f;

		public const float DefaultDoppler = 1f;

		public const float AttenuationMinDistance = 1f;

		public const float AttenuationMaxDistance = 500f;

		public const float SpatialBlend_3D = 1f;

		public const float SpatialBlend_2D = 0f;

		public const float DefaultPitch = 1f;

		public const float MinAudioSourcePitch = -3f;

		public const float MaxAudioSourcePitch = 3f;

		public const float MinPlayablePitch = 0.01f;

		public const float MaxMixerPitch = 10f;

		public const int DefaultPriority = 128;

		public const int HighestPriority = 0;

		public const float LowestPriority = 256f;

		public const float DefaultSpread = 0f;

		public const float DefaultReverZoneMix = 1f;

		public const float DefaultPanStereo = 0f;

		public const AudioRolloffMode DefaultRolloffMode = AudioRolloffMode.Logarithmic;

		public const float MinLogValue = -4f;

		public const float MaxLogValue = 1f;

		public const float FullVolumeLogValue = 0f;

		public const float MinFrequencyLogValue = 1f;

		public const float MaxFrequencyLogValue = 4.3424225f;

		public const double MixerWarmUpTime = 0.1;

		public static float DecibelVolumeFullScale => 100f;
	}
}

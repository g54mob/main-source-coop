using UnityEngine;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	[CreateAssetMenu(fileName = "MimicVoiceCaptureConfiguration_Default", menuName = "Configurations/Voice/MimicVoiceCaptureConfiguration")]
	public sealed class MimicVoiceCaptureConfiguration : ScriptableObject
	{
		[field: Header("Capture")]
		[field: SerializeField]
		public float VoiceRmsThreshold { get; private set; } = 0.005f;

		[field: SerializeField]
		public float SilenceTimeoutSeconds { get; private set; } = 0.35f;

		[field: SerializeField]
		public float MinSegmentSeconds { get; private set; } = 0.5f;

		[field: SerializeField]
		public float MaxSegmentSeconds { get; private set; } = 4f;

		[field: SerializeField]
		public int MaxSegmentsPerPlayer { get; private set; } = 8;

		[field: SerializeField]
		public float MinPlaybackIntervalSeconds { get; private set; } = 6f;

		[field: SerializeField]
		public float MaxPlaybackIntervalSeconds { get; private set; } = 14f;

		[field: Header("Mimic Archive")]
		[field: SerializeField]
		public int MaxArchivedSegmentsPerPlayer { get; private set; } = 12;

		[field: SerializeField]
		public float ArchiveSegmentDelaySeconds { get; private set; } = 30f;

		[field: SerializeField]
		public float MinArchivedPlaybackAgeSeconds { get; private set; } = 30f;

		[field: SerializeField]
		public float ArchivedSegmentCooldownSeconds { get; private set; } = 45f;

		[field: Range(0f, 1f)]
		[field: SerializeField]
		public float RecentPlayedSegmentsHistoryPercent { get; private set; } = 0.25f;

		[field: SerializeField]
		public int StorageSamplingRate { get; private set; } = 16000;

		[field: Header("Playback Effects")]
		[field: SerializeField]
		public LayerMask OcclusionLayerMask { get; private set; }

		[field: SerializeField]
		public float MaxOcclusionDistance { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalDistanceMultiplier { get; private set; } = 3f;

		[field: SerializeField]
		public float VerticalFalloffExponent { get; private set; } = 2f;

		[field: SerializeField]
		public string LowPassParameterName { get; private set; } = "VoiceOcclusionLowPass";

		[field: SerializeField]
		public float MinLowPassValue { get; private set; } = 0.35f;

		[field: SerializeField]
		public float MinDistanceForEffects { get; private set; }

		[field: SerializeField]
		public float MaxDistanceForEffects { get; private set; } = 20f;

		[field: SerializeField]
		public string DelayParameterName { get; private set; } = "DelayOcclusion";

		[field: SerializeField]
		public float MaxDelay { get; private set; } = 0.4f;

		[field: SerializeField]
		public float MinDelay { get; private set; } = 0.2f;

		[field: SerializeField]
		public string ReverbParameterName { get; private set; } = "OcclusionReverb";

		[field: SerializeField]
		public float MaxReverb { get; private set; } = 0.9f;

		[field: SerializeField]
		public float MinReverb { get; private set; } = 0.6f;

		[field: SerializeField]
		public string VoiceIntensityParameterName { get; private set; } = "VoiceIntensity";

		[field: Range(0f, 1f)]
		[field: SerializeField]
		public float VoiceIntensityFadeStartNormalizedTime { get; private set; } = 0.7f;

		[field: Header("Optional Distance Attenuation")]
		[field: SerializeField]
		public bool ProcessDistanceAttenuation { get; private set; }

		[field: SerializeField]
		public float MaxAttenuationDistance { get; private set; } = 20f;

		[field: SerializeField]
		public AnimationCurve AttenuationCurve { get; private set; } = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		[field: SerializeField]
		public string DistanceParameterName { get; private set; } = "";
	}
}

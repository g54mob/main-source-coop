using UnityEngine;

namespace Features.VoiceOcclusionModule.Scripts
{
	[CreateAssetMenu(fileName = "VoiceOcclusionConfiguration_Default", menuName = "Configurations/Voice/VoiceOcclusionConfiguration")]
	public class VoiceOcclusionConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public LayerMask OcclusionLayerMask { get; private set; }

		[field: SerializeField]
		public float OcclusionRaySpread { get; private set; } = 0.35f;

		[field: SerializeField]
		public float MaxDistance { get; private set; } = 3f;

		[field: SerializeField]
		public string LowPassParameterName { get; private set; } = "LowPass";

		[field: SerializeField]
		public float MinLowPassValue { get; private set; } = 0.35f;

		[field: SerializeField]
		public float MinDistanceForEffects { get; set; } = 5f;

		[field: SerializeField]
		public float MaxDistanceForEffects { get; set; } = 15f;

		[field: SerializeField]
		public string DelayParameterName { get; set; } = "DelayOcclusion";

		[field: SerializeField]
		public float MaxDelay { get; set; } = 1f;

		[field: SerializeField]
		public float MinDelay { get; set; }

		[field: SerializeField]
		public string ReverbParameterName { get; set; } = "OcclusionReverb";

		[field: SerializeField]
		public float MaxReverb { get; set; } = 1f;

		[field: SerializeField]
		public float MinReverb { get; set; } = 0.76f;

		[field: SerializeField]
		public float MaxAttenuationDistance { get; set; } = 15f;

		[field: SerializeField]
		public AnimationCurve AttenuationCurve { get; set; }

		[field: SerializeField]
		public string DistanceParameterName { get; set; } = "VoiceDistance";

		[field: SerializeField]
		public float StoreVoiceDistanceParameterValue { get; set; } = 0.8f;

		[field: SerializeField]
		public float VerticalDistanceMultiplier { get; set; } = 3f;

		[field: SerializeField]
		public float VerticalFalloffExponent { get; set; } = 2f;
	}
}

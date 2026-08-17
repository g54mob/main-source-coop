using UnityEngine;

namespace Ami.BroAudio.Data
{
	public class SpatialSetting : ScriptableObject
	{
		public float StereoPan;

		public float DopplerLevel = 1f;

		public float MinDistance = 1f;

		public float MaxDistance = 500f;

		public bool HasLowPassFilter;

		public AnimationCurve LowpassLevelCustomCurve;

		public AnimationCurve SpatialBlend;

		public AnimationCurve ReverbZoneMix;

		public AnimationCurve Spread;

		public AnimationCurve CustomRolloff;

		public AudioRolloffMode RolloffMode;
	}
}

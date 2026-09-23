using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0f, 1f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Sampling Intensity Track")]
	public class SamplingIntensityTrack : UIEffectTrack<SamplingIntensityMixer>
	{
		protected override string fieldName => "m_SamplingIntensity";
	}
}

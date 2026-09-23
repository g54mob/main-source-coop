using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0f, 1f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Tone Intensity Track")]
	public class ToneIntensityTrack : UIEffectTrack<ToneIntensityMixer>
	{
		protected override string fieldName => "m_ToneIntensity";
	}
}

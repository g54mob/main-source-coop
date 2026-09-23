using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0f, 1f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Gradation Intensity Track")]
	public class GradationIntensityTrack : UIEffectTrack<GradationIntensityMixer>
	{
		protected override string fieldName => "m_GradationIntensity";
	}
}

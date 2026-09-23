using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0.01f, 10f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Gradation Scale Track")]
	public class GradationScaleTrack : UIEffectTrack<GradationScaleMixer>
	{
		protected override string fieldName => "m_GradationScale";
	}
}

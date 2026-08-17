using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Gradation Offset Track")]
	public class GradationOffsetTrack : UIEffectTrack<GradationOffsetMixer>
	{
		protected override string fieldName => "m_GradationOffset";
	}
}

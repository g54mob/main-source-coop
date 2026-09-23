using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Gradation Rotation Track")]
	public class GradationRotationTrack : UIEffectTrack<GradationRotationMixer>
	{
		protected override string fieldName => "m_GradationRotation";
	}
}

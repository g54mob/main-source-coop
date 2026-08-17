using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0f, 1f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Detail Intensity Track")]
	public class DetailIntensityTrack : UIEffectTrack<DetailIntensityMixer>
	{
		protected override string fieldName => "m_DetailIntensity";
	}
}

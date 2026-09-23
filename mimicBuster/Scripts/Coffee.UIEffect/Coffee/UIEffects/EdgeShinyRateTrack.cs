using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0f, 1f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Edge Shiny Rate Track")]
	public class EdgeShinyRateTrack : UIEffectTrack<EdgeShinyRateMixer>
	{
		protected override string fieldName => "m_EdgeShinyRate";
	}
}

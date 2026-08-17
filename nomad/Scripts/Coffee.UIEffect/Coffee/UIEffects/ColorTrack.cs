using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[ColorClipUsage(false)]
	[TrackClipType(typeof(UIEffectColorClip))]
	[DisplayName("UIEffect Tracks/Color Track")]
	public class ColorTrack : UIEffectTrack<ColorMixer>
	{
		protected override string fieldName => "m_Color";
	}
}

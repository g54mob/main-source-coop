using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[ColorClipUsage(true)]
	[TrackClipType(typeof(UIEffectColorClip))]
	[DisplayName("UIEffect Tracks/Detail Color Track")]
	public class EdgeColorTrack : UIEffectTrack<EdgeColorMixer>
	{
		protected override string fieldName => "m_EdgeColor";
	}
}

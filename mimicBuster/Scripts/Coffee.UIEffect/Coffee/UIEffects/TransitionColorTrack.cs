using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[ColorClipUsage(true)]
	[TrackClipType(typeof(UIEffectColorClip))]
	[DisplayName("UIEffect Tracks/Transition Color Track")]
	public class TransitionColorTrack : UIEffectTrack<TransitionColorMixer>
	{
		protected override string fieldName => "m_TransitionColor";
	}
}

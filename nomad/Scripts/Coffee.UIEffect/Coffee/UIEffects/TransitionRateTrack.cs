using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0f, 1f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Transition Rate Track")]
	public class TransitionRateTrack : UIEffectTrack<TransitionRateMixer>
	{
		protected override string fieldName => "m_TransitionRate";
	}
}

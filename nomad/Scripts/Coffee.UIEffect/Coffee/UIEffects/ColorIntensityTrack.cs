using System.ComponentModel;
using Coffee.UIEffects.Timeline;
using UnityEngine.Timeline;

namespace Coffee.UIEffects
{
	[FloatClipUsage(0f, 1f, 0f)]
	[TrackClipType(typeof(UIEffectFloatClip))]
	[DisplayName("UIEffect Tracks/Color Intensity Track")]
	public class ColorIntensityTrack : UIEffectTrack<ColorIntensityMixer>
	{
		protected override string fieldName => "m_ColorIntensity";
	}
}

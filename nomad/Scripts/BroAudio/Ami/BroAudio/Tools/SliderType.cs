using UnityEngine;

namespace Ami.BroAudio.Tools
{
	public enum SliderType
	{
		Linear = 0,
		Logarithmic = 1,
		BroVolume = 2,
		[InspectorName(null)]
		BroVolumeNoField = 3
	}
}

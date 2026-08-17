using UnityEngine;

namespace Ami.BroAudio
{
	public enum FilterSlope
	{
		[InspectorName("12dB ∕ Oct")]
		TwoPole = 0,
		[InspectorName("24dB ∕ Oct")]
		FourPole = 1
	}
}

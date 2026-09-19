using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.VoiceOcclusionModule.Scripts
{
	public class VoiceDistanceModel : ISessionCleanup
	{
		public Dictionary<int, float> MaxDistanceMultiplier { get; } = new Dictionary<int, float>();

		public Dictionary<int, float> Distances { get; } = new Dictionary<int, float>();

		public void Cleanup()
		{
			MaxDistanceMultiplier.Clear();
			Distances.Clear();
		}
	}
}

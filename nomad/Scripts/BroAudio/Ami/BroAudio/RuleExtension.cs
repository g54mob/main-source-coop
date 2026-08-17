using UnityEngine;

namespace Ami.BroAudio
{
	public static class RuleExtension
	{
		public static PlaybackGroup.IsPlayableDelegate EmptyRuleMethod = (SoundID x, Vector3 y) => true;
	}
}

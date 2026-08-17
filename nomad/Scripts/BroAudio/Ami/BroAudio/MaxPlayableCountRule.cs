using System;

namespace Ami.BroAudio
{
	[Serializable]
	public class MaxPlayableCountRule : Rule<int>
	{
		public MaxPlayableCountRule(int value)
			: base(value)
		{
		}

		public static implicit operator MaxPlayableCountRule(int value)
		{
			return new MaxPlayableCountRule(value);
		}
	}
}

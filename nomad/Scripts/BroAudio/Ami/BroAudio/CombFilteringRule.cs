using System;

namespace Ami.BroAudio
{
	[Serializable]
	public class CombFilteringRule : Rule<float>
	{
		public CombFilteringRule(float value)
			: base(value)
		{
		}

		public static implicit operator CombFilteringRule(float value)
		{
			return new CombFilteringRule(value);
		}
	}
}

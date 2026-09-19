using System;

namespace Features.RuporModule.Scripts
{
	[Serializable]
	public class RuporData
	{
		public bool IsInInteraction;

		public RuporData(bool isInInteraction)
		{
			IsInInteraction = isInInteraction;
		}
	}
}

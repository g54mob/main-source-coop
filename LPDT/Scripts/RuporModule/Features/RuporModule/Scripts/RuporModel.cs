using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.GrabModule.Scripts;

namespace Features.RuporModule.Scripts
{
	public class RuporModel : ISessionCleanup
	{
		public Dictionary<IPointGrabable, RuporData> RuporsData { get; } = new Dictionary<IPointGrabable, RuporData>();

		public void Cleanup()
		{
			RuporsData.Clear();
		}
	}
}

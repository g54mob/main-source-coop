using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.GrabModule.Scripts
{
	public class PlayerGrabSimplePointGrabableModel : ISessionCleanup
	{
		private Dictionary<int, IPointGrabable> _playerGrabables = new Dictionary<int, IPointGrabable>();

		public Dictionary<int, IPointGrabable> PlayerGrabables => _playerGrabables;

		public void Cleanup()
		{
			_playerGrabables.Clear();
		}
	}
}

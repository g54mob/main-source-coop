using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using QuickOutline.Scripts;

namespace Features.PlayerGrabModule.Scripts
{
	public class PLayerGrabOutlineModel : ISessionCleanup
	{
		private Dictionary<int, Outline> _playerOutlines = new Dictionary<int, Outline>();

		public Dictionary<int, Outline> PlayerOutlines => _playerOutlines;

		public event Action<int, Outline> OnOutlineAdded;

		public void AddOutline(int playerId, Outline outline)
		{
			if (_playerOutlines.ContainsKey(playerId))
			{
				_playerOutlines[playerId] = outline;
			}
			else
			{
				_playerOutlines.Add(playerId, outline);
			}
			this.OnOutlineAdded?.Invoke(playerId, outline);
		}

		public void Cleanup()
		{
			_playerOutlines.Clear();
		}
	}
}

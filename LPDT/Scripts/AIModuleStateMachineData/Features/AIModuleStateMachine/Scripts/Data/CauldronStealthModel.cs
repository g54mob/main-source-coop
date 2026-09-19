using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.AIModuleStateMachine.Scripts.Data
{
	public class CauldronStealthModel : ISessionCleanup
	{
		private readonly HashSet<int> _stealthedPlayers = new HashSet<int>();

		public bool IsStealthed(int playerId)
		{
			return _stealthedPlayers.Contains(playerId);
		}

		public void SetStealthed(int playerId, bool isStealthed)
		{
			if (isStealthed)
			{
				_stealthedPlayers.Add(playerId);
			}
			else
			{
				_stealthedPlayers.Remove(playerId);
			}
		}

		public void Cleanup()
		{
			_stealthedPlayers.Clear();
		}
	}
}

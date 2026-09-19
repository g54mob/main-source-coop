using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Fusion;

namespace Features.PlayerSpawner.Scripts
{
	public class SpawnedPlayersModel : ISessionCleanup
	{
		private readonly Dictionary<PlayerRef, PlayerDataHolder> _players = new Dictionary<PlayerRef, PlayerDataHolder>();

		public IReadOnlyDictionary<PlayerRef, PlayerDataHolder> Players => _players;

		public event Action<PlayerDataHolder> OnPlayerRegistered;

		public event Action<PlayerRef> OnPlayerUnregistered;

		public void RegisterPlayer(PlayerRef playerRef, NetworkObject networkObject)
		{
			_players[playerRef] = new PlayerDataHolder(networkObject);
			this.OnPlayerRegistered?.Invoke(_players[playerRef]);
		}

		public void UnregisterPlayer(PlayerRef playerRef)
		{
			if (_players.Remove(playerRef))
			{
				this.OnPlayerUnregistered?.Invoke(playerRef);
			}
		}

		public void Cleanup()
		{
			_players.Clear();
			this.OnPlayerRegistered = null;
			this.OnPlayerUnregistered = null;
		}
	}
}

using System;
using System.Collections.Generic;

namespace Features.StoreModule.Scripts
{
	public interface IStoreReadyRoster
	{
		IReadOnlyList<int> ReadyPlayers { get; }

		event Action OnPlayerIdsChanged;

		event Action<int> OnPlayerReady;

		bool IsPlayerReady(int playerId);
	}
}

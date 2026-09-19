using System;
using System.Collections.Generic;

namespace Features.RagdollModule.Scripts
{
	public class PlayersRagdollModel
	{
		private readonly Dictionary<int, PlayerRagdollEntity> _playersRagdoll = new Dictionary<int, PlayerRagdollEntity>();

		public IReadOnlyDictionary<int, PlayerRagdollEntity> PlayersRagdoll => _playersRagdoll;

		public event Action<int, PlayerRagdollEntity> OnPlayerRagdollAdded;

		public void RegisterPlayerRagdoll(int playerId, PlayerRagdollEntity ragdoll)
		{
			if (!_playersRagdoll.TryAdd(playerId, ragdoll))
			{
				_playersRagdoll[playerId] = ragdoll;
			}
			ragdoll.PlayerId = playerId;
			this.OnPlayerRagdollAdded?.Invoke(playerId, ragdoll);
		}

		public void UnregisterPlayerRagdoll(int playerId)
		{
			_playersRagdoll.Remove(playerId);
		}

		public bool TryGetPlayerRagdoll(int playerId, out PlayerRagdollEntity ragdoll)
		{
			return _playersRagdoll.TryGetValue(playerId, out ragdoll);
		}
	}
}

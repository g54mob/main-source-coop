using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.AIModuleStateMachine.Scripts.Data
{
	public class PlayerEnemyInteractionBlocksModel : ISessionCleanup
	{
		private readonly Dictionary<int, PlayerEnemyInteractionBlockReason> _blocks = new Dictionary<int, PlayerEnemyInteractionBlockReason>();

		public event Action OnBlocksChanged;

		public bool IsBlocked(int playerId)
		{
			return _blocks.ContainsKey(playerId);
		}

		public bool IsBlocked(int playerId, out PlayerEnemyInteractionBlockReason reason)
		{
			return _blocks.TryGetValue(playerId, out reason);
		}

		public void SetBlock(int playerId, PlayerEnemyInteractionBlockReason reason)
		{
			if (!_blocks.TryGetValue(playerId, out var value) || value != reason)
			{
				_blocks[playerId] = reason;
				this.OnBlocksChanged?.Invoke();
			}
		}

		public void ClearBlock(int playerId, PlayerEnemyInteractionBlockReason reason)
		{
			if (_blocks.TryGetValue(playerId, out var value) && value == reason)
			{
				_blocks.Remove(playerId);
				this.OnBlocksChanged?.Invoke();
			}
		}

		public void Cleanup()
		{
			_blocks.Clear();
			this.OnBlocksChanged?.Invoke();
		}
	}
}

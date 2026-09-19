using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	public class PlayerDeadPartModel
	{
		private readonly List<PlayerDeadPart> _deadPartsInRange = new List<PlayerDeadPart>();

		private readonly List<PlayerDeadPart> _spawnedDeadParts = new List<PlayerDeadPart>();

		private PlayerAlivePart _playerAlivePart;

		private readonly Dictionary<PlayerRef, PlayerAlivePart> _allPlayersAliveParts = new Dictionary<PlayerRef, PlayerAlivePart>();

		public IReadOnlyDictionary<PlayerRef, PlayerAlivePart> AllPlayersAliveParts => _allPlayersAliveParts;

		public PlayerAlivePart PlayerAlivePart => _playerAlivePart;

		public IReadOnlyList<PlayerDeadPart> DeadPartsInRange => _deadPartsInRange;

		public IReadOnlyList<PlayerDeadPart> SpawnedDeadParts => _spawnedDeadParts;

		public event Action OnPlayerDeadPartsCollision;

		public void RegisterDeadPart(PlayerDeadPart deadPart)
		{
			if (!_deadPartsInRange.Contains(deadPart))
			{
				_deadPartsInRange.Add(deadPart);
				this.OnPlayerDeadPartsCollision?.Invoke();
			}
		}

		public void RemoveDeadPart(PlayerDeadPart deadPart)
		{
			if (_deadPartsInRange.Contains(deadPart))
			{
				_deadPartsInRange.Remove(deadPart);
				this.OnPlayerDeadPartsCollision?.Invoke();
			}
		}

		public void RegisterSpawnedDeadPart(PlayerDeadPart deadPart)
		{
			if (!(deadPart == null) && !_spawnedDeadParts.Contains(deadPart))
			{
				_spawnedDeadParts.Add(deadPart);
			}
		}

		public void UnregisterSpawnedDeadPart(PlayerDeadPart deadPart)
		{
			if (!(deadPart == null))
			{
				_spawnedDeadParts.Remove(deadPart);
			}
		}

		public static bool IsFreeDeadPart(PlayerDeadPart deadPart)
		{
			if (deadPart != null && deadPart.gameObject.activeSelf)
			{
				return !deadPart.IsUsed;
			}
			return false;
		}

		public bool HasAnyFreeSpawnedDeadPart()
		{
			foreach (PlayerDeadPart spawnedDeadPart in _spawnedDeadParts)
			{
				if (IsFreeDeadPart(spawnedDeadPart))
				{
					return true;
				}
			}
			return false;
		}

		public bool HasFreeDeadPartNearAlive(PlayerAlivePart alivePart, float range)
		{
			if (alivePart == null || alivePart.DownPos == null)
			{
				return false;
			}
			Vector3 position = alivePart.DownPos.position;
			foreach (PlayerDeadPart spawnedDeadPart in _spawnedDeadParts)
			{
				if (IsFreeDeadPart(spawnedDeadPart) && !(spawnedDeadPart.UpperPos == null) && Vector3.Distance(spawnedDeadPart.UpperPos.position, position) <= range)
				{
					return true;
				}
			}
			return false;
		}

		public void RegisterLocalAlivePart(PlayerAlivePart playerAlivePart)
		{
			_playerAlivePart = playerAlivePart;
		}

		public void RegisterAlivePart(PlayerRef player, PlayerAlivePart playerAlivePart)
		{
			_allPlayersAliveParts[player] = playerAlivePart;
		}

		public void UnRegisterAlivePart(PlayerRef player)
		{
			_allPlayersAliveParts.Remove(player);
		}
	}
}

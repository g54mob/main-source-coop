using System;
using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.GameUpdaterModule;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.RandomSoundPlayModule.Scripts
{
	public class EnemiesNearPlayersUpdateSystem : IInitializable, IDisposable
	{
		private readonly IGameUpdater _gameUpdater;

		private readonly EnemyTransformsModel _enemyTransformsModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly RandomSoundPlayConfiguration _configuration;

		private readonly List<int> _playersToClear = new List<int>();

		private float _nextCheckTime;

		public EnemiesNearPlayersUpdateSystem(IGameUpdater gameUpdater, EnemyTransformsModel enemyTransformsModel, PlayerMovableModel playerMovableModel, RandomSoundPlayConfiguration configuration)
		{
			_gameUpdater = gameUpdater;
			_enemyTransformsModel = enemyTransformsModel;
			_playerMovableModel = playerMovableModel;
			_configuration = configuration;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += UpdateEnemiesNearPlayers;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= UpdateEnemiesNearPlayers;
			_enemyTransformsModel.ClearEnemiesNearPlayers();
		}

		private void UpdateEnemiesNearPlayers()
		{
			if (!(Time.time < _nextCheckTime))
			{
				_nextCheckTime = Time.time + _configuration.EnemyProximityCheckInterval;
				RefreshActivePlayers();
				ClearMissingPlayers();
			}
		}

		private void RefreshActivePlayers()
		{
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				PlayerCharacterMovableBase value = allCharacterMovable.Value;
				if (!(value == null))
				{
					_enemyTransformsModel.RefreshEnemiesNearPlayer(allCharacterMovable.Key.PlayerId, value.transform.position, _configuration.EnemyProximityDistance);
				}
			}
		}

		private void ClearMissingPlayers()
		{
			_playersToClear.Clear();
			foreach (int key in _enemyTransformsModel.EnemiesNearPlayers.Keys)
			{
				if (!ContainsPlayerId(key))
				{
					_playersToClear.Add(key);
				}
			}
			foreach (int item in _playersToClear)
			{
				_enemyTransformsModel.ClearEnemiesNearPlayer(item);
			}
		}

		private bool ContainsPlayerId(int playerId)
		{
			foreach (PlayerRef key in _playerMovableModel.AllCharacterMovables.Keys)
			{
				if (key.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
		}
	}
}

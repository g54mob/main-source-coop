using System;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModule.Scripts.Services
{
	public class EnemyPlayerReboundListener : IPlayerReboundListener, IInitializable, IDisposable
	{
		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly PlayerReboundModel _playerReboundModel;

		public EnemyPlayerReboundListener(SpawnedPlayersModel spawnedPlayersModel, PlayerReboundModel playerReboundModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerReboundModel = playerReboundModel;
		}

		public void Initialize()
		{
			_playerReboundModel.OnPlayerRebound += HandlePlayerRebound;
		}

		public void Dispose()
		{
			_playerReboundModel.OnPlayerRebound -= HandlePlayerRebound;
		}

		public void OnPlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			HandlePlayerRebound(playerRef, avatar);
		}

		private void HandlePlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			if (avatar == null || !avatar.IsValid)
			{
				_spawnedPlayersModel.UnregisterPlayer(playerRef);
			}
			else
			{
				_spawnedPlayersModel.RegisterPlayer(playerRef, avatar);
			}
		}
	}
}

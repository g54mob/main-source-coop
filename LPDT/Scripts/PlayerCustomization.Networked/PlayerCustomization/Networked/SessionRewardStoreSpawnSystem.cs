using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace PlayerCustomization.Networked
{
	public class SessionRewardStoreSpawnSystem : IInitializable, ITickable
	{
		private const string PREFAB_RESOURCE = "SessionRewardStore";

		private readonly MultiplayerModel _multiplayerModel;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly SessionRewardModel _sessionRewardModel;

		private NetworkObject _prefab;

		private bool _spawnRequested;

		public SessionRewardStoreSpawnSystem(MultiplayerModel multiplayerModel, SpawnedPlayersModel spawnedPlayersModel, SessionRewardModel sessionRewardModel)
		{
			_multiplayerModel = multiplayerModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_sessionRewardModel = sessionRewardModel;
		}

		public void Initialize()
		{
			_prefab = Resources.Load<NetworkObject>("SessionRewardStore");
		}

		public void Tick()
		{
			if (_prefab == null)
			{
				return;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient || !_spawnedPlayersModel.Players.ContainsKey(networkRunner.LocalPlayer))
			{
				return;
			}
			SessionRewardStore sessionRewardStore = _sessionRewardModel.Store;
			if (sessionRewardStore != null && (sessionRewardStore.Object == null || !sessionRewardStore.Object.IsValid))
			{
				sessionRewardStore = null;
			}
			if (sessionRewardStore == null)
			{
				if (!_spawnRequested)
				{
					networkRunner.Spawn(_prefab);
					_spawnRequested = true;
				}
			}
			else
			{
				_spawnRequested = false;
				if (!sessionRewardStore.Object.HasStateAuthority)
				{
					sessionRewardStore.Object.RequestStateAuthority();
				}
			}
		}
	}
}

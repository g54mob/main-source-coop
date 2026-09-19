using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.PlayerSpawner.Scripts
{
	public class SpawnedPlayerAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _networkObject;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, MultiplayerModel multiplayerModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_multiplayerModel = multiplayerModel;
		}

		private void Start()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || _networkObject == null || !_networkObject.IsValid)
			{
				return;
			}
			PlayerRef playerRef = _networkObject.InputAuthority;
			if (playerRef == PlayerRef.None)
			{
				playerRef = _networkObject.StateAuthority;
			}
			if (!(playerRef == PlayerRef.None))
			{
				bool num = playerRef == networkRunner.LocalPlayer;
				bool isSharedModeMasterClient = networkRunner.IsSharedModeMasterClient;
				if (num || isSharedModeMasterClient)
				{
					_spawnedPlayersModel.RegisterPlayer(playerRef, _networkObject);
				}
			}
		}

		private void OnDestroy()
		{
			if (_networkObject == null || !_networkObject.IsValid)
			{
				return;
			}
			PlayerRef playerRef = _networkObject.InputAuthority;
			if (playerRef == PlayerRef.None)
			{
				playerRef = _networkObject.StateAuthority;
			}
			if (!(playerRef == PlayerRef.None))
			{
				NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
				if (!(networkRunner != null) || !networkRunner.IsRunning || !(playerRef != networkRunner.LocalPlayer) || networkRunner.IsSharedModeMasterClient)
				{
					_spawnedPlayersModel.UnregisterPlayer(playerRef);
				}
			}
		}
	}
}

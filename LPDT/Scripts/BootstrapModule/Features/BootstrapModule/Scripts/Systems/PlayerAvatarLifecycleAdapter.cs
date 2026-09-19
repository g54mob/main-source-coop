using Cysharp.Threading.Tasks;
using Features.CameraModelModule;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PositionMarkersModule;
using Features.SessionManagementModule.Models;
using Fusion;
using RSG.Muffin.AssetLoaderModule.Core;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class PlayerAvatarLifecycleAdapter : IPlayerAvatarLifecycle
	{
		private const int MAX_SPAWN_MARKER_WAIT_FRAMES = 300;

		private const string AVATAR_OBJECT_NAME = "CharacterBase";

		private const string CAMERA_OBJECT_NAME = "PlayerCamera";

		private const string PLAYER_CAMERA_RIG_ADDRESS = "Assets/Global/GameObjects/Characters/Camera.prefab";

		private const string FREE_FLY_PREFAB_ADDRESS = "Assets/Global/GameObjects/Characters/FreeFlyCharacter.prefab";

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IAssetLoaderFacadeService _assetLoaderFacadeService;

		private readonly PositionMarkersModel _positionMarkersModel;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly IPlayerReboundBroadcastService _playerReboundBroadcastService;

		private readonly PlayerReboundModel _playerReboundModel;

		private readonly CameraModel _cameraModel;

		private readonly DiContainer _diContainer;

		private NetworkObject _heldAvatar;

		private GameObject _heldFreeFly;

		public bool IsAvatarHeld
		{
			get
			{
				if (_heldAvatar != null)
				{
					return _heldAvatar.IsValid;
				}
				return false;
			}
		}

		public PlayerAvatarLifecycleAdapter(MultiplayerModel multiplayerModel, IAssetLoaderFacadeService assetLoaderFacadeService, PositionMarkersModel positionMarkersModel, SpawnedPlayersModel spawnedPlayersModel, IPlayerReboundBroadcastService playerReboundBroadcastService, PlayerReboundModel playerReboundModel, CameraModel cameraModel, DiContainer diContainer)
		{
			_multiplayerModel = multiplayerModel;
			_assetLoaderFacadeService = assetLoaderFacadeService;
			_positionMarkersModel = positionMarkersModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerReboundBroadcastService = playerReboundBroadcastService;
			_playerReboundModel = playerReboundModel;
			_cameraModel = cameraModel;
			_diContainer = diContainer;
		}

		public async UniTask EnsureSpawnedAsync(float initialHealth = -1f)
		{
			await EnsureCameraObjectAsync();
			if (_heldAvatar != null && _heldAvatar.IsValid)
			{
				return;
			}
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			PlayerRef localPlayer = runner.LocalPlayer;
			GameObject prefab = await _assetLoaderFacadeService.LoadAssetAsync<GameObject>("Assets/Global/GameObjects/Characters/CharacterBase.prefab", AssetLoadSource.Addressables);
			PositionMarker positionMarker = await ResolveDefaultSpawnMarkerAsync();
			Vector3 spawnPosition = ((positionMarker != null) ? positionMarker.transform.position : Vector3.zero);
			Quaternion spawnRotation = ((positionMarker != null) ? positionMarker.transform.rotation : Quaternion.identity);
			await EnsureFreeFlySpawnedAsync(spawnPosition);
			NetworkObject networkObject = await runner.SpawnAsync(prefab, spawnPosition, spawnRotation, localPlayer, delegate(NetworkRunner networkRunner, NetworkObject obj)
			{
				if (obj.TryGetComponent<PlayerInitializer>(out var component2))
				{
					component2.InitialHealth = initialHealth;
				}
			});
			if (!(networkObject == null))
			{
				_heldAvatar = networkObject;
				networkObject.gameObject.name = "CharacterBase";
				_spawnedPlayersModel.RegisterPlayer(localPlayer, networkObject);
				if (networkObject.TryGetComponent<PlayerCharacterMovableBase>(out var component))
				{
					component.RebindToAvatar(networkObject);
				}
				_playerReboundBroadcastService.Broadcast(localPlayer, networkObject);
				RegisterLocalPlayerCameraRig(networkObject);
			}
		}

		private async UniTask EnsureFreeFlySpawnedAsync(Vector3 position)
		{
			if (!(_heldFreeFly != null))
			{
				GameObject prefab = await _assetLoaderFacadeService.LoadAssetAsync<GameObject>("Assets/Global/GameObjects/Characters/FreeFlyCharacter.prefab", AssetLoadSource.Addressables);
				_heldFreeFly = _diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, null);
			}
		}

		public void DespawnIfHeld()
		{
			if (_heldFreeFly != null)
			{
				Object.Destroy(_heldFreeFly);
				_heldFreeFly = null;
			}
			if (!(_heldAvatar == null))
			{
				NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
				if (_heldAvatar.IsValid && networkRunner != null && networkRunner.IsRunning)
				{
					networkRunner.Despawn(_heldAvatar);
				}
				_heldAvatar = null;
				if (networkRunner != null)
				{
					_playerReboundModel.NotifyAvatarDespawned(networkRunner.LocalPlayer);
				}
			}
		}

		private async UniTask EnsureCameraObjectAsync()
		{
			if (!(_cameraModel.CameraObject != null) && !(await TryBuildLocalCameraRigAsync()))
			{
				Camera camera = new GameObject("PlayerCamera").AddComponent<Camera>();
				camera.enabled = false;
				_cameraModel.CameraObject = camera;
			}
		}

		private async UniTask<bool> TryBuildLocalCameraRigAsync()
		{
			GameObject gameObject = await _assetLoaderFacadeService.LoadAssetAsync<GameObject>("Assets/Global/GameObjects/Characters/Camera.prefab", AssetLoadSource.Addressables);
			if (gameObject == null)
			{
				return false;
			}
			GameObject cameraRoot = _diContainer.InstantiatePrefab(gameObject);
			CameraControllerBase fpCamera = await InstantiateCameraAsync("Assets/Global/GameObjects/Characters/FPCamera.prefab");
			CameraControllerBase tpCamera = await InstantiateCameraAsync("Assets/Global/GameObjects/Characters/TPCamera.prefab");
			CameraControllerBase fpFollowCamera = await InstantiateCameraAsync("Assets/Global/GameObjects/Characters/FPFollowCamera.prefab");
			CameraControllerBase controller = await InstantiateCameraAsync("Assets/Global/GameObjects/Characters/TPStoreTableCamera.prefab");
			_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.FPCamera, fpCamera);
			_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.TPCamera, tpCamera);
			_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.FPFollowCamera, fpFollowCamera);
			_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.CardTableCamera, controller);
			_cameraModel.CameraObject = cameraRoot.GetComponent<Camera>();
			_cameraModel.UpdateSensitivity();
			return true;
		}

		private async UniTask<CameraControllerBase> InstantiateCameraAsync(string cameraAddress)
		{
			GameObject gameObject = await _assetLoaderFacadeService.LoadAssetAsync<GameObject>(cameraAddress, AssetLoadSource.Addressables);
			GameObject gameObject2 = _diContainer.InstantiatePrefab(gameObject);
			gameObject2.name = gameObject.name;
			return gameObject2.GetComponent<CameraControllerBase>();
		}

		private async UniTask<PositionMarker> ResolveDefaultSpawnMarkerAsync()
		{
			for (int i = 0; i < 300; i++)
			{
				PositionMarker randomPositionMarker = _positionMarkersModel.GetRandomPositionMarker(PositionName.PlayerSpawnPoint);
				if (randomPositionMarker != null)
				{
					return randomPositionMarker;
				}
				await UniTask.Yield();
			}
			return _positionMarkersModel.GetRandomPositionMarker(PositionName.PlayerSpawnPoint);
		}

		private void RegisterLocalPlayerCameraRig(NetworkObject avatar)
		{
			if (_cameraModel.Cameras.ContainsKey(Features.CameraModelModule.CameraType.FPCamera) && avatar.TryGetComponent<PlayerCharacterMovableBase>(out var component))
			{
				Transform cameraPositionTransform = component.CameraPositionTransform;
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
			}
		}
	}
}

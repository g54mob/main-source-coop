using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;

namespace Features.LevelLightModule.Scripts
{
	public class FlashlightSpawnService : IFlashlightSpawnService
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly FlashlightConfiguration _flashlightConfiguration;

		private NetworkObject _spawnedFlashlight;

		public FlashlightSpawnService(MultiplayerModel multiplayerModel, FlashlightConfiguration flashlightConfiguration)
		{
			_multiplayerModel = multiplayerModel;
			_flashlightConfiguration = flashlightConfiguration;
		}

		public async UniTask<NetworkObject> SpawnFlashlight(FlashlightType flashlightType, PlayerRef inputAuthority)
		{
			if (!_flashlightConfiguration.FlashlightPrefabs.TryGetValue(flashlightType, out var value))
			{
				return null;
			}
			DespawnFlashlight();
			_spawnedFlashlight = await _multiplayerModel.NetworkRunner.SpawnAsync(value, null, null, inputAuthority);
			return _spawnedFlashlight;
		}

		public void DespawnFlashlight()
		{
			if (!(_spawnedFlashlight == null))
			{
				_multiplayerModel.NetworkRunner.Despawn(_spawnedFlashlight);
				_spawnedFlashlight = null;
			}
		}
	}
}

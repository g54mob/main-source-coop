using Cysharp.Threading.Tasks;
using Fusion;

namespace Features.LevelLightModule.Scripts
{
	public interface IFlashlightSpawnService
	{
		UniTask<NetworkObject> SpawnFlashlight(FlashlightType flashlightType, PlayerRef inputAuthority);

		void DespawnFlashlight();
	}
}

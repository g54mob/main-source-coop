using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class FlashlightSpawner : NetworkBehaviour
	{
		[SerializeField]
		private FlashlightType _flashlightType = FlashlightType.New;

		private IFlashlightSpawnService _flashlightSpawnService;

		[Inject]
		public void InjectDependencies(IFlashlightSpawnService flashlightSpawnService)
		{
			_flashlightSpawnService = flashlightSpawnService;
		}

		public override void Spawned()
		{
			SpawnFlashlight().Forget();
		}

		private async UniTask SpawnFlashlight()
		{
			if (base.HasStateAuthority)
			{
				await _flashlightSpawnService.SpawnFlashlight(_flashlightType, base.Object.InputAuthority);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}

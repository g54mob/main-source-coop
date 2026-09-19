using Cysharp.Threading.Tasks;
using Features.DeadPartsModule.Scripts;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.ItemSpawnerModule
{
	public sealed class PlayerDeadPartItemSpawner : ItemSpawnerBase
	{
		[Header("Player dead part")]
		[SerializeField]
		private PlayerDeadPart _playerDeadPartPrefab;

		[SerializeField]
		private int _usageCount;

		private IPlayerDeadPartSpawnService _playerDeadPartSpawnService;

		[Inject]
		private void InjectSpawnService(IPlayerDeadPartSpawnService playerDeadPartSpawnService)
		{
			_playerDeadPartSpawnService = playerDeadPartSpawnService;
		}

		protected override bool CanSpawn()
		{
			return _playerDeadPartPrefab != null;
		}

		protected override GameObject GetPreviewSource()
		{
			if (!(_playerDeadPartPrefab != null))
			{
				return null;
			}
			return _playerDeadPartPrefab.gameObject;
		}

		protected override async UniTask SpawnItemAsync()
		{
			if (!(_playerDeadPartPrefab == null) && _playerDeadPartSpawnService != null)
			{
				GetSpawnPose(out var position, out var _);
				PlayerDeadPart playerDeadPart = await _playerDeadPartSpawnService.SpawnPlayerDeadPart(_playerDeadPartPrefab.DeadPartType, position, _usageCount, new PlayerCustomizationSlotData
				{
					VariableColor = Random.ColorHSV()
				});
				if (!(playerDeadPart == null))
				{
					base.SpawnedInstance = playerDeadPart.Object;
					ApplyParentToSpawnerIfNeeded();
				}
			}
		}
	}
}

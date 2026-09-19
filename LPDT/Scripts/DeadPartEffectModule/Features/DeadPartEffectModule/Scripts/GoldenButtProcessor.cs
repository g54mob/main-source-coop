using Cysharp.Threading.Tasks;
using Features.DeadPartsModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.DeadPartEffectModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class GoldenButtProcessor : NetworkBehaviour
	{
		[SerializeField]
		private PlayerAlivePart _playerAlivePart;

		[SerializeField]
		private DeadPartJoinBoosterBehaviour _deadPartJoinBoosterBehaviour;

		private IPlayerStatsUpgradeService _playerStatsUpgradeService;

		private GoldenButtConfiguration _goldenButtConfiguration;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private IItemSpawnService _itemSpawnService;

		private MultiplayerModel _multiplayerModel;

		private IStat _goldenButtStrengthStat;

		[Inject]
		private void InjectDependencies(IPlayerStatsUpgradeService playerStatsUpgradeService, GoldenButtConfiguration goldenButtConfiguration, IItemSpawnService itemSpawnService, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel)
		{
			_playerStatsUpgradeService = playerStatsUpgradeService;
			_goldenButtConfiguration = goldenButtConfiguration;
			_itemSpawnService = itemSpawnService;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
		}

		public override void Spawned()
		{
			_deadPartJoinBoosterBehaviour.OnBeforeDisableDeadPartEffect += ProcessPlayerDeath;
			if (_playerStatsUpgradeService.IsPlayerStatsReady())
			{
				InitializeStats();
			}
			else
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_deadPartJoinBoosterBehaviour.OnBeforeDisableDeadPartEffect -= ProcessPlayerDeath;
		}

		private void OnPlayerStatRegistered(int player)
		{
			if (player == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				InitializeStats();
				_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			}
		}

		private void InitializeStats()
		{
			_goldenButtStrengthStat = _playerStatsUpgradeService.GetStat(EntityStatType.GoldenButtStrength);
		}

		private void ProcessPlayerDeath()
		{
			if (_goldenButtStrengthStat == null || !(_goldenButtStrengthStat.FullValue > 0f) || !_goldenButtConfiguration.GoldenButtObjects.TryGetValue(_playerAlivePart.DeadPartUsageCount, out var value))
			{
				return;
			}
			foreach (LevelObjectData item in value)
			{
				_itemSpawnService.SpawnItemsMultiple(item.Item, _playerAlivePart.transform.position, item.ItemCount, 0f, item.UseSpread, null, item.UseSpread, 10f).Forget();
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

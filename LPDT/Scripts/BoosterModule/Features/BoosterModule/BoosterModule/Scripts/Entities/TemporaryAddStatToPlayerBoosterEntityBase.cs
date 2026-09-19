using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using Zenject;

namespace Features.BoosterModule.BoosterModule.Scripts.Entities
{
	public class TemporaryAddStatToPlayerBoosterEntityBase : BoosterEntityBase<TemporaryAddStatToPlayerBoosterSettings>
	{
		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		private void InjectDependencies(SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
		}

		public override void Activate()
		{
			if (!_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				return;
			}
			foreach (KeyValuePair<EntityStatType, StatModifier> statModifier in base.BoosterSettings.GetStatModifiers())
			{
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].AddModifier(statModifier.Key, statModifier.Value);
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].SynchronizeStatModifiers(statModifier.Key, RPCType.InAllWays);
			}
		}

		public override void Deactivate()
		{
			if (!_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				return;
			}
			foreach (KeyValuePair<EntityStatType, StatModifier> statModifier in base.BoosterSettings.GetStatModifiers())
			{
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].RemoveModifier(statModifier.Key, statModifier.Value);
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].SynchronizeStatModifiers(statModifier.Key, RPCType.InAllWays);
			}
		}
	}
}

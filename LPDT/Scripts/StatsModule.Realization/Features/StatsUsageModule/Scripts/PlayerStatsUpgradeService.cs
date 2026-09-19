using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;

namespace Features.StatsUsageModule.Scripts
{
	public class PlayerStatsUpgradeService : IPlayerStatsUpgradeService
	{
		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		public PlayerStatsUpgradeService(SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
		}

		public void ApplyModifierStats(ModifierStatsData modifierStatsData)
		{
			StatModifier statModifier = new StatModifier(modifierStatsData.Value, modifierStatsData.ModifierType, 0);
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].AddModifier(modifierStatsData.EntityStatType, statModifier);
			}
		}

		public void DiscardModifierStats(ModifierStatsData modifierStatsData)
		{
			StatModifier statModifier = new StatModifier(modifierStatsData.Value, modifierStatsData.ModifierType, 0);
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].RemoveModifier(modifierStatsData.EntityStatType, statModifier);
			}
		}

		public void ApplyModifierStatsSynchronized(ModifierStatsData modifierStatsData)
		{
			StatModifier statModifier = new StatModifier(modifierStatsData.Value, modifierStatsData.ModifierType, 0);
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].AddModifierSynchronized(modifierStatsData.EntityStatType, statModifier, RPCType.InAllWays);
			}
		}

		public void DiscardModifierStatsSynchronized(ModifierStatsData modifierStatsData)
		{
			StatModifier statModifier = new StatModifier(modifierStatsData.Value, modifierStatsData.ModifierType, 0);
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				_spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].RemoveModifierSynchronized(modifierStatsData.EntityStatType, statModifier, RPCType.InAllWays);
			}
		}

		public IStat GetStat(EntityStatType statType)
		{
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				return _spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].GetStat(statType);
			}
			return null;
		}

		public IStat GetStat(EntityStatType statType, int targetPlayer)
		{
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(targetPlayer))
			{
				return _spawnedEntityStatsModel.PlayerStats[targetPlayer].GetStat(statType);
			}
			return null;
		}

		public bool IsPlayerStatsReady()
		{
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				return true;
			}
			return false;
		}
	}
}

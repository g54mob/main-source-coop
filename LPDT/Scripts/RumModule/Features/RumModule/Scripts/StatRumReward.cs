using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class StatRumReward : RumRewardBase
	{
		private RumConfiguration _rumConfiguration;

		private IPlayerStatsUpgradeService _playerStatsUpgradeService;

		private RumStatsRewardModel _rumTemporalStatsRewardModel;

		private DrunkennessConfiguration _drunkennessConfiguration;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		private void InjectDependencies(RumConfiguration rumConfiguration, IPlayerStatsUpgradeService playerStatsUpgradeService, RumStatsRewardModel rumTemporalStatsRewardModel, DrunkennessConfiguration drunkennessConfiguration, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel)
		{
			_rumConfiguration = rumConfiguration;
			_playerStatsUpgradeService = playerStatsUpgradeService;
			_rumTemporalStatsRewardModel = rumTemporalStatsRewardModel;
			_drunkennessConfiguration = drunkennessConfiguration;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
		}

		public override void ApplyReward(RumType rumType)
		{
			RumData rumData = _rumConfiguration.RumsData.Find((RumData rumData2) => rumData2.RumType == rumType);
			if (rumData == null)
			{
				Debug.LogWarning($"[Drunk] ApplyReward: no RumData for {rumType}");
				ApplyDrunkenness(rumType);
				return;
			}
			if (rumData.RumModifiers != null && rumData.RumModifiers.Count > 0)
			{
				List<ModifierStatsData> list = new List<ModifierStatsData>();
				foreach (ModifierStatsData rumModifier in rumData.RumModifiers)
				{
					list.Add(rumModifier);
					_playerStatsUpgradeService.ApplyModifierStatsSynchronized(rumModifier);
					if (rumData.WithDuration)
					{
						_rumTemporalStatsRewardModel.AddTemporalRumsData(new TemporalRumData(rumData, list));
					}
					else
					{
						_rumTemporalStatsRewardModel.AddRumsData(rumData);
					}
				}
			}
			ApplyDrunkenness(rumType);
		}

		private void ApplyDrunkenness(RumType rumType)
		{
			if (_drunkennessConfiguration == null)
			{
				Debug.LogWarning("[Drunk] ApplyDrunkenness: DrunkennessConfiguration is null (addressables/bind?)");
				return;
			}
			bool flag = _drunkennessConfiguration.AddsDrunkennessOnDrink(rumType);
			if (_drunkennessConfiguration.DebugLog)
			{
				Debug.Log($"[Drunk] ApplyDrunkenness rumType={rumType} adds={flag} " + $"drinkAdd={_drunkennessConfiguration.DrinkAddAmount} " + $"max={_drunkennessConfiguration.MaxDrunkenness} " + $"decay/s={_drunkennessConfiguration.DecayPerSecond}");
			}
			if (!flag)
			{
				return;
			}
			if (_multiplayerModel.NetworkRunner == null)
			{
				Debug.LogWarning("[Drunk] ApplyDrunkenness: NetworkRunner is null");
				return;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				Debug.LogWarning($"[Drunk] ApplyDrunkenness: no stats for localPlayerId={playerId}");
				return;
			}
			IStat stat = value.GetStat(EntityStatType.Drunkenness);
			if (stat == null)
			{
				Debug.LogWarning("[Drunk] ApplyDrunkenness: Drunkenness IStat is null");
				return;
			}
			float fullValue = stat.FullValue;
			stat.MinValue = 0f;
			stat.MaxValue = Mathf.Max(stat.MaxValue, _drunkennessConfiguration.MaxDrunkenness);
			value.SynchronizeStatValues(EntityStatType.Drunkenness, RPCType.InAllWays);
			float num = _drunkennessConfiguration.MaxDrunkenness - stat.FullValue;
			float num2 = Mathf.Min(_drunkennessConfiguration.DrinkAddAmount, Mathf.Max(0f, num));
			if (num2 <= 0f)
			{
				if (_drunkennessConfiguration.DebugLog)
				{
					Debug.Log($"[Drunk] ApplyDrunkenness skipped: already at cap before={fullValue} room={num}");
				}
				return;
			}
			value.AddStatValueSynchronized(EntityStatType.Drunkenness, num2, RPCType.InAllWays);
			if (_drunkennessConfiguration.DebugLog)
			{
				Debug.Log($"[Drunk] drink +{num2:0.###} | before={fullValue:0.###} after={stat.FullValue:0.###} " + $"value={stat.Value:0.###} max={stat.MaxValue:0.###} " + $"min={stat.MinValue:0.###}");
			}
		}
	}
}

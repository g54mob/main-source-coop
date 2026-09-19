using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;

namespace Features.Movement.Scripts
{
	public class PlayerStaminaService : IPlayerStaminaService
	{
		private IStat _staminaStat;

		private IStat _hiddenStaminaStat;

		private IStat _hiddenStaminaRechargeRateStat;

		private IStat _hiddenStaminaDrainRate;

		private readonly PlayerStatsConfiguration _playerStatsConfiguration;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		public PlayerStaminaService(PlayerStatsConfiguration playerStatsConfiguration, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel)
		{
			_playerStatsConfiguration = playerStatsConfiguration;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
		}

		public void SubtractStaminaLogic(float subtractTime, float staminaDrainRate)
		{
			if (!TryInitializeStats())
			{
				return;
			}
			_staminaStat.Subtract(staminaDrainRate * subtractTime);
			if (_staminaStat.FullValue < 0f)
			{
				_staminaStat.OverrideValue(0f);
			}
			if (_staminaStat.FullValue <= 0f)
			{
				if (_hiddenStaminaStat.FullValue > 0f)
				{
					_hiddenStaminaStat.Subtract(_hiddenStaminaDrainRate.FullValue * subtractTime);
				}
				if (_hiddenStaminaStat.FullValue < 0f)
				{
					_hiddenStaminaStat.OverrideValue(0f);
				}
			}
			else
			{
				_hiddenStaminaStat.AddValue(_hiddenStaminaRechargeRateStat.FullValue * subtractTime);
				if (_hiddenStaminaStat.FullValue > _playerStatsConfiguration.PlayerStats[EntityStatType.HiddenStamina])
				{
					_hiddenStaminaStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.HiddenStamina]);
				}
			}
		}

		private bool TryInitializeStats()
		{
			if (!_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				return false;
			}
			_staminaStat = _spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].GetStat(EntityStatType.Stamina);
			_hiddenStaminaStat = _spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].GetStat(EntityStatType.HiddenStamina);
			_hiddenStaminaRechargeRateStat = _spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].GetStat(EntityStatType.HiddenStaminaRechargeRate);
			_hiddenStaminaDrainRate = _spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].GetStat(EntityStatType.HiddenStaminaDrainRate);
			return true;
		}
	}
}

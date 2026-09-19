using System;
using Features.DamageableTrackModule.Scripts;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using UnityEngine;
using Zenject;

namespace Features.RegenerationModule.Scripts
{
	public class RegenerationSystem : IInitializable, IDisposable
	{
		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly MultiplayerModel _multiplayerModel;

		private float _timerHealth;

		private float _timerStamina;

		public RegenerationSystem(PlayerDamageablesTrackModel playerDamageablesTrackModel, SpawnedEntityStatsModel spawnedEntityStatsModel, IGameUpdater gameUpdater, MultiplayerModel multiplayerModel)
		{
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_gameUpdater = gameUpdater;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += HandleRegeneration;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= HandleRegeneration;
		}

		private void HandleRegeneration()
		{
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				HandleStamina();
				if (_playerDamageablesTrackModel.AllPlayerDamageables.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
				{
					HandleHealth();
				}
			}
		}

		private void HandleHealth()
		{
			float fullValue = _spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].GetStat(EntityStatType.HealthRegen).FullValue;
			if (fullValue <= 0f)
			{
				_timerHealth = 0f;
				return;
			}
			_timerHealth += Time.deltaTime;
			if (!(_timerHealth <= 1f))
			{
				_timerHealth = 0f;
				_playerDamageablesTrackModel.AllPlayerDamageables[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].Heal(fullValue, isSynchronize: true);
			}
		}

		private void HandleStamina()
		{
			EntityStatEntityNetworkedBase entityStatEntityNetworkedBase = _spawnedEntityStatsModel.PlayerStats[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId];
			if (entityStatEntityNetworkedBase.GetStat(EntityStatType.StaminaRegen).FullValue <= 0f)
			{
				_timerStamina = 0f;
				return;
			}
			_timerStamina += Time.deltaTime;
			if (!(_timerStamina <= 1f))
			{
				_timerStamina = 0f;
				entityStatEntityNetworkedBase.GetStat(EntityStatType.Stamina).AddValue(entityStatEntityNetworkedBase.GetStat(EntityStatType.StaminaRegen).FullValue);
			}
		}
	}
}

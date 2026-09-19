using Features.DamageableTrackModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;

namespace Features.DebugModule.Scripts
{
	public class DamageableDebugPresenter : PresenterBehaviour<DamageableDebugViewBase>
	{
		private const float DebugStatValue = 10000f;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		public DamageableDebugPresenter(PlayerDamageablesTrackModel playerDamageablesTrackModel, MultiplayerModel multiplayerModel, SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_multiplayerModel = multiplayerModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.DamageButton.onClick.AddListener(DamageLocalPlayer);
			if (base.View.SetHealthTo10000Button != null)
			{
				base.View.SetHealthTo10000Button.onClick.AddListener(SetLocalPlayerHealthToDebugValue);
			}
			if (base.View.SetStaminaTo10000Button != null)
			{
				base.View.SetStaminaTo10000Button.onClick.AddListener(SetLocalPlayerStaminaToDebugValue);
			}
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.DamageButton.onClick.RemoveListener(DamageLocalPlayer);
			if (base.View.SetHealthTo10000Button != null)
			{
				base.View.SetHealthTo10000Button.onClick.RemoveListener(SetLocalPlayerHealthToDebugValue);
			}
			if (base.View.SetStaminaTo10000Button != null)
			{
				base.View.SetStaminaTo10000Button.onClick.RemoveListener(SetLocalPlayerStaminaToDebugValue);
			}
		}

		private void DamageLocalPlayer()
		{
			if (float.TryParse(base.View.HealthAmountInputField.text, out var result))
			{
				int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
				_playerDamageablesTrackModel.AllPlayerDamageables[playerId].Damage(new DamageData
				{
					Damage = result
				});
			}
		}

		private void SetLocalPlayerHealthToDebugValue()
		{
			if (TryGetLocalPlayerStatEntity(out var statEntity))
			{
				SetStatToDebugValue(statEntity, EntityStatType.Health);
			}
		}

		private void SetLocalPlayerStaminaToDebugValue()
		{
			if (TryGetLocalPlayerStatEntity(out var statEntity))
			{
				SetStatToDebugValue(statEntity, EntityStatType.Stamina);
				FillStatToCurrentMax(statEntity, EntityStatType.HiddenStamina);
				SetStatToDebugValue(statEntity, EntityStatType.PunishmentStamina);
			}
		}

		private bool TryGetLocalPlayerStatEntity(out EntityStatEntityNetworkedBase statEntity)
		{
			statEntity = null;
			if (_multiplayerModel.NetworkRunner == null)
			{
				return false;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			return _spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out statEntity);
		}

		private static void SetStatToDebugValue(EntityStatEntityNetworkedBase statEntity, EntityStatType statType)
		{
			IStat stat = statEntity.GetStat(statType);
			if (stat.NonModifiedMaxValue < 10000f)
			{
				stat.MaxValue = 10000f;
			}
			stat.OverrideValue(10000f);
			statEntity.SynchronizeStatValues(statType, RPCType.InAllWays);
		}

		private static void FillStatToCurrentMax(EntityStatEntityNetworkedBase statEntity, EntityStatType statType)
		{
			IStat stat = statEntity.GetStat(statType);
			stat.OverrideValue(stat.MaxValue);
			statEntity.SynchronizeStatValues(statType, RPCType.InAllWays);
		}
	}
}

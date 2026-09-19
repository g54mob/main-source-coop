using System.Collections.Generic;
using Features.DamageableTrackModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.LevelModule.Scripts.LevelTransition
{
	public class LevelTransitionStragglerDamageService : ILevelTransitionStragglerDamage
	{
		private const float STRAGGLER_DAMAGE = 1000f;

		private readonly BeachOccupancyModel _beachOccupancyModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		public LevelTransitionStragglerDamageService(BeachOccupancyModel beachOccupancyModel, MultiplayerModel multiplayerModel, PlayerDamageablesTrackModel playerDamageablesTrackModel)
		{
			_beachOccupancyModel = beachOccupancyModel;
			_multiplayerModel = multiplayerModel;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
		}

		public void DamageOffBeachStragglers()
		{
			LevelTransitionArea.RescanAllAreas();
			List<IDamageable> list = new List<IDamageable>();
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (!_beachOccupancyModel.Players.ContainsKey(activePlayer) && _playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(activePlayer.PlayerId, out var value))
				{
					list.Add(value);
				}
			}
			_beachOccupancyModel.Players.Clear();
			foreach (IDamageable item in list)
			{
				Debug.Log($"[HealthTrace] StragglerDamage p{item.NetworkObject.InputAuthority.PlayerId} amount={1000f:0.#} (off-beach at level transition)");
				item.Damage(new DamageData
				{
					Damage = 1000f,
					Source = DamageDataSourceExtensions.ForEnvironment(DamageType.LevelTransition)
				});
			}
		}
	}
}

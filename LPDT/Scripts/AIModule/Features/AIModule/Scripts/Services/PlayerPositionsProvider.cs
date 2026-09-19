using System.Collections.Generic;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts.Services
{
	public class PlayerPositionsProvider : IPlayerPositionsProvider
	{
		private readonly SpawnedPlayersModel _playersModel;

		[Inject]
		public PlayerPositionsProvider(SpawnedPlayersModel playersModel)
		{
			_playersModel = playersModel;
		}

		public List<Vector3> GetPlayerPositions()
		{
			List<Vector3> list = new List<Vector3>();
			if (_playersModel?.Players == null)
			{
				return list;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _playersModel.Players)
			{
				PlayerDataHolder value = player.Value;
				if (value?.NetworkObject != null)
				{
					list.Add(value.NetworkObject.transform.position);
				}
			}
			return list;
		}
	}
}

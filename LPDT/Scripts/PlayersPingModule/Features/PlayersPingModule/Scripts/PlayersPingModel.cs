using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.PlayersPingModule.Scripts
{
	[Serializable]
	public class PlayersPingModel : JsonSynchronizableBase<PlayersPingModel>, ISessionCleanup
	{
		private readonly MultiplayerModel _multiplayerModel;

		[field: SerializeField]
		public Global.SerializableDictionary.SerializableDictionary<int, double> PlayersPings { get; set; } = new Global.SerializableDictionary.SerializableDictionary<int, double>();

		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public PlayersPingModel(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public void Cleanup()
		{
			PlayersPings.Clear();
		}

		public void SetNewValue(int playerId, double playersPing)
		{
			PlayersPings[playerId] = playersPing;
		}

		public void RemovePlayer(int playerId)
		{
			PlayersPings.Remove(playerId);
		}

		protected override void PrepareForSynchronize()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null)
			{
				return;
			}
			HashSet<int> hashSet = new HashSet<int>();
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				hashSet.Add(activePlayer.PlayerId);
			}
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, double> playersPing in PlayersPings)
			{
				if (!hashSet.Contains(playersPing.Key))
				{
					list.Add(playersPing.Key);
				}
			}
			foreach (int item in list)
			{
				PlayersPings.Remove(item);
			}
		}

		protected override void SetNewValues(PlayersPingModel synchronizable, bool isSynchronizedOnStart)
		{
			foreach (KeyValuePair<int, double> playersPing in synchronizable.PlayersPings)
			{
				if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId != playersPing.Key)
				{
					PlayersPings[playersPing.Key] = playersPing.Value;
				}
			}
		}
	}
}

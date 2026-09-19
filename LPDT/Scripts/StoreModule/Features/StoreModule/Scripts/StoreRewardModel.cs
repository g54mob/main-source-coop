using System;
using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;

namespace Features.StoreModule.Scripts
{
	public class StoreRewardModel
	{
		private readonly PendingRewardModel _pendingRewardModel;

		private readonly LevelModel _levelModel;

		private readonly MultiplayerModel _multiplayerModel;

		public List<StoreRewardRequest> PendingRewards
		{
			get
			{
				List<StoreRewardRequest> list = new List<StoreRewardRequest>();
				NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
				foreach (PendingRewardEntry item in _pendingRewardModel.Enumerate())
				{
					StoreCardBehaviour storeCardBehaviour = ResolveCard(networkRunner, item.CardNetId);
					if (!(storeCardBehaviour == null))
					{
						list.Add(new StoreRewardRequest(storeCardBehaviour, item.TargetPlayerId));
					}
				}
				return list;
			}
		}

		public event Action OnPendingRewardsChanged;

		public StoreRewardModel(PendingRewardModel pendingRewardModel, LevelModel levelModel, MultiplayerModel multiplayerModel)
		{
			_pendingRewardModel = pendingRewardModel;
			_levelModel = levelModel;
			_multiplayerModel = multiplayerModel;
		}

		public void AddReward(StoreCardBehaviour storeCard, int targetPlayerId)
		{
			if (storeCard.CardData != null)
			{
				_pendingRewardModel.Define((int)storeCard.Object.Id.Raw, storeCard.CardDataId, StoreRewardColorPacking.Pack(storeCard.EffectiveSpawnColor), targetPlayerId, _levelModel.CurrentLevelNumber);
				this.OnPendingRewardsChanged?.Invoke();
			}
		}

		public void RemoveReward(StoreCardBehaviour storeCard, int targetPlayerId)
		{
			_pendingRewardModel.Remove((int)storeCard.Object.Id.Raw);
			this.OnPendingRewardsChanged?.Invoke();
		}

		public void RemoveRewardsForCard(StoreCardBehaviour storeCard)
		{
			_pendingRewardModel.Remove((int)storeCard.Object.Id.Raw);
			this.OnPendingRewardsChanged?.Invoke();
		}

		private static StoreCardBehaviour ResolveCard(NetworkRunner runner, int cardNetId)
		{
			if (cardNetId == 0)
			{
				return null;
			}
			NetworkObject networkObject = runner.FindObject(new NetworkId
			{
				Raw = (uint)cardNetId
			});
			if (networkObject == null)
			{
				return null;
			}
			if (!networkObject.TryGetComponent<StoreCardBehaviour>(out var component))
			{
				return null;
			}
			return component;
		}
	}
}

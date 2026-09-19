using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.GrabModule.Scripts;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreRewardSpawnSystem : IInitializable, IDisposable, ILevelContentSpawnProvider
	{
		private const float REWARD_DROP_HEIGHT = 0.3f;

		private const float REWARD_FORWARD_OFFSET = 1.3f;

		private const float REWARD_STACK_STEP = 0.6f;

		private readonly PendingRewardModel _pendingRewardModel;

		private readonly StorePoolConfiguration _storePoolConfiguration;

		private readonly LevelModel _levelModel;

		private readonly ICardItemSpawner _cardItemSpawner;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly TeleportationPointsEventClass _teleportationPointsEventClass;

		private readonly ILevelContentSpawnRegistry _levelContentSpawnRegistry;

		private readonly SpawnedRewardsModel _spawnedRewardsModel;

		public StoreRewardSpawnSystem(PendingRewardModel pendingRewardModel, StorePoolConfiguration storePoolConfiguration, LevelModel levelModel, ICardItemSpawner cardItemSpawner, MultiplayerModel multiplayerModel, TeleportationPointsEventClass teleportationPointsEventClass, ILevelContentSpawnRegistry levelContentSpawnRegistry, SpawnedRewardsModel spawnedRewardsModel)
		{
			_pendingRewardModel = pendingRewardModel;
			_storePoolConfiguration = storePoolConfiguration;
			_levelModel = levelModel;
			_cardItemSpawner = cardItemSpawner;
			_multiplayerModel = multiplayerModel;
			_teleportationPointsEventClass = teleportationPointsEventClass;
			_levelContentSpawnRegistry = levelContentSpawnRegistry;
			_spawnedRewardsModel = spawnedRewardsModel;
		}

		public void Initialize()
		{
			_levelContentSpawnRegistry.Register(this);
		}

		public void Dispose()
		{
			_levelContentSpawnRegistry.Unregister(this);
		}

		public async UniTask SpawnLevelContentAsync()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			List<PendingRewardEntry> list = _pendingRewardModel.Enumerate().ToList();
			int targetLevelNumber = _pendingRewardModel.GetTargetLevelNumber();
			_pendingRewardModel.Clear();
			_spawnedRewardsModel.ClearSpawnedRewards();
			if (list.Count == 0)
			{
				return;
			}
			if (targetLevelNumber != _levelModel.CurrentLevelNumber)
			{
				Debug.LogWarning($"[PendingRewardModel] drained {list.Count} reward(s) targeting level {targetLevelNumber} " + $"but current level is {_levelModel.CurrentLevelNumber}; discarding without spawn.");
				return;
			}
			List<TeleportationPoint> beachPoints = _teleportationPointsEventClass.BeachTeleportationPoints;
			List<PlayerRef> orderedPlayers = networkRunner.ActivePlayers.OrderBy((PlayerRef player) => player.PlayerId).ToList();
			Dictionary<int, int> spawnedPerPlayer = new Dictionary<int, int>();
			foreach (PendingRewardEntry item in list)
			{
				StoreCardData cardData = _storePoolConfiguration.ResolveCardData(item.CardDataId);
				if (cardData != null && cardData.GetRewardCardItemType() != CardItemType.Skin && !(cardData.GetRewardItemPrefab() == null) && TryResolveBeachAnchor(item.TargetPlayerId, beachPoints, orderedPlayers, out var position, out var rotation))
				{
					spawnedPerPlayer.TryGetValue(item.TargetPlayerId, out var value);
					spawnedPerPlayer[item.TargetPlayerId] = value + 1;
					Vector3 position2 = position + rotation * Vector3.forward * 1.3f + rotation * Vector3.right * (0.6f * (float)value) + Vector3.up * 0.3f;
					NetworkObject networkObject = await _cardItemSpawner.Spawn(cardData, position2, rotation, StoreRewardColorPacking.Unpack(item.ColorPacked));
					if (!(networkObject == null))
					{
						_spawnedRewardsModel.AddSpawnedReward(new SpawnedReward
						{
							SpawnedRewardNetworkObject = networkObject,
							SpawnedRewardType = cardData.GetRewardCardItemType(),
							SpawnedRewardGrabbable = networkObject.GetComponent<IPointGrabable>()
						});
					}
				}
			}
			_spawnedRewardsModel.InvokeOnAllRewardsSpawned();
		}

		private static bool TryResolveBeachAnchor(int targetPlayerId, List<TeleportationPoint> beachPoints, List<PlayerRef> orderedPlayers, out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			if (beachPoints == null || beachPoints.Count == 0)
			{
				return false;
			}
			int num = orderedPlayers.FindIndex((PlayerRef player) => player.PlayerId == targetPlayerId);
			TeleportationPoint teleportationPoint = ((num >= 0) ? beachPoints[num % beachPoints.Count] : beachPoints[0]);
			position = teleportationPoint.Position;
			rotation = teleportationPoint.Rotation;
			return true;
		}
	}
}

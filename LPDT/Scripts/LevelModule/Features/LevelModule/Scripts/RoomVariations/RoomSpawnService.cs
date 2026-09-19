using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.LevelModule.Scripts.RoomVariations
{
	public class RoomSpawnService : IRoomSpawnService
	{
		private readonly RoomVariationsConfiguration _roomVariationsConfiguration;

		private readonly MultiplayerModel _multiplayerModel;

		public RoomSpawnService(RoomVariationsConfiguration roomVariationsConfiguration, MultiplayerModel multiplayerModel)
		{
			_roomVariationsConfiguration = roomVariationsConfiguration;
			_multiplayerModel = multiplayerModel;
		}

		public async UniTask<bool> SpawnRoom(LevelType levelType, RoomType roomType, Transform positionReference, int variationIndex)
		{
			if (levelType == LevelType.None || roomType == RoomType.None)
			{
				return false;
			}
			if (!_roomVariationsConfiguration.TryGetVariation(levelType, roomType, variationIndex, out var variationPrefab))
			{
				return false;
			}
			return await _multiplayerModel.NetworkRunner.SpawnAsync(variationPrefab, positionReference.position, positionReference.rotation, null, delegate(NetworkRunner networkRunner, NetworkObject obj)
			{
				if (obj.TryGetComponent<NetworkRoomScene>(out var component))
				{
					component.OwnerLevel = levelType;
				}
			}) != null;
		}

		public bool TryGetRandomVariationIndex(LevelType levelType, RoomType roomType, out int variationIndex)
		{
			variationIndex = -1;
			if (levelType == LevelType.None || roomType == RoomType.None)
			{
				return false;
			}
			if (!_roomVariationsConfiguration.TryGetVariations(levelType, roomType, out var variations) || variations.Count == 0)
			{
				return false;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < variations.Count; i++)
			{
				list.Add(i);
			}
			if (list.Count == 0)
			{
				return false;
			}
			variationIndex = list[Random.Range(0, list.Count)];
			return true;
		}
	}
}

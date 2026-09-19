using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.ItemsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;

namespace Features.TeethModule.Scripts.Tooth
{
	public class CharacterTeethService : ICharacterTeethService
	{
		private readonly CharacterTeethModel _characterTeethModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IItemSpawnService _itemSpawnService;

		private readonly ToothCustomizationConfig _toothCustomizationConfig;

		private int LocalPlayerId => _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;

		public CharacterTeethService(CharacterTeethModel characterTeethModel, MultiplayerModel multiplayerModel, IItemSpawnService itemSpawnService, ToothCustomizationConfig toothCustomizationConfig)
		{
			_characterTeethModel = characterTeethModel;
			_multiplayerModel = multiplayerModel;
			_itemSpawnService = itemSpawnService;
			_toothCustomizationConfig = toothCustomizationConfig;
		}

		private bool TryGetLocalTeeth(out Dictionary<int, PlayerTooth> teeth)
		{
			return _characterTeethModel.CharacterToToothCustomizations.TryGetValue(LocalPlayerId, out teeth);
		}

		public void RefreshAllTooth()
		{
			if (!TryGetLocalTeeth(out var teeth))
			{
				return;
			}
			foreach (PlayerTooth value in teeth.Values)
			{
				value.EnableToothRPC();
			}
		}

		public void RefreshTooth(int toothId)
		{
			if (TryGetLocalTeeth(out var teeth) && teeth.TryGetValue(toothId, out var value) && value.ToothData.IsKnocked)
			{
				value.EnableToothRPC();
			}
		}

		public void RemoveTooth(int toothId, bool spawnObject)
		{
			if (TryGetLocalTeeth(out var teeth) && teeth.TryGetValue(toothId, out var value) && !value.ToothData.IsKnocked)
			{
				value.DisableToothRPC();
				if (spawnObject)
				{
					SpawnToothObject(value);
				}
			}
		}

		public void RemoveTooth(List<int> toothIds, bool spawnObject)
		{
			foreach (int toothId in toothIds)
			{
				RemoveTooth(toothId, spawnObject);
			}
		}

		public void RemoveRandomTooth(int count, bool spawnObject)
		{
			if (!TryGetLocalTeeth(out var teeth))
			{
				return;
			}
			List<PlayerTooth> list = new List<PlayerTooth>();
			foreach (PlayerTooth value in teeth.Values)
			{
				if (!value.ToothData.IsKnocked)
				{
					list.Add(value);
				}
			}
			for (int num = list.Count - 1; num > 0; num--)
			{
				int num2 = Random.Range(0, num + 1);
				int index = num;
				List<PlayerTooth> list2 = list;
				int index2 = num2;
				PlayerTooth playerTooth = list[num2];
				PlayerTooth playerTooth2 = list[num];
				PlayerTooth playerTooth3 = (list[index] = playerTooth);
				playerTooth3 = (list2[index2] = playerTooth2);
			}
			int num3 = Mathf.Min(count, list.Count);
			for (int i = 0; i < num3; i++)
			{
				list[i].DisableToothRPC();
				if (spawnObject)
				{
					SpawnToothObject(list[i]);
				}
			}
		}

		public void RemoveAllTooth(bool spawnObject)
		{
			if (!TryGetLocalTeeth(out var teeth))
			{
				return;
			}
			foreach (PlayerTooth value in teeth.Values)
			{
				if (!value.ToothData.IsKnocked)
				{
					value.DisableToothRPC();
					if (spawnObject)
					{
						SpawnToothObject(value);
					}
				}
			}
		}

		private void SpawnToothObject(PlayerTooth tooth)
		{
			ToothCustomizationData toothCustomizationData = _toothCustomizationConfig.MinToothAffectDamage[tooth.ToothData.Preset];
			_itemSpawnService.SpawnItemsMultiple(toothCustomizationData.LevelObjectData.Item, tooth.transform.position, toothCustomizationData.LevelObjectData.ItemCount, 0f, toothCustomizationData.LevelObjectData.UseSpread, null, toothCustomizationData.LevelObjectData.UseSpread, 10f).Forget();
		}
	}
}

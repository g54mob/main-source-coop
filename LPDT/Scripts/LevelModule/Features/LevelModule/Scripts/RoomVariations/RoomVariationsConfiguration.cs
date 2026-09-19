using System.Collections.Generic;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[CreateAssetMenu(fileName = "RoomVariationsConfiguration_Default", menuName = "Configurations/Levels/RoomVariationsConfiguration")]
	public class RoomVariationsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public Global.SerializableDictionary.SerializableDictionary<LevelType, RoomVariationsByLevel> RoomVariationsByLevel { get; private set; }

		public bool TryGetVariation(LevelType levelType, RoomType roomType, int variationIndex, out NetworkPrefabRef variationPrefab)
		{
			variationPrefab = default(NetworkPrefabRef);
			if (!TryGetVariations(levelType, roomType, out var variations))
			{
				return false;
			}
			if (variationIndex < 0 || variationIndex >= variations.Count)
			{
				return false;
			}
			variationPrefab = variations[variationIndex];
			return true;
		}

		public bool TryGetVariations(LevelType levelType, RoomType roomType, out List<NetworkPrefabRef> variations)
		{
			variations = null;
			if (RoomVariationsByLevel == null)
			{
				return false;
			}
			if (!RoomVariationsByLevel.TryGetValue(levelType, out var value))
			{
				return false;
			}
			return value.TryGetVariations(roomType, out variations);
		}
	}
}

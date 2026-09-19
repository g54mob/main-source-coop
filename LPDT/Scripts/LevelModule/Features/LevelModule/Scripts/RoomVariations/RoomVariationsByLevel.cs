using System;
using System.Collections.Generic;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[Serializable]
	public class RoomVariationsByLevel
	{
		[field: SerializeField]
		public Global.SerializableDictionary.SerializableDictionary<RoomType, List<NetworkPrefabRef>> RoomVariations { get; private set; }

		public bool TryGetVariations(RoomType roomType, out List<NetworkPrefabRef> variations)
		{
			variations = null;
			if (RoomVariations != null && RoomVariations.TryGetValue(roomType, out variations))
			{
				return variations != null;
			}
			return false;
		}
	}
}

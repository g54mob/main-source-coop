using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[Serializable]
	public class LootRegistryEntry
	{
		[Tooltip("Unity asset GUID for stable reference")]
		public string assetGuid;

		[Tooltip("Addressable address for runtime loading")]
		public string address;

		[Tooltip("Human-readable display name")]
		public string displayName;

		[Tooltip("Category for filtering and visualization")]
		public LootCategory category;

		[Tooltip("Direct asset reference for editor operations")]
		public AssetReferenceGameObject assetReference;

		public Color GetCategoryColor()
		{
			return LootCategoryHelper.GetCategoryColor(category);
		}

		public override string ToString()
		{
			return $"{displayName} ({category})";
		}

		public override bool Equals(object obj)
		{
			if (obj is LootRegistryEntry lootRegistryEntry)
			{
				return assetGuid == lootRegistryEntry.assetGuid;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return assetGuid?.GetHashCode() ?? 0;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[CreateAssetMenu(menuName = "NomadDrive/World Generation/Loot Registry", fileName = "LootRegistry")]
	public class LootRegistry : ScriptableObject
	{
		public const string LOOT_LABEL = "Loot";

		[SerializeField]
		private List<LootRegistryEntry> _entries = new List<LootRegistryEntry>();

		public IReadOnlyList<LootRegistryEntry> Entries => _entries;

		public int Count => _entries.Count;

		public IEnumerable<LootRegistryEntry> GetByCategory(LootCategory category)
		{
			return _entries.Where((LootRegistryEntry e) => e.category == category);
		}

		public LootRegistryEntry GetByAddress(string address)
		{
			return _entries.FirstOrDefault((LootRegistryEntry e) => e.address == address);
		}

		public LootRegistryEntry GetByGuid(string guid)
		{
			return _entries.FirstOrDefault((LootRegistryEntry e) => e.assetGuid == guid);
		}

		public Dictionary<LootCategory, List<LootRegistryEntry>> GetGroupedByCategory()
		{
			Dictionary<LootCategory, List<LootRegistryEntry>> dictionary = new Dictionary<LootCategory, List<LootRegistryEntry>>();
			foreach (LootCategory value in Enum.GetValues(typeof(LootCategory)))
			{
				dictionary[value] = new List<LootRegistryEntry>();
			}
			foreach (LootRegistryEntry entry in _entries)
			{
				dictionary[entry.category].Add(entry);
			}
			return dictionary;
		}
	}
}

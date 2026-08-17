using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Locking
{
	[CreateAssetMenu(menuName = "NomadDrive/Locking/Chest Loot Registry", fileName = "ChestLootRegistry")]
	public class ChestLootRegistry : ScriptableObject
	{
		[SerializeField]
		private ChestType chestType;

		[SerializeField]
		private List<ChestLootEntry> entries = new List<ChestLootEntry>();

		public ChestType ChestType => chestType;

		public IReadOnlyList<ChestLootEntry> Entries => entries;

		public bool HasEntries
		{
			get
			{
				if (entries != null)
				{
					return entries.Count > 0;
				}
				return false;
			}
		}

		private void OnValidate()
		{
			NormalizeWeights();
		}

		private void NormalizeWeights()
		{
			if (entries == null || entries.Count == 0)
			{
				return;
			}
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				ChestLootEntry chestLootEntry = entries[i];
				if (chestLootEntry != null)
				{
					if (chestLootEntry.weight < 0f)
					{
						chestLootEntry.weight = 0f;
					}
					num += chestLootEntry.weight;
					num2++;
				}
			}
			if (num2 == 0)
			{
				return;
			}
			if (num <= 0f)
			{
				float weight = 1f / (float)num2;
				for (int j = 0; j < entries.Count; j++)
				{
					ChestLootEntry chestLootEntry2 = entries[j];
					if (chestLootEntry2 != null)
					{
						chestLootEntry2.weight = weight;
					}
				}
			}
			else
			{
				if (Mathf.Approximately(num, 1f))
				{
					return;
				}
				for (int k = 0; k < entries.Count; k++)
				{
					ChestLootEntry chestLootEntry3 = entries[k];
					if (chestLootEntry3 != null)
					{
						chestLootEntry3.weight = Mathf.Clamp01(chestLootEntry3.weight / num);
					}
				}
			}
		}
	}
}

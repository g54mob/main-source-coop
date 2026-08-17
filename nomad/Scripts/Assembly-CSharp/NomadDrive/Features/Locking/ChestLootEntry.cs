using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.Locking
{
	[Serializable]
	public class ChestLootEntry
	{
		public string displayName;

		[Tooltip("Sadece 'Loot' Addressable label'ına sahip prefab'lar seçilebilir.")]
		[AssetReferenceUILabelRestriction(new string[] { "Loot" })]
		public AssetReferenceGameObject prefab;

		[Tooltip("Goreceli agirlik. ChestLootRegistry.OnValidate butun entry'lerin toplamini 1'e normalize eder.")]
		[Range(0f, 1f)]
		public float weight = 0.5f;
	}
}

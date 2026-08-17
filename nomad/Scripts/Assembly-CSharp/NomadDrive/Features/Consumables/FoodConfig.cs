using UnityEngine;

namespace NomadDrive.Features.Consumables
{
	[CreateAssetMenu(menuName = "NomadDrive/Consumables/Food", fileName = "FoodConfig")]
	public class FoodConfig : ScriptableObject
	{
		[Tooltip("Poisonous even when fresh.")]
		public bool isPoisonous;

		[Tooltip("Becomes poisonous once it spoils (rots). On by default so all spoiled food is dangerous to eat.")]
		public bool poisonousWhenRotten = true;

		public float poisonHealthDecrease;

		[Tooltip("Poison added to the bar immediately when this item poisons you (eaten while poisonous or spoiled).")]
		public float poisonInitialAmount = 5f;

		[Tooltip("Poison added to the cumulative per-minute poisoning rate. Repeat consumption stacks this, so the bar fills faster.")]
		public float poisonRatePerMinute = 2f;

		[Range(1f, 10f)]
		public float spoilageMultiplier;

		[Tooltip("Roll the starting durability (freshness, 0-100) within the range below, derived deterministically from the loot spawn seed so a given world seed always produces the same freshness here. When off, food spawns full at the range max.")]
		public bool randomizeInitialDurability = true;

		[Tooltip("Min (x) / Max (y) starting durability. Higher = food stays fresh longer before rotting.")]
		public Vector2 durabilityRange = new Vector2(60f, 100f);

		public bool hasRottenMaterial;

		public Material rottenMaterial;
	}
}

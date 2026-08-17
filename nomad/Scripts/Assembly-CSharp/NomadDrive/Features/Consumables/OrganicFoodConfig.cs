using EvilCore;
using UnityEngine;

namespace NomadDrive.Features.Consumables
{
	[CreateAssetMenu(menuName = "NomadDrive/Consumables/Organic Food", fileName = "OrganicFoodConfig")]
	public class OrganicFoodConfig : ScriptableObject
	{
		public CookingStateMaterialSet cookingStateMaterials;

		public SerializableDictionary<CookedLevel, float> cookingDurationsCollection;

		public SerializableDictionary<CookedLevel, float> playerStatsModifiers;
	}
}

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.Planting
{
	[CreateAssetMenu(menuName = "NomadDrive/Planting/Plant Config", fileName = "PlantConfig")]
	public class PlantConfig : SerializedScriptableObject
	{
		[Tooltip("The type of plant")]
		public PlantType PlantType;

		[Tooltip("Display name of the plant")]
		public string DisplayName;

		[Tooltip("Plant icon for UI")]
		public Sprite PlantIcon;

		[Tooltip("Growth durations and visual references for each phase")]
		public List<PlantPhaseData> Phases = new List<PlantPhaseData>();

		[Tooltip("Time before plant starts drying after watering (seconds)")]
		public float DryingTimeAfterWatering = 120f;

		[Tooltip("Should the pot reset to empty after harvest?")]
		public bool ResetAfterHarvest = true;

		[Tooltip("Prefab to spawn when plant reaches FullyGrown state. Must have NetworkIdentity + NetworkedTransform")]
		public GameObject HarvestPrefab;

		[Tooltip("Auto-filled Addressable GUID of HarvestPrefab so the save system can respawn picked/carried harvest items. The HarvestPrefab must be marked Addressable.")]
		[SerializeField]
		private string harvestPrefabAddressableGuid;

		public string HarvestPrefabAddressableGuid => harvestPrefabAddressableGuid;

		public int TotalPhases => Phases.Count;

		public float GetGrowthTimeForPhase(int phaseIndex)
		{
			if (phaseIndex < 0 || phaseIndex >= Phases.Count)
			{
				return 0f;
			}
			return Phases[phaseIndex].GrowthDuration;
		}

		public float GetWaterRequiredForPhase(int phaseIndex)
		{
			if (phaseIndex < 0 || phaseIndex >= Phases.Count)
			{
				return 0f;
			}
			return Phases[phaseIndex].WaterRequired;
		}

		public bool IsLastPhase(int phaseIndex)
		{
			return phaseIndex >= Phases.Count - 1;
		}
	}
}

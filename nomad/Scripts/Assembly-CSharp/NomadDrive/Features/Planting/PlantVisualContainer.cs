using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Planting
{
	[Serializable]
	public class PlantVisualContainer
	{
		[Tooltip("Which plant type this container belongs to")]
		public PlantType PlantType;

		[Tooltip("Visual GameObjects for each phase (ordered)")]
		public List<GameObject> PhaseVisuals = new List<GameObject>();

		[Tooltip("Spawn points for harvest items. Each transform's local position and rotation will be used.")]
		public List<Transform> HarvestSpawnPoints = new List<Transform>();

		public void ActivatePhase(int phaseIndex)
		{
			DeactivateAll();
			if (phaseIndex >= 0 && phaseIndex < PhaseVisuals.Count)
			{
				PhaseVisuals[phaseIndex]?.SetActive(value: true);
			}
		}

		public void DeactivateAll()
		{
			if (PhaseVisuals == null)
			{
				return;
			}
			foreach (GameObject phaseVisual in PhaseVisuals)
			{
				phaseVisual?.SetActive(value: false);
			}
		}
	}
}

using System;
using UnityEngine;

namespace NomadDrive.Features.Planting
{
	[Serializable]
	public class PlantPhaseData
	{
		[Tooltip("Name of the phase (for editor display)")]
		public string PhaseName = "Phase";

		[Tooltip("Duration to stay in this phase after watering (in seconds)")]
		public float GrowthDuration = 60f;

		[Tooltip("Amount of water required to complete this phase (in ml)")]
		public float WaterRequired = 10f;

		[Tooltip("Visual GameObject index for this phase (child index in PlantPot prefab)")]
		public int VisualIndex;
	}
}

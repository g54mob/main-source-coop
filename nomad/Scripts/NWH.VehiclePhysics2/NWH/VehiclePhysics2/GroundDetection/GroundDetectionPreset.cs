using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.GroundDetection
{
	[Serializable]
	[CreateAssetMenu(fileName = "NWH Vehicle Physics 2", menuName = "NWH/Vehicle Physics 2/Ground Detection Preset", order = 1)]
	public class GroundDetectionPreset : ScriptableObject
	{
		[FormerlySerializedAs("dustPrefab")]
		[Tooltip("    Prefab of the particle system for generating dust as a result of traveling over sand, gravel, etc.")]
		public GameObject particlePrefab;

		[Tooltip("    Prefab of the particle system for generating surface chunks / dirt that gets thrown behind the wheel when going over soft surface.")]
		public GameObject chunkPrefab;

		[Tooltip("    Surface preset used when there are no matches in the surfaceMaps list for the current surface.")]
		public SurfacePreset fallbackSurfacePreset;

		[Tooltip("    Surface maps - each represents a single ground surface.")]
		public List<SurfaceMap> surfaceMaps = new List<SurfaceMap>();
	}
}

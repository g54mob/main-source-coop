using System;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	[Serializable]
	public class RoadObjectVariant
	{
		[Tooltip("Sign prefab variant.")]
		public GameObject prefab;

		[Tooltip("Relative selection weight (higher = appears more often).")]
		[Min(0.01f)]
		public float weight = 1f;
	}
}

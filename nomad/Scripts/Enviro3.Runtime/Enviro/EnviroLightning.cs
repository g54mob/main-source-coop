using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroLightning
	{
		public Lightning prefab;

		public GameObject customLightningEffect;

		public bool lightningStorm;

		[Range(1f, 60f)]
		public float randomLightingDelay = 10f;

		[Range(0f, 10000f)]
		public float randomSpawnRange = 5000f;

		[Range(0f, 10000f)]
		public float randomTargetRange = 5000f;

		[Range(0f, 10000f)]
		public float cloudsLightningRadius = 2500f;

		[Range(0.1f, 5f)]
		public float cloudsLightningDuration = 1f;
	}
}

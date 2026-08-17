using System;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[Serializable]
	public struct LootSpawnDataNetwork
	{
		public string lootPrefabGuid;

		public float chance;

		public Vector3 initialRotationEuler;

		public RotationAxis randomRotationAxis;

		public float minRotationAngle;

		public float maxRotationAngle;

		public bool ignoreCollisionCorrection;

		public bool HasValidReference => !string.IsNullOrEmpty(lootPrefabGuid);
	}
}

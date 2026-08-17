using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[Serializable]
	public class LootSpawnData
	{
		[Tooltip("Direct reference to the loot prefab. Uses Unity GUID for stable references.")]
		public AssetReferenceGameObject lootPrefab;

		[Range(0f, 100f)]
		[Tooltip("Spawn probability (0-100%). All chances in a spawn point should sum to 100%.")]
		public float chance;

		[Tooltip("Initial rotation in local space (Euler angles).")]
		public Vector3 initialRotationEuler;

		[Tooltip("Axes to apply random rotation on.")]
		public RotationAxis randomRotationAxis;

		[Tooltip("Minimum random rotation angle.")]
		public float minRotationAngle;

		[Tooltip("Maximum random rotation angle.")]
		public float maxRotationAngle = 360f;

		[Tooltip("If true, skip the placement solver (raycast/overlap correction) and spawn at the authored position.")]
		public bool ignoreCollisionCorrection;

		public bool HasValidReference
		{
			get
			{
				if (lootPrefab != null)
				{
					return !string.IsNullOrEmpty(lootPrefab.AssetGUID);
				}
				return false;
			}
		}

		public LootSpawnDataNetwork ToNetworkData()
		{
			return new LootSpawnDataNetwork
			{
				lootPrefabGuid = (lootPrefab?.AssetGUID ?? string.Empty),
				chance = chance,
				initialRotationEuler = initialRotationEuler,
				randomRotationAxis = randomRotationAxis,
				minRotationAngle = minRotationAngle,
				maxRotationAngle = maxRotationAngle,
				ignoreCollisionCorrection = ignoreCollisionCorrection
			};
		}

		public static LootSpawnDataNetwork[] ToNetworkArray(LootSpawnData[] datas)
		{
			if (datas == null)
			{
				return null;
			}
			LootSpawnDataNetwork[] array = new LootSpawnDataNetwork[datas.Length];
			for (int i = 0; i < datas.Length; i++)
			{
				array[i] = datas[i]?.ToNetworkData() ?? default(LootSpawnDataNetwork);
			}
			return array;
		}
	}
}

using System;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.Thumbleweed
{
	[Serializable]
	public class ThumbleweedPrefabVariant
	{
		[SerializeField]
		private GameObject prefab;

		[SerializeField]
		[Min(0.01f)]
		private float weight = 1f;

		public GameObject Prefab => prefab;

		public float Weight => weight;
	}
}

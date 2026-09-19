using System;
using Fusion;
using UnityEngine;

namespace Features.BarBeachInteractableModule.Scripts
{
	[Serializable]
	public class BarBottlePrefabWeight
	{
		[SerializeField]
		private NetworkObject _prefab;

		[SerializeField]
		[Min(0f)]
		private float _weight = 1f;

		public NetworkObject Prefab => _prefab;

		public float Weight => _weight;
	}
}

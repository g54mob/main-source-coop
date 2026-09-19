using System;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[Serializable]
	public class MonkeyItemGiftEntry
	{
		[field: SerializeField]
		public NetworkBehaviour ItemPrefab { get; private set; }

		[field: SerializeField]
		public bool IsCanActivateItem { get; private set; }

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float ActivateItemChance { get; private set; } = 0.3f;

		[field: SerializeField]
		[field: Min(0f)]
		public float ActivateItemDelay { get; private set; }

		[field: SerializeField]
		public Vector3 GiftSpawnOffset { get; private set; }
	}
}

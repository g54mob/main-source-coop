using Fusion;
using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	[CreateAssetMenu(fileName = "KrakenAggressiveThrowConfiguration_Default", menuName = "Configurations/Kraken/KrakenAggressiveThrowConfiguration")]
	public class KrakenAggressiveThrowConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float ThrowWindowSeconds { get; private set; } = 30f;

		[field: SerializeField]
		public int MinTotalThrowsToTrigger { get; private set; } = 5;

		[field: SerializeField]
		public float Damage { get; private set; } = 50f;

		[field: SerializeField]
		public float KnockbackForce { get; private set; } = 20f;

		[field: SerializeField]
		public float HitTrackingTime { get; private set; } = 3f;

		[field: SerializeField]
		public float HitRadius { get; private set; } = 1f;

		[field: SerializeField]
		public float AggressiveThrowCooldown { get; private set; } = 20f;

		[field: SerializeField]
		public NetworkObject KrakenRockPrefab { get; private set; }
	}
}

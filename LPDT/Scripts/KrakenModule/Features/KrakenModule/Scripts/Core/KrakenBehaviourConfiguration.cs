using FMODUnity;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Core
{
	[CreateAssetMenu(fileName = "KrakenBehaviourConfiguration_Default", menuName = "Configurations/Kraken/KrakenBehaviourConfiguration")]
	public class KrakenBehaviourConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float InactivityHideDelay { get; private set; } = 30f;

		[field: SerializeField]
		public float ItemAppearTriggerWindowSeconds { get; private set; } = 10f;

		[field: SerializeField]
		public int ItemAppearTriggerCount { get; private set; } = 3;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float HalfQuotaAppearThreshold { get; private set; } = 0.5f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float FullQuotaAppearThreshold { get; private set; } = 1f;

		[field: SerializeField]
		public float StateAnimationFallbackSeconds { get; private set; } = 1.5f;

		[field: SerializeField]
		public float QuotaAppearEmoteDelay { get; private set; } = 0.5f;

		[field: SerializeField]
		public EventReference StoreKrakenMuteSnapshot { get; private set; }
	}
}

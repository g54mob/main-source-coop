using UnityEngine;

namespace Features.TrampolineModule.Scripts
{
	[CreateAssetMenu(fileName = "TrampolineConfiguration_Default", menuName = "Configurations/TrampolineModule/TrampolineConfiguration")]
	public class TrampolineConfiguration : ScriptableObject
	{
		[Header("Target")]
		[field: SerializeField]
		public LayerMask AffectedLayers { get; private set; }

		[field: SerializeField]
		[field: Tooltip("If true, only bounce players currently in ragdoll simulation.")]
		public bool RequireRagdoll { get; private set; }

		[Header("Bounce height (meters)")]
		[field: SerializeField]
		[field: Min(0f)]
		[field: Tooltip("Upward launch height on the first bounce in a chain.")]
		public float InitialBounceHeight { get; private set; } = 0.75f;

		[field: SerializeField]
		[field: Min(0f)]
		[field: Tooltip("Meters added to bounce height on each subsequent bounce.")]
		public float BounceHeightGrowthPerStep { get; private set; } = 0.5f;

		[field: SerializeField]
		[field: Min(0f)]
		[field: Tooltip("Maximum bounce height the chain can reach.")]
		public float MaxBounceHeight { get; private set; } = 5f;

		[Header("Bounce count limit")]
		[field: SerializeField]
		[field: Tooltip("When enabled, bounce chain stops after MaxBounceCount bounces (until chain reset). When disabled, player can bounce indefinitely at MaxBounceHeight.")]
		public bool UseBounceCountLimit { get; private set; }

		[field: SerializeField]
		[field: Min(1f)]
		[field: Tooltip("Max bounces in a chain when UseBounceCountLimit is enabled.")]
		public int MaxBounceCount { get; private set; } = 8;

		[Header("Timing / gate")]
		[field: SerializeField]
		[field: Min(0f)]
		[field: Tooltip("Minimum relative impact speed required to bounce.")]
		public float MinImpactSpeed { get; private set; } = 0.5f;

		[field: SerializeField]
		[field: Min(0f)]
		[field: Tooltip("Seconds before the same player can bounce again.")]
		public float CooldownPerPlayer { get; private set; } = 0.35f;

		[field: SerializeField]
		[field: Min(0f)]
		[field: Tooltip("Seconds without bouncing before the height chain resets to InitialBounceHeight.")]
		public float BounceChainResetTime { get; private set; } = 3f;

		[Header("Surface animation (stretch scrub 0..1)")]
		[field: SerializeField]
		[field: Min(0.01f)]
		[field: Tooltip("Seconds to go from current pose toward target compression (mat down).")]
		public float CompressDuration { get; private set; } = 0.08f;

		[field: SerializeField]
		[field: Min(0.01f)]
		[field: Tooltip("Seconds to release from compression back to rest (mat up).")]
		public float ReleaseDuration { get; private set; } = 0.18f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		[field: Tooltip("Minimum Compression when a bounce is accepted (even tiny hops register visually).")]
		public float MinVisualCompression { get; private set; } = 0.08f;
	}
}

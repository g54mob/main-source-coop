using System.Collections.Generic;
using Features.VignetteUIEffectModule.Scripts;
using UnityEngine;

namespace Features.PostProcessingModule.Scripts
{
	[CreateAssetMenu(fileName = "VignetteAffectionConfiguration_Default", menuName = "Configurations/PostProcessing/VignetteAffectionConfiguration")]
	public class VignetteAffectionConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<PostProcessingType> AffectedPostProcessingTypes { get; private set; }

		[field: SerializeField]
		public List<VignetteUIEffectType> AffectedVignetteUIEffectTypes { get; private set; }

		[field: SerializeField]
		public float MaxAffectionHpPercent { get; private set; }

		[field: SerializeField]
		public Color MaxAffectionColor { get; private set; }

		[field: SerializeField]
		public float MaxAffectionIntensity { get; private set; }

		[field: SerializeField]
		public float DamageAnimationRedDuration { get; private set; } = 0.3f;

		[field: SerializeField]
		public float DamageAnimationReturnDuration { get; private set; } = 0.5f;

		[field: SerializeField]
		public float MaxDamageAnimationIntensity { get; private set; } = 0.8f;

		[field: SerializeField]
		public float MaxDamagePercentForFullIntensity { get; private set; } = 0.2f;

		[field: SerializeField]
		public Color StunStateColor { get; private set; }

		[field: SerializeField]
		public float StunStateIntensity { get; private set; }

		[field: SerializeField]
		public float StunStateSmoothness { get; private set; }

		[field: SerializeField]
		public Color SpectatorStateColor { get; private set; }

		[field: SerializeField]
		public float SpectatorStateIntensity { get; private set; }

		[field: SerializeField]
		public float SpectatorStateSmoothness { get; private set; }

		[field: SerializeField]
		public Color DeadStateColor { get; private set; }

		[field: SerializeField]
		public float DeadStateIntensity { get; private set; }

		[field: SerializeField]
		public float DeadStateSmoothness { get; private set; }

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float StaminaPunishmentIntensityMultiplier { get; private set; }
	}
}

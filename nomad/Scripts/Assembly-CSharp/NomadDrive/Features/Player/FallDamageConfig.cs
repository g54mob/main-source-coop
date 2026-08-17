using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Fall Damage Config")]
	public class FallDamageConfig : ScriptableObject
	{
		[Header("Height Thresholds")]
		[Tooltip("Falls at or below this height (meters) deal no damage.")]
		[SerializeField]
		[Min(0f)]
		private float safeFallHeight = 4f;

		[Tooltip("Falls at or above this height (meters) deal the maximum damage.")]
		[SerializeField]
		[Min(0.1f)]
		private float lethalFallHeight = 18f;

		[Header("Damage")]
		[Tooltip("Minimum damage dealt the instant a fall exceeds the safe height. The curve scales the total up from here to Max Damage. Keep this <= Max Damage.")]
		[SerializeField]
		[Min(0f)]
		private float minDamage = 5f;

		[Tooltip("Damage applied at (or above) the lethal fall height. 100 = guaranteed death.")]
		[SerializeField]
		[Min(0f)]
		private float maxDamage = 100f;

		[Tooltip("Maps normalized fall height (0 = safe height, 1 = lethal height) to normalized damage (0..1), which is then multiplied by Max Damage. Curve the rise for a sharper increase as the fall gets taller.")]
		[SerializeField]
		private AnimationCurve damageByHeightCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[Header("Impact Sound")]
		[Tooltip("All of these sounds are played simultaneously the moment fall damage is taken.")]
		[SerializeField]
		private SoundID[] impactSounds;

		public float SafeFallHeight => safeFallHeight;

		public float LethalFallHeight => lethalFallHeight;

		public float MaxDamage => maxDamage;

		public IReadOnlyList<SoundID> ImpactSounds => impactSounds;

		public float EvaluateDamage(float fallHeight)
		{
			if (fallHeight <= safeFallHeight)
			{
				return 0f;
			}
			float time = Mathf.InverseLerp(safeFallHeight, lethalFallHeight, fallHeight);
			float t = Mathf.Clamp01(damageByHeightCurve.Evaluate(time));
			return Mathf.Lerp(minDamage, maxDamage, t);
		}
	}
}

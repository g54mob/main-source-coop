using FMODUnity;
using UnityEngine;

namespace Features.RumModule.Scripts
{
	[CreateAssetMenu(fileName = "DrunkennessConfiguration_Default", menuName = "Configurations/Rum/DrunkennessConfiguration")]
	public class DrunkennessConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public bool DebugLog { get; private set; } = true;

		[field: SerializeField]
		public float DrinkAddAmount { get; private set; } = 1f;

		[field: SerializeField]
		public float MaxDrunkenness { get; private set; } = 5f;

		[field: SerializeField]
		public float DecayPerSecond { get; private set; } = 0.05f;

		[Header("Haze (local PP)")]
		[field: SerializeField]
		public AnimationCurve HazeBlendByDrunkenness { get; private set; } = AnimationCurve.Linear(0f, 0f, 2f, 1f);

		[Tooltip("How fast Volume haze eases toward the curve target (blend units / second).")]
		[field: SerializeField]
		[field: Min(0.01f)]
		public float HazeBlendLerpSpeed { get; private set; } = 1.25f;

		[Header("Eyes (all peers)")]
		[field: SerializeField]
		public Color EyeFlushColor { get; private set; } = new Color(0.75f, 0.1f, 0.1f, 1f);

		[field: SerializeField]
		public AnimationCurve EyeFlushByDrunkenness { get; private set; } = AnimationCurve.Linear(0f, 0f, 2f, 1f);

		[Header("Voice (heard by others)")]
		[field: SerializeField]
		public string VoiceDistortionParameterName { get; private set; } = "Distortion";

		[field: SerializeField]
		public string VoiceLowPassParameterName { get; private set; } = "LowPass";

		[field: SerializeField]
		public string VoicePitchParameterName { get; private set; } = "Pitch";

		[field: SerializeField]
		public string VoiceReverbParameterName { get; private set; } = "Reverb";

		[field: SerializeField]
		public AnimationCurve VoiceDistortionByDrunkenness { get; private set; } = AnimationCurve.Linear(0f, 0f, 5f, 0.38f);

		[field: SerializeField]
		public AnimationCurve VoiceLowPassByDrunkenness { get; private set; } = AnimationCurve.Linear(0f, 1f, 5f, 0.35f);

		[field: SerializeField]
		public AnimationCurve VoicePitchByDrunkenness { get; private set; } = AnimationCurve.Linear(0f, 0f, 5f, 0.7f);

		[field: SerializeField]
		public AnimationCurve VoiceReverbByDrunkenness { get; private set; } = AnimationCurve.Linear(0f, 0f, 5f, 0.48f);

		[Header("Pass-out (DrunkFaint)")]
		[field: SerializeField]
		public float FaintMinDrunkenness { get; private set; } = 3f;

		[field: SerializeField]
		public float FaintFadeInSeconds { get; private set; } = 2.5f;

		[field: SerializeField]
		public float FaintDurationSeconds { get; private set; } = 5f;

		[field: SerializeField]
		public float FaintCooldownSeconds { get; private set; } = 12f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float FaintBlackoutIntensity { get; private set; } = 1f;

		[field: SerializeField]
		public bool UseStaminaUiVignette { get; private set; }

		[field: SerializeField]
		public bool UseStaminaFocusPulse { get; private set; }

		[Header("Drunk faint snore")]
		[Tooltip("Loop while DrunkFaint ragdoll reason is active (Sleeper idle).")]
		[field: SerializeField]
		public EventReference FaintSnoreLoopEvent { get; private set; }

		public bool AddsDrunkennessOnDrink(RumType rumType)
		{
			if (rumType != RumType.HazeRum && rumType != RumType.ParticleRum)
			{
				return rumType == RumType.ExplosionRum;
			}
			return true;
		}

		public float EvaluateHazeBlend(float drunkenness)
		{
			return Mathf.Clamp01(HazeBlendByDrunkenness.Evaluate(Mathf.Max(0f, drunkenness)));
		}

		public float EvaluateEyeFlush(float drunkenness)
		{
			return Mathf.Clamp01(EyeFlushByDrunkenness.Evaluate(Mathf.Max(0f, drunkenness)));
		}

		public float EvaluateVoiceDistortion(float drunkenness)
		{
			return VoiceDistortionByDrunkenness.Evaluate(Mathf.Max(0f, drunkenness));
		}

		public float EvaluateVoiceLowPass(float drunkenness)
		{
			return Mathf.Clamp01(VoiceLowPassByDrunkenness.Evaluate(Mathf.Max(0f, drunkenness)));
		}

		public float EvaluateVoicePitch(float drunkenness)
		{
			return VoicePitchByDrunkenness.Evaluate(Mathf.Max(0f, drunkenness));
		}

		public float EvaluateVoiceReverb(float drunkenness)
		{
			return VoiceReverbByDrunkenness.Evaluate(Mathf.Max(0f, drunkenness));
		}
	}
}

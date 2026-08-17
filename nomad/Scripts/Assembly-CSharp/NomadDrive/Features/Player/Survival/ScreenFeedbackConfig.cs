using Ami.BroAudio;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Player.Survival
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Screen Feedback Config", fileName = "ScreenFeedbackConfig")]
	public class ScreenFeedbackConfig : ScriptableObject
	{
		[Header("Damage Vignette")]
		[Tooltip("Pass ID under CustomPassManager (GameObject name with 'CustomPass' suffix stripped).")]
		public string vignettePassId = "DamageVignette";

		[Tooltip("X = 1 - (health / maxHealth). Y = vignette intensity (0..1).")]
		public AnimationCurve vignetteByDamageRatio = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.4f, 0f), new Keyframe(0.7f, 0.35f), new Keyframe(1f, 1f));

		public float vignetteSmoothTime = 0.35f;

		public Color vignetteColor = new Color(0.6f, 0.05f, 0.05f, 1f);

		[Tooltip("Inner radius of the vignette gradient in NDC units (0 = screen center).")]
		[Range(0f, 1f)]
		public float radiusInner = 0.35f;

		[Tooltip("Outer radius of the vignette gradient in NDC units.")]
		[Range(0f, 1.5f)]
		public float radiusOuter = 0.95f;

		[Header("Health — Critical Vital Feedback")]
		[Tooltip("EffectiveHealth ratio (0..1) below which the vital feedback (vignette, grayscale, chroma, heartbeat) begins. Silent above this; ramps LINEARLY from 0 here to each effect's authored peak at 0 health.")]
		[Range(0f, 1f)]
		public float criticalHealthThreshold = 0.25f;

		[Header("Damage Pulse Flash")]
		[Tooltip("Additional intensity added on top of base vignette when a damage tick fires.")]
		[Range(0f, 1f)]
		public float pulseAmplitude = 0.45f;

		[Tooltip("Total duration of the pulse (rise + fall).")]
		public float pulseDuration = 0.4f;

		public Ease pulseEase = Ease.OutQuad;

		[Header("Fatigue Blink — Critical phase")]
		[Tooltip("Random delay range between blinks while energy is critically low (seconds).")]
		public Vector2 criticalIntervalRange = new Vector2(3.5f, 5.5f);

		[Tooltip("Total duration of one critical blink (eye-close + eye-open).")]
		public float criticalBlinkDuration = 0.35f;

		[Tooltip("Max alpha reached during a critical blink (0..1).")]
		[Range(0f, 1f)]
		public float criticalBlinkAlpha = 0.6f;

		public Ease criticalBlinkEase = Ease.OutSine;

		[Header("Fatigue Blink — Zero phase")]
		[Tooltip("Random delay range between blinks while energy is at zero (seconds).")]
		public Vector2 zeroIntervalRange = new Vector2(1.2f, 2f);

		[Tooltip("Total duration of one zero-phase blink (eye-close + eye-open).")]
		public float zeroBlinkDuration = 0.7f;

		[Tooltip("How long the screen stays fully closed at peak alpha (seconds).")]
		public float zeroBlinkHoldAtClosed = 0.25f;

		[Tooltip("Max alpha reached during a zero blink (0..1).")]
		[Range(0f, 1f)]
		public float zeroBlinkAlpha = 1f;

		public Ease zeroBlinkEase = Ease.InOutSine;

		[Tooltip("Fade-out duration when energy recovers above the critical threshold.")]
		public float fatigueFadeOutDuration = 0.6f;

		[Header("Audio — Low HP Heartbeat")]
		public SoundID heartbeatLoopSound;

		[Tooltip("X = 1 - (health / maxHealth). Y = heartbeat loop volume (0..1).")]
		public AnimationCurve heartbeatVolumeByDamageRatio = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 0f), new Keyframe(0.75f, 0.4f), new Keyframe(1f, 1f));

		public float heartbeatFadeDuration = 0.4f;

		[Header("Health — Life-to-Death Saturation")]
		[Tooltip("X = 1 - (health / maxHealth). Y = additive ColorAdjustments.saturation offset. Drives the screen toward grayscale as health drops (life/death metaphor). At zero health, push to -100 for full B&W. Composes additively with the energy saturation curve.")]
		public AnimationCurve saturationByHealthDamageRatio = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.3f, 0f), new Keyframe(0.7f, -30f), new Keyframe(1f, -100f));

		[Tooltip("X = 1 - (health / maxHealth). Y = additive ChromaticAberration intensity. Stays at 0 until health drops past the dying threshold (damage ratio 0.8 → health ~20%), then rises sharply. Critical-only visual cue. Composes additively with energy chroma.")]
		public AnimationCurve chromaticAberrationByHealthDamageRatio = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.8f, 0f, 0f, 0f), new Keyframe(0.85f, 0.18f), new Keyframe(1f, 0.55f));

		[Header("Audio — Energy Critical Cue (one-shot + interval repeat)")]
		[Tooltip("Non-spatial one-shot fired when energy drops below its critical-low threshold (fatigue/yawn/sigh cue, NOT a heavy-breath loop). Repeats at energyCriticalRepeatIntervalSeconds while energy stays below threshold AND health > 0. Recovery upward through the threshold rearms the trigger.")]
		public SoundID energyCriticalEnterSound;

		[Tooltip("Seconds between repeats while energy is below critical AND health > 0. Set to 0 to disable repeats (cue only fires once on initial entry).")]
		[Min(0f)]
		public float energyCriticalRepeatIntervalSeconds = 15f;

		[Header("Audio — Nutrition Critical Cue (one-shot + interval repeat)")]
		[Tooltip("Non-spatial one-shot fired when nutrition drops below its critical-low threshold (stomach growl / weak groan / hungry sigh). Repeats at nutritionCriticalRepeatIntervalSeconds while nutrition stays below threshold AND health > 0. Recovery upward through the threshold rearms the trigger.")]
		public SoundID nutritionCriticalEnterSound;

		[Tooltip("Seconds between repeats while nutrition is below critical AND health > 0. Set to 0 to disable repeats (cue only fires once on initial entry).")]
		[Min(0f)]
		public float nutritionCriticalRepeatIntervalSeconds = 15f;

		[Header("HDRP Volume — Energy Distortion (Baseline, very mild)")]
		[Tooltip("Baseline curve. X = 1 - (energy / maxEnergy). LENS = MOTION-SICKNESS RISK. Keep it tiny — distortion should be subliminal, never dominant.")]
		public AnimationCurve lensDistortionByEnergyDepletion = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.8f, 0f), new Keyframe(1f, -0.02f));

		[Tooltip("Baseline curve. X = 1 - (energy / maxEnergy). Y = ChromaticAberration intensity. Mild until post-zero ramp kicks in.")]
		public AnimationCurve chromaticAberrationByEnergyDepletion = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.8f, 0f), new Keyframe(1f, 0.08f));

		[Tooltip("Baseline curve. X = 1 - (energy / maxEnergy). Y = ColorAdjustments.saturation offset. Mild until post-zero ramp kicks in.")]
		public AnimationCurve saturationByEnergyDepletion = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.8f, 0f), new Keyframe(1f, -8f));

		[Header("Breath Sync — Periodic Modulation")]
		[Tooltip("Breath frequency in Hz used to oscillate lens/chroma values. ~0.4 Hz = 24 BPM exhausted breathing.")]
		public float breathSyncFrequency = 0.4f;

		[Tooltip("Peak-to-peak amplitude added to LensDistortion intensity. DEFAULT 0 — continuous geometric warping on breath frequency triggers motion sickness. Leave at 0 unless intentionally tuning low values.")]
		public float lensDistortionBreathAmplitude;

		[Tooltip("Peak-to-peak amplitude added to ChromaticAberration intensity, scaled by energy depletion. Chroma breath swing is safe (color separation, not geometry).")]
		public float chromaticAberrationBreathAmplitude = 0.05f;

		[Header("Blink Peak Spike (eye-closing peak)")]
		[Tooltip("Extra LensDistortion intensity blended in during the closed peak of a blink (additive). Brief, so a small value is enough; large values cause nausea.")]
		public float blinkLensDistortionSpike = -0.1f;

		[Tooltip("Extra ChromaticAberration intensity blended in during the closed peak of a blink (additive).")]
		public float blinkChromaticAberrationSpike = 0.4f;

		[Tooltip("Tween duration for the blink-peak distortion spike rise/fall.")]
		public float blinkSpikeDuration = 0.18f;

		public Ease blinkSpikeEase = Ease.InOutQuad;

		[Header("Stumble Events (random consciousness flicker)")]
		[Tooltip("Baseline. X = 1 - (energy / maxEnergy). Y = probability per second. Stays low until post-zero ramp scales it up via postZeroStumbleChanceMultiplier.")]
		public AnimationCurve stumbleChanceByEnergyDepletion = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.85f, 0f), new Keyframe(1f, 0.05f));

		[Tooltip("Extra LensDistortion intensity at stumble peak (additive). Short-lived spike; keep small to avoid nausea on frequent stumbles.")]
		public float stumbleLensDistortionSpike = -0.18f;

		[Tooltip("Extra ChromaticAberration intensity at stumble peak (additive).")]
		public float stumbleChromaticAberrationSpike = 0.6f;

		[Tooltip("Total duration of a stumble (rise + fall).")]
		public float stumbleDuration = 0.55f;

		public Ease stumbleEase = Ease.OutCubic;

		[Tooltip("Minimum seconds between two stumbles to avoid clustering.")]
		public float stumbleMinCooldown = 1.5f;

		[Header("Post-Zero Energy Escalation")]
		[Tooltip("Once energy hits 0 and stays there, this is how many real seconds it takes for the post-zero ramp to reach 1.0 (full peak intensity).")]
		public float postZeroEscalationDuration = 3.5f;

		[Tooltip("How fast the post-zero ramp bleeds back to 0 after energy recovers above zero. Lower = snappier visual recovery when player heals up.")]
		public float postZeroRecoveryDuration = 0.6f;

		[Tooltip("Optional shaping curve applied to the linear 0..1 ramp. Use ease-in to delay the peak, ease-out to hit it fast.")]
		public AnimationCurve postZeroEscalationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

		[Tooltip("Extra LensDistortion intensity added at full post-zero ramp (additive on top of baseline curve). Keep small — sustained fisheye is the worst nausea trigger.")]
		public float postZeroLensDistortionPeak = -0.12f;

		[Tooltip("Extra ChromaticAberration intensity added at full post-zero ramp (additive).")]
		public float postZeroChromaticAberrationPeak = 0.4f;

		[Tooltip("Extra saturation offset added at full post-zero ramp (additive). Negative = more desaturated.")]
		public float postZeroSaturationPeak = -28f;

		[Tooltip("Multiplier on lens/chroma breath amplitudes at full post-zero ramp (1 = unchanged, 2 = double swing).")]
		public float postZeroBreathAmplitudeMultiplier = 2.5f;

		[Tooltip("Multiplier on stumbleChanceByEnergyDepletion at full post-zero ramp. Combined with the curve.")]
		public float postZeroStumbleChanceMultiplier = 4f;
	}
}

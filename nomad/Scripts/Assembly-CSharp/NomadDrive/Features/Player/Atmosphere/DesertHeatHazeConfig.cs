using UnityEngine;

namespace NomadDrive.Features.Player.Atmosphere
{
	[CreateAssetMenu(fileName = "DesertHeatHazeConfig", menuName = "NomadDrive/Atmosphere/Desert Heat Haze Config")]
	public class DesertHeatHazeConfig : ScriptableObject
	{
		[Header("Custom Pass")]
		[Tooltip("Pass id = the CustomPass GameObject name with the 'CustomPass' suffix stripped. GO 'DesertHeatHazeCustomPass' -> id 'DesertHeatHaze'.")]
		public string passId = "DesertHeatHaze";

		[Header("Strength Drivers")]
		[Tooltip("X = temperature (Celsius), Y = 0..1 strength multiplier. Hotter -> stronger haze.")]
		public AnimationCurve temperatureCurve = AnimationCurve.Linear(15f, 0f, 45f, 1f);

		[Tooltip("X = hour of day (0..24), Y = 0..1 strength multiplier. Peaks around midday, ~0 at night.")]
		public AnimationCurve timeOfDayCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(7f, 0f), new Keyframe(13f, 1f), new Keyframe(18f, 0.25f), new Keyframe(21f, 0f), new Keyframe(24f, 0f));

		[Tooltip("Overall ceiling for distortion strength after the temperature * time-of-day multipliers.")]
		[Range(0f, 1f)]
		public float maxStrength = 1f;

		[Tooltip("Seconds to smoothly ramp strength when conditions change (sunrise / sunset / temp shifts).")]
		[Min(0.01f)]
		public float strengthSmoothTime = 1.5f;

		[Header("Distortion Look")]
		[Tooltip("Max UV offset magnitude in screen space. Keep small (0.003-0.02) to avoid TAA smearing.")]
		[Range(0f, 0.05f)]
		public float distortionScale = 0.012f;

		[Tooltip("Noise tiling. Higher = finer / busier shimmer.")]
		[Min(0.1f)]
		public float noiseScale = 18f;

		[Tooltip("Noise scroll speed. X = horizontal drift, Y = upward rise (heat rising).")]
		public Vector2 scrollSpeed = new Vector2(0.05f, 0.22f);

		[Tooltip("0 = equal X/Y wobble, 1 = purely vertical (heat-rise) wobble.")]
		[Range(0f, 1f)]
		public float verticalBias = 0.65f;

		[Header("Masking")]
		[Tooltip("View-space distance (meters) where the haze begins. Closer than this stays crisp.")]
		[Min(0f)]
		public float distanceStart = 25f;

		[Tooltip("View-space distance (meters) where the haze reaches full strength.")]
		[Min(0f)]
		public float distanceFull = 120f;

		[Tooltip("Screen height (0 = bottom, 1 = top) above which sky shimmer fades out. Keeps the upper sky calm while the horizon / distant ground shimmers.")]
		[Range(0f, 1f)]
		public float horizonTop = 0.6f;

		[Header("Debug (Editor only)")]
		[Tooltip("Editor only: ignore the temperature / time-of-day gating and force the haze to 'Debug Strength Override' so you can tune the LOOK at any time of day. Has no effect in builds.")]
		public bool debugForceStrength;

		[Tooltip("Editor only: strength used while 'Debug Force Strength' is on.")]
		[Range(0f, 1f)]
		public float debugStrengthOverride = 0.6f;

		[Tooltip("Editor only: scrub 'Debug Hour' (0..24) to preview how the effect looks across the day via the Time Of Day Curve, WITHOUT changing the in-game clock. Temperature is treated as fully hot while scrubbing so the time curve is the only driver. Ignored if 'Debug Force Strength' is on. No effect in builds.")]
		public bool debugScrubTimeOfDay;

		[Tooltip("Editor only: previewed hour of day (0 = midnight, 12 = noon, 24 = next midnight) used while 'Debug Scrub Time Of Day' is on.")]
		[Range(0f, 24f)]
		public float debugHour = 13f;
	}
}

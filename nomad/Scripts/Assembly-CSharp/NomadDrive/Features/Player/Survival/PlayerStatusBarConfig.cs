using UnityEngine;

namespace NomadDrive.Features.Player.Survival
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Player Status Bar Config", fileName = "PlayerStatusBarConfig")]
	public class PlayerStatusBarConfig : ScriptableObject
	{
		[Header("Zone Encroachment Limits")]
		[Tooltip("Maximum bar percentage Nutrition depletion can claim (1.0 = can fill entire bar)")]
		[Range(0f, 1f)]
		public float nutritionMaxEncroachment = 1f;

		[Tooltip("Maximum bar percentage Hydration depletion can claim")]
		[Range(0f, 1f)]
		public float hydrationMaxEncroachment = 1f;

		[Tooltip("Maximum bar percentage Energy depletion can claim")]
		[Range(0f, 1f)]
		public float energyMaxEncroachment = 0.5f;

		[Tooltip("Maximum bar percentage direct damage can claim")]
		[Range(0f, 1f)]
		public float damageMaxEncroachment = 1f;

		[Tooltip("Maximum bar percentage poison can claim. Full poison narrows the effective-health area by this much.")]
		[Range(0f, 1f)]
		public float poisonMaxEncroachment = 1f;

		[Header("Animation")]
		[Min(0.01f)]
		public float zoneSmoothTime = 0.4f;

		[Min(0.001f)]
		public float zoneSnapEpsilon = 0.002f;

		[Tooltip("Quantize zone display targets to this step size (0-1 bar ratio). Prevents sub-pixel jitter from continuous float changes. E.g. 0.01 = zones update in 1% increments.")]
		[Min(0.001f)]
		public float zoneStepSize = 0.01f;

		[Tooltip("Minimum zone size (0-1 bar ratio) to become visible. Below this the zone is hidden. E.g. 0.08 = zone appears when it would occupy 8% of the bar.")]
		[Range(0f, 0.3f)]
		public float zoneAppearThreshold = 0.08f;

		[Tooltip("Separate appear threshold for the DAMAGE (health) zone only. 0 = damage is always shown with no minimum; the other stat zones keep using Zone Appear Threshold.")]
		[Range(0f, 0.3f)]
		public float damageZoneAppearThreshold;

		[Tooltip("Separate appear threshold for the POISON zone only. 0 = poison is always shown with no minimum, so the small initial poison amount is visible immediately.")]
		[Range(0f, 0.3f)]
		public float poisonZoneAppearThreshold;

		[Header("Critical State")]
		[Range(0f, 1f)]
		public float criticalEffectiveHealthThreshold = 0.2f;

		public Color criticalPulseColor = new Color(0.8f, 0f, 0f, 1f);

		[Min(0.05f)]
		public float criticalPulseDurationMax = 0.9f;

		[Min(0.05f)]
		public float criticalPulseDurationMin = 0.18f;

		[Header("Death")]
		[Tooltip("Small buffer before declaring death to prevent micro-deaths from float precision")]
		[Min(0f)]
		public float deathThreshold = 0.001f;

		[Header("Downed (Chicken Form)")]
		[Tooltip("While the player is a chicken the bar is a PURELY VISUAL full fill in this color (not a real stat).")]
		public Color downedFormColor = new Color(0.95f, 0.75f, 0.2f, 1f);

		[Tooltip("Icon shown centered on the bar while downed (chicken form). Optional — leave null to skip the icon.")]
		public Sprite downedFormIcon;

		[Tooltip("Seconds to smoothly blend the bar into / out of the chicken-form fill.")]
		[Min(0.01f)]
		public float downedFormBlendDuration = 0.5f;
	}
}

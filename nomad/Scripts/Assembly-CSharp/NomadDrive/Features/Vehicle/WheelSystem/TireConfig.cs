using UnityEngine;

namespace NomadDrive.Features.Vehicle.WheelSystem
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Tire", fileName = "TireConfig")]
	public class TireConfig : ScriptableObject
	{
		public TireType tireType;

		[Header("Wear")]
		[Tooltip("Base wear rate coefficient fed to NWH TyreWear. Higher = faster wear.")]
		public float wearRate = 0.01f;

		[Tooltip("Scales the base wear rate by condition (X = condition ratio 0..1, Y = wear-rate multiplier). Flat 1 = constant rate; ramp up toward X=0 to make worn tires degrade faster.")]
		public AnimationCurve wearRateMultiplierByCondition = AnimationCurve.Constant(0f, 1f, 1f);

		[Tooltip("NWH TyreWear: effect of wheel load on wear.")]
		public float loadWearContribution = 1f;

		[Tooltip("NWH TyreWear: effect of lateral slip on wear.")]
		public float lateralSlipWearContribution = 1f;

		[Tooltip("NWH TyreWear: effect of longitudinal slip on wear.")]
		public float longitudinalSlipWearContribution = 1f;

		[Tooltip("NWH TyreWear coroutine update period (seconds).")]
		[Range(0.01f, 0.5f)]
		public float updateRate = 0.1f;

		[Header("Physical Effect")]
		[Tooltip("Grip multiplier vs condition (X = condition ratio 0..1, Y = grip multiplier 0..1). 1 at full condition; drop toward X=0 so worn tires lose grip. Set critical points via keyframes.")]
		public AnimationCurve gripMultiplierByCondition = AnimationCurve.Linear(0f, 0.3f, 1f, 1f);
	}
}

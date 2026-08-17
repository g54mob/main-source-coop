using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Radiator
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Radiator", fileName = "RadiatorConfig", order = 2)]
	public class RadiatorConfig : SerializedScriptableObject
	{
		[Header("Cooling")]
		[Tooltip("Base cooling multiplier at 100% condition and 100% coolant")]
		public float coolingEfficiency = 1.2f;

		[Tooltip("Cooling multiplier used when no radiator is installed (air cooling only)")]
		public float noRadiatorFallbackRate = 0.3f;

		[Header("Coolant")]
		[Tooltip("Maximum coolant capacity in liters")]
		public float coolantCapacity = 100f;

		[Tooltip("Coolant consumed per second while engine is running")]
		public float coolantConsumptionRate = 0.1f;

		[Header("Efficiency Curves")]
		[Tooltip("Maps radiator condition (0-1) to cooling efficiency factor (0-1)")]
		public AnimationCurve conditionEfficiencyCurve = new AnimationCurve(new Keyframe(0f, 0.1f), new Keyframe(0.25f, 0.5f), new Keyframe(0.5f, 0.8f), new Keyframe(1f, 1f));

		[Tooltip("Maps coolant fill ratio (0-1) to cooling efficiency factor (0-1)")]
		public AnimationCurve coolantEfficiencyCurve = new AnimationCurve(new Keyframe(0f, 0.15f), new Keyframe(0.3f, 0.6f), new Keyframe(0.7f, 0.9f), new Keyframe(1f, 1f));

		[Header("Degradation")]
		[Tooltip("Minimum efficiency when coolant is completely empty (air cooling residual)")]
		[Range(0f, 1f)]
		public float noCoolantMinEfficiency = 0.15f;
	}
}

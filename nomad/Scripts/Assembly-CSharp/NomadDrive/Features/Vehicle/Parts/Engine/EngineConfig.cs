using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Engine
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Engine", fileName = "EngineConfig", order = 0)]
	public class EngineConfig : SerializedScriptableObject
	{
		public float maxPower;

		public float fuelEfficiency;

		[Tooltip("Scales how fast the gas tank drains while driving (NWH consumptionMultiplier). 1 = NWH's physical rate (slow); raise to make fuel drop noticeably. ~60 ≈ a full 50L tank lasts ~40 min cruising / ~12 min full throttle.")]
		public float fuelConsumptionMultiplier = 60f;

		public float conditionConsumptionRate;

		public float idleConditionConsumptionRate;

		public float runningConditionConsumptionRate;

		public float engineConsumptionSpeed;

		[Header("Heat")]
		[Tooltip("Heat added per second at idle (engine running, no throttle).")]
		public float idleHeat = 0.7f;

		[Tooltip("Heat added per second at full engine load (peak power output).")]
		public float throttleHeat = 0.9f;

		[Tooltip("Constant heat dissipated per second while the engine runs (the engine's own baseline cooling, before the radiator).")]
		public float runningCoolRate = 0.3f;

		[Tooltip("Heat removed per second while the engine is OFF or the part is detached (cools fastest).")]
		public float engineOffCoolRate = 2f;

		[Tooltip("Max heat removed per second by a full, healthy radiator. Scaled by the effective coolant ratio (coolant fill * radiator condition).")]
		public float radiatorCoolRate = 0.3f;

		[Tooltip("Temperature-feedback cooling: heat removed per second scales with current heat (heat/100). Makes the engine settle at a stable operating temperature; higher = lower plateau and a slower climb.")]
		public float tempCoolFactor = 0.375f;

		[Tooltip("Heat level (0-100) at which the engine overheats and is force-stopped. Must be <= 100 or it can never trigger.")]
		public float overheatThreshold = 95f;

		[Tooltip("Heat level the engine must cool back down to before it can be restarted.")]
		public float restartThreshold = 55f;

		[Header("Overheat Damage")]
		[Tooltip("Heat level above which the engine starts taking permanent condition damage.")]
		public float overheatDamageThreshold = 85f;

		[Tooltip("Condition lost per second at maximum heat (scaled down toward the damage threshold).")]
		public float overheatDamageRate = 1.5f;

		[Tooltip("Extra heating multiplier when the engine is fully broken (0 = damage does not affect heating).")]
		public float damageHeatPenalty = 0.3f;
	}
}

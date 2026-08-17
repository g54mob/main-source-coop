using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Battery
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Battery", fileName = "BatteryConfig", order = 4)]
	public class BatteryConfig : ScriptableObject
	{
		public float maxPower;

		public float conditionConsumptionRate;

		public float lowBeamUsageMultiplier;

		public float highBeamUsageMultiplier;

		public float wiperSlowUsageMultiplier;

		public float wiperFastUsageMultiplier;

		public float indoorLightUsageMultiplier;
	}
}

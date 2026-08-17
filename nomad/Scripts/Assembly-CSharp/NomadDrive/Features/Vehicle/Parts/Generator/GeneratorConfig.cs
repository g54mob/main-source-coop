using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Generator
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Generator", fileName = "GeneratorConfig", order = 2)]
	public class GeneratorConfig : ScriptableObject
	{
		public float maxPower;

		public float fuelConsumption;

		public float fuelCapacity;
	}
}

using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.Planting
{
	public class PlantSeed : HeldItem
	{
		[Tooltip("The type of plant this seed will grow")]
		public PlantType PlantType;

		public override bool Weaved()
		{
			return true;
		}
	}
}

using NomadDrive.Features.Attachables;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Vehicle.Parts.Generator
{
	public class Generator : AttachableObject
	{
		[FormerlySerializedAs("generatorData")]
		public GeneratorConfig generatorConfig;

		public override bool Weaved()
		{
			return true;
		}
	}
}

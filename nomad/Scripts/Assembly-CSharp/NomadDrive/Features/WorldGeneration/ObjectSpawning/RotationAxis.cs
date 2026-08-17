using System;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[Flags]
	public enum RotationAxis
	{
		None = 0,
		X = 1,
		Y = 2,
		Z = 4,
		All = 7
	}
}

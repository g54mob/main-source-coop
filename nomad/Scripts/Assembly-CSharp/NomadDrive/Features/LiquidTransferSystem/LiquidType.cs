using System;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[Flags]
	public enum LiquidType
	{
		Empty = 0,
		Water = 1,
		Gasoline = 2,
		Oil = 4,
		Coffee = 8,
		Milk = 0x10,
		Coolant = 0x20
	}
}

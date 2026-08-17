using System;

namespace EvilCore.EvilPack.EvilLogger
{
	[Flags]
	public enum LogCategory
	{
		None = 0,
		General = 1,
		Network = 2,
		Vehicle = 4,
		Interaction = 8,
		WorldGen = 0x10,
		Player = 0x20,
		Audio = 0x40,
		UI = 0x80,
		Physics = 0x100,
		Animation = 0x200,
		Save = 0x400,
		DI = 0x800,
		All = -1
	}
}

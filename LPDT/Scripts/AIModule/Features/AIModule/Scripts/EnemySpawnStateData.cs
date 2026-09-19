using System.Runtime.InteropServices;
using Fusion;

namespace Features.AIModule.Scripts
{
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	[NetworkStructWeaved(3)]
	public struct EnemySpawnStateData : INetworkStruct
	{
		[FieldOffset(0)]
		public int NextSpawnSessionTime;

		[FieldOffset(4)]
		public int TotalSpawnedCount;

		[FieldOffset(8)]
		public int MaxSpawnedCount;
	}
}

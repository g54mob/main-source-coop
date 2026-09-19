using System.Runtime.InteropServices;
using Fusion;

namespace Features.StatsUsageModule.Scripts.Entities
{
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	[NetworkStructWeaved(2)]
	public struct StatWithValue : INetworkStruct
	{
		[FieldOffset(0)]
		public int StatType;

		[FieldOffset(4)]
		public float Value;

		public StatWithValue(int statType, float value)
		{
			StatType = statType;
			Value = value;
		}
	}
}

using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 40)]
	[NetworkStructWeaved(10)]
	public struct NetworkPhysicsInfo : INetworkStruct
	{
		[FieldOffset(0)]
		public float TimeScale;

		public const int SIZE = 40;

		public const int WORD_COUNT = 10;

		internal const int BYTE_OF_TIME_SCALE = 0;

		internal const int BYTE_COUNT_OF_TIME_SCALE = 4;
	}
}

using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	internal struct SimulationMessageInternal_TestMessage
	{
		[FieldOffset(0)]
		public int Item;

		[FieldOffset(4)]
		public NetworkId Id;

		[FieldOffset(8)]
		public int Value;

		public const int SIZE = 12;

		public const int WORD_COUNT = 3;

		internal const int BYTE_OF_ITEM = 0;

		internal const int BYTE_COUNT_OF_ITEM = 4;

		internal const int BYTE_OF_ID = 4;

		internal const int BYTE_COUNT_OF_ID = 4;

		internal const int BYTE_OF_VALUE = 8;

		internal const int BYTE_COUNT_OF_VALUE = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;
	}
}

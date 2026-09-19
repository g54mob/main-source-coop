using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	internal struct SimulationMessageInternal_SharedModeSetAlwaysInterested
	{
		[FieldOffset(0)]
		public NetworkId Object;

		[FieldOffset(4)]
		public int Interested;

		[FieldOffset(8)]
		public int Player;

		public const int SIZE = 12;

		public const int WORD_COUNT = 3;

		internal const int BYTE_OF_OBJECT = 0;

		internal const int BYTE_COUNT_OF_OBJECT = 4;

		internal const int BYTE_OF_INTERESTED = 4;

		internal const int BYTE_COUNT_OF_INTERESTED = 4;

		internal const int BYTE_OF_PLAYER = 8;

		internal const int BYTE_COUNT_OF_PLAYER = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;
	}
}

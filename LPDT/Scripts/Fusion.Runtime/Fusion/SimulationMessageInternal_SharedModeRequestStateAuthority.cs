using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	internal struct SimulationMessageInternal_SharedModeRequestStateAuthority
	{
		[FieldOffset(0)]
		public NetworkId Object;

		[FieldOffset(4)]
		public int Acquire;

		public const int SIZE = 8;

		public const int WORD_COUNT = 2;

		internal const int BYTE_OF_OBJECT = 0;

		internal const int BYTE_COUNT_OF_OBJECT = 4;

		internal const int BYTE_OF_ACQUIRE = 4;

		internal const int BYTE_COUNT_OF_ACQUIRE = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;
	}
}

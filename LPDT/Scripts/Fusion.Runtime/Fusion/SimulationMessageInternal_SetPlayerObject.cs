using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	internal struct SimulationMessageInternal_SetPlayerObject
	{
		[FieldOffset(0)]
		public NetworkId Object;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_OBJECT = 0;

		internal const int BYTE_COUNT_OF_OBJECT = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;
	}
}

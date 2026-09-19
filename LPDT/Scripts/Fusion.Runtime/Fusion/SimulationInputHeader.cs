using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct SimulationInputHeader
	{
		[FieldOffset(0)]
		public Tick Tick;

		[FieldOffset(4)]
		public float InterpAlpha;

		[FieldOffset(8)]
		public Tick InterpFrom;

		[FieldOffset(12)]
		public Tick InterpTo;

		public const int SIZE = 16;

		public const int WORD_COUNT = 4;

		internal const int BYTE_OF_TICK = 0;

		internal const int BYTE_COUNT_OF_TICK = 4;

		internal const int BYTE_OF_INTERP_ALPHA = 4;

		internal const int BYTE_COUNT_OF_INTERP_ALPHA = 4;

		internal const int BYTE_OF_INTERP_FROM = 8;

		internal const int BYTE_COUNT_OF_INTERP_FROM = 4;

		internal const int BYTE_OF_INTERP_TO = 12;

		internal const int BYTE_COUNT_OF_INTERP_TO = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;
	}
}

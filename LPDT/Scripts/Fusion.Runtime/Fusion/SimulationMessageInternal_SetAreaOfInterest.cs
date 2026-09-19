using System.Runtime.InteropServices;
using UnityEngine;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	internal struct SimulationMessageInternal_SetAreaOfInterest
	{
		[FieldOffset(0)]
		public Vector3 Center;

		[FieldOffset(12)]
		public float Radius;

		public const int SIZE = 16;

		public const int WORD_COUNT = 4;

		internal const int BYTE_OF_CENTER = 0;

		internal const int BYTE_COUNT_OF_CENTER = 12;

		internal const int BYTE_OF_RADIUS = 12;

		internal const int BYTE_COUNT_OF_RADIUS = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 8)]
	[NetworkStructWeaved(2)]
	public struct _2 : INetworkStruct, IFixedStorage, IReadOnlySpanAssignable
	{
		public const int SIZE = 8;

		[FieldOffset(0)]
		public unsafe fixed uint Data[2];

		[FieldOffset(0)]
		[NonSerialized]
		private uint _data0;

		[FieldOffset(4)]
		[NonSerialized]
		private uint _data1;

		public unsafe void Set(ReadOnlySpan<uint> values)
		{
			fixed (uint* data = Data)
			{
				values.CopyTo(new Span<uint>(data, 2));
			}
		}
	}
}

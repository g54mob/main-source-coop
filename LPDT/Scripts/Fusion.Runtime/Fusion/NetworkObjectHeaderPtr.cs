using System;

namespace Fusion
{
	public struct NetworkObjectHeaderPtr
	{
		public unsafe NetworkObjectHeader* Ptr;

		public unsafe readonly NetworkObjectTypeId Type => Ptr->Type;

		public unsafe readonly NetworkId Id => Ptr->Id;

		public unsafe readonly NetworkObjectHeaderFlags Flags => Ptr->Flags;

		public unsafe readonly NetworkId NestingRoot => Ptr->NestingRoot;

		public unsafe readonly Span<int> Data => new Span<int>((byte*)Ptr + (nint)20 * (nint)4, Ptr->WordCount - 20);

		public unsafe NetworkObjectHeaderPtr(NetworkObjectHeader* ptr)
		{
			Ptr = ptr;
		}
	}
}

using System;

namespace Obi
{
	public class ObiNativeIntPtrList : ObiNativeList<IntPtr>
	{
		public ObiNativeIntPtrList()
		{
		}

		public ObiNativeIntPtrList(int capacity = 8, int alignment = 16)
			: base(capacity, alignment)
		{
			for (int i = 0; i < capacity; i++)
			{
				base[i] = IntPtr.Zero;
			}
		}
	}
}

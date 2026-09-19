using System;

namespace Obi
{
	[Serializable]
	public class ObiNativeEdgeList : ObiNativeList<Edge>
	{
		public ObiNativeEdgeList()
		{
		}

		public ObiNativeEdgeList(int capacity = 8, int alignment = 16)
			: base(capacity, alignment)
		{
			for (int i = 0; i < capacity; i++)
			{
				base[i] = default(Edge);
			}
		}
	}
}

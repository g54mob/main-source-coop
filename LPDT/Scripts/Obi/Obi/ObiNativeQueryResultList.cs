using System;

namespace Obi
{
	[Serializable]
	public class ObiNativeQueryResultList : ObiNativeList<QueryResult>
	{
		public ObiNativeQueryResultList(int capacity = 8, int alignment = 16)
			: base(capacity, alignment)
		{
			for (int i = 0; i < capacity; i++)
			{
				base[i] = default(QueryResult);
			}
		}
	}
}

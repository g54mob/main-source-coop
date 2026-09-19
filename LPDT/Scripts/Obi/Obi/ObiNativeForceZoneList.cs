using System;

namespace Obi
{
	[Serializable]
	public class ObiNativeForceZoneList : ObiNativeList<ForceZone>
	{
		public ObiNativeForceZoneList()
		{
		}

		public ObiNativeForceZoneList(int capacity = 8, int alignment = 16)
			: base(capacity, alignment)
		{
			for (int i = 0; i < capacity; i++)
			{
				base[i] = default(ForceZone);
			}
		}
	}
}

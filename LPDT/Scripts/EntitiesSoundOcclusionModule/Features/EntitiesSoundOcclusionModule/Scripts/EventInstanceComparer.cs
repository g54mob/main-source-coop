using System.Collections.Generic;
using FMOD.Studio;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	public sealed class EventInstanceComparer : IEqualityComparer<EventInstance>
	{
		public bool Equals(EventInstance x, EventInstance y)
		{
			return x.handle == y.handle;
		}

		public int GetHashCode(EventInstance obj)
		{
			return obj.handle.GetHashCode();
		}
	}
}

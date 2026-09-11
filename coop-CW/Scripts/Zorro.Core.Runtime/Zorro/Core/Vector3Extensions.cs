using UnityEngine;

namespace Zorro.Core
{
	public static class Vector3Extensions
	{
		public static float Get2DDistance(this Vector3 from, Vector3 to)
		{
			Vector2 a = new Vector2(from.x, from.z);
			Vector2 b = new Vector2(to.x, to.z);
			return Vector2.Distance(a, b);
		}
	}
}

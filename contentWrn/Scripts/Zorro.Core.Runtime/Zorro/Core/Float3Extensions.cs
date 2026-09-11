using Unity.Mathematics;

namespace Zorro.Core
{
	public static class Float3Extensions
	{
		public static float Get2DDistance(this float3 from, float3 to)
		{
			return math.distance(from.xz, to.xz);
		}
	}
}

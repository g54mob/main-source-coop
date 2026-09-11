using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Zorro.Core
{
	public struct Bounds2D
	{
		public float2 Center;

		public float2 HalfSize;

		public Bounds2D(float2 center, float2 halfSize)
		{
			Center = center;
			HalfSize = halfSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(float2 point)
		{
			float2 float5 = Center + HalfSize;
			float2 float6 = Center - HalfSize;
			if (point.x > float6.x && point.x < float5.x && point.y > float6.y)
			{
				return point.y < float5.y;
			}
			return false;
		}

		public bool Overlaps(Bounds2D bounds)
		{
			float2 halfSize = HalfSize;
			float2 halfSize2 = bounds.HalfSize;
			float2 float5 = Center + halfSize;
			float2 float6 = Center - halfSize;
			float2 float7 = bounds.Center + halfSize2;
			float2 float8 = bounds.Center - halfSize2;
			if (float5.x > float8.x && float6.x < float7.x && float5.y > float8.y)
			{
				return float6.y < float7.y;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(Box2D bounds)
		{
			float2 halfSize = HalfSize;
			float2 float5 = bounds.HalfHeight;
			float2 float6 = Center + halfSize;
			float2 float7 = Center - halfSize;
			float2 float8 = bounds.Center + float5;
			float2 float9 = bounds.Center - float5;
			if (float6.x > float9.x && float7.x < float8.x && float6.y > float9.y)
			{
				return float7.y < float8.y;
			}
			return false;
		}
	}
}

using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Zorro.Core
{
	public struct Box2D
	{
		public float2 Center;

		public float HalfHeight;

		public Box2D(float2 center, float halfHeight)
		{
			Center = center;
			HalfHeight = halfHeight;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(float2 point)
		{
			float2 float5 = Center + HalfHeight;
			float2 float6 = Center - HalfHeight;
			if (point.x > float6.x && point.x < float5.x && point.y > float6.y)
			{
				return point.y < float5.y;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(Bounds2D bounds)
		{
			float2 float5 = HalfHeight;
			float2 halfSize = bounds.HalfSize;
			float2 float6 = Center + float5;
			float2 float7 = Center - float5;
			float2 float8 = bounds.Center + halfSize;
			float2 float9 = bounds.Center - halfSize;
			if (float6.x > float9.x && float7.x < float8.x && float6.y > float9.y)
			{
				return float7.y < float8.y;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(Box2D bounds)
		{
			float2 float5 = HalfHeight;
			float2 float6 = bounds.HalfHeight;
			float2 float7 = Center + float5;
			float2 float8 = Center - float5;
			float2 float9 = bounds.Center + float6;
			float2 float10 = bounds.Center - float6;
			if (float7.x > float10.x && float8.x < float9.x && float7.y > float10.y)
			{
				return float8.y < float9.y;
			}
			return false;
		}
	}
}

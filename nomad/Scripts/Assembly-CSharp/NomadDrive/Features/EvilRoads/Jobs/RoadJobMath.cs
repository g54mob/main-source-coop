using Unity.Mathematics;

namespace NomadDrive.Features.EvilRoads.Jobs
{
	internal static class RoadJobMath
	{
		public static bool PointInTriangle(float2 p, float2 a, float2 b, float2 c)
		{
			float num = (b.y - c.y) * (a.x - c.x) + (c.x - b.x) * (a.y - c.y);
			if (math.abs(num) < 1E-06f)
			{
				return false;
			}
			float num2 = ((b.y - c.y) * (p.x - c.x) + (c.x - b.x) * (p.y - c.y)) / num;
			float num3 = ((c.y - a.y) * (p.x - c.x) + (a.x - c.x) * (p.y - c.y)) / num;
			float num4 = 1f - num2 - num3;
			if (num2 >= 0f && num3 >= 0f)
			{
				return num4 >= 0f;
			}
			return false;
		}

		public static float InterpolateY(float2 p, float2 a, float2 b, float2 c, float y0, float y1, float y2)
		{
			float num = (b.y - c.y) * (a.x - c.x) + (c.x - b.x) * (a.y - c.y);
			if (math.abs(num) < 1E-06f)
			{
				return y0;
			}
			float num2 = ((b.y - c.y) * (p.x - c.x) + (c.x - b.x) * (p.y - c.y)) / num;
			float num3 = ((c.y - a.y) * (p.x - c.x) + (a.x - c.x) * (p.y - c.y)) / num;
			float num4 = 1f - num2 - num3;
			return num2 * y0 + num3 * y1 + num4 * y2;
		}

		public static float DistanceToTriangle(float2 p, float2 a, float2 b, float2 c)
		{
			float x = DistanceToSegment(p, a, b);
			float x2 = DistanceToSegment(p, b, c);
			float y = DistanceToSegment(p, c, a);
			return math.min(x, math.min(x2, y));
		}

		public static float DistanceToSegment(float2 p, float2 a, float2 b)
		{
			float2 float5 = b - a;
			float2 x = p - a;
			float num = math.lengthsq(float5);
			if (num < 1E-12f)
			{
				return math.distance(p, a);
			}
			float num2 = math.saturate(math.dot(x, float5) / num);
			float2 y = a + num2 * float5;
			return math.distance(p, y);
		}
	}
}

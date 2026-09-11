using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtBounds
	{
		public static bool ContainsXZ(this Bounds me, Vector2 point)
		{
			me.center = me.center.xoz();
			return me.Contains(point.xoy());
		}

		public static Vector2[] GetCorners(this Bounds me)
		{
			return new Vector2[4]
			{
				new Vector2(me.max.x, me.max.z),
				new Vector2(me.min.x, me.min.z),
				new Vector2(me.max.x, me.min.z),
				new Vector2(me.min.x, me.max.z)
			};
		}

		public static (Vector2, Vector2) Find2ClosestCorners(this Bounds me, Vector2 point)
		{
			Vector2[] corners = me.GetCorners();
			Vector2 vector = Vector2.zero;
			Vector2 item = Vector2.zero;
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			Vector2[] array = corners;
			foreach (Vector2 vector2 in array)
			{
				float num3 = Vector2.Distance(vector2, point);
				if (num3 < num)
				{
					num2 = num;
					num = num3;
					item = vector;
					vector = vector2;
				}
				else if (num3 < num2)
				{
					num2 = num3;
					item = vector2;
				}
			}
			return (vector, item);
		}

		public static Vector2 GetClosestPointOnEdgeOfBounds(this Bounds me, Vector2 point)
		{
			var (origin, end) = me.Find2ClosestCorners(point);
			return ExtMath.NearestPointOnLineSegment(origin, end, point);
		}
	}
}

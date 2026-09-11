using System.Runtime.CompilerServices;
using UnityEngine;

namespace Zorro.Core
{
	public static class Math2DUtility
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistancePointToLineSegment(Vector2 p, Vector2 a, Vector2 b)
		{
			return 0f;
		}

		public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
		{
			float num = 1E-05f;
			bool result = false;
			float num2 = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);
			if (num2 != 0f)
			{
				float num3 = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) / num2;
				float num4 = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) / num2;
				if (shouldIncludeEndPoints)
				{
					if (num3 >= 0f + num && num3 <= 1f - num && num4 >= 0f + num && num4 <= 1f - num)
					{
						result = true;
					}
				}
				else if (num3 > 0f + num && num3 < 1f - num && num4 > 0f + num && num4 < 1f - num)
				{
					result = true;
				}
			}
			return result;
		}
	}
}

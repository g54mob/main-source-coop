using UnityEngine;

namespace Rewired.UI
{
	public struct UIPivot
	{
		public float min;

		public float max;

		public static UIPivot TopLeft => new UIPivot(0f, 1f);

		public static UIPivot TopCenter => new UIPivot(0.5f, 1f);

		public static UIPivot TopRight => new UIPivot(0.1f, 1f);

		public static UIPivot MiddleLeft => new UIPivot(0f, 0.5f);

		public static UIPivot MiddleCenter => new UIPivot(0.5f, 0.5f);

		public static UIPivot MiddleRight => new UIPivot(0.1f, 0.5f);

		public static UIPivot BottomLeft => new UIPivot(0f, 0f);

		public static UIPivot BottomCenter => new UIPivot(0.5f, 0f);

		public static UIPivot BottomRight => new UIPivot(1f, 0f);

		public UIPivot(float P_0, float P_1)
		{
			if (P_0 < 0f)
			{
				P_0 = 0f;
			}
			if (P_1 < 0f)
			{
				P_1 = 0f;
			}
			min = P_0;
			max = P_1;
		}

		public static implicit operator Vector2(UIPivot x)
		{
			return new Vector2(x.min, x.max);
		}

		public static implicit operator UIPivot(Vector2 x)
		{
			return new UIPivot(x.x, x.y);
		}
	}
}

using UnityEngine;

namespace Rewired.UI
{
	public struct UIAnchor
	{
		public Vector2 min;

		public Vector2 max;

		public static UIAnchor TopLeft => new UIAnchor(0f, 1f, 0f, 1f);

		public static UIAnchor TopCenter => new UIAnchor(0.5f, 1f, 0.5f, 1f);

		public static UIAnchor TopRight => new UIAnchor(1f, 1f, 1f, 1f);

		public static UIAnchor MiddleLeft => new UIAnchor(0f, 0.5f, 0f, 0.5f);

		public static UIAnchor MiddleCenter => new UIAnchor(0.5f, 0.5f, 0.5f, 0.5f);

		public static UIAnchor MiddleRight => new UIAnchor(1f, 0.5f, 1f, 0.5f);

		public static UIAnchor BottomLeft => new UIAnchor(0f, 0f, 0f, 0f);

		public static UIAnchor BottomCenter => new UIAnchor(0.5f, 0f, 0.5f, 0f);

		public static UIAnchor BottomRight => new UIAnchor(1f, 0f, 1f, 0f);

		public static UIAnchor TopHStretch => new UIAnchor(0f, 1f, 1f, 1f);

		public static UIAnchor MiddleHStretch => new UIAnchor(0f, 0.5f, 1f, 0.5f);

		public static UIAnchor BottomHStretch => new UIAnchor(0f, 0f, 1f, 0f);

		public static UIAnchor LeftVStretch => new UIAnchor(0f, 0f, 0f, 1f);

		public static UIAnchor CenterVStretch => new UIAnchor(0.5f, 0f, 0.5f, 1f);

		public static UIAnchor RightVStretch => new UIAnchor(1f, 0f, 1f, 1f);

		public static UIAnchor Stretch => new UIAnchor(0f, 0f, 1f, 1f);

		public UIAnchor(float P_0, float P_1, float P_2, float P_3)
		{
			if (P_0 < 0f)
			{
				P_0 = 0f;
			}
			if (P_1 < 0f)
			{
				P_1 = 0f;
			}
			if (P_2 < 0f)
			{
				P_2 = 0f;
			}
			if (P_3 < 0f)
			{
				P_3 = 0f;
			}
			min = new Vector2(P_0, P_1);
			max = new Vector2(P_2, P_3);
		}

		public UIAnchor(Vector2 P_0, Vector2 P_1)
		{
			if (P_0.x < 0f)
			{
				P_0.x = 0f;
			}
			if (P_0.y < 0f)
			{
				P_0.y = 0f;
			}
			if (P_1.x < 0f)
			{
				P_1.x = 0f;
			}
			if (P_1.y < 0f)
			{
				P_1.y = 0f;
			}
			min = P_0;
			max = P_1;
		}
	}
}

using UnityEngine;

namespace EvilCore.Extensions
{
	public static class RectTransformExtensions
	{
		public static void SetSizeDeltaX(this RectTransform rectTransform, float x)
		{
			rectTransform.sizeDelta = rectTransform.sizeDelta.WithX(x);
		}

		public static void SetSizeDeltaY(this RectTransform rectTransform, float y)
		{
			rectTransform.sizeDelta = rectTransform.sizeDelta.WithY(y);
		}

		public static void SetAnchorMinX(this RectTransform rectTransform, float x)
		{
			rectTransform.anchorMin = rectTransform.anchorMin.WithX(x);
		}

		public static void SetAnchorMinY(this RectTransform rectTransform, float y)
		{
			rectTransform.anchorMin = rectTransform.anchorMin.WithY(y);
		}

		public static void SetAnchorMaxX(this RectTransform rectTransform, float x)
		{
			rectTransform.anchorMax = rectTransform.anchorMax.WithX(x);
		}

		public static void SetAnchorMaxY(this RectTransform rectTransform, float y)
		{
			rectTransform.anchorMax = rectTransform.anchorMax.WithY(y);
		}

		public static void SetOffsetMinX(this RectTransform rectTransform, float x)
		{
			rectTransform.offsetMin = rectTransform.offsetMin.WithX(x);
		}

		public static void SetLeft(this RectTransform rectTransform, float x)
		{
			rectTransform.SetOffsetMinX(x);
		}

		public static void SetOffsetMinY(this RectTransform rectTransform, float y)
		{
			rectTransform.offsetMin = rectTransform.offsetMin.WithY(y);
		}

		public static void SetBottom(this RectTransform rectTransform, float y)
		{
			rectTransform.SetOffsetMinY(y);
		}

		public static void SetOffsetMaxX(this RectTransform rectTransform, float x)
		{
			rectTransform.offsetMax = rectTransform.offsetMax.WithX(x);
		}

		public static void SetRight(this RectTransform rectTransform, float x)
		{
			rectTransform.SetOffsetMaxX(x);
		}

		public static void SetOffsetMaxY(this RectTransform rectTransform, float y)
		{
			rectTransform.offsetMax = rectTransform.offsetMax.WithY(y);
		}

		public static void SetTop(this RectTransform rectTransform, float y)
		{
			rectTransform.SetOffsetMaxY(y);
		}

		public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
		{
			rectTransform.anchoredPosition = rectTransform.anchoredPosition.WithX(x);
		}

		public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
		{
			rectTransform.anchoredPosition = rectTransform.anchoredPosition.WithY(y);
		}

		public static void SetAnchoredPosition3Dx(this RectTransform rectTransform, float x)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithX(x);
		}

		public static void SetAnchoredPosition3Dy(this RectTransform rectTransform, float y)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithY(y);
		}

		public static void SetAnchoredPosition3Dz(this RectTransform rectTransform, float z)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithZ(z);
		}

		public static void SetAnchoredPosition3Dxy(this RectTransform rectTransform, float x, float y)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithXY(x, y);
		}

		public static void SetAnchoredPosition3Dxy(this RectTransform rectTransform, Vector2 position)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithXY(position.x, position.y);
		}

		public static void SetAnchoredPosition3Dxz(this RectTransform rectTransform, float x, float z)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithXZ(x, z);
		}

		public static void SetAnchoredPosition3Dxz(this RectTransform rectTransform, Vector2 position)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithXZ(position.x, position.y);
		}

		public static void SetAnchoredPosition3Dyz(this RectTransform rectTransform, float y, float z)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithYZ(y, z);
		}

		public static void SetAnchoredPosition3Dyz(this RectTransform rectTransform, Vector2 position)
		{
			rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D.WithYZ(position.x, position.y);
		}

		public static void SetPivotX(this RectTransform rectTransform, float x)
		{
			rectTransform.pivot = rectTransform.pivot.WithX(x);
		}

		public static void SetPivotY(this RectTransform rectTransform, float y)
		{
			rectTransform.pivot = rectTransform.pivot.WithY(y);
		}

		public static void SetPivotOnlyX(this RectTransform rectTransform, float x)
		{
			float num = rectTransform.pivot.x - x;
			rectTransform.SetPivotX(x);
			rectTransform.SetAnchoredPositionX(rectTransform.anchoredPosition.x - rectTransform.sizeDelta.x * num);
		}

		public static void SetPivotOnlyY(this RectTransform rectTransform, float y)
		{
			float num = rectTransform.pivot.y - y;
			rectTransform.SetPivotY(y);
			rectTransform.SetAnchoredPositionY(rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y * num);
		}

		public static void SetPivotOnly(this RectTransform rectTransform, Vector2 pivot)
		{
			rectTransform.SetPivotOnly(pivot.x, pivot.y);
		}

		public static void SetPivotOnly(this RectTransform rectTransform, float x, float y)
		{
			rectTransform.SetPivotOnlyX(x);
			rectTransform.SetPivotOnlyY(y);
		}
	}
}

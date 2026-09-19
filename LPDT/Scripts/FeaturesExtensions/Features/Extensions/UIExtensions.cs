using UnityEngine;
using UnityEngine.UI;

namespace Features.Extensions
{
	public static class UIExtensions
	{
		public static void SetAlpha(this Image image, float alpha)
		{
			Color color = image.color;
			color.a = alpha;
			image.color = color;
		}

		public static void SetColor(this Image image, Color color)
		{
			image.color = color;
		}

		public static void SetColorKeepAlpha(this Image image, Color color)
		{
			color.a = image.color.a;
			image.color = color;
		}

		public static void SetFillAmount(this Image image, float fill)
		{
			image.fillAmount = Mathf.Clamp01(fill);
		}

		public static void SetAlpha(this CanvasGroup group, float alpha)
		{
			group.alpha = Mathf.Clamp01(alpha);
		}

		public static void SetInteractable(this CanvasGroup group, bool interactable)
		{
			group.interactable = interactable;
			group.blocksRaycasts = interactable;
		}

		public static void Show(this CanvasGroup group)
		{
			group.alpha = 1f;
			group.interactable = true;
			group.blocksRaycasts = true;
		}

		public static void Hide(this CanvasGroup group)
		{
			group.alpha = 0f;
			group.interactable = false;
			group.blocksRaycasts = false;
		}

		public static void SetAlpha(this Graphic graphic, float alpha)
		{
			Color color = graphic.color;
			color.a = Mathf.Clamp01(alpha);
			graphic.color = color;
		}

		public static void SetAlpha(this Text text, float alpha)
		{
			((Graphic)text).SetAlpha(alpha);
		}

		public static void SetText(this Text text, string value)
		{
			text.text = value;
		}

		public static void SetText(this Text text, int value)
		{
			text.text = value.ToString();
		}

		public static void SetText(this Text text, float value, string format = "F2")
		{
			text.text = value.ToString(format);
		}

		public static void SetInteractable(this Button button, bool interactable)
		{
			button.interactable = interactable;
		}

		public static void SetAnchoredPositionX(this RectTransform rect, float x)
		{
			Vector2 anchoredPosition = rect.anchoredPosition;
			anchoredPosition.x = x;
			rect.anchoredPosition = anchoredPosition;
		}

		public static void SetAnchoredPositionY(this RectTransform rect, float y)
		{
			Vector2 anchoredPosition = rect.anchoredPosition;
			anchoredPosition.y = y;
			rect.anchoredPosition = anchoredPosition;
		}

		public static void SetSizeDeltaX(this RectTransform rect, float width)
		{
			Vector2 sizeDelta = rect.sizeDelta;
			sizeDelta.x = width;
			rect.sizeDelta = sizeDelta;
		}

		public static void SetSizeDeltaY(this RectTransform rect, float height)
		{
			Vector2 sizeDelta = rect.sizeDelta;
			sizeDelta.y = height;
			rect.sizeDelta = sizeDelta;
		}

		public static void ResetAnchors(this RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}
	}
}

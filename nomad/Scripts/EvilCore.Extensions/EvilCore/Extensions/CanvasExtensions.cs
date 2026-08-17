using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.Extensions
{
	public static class CanvasExtensions
	{
		public static float GetScaleFactor(this CanvasScaler scaler)
		{
			return Mathf.Lerp((float)Screen.width / scaler.referenceResolution.x, (float)Screen.height / scaler.referenceResolution.y, scaler.matchWidthOrHeight);
		}

		public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
		{
			Vector3 viewportPosition = new Vector3(screenPosition.x / (float)Screen.width, screenPosition.y / (float)Screen.height, 0f);
			return canvas.ViewportToCanvasPosition(viewportPosition);
		}

		public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
		{
			Vector3 a = viewportPosition - new Vector3(0.5f, 0.5f, 0f);
			Vector2 sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
			return Vector3.Scale(a, sizeDelta);
		}

		public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera = null, bool useNormalizeViewPort = false)
		{
			if (camera == null)
			{
				camera = Camera.main;
			}
			Vector3 viewportPosition = camera.WorldToViewportPoint(worldPosition);
			if (useNormalizeViewPort)
			{
				Rect rect = camera.rect;
				viewportPosition.x = viewportPosition.x * rect.width + rect.x;
				viewportPosition.y = viewportPosition.y * rect.height + rect.y;
			}
			return canvas.ViewportToCanvasPosition(viewportPosition);
		}
	}
}

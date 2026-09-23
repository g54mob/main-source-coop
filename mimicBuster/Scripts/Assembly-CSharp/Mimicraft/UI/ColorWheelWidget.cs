using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ColorWheelWidget : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
	{
		private const int TextureSize = 128;

		private const float HandleSize = 14f;

		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private RectTransform selectorHandle;

		[SerializeField]
		private RawImage wheelImage;

		private float hue;

		private float saturation;

		private float value = 1f;

		public event Action<Color> ColorChanged;

		public event Action ColorCommitted;

		public static ColorWheelWidget Create(Transform parent, float size)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "ColorWheelWidget");
			rectTransform.sizeDelta = new Vector2(size, size);
			RectTransform obj = UIFactory.CreateRect(rectTransform, "Wheel");
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.offsetMin = Vector2.zero;
			obj.offsetMax = Vector2.zero;
			RawImage rawImage = obj.gameObject.AddComponent<RawImage>();
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Handle");
			rectTransform2.sizeDelta = new Vector2(14f, 14f);
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = Color.white;
			image.raycastTarget = false;
			ColorWheelWidget colorWheelWidget = rectTransform.gameObject.AddComponent<ColorWheelWidget>();
			colorWheelWidget.rectTransform = rectTransform;
			colorWheelWidget.selectorHandle = rectTransform2;
			colorWheelWidget.wheelImage = rawImage;
			return colorWheelWidget;
		}

		private void Awake()
		{
			wheelImage.texture = BuildWheelTexture(128);
			PositionHandle();
		}

		public void SetColor(Color color)
		{
			Color.RGBToHSV(color, out hue, out saturation, out value);
			PositionHandle();
		}

		public void SetValue(float v)
		{
			value = Mathf.Clamp01(v);
			this.ColorChanged?.Invoke(Color.HSVToRGB(hue, saturation, value));
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			HandlePointer(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			HandlePointer(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this.ColorCommitted?.Invoke();
		}

		private void HandlePointer(PointerEventData eventData)
		{
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
			{
				float num = rectTransform.rect.width * 0.5f;
				float num2 = Mathf.Clamp01(localPoint.magnitude / num);
				float num3 = Mathf.Atan2(localPoint.y, localPoint.x);
				hue = Mathf.Repeat(num3 / (MathF.PI * 2f) + 1f, 1f);
				saturation = num2;
				PositionHandle();
				this.ColorChanged?.Invoke(Color.HSVToRGB(hue, saturation, value));
			}
		}

		private void PositionHandle()
		{
			float num = rectTransform.rect.width * 0.5f * saturation;
			float f = hue * MathF.PI * 2f;
			selectorHandle.anchoredPosition = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * num;
		}

		private static Texture2D BuildWheelTexture(int size)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, mipChain: false)
			{
				wrapMode = TextureWrapMode.Clamp
			};
			Vector2 vector = new Vector2(size - 1, size - 1) * 0.5f;
			float num = (float)size * 0.5f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					Vector2 vector2 = new Vector2(j, i) - vector;
					float num2 = vector2.magnitude / num;
					if (num2 > 1f)
					{
						texture2D.SetPixel(j, i, Color.clear);
						continue;
					}
					float h = Mathf.Repeat(Mathf.Atan2(vector2.y, vector2.x) / (MathF.PI * 2f) + 1f, 1f);
					texture2D.SetPixel(j, i, Color.HSVToRGB(h, num2, 1f));
				}
			}
			texture2D.Apply();
			return texture2D;
		}
	}
}

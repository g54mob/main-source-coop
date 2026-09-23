using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public static class UIFactory
	{
		public static RectTransform CreateRect(Transform parent, string name)
		{
			GameObject gameObject = new GameObject(name, typeof(RectTransform));
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			return (RectTransform)gameObject.transform;
		}

		public static Image CreatePanel(Transform parent, string name, Color color)
		{
			Image image = CreateRect(parent, name).gameObject.AddComponent<Image>();
			image.color = color;
			return image;
		}

		public static TextMeshProUGUI CreateLabel(Transform parent, string name, string text, int fontSize = 14)
		{
			TextMeshProUGUI textMeshProUGUI = CreateRect(parent, name).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = fontSize;
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			textMeshProUGUI.color = Color.white;
			textMeshProUGUI.raycastTarget = false;
			return textMeshProUGUI;
		}

		public static Button CreateButton(Transform parent, string name, string label, out TextMeshProUGUI text)
		{
			Image image = CreatePanel(parent, name, new Color(0.18f, 0.18f, 0.18f, 0.9f));
			Button result = image.gameObject.AddComponent<Button>();
			text = CreateLabel(image.transform, "Label", label);
			RectTransform obj = (RectTransform)text.transform;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.offsetMin = Vector2.zero;
			obj.offsetMax = Vector2.zero;
			return result;
		}

		public static Button CreateIconButton(Transform parent, string name, Sprite iconSprite, Vector2 iconSize, out Image iconImage)
		{
			Image image = CreatePanel(parent, name, new Color(0.18f, 0.18f, 0.18f, 0.9f));
			Button result = image.gameObject.AddComponent<Button>();
			RectTransform rectTransform = CreateRect(image.transform, "Icon");
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = iconSize;
			iconImage = rectTransform.gameObject.AddComponent<Image>();
			iconImage.sprite = iconSprite;
			iconImage.preserveAspect = true;
			iconImage.raycastTarget = false;
			return result;
		}

		public static TMP_InputField CreateInputField(Transform parent, string name, string placeholder)
		{
			Image image = CreatePanel(parent, name, new Color(0.12f, 0.12f, 0.12f, 0.95f));
			TMP_InputField tMP_InputField = image.gameObject.AddComponent<TMP_InputField>();
			RectTransform rectTransform = CreateRect(image.transform, "Text Area");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(8f, 2f);
			rectTransform.offsetMax = new Vector2(-8f, -2f);
			rectTransform.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform2 = CreateRect(rectTransform, "Placeholder");
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = Vector2.zero;
			rectTransform2.offsetMax = Vector2.zero;
			TextMeshProUGUI textMeshProUGUI = rectTransform2.gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = placeholder;
			textMeshProUGUI.fontSize = 14f;
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.color = new Color(1f, 1f, 1f, 0.4f);
			textMeshProUGUI.fontStyle = FontStyles.Italic;
			textMeshProUGUI.raycastTarget = false;
			RectTransform rectTransform3 = CreateRect(rectTransform, "Text");
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.offsetMin = Vector2.zero;
			rectTransform3.offsetMax = Vector2.zero;
			TextMeshProUGUI textMeshProUGUI2 = rectTransform3.gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI2.fontSize = 14f;
			textMeshProUGUI2.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI2.color = Color.white;
			textMeshProUGUI2.raycastTarget = false;
			tMP_InputField.textViewport = rectTransform;
			tMP_InputField.textComponent = textMeshProUGUI2;
			tMP_InputField.placeholder = textMeshProUGUI;
			tMP_InputField.targetGraphic = image;
			return tMP_InputField;
		}

		public static Slider CreateSlider(Transform parent, string name, float min, float max, float value)
		{
			RectTransform rectTransform = CreateRect(parent, name);
			RectTransform obj = (RectTransform)CreatePanel(rectTransform, "Background", new Color(0.15f, 0.15f, 0.15f, 0.9f)).transform;
			obj.anchorMin = new Vector2(0f, 0.25f);
			obj.anchorMax = new Vector2(1f, 0.75f);
			obj.offsetMin = Vector2.zero;
			obj.offsetMax = Vector2.zero;
			RectTransform rectTransform2 = CreateRect(rectTransform, "Fill Area");
			rectTransform2.anchorMin = new Vector2(0f, 0.25f);
			rectTransform2.anchorMax = new Vector2(1f, 0.75f);
			rectTransform2.offsetMin = new Vector2(5f, 0f);
			rectTransform2.offsetMax = new Vector2(-5f, 0f);
			RectTransform rectTransform3 = (RectTransform)CreatePanel(rectTransform2, "Fill", new Color(0.85f, 0.85f, 0.85f, 1f)).transform;
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = new Vector2(0f, 1f);
			rectTransform3.offsetMin = Vector2.zero;
			rectTransform3.offsetMax = Vector2.zero;
			RectTransform rectTransform4 = CreateRect(rectTransform, "Handle Slide Area");
			rectTransform4.anchorMin = Vector2.zero;
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.offsetMin = Vector2.zero;
			rectTransform4.offsetMax = Vector2.zero;
			Image image = CreatePanel(rectTransform4, "Handle", Color.white);
			RectTransform rectTransform5 = (RectTransform)image.transform;
			rectTransform5.sizeDelta = new Vector2(14f, 0f);
			Slider slider = rectTransform.gameObject.AddComponent<Slider>();
			slider.fillRect = rectTransform3;
			slider.handleRect = rectTransform5;
			slider.targetGraphic = image;
			slider.direction = Slider.Direction.LeftToRight;
			slider.minValue = min;
			slider.maxValue = max;
			slider.value = value;
			return slider;
		}
	}
}

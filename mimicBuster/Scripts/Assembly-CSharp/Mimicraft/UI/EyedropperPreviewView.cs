using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class EyedropperPreviewView : MonoBehaviour
	{
		private const int CircleDiameter = 40;

		private const float RingThickness = 2f;

		private static readonly Vector2 CursorOffset = new Vector2(49f, 49f);

		private RectTransform root;

		private Image oldColorHalf;

		private Image newColorHalf;

		private static readonly Color32 WhiteOpaque = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static readonly Color32 Clear = new Color32(0, 0, 0, 0);

		public static EyedropperPreviewView Create(Transform canvasParent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(canvasParent, "EyedropperPreview");
			rectTransform.sizeDelta = new Vector2(40f, 40f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			Sprite sprite = BuildHalfCircleSprite(40, left: true);
			Sprite sprite2 = BuildHalfCircleSprite(40, left: false);
			Sprite sprite3 = BuildRingSprite(40, 2f);
			Image image = CreateFullRectImage(rectTransform, "OldColorHalf", sprite);
			Image image2 = CreateFullRectImage(rectTransform, "NewColorHalf", sprite2);
			CreateFullRectImage(rectTransform, "Ring", sprite3).color = new Color(1f, 1f, 1f, 0.9f);
			EyedropperPreviewView eyedropperPreviewView = rectTransform.gameObject.AddComponent<EyedropperPreviewView>();
			eyedropperPreviewView.root = rectTransform;
			eyedropperPreviewView.oldColorHalf = image;
			eyedropperPreviewView.newColorHalf = image2;
			rectTransform.gameObject.SetActive(value: false);
			return eyedropperPreviewView;
		}

		private static Image CreateFullRectImage(Transform parent, string name, Sprite sprite)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, name);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.sprite = sprite;
			image.raycastTarget = false;
			return image;
		}

		public void Show(Color oldColor, Color newColor, Vector2 screenPosition)
		{
			base.gameObject.SetActive(value: true);
			oldColorHalf.color = oldColor;
			newColorHalf.color = newColor;
			root.position = new Vector3(screenPosition.x + CursorOffset.x, screenPosition.y + CursorOffset.y, 0f);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		private static Sprite BuildHalfCircleSprite(int size, bool left)
		{
			Color32[] array = new Color32[size * size];
			float num = (float)size * 0.5f - 1f;
			Vector2 b = new Vector2(size - 1, size - 1) * 0.5f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					bool flag = (left ? ((float)j < b.x) : ((float)j >= b.x));
					bool flag2 = Vector2.Distance(new Vector2(j, i), b) <= num;
					array[i * size + j] = ((flag && flag2) ? WhiteOpaque : Clear);
				}
			}
			return ToSprite(size, array);
		}

		private static Sprite BuildRingSprite(int size, float thickness)
		{
			Color32[] array = new Color32[size * size];
			float num = (float)size * 0.5f - 1f;
			Vector2 b = new Vector2(size - 1, size - 1) * 0.5f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num2 = Vector2.Distance(new Vector2(j, i), b);
					array[i * size + j] = ((Mathf.Abs(num2 - num) <= thickness * 0.5f) ? WhiteOpaque : Clear);
				}
			}
			return ToSprite(size, array);
		}

		private static Sprite ToSprite(int size, Color32[] pixels)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, mipChain: false);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.SetPixels32(pixels);
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f));
		}
	}
}

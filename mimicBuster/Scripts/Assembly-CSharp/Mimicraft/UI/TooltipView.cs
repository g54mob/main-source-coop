using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class TooltipView : MonoBehaviour
	{
		private const float VerticalOffset = 36f;

		private const float Margin = 8f;

		private const float MaxWidth = 420f;

		private const float FallbackPaddingX = 24f;

		private const float FallbackPaddingY = 16f;

		private const float Unbounded = 32767f;

		[SerializeField]
		private RectTransform panelRect;

		[SerializeField]
		private TextMeshProUGUI label;

		private RectTransform owner;

		private Vector2 padding;

		private bool layoutOwned;

		public static TooltipView Instance { get; private set; }

		public static TooltipView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "Tooltip");
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(0f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.sizeDelta = new Vector2(220f, 28f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.05f, 0.05f, 0.05f, 0.92f);
			image.raycastTarget = false;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "TooltipLabel", "", 13);
			textMeshProUGUI.margin = new Vector4(8f, 2f, 8f, 2f);
			TooltipView tooltipView = rectTransform.gameObject.AddComponent<TooltipView>();
			tooltipView.panelRect = rectTransform;
			tooltipView.label = textMeshProUGUI;
			return tooltipView;
		}

		private void Awake()
		{
			Instance = this;
			base.gameObject.SetActive(value: false);
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Show(string text, RectTransform anchor)
		{
			if (!string.IsNullOrEmpty(text) && !(anchor == null))
			{
				owner = anchor;
				TakeOverLayout();
				label.text = text;
				base.gameObject.SetActive(value: true);
				Resize(text);
				Place(anchor);
				base.gameObject.transform.DOKill();
				base.gameObject.transform.localScale = Vector3.zero;
				base.gameObject.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
			}
		}

		private void TakeOverLayout()
		{
			if (!layoutOwned && !(panelRect == null) && !(label == null))
			{
				layoutOwned = true;
				Vector4 margin = label.margin;
				padding = new Vector2(margin.x + margin.z, margin.y + margin.w);
				if (padding.x <= 0f)
				{
					padding.x = 24f;
				}
				if (padding.y <= 0f)
				{
					padding.y = 16f;
				}
				label.margin = Vector4.zero;
				label.textWrappingMode = TextWrappingModes.Normal;
				RectTransform obj = (RectTransform)label.transform;
				obj.anchorMin = Vector2.zero;
				obj.anchorMax = Vector2.one;
				obj.pivot = new Vector2(0.5f, 0.5f);
				obj.offsetMin = new Vector2(padding.x * 0.5f, padding.y * 0.5f);
				obj.offsetMax = new Vector2((0f - padding.x) * 0.5f, (0f - padding.y) * 0.5f);
				Silence<ContentSizeFitter>(panelRect);
				Silence<LayoutGroup>(panelRect);
				Silence<ContentSizeFitter>(obj);
				Silence<LayoutElement>(obj);
			}
		}

		private static void Silence<T>(Component target) where T : Behaviour
		{
			T component = target.GetComponent<T>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		private void Resize(string text)
		{
			Rect rect = Area();
			float b = Mathf.Max(1f, rect.width - 16f);
			float b2 = Mathf.Max(1f, rect.height - 16f);
			float num = Mathf.Max(1f, Mathf.Min(420f, b) - padding.x);
			Vector2 preferredValues = label.GetPreferredValues(text, num, 32767f);
			panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Min(Mathf.Min(preferredValues.x, num) + padding.x, b));
			panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Min(preferredValues.y + padding.y, b2));
		}

		private Rect Area()
		{
			RectTransform rectTransform = panelRect.parent as RectTransform;
			if (rectTransform == null)
			{
				return new Rect(0f, 0f, Screen.width, Screen.height);
			}
			Canvas componentInParent = panelRect.GetComponentInParent<Canvas>();
			RectTransform rectTransform2 = ((componentInParent != null) ? (componentInParent.rootCanvas.transform as RectTransform) : null);
			if (rectTransform2 == null || rectTransform2 == rectTransform)
			{
				return rectTransform.rect;
			}
			Rect rect = rectTransform2.rect;
			Vector3 vector = rectTransform.InverseTransformPoint(rectTransform2.TransformPoint(new Vector3(rect.xMin, rect.yMin, 0f)));
			Vector3 vector2 = rectTransform.InverseTransformPoint(rectTransform2.TransformPoint(new Vector3(rect.xMax, rect.yMax, 0f)));
			return Rect.MinMaxRect(Mathf.Min(vector.x, vector2.x), Mathf.Min(vector.y, vector2.y), Mathf.Max(vector.x, vector2.x), Mathf.Max(vector.y, vector2.y));
		}

		private void Place(RectTransform anchor)
		{
			RectTransform rectTransform = panelRect.parent as RectTransform;
			if (rectTransform == null)
			{
				panelRect.position = anchor.position + new Vector3(0f, 36f, 0f);
				return;
			}
			Vector3 vector = rectTransform.InverseTransformPoint(anchor.position);
			Vector2 size = panelRect.rect.size;
			Vector2 pivot = panelRect.pivot;
			Rect rect = Area();
			float min = rect.xMin + 8f + pivot.x * size.x;
			float max = rect.xMax - 8f - (1f - pivot.x) * size.x;
			float num = rect.yMin + 8f + pivot.y * size.y;
			float num2 = rect.yMax - 8f - (1f - pivot.y) * size.y;
			float num3 = vector.y + 36f + pivot.y * size.y;
			float num4 = vector.y - 36f - (1f - pivot.y) * size.y;
			float value = ((num3 > num2 && num4 >= num) ? num4 : num3);
			panelRect.localPosition = new Vector3(Fit(vector.x, min, max), Fit(value, num, num2), 0f);
		}

		private static float Fit(float value, float min, float max)
		{
			if (!(min > max))
			{
				return Mathf.Clamp(value, min, max);
			}
			return (min + max) * 0.5f;
		}

		public void Hide()
		{
			owner = null;
			base.gameObject.transform.DOKill();
			base.gameObject.SetActive(value: false);
		}

		public void HideOwnedBy(RectTransform anchor)
		{
			if (anchor != null && owner == anchor)
			{
				Hide();
			}
		}

		public void RefreshOwnedBy(RectTransform anchor, string text)
		{
			if (!(anchor == null) && !(owner != anchor) && !string.IsNullOrEmpty(text) && base.gameObject.activeSelf)
			{
				label.text = text;
				Resize(text);
				Place(anchor);
			}
		}
	}
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ToastView : MonoBehaviour
	{
		private const float DefaultDuration = 2.5f;

		[SerializeField]
		private RectTransform panelRect;

		[SerializeField]
		private TextMeshProUGUI label;

		private float hideAtTime = -1f;

		public static ToastView Instance { get; private set; }

		public static ToastView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "Toast");
			rectTransform.anchorMin = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -64f);
			rectTransform.sizeDelta = new Vector2(380f, 40f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.35f, 0.05f, 0.05f, 0.92f);
			image.raycastTarget = false;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "ToastLabel", "", 15);
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			RectTransform obj = (RectTransform)textMeshProUGUI.transform;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.offsetMin = new Vector2(12f, 4f);
			obj.offsetMax = new Vector2(-12f, -4f);
			ToastView toastView = rectTransform.gameObject.AddComponent<ToastView>();
			toastView.panelRect = rectTransform;
			toastView.label = textMeshProUGUI;
			return toastView;
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

		public void Show(string message, float duration = 2.5f)
		{
			if (!string.IsNullOrEmpty(message))
			{
				label.text = message;
				hideAtTime = Time.unscaledTime + duration;
				base.gameObject.SetActive(value: true);
				LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
			}
		}

		private void Update()
		{
			if (hideAtTime >= 0f && Time.unscaledTime >= hideAtTime)
			{
				hideAtTime = -1f;
				base.gameObject.SetActive(value: false);
			}
		}
	}
}

using TMPro;
using UnityEngine;

namespace Mimicraft.UI
{
	public class InteractPromptView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI label;

		public static InteractPromptView Instance { get; private set; }

		public static InteractPromptView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "InteractPrompt");
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(0f, -70f);
			rectTransform.sizeDelta = new Vector2(560f, 34f);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Label", "", 20);
			RectTransform obj = (RectTransform)textMeshProUGUI.transform;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.offsetMin = Vector2.zero;
			obj.offsetMax = Vector2.zero;
			textMeshProUGUI.color = new Color(0.95f, 0.95f, 0.9f);
			InteractPromptView interactPromptView = rectTransform.gameObject.AddComponent<InteractPromptView>();
			interactPromptView.label = textMeshProUGUI;
			return interactPromptView;
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

		public void Show(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				Hide();
				return;
			}
			if (label != null && label.text != text)
			{
				label.text = text;
			}
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
		}

		public void Hide()
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}
}

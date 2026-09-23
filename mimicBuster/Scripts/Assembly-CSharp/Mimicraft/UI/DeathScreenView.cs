using System;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class DeathScreenView : MonoBehaviour
	{
		private const float AutoDismissSeconds = 3f;

		private const float FadeInSpeed = 3f;

		private const float BackdropAlpha = 0.72f;

		[SerializeField]
		private Image backdrop;

		[SerializeField]
		private TextMeshProUGUI titleLabel;

		[SerializeField]
		private TextMeshProUGUI subtitleLabel;

		[SerializeField]
		private Button watchButton;

		private Action onDismissed;

		private float dismissAtTime;

		public static DeathScreenView Instance { get; private set; }

		public static DeathScreenView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "DeathScreen");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.05f, 0f, 0f, 0f);
			image.raycastTarget = true;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Title", "", 48);
			LocalizedText.Attach(textMeshProUGUI, "Overlay.Eliminated");
			RectTransform obj = (RectTransform)textMeshProUGUI.transform;
			obj.anchorMin = new Vector2(0.5f, 0.5f);
			obj.anchorMax = new Vector2(0.5f, 0.5f);
			obj.anchoredPosition = new Vector2(0f, 60f);
			obj.sizeDelta = new Vector2(600f, 70f);
			textMeshProUGUI.color = new Color(0.95f, 0.3f, 0.28f);
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform, "Subtitle", "", 20);
			RectTransform obj2 = (RectTransform)textMeshProUGUI2.transform;
			obj2.anchorMin = new Vector2(0.5f, 0.5f);
			obj2.anchorMax = new Vector2(0.5f, 0.5f);
			obj2.anchoredPosition = new Vector2(0f, 10f);
			obj2.sizeDelta = new Vector2(600f, 30f);
			textMeshProUGUI2.color = new Color(0.85f, 0.85f, 0.85f);
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform, "WatchButton", "İzlemeye Geç", out text);
			RectTransform obj3 = (RectTransform)button.transform;
			obj3.anchorMin = new Vector2(0.5f, 0.5f);
			obj3.anchorMax = new Vector2(0.5f, 0.5f);
			obj3.anchoredPosition = new Vector2(0f, -50f);
			obj3.sizeDelta = new Vector2(200f, 40f);
			DeathScreenView deathScreenView = rectTransform.gameObject.AddComponent<DeathScreenView>();
			deathScreenView.backdrop = image;
			deathScreenView.titleLabel = textMeshProUGUI;
			deathScreenView.subtitleLabel = textMeshProUGUI2;
			deathScreenView.watchButton = button;
			return deathScreenView;
		}

		private void Awake()
		{
			Instance = this;
			watchButton.onClick.AddListener(Dismiss);
			base.gameObject.SetActive(value: false);
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Show(string subtitle, Action onDismissed)
		{
			this.onDismissed = onDismissed;
			subtitleLabel.text = subtitle;
			Color color = backdrop.color;
			color.a = 0f;
			backdrop.color = color;
			dismissAtTime = Time.unscaledTime + 3f;
			base.gameObject.SetActive(value: true);
			GameMenuState.SetMenuOpen(this, open: true);
		}

		public void Cancel()
		{
			onDismissed = null;
			Close();
		}

		private void Dismiss()
		{
			Action action = onDismissed;
			onDismissed = null;
			Close();
			action?.Invoke();
		}

		private void Close()
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
				GameMenuState.SetMenuOpen(this, open: false);
			}
		}

		private void Update()
		{
			if (backdrop.color.a < 0.72f)
			{
				Color color = backdrop.color;
				color.a = Mathf.Min(0.72f, color.a + 3f * Time.unscaledDeltaTime);
				backdrop.color = color;
			}
			if (Time.unscaledTime >= dismissAtTime)
			{
				Dismiss();
			}
		}
	}
}

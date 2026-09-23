using System;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class CompanionChoiceView : MonoBehaviour
	{
		private const float FadeInSpeed = 3f;

		private const float BackdropAlpha = 0.72f;

		[SerializeField]
		private Image backdrop;

		[SerializeField]
		private TextMeshProUGUI titleLabel;

		[SerializeField]
		private TextMeshProUGUI subtitleLabel;

		[SerializeField]
		private Button helpHuntersButton;

		[SerializeField]
		private Button helpHidersButton;

		[SerializeField]
		private Button watchButton;

		private Action onHelpHunters;

		private Action onWatch;

		public static CompanionChoiceView Instance { get; private set; }

		public static CompanionChoiceView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "CompanionChoice");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.03f, 0.03f, 0.06f, 0f);
			image.raycastTarget = true;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Title", "", 40);
			LocalizedText.Attach(textMeshProUGUI, "Overlay.WhoWillYouHelp");
			PlaceCentred((RectTransform)textMeshProUGUI.transform, 120f, new Vector2(700f, 60f));
			textMeshProUGUI.color = new Color(0.95f, 0.9f, 0.75f);
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform, "Subtitle", "", 20);
			LocalizedText.Attach(textMeshProUGUI2, "Overlay.Companion");
			PlaceCentred((RectTransform)textMeshProUGUI2.transform, 72f, new Vector2(700f, 30f));
			textMeshProUGUI2.color = new Color(0.8f, 0.8f, 0.8f);
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform, "HelpHuntersButton", "Avcılara yardım et", out text);
			PlaceCentred((RectTransform)button.transform, 10f, new Vector2(280f, 46f));
			TextMeshProUGUI text2;
			Button button2 = UIFactory.CreateButton(rectTransform, "HelpHidersButton", "Saklananlara yardım et", out text2);
			PlaceCentred((RectTransform)button2.transform, -46f, new Vector2(280f, 46f));
			button2.interactable = false;
			if (text2 != null)
			{
				text2.color = new Color(0.55f, 0.55f, 0.55f);
			}
			Button button3 = UIFactory.CreateButton(rectTransform, "WatchButton", "Sadece izle", out text);
			PlaceCentred((RectTransform)button3.transform, -110f, new Vector2(280f, 40f));
			CompanionChoiceView companionChoiceView = rectTransform.gameObject.AddComponent<CompanionChoiceView>();
			companionChoiceView.backdrop = image;
			companionChoiceView.titleLabel = textMeshProUGUI;
			companionChoiceView.subtitleLabel = textMeshProUGUI2;
			companionChoiceView.helpHuntersButton = button;
			companionChoiceView.helpHidersButton = button2;
			companionChoiceView.watchButton = button3;
			return companionChoiceView;
		}

		private static void PlaceCentred(RectTransform rect, float y, Vector2 size)
		{
			rect.anchorMin = new Vector2(0.5f, 0.5f);
			rect.anchorMax = new Vector2(0.5f, 0.5f);
			rect.anchoredPosition = new Vector2(0f, y);
			rect.sizeDelta = size;
		}

		private void Awake()
		{
			Instance = this;
			if (helpHuntersButton != null)
			{
				helpHuntersButton.onClick.AddListener(ChooseHunters);
			}
			if (watchButton != null)
			{
				watchButton.onClick.AddListener(ChooseWatch);
			}
			if (helpHidersButton != null)
			{
				helpHidersButton.interactable = false;
			}
			base.gameObject.SetActive(value: false);
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Show(Action onHelpHunters, Action onWatch)
		{
			this.onHelpHunters = onHelpHunters;
			this.onWatch = onWatch;
			Color color = backdrop.color;
			color.a = 0f;
			backdrop.color = color;
			base.gameObject.SetActive(value: true);
			GameMenuState.SetMenuOpen(this, open: true);
		}

		public void Cancel()
		{
			onHelpHunters = null;
			onWatch = null;
			Close();
		}

		private void ChooseHunters()
		{
			Answer(ref onHelpHunters);
		}

		private void ChooseWatch()
		{
			Answer(ref onWatch);
		}

		private void Answer(ref Action chosen)
		{
			Action obj = chosen;
			onHelpHunters = null;
			onWatch = null;
			Close();
			obj?.Invoke();
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
			if (!(backdrop.color.a >= 0.72f))
			{
				Color color = backdrop.color;
				color.a = Mathf.Min(0.72f, color.a + 3f * Time.unscaledDeltaTime);
				backdrop.color = color;
			}
		}
	}
}

using System;
using DG.Tweening;
using Mimicraft.Localization;
using Mimicraft.Tutorial;
using Mimicraft.VoxelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Mimicraft.UI
{
	public class TutorialCoachView : MonoBehaviour
	{
		private const float HintHeight = 52f;

		private const int ClipWidth = 320;

		private const int ClipHeight = 180;

		[SerializeField]
		private TextMeshProUGUI progressLabel;

		[SerializeField]
		private TextMeshProUGUI titleLabel;

		[SerializeField]
		private TextMeshProUGUI bodyLabel;

		[SerializeField]
		private TextMeshProUGUI hintLabel;

		[SerializeField]
		private TextMeshProUGUI stepProgressLabel;

		[SerializeField]
		private RectTransform stepProgressFill;

		[SerializeField]
		private Button advanceButton;

		[SerializeField]
		private Button skipButton;

		[SerializeField]
		private Button quitButton;

		[SerializeField]
		private Button hintButton;

		[SerializeField]
		private Button findModelButton;

		[SerializeField]
		private Button doItButton;

		[SerializeField]
		private TextMeshProUGUI doItText;

		[SerializeField]
		private RawImage clipImage;

		[SerializeField]
		private VideoPlayer clipPlayer;

		private RenderTexture clipTexture;

		[SerializeField]
		private Image highlightRing;

		private bool layoutCaptured;

		private float authoredHeight;

		private Vector2 authoredBodyOffsetMin;

		private Tween ringTween;

		public static TutorialCoachView Instance { get; private set; }

		public static TutorialCoachView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "TutorialCoach");
			rectTransform.anchorMin = new Vector2(0.5f, 0f);
			rectTransform.anchorMax = new Vector2(0.5f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.anchoredPosition = new Vector2(0f, 96f);
			rectTransform.sizeDelta = new Vector2(540f, 172f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.09f, 0.11f, 0.14f, 0.94f);
			image.raycastTarget = false;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Progress", "", 12);
			textMeshProUGUI.alignment = TextAlignmentOptions.TopRight;
			textMeshProUGUI.color = new Color(0.55f, 0.75f, 0.72f, 1f);
			textMeshProUGUI.raycastTarget = false;
			Stretch(textMeshProUGUI, new Vector2(14f, -36f), new Vector2(-14f, -12f));
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform, "Title", "", 17);
			textMeshProUGUI2.alignment = TextAlignmentOptions.TopLeft;
			textMeshProUGUI2.fontStyle = FontStyles.Bold;
			textMeshProUGUI2.raycastTarget = false;
			Stretch(textMeshProUGUI2, new Vector2(14f, -36f), new Vector2(-70f, -12f));
			TextMeshProUGUI textMeshProUGUI3 = UIFactory.CreateLabel(rectTransform, "Body", "", 13);
			textMeshProUGUI3.alignment = TextAlignmentOptions.TopLeft;
			textMeshProUGUI3.textWrappingMode = TextWrappingModes.Normal;
			textMeshProUGUI3.raycastTarget = false;
			Stretch(textMeshProUGUI3, new Vector2(14f, 44f), new Vector2(-14f, -40f));
			Button button = MakeFooterButton(rectTransform, "AdvanceButton", "Button.Continue", 0f);
			Button button2 = MakeFooterButton(rectTransform, "SkipButton", "Button.Skip", 0f);
			Button button3 = MakeFooterButton(rectTransform, "QuitButton", "Button.Quit", -96f);
			TutorialCoachView tutorialCoachView = rectTransform.gameObject.AddComponent<TutorialCoachView>();
			tutorialCoachView.progressLabel = textMeshProUGUI;
			tutorialCoachView.titleLabel = textMeshProUGUI2;
			tutorialCoachView.bodyLabel = textMeshProUGUI3;
			tutorialCoachView.advanceButton = button;
			tutorialCoachView.skipButton = button2;
			tutorialCoachView.quitButton = button3;
			return tutorialCoachView;
		}

		private void EnsureParts()
		{
			RectTransform rectTransform = (RectTransform)base.transform;
			if (stepProgressFill == null)
			{
				Image image = UIFactory.CreatePanel(rectTransform, "StepProgressTrack", new Color(1f, 1f, 1f, 0.08f));
				image.raycastTarget = false;
				TopEdge((RectTransform)image.transform, 1f);
				Image image2 = UIFactory.CreatePanel(rectTransform, "StepProgressFill", new Color(0.4f, 0.9f, 0.8f, 0.9f));
				image2.raycastTarget = false;
				stepProgressFill = (RectTransform)image2.transform;
				TopEdge(stepProgressFill, 0f);
			}
			if (stepProgressLabel == null)
			{
				stepProgressLabel = UIFactory.CreateLabel(rectTransform, "StepProgress", "", 11);
				stepProgressLabel.alignment = TextAlignmentOptions.TopRight;
				stepProgressLabel.color = new Color(0.4f, 0.9f, 0.8f, 1f);
				stepProgressLabel.raycastTarget = false;
				Stretch(stepProgressLabel, new Vector2(14f, -52f), new Vector2(-14f, -30f));
			}
			if (hintLabel == null)
			{
				hintLabel = UIFactory.CreateLabel(rectTransform, "Hint", "", 12);
				hintLabel.alignment = TextAlignmentOptions.TopLeft;
				hintLabel.textWrappingMode = TextWrappingModes.Normal;
				hintLabel.color = new Color(1f, 0.83f, 0.48f, 1f);
				hintLabel.raycastTarget = false;
				RectTransform obj = (RectTransform)hintLabel.transform;
				obj.anchorMin = new Vector2(0f, 0f);
				obj.anchorMax = new Vector2(1f, 0f);
				obj.offsetMin = new Vector2(14f, 44f);
				obj.offsetMax = new Vector2(-14f, 90f);
				hintLabel.gameObject.SetActive(value: false);
			}
			if (hintButton == null)
			{
				hintButton = MakeFooterButton(rectTransform, "HintButton", "Button.Hint", -192f);
				hintButton.gameObject.SetActive(value: false);
			}
			if (doItButton == null)
			{
				doItButton = MakeFooterButton(rectTransform, "DoItButton", "Button.ShowMe", -288f);
				doItButton.gameObject.SetActive(value: false);
			}
			if (doItText == null && doItButton != null)
			{
				doItText = doItButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
			}
			if (findModelButton == null)
			{
				findModelButton = UIFactory.CreateButton(rectTransform, "FindModelButton", "", out var text);
				LocalizedText.Attach(text, "Button.FindModel");
				RectTransform obj2 = (RectTransform)findModelButton.transform;
				obj2.anchorMin = new Vector2(0f, 0f);
				obj2.anchorMax = new Vector2(0f, 0f);
				obj2.pivot = new Vector2(0f, 0f);
				obj2.anchoredPosition = new Vector2(14f, 12f);
				obj2.sizeDelta = new Vector2(104f, 26f);
			}
			if (clipImage == null)
			{
				RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Clip");
				rectTransform2.anchorMin = new Vector2(0.5f, 1f);
				rectTransform2.anchorMax = new Vector2(0.5f, 1f);
				rectTransform2.pivot = new Vector2(0.5f, 0f);
				rectTransform2.anchoredPosition = new Vector2(0f, 8f);
				rectTransform2.sizeDelta = new Vector2(320f, 180f);
				clipImage = rectTransform2.gameObject.AddComponent<RawImage>();
				clipImage.raycastTarget = false;
				rectTransform2.gameObject.SetActive(value: false);
			}
			if (clipPlayer == null)
			{
				clipPlayer = GetComponent<VideoPlayer>();
				if (clipPlayer == null)
				{
					clipPlayer = base.gameObject.AddComponent<VideoPlayer>();
				}
				clipPlayer.playOnAwake = false;
				clipPlayer.isLooping = true;
				clipPlayer.renderMode = VideoRenderMode.RenderTexture;
				clipPlayer.audioOutputMode = VideoAudioOutputMode.None;
				clipPlayer.skipOnDrop = true;
			}
			if (highlightRing == null)
			{
				highlightRing = BuildHighlightRing((rectTransform.parent != null) ? rectTransform.parent : rectTransform);
			}
		}

		private static void TopEdge(RectTransform rect, float width)
		{
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = new Vector2(width, 1f);
			rect.offsetMin = new Vector2(0f, -4f);
			rect.offsetMax = Vector2.zero;
		}

		private static Button MakeFooterButton(RectTransform parent, string name, string labelKey, float xOffset)
		{
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(parent, name, "", out text);
			LocalizedText.Attach(text, labelKey);
			RectTransform obj = (RectTransform)button.transform;
			obj.anchorMin = new Vector2(1f, 0f);
			obj.anchorMax = new Vector2(1f, 0f);
			obj.pivot = new Vector2(1f, 0f);
			obj.anchoredPosition = new Vector2(-14f + xOffset, 12f);
			obj.sizeDelta = new Vector2(88f, 26f);
			return button;
		}

		private static Image BuildHighlightRing(Transform canvas)
		{
			Image image = UIFactory.CreatePanel(canvas, "TutorialHighlight", new Color(0.4f, 0.9f, 0.8f, 0.35f));
			RectTransform rectTransform = (RectTransform)image.transform;
			rectTransform.sizeDelta = new Vector2(44f, 44f);
			image.raycastTarget = false;
			rectTransform.SetAsFirstSibling();
			image.gameObject.SetActive(value: false);
			return image;
		}

		private static void Stretch(Graphic graphic, Vector2 offsetMin, Vector2 offsetMax)
		{
			RectTransform obj = (RectTransform)graphic.transform;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.offsetMin = offsetMin;
			obj.offsetMax = offsetMax;
		}

		private void Awake()
		{
			Instance = this;
			EnsureParts();
			base.gameObject.SetActive(value: false);
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			KillRingTween();
			if (clipTexture != null)
			{
				clipTexture.Release();
				UnityEngine.Object.Destroy(clipTexture);
			}
		}

		public void Bind(Action onAdvance, Action onSkip, Action onQuit, Action onHint, Action onDoIt, Action onFindModel)
		{
			EnsureParts();
			Wire(advanceButton, onAdvance);
			Wire(skipButton, onSkip);
			Wire(quitButton, onQuit);
			Wire(hintButton, onHint);
			Wire(doItButton, onDoIt);
			Wire(findModelButton, onFindModel);
		}

		private static void Wire(Button button, Action action)
		{
			if (!(button == null))
			{
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(delegate
				{
					action?.Invoke();
				});
			}
		}

		public void Show(TutorialStep step, int index, int total)
		{
			base.gameObject.SetActive(value: true);
			if (progressLabel != null)
			{
				progressLabel.text = Loc.Format("Common.Progress", index + 1, total);
			}
			if (titleLabel != null)
			{
				LocalizedText.Attach(titleLabel, step.TitleKey);
			}
			if (bodyLabel != null)
			{
				LocalizedText.Attach(bodyLabel, step.BodyKey);
			}
			bool needsManualAdvance = step.NeedsManualAdvance;
			if (advanceButton != null)
			{
				advanceButton.gameObject.SetActive(needsManualAdvance);
			}
			if (skipButton != null)
			{
				skipButton.gameObject.SetActive(!needsManualAdvance);
			}
			if (hintButton != null)
			{
				hintButton.gameObject.SetActive(value: false);
			}
			HideDoIt();
			SetHintShown(shown: false);
			SetProgressShown(shown: false);
			SetHighlight(step.Highlight);
		}

		public void Hide()
		{
			SetHighlight(null);
			base.gameObject.SetActive(value: false);
		}

		public void SetProgress(int done, int total)
		{
			if (total <= 0 || stepProgressFill == null)
			{
				SetProgressShown(shown: false);
				return;
			}
			SetProgressShown(shown: true);
			float x = Mathf.Clamp01((float)done / (float)total);
			stepProgressFill.anchorMax = new Vector2(x, 1f);
			if (stepProgressLabel != null)
			{
				stepProgressLabel.text = Loc.Format("Progress.Done", done, total);
			}
		}

		private void SetProgressShown(bool shown)
		{
			if (stepProgressFill != null)
			{
				stepProgressFill.gameObject.SetActive(shown);
			}
			if (stepProgressLabel != null)
			{
				stepProgressLabel.gameObject.SetActive(shown);
			}
		}

		public void OfferHint()
		{
			if (hintButton != null)
			{
				hintButton.gameObject.SetActive(value: true);
			}
		}

		public void ShowHint(string hintKey)
		{
			if (!(hintLabel == null))
			{
				LocalizedText.Attach(hintLabel, hintKey);
				SetHintShown(shown: true);
				if (hintButton != null)
				{
					hintButton.gameObject.SetActive(value: false);
				}
			}
		}

		public void OfferDoIt(bool keep)
		{
			if (!(doItButton == null))
			{
				if (doItText != null)
				{
					LocalizedText.Attach(doItText, keep ? "Button.DoIt" : "Button.ShowMe");
				}
				doItButton.gameObject.SetActive(value: true);
			}
		}

		public void HideDoIt()
		{
			if (doItButton != null)
			{
				doItButton.gameObject.SetActive(value: false);
			}
		}

		private void SetHintShown(bool shown)
		{
			if (hintLabel == null)
			{
				return;
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			RectTransform rectTransform2 = ((bodyLabel != null) ? ((RectTransform)bodyLabel.transform) : null);
			if (!layoutCaptured)
			{
				layoutCaptured = true;
				authoredHeight = rectTransform.sizeDelta.y;
				if (rectTransform2 != null)
				{
					authoredBodyOffsetMin = rectTransform2.offsetMin;
				}
			}
			hintLabel.gameObject.SetActive(shown);
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, shown ? (authoredHeight + 52f) : authoredHeight);
			if (rectTransform2 != null)
			{
				rectTransform2.offsetMin = authoredBodyOffsetMin + (shown ? new Vector2(0f, 52f) : Vector2.zero);
			}
		}

		public void SetFindModelVisible(bool visible)
		{
			if (findModelButton != null && findModelButton.gameObject.activeSelf != visible)
			{
				findModelButton.gameObject.SetActive(visible);
			}
		}

		public void ShowClip(VideoClip clip)
		{
			if (clipImage == null || clipPlayer == null)
			{
				return;
			}
			if (clip == null)
			{
				clipPlayer.Stop();
				clipPlayer.clip = null;
				clipImage.gameObject.SetActive(value: false);
				return;
			}
			if (clipTexture == null)
			{
				clipTexture = new RenderTexture(320, 180, 0)
				{
					name = "TutorialClip"
				};
				clipPlayer.targetTexture = clipTexture;
				clipImage.texture = clipTexture;
			}
			clipPlayer.clip = clip;
			clipImage.gameObject.SetActive(value: true);
			clipPlayer.Play();
		}

		private void SetHighlight(EditorState? state)
		{
			KillRingTween();
			if (!(highlightRing == null))
			{
				RectTransform rectTransform = ((state.HasValue && ToolbarView.Instance != null) ? ToolbarView.Instance.GetToolButton(state.Value) : null);
				if (rectTransform == null)
				{
					highlightRing.gameObject.SetActive(value: false);
					return;
				}
				RectTransform rectTransform2 = (RectTransform)highlightRing.transform;
				rectTransform2.position = rectTransform.position;
				highlightRing.gameObject.SetActive(value: true);
				rectTransform2.localScale = Vector3.one;
				ringTween = rectTransform2.DOScale(1.35f, 0.65f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine)
					.SetUpdate(isIndependentUpdate: true);
			}
		}

		private void KillRingTween()
		{
			ringTween?.Kill();
			ringTween = null;
			if (highlightRing != null)
			{
				highlightRing.transform.localScale = Vector3.one;
			}
		}
	}
}

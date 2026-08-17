using EvilCore.Localization;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Objectives
{
	public class ObjectiveStepUI : MonoBehaviour
	{
		[Header("Icon")]
		[SerializeField]
		private GameObject iconRoot;

		[SerializeField]
		private Image iconImage;

		[Header("Text")]
		[SerializeField]
		private TMP_Text label;

		[Header("Checkmark")]
		[SerializeField]
		private GameObject checkmarkRoot;

		[SerializeField]
		private CanvasGroup checkmarkGroup;

		[SerializeField]
		private float checkmarkAnimDuration = 0.3f;

		[SerializeField]
		private Ease checkmarkAnimEase = Ease.OutBack;

		[Header("Discrete Progress (X/Y)")]
		[SerializeField]
		private GameObject discreteRoot;

		[SerializeField]
		private TMP_Text discreteLabel;

		[Header("Float Progress (Bar)")]
		[SerializeField]
		private GameObject barRoot;

		[SerializeField]
		private Image barFill;

		[SerializeField]
		private TMP_Text barPercentLabel;

		[SerializeField]
		private float barTweenDuration = 0.25f;

		[SerializeField]
		private Ease barTweenEase = Ease.OutQuad;

		[Header("Completion Style")]
		[SerializeField]
		private CanvasGroup rowGroup;

		[SerializeField]
		private Color completedLabelColor = new Color(0.55f, 0.55f, 0.55f, 1f);

		[SerializeField]
		private bool applyStrikethroughOnComplete = true;

		[SerializeField]
		private float completionColorTweenDuration = 0.25f;

		[SerializeField]
		private Ease completionColorEase = Ease.OutCubic;

		[Header("Completion Feedback")]
		[SerializeField]
		private Color completionFlashColor = new Color(1f, 0.95f, 0.4f, 1f);

		[SerializeField]
		private float completionFlashRiseDuration = 0.12f;

		[SerializeField]
		private float completionFlashHold = 0.08f;

		[SerializeField]
		private Vector3 rowPunchStrength = new Vector3(0.05f, 0.05f, 0f);

		[SerializeField]
		private float rowPunchDuration = 0.4f;

		[SerializeField]
		private Vector3 checkmarkPunchStrength = new Vector3(0.35f, 0.35f, 0f);

		[SerializeField]
		private float checkmarkPunchDuration = 0.3f;

		[Inject]
		private ILocalizationService _localizationService;

		private string _descriptionKey;

		private StepProgressMode _progressMode;

		private Color _defaultLabelColor = Color.white;

		private FontStyles _defaultFontStyle;

		private bool _defaultsCaptured;

		private Sequence _completionSequence;

		public CanvasGroup RowGroup => rowGroup;

		private void Awake()
		{
			CaptureLabelDefaults();
		}

		public void PrepareForReveal()
		{
			if (rowGroup != null)
			{
				rowGroup.alpha = 0f;
			}
		}

		private void CaptureLabelDefaults()
		{
			if (!_defaultsCaptured && !(label == null))
			{
				_defaultLabelColor = label.color;
				_defaultFontStyle = label.fontStyle;
				_defaultsCaptured = true;
			}
		}

		public void Bind(ObjectiveStepDefinition step, ObjectiveStepProgress progress)
		{
			if (step != null)
			{
				CaptureLabelDefaults();
				_descriptionKey = step.DescriptionKey;
				_progressMode = step.ProgressMode;
				if (iconRoot != null)
				{
					iconRoot.SetActive(step.Icon != null);
				}
				if (iconImage != null && step.Icon != null)
				{
					iconImage.sprite = step.Icon;
				}
				RefreshText();
				SubscribeLocale();
				bool flag = progress?.IsCompleted ?? false;
				ApplyCompletionVisual(flag);
				switch (_progressMode)
				{
				case StepProgressMode.DiscreteCount:
				{
					int num2 = Mathf.Max(1, step.DiscreteTarget);
					int current = ((progress != null) ? Mathf.Clamp(progress.Count, 0, num2) : 0);
					SetDiscreteVisible(!flag);
					SetBarVisible(visible: false);
					UpdateDiscreteLabel(current, num2);
					break;
				}
				case StepProgressMode.FloatRatio:
				{
					float num = ((progress != null) ? Mathf.Clamp01(progress.Ratio) : 0f);
					SetDiscreteVisible(visible: false);
					SetBarVisible(!flag);
					SetBarFillImmediate(flag ? 1f : num);
					break;
				}
				default:
					SetDiscreteVisible(visible: false);
					SetBarVisible(visible: false);
					break;
				}
			}
		}

		public void UpdateDiscreteProgress(int current, int target)
		{
			if (_progressMode == StepProgressMode.DiscreteCount)
			{
				int num = Mathf.Max(1, target);
				int current2 = Mathf.Clamp(current, 0, num);
				UpdateDiscreteLabel(current2, num);
			}
		}

		public void UpdateRatioProgress(float ratio)
		{
			if (_progressMode == StepProgressMode.FloatRatio)
			{
				float targetRatio = Mathf.Clamp01(ratio);
				TweenBarFill(targetRatio);
			}
		}

		public void MarkComplete(bool animate)
		{
			if (barFill != null)
			{
				Tween.CompleteAll(barFill);
			}
			SetDiscreteVisible(visible: false);
			SetBarVisible(visible: false);
			ApplyCompletedLabelStyle(animate);
			if (animate && rowPunchDuration > 0f && rowPunchStrength.sqrMagnitude > 0f)
			{
				Tween.PunchScale(base.transform, rowPunchStrength, rowPunchDuration);
			}
			if (checkmarkRoot != null)
			{
				checkmarkRoot.SetActive(value: true);
			}
			if (checkmarkGroup == null)
			{
				return;
			}
			if (!animate)
			{
				checkmarkGroup.alpha = 1f;
				return;
			}
			Tween.CompleteAll(checkmarkGroup);
			checkmarkGroup.alpha = 0f;
			Tween.Alpha(checkmarkGroup, 1f, checkmarkAnimDuration, checkmarkAnimEase);
			Tween.Scale(checkmarkRoot.transform, Vector3.zero, Vector3.one, checkmarkAnimDuration, checkmarkAnimEase);
			if (checkmarkPunchDuration > 0f && checkmarkPunchStrength.sqrMagnitude > 0f)
			{
				Tween.PunchScale(checkmarkRoot.transform, checkmarkPunchStrength, checkmarkPunchDuration, 10f, enableFalloff: true, Ease.Default, 0f, 1, checkmarkAnimDuration * 0.6f);
			}
		}

		private void ApplyCompletedLabelStyle(bool animate)
		{
			if (label == null)
			{
				return;
			}
			CaptureLabelDefaults();
			if (applyStrikethroughOnComplete)
			{
				label.fontStyle |= FontStyles.Strikethrough;
			}
			_completionSequence.Stop();
			if (!animate || completionColorTweenDuration <= 0f)
			{
				label.color = completedLabelColor;
				return;
			}
			Color color = label.color;
			_completionSequence = Sequence.Create().Chain(Tween.Custom(label, color, completionFlashColor, completionFlashRiseDuration, delegate(TMP_Text tmp, Color c)
			{
				if (tmp != null)
				{
					tmp.color = c;
				}
			}, Ease.OutCubic)).ChainDelay(completionFlashHold)
				.Chain(Tween.Custom(label, completionFlashColor, completedLabelColor, completionColorTweenDuration, delegate(TMP_Text tmp, Color c)
				{
					if (tmp != null)
					{
						tmp.color = c;
					}
				}, completionColorEase));
		}

		private void OnEnable()
		{
			SubscribeLocale();
		}

		private void OnDisable()
		{
			UnsubscribeLocale();
		}

		private void SubscribeLocale()
		{
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= RefreshText;
				_localizationService.OnLocaleChanged += RefreshText;
			}
		}

		private void UnsubscribeLocale()
		{
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= RefreshText;
			}
		}

		private void RefreshText()
		{
			if (!(label == null))
			{
				label.text = ResolveText(_descriptionKey);
			}
		}

		private string ResolveText(string keyOrRaw)
		{
			if (string.IsNullOrEmpty(keyOrRaw))
			{
				return string.Empty;
			}
			if (_localizationService == null)
			{
				return keyOrRaw;
			}
			return _localizationService.Localize(keyOrRaw);
		}

		private void ApplyCompletionVisual(bool isCompleted)
		{
			if (checkmarkRoot != null)
			{
				checkmarkRoot.SetActive(isCompleted);
			}
			if (checkmarkGroup != null)
			{
				checkmarkGroup.alpha = (isCompleted ? 1f : 0f);
			}
			if (!(label != null))
			{
				return;
			}
			CaptureLabelDefaults();
			label.color = (isCompleted ? completedLabelColor : _defaultLabelColor);
			if (applyStrikethroughOnComplete)
			{
				if (isCompleted)
				{
					label.fontStyle |= FontStyles.Strikethrough;
				}
				else
				{
					label.fontStyle = _defaultFontStyle;
				}
			}
		}

		private void SetDiscreteVisible(bool visible)
		{
			if (discreteRoot != null)
			{
				discreteRoot.SetActive(visible);
			}
		}

		private void SetBarVisible(bool visible)
		{
			if (barRoot != null)
			{
				barRoot.SetActive(visible);
			}
		}

		private void UpdateDiscreteLabel(int current, int target)
		{
			if (discreteLabel != null)
			{
				discreteLabel.text = $"{current}/{target}";
			}
		}

		private void SetBarFillImmediate(float ratio)
		{
			if (barFill != null)
			{
				barFill.fillAmount = ratio;
			}
			UpdateBarPercentLabel(ratio);
		}

		private void TweenBarFill(float targetRatio)
		{
			UpdateBarPercentLabel(targetRatio);
			if (barFill == null)
			{
				return;
			}
			if (barTweenDuration <= 0f)
			{
				barFill.fillAmount = targetRatio;
				return;
			}
			Tween.CompleteAll(barFill);
			float fillAmount = barFill.fillAmount;
			if (Mathf.Approximately(fillAmount, targetRatio))
			{
				return;
			}
			Tween.Custom(barFill, fillAmount, targetRatio, barTweenDuration, delegate(Image img, float v)
			{
				if (img != null)
				{
					img.fillAmount = v;
				}
			}, barTweenEase);
		}

		private void UpdateBarPercentLabel(float ratio)
		{
			if (!(barPercentLabel == null))
			{
				int num = Mathf.RoundToInt(Mathf.Clamp01(ratio) * 100f);
				barPercentLabel.text = $"{num}%";
			}
		}
	}
}

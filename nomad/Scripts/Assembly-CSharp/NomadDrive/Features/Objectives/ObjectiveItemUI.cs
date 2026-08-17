using System;
using System.Collections.Generic;
using EvilCore.Extensions;
using EvilCore.Localization;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Objectives
{
	public class ObjectiveItemUI : MonoBehaviour
	{
		[Header("Title")]
		[SerializeField]
		private GameObject iconRoot;

		[SerializeField]
		private Image iconImage;

		[SerializeField]
		private TMP_Text titleLabel;

		[Header("Steps")]
		[SerializeField]
		private RectTransform stepsContainer;

		[SerializeField]
		private ObjectiveStepUI stepPrefab;

		[Header("Intro Animation")]
		[SerializeField]
		private CanvasGroup itemGroup;

		[SerializeField]
		private RectTransform animatedRoot;

		[SerializeField]
		private Vector2 introSlideOffset = new Vector2(0f, 30f);

		[SerializeField]
		private float introDuration = 0.35f;

		[SerializeField]
		private Ease introEase = Ease.OutCubic;

		[SerializeField]
		private float stepInitialDelay = 0.12f;

		[SerializeField]
		private float stepStagger = 0.06f;

		[SerializeField]
		private float stepFadeDuration = 0.25f;

		[SerializeField]
		private Ease stepFadeEase = Ease.OutCubic;

		[Header("Close Animation")]
		[SerializeField]
		private Color closeFlashColor = new Color(1f, 0.95f, 0.4f, 1f);

		[SerializeField]
		private float closeFlashDuration = 0.14f;

		[SerializeField]
		private float closeFlashHold = 0.08f;

		[SerializeField]
		private Vector3 closePunchStrength = new Vector3(0.07f, 0.07f, 0f);

		[SerializeField]
		private float closePunchDuration = 0.4f;

		[SerializeField]
		private float closeShrinkScale = 0.88f;

		[SerializeField]
		private float closeShrinkDuration = 0.3f;

		[SerializeField]
		private float closeFadeDuration = 0.3f;

		[SerializeField]
		private Ease closeShrinkEase = Ease.InBack;

		[Inject]
		private ILocalizationService _localizationService;

		private readonly Dictionary<string, ObjectiveStepUI> _stepUIs = new Dictionary<string, ObjectiveStepUI>();

		private Sequence _currentSequence;

		private Vector2 _restingPosition;

		private bool _capturedRestingPosition;

		private ObjectiveDefinition _definition;

		private ObjectiveTrackerState _state;

		public string ObjectiveId { get; private set; }

		private void OnValidate()
		{
			if (animatedRoot == null)
			{
				animatedRoot = base.transform as RectTransform;
			}
		}

		private void Awake()
		{
			if (animatedRoot == null)
			{
				animatedRoot = base.transform as RectTransform;
			}
			if (itemGroup == null)
			{
				itemGroup = GetComponent<CanvasGroup>();
			}
			CaptureRestingPosition();
		}

		private void CaptureRestingPosition()
		{
			if (!_capturedRestingPosition && !(animatedRoot == null))
			{
				_restingPosition = animatedRoot.anchoredPosition;
				_capturedRestingPosition = true;
			}
		}

		public void Build(ObjectiveDefinition definition, ObjectiveTrackerState state)
		{
			if (definition == null)
			{
				return;
			}
			_definition = definition;
			_state = state;
			ObjectiveId = definition.ObjectiveId;
			if (iconRoot != null)
			{
				iconRoot.SetActive(definition.Icon != null);
			}
			if (iconImage != null && definition.Icon != null)
			{
				iconImage.sprite = definition.Icon;
			}
			RefreshTitle();
			SubscribeLocale();
			ClearStepRows();
			if (stepsContainer != null && stepPrefab != null && definition.Steps != null)
			{
				foreach (ObjectiveStepDefinition step in definition.Steps)
				{
					if (step != null && !string.IsNullOrEmpty(step.StepId))
					{
						ObjectiveStepUI objectiveStepUI = UnityEngine.Object.Instantiate(stepPrefab, stepsContainer);
						objectiveStepUI.gameObject.InjectGameObject();
						ObjectiveStepProgress progress = state?.GetOrCreate(step.StepId);
						objectiveStepUI.Bind(step, progress);
						objectiveStepUI.PrepareForReveal();
						_stepUIs[step.StepId] = objectiveStepUI;
					}
				}
			}
			PlayIntroAnimation();
		}

		private void PlayIntroAnimation()
		{
			if (animatedRoot == null)
			{
				animatedRoot = base.transform as RectTransform;
			}
			if (animatedRoot == null)
			{
				return;
			}
			CaptureRestingPosition();
			_currentSequence.Stop();
			animatedRoot.anchoredPosition = _restingPosition + introSlideOffset;
			if (itemGroup != null)
			{
				itemGroup.alpha = 0f;
			}
			Sequence currentSequence = Sequence.Create().Chain(Tween.UIAnchoredPosition(animatedRoot, _restingPosition, introDuration, introEase));
			if (itemGroup != null)
			{
				currentSequence = currentSequence.Group(Tween.Alpha(itemGroup, 1f, introDuration, introEase));
			}
			if (_definition != null && _definition.Steps != null)
			{
				int num = 0;
				foreach (ObjectiveStepDefinition step in _definition.Steps)
				{
					if (step == null || string.IsNullOrEmpty(step.StepId))
					{
						continue;
					}
					if (!_stepUIs.TryGetValue(step.StepId, out var value) || value == null)
					{
						num++;
						continue;
					}
					CanvasGroup rowGroup = value.RowGroup;
					if (rowGroup == null)
					{
						num++;
						continue;
					}
					float startDelay = stepInitialDelay + (float)num * stepStagger;
					currentSequence = currentSequence.Group(Tween.Alpha(rowGroup, 1f, stepFadeDuration, stepFadeEase, 1, CycleMode.Restart, startDelay));
					num++;
				}
			}
			_currentSequence = currentSequence;
		}

		public void PlayCloseAnimation(Action onComplete)
		{
			if (animatedRoot == null)
			{
				animatedRoot = base.transform as RectTransform;
			}
			_currentSequence.Stop();
			Sequence sequence = Sequence.Create().Chain(Tween.PunchScale(animatedRoot, closePunchStrength, closePunchDuration));
			if (titleLabel != null && closeFlashDuration > 0f)
			{
				Color color = titleLabel.color;
				sequence = sequence.Group(Tween.Custom(titleLabel, color, closeFlashColor, closeFlashDuration, delegate(TMP_Text t, Color c)
				{
					if (t != null)
					{
						t.color = c;
					}
				}, Ease.OutCubic, 2, CycleMode.Yoyo));
			}
			sequence = sequence.ChainDelay(closeFlashHold).Chain(Tween.Scale(animatedRoot, Vector3.one * closeShrinkScale, closeShrinkDuration, closeShrinkEase));
			if (itemGroup != null)
			{
				sequence = sequence.Group(Tween.Alpha(itemGroup, 0f, closeFadeDuration, Ease.InCubic));
			}
			Action cb = onComplete;
			_currentSequence = sequence.ChainCallback(this, delegate
			{
				cb?.Invoke();
			});
		}

		public void MarkStepComplete(string stepId)
		{
			if (_stepUIs.TryGetValue(stepId, out var value))
			{
				value.MarkComplete(animate: true);
			}
		}

		public void UpdateStepProgress(string stepId, ObjectiveStepProgress progress, ObjectiveStepDefinition step)
		{
			if (step != null && progress != null && _stepUIs.TryGetValue(stepId, out var value))
			{
				switch (step.ProgressMode)
				{
				case StepProgressMode.DiscreteCount:
					value.UpdateDiscreteProgress(progress.Count, Mathf.Max(1, step.DiscreteTarget));
					break;
				case StepProgressMode.FloatRatio:
					value.UpdateRatioProgress(progress.Ratio);
					break;
				}
			}
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
				_localizationService.OnLocaleChanged -= RefreshTitle;
				_localizationService.OnLocaleChanged += RefreshTitle;
			}
		}

		private void UnsubscribeLocale()
		{
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= RefreshTitle;
			}
		}

		private void RefreshTitle()
		{
			if (!(titleLabel == null) && !(_definition == null))
			{
				string raw = ResolveText(_definition.DescriptionKey);
				ObjectiveTextFormatter.Apply(titleLabel, raw, _definition.DescriptionStyle);
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

		private void ClearStepRows()
		{
			foreach (ObjectiveStepUI value in _stepUIs.Values)
			{
				if (value != null)
				{
					UnityEngine.Object.Destroy(value.gameObject);
				}
			}
			_stepUIs.Clear();
		}
	}
}

using EvilCore.Localization;
using EvilCore.UI.Scripts;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Objectives
{
	public class ObjectiveDiscoveryBanner : GameCanvasGroup
	{
		[Header("Content")]
		[SerializeField]
		private GameObject iconRoot;

		[SerializeField]
		private Image iconImage;

		[SerializeField]
		private TMP_Text headerLabel;

		[SerializeField]
		private TMP_Text titleLabel;

		[Header("Header")]
		[SerializeField]
		private string headerLocalizationKey = "@objectives.discovered_banner";

		[Header("Container Motion")]
		[SerializeField]
		private RectTransform animatedRoot;

		[SerializeField]
		private Vector2 hiddenOffset = new Vector2(0f, 220f);

		[SerializeField]
		private float slideInDuration = 0.45f;

		[SerializeField]
		private float holdDuration = 2.5f;

		[SerializeField]
		private float slideOutDuration = 0.35f;

		[SerializeField]
		private Ease slideInEase = Ease.OutBack;

		[SerializeField]
		private Ease slideOutEase = Ease.InBack;

		[Header("Juicy Reveal (child elements)")]
		[SerializeField]
		private RectTransform iconAnimRoot;

		[SerializeField]
		private CanvasGroup iconGroup;

		[SerializeField]
		private CanvasGroup headerGroup;

		[SerializeField]
		private CanvasGroup titleGroup;

		[SerializeField]
		private float elementRevealDelay = 0.18f;

		[SerializeField]
		private float elementStagger = 0.07f;

		[SerializeField]
		private float elementFadeDuration = 0.28f;

		[SerializeField]
		private Ease elementScaleEase = Ease.OutBack;

		[SerializeField]
		private Ease elementFadeEase = Ease.OutCubic;

		[SerializeField]
		private Vector3 iconStartScale = new Vector3(0.2f, 0.2f, 1f);

		[Header("Landing Impact")]
		[SerializeField]
		private bool playLandingPunch = true;

		[SerializeField]
		private float landingPunchDelay = 0.32f;

		[SerializeField]
		private Vector3 landingPunchStrength = new Vector3(0.06f, 0.06f, 0f);

		[SerializeField]
		private float landingPunchDuration = 0.35f;

		[Inject]
		private ILocalizationService _localizationService;

		private Sequence _currentSequence;

		private Vector2 _restingPosition;

		private bool _capturedRestingPosition;

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
			if (animatedRoot != null && !_capturedRestingPosition)
			{
				_restingPosition = animatedRoot.anchoredPosition;
				_capturedRestingPosition = true;
			}
			Initialize();
		}

		public void Show(ObjectiveDefinition definition)
		{
			if (definition == null)
			{
				return;
			}
			if (animatedRoot == null)
			{
				animatedRoot = base.transform as RectTransform;
			}
			if (!(animatedRoot == null))
			{
				if (!_capturedRestingPosition)
				{
					_restingPosition = animatedRoot.anchoredPosition;
					_capturedRestingPosition = true;
				}
				BindContent(definition);
				_currentSequence.Stop();
				Vector2 vector = _restingPosition + hiddenOffset;
				animatedRoot.anchoredPosition = vector;
				if (iconAnimRoot != null)
				{
					iconAnimRoot.localScale = iconStartScale;
				}
				if (iconGroup != null)
				{
					iconGroup.alpha = 0f;
				}
				if (headerGroup != null)
				{
					headerGroup.alpha = 0f;
				}
				if (titleGroup != null)
				{
					titleGroup.alpha = 0f;
				}
				Show(interactable: false, blockRaycast: false);
				float startDelay = elementRevealDelay + elementStagger;
				float startDelay2 = elementRevealDelay + 2f * elementStagger;
				float duration = slideOutDuration * 0.6f;
				Sequence sequence = Sequence.Create().Chain(Tween.UIAnchoredPosition(animatedRoot, _restingPosition, slideInDuration, slideInEase));
				if (iconAnimRoot != null)
				{
					sequence = sequence.Group(Tween.Scale(iconAnimRoot, Vector3.one, elementFadeDuration, elementScaleEase, 1, CycleMode.Restart, elementRevealDelay));
				}
				if (iconGroup != null)
				{
					sequence = sequence.Group(Tween.Alpha(iconGroup, 1f, elementFadeDuration, elementFadeEase, 1, CycleMode.Restart, elementRevealDelay));
				}
				if (headerGroup != null)
				{
					sequence = sequence.Group(Tween.Alpha(headerGroup, 1f, elementFadeDuration, elementFadeEase, 1, CycleMode.Restart, startDelay));
				}
				if (titleGroup != null)
				{
					sequence = sequence.Group(Tween.Alpha(titleGroup, 1f, elementFadeDuration, elementFadeEase, 1, CycleMode.Restart, startDelay2));
				}
				if (playLandingPunch)
				{
					sequence = sequence.Group(Tween.PunchScale(animatedRoot, landingPunchStrength, landingPunchDuration, 10f, enableFalloff: true, Ease.Default, 0f, 1, landingPunchDelay));
				}
				sequence = sequence.ChainDelay(holdDuration).Chain(Tween.UIAnchoredPosition(animatedRoot, vector, slideOutDuration, slideOutEase));
				if (iconGroup != null)
				{
					sequence = sequence.Group(Tween.Alpha(iconGroup, 0f, duration, Ease.InCubic));
				}
				if (headerGroup != null)
				{
					sequence = sequence.Group(Tween.Alpha(headerGroup, 0f, duration, Ease.InCubic));
				}
				if (titleGroup != null)
				{
					sequence = sequence.Group(Tween.Alpha(titleGroup, 0f, duration, Ease.InCubic));
				}
				_currentSequence = sequence.ChainCallback(this, delegate(ObjectiveDiscoveryBanner target)
				{
					target.Hide();
				});
			}
		}

		private void BindContent(ObjectiveDefinition definition)
		{
			if (iconRoot != null)
			{
				iconRoot.SetActive(definition.Icon != null);
			}
			if (iconImage != null && definition.Icon != null)
			{
				iconImage.sprite = definition.Icon;
			}
			if (headerLabel != null)
			{
				headerLabel.text = ResolveText(headerLocalizationKey);
			}
			if (titleLabel != null)
			{
				string raw = ResolveText(definition.DescriptionKey);
				ObjectiveTextFormatter.Apply(titleLabel, raw, definition.DescriptionStyle);
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
	}
}

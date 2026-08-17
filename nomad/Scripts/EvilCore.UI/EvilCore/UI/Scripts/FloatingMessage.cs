using System;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
	public class FloatingMessage : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI messageTextMeshProGUI;

		[Tooltip("Optional. Tinted with the style's accent color. If unset, the text itself is tinted.")]
		[SerializeField]
		private Image background;

		[Tooltip("Optional severity icon. Hidden when the style has no icon.")]
		[SerializeField]
		private Image icon;

		[Tooltip("Optional inner transform the punch/scale/shake accent plays on. Falls back to the text transform so the accent never fights the root's rise animation.")]
		[SerializeField]
		private Transform contentTransform;

		private CanvasGroup _canvasGroup;

		private RectTransform _rect;

		private Transform _content;

		private Vector3 _contentBaseScale;

		private Vector3 _contentBasePosition;

		private Sequence _sequence;

		private Action<FloatingMessage> _onComplete;

		private bool _cached;

		private void Awake()
		{
			Cache();
		}

		private void Cache()
		{
			if (!_cached)
			{
				_canvasGroup = GetComponent<CanvasGroup>();
				_rect = GetComponent<RectTransform>();
				_content = ((contentTransform != null) ? contentTransform : ((messageTextMeshProGUI != null) ? messageTextMeshProGUI.transform : base.transform));
				_contentBaseScale = _content.localScale;
				_contentBasePosition = _content.localPosition;
				_cached = true;
			}
		}

		public void Play(string message, FeedbackStyle style, float onScreen, float rise, float fade, float riseDistance, float settleY, Action<FloatingMessage> onComplete)
		{
			Cache();
			_sequence.Stop();
			_onComplete = onComplete;
			if (messageTextMeshProGUI != null)
			{
				messageTextMeshProGUI.text = message;
			}
			ApplyStyleVisuals(style);
			_content.localScale = _contentBaseScale;
			_content.localPosition = _contentBasePosition;
			Vector2 anchoredPosition = _rect.anchoredPosition;
			anchoredPosition.y = settleY - riseDistance;
			_rect.anchoredPosition = anchoredPosition;
			_canvasGroup.alpha = 0f;
			float duration = Mathf.Min(0.2f, rise * 0.6f);
			Tween tween = BuildEntranceAccent(style);
			_sequence = Sequence.Create(1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).Chain(Tween.UIAnchoredPositionY(_rect, settleY, rise, Ease.OutCubic, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true)).Group(Tween.Alpha(_canvasGroup, 1f, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true))
				.Group(tween)
				.ChainDelay(onScreen)
				.Chain(Tween.Alpha(_canvasGroup, 0f, fade, Ease.OutExpo, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true))
				.ChainCallback(this, delegate(FloatingMessage t)
				{
					t.HandleComplete();
				});
		}

		private void ApplyStyleVisuals(FeedbackStyle style)
		{
			Color color = style?.AccentColor ?? Color.white;
			if (background != null)
			{
				background.color = color;
			}
			else if (messageTextMeshProGUI != null)
			{
				messageTextMeshProGUI.color = color;
			}
			if (icon != null)
			{
				bool flag = style?.Icon != null;
				icon.enabled = flag;
				if (flag)
				{
					icon.sprite = style.Icon;
				}
			}
		}

		private Tween BuildEntranceAccent(FeedbackStyle style)
		{
			FeedbackEntrance feedbackEntrance = style?.Entrance ?? FeedbackEntrance.Fade;
			float duration = style?.AccentDuration ?? 0.3f;
			float frequency = style?.AccentFrequency ?? 10f;
			switch (feedbackEntrance)
			{
			case FeedbackEntrance.Punch:
				return Tween.PunchScale(_content, style.PunchStrength, duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, 0f, 0f, useUnscaledTime: true);
			case FeedbackEntrance.Shake:
				return Tween.ShakeLocalPosition(_content, style.ShakeStrength, duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, 0f, 0f, useUnscaledTime: true);
			default:
			{
				float num = style?.EntranceScaleFrom ?? 0.9f;
				_content.localScale = _contentBaseScale * num;
				return Tween.Scale(_content, _contentBaseScale, duration, Ease.OutBack, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			}
		}

		private void HandleComplete()
		{
			Action<FloatingMessage> onComplete = _onComplete;
			_onComplete = null;
			onComplete?.Invoke(this);
		}

		private void OnDisable()
		{
			_sequence.Stop();
		}
	}
}

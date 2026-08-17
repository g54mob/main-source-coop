using System;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Extensions;
using PrimeTween;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Scripts
{
	[RequireComponent(typeof(CanvasGroup))]
	public class UIAppear : MonoBehaviour
	{
		[Header("Channels")]
		[SerializeField]
		private bool useFade = true;

		[SerializeField]
		private bool useScale = true;

		[SerializeField]
		private bool useSlide = true;

		[Header("Settings")]
		[SerializeField]
		private float fromScale = 0.9f;

		[SerializeField]
		private AppearDirection direction = AppearDirection.Down;

		[SerializeField]
		private float slideDistance = 40f;

		[SerializeField]
		private float duration = 0.3f;

		[SerializeField]
		private float startDelay;

		[SerializeField]
		private Ease ease = Ease.OutCubic;

		[SerializeField]
		private bool playOnEnable = true;

		[Header("Sound (swoosh, optional)")]
		[SerializeField]
		private SoundID appearSound;

		[SerializeField]
		private SoundID exitSound;

		[Inject]
		private IAudioManager _audioManager;

		private CanvasGroup _canvasGroup;

		private RectTransform _rect;

		private Vector3 _baseScale;

		private Vector2 _basePosition;

		private bool _cached;

		private Tween _fade;

		private Tween _scale;

		private Tween _slide;

		private Sequence _exitSeq;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			Cache();
		}

		private void Cache()
		{
			if (!_cached)
			{
				_canvasGroup = GetComponent<CanvasGroup>();
				_rect = base.transform as RectTransform;
				_baseScale = base.transform.localScale;
				if (_rect != null)
				{
					_basePosition = _rect.anchoredPosition;
				}
				_cached = true;
			}
		}

		private void OnEnable()
		{
			if (playOnEnable)
			{
				Play();
			}
		}

		public void Play()
		{
			Cache();
			StopOwnTweens();
			PlaySound(appearSound);
			if (useFade)
			{
				_canvasGroup.alpha = 0f;
				_fade = Tween.Alpha(_canvasGroup, 1f, duration, ease, 1, CycleMode.Restart, startDelay, 0f, useUnscaledTime: true);
			}
			if (useScale)
			{
				base.transform.localScale = _baseScale * fromScale;
				_scale = Tween.Scale(base.transform, _baseScale, duration, ease, 1, CycleMode.Restart, startDelay, 0f, useUnscaledTime: true);
			}
			if (useSlide && _rect != null)
			{
				_rect.anchoredPosition = _basePosition + GetOffset();
				_slide = Tween.UIAnchoredPosition(_rect, _basePosition, duration, ease, 1, CycleMode.Restart, startDelay, 0f, useUnscaledTime: true);
			}
		}

		public void PlayExit(Action onComplete = null)
		{
			Cache();
			StopOwnTweens();
			PlaySound(exitSound);
			_exitSeq = Sequence.Create(1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true);
			bool flag = false;
			if (useSlide && _rect != null)
			{
				_exitSeq.Chain(Tween.UIAnchoredPosition(_rect, _basePosition + GetOffset(), duration, ease, 1, CycleMode.Restart, startDelay, 0f, useUnscaledTime: true));
				flag = true;
			}
			if (useScale)
			{
				Tween tween = Tween.Scale(base.transform, _baseScale * fromScale, duration, ease, 1, CycleMode.Restart, startDelay, 0f, useUnscaledTime: true);
				if (flag)
				{
					_exitSeq.Group(tween);
				}
				else
				{
					_exitSeq.Chain(tween);
					flag = true;
				}
			}
			if (useFade)
			{
				Tween tween2 = Tween.Alpha(_canvasGroup, 0f, duration, ease, 1, CycleMode.Restart, startDelay, 0f, useUnscaledTime: true);
				if (flag)
				{
					_exitSeq.Group(tween2);
				}
				else
				{
					_exitSeq.Chain(tween2);
					flag = true;
				}
			}
			if (onComplete != null)
			{
				if (flag)
				{
					_exitSeq.ChainCallback(onComplete);
				}
				else
				{
					onComplete();
				}
			}
		}

		private Vector2 GetOffset()
		{
			return direction switch
			{
				AppearDirection.Up => new Vector2(0f, 0f - slideDistance), 
				AppearDirection.Down => new Vector2(0f, slideDistance), 
				AppearDirection.Left => new Vector2(slideDistance, 0f), 
				AppearDirection.Right => new Vector2(0f - slideDistance, 0f), 
				_ => Vector2.zero, 
			};
		}

		private void StopOwnTweens()
		{
			_fade.Stop();
			_scale.Stop();
			_slide.Stop();
			_exitSeq.Stop();
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}

		private void OnDisable()
		{
			if (_cached)
			{
				StopOwnTweens();
				_canvasGroup.alpha = 1f;
				base.transform.localScale = _baseScale;
				if (_rect != null)
				{
					_rect.anchoredPosition = _basePosition;
				}
			}
		}
	}
}

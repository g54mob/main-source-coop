using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Features.UIAnimationsModule.Scripts
{
	public class UIAnimationPlaybackController
	{
		private struct ActiveEntry
		{
			public Sequence Sequence;

			public UIAnimationKey Key;

			public UniTaskCompletionSource CompletionSource;

			public bool EndedFired;
		}

		private struct PositionRestState
		{
			public Vector2 AnchoredPosition;

			public Vector2 OffsetMin;

			public Vector2 OffsetMax;
		}

		private readonly IUIAnimationPresetResolver _resolver;

		private readonly UIAnimationModel _model;

		private readonly Dictionary<RectTransform, ActiveEntry> _active = new Dictionary<RectTransform, ActiveEntry>();

		private readonly Dictionary<RectTransform, PositionRestState> _positionRest = new Dictionary<RectTransform, PositionRestState>();

		public UIAnimationPlaybackController(IUIAnimationPresetResolver resolver, UIAnimationModel model)
		{
			_resolver = resolver;
			_model = model;
		}

		public UniTask PlayAsync(RectTransform target, UIAnimationKey key, CancellationToken ct = default(CancellationToken))
		{
			if (target == null)
			{
				Debug.LogError($"[UIAnimations] PlayAsync called with null target for key {key}.");
				return UniTask.CompletedTask;
			}
			if (!_resolver.TryGet(key, out var preset))
			{
				return UniTask.CompletedTask;
			}
			StopInternal(target, UIAnimationEventReason.Replaced);
			Sequence sequence = BuildSequence(target, preset);
			UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();
			_active[target] = new ActiveEntry
			{
				Sequence = sequence,
				Key = key,
				CompletionSource = uniTaskCompletionSource
			};
			_model.InvokeStarted(new UIAnimationEventData(target, key, UIAnimationEventReason.Completed));
			sequence.OnComplete(delegate
			{
				FireEnded(target, UIAnimationEventReason.Completed);
			});
			sequence.OnKill(delegate
			{
				FireEnded(target, UIAnimationEventReason.Cancelled);
			});
			if (ct.CanBeCanceled)
			{
				ct.Register(delegate
				{
					if (sequence.IsActive())
					{
						sequence.Kill();
					}
				});
			}
			return uniTaskCompletionSource.Task;
		}

		public void Play(RectTransform target, UIAnimationKey key)
		{
			PlayAsync(target, key).Forget();
		}

		public void Stop(RectTransform target)
		{
			if (!(target == null))
			{
				StopInternal(target, UIAnimationEventReason.Cancelled);
			}
		}

		public bool IsPlaying(RectTransform target)
		{
			if (target != null)
			{
				return _active.ContainsKey(target);
			}
			return false;
		}

		private void StopInternal(RectTransform target, UIAnimationEventReason reason)
		{
			if (_active.TryGetValue(target, out var value))
			{
				FireEnded(target, reason);
				value.Sequence.Kill();
			}
		}

		private void FireEnded(RectTransform target, UIAnimationEventReason reason)
		{
			if (_active.TryGetValue(target, out var value) && !value.EndedFired)
			{
				value.EndedFired = true;
				_active[target] = value;
				_model.InvokeEnded(new UIAnimationEventData(target, value.Key, reason));
				value.CompletionSource.TrySetResult();
				_active.Remove(target);
			}
		}

		private Sequence BuildSequence(RectTransform target, UIAnimationPreset preset)
		{
			Sequence sequence = DOTween.Sequence().SetUpdate(preset.UseUnscaledTime).SetTarget(target);
			if (preset.Delay > 0f)
			{
				sequence.SetDelay(preset.Delay);
			}
			if (preset.AnimateAlpha)
			{
				CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = target.gameObject.AddComponent<CanvasGroup>();
				}
				canvasGroup.alpha = preset.AlphaFrom;
				Tween tween = canvasGroup.DOFade(preset.AlphaTo, preset.Duration);
				ApplyEase(tween, preset);
				sequence.Join(tween);
			}
			if (preset.AnimateScale)
			{
				target.localScale = preset.ScaleFrom;
				Tween tween2 = target.DOScale(preset.ScaleTo, preset.Duration);
				ApplyEase(tween2, preset);
				sequence.Join(tween2);
			}
			if (preset.AnimatePosition)
			{
				JoinPositionTween(target, preset, sequence);
			}
			if (preset.AnimateRotation)
			{
				target.localEulerAngles = preset.RotationFrom;
				Tween tween3 = target.DOLocalRotate(preset.RotationTo, preset.Duration, preset.RotateMode);
				ApplyEase(tween3, preset);
				sequence.Join(tween3);
			}
			if (preset.Loops != 0)
			{
				sequence.SetLoops(preset.Loops, preset.LoopType);
			}
			return sequence;
		}

		private void JoinPositionTween(RectTransform target, UIAnimationPreset preset, Sequence sequence)
		{
			PositionRestState orCapturePositionRest = GetOrCapturePositionRest(target);
			bool flag = !Mathf.Approximately(target.anchorMin.x, target.anchorMax.x);
			bool flag2 = !Mathf.Approximately(target.anchorMin.y, target.anchorMax.y);
			if (flag || flag2)
			{
				Vector2 offsetMin = orCapturePositionRest.OffsetMin;
				Vector2 offsetMax = orCapturePositionRest.OffsetMax;
				Vector2 offsetMin2 = orCapturePositionRest.OffsetMin;
				Vector2 offsetMax2 = orCapturePositionRest.OffsetMax;
				if (flag)
				{
					offsetMin.x += preset.PositionFromOffset.x;
					offsetMax.x -= preset.PositionFromOffset.x;
					offsetMin2.x += preset.PositionToOffset.x;
					offsetMax2.x -= preset.PositionToOffset.x;
				}
				if (flag2)
				{
					offsetMin.y += preset.PositionFromOffset.y;
					offsetMax.y -= preset.PositionFromOffset.y;
					offsetMin2.y += preset.PositionToOffset.y;
					offsetMax2.y -= preset.PositionToOffset.y;
				}
				target.offsetMin = offsetMin;
				target.offsetMax = offsetMax;
				Tween tween = DOTween.To(() => target.offsetMin, delegate(Vector2 value)
				{
					target.offsetMin = value;
				}, offsetMin2, preset.Duration);
				Tween tween2 = DOTween.To(() => target.offsetMax, delegate(Vector2 value)
				{
					target.offsetMax = value;
				}, offsetMax2, preset.Duration);
				ApplyEase(tween, preset);
				ApplyEase(tween2, preset);
				sequence.Join(tween);
				sequence.Join(tween2);
			}
			else
			{
				Vector2 anchoredPosition = orCapturePositionRest.AnchoredPosition + preset.PositionFromOffset;
				Vector2 endValue = orCapturePositionRest.AnchoredPosition + preset.PositionToOffset;
				target.anchoredPosition = anchoredPosition;
				Tween tween3 = target.DOAnchorPos(endValue, preset.Duration);
				ApplyEase(tween3, preset);
				sequence.Join(tween3);
			}
		}

		private PositionRestState GetOrCapturePositionRest(RectTransform target)
		{
			if (_positionRest.TryGetValue(target, out var value))
			{
				return value;
			}
			value = new PositionRestState
			{
				AnchoredPosition = target.anchoredPosition,
				OffsetMin = target.offsetMin,
				OffsetMax = target.offsetMax
			};
			_positionRest[target] = value;
			return value;
		}

		private void ApplyEase(Tween tween, UIAnimationPreset preset)
		{
			if (preset.CustomCurve != null && preset.CustomCurve.length > 0)
			{
				tween.SetEase(preset.CustomCurve);
			}
			else
			{
				tween.SetEase(preset.Ease);
			}
		}
	}
}

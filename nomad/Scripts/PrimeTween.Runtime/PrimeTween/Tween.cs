using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PrimeTween
{
	public readonly struct Tween : IEnumerator, IEquatable<Tween>
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This struct is needed for async/await support, you should not use it directly.")]
		public readonly struct TweenAwaiter : INotifyCompletion
		{
			private readonly Tween tween;

			public bool IsCompleted => !tween.isAlive;

			internal TweenAwaiter(Tween tween)
			{
				if (tween.isAlive && !tween.TryManipulate())
				{
					this.tween = default(Tween);
				}
				else
				{
					this.tween = tween;
				}
			}

			public void OnCompleted([NotNull] Action continuation)
			{
				try
				{
					TweenSettings<float> settings = new TweenSettings<float>(0f, 0f, 3.4028235E+38f, Ease.Linear, -1);
					Tween obj = animate(tween.tween, ref settings, TweenAnimation.TweenType.TweenAwaiter);
					obj.tween.longParam = tween.id;
					obj.tween.managedData.OnComplete(continuation, true);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					throw;
				}
			}

			internal static void UpdateTweenAwaiter(ref TweenData rt, ref UnmanagedTweenData d)
			{
				if (d.isAlive)
				{
					ColdData coldData = rt.target as ColdData;
					if (rt.cold.longParam != coldData.id || !coldData.data.isAlive)
					{
						rt.ForceComplete(ref d);
					}
				}
			}

			public void GetResult()
			{
			}
		}

		private const string minMaxExpected = "numMinExpected/numMaxExpected parameters are no longer supported.";

		private const string localScaleRenamed = "please use 'Scale' instead of 'LocalScale'.";

		private static readonly Dictionary<int, int> textureIdTo_ST = new Dictionary<int, int>(10);

		internal readonly long id;

		internal readonly ColdData tween;

		object IEnumerator.Current => null;

		internal bool IsCreated => id != 0;

		public bool isAlive
		{
			get
			{
				if (id != 0L && tween.id == id && tween.hasData)
				{
					return tween.data.isAlive;
				}
				return false;
			}
		}

		public float elapsedTime
		{
			get
			{
				if (!ValidateIsAlive())
				{
					return 0f;
				}
				if (cyclesDone == cyclesTotal)
				{
					return duration;
				}
				float num = elapsedTimeTotal - duration * (float)cyclesDone;
				if (num < 0f)
				{
					return 0f;
				}
				return num;
			}
		}

		public int cyclesTotal
		{
			get
			{
				if (!ValidateIsAlive())
				{
					return 0;
				}
				return tween.data.cyclesTotal;
			}
		}

		public int cyclesDone
		{
			get
			{
				if (!ValidateIsAlive())
				{
					return 0;
				}
				return tween.data.getCyclesDone();
			}
		}

		public float duration
		{
			get
			{
				if (!ValidateIsAlive())
				{
					return 0f;
				}
				float f = tween.data.cycleDuration;
				TweenSettings.validateFiniteDuration(ref f);
				return f;
			}
		}

		public float elapsedTimeTotal
		{
			get
			{
				if (!ValidateIsAlive())
				{
					return 0f;
				}
				if (tween.data.elapsedTimeTotal == 3.4028235E+38f)
				{
					return durationTotal;
				}
				return Mathf.Clamp(tween.data.elapsedTimeTotal - tween.data.waitDelay, 0f, durationTotal);
			}
		}

		public float durationTotal
		{
			get
			{
				if (!ValidateIsAlive())
				{
					return 0f;
				}
				int num = tween.data.cyclesTotal;
				if (num == -1)
				{
					return 1f / 0f;
				}
				return tween.data.cycleDuration * (float)num;
			}
		}

		public float progressTotal
		{
			get
			{
				if (!ValidateIsAlive())
				{
					return 0f;
				}
				if (cyclesTotal == -1)
				{
					return 0f;
				}
				float num = durationTotal;
				if (num == 0f)
				{
					return GetProgressFromState();
				}
				return Mathf.Min(elapsedTimeTotal / num, 1f);
			}
			set
			{
				if (!ValidateIsAlive())
				{
					return;
				}
				if (cyclesTotal == -1)
				{
					Debug.LogError("It's not allowed to set progressTotal on infinite tween (cyclesTotal == -1), tween: " + ToString() + ".");
					return;
				}
				value = Mathf.Clamp01(value);
				if (value == 1f)
				{
					SetElapsedTimeTotal(3.4028235E+38f);
				}
				else
				{
					SetElapsedTimeTotal(value * durationTotal);
				}
			}
		}

		public bool isPaused
		{
			get
			{
				if (TryManipulate())
				{
					return tween.data.isPaused;
				}
				return false;
			}
			set
			{
				if (!TryManipulate())
				{
					return;
				}
				ref TweenData managedData = ref tween.managedData;
				ref UnmanagedTweenData data = ref tween.data;
				if (data.trySetPause(value) && !value && ((timeScale > 0f && progressTotal >= 1f) || (timeScale < 0f && progressTotal == 0f)))
				{
					if (data.IsMainSequenceRoot())
					{
						new Sequence(managedData.cold.sequence).ReleaseTweens();
					}
					else
					{
						managedData.Kill(ref data);
					}
				}
			}
		}

		public float timeScale
		{
			get
			{
				if (!TryManipulate())
				{
					return 1f;
				}
				return tween.data.timeScale;
			}
		}

		internal float durationWithWaitDelay => tween.data.calcDurationWithWaitDependencies();

		[NotNull]
		public IEnumerator ToYieldInstruction()
		{
			if (!isAlive || !TryManipulate())
			{
				return Array.Empty<object>().GetEnumerator();
			}
			List<CoroutineIterator> coroutineIterators = PrimeTweenManager.Instance._coroutineIterators;
			CoroutineIterator coroutineIterator;
			if (coroutineIterators.Count > 0)
			{
				coroutineIterator = coroutineIterators[coroutineIterators.Count - 1];
				coroutineIterators.RemoveAt(coroutineIterators.Count - 1);
			}
			else
			{
				coroutineIterator = new CoroutineIterator();
			}
			coroutineIterator._tween = this;
			return coroutineIterator;
		}

		bool IEnumerator.MoveNext()
		{
			return isAlive;
		}

		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		public static Tween LightIntensity([NotNull] Light target, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LightIntensity(target, new TweenSettings<float>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween LightIntensity([NotNull] Light target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.LightIntensity);
		}

		public static Tween LightColor([NotNull] Light target, TweenSettings<Color> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.LightColor);
		}

		public static Tween CameraBackgroundColor([NotNull] Camera target, TweenSettings<Color> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.CameraBackgroundColor);
		}

		public static Tween LocalRotation([NotNull] Transform target, Vector3 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LocalRotation(target, new TweenSettings<Vector3>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Position([NotNull] Transform target, Vector3 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Position(target, new TweenSettings<Vector3>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Position([NotNull] Transform target, TweenSettings<Vector3> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.Position);
		}

		public static Tween LocalPosition([NotNull] Transform target, Vector3 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LocalPosition(target, new TweenSettings<Vector3>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween LocalPosition([NotNull] Transform target, TweenSettings<Vector3> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.LocalPosition);
		}

		public static Tween LocalPositionY([NotNull] Transform target, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LocalPositionY(target, new TweenSettings<float>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween LocalPositionY([NotNull] Transform target, float startValue, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LocalPositionY(target, new TweenSettings<float>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween LocalPositionY([NotNull] Transform target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.LocalPositionY);
		}

		public static Tween LocalPositionZ([NotNull] Transform target, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LocalPositionZ(target, new TweenSettings<float>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween LocalPositionZ([NotNull] Transform target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.LocalPositionZ);
		}

		public static Tween Rotation([NotNull] Transform target, Quaternion endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Rotation(target, new TweenSettings<Quaternion>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Rotation([NotNull] Transform target, TweenSettings<Quaternion> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.RotationQuaternion);
		}

		public static Tween LocalRotation([NotNull] Transform target, Quaternion endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LocalRotation(target, new TweenSettings<Quaternion>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween LocalRotation([NotNull] Transform target, TweenSettings<Quaternion> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.LocalRotationQuaternion);
		}

		public static Tween Scale([NotNull] Transform target, Vector3 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Scale(target, new TweenSettings<Vector3>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Scale([NotNull] Transform target, Vector3 startValue, Vector3 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Scale(target, new TweenSettings<Vector3>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Scale([NotNull] Transform target, TweenSettings<Vector3> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.Scale);
		}

		public static Tween Color([NotNull] Graphic target, Color endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Color(target, new TweenSettings<Color>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Color([NotNull] Graphic target, Color startValue, Color endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Color(target, new TweenSettings<Color>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Color([NotNull] Graphic target, TweenSettings<Color> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UIColorGraphic);
		}

		public static Tween UIAnchoredPosition([NotNull] RectTransform target, Vector2 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return UIAnchoredPosition(target, new TweenSettings<Vector2>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween UIAnchoredPosition([NotNull] RectTransform target, TweenSettings<Vector2> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UIAnchoredPosition);
		}

		public static Tween UIAnchoredPositionX([NotNull] RectTransform target, float startValue, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return UIAnchoredPositionX(target, new TweenSettings<float>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween UIAnchoredPositionX([NotNull] RectTransform target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UIAnchoredPositionX);
		}

		public static Tween UIAnchoredPositionY([NotNull] RectTransform target, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return UIAnchoredPositionY(target, new TweenSettings<float>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween UIAnchoredPositionY([NotNull] RectTransform target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UIAnchoredPositionY);
		}

		public static Tween UISizeDelta([NotNull] RectTransform target, Vector2 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return UISizeDelta(target, new TweenSettings<Vector2>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween UISizeDelta([NotNull] RectTransform target, TweenSettings<Vector2> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UISizeDelta);
		}

		public static Tween Alpha([NotNull] CanvasGroup target, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Alpha(target, new TweenSettings<float>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Alpha([NotNull] CanvasGroup target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UIAlphaCanvasGroup);
		}

		public static Tween Alpha([NotNull] Graphic target, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Alpha(target, new TweenSettings<float>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Alpha([NotNull] Graphic target, float startValue, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Alpha(target, new TweenSettings<float>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Alpha([NotNull] Graphic target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UIAlphaGraphic);
		}

		public static Tween UIFillAmount([NotNull] Image target, float endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return UIFillAmount(target, new TweenSettings<float>(endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween UIFillAmount([NotNull] Image target, TweenSettings<float> settings)
		{
			return animate(target, ref settings, TweenAnimation.TweenType.UIFillAmount);
		}

		public static Tween TextMaxVisibleCharacters([NotNull] TMP_Text target, int startValue, int endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return TextMaxVisibleCharacters(target, new TweenSettings<int>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween Custom(float startValue, float endValue, float duration, [NotNull] Action<float> onValueChange, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return Custom(new TweenSettings<float>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime), onValueChange);
		}

		public static Tween Custom<T>([NotNull] T target, float startValue, float endValue, float duration, [NotNull] Action<T, float> onValueChange, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false) where T : class
		{
			return Custom_internal(target, new TweenSettings<float>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime), onValueChange);
		}

		public static Tween Custom<T>([NotNull] T target, TweenSettings<float> settings, [NotNull] Action<T, float> onValueChange) where T : class
		{
			return Custom_internal(target, settings, onValueChange);
		}

		public static Tween Custom(TweenSettings<float> settings, [NotNull] Action<float> onValueChange)
		{
			if (settings.startFromCurrent)
			{
				Debug.LogWarning("Custom tweens don't support the 'startFromCurrent' because they don't know the current value of animated property.\nThis means that the animated value will be changed abruptly if a new tween is started mid-way.\nPlease pass the current value to the 'T.WithDirection(bool toEndValue, T currentValue)' method or use the constructor that accepts the 'startValue'.\n");
			}
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref TweenData managedData = ref coldData.managedData;
			ref UnmanagedTweenData data = ref coldData.data;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.customOnValueChange = onValueChange;
			coldData.Setup(PrimeTweenManager.dummyTarget, ref settings.settings, _startFromCurrent: false, TweenAnimation.TweenType.CustomFloat, ref managedData, ref data);
			coldData.onValueChange = delegate(ref TweenData rt2, ref UnmanagedTweenData d2)
			{
				if (d2.isUpdating)
				{
					Debug.LogError("Please don't call this API recursively from Tween.Custom() or tween.OnUpdate().");
					return;
				}
				d2.isUpdating = true;
				TweenAnimation.ValueWrapper startValue = d2.startValue;
				float easedInterpolationFactor = d2.easedInterpolationFactor;
				TweenAnimation.ValueWrapper endValueOrDiff = rt2.endValueOrDiff;
				float obj = TweenData.FloatVal(startValue, easedInterpolationFactor, endValueOrDiff);
				Action<float> action = rt2.cold.customOnValueChange as Action<float>;
				try
				{
					action(obj);
				}
				finally
				{
					d2.isUpdating = false;
				}
			};
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween Custom_internal<T>([NotNull] T target, TweenSettings<float> settings, [NotNull] Action<T, float> onValueChange, bool isAdditive = false) where T : class
		{
			if (settings.startFromCurrent)
			{
				Debug.LogWarning("Custom tweens don't support the 'startFromCurrent' because they don't know the current value of animated property.\nThis means that the animated value will be changed abruptly if a new tween is started mid-way.\nPlease pass the current value to the 'T.WithDirection(bool toEndValue, T currentValue)' method or use the constructor that accepts the 'startValue'.\n");
			}
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref TweenData managedData = ref coldData.managedData;
			ref UnmanagedTweenData data = ref coldData.data;
			data.startValue.CopyFrom(ref settings.startValue);
			data.isAdditive = isAdditive;
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.customOnValueChange = onValueChange;
			coldData.Setup(target, ref settings.settings, _startFromCurrent: false, TweenAnimation.TweenType.CustomFloat, ref managedData, ref data);
			coldData.onValueChange = delegate(ref TweenData rt2, ref UnmanagedTweenData d2)
			{
				if (d2.isUpdating)
				{
					Debug.LogError("Please don't call this API recursively from Tween.Custom() or tween.OnUpdate().");
					return;
				}
				d2.isUpdating = true;
				TweenAnimation.ValueWrapper startValue = d2.startValue;
				float easedInterpolationFactor = d2.easedInterpolationFactor;
				bool isAdditive2 = d2.isAdditive;
				T arg = rt2.target as T;
				TweenAnimation.ValueWrapper endValueOrDiff = rt2.endValueOrDiff;
				float arg2;
				if (isAdditive2)
				{
					float num = TweenData.FloatVal(startValue, easedInterpolationFactor, endValueOrDiff);
					arg2 = num.calcDelta(rt2.cold.prevVal);
					rt2.cold.prevVal.single = num;
				}
				else
				{
					arg2 = TweenData.FloatVal(startValue, easedInterpolationFactor, endValueOrDiff);
				}
				Action<T, float> action = rt2.cold.customOnValueChange as Action<T, float>;
				try
				{
					action(arg, arg2);
				}
				finally
				{
					d2.isUpdating = false;
				}
			};
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween animate(object target, ref TweenSettings<float> settings, TweenAnimation.TweenType _tweenType)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref UnmanagedTweenData data = ref coldData.data;
			ref TweenData managedData = ref coldData.managedData;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.Setup(target, ref settings.settings, settings.startFromCurrent, _tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		public static Tween Custom<T>([NotNull] T target, Color startValue, Color endValue, float duration, [NotNull] Action<T, Color> onValueChange, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false) where T : class
		{
			return Custom_internal(target, new TweenSettings<Color>(startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime), onValueChange);
		}

		public static Tween Custom(TweenSettings<Color> settings, [NotNull] Action<Color> onValueChange)
		{
			if (settings.startFromCurrent)
			{
				Debug.LogWarning("Custom tweens don't support the 'startFromCurrent' because they don't know the current value of animated property.\nThis means that the animated value will be changed abruptly if a new tween is started mid-way.\nPlease pass the current value to the 'T.WithDirection(bool toEndValue, T currentValue)' method or use the constructor that accepts the 'startValue'.\n");
			}
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref TweenData managedData = ref coldData.managedData;
			ref UnmanagedTweenData data = ref coldData.data;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.customOnValueChange = onValueChange;
			coldData.Setup(PrimeTweenManager.dummyTarget, ref settings.settings, _startFromCurrent: false, TweenAnimation.TweenType.CustomColor, ref managedData, ref data);
			coldData.onValueChange = delegate(ref TweenData rt2, ref UnmanagedTweenData d2)
			{
				if (d2.isUpdating)
				{
					Debug.LogError("Please don't call this API recursively from Tween.Custom() or tween.OnUpdate().");
					return;
				}
				d2.isUpdating = true;
				TweenAnimation.ValueWrapper startValue = d2.startValue;
				float easedInterpolationFactor = d2.easedInterpolationFactor;
				TweenAnimation.ValueWrapper endValueOrDiff = rt2.endValueOrDiff;
				Color obj = TweenData.ColorVal(startValue, easedInterpolationFactor, endValueOrDiff);
				Action<Color> action = rt2.cold.customOnValueChange as Action<Color>;
				try
				{
					action(obj);
				}
				finally
				{
					d2.isUpdating = false;
				}
			};
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween Custom_internal<T>([NotNull] T target, TweenSettings<Color> settings, [NotNull] Action<T, Color> onValueChange, bool isAdditive = false) where T : class
		{
			if (settings.startFromCurrent)
			{
				Debug.LogWarning("Custom tweens don't support the 'startFromCurrent' because they don't know the current value of animated property.\nThis means that the animated value will be changed abruptly if a new tween is started mid-way.\nPlease pass the current value to the 'T.WithDirection(bool toEndValue, T currentValue)' method or use the constructor that accepts the 'startValue'.\n");
			}
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref TweenData managedData = ref coldData.managedData;
			ref UnmanagedTweenData data = ref coldData.data;
			data.startValue.CopyFrom(ref settings.startValue);
			data.isAdditive = isAdditive;
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.customOnValueChange = onValueChange;
			coldData.Setup(target, ref settings.settings, _startFromCurrent: false, TweenAnimation.TweenType.CustomColor, ref managedData, ref data);
			coldData.onValueChange = delegate(ref TweenData rt2, ref UnmanagedTweenData d2)
			{
				if (d2.isUpdating)
				{
					Debug.LogError("Please don't call this API recursively from Tween.Custom() or tween.OnUpdate().");
					return;
				}
				d2.isUpdating = true;
				TweenAnimation.ValueWrapper startValue = d2.startValue;
				float easedInterpolationFactor = d2.easedInterpolationFactor;
				bool isAdditive2 = d2.isAdditive;
				T arg = rt2.target as T;
				TweenAnimation.ValueWrapper endValueOrDiff = rt2.endValueOrDiff;
				Color arg2;
				if (isAdditive2)
				{
					Color color = TweenData.ColorVal(startValue, easedInterpolationFactor, endValueOrDiff);
					arg2 = color.calcDelta(rt2.cold.prevVal);
					rt2.cold.prevVal.color = color;
				}
				else
				{
					arg2 = TweenData.ColorVal(startValue, easedInterpolationFactor, endValueOrDiff);
				}
				Action<T, Color> action = rt2.cold.customOnValueChange as Action<T, Color>;
				try
				{
					action(arg, arg2);
				}
				finally
				{
					d2.isUpdating = false;
				}
			};
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween animate(object target, ref TweenSettings<Color> settings, TweenAnimation.TweenType _tweenType)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref UnmanagedTweenData data = ref coldData.data;
			ref TweenData managedData = ref coldData.managedData;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.Setup(target, ref settings.settings, settings.startFromCurrent, _tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween AnimateMaterial([CanBeNull] object target, long longParam, ref TweenSettings<Color> settings, TweenAnimation.TweenType _tweenType)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref UnmanagedTweenData data = ref coldData.data;
			ref TweenData managedData = ref coldData.managedData;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.longParam = longParam;
			coldData.Setup(target, ref settings.settings, settings.startFromCurrent, _tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween animate(object target, ref TweenSettings<Vector2> settings, TweenAnimation.TweenType _tweenType)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref UnmanagedTweenData data = ref coldData.data;
			ref TweenData managedData = ref coldData.managedData;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.Setup(target, ref settings.settings, settings.startFromCurrent, _tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween animate(object target, ref TweenSettings<Vector3> settings, TweenAnimation.TweenType _tweenType)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref UnmanagedTweenData data = ref coldData.data;
			ref TweenData managedData = ref coldData.managedData;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.Setup(target, ref settings.settings, settings.startFromCurrent, _tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static Tween animate(object target, ref TweenSettings<Quaternion> settings, TweenAnimation.TweenType _tweenType)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref UnmanagedTweenData data = ref coldData.data;
			ref TweenData managedData = ref coldData.managedData;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.Setup(target, ref settings.settings, settings.startFromCurrent, _tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		public static int GetTweensCount([CanBeNull] object onTarget = null)
		{
			PrimeTweenManager instance = PrimeTweenManager.Instance;
			if (onTarget == null && instance.updateDepth == 0)
			{
				return instance.tweensCount;
			}
			return PrimeTweenManager.ProcessAll(onTarget, (ColdData _) => true, allowToProcessTweensInsideSequence: true);
		}

		public static int StopAll([CanBeNull] object onTarget = null)
		{
			int result = PrimeTweenManager.ProcessAll(onTarget, delegate(ColdData tween)
			{
				ref UnmanagedTweenData data = ref tween.data;
				if (data.isInSequence)
				{
					if (data.IsMainSequenceRoot())
					{
						new Sequence(tween.sequence).Stop();
					}
				}
				else
				{
					tween.managedData.Kill(ref data);
				}
				return true;
			}, allowToProcessTweensInsideSequence: false);
			ForceUpdateManagerIfTargetIsNull(onTarget);
			return result;
		}

		public static int CompleteAll([CanBeNull] object onTarget = null)
		{
			PrimeTweenManager instance = PrimeTweenManager.Instance;
			if (instance.updateDepth != 0)
			{
				instance.completeAllRequested = true;
				instance.completeAllRequestedTarget = onTarget;
				if (onTarget != null)
				{
					return GetTweensCount(onTarget);
				}
				return instance.tweensCount;
			}
			instance.AddNewTweens(_UpdateType.Update);
			instance.AddNewTweens(_UpdateType.LateUpdate);
			instance.AddNewTweens(_UpdateType.FixedUpdate);
			int result = PrimeTweenManager.ProcessAll(onTarget, delegate(ColdData tween)
			{
				ref UnmanagedTweenData data = ref tween.data;
				if (data.isInSequence)
				{
					if (data.IsMainSequenceRoot())
					{
						new Sequence(tween.sequence).Complete();
					}
				}
				else
				{
					new Tween(tween).Complete();
				}
				return true;
			}, allowToProcessTweensInsideSequence: false);
			ForceUpdateManagerIfTargetIsNull(onTarget);
			return result;
		}

		private static void ForceUpdateManagerIfTargetIsNull([CanBeNull] object onTarget)
		{
			if (onTarget == null)
			{
				PrimeTweenManager instance = PrimeTweenManager.Instance;
				if (instance != null && instance.updateDepth == 0)
				{
					instance.UpdateTweens(_UpdateType.Update, 0f, 0f);
					instance.UpdateTweens(_UpdateType.LateUpdate, 0f, 0f);
					instance.UpdateTweens(_UpdateType.FixedUpdate, 0f, 0f);
				}
			}
		}

		public static Tween Delay(float duration, [CanBeNull] Action onComplete = null, bool useUnscaledTime = false, bool? warnIfTargetDestroyed = null)
		{
			return DelayInternal(PrimeTweenManager.dummyTarget, duration, onComplete, useUnscaledTime, warnIfTargetDestroyed);
		}

		private static Tween DelayInternal([CanBeNull] object target, float duration, [CanBeNull] Action onComplete, bool useUnscaledTime, bool? warnIfTargetDestroyed)
		{
			Tween? tween = delay_internal(target, duration, useUnscaledTime);
			if (onComplete != null)
			{
				tween?.tween.managedData.OnComplete(onComplete, warnIfTargetDestroyed);
			}
			return tween.GetValueOrDefault();
		}

		public static Tween Delay<T>([NotNull] T target, float duration, [NotNull] Action<T> onComplete, bool useUnscaledTime = false, bool? warnIfTargetDestroyed = null) where T : class
		{
			Tween? tween = delay_internal(target, duration, useUnscaledTime);
			if (!tween.HasValue)
			{
				return default(Tween);
			}
			Tween value = tween.Value;
			value.tween.managedData.OnComplete(target, onComplete, warnIfTargetDestroyed);
			return value;
		}

		private static Tween? delay_internal([CanBeNull] object target, float duration, bool useUnscaledTime)
		{
			PrimeTweenManager.CheckDuration(target, duration);
			return PrimeTweenManager.DelayWithoutDurationCheck(target, duration, useUnscaledTime);
		}

		public static Tween MaterialColor([NotNull] Material target, int propertyId, Color endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return MaterialColor(target, propertyId, new TweenSettings<Color>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
		}

		public static Tween MaterialColor([NotNull] Material target, int propertyId, TweenSettings<Color> settings)
		{
			return AnimateMaterial(target, propertyId, ref settings, TweenAnimation.TweenType.MaterialColorProperty);
		}

		public static Tween LocalEulerAngles([NotNull] Transform target, Vector3 startValue, Vector3 endValue, float duration, Easing ease = default(Easing), int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return LocalEulerAngles(target, new TweenSettings<Vector3>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
		}

		public static Tween LocalEulerAngles([NotNull] Transform target, TweenSettings<Vector3> settings)
		{
			ValidateEulerAnglesData(ref settings);
			return animate(target, ref settings, TweenAnimation.TweenType.LocalEulerAngles);
		}

		private static void ValidateEulerAnglesData(ref TweenSettings<Vector3> settings)
		{
			if (settings.startFromCurrent)
			{
				settings.startFromCurrent = false;
				Debug.LogWarning("Animating euler angles from the current value may produce unexpected results because there is more than one way to represent the current rotation using Euler angles.\n'startFromCurrent' was ignored.\nMore info: https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html\n");
			}
		}

		public static Tween LocalRotation([NotNull] Transform target, TweenSettings<Vector3> localEulerAnglesSettings)
		{
			return LocalRotation(target, ToQuaternion(localEulerAnglesSettings));
		}

		private static TweenSettings<Quaternion> ToQuaternion(TweenSettings<Vector3> s)
		{
			TweenSettings<Quaternion> result = new TweenSettings<Quaternion>(Quaternion.Euler(s.startValue), Quaternion.Euler(s.endValue), s.settings);
			result.startFromCurrent = s.startFromCurrent;
			return result;
		}

		public static Tween TextMaxVisibleCharacters([NotNull] TMP_Text target, TweenSettings<int> settings)
		{
			int characterCount = target.textInfo.characterCount;
			target.ForceMeshUpdate();
			if (characterCount != target.textInfo.characterCount)
			{
				Debug.LogWarning("Please call TMP_Text.ForceMeshUpdate() before animating maxVisibleCharacters.");
			}
			TweenSettings<float> settings2 = new TweenSettings<float>(settings.startValue, settings.endValue, settings.settings);
			return AnimateIntAsFloat(target, ref settings2, TweenAnimation.TweenType.TextMaxVisibleCharacters);
		}

		private static Tween AnimateIntAsFloat(object target, ref TweenSettings<float> settings, TweenAnimation.TweenType tweenType)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings.settings._updateType);
			ref TweenData managedData = ref coldData.managedData;
			ref UnmanagedTweenData data = ref coldData.data;
			data.startValue.CopyFrom(ref settings.startValue);
			managedData.endValueOrDiff.CopyFrom(ref settings.endValue);
			coldData.Setup(target, ref settings.settings, settings.startFromCurrent, tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		public static Sequence ShakeCamera([NotNull] Camera camera, float strengthFactor, float duration = 0.5f, float frequency = 10f, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			Transform transform = camera.transform;
			if (camera.orthographic)
			{
				float num = strengthFactor * camera.orthographicSize * 0.03f;
				return Sequence.Create().Group(ShakeLocalPosition(transform, new ShakeSettings(new Vector3(num, num), duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime))).Group(ShakeLocalRotation(transform, new ShakeSettings(new Vector3(0f, 0f, strengthFactor * 0.6f), duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)));
			}
			return Sequence.Create().Group(ShakeLocalRotation(transform, new ShakeSettings(strengthFactor * Vector3.one, duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)));
		}

		public static Tween ShakeLocalPosition([NotNull] Transform target, Vector3 strength, float duration, float frequency = 10f, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return ShakeLocalPosition(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween ShakeLocalPosition([NotNull] Transform target, ShakeSettings settings)
		{
			return ShakeTransform(TweenAnimation.TweenType.ShakeLocalPosition, target, settings);
		}

		public static Tween ShakeLocalRotation([NotNull] Transform target, Vector3 strength, float duration, float frequency = 10f, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return ShakeLocalRotation(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween ShakeLocalRotation([NotNull] Transform target, ShakeSettings settings)
		{
			return ShakeTransform(TweenAnimation.TweenType.ShakeLocalRotation, target, settings);
		}

		public static Tween ShakeScale([NotNull] Transform target, ShakeSettings settings)
		{
			return ShakeTransform(TweenAnimation.TweenType.ShakeScale, target, settings);
		}

		public static Tween PunchScale([NotNull] Transform target, Vector3 strength, float duration, float frequency = 10f, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
		{
			return PunchScale(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
		}

		public static Tween PunchScale([NotNull] Transform target, ShakeSettings settings)
		{
			return ShakeScale(target, settings.WithPunch());
		}

		private static Tween ShakeTransform(TweenAnimation.TweenType tweenType, [NotNull] Transform target, ShakeSettings settings)
		{
			if (PrimeTweenManager.Instance.isDestroyed)
			{
				return default(Tween);
			}
			ColdData coldData = PrimeTweenManager.FetchTween(settings._updateType);
			ref TweenData managedData = ref coldData.managedData;
			ref UnmanagedTweenData data = ref coldData.data;
			prepareShakeData(settings, ref managedData, ref data, target);
			TweenSettings _settings = settings.tweenSettings;
			coldData.Setup(target, ref _settings, _startFromCurrent: true, tweenType, ref managedData, ref data);
			return PrimeTweenManager.Animate(ref managedData, ref data);
		}

		private static void prepareShakeData(ShakeSettings settings, ref TweenData rt, ref UnmanagedTweenData d, object target)
		{
			rt.endValueOrDiff.Reset();
			rt.cold.shakeData.Setup(settings, ref rt, ref d, target);
		}

		internal static Vector3 getShakeVal(ref TweenData rt, ref UnmanagedTweenData d)
		{
			float num = calcFadeInOutFactor(ref rt, ref d);
			return rt.shakeData.getNextVal(ref rt, ref d) * num;
		}

		private static float calcFadeInOutFactor(ref TweenData tween, ref UnmanagedTweenData d)
		{
			float animationDuration = d.animationDuration;
			float num = d.easedInterpolationFactor * animationDuration;
			if (animationDuration == 0f)
			{
				return 0f;
			}
			float num2 = animationDuration * 0.5f;
			float num3 = 1f / tween.cold.shakeData.frequency;
			if (num3 > num2)
			{
				num3 = num2;
			}
			float num4 = num3 * 0.5f;
			if (num < num4)
			{
				return Mathf.InverseLerp(0f, num4, num);
			}
			float num5 = animationDuration - num3;
			if (num > num5)
			{
				return Mathf.InverseLerp(animationDuration, num5, num);
			}
			return 1f;
		}

		internal Tween([NotNull] ColdData tween)
		{
			id = tween.id;
			this.tween = tween;
		}

		[NotNull]
		public override string ToString()
		{
			if (isAlive && tween.hasData)
			{
				return tween.managedData.GetDescription();
			}
			return $"DEAD / id {id}";
		}

		private void SetElapsedTimeTotal(float value)
		{
			if (!TryManipulate())
			{
				return;
			}
			if (value < 0f || float.IsNaN(value) || (cyclesTotal == -1 && value >= 3.4028235E+38f))
			{
				Debug.LogError($"Invalid elapsedTimeTotal value: {value}, tween: {ToString()}");
				return;
			}
			ref TweenData managedData = ref tween.managedData;
			ref UnmanagedTweenData data = ref tween.data;
			managedData.SetElapsedTimeTotal(value, earlyExitSequenceIfPaused: false, ref data);
			if (data.isAlive)
			{
				float num = durationTotal;
				if (value > num)
				{
					data.elapsedTimeTotal = num;
				}
			}
		}

		private float GetProgressFromState()
		{
			if ((tween.data.flags & Flags.StateAfter) == 0)
			{
				return 0f;
			}
			return 1f;
		}

		public void Stop()
		{
			if (isAlive && TryManipulate(checkRecursive: false))
			{
				tween.managedData.Kill(ref tween.data);
			}
		}

		public void Complete()
		{
			if (isAlive && TryManipulate(checkRecursive: false))
			{
				tween.managedData.ForceComplete(ref tween.data);
			}
		}

		internal bool TryManipulate(bool checkRecursive = true)
		{
			if (!ValidateIsAlive())
			{
				return false;
			}
			ref UnmanagedTweenData data = ref tween.data;
			if (!data.canManipulate())
			{
				tween.managedData.LogErrorWithStackTrace("It's not allowed to manipulate 'nested' animations, please use the parent Sequence instead.\nWhen an animation is added to another sequence, it becomes 'nested', and manipulating it directly is no longer allowed.\nUse Stop()/Complete()/isPaused/timeScale/elapsedTime/etc. of the parent animation instead.\n");
				return false;
			}
			if (data.isInSequence)
			{
				if (checkRecursive)
				{
					Sequence.SequenceChildrenEnumerator enumerator = new Sequence(tween).GetAllTweens().GetEnumerator();
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.data.isUpdating)
						{
							Debug.LogError("Please don't call this API recursively from Tween.Custom() or tween.OnUpdate().");
							return false;
						}
					}
				}
				else
				{
					Sequence.SequenceChildrenEnumerator enumerator = new Sequence(tween).GetAllTweens().GetEnumerator();
					while (enumerator.MoveNext())
					{
						enumerator.Current.data.isUpdating = false;
					}
				}
			}
			else if (checkRecursive)
			{
				if (data.isUpdating)
				{
					Debug.LogError("Please don't call this API recursively from Tween.Custom() or tween.OnUpdate().");
					return false;
				}
			}
			else
			{
				data.isUpdating = false;
			}
			return true;
		}

		public void SetRemainingCycles(bool stopAtEndValue)
		{
			if (TryManipulate())
			{
				ref UnmanagedTweenData data = ref tween.data;
				if (data.cycleMode == CycleMode.Restart || data.cycleMode == CycleMode.Incremental)
				{
					Debug.LogWarning("SetRemainingCycles(bool stopAtEndValue) is meant to be used with CycleMode.Yoyo or Rewind. Please consider using the overload that accepts int instead.");
				}
				bool flag = data.getCyclesDone() % 2 == 0 == stopAtEndValue;
				if (tween.data.isSequenceInverted)
				{
					flag = !flag;
				}
				SetRemainingCycles(flag ? 1 : 2);
			}
		}

		public void SetRemainingCycles(int cycles)
		{
			if (!TryManipulate())
			{
				return;
			}
			ref UnmanagedTweenData data = ref tween.data;
			if (data.tweenType == TweenAnimation.TweenType.Delay && tween.managedData.HasOnComplete)
			{
				Debug.LogError("Applying cycles to Delay will not repeat the OnComplete() callback, but instead will increase the Delay duration.\nOnComplete() is called only once when ALL tween cycles complete. To repeat the OnComplete() callback, please use the Sequence.Create(cycles: numCycles) and put the tween inside a Sequence.\nMore info: https://discussions.unity.com/t/926420/101\n");
			}
			if (cycles == -1)
			{
				if (data.timeScale > 0f)
				{
					data.cyclesTotal = -1;
				}
				else
				{
					Debug.LogError("'SetRemainingCycles()' doesn't work with negative 'timeScale' and infinite(-1) 'cycles'.");
				}
				return;
			}
			TweenSettings.setCyclesTo1If0(ref cycles);
			if (data.timeScale > 0f)
			{
				data.cyclesTotal = data.getCyclesDone() + cycles;
				return;
			}
			int num = cycles - 1;
			data.elapsedTimeTotal = (float)num * data.cycleDuration + elapsedTime;
			data.cyclesDone = num;
			if (data.cyclesTotal < num)
			{
				data.cyclesTotal = num + 1;
			}
		}

		public Tween OnComplete([CanBeNull] Action onComplete, bool? warnIfTargetDestroyed = null)
		{
			if (ValidateIsAlive())
			{
				tween.managedData.OnComplete(onComplete, warnIfTargetDestroyed);
			}
			return this;
		}

		public Tween OnComplete<T>([NotNull] T target, [CanBeNull] Action<T> onComplete, bool? warnIfTargetDestroyed = null) where T : class
		{
			if (ValidateIsAlive())
			{
				tween.managedData.OnComplete(target, onComplete, warnIfTargetDestroyed);
			}
			return this;
		}

		public Sequence Chain(Tween _tween)
		{
			if (!TryManipulate())
			{
				return default(Sequence);
			}
			return Sequence.Create(this).Chain(_tween);
		}

		internal bool ValidateIsAlive()
		{
			if (!IsCreated)
			{
				if (!PrimeTweenManager.Instance.isDestroyed)
				{
					Debug.LogError("Animation is not created. Please check the 'isAlive' property before calling this API.\n- Use 'Sequence.Create()' to start a Sequence.\n- Use static 'Tween.' methods to start a Tween.\n");
				}
			}
			else if (!isAlive)
			{
				Assert.LogErrorWithStackTrace("Animation is not alive. Please check the 'isAlive' property before calling this API.\n", id, null);
			}
			return isAlive;
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public bool Equals(Tween other)
		{
			if (isAlive && other.isAlive)
			{
				return id == other.id;
			}
			return false;
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween
{
	internal class ColdData
	{
		internal TweenArray _tweenArray;

		internal int _index;

		internal ColdData sequence;

		internal ColdData prev;

		internal ColdData next;

		internal ColdData prevSibling;

		internal ColdData nextSibling;

		[CanBeNull]
		internal Action<TweenData> onComplete;

		[CanBeNull]
		internal object onCompleteCallback;

		[CanBeNull]
		internal object onCompleteTarget;

		internal OnValueChangeDelegate onValueChange;

		internal object customOnValueChange;

		internal AnimationCurve customEase;

		internal ParametricEase parametricEase;

		internal float parametricEaseStrength;

		internal float parametricEasePeriod;

		internal long longParam;

		internal TweenAnimation.ValueWrapper prevVal;

		internal ShakeData shakeData;

		[CanBeNull]
		internal object onUpdateTarget;

		internal object onUpdateCallback;

		internal Action<TweenData> onUpdate;

		internal long id = -1L;

		internal bool hasData
		{
			get
			{
				if (_tweenArray != null)
				{
					return _tweenArray.GetData().Length > _index;
				}
				return false;
			}
		}

		internal unsafe ref UnmanagedTweenData data
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref _tweenArray._dataPtr[_index];
			}
		}

		internal ref TweenData managedData
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref _tweenArray[_index];
			}
		}

		internal int intParam
		{
			get
			{
				return (int)longParam;
			}
			set
			{
				longParam = value;
			}
		}

		internal void Setup([CanBeNull] object _target, ref TweenSettings _settings, bool _startFromCurrent, TweenAnimation.TweenType _tweenType, ref TweenData rt, ref UnmanagedTweenData d)
		{
			PrimeTweenManager instance = PrimeTweenManager.Instance;
			PropType item = Utils.TweenTypeToTweenData(_tweenType).Item1;
			d.tweenType = _tweenType;
			if (_settings.ease == Ease.Default)
			{
				_settings.ease = instance.defaultEase;
			}
			else if (_settings.ease == Ease.Custom && _settings.parametricEase == ParametricEase.None)
			{
				AnimationCurve animationCurve = _settings.customEase;
				if (animationCurve != null && TweenSettings.ValidateCustomCurveKeyframes(animationCurve))
				{
					Keyframe keyframe = animationCurve[0];
					Keyframe keyframe2 = animationCurve[animationCurve.length - 1];
					d.IsCustomEaseSameStartEndValues = Mathf.Approximately(keyframe.value, keyframe2.value);
				}
				else
				{
					Debug.LogError("Ease type is Ease.Custom, but customEase is not configured correctly.", _target as UnityEngine.Object);
					_settings.ease = instance.defaultEase;
				}
			}
			Revive(ref d);
			d.flags |= Flags.WarnIgnoredOnCompleteIfTargetDestroyed;
			d.flags &= ~(Flags.StateRunning | Flags.StateAfter);
			d.flags |= Flags.StateBefore;
			d.easedInterpolationFactor = -3.4028235E+38f;
			d.cyclesDone = -1;
			d.timeScale = 1f;
			_settings.SetValidValues();
			d.animationDuration = _settings.duration;
			d.ease = _settings.ease;
			d.cyclesTotal = _settings.cycles;
			d.cycleMode = _settings.cycleMode;
			d.startDelay = _settings.startDelay;
			d.useUnscaledTime = _settings.useUnscaledTime;
			d.startFromCurrent = _startFromCurrent;
			customEase = _settings.customEase;
			parametricEase = _settings.parametricEase;
			parametricEaseStrength = _settings.parametricEaseStrength;
			parametricEasePeriod = _settings.parametricEasePeriod;
			TweenData.CalculateCycleDuration(_settings.endDelay, ref d);
			if (item == PropType.Quaternion)
			{
				prevVal.x = (prevVal.y = (prevVal.z = 0f));
				prevVal.w = 1f;
			}
			else
			{
				prevVal.Reset();
			}
			d.warnEndValueEqualsCurrent = instance.warnEndValueEqualsCurrent;
			if (!_startFromCurrent)
			{
				TweenData.CacheDiff(ref d, ref rt);
			}
			rt.target = _target;
		}

		private void Revive(ref UnmanagedTweenData d)
		{
			d.isAlive = true;
		}
	}
}

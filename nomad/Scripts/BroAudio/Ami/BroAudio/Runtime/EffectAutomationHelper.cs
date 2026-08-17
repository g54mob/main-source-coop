using System;
using System.Collections;
using System.Collections.Generic;
using Ami.Extension;
using UnityEngine;
using UnityEngine.Audio;

namespace Ami.BroAudio.Runtime
{
	public class EffectAutomationHelper : CoroutineBehaviour, IAutoResetWaitable
	{
		private class Tweaker
		{
			public bool IsTweaking;

			public Coroutine Coroutine;

			public List<ITweakingWaitable> WaitableList;
		}

		public interface ITweakingWaitable : IComparable<ITweakingWaitable>
		{
			Effect Effect { get; }

			bool IsFinished();

			IEnumerator GetYieldInstruction();
		}

		private class TweakingWaitableBase : ITweakingWaitable, IComparable<ITweakingWaitable>
		{
			public Effect Effect { get; set; }

			public TweakingWaitableBase(Effect effect)
			{
				Effect = effect;
			}

			public IEnumerator GetYieldInstruction()
			{
				return null;
			}

			public bool IsFinished()
			{
				return false;
			}

			public int CompareTo(ITweakingWaitable other)
			{
				return Effect.CompareTo(other.Effect);
			}
		}

		private abstract class TweakingWaitableDecorator : ITweakingWaitable, IComparable<ITweakingWaitable>
		{
			protected ITweakingWaitable Base;

			public Effect Effect => Base.Effect;

			public void AttachTo(ITweakingWaitable waitable)
			{
				Base = waitable;
			}

			public abstract IEnumerator GetYieldInstruction();

			public abstract bool IsFinished();

			public int CompareTo(ITweakingWaitable other)
			{
				return Base.Effect.CompareTo(other.Effect);
			}
		}

		private class TweakAndWaitSeconds : TweakingWaitableDecorator
		{
			public readonly float EndTime;

			private WaitUntil _waitUntil;

			public TweakAndWaitSeconds(float seconds)
			{
				EndTime = Time.time + seconds;
			}

			public override bool IsFinished()
			{
				return Time.time >= EndTime;
			}

			public override IEnumerator GetYieldInstruction()
			{
				if (_waitUntil == null)
				{
					_waitUntil = new WaitUntil(IsFinished);
				}
				yield return _waitUntil;
			}
		}

		private class TweakAndWaitUntil : TweakingWaitableDecorator
		{
			public readonly IEnumerator Enumerator;

			public readonly Func<bool> Condition;

			private readonly bool _invertCondition;

			public TweakAndWaitUntil(IEnumerator enumerator, Func<bool> condition, bool invertCondition = false)
			{
				Enumerator = enumerator;
				Condition = condition;
				_invertCondition = invertCondition;
			}

			public override IEnumerator GetYieldInstruction()
			{
				return Enumerator;
			}

			public override bool IsFinished()
			{
				if (!_invertCondition)
				{
					return Condition();
				}
				return !Condition();
			}
		}

		private readonly AudioMixer _mixer;

		private Dictionary<EffectType, Tweaker> _tweakerDict = new Dictionary<EffectType, Tweaker>();

		private EffectType _latestEffect;

		public EffectAutomationHelper(MonoBehaviour mono, AudioMixer mixer)
			: base(mono)
		{
			_mixer = mixer;
		}

		public WaitForSeconds ForSeconds(float seconds)
		{
			TweakAndWaitSeconds decoration = new TweakAndWaitSeconds(seconds);
			DecorateTweakingWaitable(decoration);
			return new WaitForSeconds(seconds);
		}

		public WaitUntil Until(Func<bool> condition)
		{
			WaitUntil waitUntil = new WaitUntil(condition);
			TweakAndWaitUntil decoration = new TweakAndWaitUntil(waitUntil, condition);
			DecorateTweakingWaitable(decoration);
			return waitUntil;
		}

		public WaitWhile While(Func<bool> condition)
		{
			WaitWhile waitWhile = new WaitWhile(condition);
			TweakAndWaitUntil decoration = new TweakAndWaitUntil(waitWhile, condition, invertCondition: true);
			DecorateTweakingWaitable(decoration);
			return waitWhile;
		}

		private void DecorateTweakingWaitable(TweakingWaitableDecorator decoration)
		{
			Tweaker value;
			if (_latestEffect == EffectType.None)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"AutoResetWaitable on {_latestEffect} is not supported.");
			}
			else if (_tweakerDict.TryGetValue(_latestEffect, out value))
			{
				int index = value.WaitableList.Count - 1;
				ITweakingWaitable tweakingWaitable = value.WaitableList[index];
				if (tweakingWaitable is TweakingWaitableBase)
				{
					decoration.AttachTo(tweakingWaitable);
					value.WaitableList[index] = decoration;
				}
				else
				{
					Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>The latest waitable isn't the base type:TweakingWaitableBase");
				}
			}
		}

		public void SetEffectTrackParameter(Effect effect, Action<EffectType> onReset)
		{
			_latestEffect = effect.Type;
			if (_latestEffect == EffectType.None)
			{
				ResetAllEffect(effect, onReset);
				return;
			}
			if (!_tweakerDict.TryGetValue(effect.Type, out var value))
			{
				value = new Tweaker();
				_tweakerDict.Add(effect.Type, value);
			}
			bool flag = value.WaitableList != null && value.WaitableList.Count != 0 && effect.IsMoreIntenseThan(value.WaitableList[value.WaitableList.Count - 1].Effect);
			Tweaker tweaker = value;
			if (tweaker.WaitableList == null)
			{
				tweaker.WaitableList = new List<ITweakingWaitable>();
			}
			value.WaitableList.Add(new TweakingWaitableBase(effect));
			value.WaitableList.Sort();
			if (!value.IsTweaking || flag)
			{
				StartCoroutineAndReassign(TweakTrackParameter(value, effect.Type, effect.IsDominator, onReset), ref value.Coroutine);
				if (effect.IsDominator)
				{
					SwitchMainTrackMode(isDominatorActive: true);
				}
			}
		}

		private void SwitchMainTrackMode(bool isDominatorActive)
		{
			string text = (isDominatorActive ? "Main" : "Main_Dominated");
			string to = (isDominatorActive ? "Main_Dominated" : "Main");
			_mixer.ChangeChannel(text, to, 0f);
		}

		private IEnumerator TweakTrackParameter(Tweaker tweaker, EffectType resetType, bool isDominator, Action<EffectType> onReset)
		{
			tweaker.IsTweaking = true;
			while (tweaker.WaitableList.Count > 0)
			{
				int lastIndex = tweaker.WaitableList.Count - 1;
				Effect effect = tweaker.WaitableList[lastIndex].Effect;
				bool hasSecondaryParameter;
				string paraName = GetEffectParameterName(effect, out hasSecondaryParameter);
				float currentValue = GetCurrentValue(effect);
				yield return Tweak(currentValue, effect.Value, effect.Fading.FadeIn, effect.Fading.FadeInEase, paraName, hasSecondaryParameter);
				ITweakingWaitable tweakingWaitable = tweaker.WaitableList[lastIndex];
				IEnumerator yieldInstruction = tweakingWaitable.GetYieldInstruction();
				if (yieldInstruction != null)
				{
					if (!tweakingWaitable.IsFinished())
					{
						yield return yieldInstruction;
					}
					if (tweaker.WaitableList.Count == 1)
					{
						yield return Tweak(GetCurrentValue(effect), GetEffectDefaultValue(effect.Type), effect.Fading.FadeOut, effect.Fading.FadeOutEase, paraName, hasSecondaryParameter);
					}
				}
				tweaker.WaitableList.RemoveAt(lastIndex);
			}
			tweaker.IsTweaking = false;
			onReset?.Invoke(resetType);
			if (isDominator)
			{
				SwitchMainTrackMode(isDominatorActive: false);
			}
		}

		private IEnumerator Tweak(float from, float to, float fadeTime, Ease ease, string paraName, bool hasSecondaryParameter = false, Action onTweakingFinshed = null)
		{
			if (from != to)
			{
				string secondaryParaName = (hasSecondaryParameter ? (paraName + "2") : null);
				float currentTime = 0f;
				while (currentTime < fadeTime)
				{
					currentTime += Utility.GetDeltaTime();
					float value = Mathf.Lerp(from, to, (currentTime / fadeTime).SetEase(ease));
					_mixer.SafeSetFloat(paraName, value);
					_mixer.SafeSetFloat(secondaryParaName, value);
					yield return null;
				}
				_mixer.SafeSetFloat(paraName, to);
				_mixer.SafeSetFloat(secondaryParaName, to);
				onTweakingFinshed?.Invoke();
			}
		}

		private bool TryGetCurrentValue(Effect effect, out float value)
		{
			bool hasSecondaryParameter;
			string effectParameterName = GetEffectParameterName(effect, out hasSecondaryParameter);
			if (!_mixer.SafeGetFloat(effectParameterName, out value))
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Can't get exposed parameter of {effect.Type}");
				return false;
			}
			return true;
		}

		private float GetCurrentValue(Effect effect)
		{
			if (TryGetCurrentValue(effect, out var value))
			{
				return value;
			}
			return 0f;
		}

		private void ResetAllEffect(Effect effect, Action<EffectType> onResetFinished)
		{
			int tweakingCount = 0;
			Action onTweakingFinshed = OnTweakingFinished;
			foreach (KeyValuePair<EffectType, Tweaker> item in _tweakerDict)
			{
				Tweaker value = item.Value;
				EffectType key = item.Key;
				if (TryGetCurrentValue(effect, out var value2))
				{
					bool hasSecondaryParameter;
					string effectParameterName = GetEffectParameterName(effect, out hasSecondaryParameter);
					SafeStopCoroutine(value.Coroutine);
					value.Coroutine = StartCoroutine(Tweak(value2, GetEffectDefaultValue(key), effect.Fading.FadeOut, effect.Fading.FadeOutEase, effectParameterName, hasSecondaryParameter, onTweakingFinshed));
					value.WaitableList.Clear();
					tweakingCount++;
				}
			}
			void OnTweakingFinished()
			{
				tweakingCount--;
				if (tweakingCount <= 0)
				{
					onResetFinished?.Invoke(EffectType.All);
				}
			}
		}

		private string GetEffectParameterName(Effect effect, out bool hasSecondaryParameter)
		{
			hasSecondaryParameter = false;
			switch (effect.Type)
			{
			case EffectType.Volume:
				if (effect.IsDominator)
				{
					return "Main_Dominated";
				}
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"{effect.Type} is only supported on Dominator");
				return string.Empty;
			case EffectType.LowPass:
				hasSecondaryParameter = SoundManager.Instance.Setting.AudioFilterSlope == FilterSlope.FourPole;
				if (!effect.IsDominator)
				{
					return "Effect_LowPass";
				}
				return "Main_LowPass";
			case EffectType.HighPass:
				hasSecondaryParameter = SoundManager.Instance.Setting.AudioFilterSlope == FilterSlope.FourPole;
				if (!effect.IsDominator)
				{
					return "Effect_HighPass";
				}
				return "Main_HighPass";
			case EffectType.Custom:
				return effect.CustomExposedParameter;
			default:
				return string.Empty;
			}
		}

		private float GetEffectDefaultValue(EffectType effectType)
		{
			return effectType switch
			{
				EffectType.Volume => 0f, 
				EffectType.LowPass => 22000f, 
				EffectType.HighPass => 10f, 
				_ => -1f, 
			};
		}
	}
}

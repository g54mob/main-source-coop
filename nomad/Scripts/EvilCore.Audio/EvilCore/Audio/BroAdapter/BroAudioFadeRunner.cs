using System;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

namespace EvilCore.Audio.BroAdapter
{
	public class BroAudioFadeRunner : MonoBehaviour
	{
		private enum ParamKind
		{
			Volume = 0,
			Pitch = 1
		}

		private readonly struct Key : IEquatable<Key>
		{
			public readonly SoundID Id;

			public readonly ParamKind Kind;

			public Key(SoundID id, ParamKind kind)
			{
				Id = id;
				Kind = kind;
			}

			public bool Equals(Key other)
			{
				if (Id.Equals(other.Id))
				{
					return Kind == other.Kind;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is Key other)
				{
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (Id.GetHashCode() * 397) ^ (int)Kind;
			}
		}

		private readonly Dictionary<Key, Coroutine> _running = new Dictionary<Key, Coroutine>();

		public void FadeVolume(SoundID id, float current, float target, float duration, AnimationCurve curve)
		{
			StartFade(new Key(id, ParamKind.Volume), current, target, duration, curve);
		}

		public void FadePitch(SoundID id, float current, float target, float duration, AnimationCurve curve)
		{
			StartFade(new Key(id, ParamKind.Pitch), current, target, duration, curve);
		}

		public void CancelAll(SoundID id)
		{
			CancelKey(new Key(id, ParamKind.Volume));
			CancelKey(new Key(id, ParamKind.Pitch));
		}

		private void StartFade(Key key, float current, float target, float duration, AnimationCurve curve)
		{
			CancelKey(key);
			if (duration <= 0f)
			{
				Apply(key, target);
			}
			else
			{
				_running[key] = StartCoroutine(Run(key, current, target, duration, curve));
			}
		}

		private IEnumerator Run(Key key, float from, float to, float duration, AnimationCurve curve)
		{
			float t = 0f;
			while (t < duration)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(t / duration);
				float t2 = ((curve != null) ? Mathf.Clamp01(curve.Evaluate(num)) : num);
				Apply(key, Mathf.Lerp(from, to, t2));
				yield return null;
			}
			Apply(key, to);
			_running.Remove(key);
		}

		private void Apply(Key key, float value)
		{
			switch (key.Kind)
			{
			case ParamKind.Volume:
				BroAudio.SetVolume(key.Id, value);
				break;
			case ParamKind.Pitch:
				BroAudio.SetPitch(key.Id, value);
				break;
			}
		}

		private void CancelKey(Key key)
		{
			if (_running.TryGetValue(key, out var value))
			{
				if (value != null)
				{
					StopCoroutine(value);
				}
				_running.Remove(key);
			}
		}

		private void OnDisable()
		{
			foreach (KeyValuePair<Key, Coroutine> item in _running)
			{
				if (item.Value != null)
				{
					StopCoroutine(item.Value);
				}
			}
			_running.Clear();
		}
	}
}

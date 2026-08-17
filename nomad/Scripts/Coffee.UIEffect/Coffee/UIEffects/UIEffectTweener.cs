using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Coffee.UIEffects
{
	[ExecuteAlways]
	[RequireComponent(typeof(UIEffectBase))]
	public class UIEffectTweener : MonoBehaviour
	{
		[Serializable]
		public class TweenerEvent : UnityEvent<float>
		{
		}

		[Flags]
		public enum CullingMask
		{
			Tone = 1,
			Color = 2,
			Sampling = 4,
			Transition = 8,
			GradiationOffset = 0x20,
			GradiationRotation = 0x40,
			EdgeShiny = 0x100,
			Event = -2147483648
		}

		public enum UpdateMode
		{
			Normal = 0,
			Unscaled = 1,
			Manual = 2
		}

		public enum WrapMode
		{
			Once = 0,
			Loop = 1,
			PingPongOnce = 2,
			PingPongLoop = 3
		}

		public enum Direction
		{
			Forward = 0,
			Reverse = 1
		}

		public enum PlayOnEnable
		{
			None = 0,
			Forward = 1,
			Reverse = 2,
			KeepDirection = 3
		}

		[Tooltip("The culling mask of the tween.")]
		[SerializeField]
		private CullingMask m_CullingMask = CullingMask.Tone | CullingMask.Color | CullingMask.Sampling | CullingMask.Transition | CullingMask.GradiationOffset | CullingMask.GradiationRotation | CullingMask.EdgeShiny;

		[Tooltip("The direction of the tween.")]
		[SerializeField]
		private Direction m_Direction;

		[Tooltip("The curve to tween the properties.")]
		[SerializeField]
		private AnimationCurve m_Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private bool m_SeparateReverseCurve;

		[Tooltip("The curve to tween the properties.")]
		[SerializeField]
		private AnimationCurve m_ReverseCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[Tooltip("The delay in seconds before the tween starts.")]
		[SerializeField]
		[Range(0f, 10f)]
		private float m_Delay;

		[Tooltip("The duration in seconds of the tween.")]
		[SerializeField]
		[Range(0.05f, 10f)]
		private float m_Duration = 1f;

		[Tooltip("The interval in seconds between each loop.")]
		[SerializeField]
		[Range(0f, 10f)]
		private float m_Interval;

		[FormerlySerializedAs("m_ResetTimeOnEnable")]
		[Tooltip("Play the tween when the component is enabled.")]
		[SerializeField]
		private PlayOnEnable m_PlayOnEnable = PlayOnEnable.Forward;

		[Tooltip("Reset the tweening time when the component is enabled.")]
		[SerializeField]
		private bool m_ResetTimeOnEnable = true;

		[Tooltip("The wrap mode of the tween.\n  Once: Clamp the tween value (not loop).\n  Loop: Loop the tween value.\n  PingPongOnce: PingPong the tween value (not loop).\n  PingPong: PingPong the tween value.")]
		[SerializeField]
		private WrapMode m_WrapMode = WrapMode.Loop;

		[Tooltip("Specifies how to get delta time.\n  Normal: Use `Time.deltaTime`.\n  Unscaled: Use `Time.unscaledDeltaTime`.\n  Manual: Not updated automatically and update manually with `UpdateTime` or `SetTime` method.")]
		[SerializeField]
		private UpdateMode m_UpdateMode;

		[Tooltip("Event to invoke when the tween has completed.")]
		[SerializeField]
		private UnityEvent m_OnComplete = new UnityEvent();

		[Tooltip("Event to invoke when the rate was changed.")]
		[SerializeField]
		private TweenerEvent m_OnChangedRate = new TweenerEvent();

		private bool _isPaused;

		private float _rate = -1f;

		private float _time;

		private UIEffectBase _target;

		private UIEffectBase target
		{
			get
			{
				if (!_target)
				{
					return _target = GetComponent<UIEffectBase>();
				}
				return _target;
			}
		}

		public CullingMask cullingMask
		{
			get
			{
				return m_CullingMask;
			}
			set
			{
				m_CullingMask = value;
			}
		}

		public Direction direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				m_Direction = value;
			}
		}

		public float rate
		{
			get
			{
				return _rate;
			}
			private set
			{
				value = Mathf.Clamp01(value);
				if (Mathf.Approximately(_rate, value))
				{
					return;
				}
				_rate = value;
				if (!target || cullingMask == (CullingMask)0)
				{
					return;
				}
				AnimationCurve animationCurve = curve;
				if (separateReverseCurve)
				{
					switch (wrapMode)
					{
					case WrapMode.Once:
					case WrapMode.Loop:
						if (direction == Direction.Reverse)
						{
							animationCurve = reverseCurve;
						}
						break;
					case WrapMode.PingPongOnce:
					case WrapMode.PingPongLoop:
						if (delay + duration + interval <= _time)
						{
							animationCurve = reverseCurve;
						}
						break;
					}
				}
				float arg = animationCurve.Evaluate(_rate);
				target.SetRate(arg, cullingMask);
				if ((cullingMask & CullingMask.Event) != 0)
				{
					onChangedRate.Invoke(arg);
				}
			}
		}

		public float duration
		{
			get
			{
				return m_Duration;
			}
			set
			{
				m_Duration = Mathf.Max(0.001f, value);
			}
		}

		public float delay
		{
			get
			{
				return m_Delay;
			}
			set
			{
				m_Delay = Mathf.Max(0f, value);
			}
		}

		public float interval
		{
			get
			{
				return m_Interval;
			}
			set
			{
				m_Interval = Mathf.Max(0f, value);
			}
		}

		public float time
		{
			get
			{
				if (wrapMode == WrapMode.Once || wrapMode == WrapMode.PingPongOnce)
				{
					return Mathf.Clamp(_time, 0f, totalTime);
				}
				return Mathf.Repeat(_time, totalTime);
			}
		}

		public float totalTime => wrapMode switch
		{
			WrapMode.Once => delay + duration, 
			WrapMode.Loop => delay + duration + interval, 
			WrapMode.PingPongOnce => delay + duration * 2f + interval, 
			WrapMode.PingPongLoop => delay + duration * 2f + interval * 2f, 
			_ => throw new ArgumentOutOfRangeException(), 
		};

		public PlayOnEnable playOnEnable
		{
			get
			{
				return m_PlayOnEnable;
			}
			set
			{
				m_PlayOnEnable = value;
			}
		}

		public bool resetTimeOnEnable
		{
			get
			{
				return m_ResetTimeOnEnable;
			}
			set
			{
				m_ResetTimeOnEnable = value;
			}
		}

		public WrapMode wrapMode
		{
			get
			{
				return m_WrapMode;
			}
			set
			{
				m_WrapMode = value;
			}
		}

		public UpdateMode updateMode
		{
			get
			{
				return m_UpdateMode;
			}
			set
			{
				m_UpdateMode = value;
			}
		}

		public AnimationCurve curve
		{
			get
			{
				return m_Curve;
			}
			set
			{
				m_Curve = value;
			}
		}

		public bool separateReverseCurve
		{
			get
			{
				return m_SeparateReverseCurve;
			}
			set
			{
				m_SeparateReverseCurve = value;
			}
		}

		public AnimationCurve reverseCurve
		{
			get
			{
				return m_ReverseCurve;
			}
			set
			{
				m_ReverseCurve = value;
			}
		}

		public UnityEvent onComplete => m_OnComplete;

		public TweenerEvent onChangedRate => m_OnChangedRate;

		public bool isTweening
		{
			get
			{
				if (_isPaused)
				{
					return false;
				}
				if (wrapMode == WrapMode.Loop || wrapMode == WrapMode.PingPongLoop)
				{
					return true;
				}
				if (direction != Direction.Forward)
				{
					return 0f < _time;
				}
				return _time < totalTime;
			}
		}

		public bool isPaused => _isPaused;

		public bool isDelaying => _time < delay;

		private void OnEnable()
		{
			_isPaused = true;
			switch (playOnEnable)
			{
			case PlayOnEnable.KeepDirection:
				Play(resetTimeOnEnable);
				break;
			case PlayOnEnable.Forward:
				PlayForward(resetTimeOnEnable);
				break;
			case PlayOnEnable.Reverse:
				PlayReverse(resetTimeOnEnable);
				break;
			}
		}

		private void OnDisable()
		{
			_isPaused = true;
		}

		private void Update()
		{
			if (isTweening)
			{
				float num = ((m_UpdateMode == UpdateMode.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime);
				UpdateTime((direction == Direction.Forward) ? num : (0f - num));
			}
		}

		public void Play(bool resetTime)
		{
			if (resetTime)
			{
				ResetTime(direction);
			}
			Play();
		}

		public void Play()
		{
			_isPaused = false;
			if (!isTweening)
			{
				m_OnComplete.Invoke();
			}
		}

		public void PlayForward(bool resetTime)
		{
			if (resetTime)
			{
				ResetTime(Direction.Forward);
			}
			PlayForward();
		}

		public void PlayForward()
		{
			direction = Direction.Forward;
			_isPaused = false;
			if (!isTweening)
			{
				m_OnComplete.Invoke();
			}
		}

		public void PlayReverse(bool resetTime)
		{
			if (resetTime)
			{
				ResetTime(Direction.Reverse);
			}
			PlayReverse();
		}

		public void PlayReverse()
		{
			direction = Direction.Reverse;
			_isPaused = false;
			if (!isTweening)
			{
				m_OnComplete.Invoke();
			}
		}

		public void Stop()
		{
			_isPaused = true;
			ResetTime();
		}

		public void SetPause(bool pause)
		{
			_isPaused = pause;
		}

		public void ResetTime()
		{
			SetTime(0f);
		}

		public void ResetTime(Direction dir)
		{
			if (dir == Direction.Forward)
			{
				SetTime(0f);
			}
			else
			{
				SetTime(totalTime - 0.0001f);
			}
		}

		[Obsolete("UIEffectTweener.Restart has been deprecated. Use UIEffectTweener.ResetTime instead (UnityUpgradable) -> ResetTime")]
		public void Restart()
		{
			ResetTime();
		}

		public void SetTime(float sec)
		{
			_time = 0f;
			UpdateTime(sec);
		}

		public void UpdateTime(float deltaSec)
		{
			bool flag = isTweening;
			bool num = wrapMode == WrapMode.Loop || wrapMode == WrapMode.PingPongLoop;
			_time += deltaSec;
			if (num)
			{
				if (_time < 0f)
				{
					_time = Mathf.Repeat(_time, totalTime);
				}
				else if (delay < _time)
				{
					_time = Mathf.Repeat(_time - delay, totalTime - delay) + delay;
				}
				else if (deltaSec < 0f && delay <= _time - deltaSec)
				{
					_time = Mathf.Repeat(_time - delay, totalTime - delay) + delay;
				}
			}
			else
			{
				_time = Mathf.Clamp(_time, 0f, totalTime);
			}
			float num2 = _time - delay;
			if (num2 <= 0f && 0f <= _time)
			{
				rate = 0f;
				if (flag && !isTweening)
				{
					m_OnComplete.Invoke();
				}
				return;
			}
			switch (wrapMode)
			{
			case WrapMode.Once:
				num2 = Mathf.Clamp(num2, 0f, duration);
				_time = num2 + delay;
				break;
			case WrapMode.Loop:
				num2 = Mathf.Repeat(num2, duration + interval);
				_time = num2 + delay;
				break;
			case WrapMode.PingPongOnce:
				num2 = Mathf.Clamp(num2, 0f, duration * 2f + interval);
				_time = num2 + delay;
				num2 = Mathf.PingPong(num2, duration + interval * 0.5f);
				break;
			case WrapMode.PingPongLoop:
				num2 = Mathf.Repeat(num2, (duration + interval) * 2f);
				_time = num2 + delay;
				num2 = ((num2 < duration * 2f + interval) ? Mathf.PingPong(num2, duration + interval * 0.5f) : 0f);
				break;
			}
			rate = Mathf.Clamp(num2, 0f, duration) / duration;
			if (flag && !isTweening)
			{
				m_OnComplete.Invoke();
			}
		}
	}
}

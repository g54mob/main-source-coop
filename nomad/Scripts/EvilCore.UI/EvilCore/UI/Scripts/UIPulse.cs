using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIPulse : MonoBehaviour
	{
		[Header("Pulse")]
		[SerializeField]
		private float pulseScale = 1.05f;

		[SerializeField]
		private float period = 1f;

		[SerializeField]
		private Ease ease = Ease.InOutSine;

		[SerializeField]
		private bool playOnEnable = true;

		private Vector3 _defaultScale;

		private Tween _tween;

		private bool _cached;

		private void Awake()
		{
			CacheScale();
		}

		private void CacheScale()
		{
			if (!_cached)
			{
				_defaultScale = base.transform.localScale;
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
			CacheScale();
			_tween.Stop();
			_tween = Tween.Scale(base.transform, _defaultScale * pulseScale, period, ease, -1, CycleMode.Yoyo, 0f, 0f, useUnscaledTime: true);
		}

		public void StopPulse()
		{
			_tween.Stop();
			if (_cached)
			{
				base.transform.localScale = _defaultScale;
			}
		}

		private void OnDisable()
		{
			_tween.Stop();
			if (_cached)
			{
				base.transform.localScale = _defaultScale;
			}
		}
	}
}

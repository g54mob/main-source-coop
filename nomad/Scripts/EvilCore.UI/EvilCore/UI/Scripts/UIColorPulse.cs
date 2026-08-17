using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public class UIColorPulse : MonoBehaviour
	{
		[Header("Target")]
		[SerializeField]
		private Graphic graphic;

		[Header("Pulse")]
		[SerializeField]
		private Color colorA = Color.white;

		[SerializeField]
		private Color colorB = new Color(1f, 1f, 1f, 0.4f);

		[SerializeField]
		private float period = 1f;

		[SerializeField]
		private Ease ease = Ease.InOutSine;

		[SerializeField]
		private bool playOnEnable = true;

		private Color _default;

		private Tween _tween;

		private bool _cached;

		private void Awake()
		{
			if (graphic == null)
			{
				graphic = GetComponent<Graphic>();
			}
			CacheColor();
		}

		private void CacheColor()
		{
			if (!_cached && !(graphic == null))
			{
				_default = graphic.color;
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
			if (!(graphic == null))
			{
				CacheColor();
				_tween.Stop();
				_tween = Tween.Color(graphic, colorA, colorB, period, ease, -1, CycleMode.Yoyo, 0f, 0f, useUnscaledTime: true);
			}
		}

		public void StopPulse()
		{
			_tween.Stop();
			if (_cached && graphic != null)
			{
				graphic.color = _default;
			}
		}

		private void OnDisable()
		{
			_tween.Stop();
			if (_cached && graphic != null)
			{
				graphic.color = _default;
			}
		}
	}
}

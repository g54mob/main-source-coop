using System.Globalization;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UINumberTicker : MonoBehaviour
	{
		[Header("Target")]
		[SerializeField]
		private TextMeshProUGUI text;

		[Header("Format")]
		[SerializeField]
		private string format = "{0:0}";

		[SerializeField]
		private float duration = 0.4f;

		[SerializeField]
		private Ease ease = Ease.OutCubic;

		private float _current;

		private Tween _tween;

		private void Awake()
		{
			if (text == null)
			{
				text = GetComponent<TextMeshProUGUI>();
			}
			Render(_current);
		}

		public void SetValue(float target)
		{
			_tween.Stop();
			_tween = Tween.Custom(this, _current, target, duration, delegate(UINumberTicker ticker, float value)
			{
				ticker.Render(value);
			}, ease, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		public void SetValueInstant(float target)
		{
			_tween.Stop();
			Render(target);
		}

		private void Render(float value)
		{
			_current = value;
			if (text != null)
			{
				text.text = string.Format(CultureInfo.CurrentCulture, format, value);
			}
		}

		private void OnDisable()
		{
			_tween.Stop();
		}
	}
}

using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIShimmer : MonoBehaviour
	{
		[Header("Shine")]
		[SerializeField]
		private RectTransform shine;

		[SerializeField]
		private float startX = -120f;

		[SerializeField]
		private float endX = 120f;

		[SerializeField]
		private float sweepDuration = 0.6f;

		[SerializeField]
		private float delayBetween = 1.5f;

		[SerializeField]
		private bool playOnEnable = true;

		private Sequence _sequence;

		private void OnEnable()
		{
			if (playOnEnable)
			{
				Play();
			}
		}

		public void Play()
		{
			if (!(shine == null))
			{
				_sequence.Stop();
				_sequence = Sequence.Create(-1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).Chain(Tween.UIAnchoredPositionX(shine, startX, endX, sweepDuration, Ease.InOutSine, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true)).ChainDelay(delayBetween);
			}
		}

		public void StopShimmer()
		{
			_sequence.Stop();
		}

		private void OnDisable()
		{
			_sequence.Stop();
			if (shine != null)
			{
				shine.anchoredPosition = new Vector2(startX, shine.anchoredPosition.y);
			}
		}
	}
}

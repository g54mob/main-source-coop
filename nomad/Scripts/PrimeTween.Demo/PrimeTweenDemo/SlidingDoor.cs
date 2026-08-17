using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class SlidingDoor : Animatable
	{
		[SerializeField]
		private Demo demo;

		[SerializeField]
		private Transform animationAnchor;

		[SerializeField]
		private Vector3 openedPos;

		[SerializeField]
		private Vector3 midPos;

		[SerializeField]
		private Vector3 closedPos;

		private bool isClosed;

		private Sequence sequence;

		public override void OnClick()
		{
			if (!demo.animateAllSequence.isAlive)
			{
				Animate(!isClosed);
			}
		}

		public override Sequence Animate(bool _isClosed)
		{
			if (isClosed == _isClosed)
			{
				return Sequence.Create();
			}
			isClosed = _isClosed;
			if (sequence.isAlive)
			{
				sequence.Stop();
			}
			TweenSettings settings = new TweenSettings(0.4f, Ease.OutBack, 1, CycleMode.Restart, 0f, 0.1f);
			sequence = Tween.LocalPosition(animationAnchor, new TweenSettings<Vector3>(midPos, settings)).Chain(Tween.LocalPosition(animationAnchor, new TweenSettings<Vector3>(_isClosed ? closedPos : openedPos, settings)));
			return sequence;
		}
	}
}

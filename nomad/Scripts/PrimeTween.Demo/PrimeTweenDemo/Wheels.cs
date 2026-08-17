using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class Wheels : Animatable
	{
		[SerializeField]
		private Demo demo;

		[SerializeField]
		private Transform[] wheels;

		private bool isAnimating;

		private Sequence sequence;

		public override void OnClick()
		{
			demo.AnimateAll(!isAnimating);
		}

		public override Sequence Animate(bool _isAnimating)
		{
			isAnimating = _isAnimating;
			return Sequence.Create().ChainCallback(this, delegate(Wheels target)
			{
				target.SpinWheelsInfinitely();
			});
		}

		private void SpinWheelsInfinitely()
		{
			if (isAnimating)
			{
				sequence.Complete();
				sequence = Sequence.Create(-1);
				Transform[] array = wheels;
				foreach (Transform target in array)
				{
					sequence.Group(Tween.LocalEulerAngles(target, Vector3.zero, new Vector3(360f, 0f), 1f, Ease.Linear));
				}
			}
			else if (sequence.isAlive)
			{
				sequence.SetRemainingCycles(0);
			}
		}
	}
}

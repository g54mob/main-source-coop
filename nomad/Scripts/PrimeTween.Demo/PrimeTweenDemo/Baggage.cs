using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class Baggage : Animatable
	{
		[SerializeField]
		private Transform animationAnchor;

		private Sequence sequence;

		public override void OnClick()
		{
			PlayFlipAnimation();
		}

		public override Sequence Animate(bool _)
		{
			return PlayFlipAnimation();
		}

		private Sequence PlayFlipAnimation()
		{
			if (!sequence.isAlive)
			{
				sequence = Sequence.Create().Chain(Tween.LocalPositionZ(animationAnchor, 0.2f, 0.3f)).Chain(Tween.LocalEulerAngles(animationAnchor, Vector3.zero, new Vector3(0f, 360f, 0f), 0.9f, Ease.InOutBack))
					.Chain(Tween.LocalPositionZ(animationAnchor, 0f, 0.3f));
			}
			return sequence;
		}
	}
}

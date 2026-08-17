using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class JumpAnimation : Clickable
	{
		[SerializeField]
		private Transform target;

		private Sequence sequence;

		public override void OnClick()
		{
			PlayAnimation();
		}

		public void PlayAnimation()
		{
			if (!sequence.isAlive)
			{
				sequence = Tween.Scale(target, new Vector3(1.1f, 0.8f, 1.1f), 0.15f, Ease.OutQuad, 2, CycleMode.Yoyo).Chain(Tween.LocalPositionY(target, 1f, 0.3f)).Chain(Tween.LocalEulerAngles(target, Vector3.zero, new Vector3(0f, 360f), 1.5f, Ease.InOutBack))
					.Chain(Tween.LocalPositionY(target, 0f, 0.3f));
			}
		}
	}
}

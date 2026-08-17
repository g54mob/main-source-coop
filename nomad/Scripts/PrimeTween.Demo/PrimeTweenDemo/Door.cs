using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class Door : Animatable
	{
		[SerializeField]
		private CameraController cameraController;

		[SerializeField]
		private Transform animationAnchor;

		private bool isClosed;

		public override void OnClick()
		{
			Animate(!isClosed);
		}

		public override Sequence Animate(bool _isClosed)
		{
			if (isClosed == _isClosed)
			{
				return Sequence.Create();
			}
			isClosed = _isClosed;
			Sequence result = Sequence.Create();
			Tween tween = Tween.LocalRotation(animationAnchor, _isClosed ? new Vector3(0f, -90f) : Vector3.zero, 0.7f, Ease.InOutElastic);
			result.Group(tween);
			if (_isClosed)
			{
				result.Group(cameraController.Shake(0.5f));
			}
			return result;
		}
	}
}

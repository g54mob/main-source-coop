using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class Road : Animatable
	{
		[SerializeField]
		private MeshRenderer roadModel;

		[SerializeField]
		private AnimationCurve ease;

		private float currentSpeed;

		private void Awake()
		{
			roadModel.sharedMaterial = new Material(roadModel.sharedMaterial);
		}

		public override Sequence Animate(bool isAnimating)
		{
			Sequence result = Sequence.Create(Tween.Custom(this, currentSpeed, isAnimating ? 0.3f : 0f, 1f, delegate(Road _this, float val)
			{
				_this.currentSpeed = val;
			}));
			if (isAnimating)
			{
				result.Group(Tween.LocalPositionY(base.transform, 0f, -0.5f, 0.7f, ease));
			}
			return result;
		}

		private void Update()
		{
			roadModel.sharedMaterial.mainTextureOffset += new Vector2(-1f, 1f) * currentSpeed * Time.deltaTime;
		}
	}
}

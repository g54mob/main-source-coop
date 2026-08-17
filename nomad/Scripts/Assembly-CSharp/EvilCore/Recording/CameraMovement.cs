using UnityEngine;

namespace EvilCore.Recording
{
	public abstract class CameraMovement : MonoBehaviour
	{
		[Header("Timing")]
		[SerializeField]
		private float duration = 5f;

		[SerializeField]
		private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private bool loop;

		public float Duration => duration;

		public bool Loop => loop;

		protected float EaseTime(float normalizedTime)
		{
			return easingCurve.Evaluate(Mathf.Clamp01(normalizedTime));
		}

		public virtual void Begin(Transform cameraTransform)
		{
		}

		public abstract void Evaluate(float normalizedTime, Transform cameraTransform);

		public virtual void End(Transform cameraTransform)
		{
		}
	}
}

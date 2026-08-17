using UnityEngine;
using UnityEngine.Splines;

namespace EvilCore.Recording
{
	public class SplineMovement : CameraMovement
	{
		[Header("Spline Settings")]
		[SerializeField]
		private SplineContainer splineContainer;

		[SerializeField]
		private Transform lookAtTarget;

		[SerializeField]
		private bool alignToSplineTangent = true;

		public override void Begin(Transform cameraTransform)
		{
			if (splineContainer == null || splineContainer.Spline == null)
			{
				return;
			}
			splineContainer.Spline.Evaluate(0f, out var position, out var tangent, out var upVector);
			cameraTransform.position = position;
			if (lookAtTarget != null)
			{
				Vector3 forward = lookAtTarget.position - cameraTransform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					cameraTransform.rotation = Quaternion.LookRotation(forward);
				}
			}
			else if (alignToSplineTangent)
			{
				Vector3 forward2 = tangent;
				if (forward2.sqrMagnitude > 0.001f)
				{
					cameraTransform.rotation = Quaternion.LookRotation(forward2, upVector);
				}
			}
		}

		public override void Evaluate(float normalizedTime, Transform cameraTransform)
		{
			if (splineContainer == null || splineContainer.Spline == null)
			{
				return;
			}
			float t = EaseTime(normalizedTime);
			splineContainer.Spline.Evaluate(t, out var position, out var tangent, out var upVector);
			cameraTransform.position = position;
			if (lookAtTarget != null)
			{
				Vector3 forward = lookAtTarget.position - cameraTransform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					cameraTransform.rotation = Quaternion.LookRotation(forward);
				}
			}
			else if (alignToSplineTangent)
			{
				Vector3 forward2 = tangent;
				if (forward2.sqrMagnitude > 0.001f)
				{
					cameraTransform.rotation = Quaternion.LookRotation(forward2, upVector);
				}
			}
		}
	}
}

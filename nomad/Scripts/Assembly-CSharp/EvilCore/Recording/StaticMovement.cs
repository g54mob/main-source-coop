using UnityEngine;

namespace EvilCore.Recording
{
	public class StaticMovement : CameraMovement
	{
		[Header("Static Settings")]
		[SerializeField]
		private Transform positionReference;

		[SerializeField]
		private Transform lookAtTarget;

		[SerializeField]
		private bool smoothLookAt = true;

		private Vector3 _startPosition;

		private Quaternion _startRotation;

		public override void Begin(Transform cameraTransform)
		{
			_startPosition = ((positionReference != null) ? positionReference.position : cameraTransform.position);
			cameraTransform.position = _startPosition;
			if (lookAtTarget != null && !smoothLookAt)
			{
				cameraTransform.rotation = Quaternion.LookRotation(lookAtTarget.position - cameraTransform.position);
			}
			_startRotation = cameraTransform.rotation;
		}

		public override void Evaluate(float normalizedTime, Transform cameraTransform)
		{
			cameraTransform.position = _startPosition;
			if (!(lookAtTarget == null))
			{
				Quaternion quaternion = Quaternion.LookRotation(lookAtTarget.position - cameraTransform.position);
				if (smoothLookAt)
				{
					float t = EaseTime(normalizedTime);
					cameraTransform.rotation = Quaternion.Slerp(_startRotation, quaternion, t);
				}
				else
				{
					cameraTransform.rotation = quaternion;
				}
			}
		}
	}
}

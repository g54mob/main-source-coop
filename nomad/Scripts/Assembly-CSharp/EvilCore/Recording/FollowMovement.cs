using UnityEngine;

namespace EvilCore.Recording
{
	public class FollowMovement : CameraMovement
	{
		[Header("Follow Settings")]
		[SerializeField]
		private Transform target;

		[SerializeField]
		private Vector3 positionOffset = new Vector3(0f, 3f, -8f);

		[SerializeField]
		private float followSmoothTime = 0.3f;

		[SerializeField]
		private bool lookAtTarget = true;

		[SerializeField]
		private bool useLocalOffset = true;

		private Vector3 _currentVelocity;

		public override void Begin(Transform cameraTransform)
		{
			_currentVelocity = Vector3.zero;
			if (target != null)
			{
				Vector3 desiredPosition = GetDesiredPosition();
				cameraTransform.position = desiredPosition;
				if (lookAtTarget)
				{
					cameraTransform.rotation = Quaternion.LookRotation(target.position - cameraTransform.position);
				}
			}
		}

		public override void Evaluate(float normalizedTime, Transform cameraTransform)
		{
			if (target == null)
			{
				return;
			}
			Vector3 desiredPosition = GetDesiredPosition();
			cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref _currentVelocity, followSmoothTime);
			if (lookAtTarget)
			{
				Vector3 forward = target.position - cameraTransform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					cameraTransform.rotation = Quaternion.LookRotation(forward);
				}
			}
		}

		private Vector3 GetDesiredPosition()
		{
			if (useLocalOffset)
			{
				return target.TransformPoint(positionOffset);
			}
			return target.position + positionOffset;
		}
	}
}

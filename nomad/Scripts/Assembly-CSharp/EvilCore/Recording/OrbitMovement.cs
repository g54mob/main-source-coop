using System;
using UnityEngine;

namespace EvilCore.Recording
{
	public class OrbitMovement : CameraMovement
	{
		[Header("Orbit Settings")]
		[SerializeField]
		private Transform target;

		[SerializeField]
		private float radius = 5f;

		[SerializeField]
		private float height = 2f;

		[SerializeField]
		private float startAngle;

		[SerializeField]
		private float endAngle = 360f;

		[SerializeField]
		private bool lookAtTarget = true;

		[SerializeField]
		private Vector3 targetOffset;

		public override void Evaluate(float normalizedTime, Transform cameraTransform)
		{
			if (target == null)
			{
				return;
			}
			float t = EaseTime(normalizedTime);
			float f = Mathf.Lerp(startAngle, endAngle, t) * ((float)Math.PI / 180f);
			Vector3 vector = target.position + targetOffset;
			Vector3 vector2 = new Vector3(Mathf.Sin(f) * radius, height, Mathf.Cos(f) * radius);
			cameraTransform.position = vector + vector2;
			if (lookAtTarget)
			{
				Vector3 forward = vector - cameraTransform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					cameraTransform.rotation = Quaternion.LookRotation(forward);
				}
			}
		}
	}
}

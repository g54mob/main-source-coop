using UnityEngine;

namespace EvilCore.Recording
{
	public struct PositionSmoother
	{
		private Vector3 _positionVelocity;

		public Vector3 Smooth(Vector3 current, Vector3 target, float smoothing, float deltaTime)
		{
			if (smoothing <= 0f)
			{
				return target;
			}
			float smoothTime = Mathf.Lerp(0.01f, 0.5f, smoothing);
			return Vector3.SmoothDamp(current, target, ref _positionVelocity, smoothTime, 1f / 0f, deltaTime);
		}

		public Quaternion Smooth(Quaternion current, Quaternion target, float smoothing, float deltaTime)
		{
			if (smoothing <= 0f)
			{
				return target;
			}
			float t = 1f - Mathf.Exp((0f - (1f - smoothing * 0.9f)) * 10f * deltaTime);
			return Quaternion.Slerp(current, target, t);
		}

		public void Reset()
		{
			_positionVelocity = Vector3.zero;
		}
	}
}

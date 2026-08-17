using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Recording
{
	public class DollyMovement : CameraMovement
	{
		[Header("Dolly Settings")]
		[SerializeField]
		private List<Transform> waypoints = new List<Transform>();

		[SerializeField]
		private Transform lookAtTarget;

		[SerializeField]
		private bool smoothPath = true;

		private Vector3[] _positions;

		private Quaternion[] _rotations;

		public override void Begin(Transform cameraTransform)
		{
			if (waypoints.Count == 0)
			{
				return;
			}
			_positions = new Vector3[waypoints.Count];
			_rotations = new Quaternion[waypoints.Count];
			for (int i = 0; i < waypoints.Count; i++)
			{
				if (!(waypoints[i] == null))
				{
					_positions[i] = waypoints[i].position;
					_rotations[i] = waypoints[i].rotation;
				}
			}
			cameraTransform.position = _positions[0];
			if (lookAtTarget != null)
			{
				cameraTransform.rotation = Quaternion.LookRotation(lookAtTarget.position - _positions[0]);
			}
			else
			{
				cameraTransform.rotation = _rotations[0];
			}
		}

		public override void Evaluate(float normalizedTime, Transform cameraTransform)
		{
			if (_positions == null || _positions.Length < 2)
			{
				return;
			}
			float t = EaseTime(normalizedTime);
			if (smoothPath && _positions.Length >= 2)
			{
				cameraTransform.position = CatmullRomPosition(t);
				if (lookAtTarget != null)
				{
					Vector3 forward = lookAtTarget.position - cameraTransform.position;
					if (forward.sqrMagnitude > 0.001f)
					{
						cameraTransform.rotation = Quaternion.LookRotation(forward);
					}
				}
				else
				{
					cameraTransform.rotation = CatmullRomRotation(t);
				}
			}
			else
			{
				LinearInterpolate(t, cameraTransform);
			}
		}

		private Vector3 CatmullRomPosition(float t)
		{
			int num = _positions.Length;
			float num2 = t * (float)(num - 1);
			int value = Mathf.FloorToInt(num2);
			value = Mathf.Clamp(value, 0, num - 2);
			float t2 = num2 - (float)value;
			int num3 = Mathf.Max(value - 1, 0);
			int num4 = Mathf.Min(value + 1, num - 1);
			int num5 = Mathf.Min(value + 2, num - 1);
			return CatmullRom(_positions[num3], _positions[value], _positions[num4], _positions[num5], t2);
		}

		private Quaternion CatmullRomRotation(float t)
		{
			int num = _rotations.Length;
			float num2 = t * (float)(num - 1);
			int value = Mathf.FloorToInt(num2);
			value = Mathf.Clamp(value, 0, num - 2);
			float t2 = num2 - (float)value;
			int num3 = Mathf.Min(value + 1, num - 1);
			return Quaternion.Slerp(_rotations[value], _rotations[num3], t2);
		}

		private void LinearInterpolate(float t, Transform cameraTransform)
		{
			int num = _positions.Length;
			float num2 = t * (float)(num - 1);
			int value = Mathf.FloorToInt(num2);
			value = Mathf.Clamp(value, 0, num - 2);
			float t2 = num2 - (float)value;
			cameraTransform.position = Vector3.Lerp(_positions[value], _positions[value + 1], t2);
			if (lookAtTarget != null)
			{
				Vector3 forward = lookAtTarget.position - cameraTransform.position;
				if (forward.sqrMagnitude > 0.001f)
				{
					cameraTransform.rotation = Quaternion.LookRotation(forward);
				}
			}
			else
			{
				cameraTransform.rotation = Quaternion.Slerp(_rotations[value], _rotations[value + 1], t2);
			}
		}

		private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float num = t * t;
			float num2 = num * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (-p0 + 3f * p1 - 3f * p2 + p3) * num2);
		}
	}
}

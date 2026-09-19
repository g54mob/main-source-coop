using UnityEngine;

namespace Features.BarBeachInteractableModule.Scripts
{
	public class BarServePath : MonoBehaviour
	{
		[SerializeField]
		private Transform _startPoint;

		[SerializeField]
		private Transform _midPoint;

		[SerializeField]
		private Transform[] _servePoints;

		public Transform StartPoint => _startPoint;

		public Transform MidPoint => _midPoint;

		public int ServePointCount
		{
			get
			{
				if (_servePoints == null)
				{
					return 0;
				}
				return _servePoints.Length;
			}
		}

		public bool TryGetServePoint(int index, out Transform servePoint)
		{
			servePoint = null;
			if (_servePoints == null || index < 0 || index >= _servePoints.Length)
			{
				return false;
			}
			servePoint = _servePoints[index];
			return servePoint != null;
		}

		public Vector3 EvaluatePosition(float t, int serveIndex)
		{
			t = Mathf.Clamp01(t);
			Vector3 a = ((_startPoint != null) ? _startPoint.position : base.transform.position);
			Vector3 servePosition = GetServePosition(serveIndex);
			if (_midPoint == null)
			{
				return Vector3.Lerp(a, servePosition, t);
			}
			Vector3 a2 = Vector3.Lerp(a, _midPoint.position, t);
			Vector3 b = Vector3.Lerp(_midPoint.position, servePosition, t);
			return Vector3.Lerp(a2, b, t);
		}

		public Quaternion EvaluateRotation(float pathT, float spinT, float yawSpinDegrees, int serveIndex)
		{
			Quaternion quaternion = EvaluateFacing(pathT, serveIndex);
			float y = yawSpinDegrees * Mathf.Clamp01(spinT);
			return quaternion * Quaternion.Euler(0f, y, 0f);
		}

		public Quaternion EvaluateFacing(float t, int serveIndex)
		{
			Vector3 servePosition = GetServePosition(serveIndex);
			Vector3 vector = ((_startPoint != null) ? _startPoint.position : base.transform.position);
			Vector3 vector2 = servePosition - vector;
			vector2.y = 0f;
			if (vector2.sqrMagnitude > 0.0001f)
			{
				return Quaternion.LookRotation(vector2.normalized, Vector3.up);
			}
			if (TryGetServePoint(serveIndex, out var servePoint))
			{
				return servePoint.rotation;
			}
			return base.transform.rotation;
		}

		public Vector3 GetServePosition(int serveIndex)
		{
			if (TryGetServePoint(serveIndex, out var servePoint))
			{
				return servePoint.position;
			}
			return base.transform.position;
		}

		public Quaternion GetServeRotation(int serveIndex)
		{
			if (TryGetServePoint(serveIndex, out var servePoint))
			{
				return servePoint.rotation;
			}
			return base.transform.rotation;
		}
	}
}

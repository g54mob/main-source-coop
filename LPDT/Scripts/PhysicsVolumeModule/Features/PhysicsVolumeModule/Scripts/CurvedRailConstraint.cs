using System;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[RequireComponent(typeof(Rigidbody))]
	public class CurvedRailConstraint : MonoBehaviour
	{
		[SerializeField]
		private float _arcRadius = 12f;

		[SerializeField]
		private float _arcAngle = 30f;

		[SerializeField]
		private float _entryYawDegrees = 180f;

		[SerializeField]
		private bool _turnLeft = true;

		[SerializeField]
		private float _bumpScale = 1f;

		[SerializeField]
		private float _maxSpeed = 6f;

		[SerializeField]
		private int _sampleCount = 128;

		private Rigidbody _body;

		private Vector3[] _points;

		private bool _built;

		private void Awake()
		{
			_body = GetComponent<Rigidbody>();
		}

		public void Configure(float arcRadius, float arcAngle, float entryYawDegrees, bool turnLeft)
		{
			_arcRadius = arcRadius;
			_arcAngle = arcAngle;
			_entryYawDegrees = entryYawDegrees;
			_turnLeft = turnLeft;
			if (_body == null)
			{
				_body = GetComponent<Rigidbody>();
			}
			BuildCurve();
		}

		private void BuildCurve()
		{
			float num = Mathf.Max(0.1f, _arcRadius);
			Vector3 position = base.transform.position;
			Vector3 rhs = Quaternion.Euler(0f, _entryYawDegrees, 0f) * Vector3.forward;
			rhs.y = 0f;
			if (rhs.sqrMagnitude < 0.0001f)
			{
				rhs = Vector3.forward;
			}
			rhs.Normalize();
			Vector3 normalized = Vector3.Cross(Vector3.up, rhs).normalized;
			Vector3 vector = (_turnLeft ? normalized : (-normalized));
			Vector3 vector2 = position + vector * num;
			Vector3 vector3 = position - vector2;
			int num2 = Mathf.Max(2, _sampleCount);
			_points = new Vector3[num2];
			float num3 = (_turnLeft ? 1f : (-1f));
			for (int i = 0; i < num2; i++)
			{
				float num4 = (float)i / (float)(num2 - 1);
				Vector3 vector4 = Quaternion.AngleAxis(num3 * _arcAngle * num4, Vector3.up) * vector3;
				Vector3 vector5 = vector2 + vector4;
				vector5.y = position.y + _bumpScale * VerticalBump(num4);
				_points[i] = vector5;
			}
			_built = true;
			if (_body != null)
			{
				_body.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}

		private static float VerticalBump(float u)
		{
			return Pulse(u, 0.14f, 0.075f, 0.6f) + Pulse(u, 0.46f, 0.06f, 0.42f) + Pulse(u, 0.77f, 0.09f, 0.7f);
		}

		private static float Pulse(float u, float centre, float halfWidth, float height)
		{
			float num = Mathf.Abs(u - centre);
			if (num >= halfWidth)
			{
				return 0f;
			}
			float num2 = num / halfWidth;
			return height * 0.5f * (1f + Mathf.Cos(MathF.PI * num2));
		}

		private void Update()
		{
			if (!_built)
			{
				BuildCurve();
			}
		}

		private void FixedUpdate()
		{
			if (_body == null || _body.isKinematic)
			{
				return;
			}
			if (!_built)
			{
				BuildCurve();
			}
			if (_points != null)
			{
				ProjectOntoCurve(_body.position, out var closest, out var tangent);
				_body.position = closest;
				Vector3 linearVelocity = _body.linearVelocity;
				Vector3 linearVelocity2 = tangent * Vector3.Dot(linearVelocity, tangent);
				if (linearVelocity2.magnitude > _maxSpeed)
				{
					linearVelocity2 = linearVelocity2.normalized * _maxSpeed;
				}
				_body.linearVelocity = linearVelocity2;
				Vector3 vector = new Vector3(tangent.x, 0f, tangent.z);
				if (vector.sqrMagnitude > 0.0001f)
				{
					_body.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
				}
			}
		}

		private void ProjectOntoCurve(Vector3 position, out Vector3 closest, out Vector3 tangent)
		{
			closest = _points[0];
			tangent = (_points[1] - _points[0]).normalized;
			float num = float.MaxValue;
			for (int i = 0; i < _points.Length - 1; i++)
			{
				Vector3 vector = _points[i];
				Vector3 vector2 = _points[i + 1] - vector;
				float sqrMagnitude = vector2.sqrMagnitude;
				if (!(sqrMagnitude < 1E-06f))
				{
					float num2 = Mathf.Clamp01(Vector3.Dot(position - vector, vector2) / sqrMagnitude);
					Vector3 vector3 = vector + vector2 * num2;
					float sqrMagnitude2 = (position - vector3).sqrMagnitude;
					if (sqrMagnitude2 < num)
					{
						num = sqrMagnitude2;
						closest = vector3;
						tangent = vector2 / Mathf.Sqrt(sqrMagnitude);
					}
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (_points != null)
			{
				Gizmos.color = Color.cyan;
				for (int i = 0; i < _points.Length - 1; i++)
				{
					Gizmos.DrawLine(_points[i], _points[i + 1]);
				}
			}
		}
	}
}

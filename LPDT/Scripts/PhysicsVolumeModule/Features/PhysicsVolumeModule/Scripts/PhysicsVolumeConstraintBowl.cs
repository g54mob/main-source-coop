using System;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class PhysicsVolumeConstraintBowl : PhysicsVolumeConstraintShape
	{
		private const float SURFACE_INSET = 0.001f;

		private const int GIZMO_SEGMENTS = 48;

		private const int GIZMO_RINGS = 12;

		private const int GIZMO_MERIDIANS = 6;

		private static readonly Color _hardWallColor = new Color(1f, 0.55f, 0.1f, 0.95f);

		private static readonly Color _openColor = new Color(1f, 0.55f, 0.1f, 0.18f);

		private static readonly Color _rimColor = new Color(0.2f, 0.9f, 1f, 1f);

		private static readonly Color _floorColor = new Color(1f, 0.25f, 0.2f, 0.95f);

		private static readonly Color _centerColor = new Color(1f, 1f, 1f, 0.8f);

		[SerializeField]
		private Vector3 _center = Vector3.zero;

		[SerializeField]
		private float _radius = 0.5f;

		[Tooltip("Local height of the flat bottom, relative to the centre. Cargo may never cross it. Set it at or below -radius to let the sphere close the bottom on its own.")]
		[SerializeField]
		private float _floorOffset = -0.35f;

		[Tooltip("Local height above which the wall is OPEN, relative to the centre. Cargo lifted out over the rim leaves freely; below it the wall is hard. Set at or above +radius for a fully closed sphere.")]
		[SerializeField]
		private float _rimOffset = 0.35f;

		[SerializeField]
		private bool _isWallHard = true;

		[SerializeField]
		private bool _isFloorHard = true;

		public override bool HasHardSurface
		{
			get
			{
				if (_isWallHard || _isFloorHard)
				{
					return _radius > 0f;
				}
				return false;
			}
		}

		public float Radius => _radius;

		public float RimOffset => _rimOffset;

		public float FloorOffset => _floorOffset;

		public Vector3 Center => _center;

		public bool IsWallHard => _isWallHard;

		public bool IsFloorHard => _isFloorHard;

		public override bool ContainsColumnLocal(Vector3 localPoint, float columnHeight)
		{
			Vector3 vector = localPoint - _center;
			if (new Vector2(vector.x, vector.z).magnitude <= _radius && vector.y >= _floorOffset)
			{
				return vector.y <= _floorOffset + columnHeight;
			}
			return false;
		}

		public override bool TryGetFloorCentreLocal(out Vector3 floorCentreLocal)
		{
			floorCentreLocal = _center + new Vector3(0f, _floorOffset, 0f);
			return true;
		}

		public override bool ContainsLocal(Vector3 localPoint)
		{
			Vector3 vector = localPoint - _center;
			if (vector.sqrMagnitude <= _radius * _radius)
			{
				return vector.y >= _floorOffset;
			}
			return false;
		}

		public override bool ContainsWithinHardSurfaces(Vector3 localPoint)
		{
			if (_isWallHard && ContainsLocal(localPoint))
			{
				return localPoint.y - _center.y <= _rimOffset;
			}
			return false;
		}

		public override bool TryConstrain(Vector3 previousLocal, Vector3 currentLocal, out Vector3 constrainedLocal, out Vector3 outwardWorld)
		{
			constrainedLocal = currentLocal;
			outwardWorld = Vector3.zero;
			if (!HasHardSurface)
			{
				return false;
			}
			if (ContainsLocal(currentLocal) || !ContainsLocal(previousLocal))
			{
				return false;
			}
			Vector3 vector = previousLocal - _center;
			Vector3 to = currentLocal - _center;
			float t = 0f;
			float t2 = 0f;
			bool flag = _isWallHard && TryWallCrossing(vector, to, out t);
			bool flag2 = _isFloorHard && TryFloorCrossing(vector, to, out t2);
			if (!flag && !flag2)
			{
				return false;
			}
			if (flag && (!flag2 || t <= t2))
			{
				if (vector.y + (to.y - vector.y) * t > _rimOffset)
				{
					return false;
				}
				Vector3 vector2 = ((to.sqrMagnitude > Mathf.Epsilon) ? to.normalized : Vector3.up);
				Vector3 vector3 = vector2 * (_radius - 0.001f);
				vector3.y = Mathf.Max(vector3.y, _floorOffset + 0.001f);
				constrainedLocal = _center + vector3;
				outwardWorld = base.transform.TransformDirection(vector2).normalized;
				return true;
			}
			Vector3 vector4 = new Vector3(currentLocal.x - _center.x, _floorOffset + 0.001f, currentLocal.z - _center.z);
			float num = Mathf.Max(0f, RingRadiusAt(vector4.y) - 0.001f);
			Vector2 vector5 = new Vector2(vector4.x, vector4.z);
			if (vector5.magnitude > num)
			{
				vector5 = vector5.normalized * num;
				vector4.x = vector5.x;
				vector4.z = vector5.y;
			}
			constrainedLocal = _center + vector4;
			outwardWorld = -base.transform.up;
			return true;
		}

		public override string DescribeExit(Vector3 previousLocal, Vector3 currentLocal)
		{
			if (ContainsLocal(currentLocal) || !ContainsLocal(previousLocal))
			{
				return null;
			}
			Vector3 vector = previousLocal - _center;
			Vector3 to = currentLocal - _center;
			float t = 0f;
			float t2 = 0f;
			bool flag = TryWallCrossing(vector, to, out t);
			bool flag2 = TryFloorCrossing(vector, to, out t2);
			if (!flag && !flag2)
			{
				return null;
			}
			if (flag && (!flag2 || t <= t2))
			{
				if (!(vector.y + (to.y - vector.y) * t > _rimOffset))
				{
					return "Wall:" + (_isWallHard ? "HARD" : "open");
				}
				return "Cap:open";
			}
			return "Floor:" + (_isFloorHard ? "HARD" : "open");
		}

		private bool TryWallCrossing(Vector3 from, Vector3 to, out float t)
		{
			t = 0f;
			Vector3 rhs = to - from;
			float sqrMagnitude = rhs.sqrMagnitude;
			if (sqrMagnitude < Mathf.Epsilon)
			{
				return false;
			}
			float num = 2f * Vector3.Dot(from, rhs);
			float num2 = from.sqrMagnitude - _radius * _radius;
			float num3 = num * num - 4f * sqrMagnitude * num2;
			if (num3 < 0f)
			{
				return false;
			}
			float num4 = (0f - num + Mathf.Sqrt(num3)) / (2f * sqrMagnitude);
			if (num4 < 0f || num4 > 1f)
			{
				return false;
			}
			t = num4;
			return true;
		}

		private bool TryFloorCrossing(Vector3 from, Vector3 to, out float t)
		{
			t = 0f;
			float num = to.y - from.y;
			if (num >= 0f - Mathf.Epsilon || to.y >= _floorOffset)
			{
				return false;
			}
			t = (_floorOffset - from.y) / num;
			if (t >= 0f)
			{
				return t <= 1f;
			}
			return false;
		}

		public float RingRadiusAt(float heightOffset)
		{
			float num = _radius * _radius - heightOffset * heightOffset;
			if (!(num <= 0f))
			{
				return Mathf.Sqrt(num);
			}
			return 0f;
		}

		private void OnDrawGizmos()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			float toHeight = Mathf.Min(_rimOffset, _radius);
			for (int i = 0; i <= 12; i++)
			{
				float num = Mathf.Lerp(0f - _radius, _radius, (float)i / 12f);
				bool flag = num > _rimOffset || !_isWallHard;
				if (!(num < _floorOffset))
				{
					Gizmos.color = (flag ? _openColor : _hardWallColor);
					DrawRing(num, RingRadiusAt(num));
				}
			}
			Gizmos.color = (_isWallHard ? _hardWallColor : _openColor);
			for (int j = 0; j < 6; j++)
			{
				float f = (float)j / 6f * MathF.PI;
				DrawMeridian(Mathf.Cos(f), Mathf.Sin(f), Mathf.Max(_floorOffset, 0f - _radius), toHeight);
			}
			Gizmos.color = _rimColor;
			DrawRing(_rimOffset, RingRadiusAt(_rimOffset));
			Gizmos.color = (_isFloorHard ? _floorColor : _openColor);
			float num2 = RingRadiusAt(_floorOffset);
			DrawRing(_floorOffset, num2);
			for (int k = 0; k < 6; k++)
			{
				float f2 = (float)k / 6f * MathF.PI;
				Vector3 vector = new Vector3(Mathf.Cos(f2), 0f, Mathf.Sin(f2)) * num2;
				Gizmos.DrawLine(_center + vector + Vector3.up * _floorOffset, _center - vector + Vector3.up * _floorOffset);
			}
			Gizmos.color = _centerColor;
			float num3 = _radius * 0.08f;
			Gizmos.DrawLine(_center - Vector3.right * num3, _center + Vector3.right * num3);
			Gizmos.DrawLine(_center - Vector3.up * num3, _center + Vector3.up * num3);
			Gizmos.DrawLine(_center - Vector3.forward * num3, _center + Vector3.forward * num3);
			Gizmos.DrawLine(_center, _center + Vector3.right * _radius);
		}

		private void DrawRing(float heightOffset, float ringRadius)
		{
			if (!(ringRadius <= 0f))
			{
				Vector3 vector = _center + new Vector3(ringRadius, heightOffset, 0f);
				for (int i = 1; i <= 48; i++)
				{
					float f = (float)i / 48f * MathF.PI * 2f;
					Vector3 vector2 = _center + new Vector3(Mathf.Cos(f) * ringRadius, heightOffset, Mathf.Sin(f) * ringRadius);
					Gizmos.DrawLine(vector, vector2);
					vector = vector2;
				}
			}
		}

		private void DrawMeridian(float dirX, float dirZ, float fromHeight, float toHeight)
		{
			Vector3 vector = Vector3.zero;
			bool flag = false;
			for (int i = 0; i <= 48; i++)
			{
				float num = Mathf.Lerp(fromHeight, toHeight, (float)i / 48f);
				float num2 = RingRadiusAt(num);
				Vector3 vector2 = _center + new Vector3(dirX * num2, num, dirZ * num2);
				if (flag)
				{
					Gizmos.DrawLine(vector, vector2);
				}
				vector = vector2;
				flag = true;
			}
		}
	}
}

using System;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class PhysicsVolumeLockZone : MonoBehaviour
	{
		private const int GIZMO_SEGMENTS = 32;

		[SerializeField]
		private Vector3 _center = Vector3.zero;

		[SerializeField]
		private Vector3 _size = new Vector3(2f, 0.3f, 2f);

		[SerializeField]
		private PhysicsVolumeRegionShape _shape = PhysicsVolumeRegionShape.Box;

		public Vector3 Center => _center;

		public Vector3 Size => _size;

		public PhysicsVolumeRegionShape Shape => _shape;

		public bool Contains(Vector3 worldPoint)
		{
			Vector3 vector = base.transform.InverseTransformPoint(worldPoint) - _center;
			Vector3 vector2 = _size * 0.5f;
			if (Mathf.Abs(vector.y) > vector2.y)
			{
				return false;
			}
			if (_shape == PhysicsVolumeRegionShape.Cylinder)
			{
				return vector.x * vector.x + vector.z * vector.z <= vector2.x * vector2.x;
			}
			if (_shape == PhysicsVolumeRegionShape.Capsule)
			{
				float num = Mathf.Max(0f, vector2.y - vector2.x);
				float num2 = vector.y - Mathf.Clamp(vector.y, 0f - num, num);
				return vector.x * vector.x + vector.z * vector.z + num2 * num2 <= vector2.x * vector2.x;
			}
			if (Mathf.Abs(vector.x) <= vector2.x)
			{
				return Mathf.Abs(vector.z) <= vector2.z;
			}
			return false;
		}

		private void OnDrawGizmos()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(0.35f, 1f, 0.45f, 0.9f);
			if (_shape == PhysicsVolumeRegionShape.Cylinder || _shape == PhysicsVolumeRegionShape.Capsule)
			{
				float num = _size.x * 0.5f;
				DrawCircle(_center + Vector3.up * (_size.y * 0.5f), num);
				DrawCircle(_center - Vector3.up * (_size.y * 0.5f), num);
				DrawCircle(_center, num);
				for (int i = 0; i < 4; i++)
				{
					float f = (float)i * MathF.PI * 0.5f;
					Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * num;
					Gizmos.DrawLine(_center + vector - Vector3.up * (_size.y * 0.5f), _center + vector + Vector3.up * (_size.y * 0.5f));
				}
			}
			else
			{
				Gizmos.DrawWireCube(_center, _size);
				Gizmos.color = new Color(0.35f, 1f, 0.45f, 0.12f);
				Gizmos.DrawCube(_center, _size);
			}
		}

		private static void DrawCircle(Vector3 center, float radius)
		{
			Vector3 vector = center + new Vector3(radius, 0f, 0f);
			for (int i = 1; i <= 32; i++)
			{
				float f = (float)i / 32f * MathF.PI * 2f;
				Vector3 vector2 = center + new Vector3(Mathf.Cos(f) * radius, 0f, Mathf.Sin(f) * radius);
				Gizmos.DrawLine(vector, vector2);
				vector = vector2;
			}
		}
	}
}

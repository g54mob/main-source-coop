using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class PhysicsVolumeRegion : MonoBehaviour
	{
		private const int MAX_HITS = 128;

		[SerializeField]
		private Vector3 _center = Vector3.zero;

		[SerializeField]
		private Vector3 _size = new Vector3(2f, 1.5f, 2f);

		[SerializeField]
		private PhysicsVolumeRegionShape _shape = PhysicsVolumeRegionShape.Box;

		[SerializeField]
		private Transform _carrierRootOverride;

		private readonly Collider[] _hits = new Collider[128];

		private static readonly string[] NON_CARGO_LAYER_NAMES = new string[9] { "Ground", "Wall", "Floor", "WindowGeometry", "NavMeshProp", "Gate", "Water", "ExcludeFog", "UI" };

		private static int _cargoQueryMask;

		private static bool _hasCargoQueryMask;

		private readonly HashSet<Rigidbody> _current = new HashSet<Rigidbody>();

		private readonly HashSet<Rigidbody> _previous = new HashSet<Rigidbody>();

		private readonly HashSet<Rigidbody> _seam = new HashSet<Rigidbody>();

		private readonly HashSet<Rigidbody> _seamPrevious = new HashSet<Rigidbody>();

		private readonly List<Rigidbody> _changed = new List<Rigidbody>();

		private readonly HashSet<Rigidbody> _scanned = new HashSet<Rigidbody>();

		private static int CargoQueryMask
		{
			get
			{
				if (_hasCargoQueryMask)
				{
					return _cargoQueryMask;
				}
				_cargoQueryMask = -1;
				for (int i = 0; i < 32; i++)
				{
					string value = LayerMask.LayerToName(i);
					if (!string.IsNullOrEmpty(value) && Array.IndexOf(NON_CARGO_LAYER_NAMES, value) >= 0)
					{
						_cargoQueryMask &= ~(1 << i);
					}
				}
				_hasCargoQueryMask = true;
				return _cargoQueryMask;
			}
		}

		public IReadOnlyCollection<Rigidbody> Bodies => _current;

		public Transform CarrierRoot
		{
			get
			{
				if (!(_carrierRootOverride != null))
				{
					if (!(base.transform.parent != null))
					{
						return base.transform;
					}
					return base.transform.parent;
				}
				return _carrierRootOverride;
			}
		}

		public float FloorWorldY => base.transform.TransformPoint(_center).y - _size.y * base.transform.lossyScale.y * 0.5f;

		public float WorldMeasure
		{
			get
			{
				Vector3 vector = Vector3.Scale(_size, base.transform.lossyScale);
				if (_shape == PhysicsVolumeRegionShape.Cylinder)
				{
					return Mathf.Abs(MathF.PI * (vector.x * 0.5f) * (vector.z * 0.5f) * vector.y);
				}
				if (_shape == PhysicsVolumeRegionShape.Capsule)
				{
					float num = Mathf.Abs(vector.x) * 0.5f;
					float num2 = Mathf.Max(0f, Mathf.Abs(vector.y) - num * 2f);
					return MathF.PI * num * num * num2 + 4.1887903f * num * num * num;
				}
				return Mathf.Abs(vector.x * vector.y * vector.z);
			}
		}

		public event Action<Rigidbody> OnBodyEntered;

		public event Action<Rigidbody> OnBodyExited;

		public event Action<Rigidbody> OnSeamEntered;

		public event Action<Rigidbody> OnSeamExited;

		public bool Sees(Rigidbody body)
		{
			return _current.Contains(body);
		}

		public bool SeamSees(Rigidbody body)
		{
			return _seam.Contains(body);
		}

		public bool ContainsWithFloorSkin(Vector3 worldPoint, float floorSkin)
		{
			Vector3 vector = base.transform.InverseTransformPoint(worldPoint) - _center;
			Vector3 vector2 = _size * 0.5f;
			float num = floorSkin / Mathf.Max(0.0001f, Mathf.Abs(base.transform.lossyScale.y));
			if (vector.y > vector2.y || vector.y < 0f - vector2.y - num)
			{
				return false;
			}
			if (_shape == PhysicsVolumeRegionShape.Cylinder)
			{
				return vector.x * vector.x + vector.z * vector.z <= vector2.x * vector2.x;
			}
			if (_shape == PhysicsVolumeRegionShape.Capsule)
			{
				float num2 = Mathf.Max(0f, vector2.y - vector2.x);
				float num3 = Mathf.Clamp(vector.y, 0f - num2 - num, num2);
				float num4 = vector.y - num3;
				return vector.x * vector.x + vector.z * vector.z + num4 * num4 <= vector2.x * vector2.x;
			}
			if (Mathf.Abs(vector.x) <= vector2.x)
			{
				return Mathf.Abs(vector.z) <= vector2.z;
			}
			return false;
		}

		public bool ContainsPadded(Vector3 worldPoint, float padding)
		{
			Vector3 vector = base.transform.InverseTransformPoint(worldPoint) - _center;
			Vector3 vector2 = _size * 0.5f;
			Vector3 lossyScale = base.transform.lossyScale;
			float num = padding / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.x));
			float num2 = padding / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.y));
			float num3 = padding / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.z));
			if (Mathf.Abs(vector.y) > vector2.y + num2)
			{
				return false;
			}
			if (_shape == PhysicsVolumeRegionShape.Cylinder || _shape == PhysicsVolumeRegionShape.Capsule)
			{
				float num4 = vector2.x + num;
				return vector.x * vector.x + vector.z * vector.z <= num4 * num4;
			}
			if (Mathf.Abs(vector.x) <= vector2.x + num)
			{
				return Mathf.Abs(vector.z) <= vector2.z + num3;
			}
			return false;
		}

		public float DistanceOutside(Vector3 worldPoint)
		{
			Vector3 vector = base.transform.InverseTransformPoint(worldPoint) - _center;
			Vector3 vector2 = _size * 0.5f;
			Vector3 lossyScale = base.transform.lossyScale;
			float num = Mathf.Max(0f, Mathf.Abs(vector.y) - vector2.y) * Mathf.Abs(lossyScale.y);
			if (_shape == PhysicsVolumeRegionShape.Cylinder || _shape == PhysicsVolumeRegionShape.Capsule)
			{
				float b = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z) - vector2.x;
				float num2 = Mathf.Max(0f, b) * Mathf.Abs(lossyScale.x);
				return Mathf.Sqrt(num2 * num2 + num * num);
			}
			float num3 = Mathf.Max(0f, Mathf.Abs(vector.x) - vector2.x) * Mathf.Abs(lossyScale.x);
			float num4 = Mathf.Max(0f, Mathf.Abs(vector.z) - vector2.z) * Mathf.Abs(lossyScale.z);
			return Mathf.Sqrt(num3 * num3 + num * num + num4 * num4);
		}

		public bool ContainsXZ(Vector3 worldPoint, float margin)
		{
			Vector3 vector = base.transform.InverseTransformPoint(worldPoint) - _center;
			Vector3 vector2 = _size * 0.5f;
			float num = margin / Mathf.Max(0.0001f, Mathf.Abs(base.transform.lossyScale.x));
			if (_shape == PhysicsVolumeRegionShape.Cylinder || _shape == PhysicsVolumeRegionShape.Capsule)
			{
				float num2 = vector2.x + num;
				return vector.x * vector.x + vector.z * vector.z <= num2 * num2;
			}
			if (Mathf.Abs(vector.x) <= vector2.x + num)
			{
				return Mathf.Abs(vector.z) <= vector2.z + num;
			}
			return false;
		}

		private void FixedUpdate()
		{
			_previous.Clear();
			foreach (Rigidbody item in _current)
			{
				_previous.Add(item);
			}
			_current.Clear();
			_seamPrevious.Clear();
			foreach (Rigidbody item2 in _seam)
			{
				_seamPrevious.Add(item2);
			}
			_seam.Clear();
			Vector3 vector = base.transform.TransformPoint(_center);
			Vector3 halfExtents = Vector3.Scale(_size, base.transform.lossyScale) * 0.5f;
			int cargoQueryMask = CargoQueryMask;
			int num3;
			if (_shape == PhysicsVolumeRegionShape.Capsule)
			{
				float num = Mathf.Abs(halfExtents.x);
				float num2 = Mathf.Max(0f, Mathf.Abs(halfExtents.y) - num);
				Vector3 vector2 = base.transform.rotation * Vector3.up * num2;
				num3 = Physics.OverlapCapsuleNonAlloc(vector - vector2, vector + vector2, num, _hits, cargoQueryMask, QueryTriggerInteraction.Ignore);
			}
			else
			{
				num3 = Physics.OverlapBoxNonAlloc(vector, halfExtents, _hits, base.transform.rotation, cargoQueryMask, QueryTriggerInteraction.Ignore);
			}
			_scanned.Clear();
			for (int i = 0; i < num3; i++)
			{
				Rigidbody attachedRigidbody = _hits[i].attachedRigidbody;
				if (!(attachedRigidbody == null) && _scanned.Add(attachedRigidbody) && !IsCarrierBody(attachedRigidbody))
				{
					_seam.Add(attachedRigidbody);
					if (ContainsWithFloorSkin(attachedRigidbody.worldCenterOfMass, 0f))
					{
						_current.Add(attachedRigidbody);
					}
				}
			}
			_changed.Clear();
			foreach (Rigidbody item3 in _seam)
			{
				if (!_seamPrevious.Contains(item3))
				{
					_changed.Add(item3);
				}
			}
			foreach (Rigidbody item4 in _changed)
			{
				this.OnSeamEntered?.Invoke(item4);
			}
			_changed.Clear();
			foreach (Rigidbody item5 in _current)
			{
				if (!_previous.Contains(item5))
				{
					_changed.Add(item5);
				}
			}
			foreach (Rigidbody item6 in _changed)
			{
				this.OnBodyEntered?.Invoke(item6);
			}
			_changed.Clear();
			foreach (Rigidbody previou in _previous)
			{
				if (!_current.Contains(previou))
				{
					_changed.Add(previou);
				}
			}
			foreach (Rigidbody item7 in _changed)
			{
				this.OnBodyExited?.Invoke(item7);
			}
			_changed.Clear();
			foreach (Rigidbody seamPreviou in _seamPrevious)
			{
				if (!_seam.Contains(seamPreviou))
				{
					_changed.Add(seamPreviou);
				}
			}
			foreach (Rigidbody item8 in _changed)
			{
				this.OnSeamExited?.Invoke(item8);
			}
		}

		private bool IsCarrierBody(Rigidbody body)
		{
			if (!(body.transform == CarrierRoot))
			{
				return body.transform.IsChildOf(CarrierRoot);
			}
			return true;
		}

		private void OnDrawGizmos()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.9f);
			if (_shape == PhysicsVolumeRegionShape.Cylinder)
			{
				DrawGizmoCircle(_center + Vector3.up * (_size.y * 0.5f), _size.x * 0.5f);
				DrawGizmoCircle(_center - Vector3.up * (_size.y * 0.5f), _size.x * 0.5f);
				for (int i = 0; i < 4; i++)
				{
					float f = (float)i * MathF.PI * 0.5f;
					Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * (_size.x * 0.5f);
					Gizmos.DrawLine(_center + vector - Vector3.up * (_size.y * 0.5f), _center + vector + Vector3.up * (_size.y * 0.5f));
				}
			}
			else if (_shape == PhysicsVolumeRegionShape.Capsule)
			{
				float num = _size.x * 0.5f;
				float num2 = Mathf.Max(0f, _size.y * 0.5f - num);
				Vector3 vector2 = _center + Vector3.up * num2;
				Vector3 vector3 = _center - Vector3.up * num2;
				DrawGizmoCircle(vector2, num);
				DrawGizmoCircle(vector3, num);
				for (int j = 0; j < 4; j++)
				{
					float f2 = (float)j * MathF.PI * 0.5f;
					Vector3 vector4 = new Vector3(Mathf.Cos(f2), 0f, Mathf.Sin(f2)) * num;
					Gizmos.DrawLine(vector3 + vector4, vector2 + vector4);
				}
				DrawGizmoHemisphere(vector2, num, 1f);
				DrawGizmoHemisphere(vector3, num, -1f);
			}
			else
			{
				Gizmos.DrawWireCube(_center, _size);
				Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.08f);
				Gizmos.DrawCube(_center, _size);
			}
		}

		private static void DrawGizmoHemisphere(Vector3 capCenter, float radius, float upSign)
		{
			Vector3 vector = capCenter + new Vector3(radius, 0f, 0f);
			Vector3 vector2 = capCenter + new Vector3(0f, 0f, radius);
			for (int i = 1; i <= 16; i++)
			{
				float f = (float)i / 16f * MathF.PI;
				float num = Mathf.Cos(f);
				float num2 = Mathf.Sin(f) * upSign;
				Vector3 vector3 = capCenter + new Vector3(num * radius, num2 * radius, 0f);
				Vector3 vector4 = capCenter + new Vector3(0f, num2 * radius, num * radius);
				Gizmos.DrawLine(vector, vector3);
				Gizmos.DrawLine(vector2, vector4);
				vector = vector3;
				vector2 = vector4;
			}
		}

		private static void DrawGizmoCircle(Vector3 center, float radius)
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

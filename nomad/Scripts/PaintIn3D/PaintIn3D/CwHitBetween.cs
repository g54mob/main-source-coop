using System;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwHitBetween")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Between")]
	public class CwHitBetween : MonoBehaviour
	{
		public enum PhaseType
		{
			Update = 0,
			FixedUpdate = 1
		}

		public enum OrientationType
		{
			WorldUp = 0,
			CameraUp = 1,
			ThisRotation = 2,
			ThisLocalRotation = 3,
			CustomRotation = 4,
			CustomLocalRotation = 5
		}

		public enum NormalType
		{
			HitNormal = 0,
			RayDirection = 1
		}

		public enum EmitType
		{
			PointsIn3D = 0,
			PointsOnUV = 20,
			TrianglesIn3D = 30
		}

		[SerializeField]
		private PhaseType paintIn;

		[SerializeField]
		private float interval = 0.05f;

		[SerializeField]
		private Transform pointA;

		[SerializeField]
		private Transform pointB;

		[SerializeField]
		private float fraction = 1f;

		[SerializeField]
		private LayerMask layers = -5;

		[SerializeField]
		private OrientationType orientation;

		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private Transform customTransform;

		[SerializeField]
		private NormalType normal;

		[SerializeField]
		private float offset;

		[SerializeField]
		private bool preview;

		[SerializeField]
		private int priority;

		[Range(0f, 1f)]
		[SerializeField]
		private float pressure = 1f;

		[SerializeField]
		private EmitType emit;

		[SerializeField]
		private Transform point;

		[SerializeField]
		private LineRenderer line;

		[SerializeField]
		private CwPointConnector connector;

		[NonSerialized]
		private float current;

		public PhaseType PaintIn
		{
			get
			{
				return paintIn;
			}
			set
			{
				paintIn = value;
			}
		}

		public float Interval
		{
			get
			{
				return interval;
			}
			set
			{
				interval = value;
			}
		}

		public Transform PointA
		{
			get
			{
				return pointA;
			}
			set
			{
				pointA = value;
			}
		}

		public Transform PointB
		{
			get
			{
				return pointB;
			}
			set
			{
				pointB = value;
			}
		}

		public float Fraction => fraction;

		public LayerMask Layers
		{
			get
			{
				return layers;
			}
			set
			{
				layers = value;
			}
		}

		public OrientationType Orientation
		{
			get
			{
				return orientation;
			}
			set
			{
				orientation = value;
			}
		}

		public Camera Camera
		{
			get
			{
				return _camera;
			}
			set
			{
				_camera = value;
			}
		}

		public Transform CustomTransform
		{
			get
			{
				return customTransform;
			}
			set
			{
				customTransform = value;
			}
		}

		public NormalType Normal
		{
			get
			{
				return normal;
			}
			set
			{
				normal = value;
			}
		}

		public float Offset
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
			}
		}

		public bool Preview
		{
			get
			{
				return preview;
			}
			set
			{
				preview = value;
			}
		}

		public int Priority
		{
			get
			{
				return priority;
			}
			set
			{
				priority = value;
			}
		}

		public float Pressure
		{
			get
			{
				return pressure;
			}
			set
			{
				pressure = value;
			}
		}

		public EmitType Draw
		{
			get
			{
				return emit;
			}
			set
			{
				emit = value;
			}
		}

		public Transform Point
		{
			get
			{
				return point;
			}
			set
			{
				point = value;
			}
		}

		public LineRenderer Line
		{
			get
			{
				return line;
			}
			set
			{
				line = value;
			}
		}

		public CwPointConnector Connector
		{
			get
			{
				if (connector == null)
				{
					connector = new CwPointConnector();
				}
				return connector;
			}
		}

		[ContextMenu("Manually Hit Now")]
		public void ManuallyHitNow()
		{
			SubmitHit(preview: false);
		}

		[ContextMenu("Clear Hit Cache")]
		public void ClearHitCache()
		{
			Connector.ClearHitCache();
		}

		[ContextMenu("Reset Connections")]
		public void ResetConnections()
		{
			connector.ResetConnections();
		}

		protected virtual void OnEnable()
		{
			Connector.ResetConnections();
		}

		protected virtual void OnDisable()
		{
			if (point != null && pointB != null)
			{
				point.position = pointB.position;
			}
		}

		protected virtual void Update()
		{
			connector.Update();
			if (preview)
			{
				SubmitHit(preview: true);
			}
			else if (paintIn == PhaseType.Update)
			{
				UpdateHit();
			}
		}

		protected virtual void LateUpdate()
		{
			UpdatePointAndLine();
		}

		protected virtual void FixedUpdate()
		{
			if (!preview && paintIn == PhaseType.FixedUpdate)
			{
				UpdateHit();
			}
		}

		private void SubmitHit(bool preview)
		{
			if (!(pointA != null) || !(pointB != null))
			{
				return;
			}
			Vector3 direction = pointB.position - pointA.position;
			float magnitude = direction.magnitude;
			Ray ray = new Ray(pointA.position, direction);
			RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(ray, 1f / 0f, layers);
			RaycastHit hitInfo = default(RaycastHit);
			Vector3 finalPosition = default(Vector3);
			Quaternion finalRotation = default(Quaternion);
			if (Physics.Raycast(ray, out hitInfo, magnitude, layers) && (rayIntersection.collider == null || hitInfo.distance < rayIntersection.distance))
			{
				CalcHitData(hitInfo.point, hitInfo.normal, ray, out finalPosition, out finalRotation);
				fraction = (hitInfo.distance + offset) / magnitude;
				if (emit == EmitType.PointsIn3D)
				{
					connector.SubmitPoint(base.gameObject, preview, priority, pressure, finalPosition, finalRotation, this);
				}
				else if (emit == EmitType.PointsOnUV)
				{
					connector.HitCache.InvokeCoord(base.gameObject, preview, priority, pressure, new CwHit(hitInfo), finalRotation);
				}
				else if (emit == EmitType.TrianglesIn3D)
				{
					connector.HitCache.InvokeTriangle(base.gameObject, preview, priority, pressure, new CwHit(hitInfo), finalRotation);
				}
			}
			else if (rayIntersection.collider != null)
			{
				CalcHitData(rayIntersection.point, rayIntersection.normal, ray, out finalPosition, out finalRotation);
				fraction = (hitInfo.distance + offset) / magnitude;
				if (emit == EmitType.PointsIn3D)
				{
					connector.SubmitPoint(base.gameObject, preview, priority, pressure, finalPosition, finalRotation, this);
				}
			}
			else
			{
				connector.BreakHits(this);
				fraction = 1f;
			}
		}

		private void CalcHitData(Vector3 hitPoint, Vector3 hitNormal, Ray ray, out Vector3 finalPosition, out Quaternion finalRotation)
		{
			finalPosition = hitPoint + hitNormal * offset;
			switch (orientation)
			{
			case OrientationType.WorldUp:
			{
				Vector3 up = Vector3.up;
				Vector3 vector2 = ((normal == NormalType.HitNormal) ? hitNormal : (-ray.direction));
				finalRotation = Quaternion.LookRotation(-vector2, up);
				return;
			}
			case OrientationType.CameraUp:
			{
				Vector3 cameraUp = PaintCore.CwCommon.GetCameraUp(_camera);
				Vector3 vector = ((normal == NormalType.HitNormal) ? hitNormal : (-ray.direction));
				finalRotation = Quaternion.LookRotation(-vector, cameraUp);
				return;
			}
			case OrientationType.ThisRotation:
				finalRotation = base.transform.rotation;
				return;
			case OrientationType.ThisLocalRotation:
				finalRotation = base.transform.localRotation;
				return;
			case OrientationType.CustomRotation:
				if (customTransform != null)
				{
					finalRotation = customTransform.rotation;
					return;
				}
				break;
			case OrientationType.CustomLocalRotation:
				if (customTransform != null)
				{
					finalRotation = customTransform.localRotation;
					return;
				}
				break;
			}
			finalRotation = Quaternion.identity;
		}

		private void UpdatePointAndLine()
		{
			if (pointA != null && pointB != null)
			{
				Vector3 position = pointA.position;
				Vector3 position2 = pointB.position;
				Vector3 position3 = Vector3.Lerp(position, position2, fraction);
				if (point != null)
				{
					point.position = position3;
				}
				if (line != null)
				{
					line.positionCount = 2;
					line.SetPosition(0, position);
					line.SetPosition(1, position3);
				}
			}
		}

		private void UpdateHit()
		{
			current += Time.deltaTime;
			if (interval > 0f)
			{
				if (current >= interval)
				{
					current %= interval;
					SubmitHit(preview: false);
				}
			}
			else if (interval == 0f)
			{
				SubmitHit(preview: false);
			}
		}
	}
}

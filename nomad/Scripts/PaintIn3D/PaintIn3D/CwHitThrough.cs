using System;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwHitThrough")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Through")]
	public class CwHitThrough : MonoBehaviour
	{
		public enum PhaseType
		{
			Update = 0,
			FixedUpdate = 1
		}

		public enum OrientationType
		{
			WorldUp = 0,
			CameraUp = 1
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
		private OrientationType orientation;

		[SerializeField]
		private Camera _camera;

		[Range(0f, 1f)]
		[SerializeField]
		private float pressure = 1f;

		[SerializeField]
		private bool preview;

		[SerializeField]
		private int priority;

		[SerializeField]
		private LineRenderer line;

		[SerializeField]
		private CwLineConnector connector;

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

		public CwLineConnector Connector
		{
			get
			{
				if (connector == null)
				{
					connector = new CwLineConnector();
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
			if (pointA != null && pointB != null)
			{
				Camera camera = CwHelper.GetCamera(_camera);
				Vector3 position = pointA.position;
				Vector3 position2 = pointB.position;
				Vector3 upwards = ((orientation == OrientationType.CameraUp && camera != null) ? camera.transform.up : Vector3.up);
				Vector3 vector = position2 - position;
				Quaternion rotation = ((vector != Vector3.zero) ? Quaternion.LookRotation(vector, upwards) : Quaternion.identity);
				connector.SubmitLine(base.gameObject, preview, priority, pressure, pointA.position, pointB.position, rotation, this);
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

		private void UpdatePointAndLine()
		{
			if (pointA != null && pointB != null)
			{
				Vector3 position = pointA.position;
				Vector3 position2 = pointB.position;
				if (line != null)
				{
					line.positionCount = 2;
					line.SetPosition(0, position);
					line.SetPosition(1, position2);
				}
			}
		}
	}
}

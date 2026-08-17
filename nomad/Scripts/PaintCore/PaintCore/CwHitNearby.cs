using System;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwHitNearby")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Nearby")]
	public class CwHitNearby : MonoBehaviour
	{
		public enum PhaseType
		{
			ManuallyOnly = -1,
			Update = 0,
			FixedUpdate = 1,
			Start = 2
		}

		[SerializeField]
		private PhaseType paintIn;

		[SerializeField]
		private float interval = 0.05f;

		[SerializeField]
		private bool preview;

		[SerializeField]
		private int priority;

		[Range(0f, 1f)]
		[SerializeField]
		private float pressure = 1f;

		[SerializeField]
		private CwPointConnector connector;

		[NonSerialized]
		private float current;

		[SerializeField]
		private Vector3 lastPosition;

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

		protected virtual void Start()
		{
			if (paintIn == PhaseType.Start)
			{
				paintIn = PhaseType.ManuallyOnly;
				SubmitHit(preview: false);
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

		protected virtual void FixedUpdate()
		{
			if (!preview && paintIn == PhaseType.FixedUpdate)
			{
				UpdateHit();
			}
		}

		private void SubmitHit(bool preview)
		{
			connector.SubmitPoint(base.gameObject, preview, priority, pressure, base.transform.position, base.transform.rotation, this);
		}

		private void UpdateHit()
		{
			current += (Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime);
			if (interval > 0f)
			{
				if (current >= interval)
				{
					current %= interval;
					SubmitHit(preview: false);
				}
			}
			else
			{
				SubmitHit(preview: false);
			}
		}
	}
}

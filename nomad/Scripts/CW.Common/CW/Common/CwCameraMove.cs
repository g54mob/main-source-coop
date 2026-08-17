using System;
using UnityEngine;

namespace CW.Common
{
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwCameraMove")]
	[AddComponentMenu("Common/CW Camera Move")]
	public class CwCameraMove : MonoBehaviour
	{
		[SerializeField]
		private bool listen = true;

		[SerializeField]
		private float damping = 10f;

		[SerializeField]
		private float sensitivity = 1f;

		[SerializeField]
		[Range(0f, 0.5f)]
		private float speedWheel = 0.1f;

		[SerializeField]
		private CwInputManager.Axis horizontalControls = new CwInputManager.Axis(2, fInvert: false, CwInputManager.AxisGesture.HorizontalDrag, 1f, KeyCode.A, KeyCode.D, KeyCode.LeftArrow, KeyCode.RightArrow, 100f);

		[SerializeField]
		private CwInputManager.Axis depthControls = new CwInputManager.Axis(2, fInvert: false, CwInputManager.AxisGesture.HorizontalDrag, 1f, KeyCode.S, KeyCode.W, KeyCode.DownArrow, KeyCode.UpArrow, 100f);

		[SerializeField]
		private CwInputManager.Axis verticalControls = new CwInputManager.Axis(3, fInvert: false, CwInputManager.AxisGesture.HorizontalDrag, 1f, KeyCode.F, KeyCode.R, KeyCode.None, KeyCode.None, 100f);

		[NonSerialized]
		private Vector3 remainingDelta;

		public bool Listen
		{
			get
			{
				return listen;
			}
			set
			{
				listen = value;
			}
		}

		public float Damping
		{
			get
			{
				return damping;
			}
			set
			{
				damping = value;
			}
		}

		public float Sensitivity
		{
			get
			{
				return sensitivity;
			}
			set
			{
				sensitivity = value;
			}
		}

		public float SpeedWheel
		{
			get
			{
				return speedWheel;
			}
			set
			{
				speedWheel = value;
			}
		}

		public CwInputManager.Axis HorizontalControls
		{
			get
			{
				return horizontalControls;
			}
			set
			{
				horizontalControls = value;
			}
		}

		public CwInputManager.Axis DepthControls
		{
			get
			{
				return depthControls;
			}
			set
			{
				depthControls = value;
			}
		}

		public CwInputManager.Axis VerticalControls
		{
			get
			{
				return verticalControls;
			}
			set
			{
				verticalControls = value;
			}
		}

		protected virtual void Start()
		{
			CwInputManager.EnsureThisComponentExists();
		}

		protected virtual void Update()
		{
			if (CwInput.GetMouseExists())
			{
				sensitivity *= 1f + Mathf.Clamp(CwInput.GetMouseWheelDelta(), -1f, 1f) * speedWheel;
			}
			if (listen)
			{
				AddToDelta();
			}
			DampenDelta();
		}

		private void AddToDelta()
		{
			Vector3 vector = new Vector3
			{
				x = horizontalControls.GetValue(Time.deltaTime),
				y = verticalControls.GetValue(Time.deltaTime),
				z = depthControls.GetValue(Time.deltaTime)
			};
			Vector3 position = base.transform.position;
			base.transform.Translate(vector * sensitivity, Space.Self);
			Vector3 vector2 = base.transform.position - position;
			remainingDelta += vector2;
			base.transform.position = position;
		}

		private void DampenDelta()
		{
			float t = CwHelper.DampenFactor(damping, Time.deltaTime);
			Vector3 vector = Vector3.Lerp(remainingDelta, Vector3.zero, t);
			base.transform.position += remainingDelta - vector;
			remainingDelta = vector;
		}
	}
}

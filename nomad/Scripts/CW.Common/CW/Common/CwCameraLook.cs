using System;
using UnityEngine;

namespace CW.Common
{
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwCameraLook")]
	[AddComponentMenu("Common/CW Camera Look")]
	public class CwCameraLook : MonoBehaviour
	{
		[SerializeField]
		private bool listen = true;

		[SerializeField]
		private float damping = 10f;

		[SerializeField]
		private float sensitivity = 1f;

		[SerializeField]
		private CwInputManager.Axis pitchControls = new CwInputManager.Axis(1, fInvert: true, CwInputManager.AxisGesture.VerticalDrag, -0.1f, KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None, 45f);

		[SerializeField]
		private CwInputManager.Axis yawControls = new CwInputManager.Axis(1, fInvert: true, CwInputManager.AxisGesture.HorizontalDrag, 0.1f, KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None, 45f);

		[SerializeField]
		private CwInputManager.Axis rollControls = new CwInputManager.Axis(2, fInvert: true, CwInputManager.AxisGesture.Twist, 1f, KeyCode.E, KeyCode.Q, KeyCode.None, KeyCode.None, 45f);

		[NonSerialized]
		private Quaternion remainingDelta = Quaternion.identity;

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

		public CwInputManager.Axis PitchControls
		{
			get
			{
				return pitchControls;
			}
			set
			{
				pitchControls = value;
			}
		}

		public CwInputManager.Axis YawControls
		{
			get
			{
				return yawControls;
			}
			set
			{
				yawControls = value;
			}
		}

		public CwInputManager.Axis RollControls
		{
			get
			{
				return rollControls;
			}
			set
			{
				rollControls = value;
			}
		}

		protected virtual void Start()
		{
			CwInputManager.EnsureThisComponentExists();
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void Update()
		{
			if (listen)
			{
				AddToDelta();
			}
			DampenDelta();
		}

		protected virtual void OnApplicationFocus(bool focus)
		{
		}

		private void AddToDelta()
		{
			Vector3 vector = new Vector3
			{
				x = pitchControls.GetValue(Time.deltaTime),
				y = yawControls.GetValue(Time.deltaTime),
				z = rollControls.GetValue(Time.deltaTime)
			};
			vector *= sensitivity;
			Quaternion localRotation = base.transform.localRotation;
			base.transform.Rotate(vector.x, vector.y, 0f, Space.Self);
			base.transform.Rotate(0f, 0f, vector.z, Space.Self);
			remainingDelta *= Quaternion.Inverse(localRotation) * base.transform.localRotation;
			base.transform.localRotation = localRotation;
		}

		private void DampenDelta()
		{
			float t = CwHelper.DampenFactor(damping, Time.deltaTime);
			Quaternion rotation = Quaternion.Slerp(remainingDelta, Quaternion.identity, t);
			base.transform.localRotation = base.transform.localRotation * Quaternion.Inverse(rotation) * remainingDelta;
			remainingDelta = rotation;
		}
	}
}

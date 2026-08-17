using System;
using UnityEngine;

namespace CW.Common
{
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwCameraPivot")]
	[AddComponentMenu("Common/CW Camera Pivot")]
	public class CwCameraPivot : MonoBehaviour
	{
		[SerializeField]
		private bool listen = true;

		[SerializeField]
		private float damping = 10f;

		[SerializeField]
		private CwInputManager.Axis pitchControls = new CwInputManager.Axis(1, fInvert: true, CwInputManager.AxisGesture.VerticalDrag, -0.1f, KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None, 45f);

		[SerializeField]
		private CwInputManager.Axis yawControls = new CwInputManager.Axis(1, fInvert: true, CwInputManager.AxisGesture.HorizontalDrag, 0.1f, KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None, 45f);

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

		protected virtual void OnEnable()
		{
			CwInputManager.EnsureThisComponentExists();
		}

		protected virtual void Update()
		{
			if (listen)
			{
				AddToDelta();
			}
			DampenDelta();
		}

		private void AddToDelta()
		{
			remainingDelta.x += pitchControls.GetValue(Time.deltaTime);
			remainingDelta.y += yawControls.GetValue(Time.deltaTime);
		}

		private void DampenDelta()
		{
			float t = CwHelper.DampenFactor(damping, Time.deltaTime);
			Vector3 vector = Vector3.Lerp(remainingDelta, Vector3.zero, t);
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			localEulerAngles.x = 0f - Mathf.DeltaAngle(localEulerAngles.x, 0f);
			localEulerAngles += remainingDelta - vector;
			localEulerAngles.x = Mathf.Clamp(localEulerAngles.x, -89f, 89f);
			base.transform.localEulerAngles = localEulerAngles;
			remainingDelta = vector;
		}
	}
}

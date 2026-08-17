using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintIn3D
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwDragPitchYaw")]
	[AddComponentMenu("CW/Paint in 3D/CW Drag Pitch Yaw")]
	public class CwDragPitchYaw : MonoBehaviour
	{
		[SerializeField]
		private Transform tools;

		[SerializeField]
		private KeyCode key = KeyCode.Mouse1;

		[SerializeField]
		private LayerMask guiLayers = 32;

		[SerializeField]
		private float pitch;

		[SerializeField]
		private float pitchSensitivity = 0.1f;

		[SerializeField]
		private float pitchMin = -90f;

		[SerializeField]
		private float pitchMax = 90f;

		[SerializeField]
		private float yaw;

		[SerializeField]
		private float yawSensitivity = 0.1f;

		[SerializeField]
		private float dampening = 10f;

		[SerializeField]
		private float currentPitch;

		[SerializeField]
		private float currentYaw;

		[NonSerialized]
		private List<CwInputManager.Finger> fingers = new List<CwInputManager.Finger>();

		public Transform Tools
		{
			get
			{
				return tools;
			}
			set
			{
				tools = value;
			}
		}

		public KeyCode Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		public LayerMask GuiLayers
		{
			get
			{
				return guiLayers;
			}
			set
			{
				guiLayers = value;
			}
		}

		public float Pitch
		{
			get
			{
				return pitch;
			}
			set
			{
				pitch = value;
			}
		}

		public float PitchSensitivity
		{
			get
			{
				return pitchSensitivity;
			}
			set
			{
				pitchSensitivity = value;
			}
		}

		public float PitchMin
		{
			get
			{
				return pitchMin;
			}
			set
			{
				pitchMin = value;
			}
		}

		public float PitchMax
		{
			get
			{
				return pitchMax;
			}
			set
			{
				pitchMax = value;
			}
		}

		public float Yaw
		{
			get
			{
				return yaw;
			}
			set
			{
				yaw = value;
			}
		}

		public float YawSensitivity
		{
			get
			{
				return yawSensitivity;
			}
			set
			{
				yawSensitivity = value;
			}
		}

		public float Dampening
		{
			get
			{
				return dampening;
			}
			set
			{
				dampening = value;
			}
		}

		private bool CanRotate
		{
			get
			{
				if (CwInput.GetKeyIsHeld(key))
				{
					return true;
				}
				if (tools != null)
				{
					for (int i = 0; i < tools.childCount; i++)
					{
						if (tools.GetChild(i).gameObject.activeSelf)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		protected virtual void OnEnable()
		{
			CwInputManager.EnsureThisComponentExists();
			CwInputManager.OnFingerDown += HandleFingerDown;
			CwInputManager.OnFingerUp += HandleFingerUp;
		}

		protected virtual void OnDisable()
		{
			CwInputManager.OnFingerDown -= HandleFingerDown;
			CwInputManager.OnFingerUp -= HandleFingerUp;
		}

		private void HandleFingerDown(CwInputManager.Finger finger)
		{
			if (finger.Index != -1337 && !CwInputManager.PointOverGui(finger.ScreenPosition, guiLayers) && (key == KeyCode.None || CwInput.GetKeyIsHeld(key)))
			{
				fingers.Add(finger);
			}
		}

		private void HandleFingerUp(CwInputManager.Finger finger)
		{
			fingers.Remove(finger);
		}

		protected virtual void Update()
		{
			if (CanRotate && Application.isPlaying)
			{
				Vector2 averageDeltaScaled = CwInputManager.GetAverageDeltaScaled(fingers);
				pitch -= averageDeltaScaled.y * pitchSensitivity;
				yaw += averageDeltaScaled.x * yawSensitivity;
			}
			pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
			float t = CwHelper.DampenFactor(dampening, Time.deltaTime);
			currentPitch = Mathf.Lerp(currentPitch, pitch, t);
			currentYaw = Mathf.Lerp(currentYaw, yaw, t);
			base.transform.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
		}
	}
}

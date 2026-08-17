using System;
using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwTapThrow")]
	[AddComponentMenu("CW/Paint in 3D/CW Tap Throw")]
	public class CwTapThrow : MonoBehaviour
	{
		[SerializeField]
		private KeyCode key = KeyCode.Mouse0;

		[SerializeField]
		private LayerMask guiLayers = 32;

		[SerializeField]
		private GameObject prefab;

		[SerializeField]
		private float speed = 10f;

		[SerializeField]
		protected bool storeStates;

		[NonSerialized]
		private List<CwInputManager.Finger> fingers = new List<CwInputManager.Finger>();

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

		public GameObject Prefab
		{
			get
			{
				return prefab;
			}
			set
			{
				prefab = value;
			}
		}

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		public bool StoreStates
		{
			get
			{
				return storeStates;
			}
			set
			{
				storeStates = value;
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
			if (fingers.Remove(finger) && finger.Age < 0.5f)
			{
				DoThrow(finger.ScreenPosition);
			}
		}

		private void DoThrow(Vector2 screenPosition)
		{
			if (!(prefab != null))
			{
				return;
			}
			Camera camera = CwHelper.GetCamera(null);
			if (camera != null)
			{
				if (storeStates)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
				Ray ray = camera.ScreenPointToRay(screenPosition);
				Quaternion rotation = Quaternion.LookRotation(ray.direction);
				GameObject gameObject = UnityEngine.Object.Instantiate(prefab, ray.origin, rotation);
				gameObject.SetActive(value: true);
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.linearVelocity = gameObject.transform.forward * Speed;
				}
			}
		}
	}
}

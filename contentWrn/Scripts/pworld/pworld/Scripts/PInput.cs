using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class PInput : PSingleton<PInput>
	{
		public Vector3 daeqws;

		public Vector3 daeqws_down;

		public Vector2 mouseD;

		public float mouseScrollDelta;

		public KEY_STATE mouseLeft;

		public KEY_STATE mouseRight;

		public KEY_STATE space;

		public KEY_STATE tab;

		public float mouseLR;

		public Vector3 daeqws_time;

		public Vector2 mouseFrustumDelta;

		public Vector2 rawMouseD;

		public Vector3 lastMousePosInWorld;

		private Camera mainCam;

		private Stack<CursorLockMode> lockStack = new Stack<CursorLockMode>();

		public CursorLockMode CursorLockMode
		{
			get
			{
				return lockStack.Peek();
			}
			set
			{
				lockStack.Push(value);
				Cursor.lockState = lockStack.Peek();
				Cursor.visible = Cursor.lockState == CursorLockMode.None;
			}
		}

		private Camera Cam
		{
			get
			{
				if (mainCam == null)
				{
					mainCam = Camera.main;
				}
				return mainCam;
			}
			set
			{
				mainCam = value;
			}
		}

		public static Vector2 MouseUV => Input.mousePosition / new Vector2(Screen.width, Screen.height);

		public override void Awake()
		{
			base.Awake();
			lockStack.Push(Cursor.lockState);
		}

		public void GoToPreviousLockMode()
		{
			if (PSingleton<PInput>.Me.lockStack.Count > 1)
			{
				PSingleton<PInput>.Me.lockStack.Pop();
				Cursor.lockState = lockStack.Peek();
			}
		}

		public static Vector3 GetMousePosInWorld(Camera cam)
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = cam.nearClipPlane;
			return cam.ScreenToWorldPoint(mousePosition);
		}

		public static Vector3 GetMouseDeltaInWorld(Camera cam)
		{
			return GetMousePosInWorld(cam) - PSingleton<PInput>.Me.lastMousePosInWorld;
		}

		private void Start()
		{
		}

		private void Update()
		{
			ReadMouseMove();
			daeqws.x = Input.GetKey(KeyCode.D).ToInt() - Input.GetKey(KeyCode.A).ToInt();
			daeqws.z = Input.GetKey(KeyCode.W).ToInt() - Input.GetKey(KeyCode.S).ToInt();
			daeqws.y = Input.GetKey(KeyCode.E).ToInt() - Input.GetKey(KeyCode.Q).ToInt();
			daeqws = Vector3.ClampMagnitude(daeqws, 1f);
			daeqws_down.x = Input.GetKeyDown(KeyCode.D).ToInt() - Input.GetKeyDown(KeyCode.A).ToInt();
			daeqws_down.z = Input.GetKeyDown(KeyCode.W).ToInt() - Input.GetKeyDown(KeyCode.S).ToInt();
			daeqws_down.y = Input.GetKeyDown(KeyCode.E).ToInt() - Input.GetKeyDown(KeyCode.Q).ToInt();
			daeqws_down = Vector3.ClampMagnitude(daeqws_down, 1f);
			if (daeqws.x != 0f)
			{
				daeqws_time.x = Time.realtimeSinceStartup;
			}
			if (daeqws.y != 0f)
			{
				daeqws_time.y = Time.realtimeSinceStartup;
			}
			if (daeqws.z != 0f)
			{
				daeqws_time.z = Time.realtimeSinceStartup;
			}
			mouseScrollDelta = Input.mouseScrollDelta.y;
			Input input = new Input();
			space = input.GetKeyState(KeyCode.Space);
			tab = input.GetKeyState(KeyCode.Tab);
			mouseLeft = input.GetKeyState(KeyCode.Mouse0);
			mouseRight = input.GetKeyState(KeyCode.Mouse1);
			mouseLR = Input.GetKey(KeyCode.Mouse0).ToInt() - Input.GetKey(KeyCode.Mouse1).ToInt();
		}

		private void ReadMouseMove()
		{
			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q))
			{
				if (Cursor.lockState == CursorLockMode.Locked)
				{
					Cursor.lockState = CursorLockMode.None;
				}
				else
				{
					Cursor.lockState = CursorLockMode.Locked;
				}
			}
			if (Cursor.lockState == CursorLockMode.None)
			{
				mouseD = Vector2.zero;
			}
			if (Cursor.lockState == CursorLockMode.Locked)
			{
				mouseD.x = Input.GetAxis("Mouse X");
				mouseD.y = Input.GetAxis("Mouse Y");
			}
			rawMouseD.x = Input.GetAxis("Mouse X");
			rawMouseD.y = Input.GetAxis("Mouse Y");
			lastMousePosInWorld = GetMousePosInWorld(Cam);
		}

		public static IEnumerator HoldClick(KeyCode keyCode, float holdTime, Action callBack)
		{
			float startTime = Time.time + holdTime;
			while (startTime > Time.time)
			{
				if (!Input.GetKey(keyCode))
				{
					yield break;
				}
				yield return new WaitForSeconds(0f);
			}
			callBack?.Invoke();
			yield return null;
		}

		public static IEnumerator RepeatingClick(KeyCode keyCode, float startTime, Action callBack, float timeBetweenRepeats)
		{
			float timeElapsed = 0f;
			while (Input.GetKey(keyCode))
			{
				if (timeElapsed >= startTime)
				{
					callBack?.Invoke();
					timeElapsed -= timeBetweenRepeats;
				}
				yield return null;
				timeElapsed += Time.deltaTime;
			}
		}
	}
}

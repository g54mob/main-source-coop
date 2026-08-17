using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Utilities;

namespace PrimeTweenDemo
{
	public class InputController : MonoBehaviour
	{
		private static bool isNewInputSystemEnabled => true;

		private static bool isLegacyInputManagerEnabled => true;

		public static bool touchSupported
		{
			get
			{
				if (isNewInputSystemEnabled)
				{
					return Touchscreen.current != null;
				}
				return Input.touchSupported;
			}
		}

		public static Vector2 screenPosition
		{
			get
			{
				if (isNewInputSystemEnabled)
				{
					if (Mouse.current != null)
					{
						return Mouse.current.position.ReadValue();
					}
					ReadOnlyArray<UnityEngine.InputSystem.EnhancedTouch.Touch> activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
					if (activeTouches.Count <= 0)
					{
						return Vector2.zero;
					}
					return activeTouches[0].screenPosition;
				}
				return Input.mousePosition;
			}
		}

		private void Awake()
		{
			if (isNewInputSystemEnabled && !isLegacyInputManagerEnabled)
			{
				base.gameObject.SetActive(value: false);
				base.gameObject.AddComponent<InputSystemUIInputModule>().pointerBehavior = UIPointerBehavior.AllPointersAsIs;
				EnhancedTouchSupport.Enable();
				base.gameObject.SetActive(value: true);
			}
			else
			{
				base.gameObject.AddComponent<StandaloneInputModule>();
			}
		}

		public static bool GetDown()
		{
			if (Mouse.current != null)
			{
				return Mouse.current.leftButton.wasPressedThisFrame;
			}
			if (isNewInputSystemEnabled)
			{
				if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 0)
				{
					return UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began;
				}
				return false;
			}
			return Input.GetMouseButtonDown(0);
		}

		public static bool Get()
		{
			if (isNewInputSystemEnabled)
			{
				if (Mouse.current != null)
				{
					return Mouse.current.leftButton.isPressed;
				}
				if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 0)
				{
					return false;
				}
				UnityEngine.InputSystem.TouchPhase phase = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0].phase;
				if (phase != UnityEngine.InputSystem.TouchPhase.Stationary)
				{
					return phase == UnityEngine.InputSystem.TouchPhase.Moved;
				}
				return true;
			}
			return Input.GetMouseButtonDown(0);
		}

		public static bool GetUp()
		{
			if (isNewInputSystemEnabled)
			{
				if (Mouse.current != null)
				{
					return Mouse.current.leftButton.wasReleasedThisFrame;
				}
				if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 0)
				{
					return UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Ended;
				}
				return false;
			}
			return Input.GetMouseButtonUp(0);
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeStage.AdvancedFPSCounter
{
	public static class AFPSInputProxy
	{
		private static Key cachedHotKey;

		private static KeyCode lastHotKeyLegacy;

		public static Vector2 mousePosition
		{
			get
			{
				if (Mouse.current == null)
				{
					return Vector2.zero;
				}
				return Mouse.current.position.value;
			}
		}

		public static bool GetHotKeyDown(KeyCode key)
		{
			if (key == KeyCode.None)
			{
				return false;
			}
			if (Keyboard.current == null)
			{
				return false;
			}
			if (lastHotKeyLegacy != key)
			{
				cachedHotKey = ConvertLegacyKeyCode(key);
				lastHotKeyLegacy = key;
			}
			return Keyboard.current[cachedHotKey].wasPressedThisFrame;
		}

		public static bool GetControlKey()
		{
			if (Keyboard.current != null)
			{
				if (!Keyboard.current.leftCtrlKey.isPressed && !Keyboard.current.rightCtrlKey.isPressed && !Keyboard.current.leftCommandKey.isPressed)
				{
					return Keyboard.current.rightCommandKey.isPressed;
				}
				return true;
			}
			return false;
		}

		public static bool GetAltKey()
		{
			if (Keyboard.current != null)
			{
				if (!Keyboard.current.leftAltKey.isPressed)
				{
					return Keyboard.current.rightAltKey.isPressed;
				}
				return true;
			}
			return false;
		}

		public static bool GetShiftKey()
		{
			if (Keyboard.current != null)
			{
				if (!Keyboard.current.leftShiftKey.isPressed)
				{
					return Keyboard.current.rightShiftKey.isPressed;
				}
				return true;
			}
			return false;
		}

		private static Key ConvertLegacyKeyCode(KeyCode keyCode)
		{
			if (!Enum.TryParse<Key>(keyCode.ToString(), ignoreCase: true, out var result))
			{
				Debug.LogError("Couldn't convert legacy input KeyCode " + keyCode.ToString() + " to the new Input System format!\nPlease report to https://codestage.net/contacts/");
			}
			return result;
		}

		public static bool GetMouseButton(int i)
		{
			if (Mouse.current != null)
			{
				if ((i != 0 || !Mouse.current.leftButton.isPressed) && (i != 1 || !Mouse.current.rightButton.isPressed))
				{
					if (i == 2)
					{
						return Mouse.current.middleButton.isPressed;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool GetMouseButtonUp(int i)
		{
			if (Mouse.current != null)
			{
				if ((i != 0 || !Mouse.current.leftButton.wasReleasedThisFrame) && (i != 1 || !Mouse.current.rightButton.wasReleasedThisFrame))
				{
					if (i == 2)
					{
						return Mouse.current.middleButton.wasReleasedThisFrame;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}
}

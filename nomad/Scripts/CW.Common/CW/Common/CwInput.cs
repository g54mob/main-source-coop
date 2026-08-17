using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

namespace CW.Common
{
	public static class CwInput
	{
		private static Dictionary<KeyCode, Key> keyMapping = new Dictionary<KeyCode, Key>
		{
			{
				KeyCode.Backspace,
				Key.Backspace
			},
			{
				KeyCode.Tab,
				Key.Tab
			},
			{
				KeyCode.Clear,
				Key.None
			},
			{
				KeyCode.Return,
				Key.Enter
			},
			{
				KeyCode.Pause,
				Key.Pause
			},
			{
				KeyCode.Escape,
				Key.Escape
			},
			{
				KeyCode.Space,
				Key.Space
			},
			{
				KeyCode.Exclaim,
				Key.None
			},
			{
				KeyCode.DoubleQuote,
				Key.None
			},
			{
				KeyCode.Hash,
				Key.None
			},
			{
				KeyCode.Dollar,
				Key.None
			},
			{
				KeyCode.Percent,
				Key.None
			},
			{
				KeyCode.Ampersand,
				Key.None
			},
			{
				KeyCode.Quote,
				Key.Quote
			},
			{
				KeyCode.LeftParen,
				Key.None
			},
			{
				KeyCode.RightParen,
				Key.None
			},
			{
				KeyCode.Asterisk,
				Key.None
			},
			{
				KeyCode.Plus,
				Key.None
			},
			{
				KeyCode.Comma,
				Key.Comma
			},
			{
				KeyCode.Minus,
				Key.Minus
			},
			{
				KeyCode.Period,
				Key.Period
			},
			{
				KeyCode.Slash,
				Key.Slash
			},
			{
				KeyCode.Alpha1,
				Key.Digit1
			},
			{
				KeyCode.Alpha2,
				Key.Digit2
			},
			{
				KeyCode.Alpha3,
				Key.Digit3
			},
			{
				KeyCode.Alpha4,
				Key.Digit4
			},
			{
				KeyCode.Alpha5,
				Key.Digit5
			},
			{
				KeyCode.Alpha6,
				Key.Digit6
			},
			{
				KeyCode.Alpha7,
				Key.Digit7
			},
			{
				KeyCode.Alpha8,
				Key.Digit8
			},
			{
				KeyCode.Alpha9,
				Key.Digit9
			},
			{
				KeyCode.Alpha0,
				Key.Digit0
			},
			{
				KeyCode.Colon,
				Key.None
			},
			{
				KeyCode.Semicolon,
				Key.Semicolon
			},
			{
				KeyCode.Less,
				Key.None
			},
			{
				KeyCode.Equals,
				Key.Equals
			},
			{
				KeyCode.Greater,
				Key.None
			},
			{
				KeyCode.Question,
				Key.None
			},
			{
				KeyCode.At,
				Key.None
			},
			{
				KeyCode.LeftBracket,
				Key.LeftBracket
			},
			{
				KeyCode.Backslash,
				Key.Backslash
			},
			{
				KeyCode.RightBracket,
				Key.RightBracket
			},
			{
				KeyCode.Caret,
				Key.None
			},
			{
				KeyCode.Underscore,
				Key.None
			},
			{
				KeyCode.BackQuote,
				Key.Backquote
			},
			{
				KeyCode.A,
				Key.A
			},
			{
				KeyCode.B,
				Key.B
			},
			{
				KeyCode.C,
				Key.C
			},
			{
				KeyCode.D,
				Key.D
			},
			{
				KeyCode.E,
				Key.E
			},
			{
				KeyCode.F,
				Key.F
			},
			{
				KeyCode.G,
				Key.G
			},
			{
				KeyCode.H,
				Key.H
			},
			{
				KeyCode.I,
				Key.I
			},
			{
				KeyCode.J,
				Key.J
			},
			{
				KeyCode.K,
				Key.K
			},
			{
				KeyCode.L,
				Key.L
			},
			{
				KeyCode.M,
				Key.M
			},
			{
				KeyCode.N,
				Key.N
			},
			{
				KeyCode.O,
				Key.O
			},
			{
				KeyCode.P,
				Key.P
			},
			{
				KeyCode.Q,
				Key.Q
			},
			{
				KeyCode.R,
				Key.R
			},
			{
				KeyCode.S,
				Key.S
			},
			{
				KeyCode.T,
				Key.T
			},
			{
				KeyCode.U,
				Key.U
			},
			{
				KeyCode.V,
				Key.V
			},
			{
				KeyCode.W,
				Key.W
			},
			{
				KeyCode.X,
				Key.X
			},
			{
				KeyCode.Y,
				Key.Y
			},
			{
				KeyCode.Z,
				Key.Z
			},
			{
				KeyCode.LeftCurlyBracket,
				Key.None
			},
			{
				KeyCode.Pipe,
				Key.None
			},
			{
				KeyCode.RightCurlyBracket,
				Key.None
			},
			{
				KeyCode.Tilde,
				Key.None
			},
			{
				KeyCode.Delete,
				Key.Delete
			},
			{
				KeyCode.Keypad0,
				Key.Numpad0
			},
			{
				KeyCode.Keypad1,
				Key.Numpad1
			},
			{
				KeyCode.Keypad2,
				Key.Numpad2
			},
			{
				KeyCode.Keypad3,
				Key.Numpad3
			},
			{
				KeyCode.Keypad4,
				Key.Numpad4
			},
			{
				KeyCode.Keypad5,
				Key.Numpad5
			},
			{
				KeyCode.Keypad6,
				Key.Numpad6
			},
			{
				KeyCode.Keypad7,
				Key.Numpad7
			},
			{
				KeyCode.Keypad8,
				Key.Numpad8
			},
			{
				KeyCode.Keypad9,
				Key.Numpad9
			},
			{
				KeyCode.KeypadPeriod,
				Key.NumpadPeriod
			},
			{
				KeyCode.KeypadDivide,
				Key.NumpadDivide
			},
			{
				KeyCode.KeypadMultiply,
				Key.NumpadMultiply
			},
			{
				KeyCode.KeypadMinus,
				Key.NumpadMinus
			},
			{
				KeyCode.KeypadPlus,
				Key.NumpadPlus
			},
			{
				KeyCode.KeypadEnter,
				Key.NumpadEnter
			},
			{
				KeyCode.KeypadEquals,
				Key.NumpadEquals
			},
			{
				KeyCode.UpArrow,
				Key.UpArrow
			},
			{
				KeyCode.DownArrow,
				Key.DownArrow
			},
			{
				KeyCode.RightArrow,
				Key.RightArrow
			},
			{
				KeyCode.LeftArrow,
				Key.LeftArrow
			},
			{
				KeyCode.Insert,
				Key.Insert
			},
			{
				KeyCode.Home,
				Key.Home
			},
			{
				KeyCode.End,
				Key.End
			},
			{
				KeyCode.PageUp,
				Key.PageUp
			},
			{
				KeyCode.PageDown,
				Key.PageDown
			},
			{
				KeyCode.F1,
				Key.F1
			},
			{
				KeyCode.F2,
				Key.F2
			},
			{
				KeyCode.F3,
				Key.F3
			},
			{
				KeyCode.F4,
				Key.F4
			},
			{
				KeyCode.F5,
				Key.F5
			},
			{
				KeyCode.F6,
				Key.F6
			},
			{
				KeyCode.F7,
				Key.F7
			},
			{
				KeyCode.F8,
				Key.F8
			},
			{
				KeyCode.F9,
				Key.F9
			},
			{
				KeyCode.F10,
				Key.F10
			},
			{
				KeyCode.F11,
				Key.F11
			},
			{
				KeyCode.F12,
				Key.F12
			},
			{
				KeyCode.F13,
				Key.None
			},
			{
				KeyCode.F14,
				Key.None
			},
			{
				KeyCode.F15,
				Key.None
			},
			{
				KeyCode.Numlock,
				Key.NumLock
			},
			{
				KeyCode.CapsLock,
				Key.CapsLock
			},
			{
				KeyCode.ScrollLock,
				Key.ScrollLock
			},
			{
				KeyCode.RightShift,
				Key.RightShift
			},
			{
				KeyCode.LeftShift,
				Key.LeftShift
			},
			{
				KeyCode.RightControl,
				Key.RightCtrl
			},
			{
				KeyCode.LeftControl,
				Key.LeftCtrl
			},
			{
				KeyCode.RightAlt,
				Key.RightAlt
			},
			{
				KeyCode.LeftAlt,
				Key.LeftAlt
			},
			{
				KeyCode.RightMeta,
				Key.RightMeta
			},
			{
				KeyCode.LeftMeta,
				Key.LeftMeta
			},
			{
				KeyCode.LeftWindows,
				Key.LeftMeta
			},
			{
				KeyCode.RightWindows,
				Key.RightMeta
			},
			{
				KeyCode.AltGr,
				Key.RightAlt
			},
			{
				KeyCode.Help,
				Key.None
			},
			{
				KeyCode.Print,
				Key.PrintScreen
			},
			{
				KeyCode.SysReq,
				Key.None
			},
			{
				KeyCode.Break,
				Key.None
			},
			{
				KeyCode.Menu,
				Key.ContextMenu
			}
		};

		[RuntimeInitializeOnLoadMethod]
		private static void Enable()
		{
			EnhancedTouchSupport.Enable();
		}

		private static ButtonControl GetMouseButtonControl(int index)
		{
			if (Mouse.current != null)
			{
				switch (index)
				{
				case 0:
					return Mouse.current.leftButton;
				case 1:
					return Mouse.current.rightButton;
				case 2:
					return Mouse.current.middleButton;
				case 3:
					return Mouse.current.forwardButton;
				case 4:
					return Mouse.current.backButton;
				}
			}
			return null;
		}

		private static ButtonControl GetButtonControl(KeyCode oldKey)
		{
			if (Mouse.current != null)
			{
				switch (oldKey)
				{
				case KeyCode.Mouse0:
					return Mouse.current.leftButton;
				case KeyCode.Mouse1:
					return Mouse.current.rightButton;
				case KeyCode.Mouse2:
					return Mouse.current.middleButton;
				case KeyCode.Mouse3:
					return Mouse.current.forwardButton;
				case KeyCode.Mouse4:
					return Mouse.current.backButton;
				}
			}
			if (Keyboard.current != null)
			{
				Key value = Key.None;
				if (keyMapping.TryGetValue(oldKey, out value))
				{
					return Keyboard.current[value];
				}
			}
			return null;
		}

		public static int GetTouchCount()
		{
			if (!EnhancedTouchSupport.enabled)
			{
				EnhancedTouchSupport.Enable();
			}
			return Touch.activeTouches.Count;
		}

		public static void GetTouch(int index, out int id, out Vector2 position, out float pressure, out bool set)
		{
			Touch touch = Touch.activeTouches[index];
			id = touch.finger.index;
			position = touch.screenPosition;
			pressure = touch.pressure;
			set = touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
		}

		public static Vector2 GetMousePosition()
		{
			if (Mouse.current == null)
			{
				return default(Vector2);
			}
			return Mouse.current.position.ReadValue();
		}

		public static bool GetKeyWentDown(KeyCode oldKey)
		{
			return GetButtonControl(oldKey)?.wasPressedThisFrame ?? false;
		}

		public static bool GetKeyIsHeld(KeyCode oldKey)
		{
			return GetButtonControl(oldKey)?.isPressed ?? false;
		}

		public static bool GetKeyWentUp(KeyCode oldKey)
		{
			return GetButtonControl(oldKey)?.wasReleasedThisFrame ?? false;
		}

		public static bool GetMouseWentDown(int index)
		{
			return GetMouseButtonControl(index)?.wasPressedThisFrame ?? false;
		}

		public static bool GetMouseIsHeld(int index)
		{
			return GetMouseButtonControl(index)?.isPressed ?? false;
		}

		public static bool GetMouseWentUp(int index)
		{
			return GetMouseButtonControl(index)?.wasReleasedThisFrame ?? false;
		}

		public static float GetMouseWheelDelta()
		{
			if (Mouse.current.scroll == null)
			{
				return 0f;
			}
			return Mouse.current.scroll.ReadValue().y;
		}

		public static bool GetMouseExists()
		{
			return Mouse.current != null;
		}

		public static bool GetKeyboardExists()
		{
			return Keyboard.current != null;
		}
	}
}

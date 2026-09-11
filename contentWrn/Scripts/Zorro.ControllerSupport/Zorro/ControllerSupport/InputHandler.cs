using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;
using Zorro.Core;

namespace Zorro.ControllerSupport
{
	public class InputHandler : RetrievableResourceSingleton<InputHandler>
	{
		private PlayerInput m_playerInput;

		private Func<bool> m_isSteamDeck;

		private static InputScheme m_inputScheme;

		private char m_inputChar;

		private Optionable<int> m_inputBlockCount = Optionable<int>.None;

		public Action<InputScheme> InputSchemeChanged;

		private GamepadType m_gamepadType;

		public static GamepadType GetGamepadType()
		{
			return RetrievableResourceSingleton<InputHandler>.Instance.m_gamepadType;
		}

		protected override void OnCreated()
		{
			base.OnCreated();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			m_playerInput = GetComponent<PlayerInput>();
			m_inputScheme = ToInputScheme(m_playerInput.currentControlScheme);
			Debug.Log("Initialized InputHandler");
		}

		public void Initialize(Func<bool> isSteamDeck)
		{
			m_isSteamDeck = isSteamDeck;
			RetrievableSingleton<NavigationInfoHandler>.Instance.RegisterPage();
		}

		public static InputScheme GetCurrentUsedInputScheme()
		{
			return m_inputScheme;
		}

		public static bool GetKeyDown<T>(T key) where T : unmanaged, Enum
		{
			return RetrievableResourceSingleton<InputHandler>.Instance.m_playerInput.actions[key.ToString()].WasPressedThisFrame();
		}

		public static bool HasDevice<T>() where T : InputDevice
		{
			foreach (InputDevice device in InputSystem.devices)
			{
				if (device as T != null)
				{
					return true;
				}
			}
			return false;
		}

		private static GamepadType FindGamepadType(ReadOnlyArray<InputDevice> devices)
		{
			foreach (InputDevice item in devices)
			{
				GamepadType gamepadType = FindGamepadType(item);
				if (gamepadType != GamepadType.Unknown)
				{
					return gamepadType;
				}
			}
			return GamepadType.Unknown;
		}

		public static GamepadType FindGamepadType(InputDevice device)
		{
			if (RetrievableResourceSingleton<InputHandler>.Instance.m_isSteamDeck != null && RetrievableResourceSingleton<InputHandler>.Instance.m_isSteamDeck())
			{
				return GamepadType.SteamDeck;
			}
			if (device is XInputController)
			{
				return GamepadType.Xbox;
			}
			if (device is DualSenseGamepadHID)
			{
				return GamepadType.Dualsense;
			}
			if (device is DualShockGamepad)
			{
				return GamepadType.Dualshock;
			}
			return GamepadType.Unknown;
		}

		private void Update()
		{
			InputScheme inputScheme = ToInputScheme(m_playerInput.currentControlScheme);
			GamepadType gamepadType = FindGamepadType(m_playerInput.devices);
			if (m_inputScheme != inputScheme || m_gamepadType != gamepadType)
			{
				m_inputScheme = inputScheme;
				m_gamepadType = gamepadType;
				Debug.Log(string.Format("Control scheme changed to {0} {1}", m_inputScheme, (m_inputScheme == InputScheme.Gamepad) ? $"(gamepad type: {m_gamepadType})" : ""));
				InputSchemeChanged?.Invoke(m_inputScheme);
			}
		}

		public static void AddInputBlock()
		{
			RetrievableResourceSingleton<InputHandler>.Instance.m_inputBlockCount = Optionable<int>.Some(Time.frameCount);
		}

		public static bool HasInputBlock()
		{
			if (RetrievableResourceSingleton<InputHandler>.Instance.m_inputBlockCount.IsNone)
			{
				return false;
			}
			return Time.frameCount == RetrievableResourceSingleton<InputHandler>.Instance.m_inputBlockCount.Value;
		}

		private static InputScheme ToInputScheme(string playerInput)
		{
			string currentControlScheme = RetrievableResourceSingleton<InputHandler>.Instance.m_playerInput.currentControlScheme;
			if (!(currentControlScheme == "Keyboard&Mouse"))
			{
				if (currentControlScheme == "Gamepad")
				{
					return InputScheme.Gamepad;
				}
				return InputScheme.Unknown;
			}
			return InputScheme.KeyboardMouse;
		}
	}
}

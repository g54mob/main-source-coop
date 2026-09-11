using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zorro.Core;

namespace Zorro.ControllerSupport
{
	public class GamepadTypeIconSelector : MonoBehaviour
	{
		[Serializable]
		public class GamepadIcon
		{
			public GamepadType Gamepad;

			public Image Icon;
		}

		public GamepadIcon[] m_icons;

		private GamepadIcon m_enabledIcon;

		private void OnEnable()
		{
			InputHandler instance = RetrievableResourceSingleton<InputHandler>.Instance;
			instance.InputSchemeChanged = (Action<InputScheme>)Delegate.Combine(instance.InputSchemeChanged, new Action<InputScheme>(HandleSchemeChange));
		}

		private void OnDisable()
		{
			InputHandler instance = RetrievableResourceSingleton<InputHandler>.Instance;
			instance.InputSchemeChanged = (Action<InputScheme>)Delegate.Remove(instance.InputSchemeChanged, new Action<InputScheme>(HandleSchemeChange));
		}

		private void Start()
		{
			GamepadIcon[] icons = m_icons;
			for (int i = 0; i < icons.Length; i++)
			{
				icons[i].Icon.enabled = false;
			}
			HandleSchemeChange(InputHandler.GetCurrentUsedInputScheme());
		}

		private void HandleSchemeChange(InputScheme scheme)
		{
			if (scheme != InputScheme.Gamepad)
			{
				SetIcon(null);
				return;
			}
			GamepadType gamepadType = InputHandler.GetGamepadType();
			GamepadIcon icon = m_icons.FirstOrDefault((GamepadIcon x) => x.Gamepad == gamepadType);
			SetIcon(icon);
		}

		private void SetIcon(GamepadIcon icon)
		{
			if (m_enabledIcon != null)
			{
				m_enabledIcon.Icon.enabled = false;
			}
			if (icon != null)
			{
				icon.Icon.enabled = true;
			}
			m_enabledIcon = icon;
		}
	}
}

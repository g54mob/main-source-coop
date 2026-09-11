using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;

public class EscapePlayerCellGlyphs : MonoBehaviour
{
	[Serializable]
	public class GamepadIcon
	{
		public GamepadType Gamepad;

		public Image Icon;
	}

	public GamepadIcon[] m_icons;

	private GamepadIcon m_enabledIcon;

	private void Start()
	{
		ResetGlyphs();
	}

	private void ResetGlyphs()
	{
		GamepadIcon[] icons = m_icons;
		for (int i = 0; i < icons.Length; i++)
		{
			icons[i].Icon.gameObject.SetActive(value: false);
		}
	}

	private void LateUpdate()
	{
		if (InputHandler.GetCurrentUsedInputScheme() != InputScheme.Gamepad)
		{
			ResetGlyphs();
			m_enabledIcon = null;
			return;
		}
		GamepadType scheme = InputHandler.GetGamepadType();
		GamepadIcon gamepadIcon = m_icons.FirstOrDefault((GamepadIcon gamepadIcon2) => gamepadIcon2.Gamepad == scheme);
		if (gamepadIcon != m_enabledIcon)
		{
			if (m_enabledIcon != null)
			{
				m_enabledIcon.Icon.gameObject.SetActive(value: false);
			}
			gamepadIcon?.Icon.gameObject.SetActive(value: true);
			m_enabledIcon = gamepadIcon;
		}
	}
}

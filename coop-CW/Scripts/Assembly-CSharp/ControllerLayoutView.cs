using UnityEngine;
using UnityEngine.InputSystem;
using Zorro.ControllerSupport;

public class ControllerLayoutView : MonoBehaviour
{
	public GameObject m_noControllerLayout;

	public GameObject m_xboxControllerLayout;

	public GameObject m_dualSenseControllerLayout;

	public GameObject m_dualShockControllerLayout;

	public GameObject m_steamDeckLayout;

	public GameObject m_switchControllerLayout;

	public GameObject m_switchProControllerLayout;

	public GameObject m_switch2ControllerLayout;

	public GameObject m_switch2ProControllerLayout;

	private void OnEnable()
	{
		SetGamemapd();
		InputSystem.onDeviceChange += OnDeviceChange;
	}

	private void OnDisable()
	{
		InputSystem.onDeviceChange -= OnDeviceChange;
	}

	private void OnDeviceChange(InputDevice arg1, InputDeviceChange arg2)
	{
		SetGamemapd();
	}

	private void SetGamemapd()
	{
		m_noControllerLayout.SetActive(value: false);
		m_xboxControllerLayout.SetActive(value: false);
		m_dualSenseControllerLayout.SetActive(value: false);
		m_dualShockControllerLayout.SetActive(value: false);
		m_steamDeckLayout.SetActive(value: false);
		m_switchControllerLayout.SetActive(value: false);
		m_switchProControllerLayout.SetActive(value: false);
		m_switch2ControllerLayout.SetActive(value: false);
		m_switch2ProControllerLayout.SetActive(value: false);
		(InputHandler.GetGamepadType() switch
		{
			GamepadType.Dualsense => m_dualSenseControllerLayout, 
			GamepadType.Dualshock => m_dualShockControllerLayout, 
			GamepadType.Xbox => m_xboxControllerLayout, 
			GamepadType.SteamDeck => m_steamDeckLayout, 
			GamepadType.Switch => m_switchControllerLayout, 
			GamepadType.Switch2 => m_switch2ControllerLayout, 
			_ => m_noControllerLayout, 
		}).SetActive(value: true);
	}
}

using System;
using UnityEngine;
using Zorro.ControllerSupport;

public class KeybinderHandler : RetrievableSingleton<KeybinderHandler>
{
	private KeybindListenHandle _currentListenHandle;

	public KeybindListenHandle StartListening(Action<KeyCode> onKeycodeSet)
	{
		_currentListenHandle = new KeybindListenHandle(onKeycodeSet);
		return _currentListenHandle;
	}

	public void StopListening(KeybindListenHandle handle)
	{
		if (_currentListenHandle == handle)
		{
			_currentListenHandle = null;
		}
	}

	private void Update()
	{
		if (_currentListenHandle == null || InputHandler.GetCurrentUsedInputScheme() != InputScheme.KeyboardMouse)
		{
			return;
		}
		foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
		{
			if (value < KeyCode.JoystickButton0 && Input.GetKeyDown(value))
			{
				_currentListenHandle.OnKeycodeSet(value);
				_currentListenHandle = null;
				break;
			}
		}
	}
}

using System;
using UnityEngine;

public class KeybindListenHandle
{
	public Action<KeyCode> OnKeycodeSet;

	public KeybindListenHandle(Action<KeyCode> onKeycodeSet)
	{
		OnKeycodeSet = onKeycodeSet;
	}
}

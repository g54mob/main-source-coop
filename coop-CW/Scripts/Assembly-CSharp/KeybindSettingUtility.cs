using UnityEngine;
using UnityEngine.InputSystem;

public static class KeybindSettingUtility
{
	public static void RebindInputAction(string name, KeyCode code)
	{
		int num = (int)code;
		string text = code.ToString().ToLowerInvariant();
		if (num < 320)
		{
			text = "<Keyboard>/" + text;
		}
		else if (num < 330)
		{
			text = "<Mouse>/" + text;
		}
		InputSystem.actions[name].ChangeBindingWithGroup("Keyboard&Mouse").WithPath(text);
	}
}

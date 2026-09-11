using System.Collections;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.CLI;
using Zorro.Settings;

public class GlobalInputHandler : RetrievableSingleton<GlobalInputHandler>
{
	public class InputKey
	{
		public KeyCodeSetting KeyCodeSetting;

		public void SetKeybind(KeyCodeSetting getSetting)
		{
			KeyCodeSetting = getSetting;
		}

		public bool GetKeyDown()
		{
			if (RetrievableSingleton<GlobalInputHandler>.Instance == null)
			{
				return false;
			}
			if (KeyCodeSetting == null)
			{
				return false;
			}
			if (CanTakeInput())
			{
				return Input.GetKeyDown(KeyCodeSetting.Keycode());
			}
			return false;
		}

		public bool GetKey()
		{
			if (RetrievableSingleton<GlobalInputHandler>.Instance == null)
			{
				return false;
			}
			if (KeyCodeSetting == null)
			{
				return false;
			}
			if (CanTakeInput())
			{
				return Input.GetKey(KeyCodeSetting.Keycode());
			}
			return false;
		}

		public bool GetKeyUp()
		{
			if (RetrievableSingleton<GlobalInputHandler>.Instance == null)
			{
				return false;
			}
			if (KeyCodeSetting == null)
			{
				return false;
			}
			return Input.GetKeyUp(KeyCodeSetting.Keycode());
		}
	}

	public static InputKey WalkForwardKey = new InputKey();

	public static InputKey WalkBackwardKey = new InputKey();

	public static InputKey WalkLeftKey = new InputKey();

	public static InputKey WalkRightKey = new InputKey();

	public static InputKey SprintKey = new InputKey();

	public static InputKey JumpKey = new InputKey();

	public static InputKey CrouchKey = new InputKey();

	public static InputKey InteractKey = new InputKey();

	public static InputKey DropKey = new InputKey();

	public static InputKey EmoteKey = new InputKey();

	public static InputKey ToggleSelfieModeKey = new InputKey();

	public static InputKey PushToTalkKey = new InputKey();

	public static InputKey SelectItem1Key = new InputKey();

	public static InputKey SelectItem2Key = new InputKey();

	public static InputKey SelectItem3Key = new InputKey();

	protected override void OnCreated()
	{
		base.OnCreated();
		Object.DontDestroyOnLoad(base.gameObject);
		StartCoroutine(SetKeybinds());
		static IEnumerator SetKeybinds()
		{
			yield return null;
			WalkForwardKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<WalkForwardKeybindSetting>());
			WalkBackwardKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<WalkBackwardKeybindSetting>());
			WalkLeftKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<WalkLeftKeybindSetting>());
			WalkRightKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<WalkRightKeybindSetting>());
			SprintKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<SprintKeybindSetting>());
			JumpKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<JumpKeybindSetting>());
			CrouchKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<CrouchKeybindSetting>());
			InteractKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<InteractKeybindSetting>());
			DropKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<DropKeybindSetting>());
			EmoteKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<EmoteKeybindSetting>());
			ToggleSelfieModeKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<ToggleSelfieModeKeybindSetting>());
			PushToTalkKey.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<PushToTalkButtonSetting>());
			SelectItem1Key.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<SelectItem1KeybindSetting>());
			SelectItem2Key.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<SelectItem2KeybindSetting>());
			SelectItem3Key.SetKeybind(GameHandler.Instance.SettingsHandler.GetSetting<SelectItem3KeybindSetting>());
		}
	}

	public static bool CanTakeInput()
	{
		if (RetrievableResourceSingleton<Modal>.Instance != null && RetrievableResourceSingleton<Modal>.Instance.Open)
		{
			return false;
		}
		if (Singleton<EscapeMenu>.Instance != null && Singleton<EscapeMenu>.Instance.Open)
		{
			return false;
		}
		if (Singleton<DebugUIHandler>.Instance.IsOpen)
		{
			return false;
		}
		return true;
	}

	public static bool GetKey(KeyCode keyCode)
	{
		if (CanTakeInput())
		{
			return Input.GetKey(keyCode);
		}
		return false;
	}

	public static bool GetKeyDown(KeyCode escape)
	{
		if (CanTakeInput())
		{
			return Input.GetKeyDown(escape);
		}
		return false;
	}

	public static bool GetKeyUp(KeyCode keyCode)
	{
		if (CanTakeInput())
		{
			return Input.GetKeyUp(keyCode);
		}
		return false;
	}

	public static bool GetMouseButtonDown(int button)
	{
		if (CanTakeInput())
		{
			return Input.GetMouseButtonDown(button);
		}
		return false;
	}

	public static bool GetMouseButtonUp(int button)
	{
		return Input.GetMouseButtonUp(button);
	}
}

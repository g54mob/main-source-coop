using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Settings;

public class KeyCodeSettingUI : SettingInputUICell
{
	public TextMeshProUGUI label;

	public Button button;

	private KeyCodeSetting keyCodeSetting;

	private KeybindListenHandle keybindListenHandle;

	private ISettingHandler settingHandler;

	public override void Setup(Setting setting, ISettingHandler settingHandler)
	{
		this.settingHandler = settingHandler;
		if (setting is KeyCodeSetting keyCodeSetting)
		{
			this.keyCodeSetting = keyCodeSetting;
			label.text = keyCodeSetting.Keycode().ToString();
			button.onClick.AddListener(OnButtonClicked);
		}
	}

	private void Update()
	{
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad && keybindListenHandle != null)
		{
			label.text = keyCodeSetting.Keycode().ToString();
			RetrievableSingleton<KeybinderHandler>.Instance.StopListening(keybindListenHandle);
			keybindListenHandle = null;
		}
	}

	private void OnButtonClicked()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PressAnyKeySetting);
		label.text = localizedString;
		Debug.Log($"{keyCodeSetting} button clicked!");
		keybindListenHandle = RetrievableSingleton<KeybinderHandler>.Instance.StartListening(delegate(KeyCode keycode)
		{
			label.text = keycode.ToString();
			keyCodeSetting.SetValue((int)keycode, settingHandler);
			keybindListenHandle = null;
		});
	}

	private void OnDisable()
	{
		if (keybindListenHandle != null)
		{
			label.text = keyCodeSetting.Keycode().ToString();
			RetrievableSingleton<KeybinderHandler>.Instance.StopListening(keybindListenHandle);
			keybindListenHandle = null;
		}
	}
}

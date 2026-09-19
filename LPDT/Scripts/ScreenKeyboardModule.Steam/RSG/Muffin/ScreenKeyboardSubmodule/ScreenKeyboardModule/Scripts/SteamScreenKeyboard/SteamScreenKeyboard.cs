using Steamworks;

namespace RSG.Muffin.ScreenKeyboardSubmodule.ScreenKeyboardModule.Scripts.SteamScreenKeyboard
{
	public class SteamScreenKeyboard : IScreenKeyboardService
	{
		private const int DEFAULT_TEXT_FIELD_X_POSITION = 0;

		private const int DEFAULT_TEXT_FIELD_Y_POSITION = 0;

		private const int DEFAULT_TEXT_FIELD_WIDTH = 0;

		private const int DEFAULT_TEXT_FIELD_HEIGHT = 0;

		public void ShowScreenKeyboard()
		{
			SteamUtils.ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, 0, 0, 0, 0);
		}

		public void HideScreenKeyboard()
		{
			SteamUtils.DismissFloatingGamepadTextInput();
		}
	}
}

using Rewired;
using UnityEngine;

namespace ButtonsCodesConst
{
	public class ConstInputVisuals : StaticInstance<ConstInputVisuals>
	{
		[SerializeField]
		private KeyboardSprites _keyboardCodes;

		[SerializeField]
		private ControllerSprites _gamepadButtonCode;

		public Sprite GetKeyboardSprite(KeyboardKeyCode code)
		{
			return _keyboardCodes.GetSpriteWithCode(code);
		}

		public Sprite GetControllerSprite(int code, AxisRange range = AxisRange.Full)
		{
			return _gamepadButtonCode.GetSpriteWithID(code, range);
		}
	}
}

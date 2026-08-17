using System;
using Rewired;
using UnityEngine;

namespace ButtonsCodesConst
{
	[CreateAssetMenu(fileName = "ControllerSprites", menuName = "ControllerSprites", order = 1)]
	public class ControllerSprites : ScriptableObject
	{
		[Serializable]
		public struct Button
		{
			public Sprite Sprite;

			public GamepadButtonCode code;
		}

		public Button[] Buttons;

		public Sprite GetSpriteWithID(int id, AxisRange range = AxisRange.Full)
		{
			int num = id;
			switch (range)
			{
			case AxisRange.Positive:
				switch (id)
				{
				case 0:
					num = 26;
					break;
				case 2:
					num = 28;
					break;
				case 22:
					num = 20;
					break;
				}
				break;
			case AxisRange.Negative:
				switch (id)
				{
				case 1:
					num = 27;
					break;
				case 3:
					num = 29;
					break;
				case 19:
					num = 21;
					break;
				}
				break;
			}
			Button[] buttons = Buttons;
			for (int i = 0; i < buttons.Length; i++)
			{
				Button button = buttons[i];
				if (button.code == (GamepadButtonCode)num)
				{
					return button.Sprite;
				}
			}
			return null;
		}
	}
}

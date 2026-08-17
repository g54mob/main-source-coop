using System.Collections.Generic;
using GameKit.Utilities.Types.CanvasContainers;

namespace GameKit.Utilities.Types.OptionMenuButtons
{
	public class FloatingOptions : CanvasGroupFader
	{
		protected List<ButtonData> Buttons = new List<ButtonData>();

		protected virtual void AddButtons(bool clearExisting, IEnumerable<ButtonData> buttonDatas)
		{
			if (clearExisting)
			{
				RemoveButtons();
			}
			foreach (ButtonData buttonData in buttonDatas)
			{
				Buttons.Add(buttonData);
			}
		}

		protected virtual void RemoveButtons()
		{
			foreach (ButtonData button in Buttons)
			{
				ResettableObjectCaches<ButtonData>.Store(button);
			}
			Buttons.Clear();
		}
	}
}

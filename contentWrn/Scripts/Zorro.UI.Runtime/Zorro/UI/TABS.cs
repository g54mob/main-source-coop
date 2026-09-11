using System.Collections.Generic;
using UnityEngine;

namespace Zorro.UI
{
	public abstract class TABS<ButtonType> : MonoBehaviour, ITABS where ButtonType : TAB_Button
	{
		public ButtonType selectedButton;

		private List<ButtonType> buttons = new List<ButtonType>();

		protected virtual void Start()
		{
			buttons.AddRange(GetComponentsInChildren<ButtonType>(includeInactive: true));
			Debug.Log($"Found {buttons.Count} buttons of type {typeof(ButtonType).Name}");
			Select(selectedButton);
		}

		public void Select(ButtonType button)
		{
			if (selectedButton != null)
			{
				Deslect(selectedButton);
				selectedButton = null;
			}
			selectedButton = button;
			button.Select();
			OnSelected(button);
		}

		public void Deslect(ButtonType button)
		{
			selectedButton = null;
			button.Deselect();
		}

		public abstract void OnSelected(ButtonType button);

		public void SelectGeneric(TAB_Button button)
		{
			Select(button as ButtonType);
		}

		public void SelectIndex(int index)
		{
			if (index >= 0 && index < buttons.Count)
			{
				Select(buttons[index]);
			}
		}

		public void SelectNext()
		{
			int num = buttons.IndexOf(selectedButton);
			num++;
			if (num >= buttons.Count)
			{
				num = 0;
			}
			Debug.Log($"SelectNext: {num}, out of {buttons.Count}");
			SelectIndex(num);
			if (!buttons[num].gameObject.activeInHierarchy)
			{
				Debug.Log("SelectNext   : button is inactive, skipping");
				SelectNext();
			}
		}

		public void SelectPrevious()
		{
			int num = buttons.IndexOf(selectedButton);
			num--;
			if (num < 0)
			{
				num = buttons.Count - 1;
			}
			Debug.Log($"SelectNext: {num}, out of {buttons.Count}");
			SelectIndex(num);
			if (!buttons[num].gameObject.activeInHierarchy)
			{
				Debug.Log("SelectPrevious: button is inactive, skipping");
				SelectPrevious();
			}
		}
	}
}

using AeLa.EasyFeedback.FormInput;
using UnityEngine.InputSystem;

namespace AeLa.EasyFeedback.InputSystemSupport
{
	public class TabNextInputSystem : TabNextBase
	{
		private InputAction tabAction;

		private InputAction shiftAction;

		private void OnEnable()
		{
			if (tabAction == null)
			{
				tabAction = new InputAction(null, InputActionType.Value, "<Keyboard>/tab");
				tabAction.performed += OnTab;
			}
			if (shiftAction == null)
			{
				shiftAction = new InputAction(null, InputActionType.Value, "<Keyboard>/shift");
			}
			tabAction.Enable();
			shiftAction.Enable();
		}

		private void OnDisable()
		{
			tabAction.Disable();
			shiftAction.Disable();
		}

		private void OnTab(InputAction.CallbackContext ctx)
		{
			if (input.IsFocused)
			{
				bool flag = shiftAction.IsPressed();
				if ((bool)Next && !flag)
				{
					Select(Next);
				}
				else if ((bool)Previous && flag)
				{
					Select(Previous);
				}
			}
		}
	}
}

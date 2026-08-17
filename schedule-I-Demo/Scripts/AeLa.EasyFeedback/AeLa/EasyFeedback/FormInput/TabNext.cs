using UnityEngine;

namespace AeLa.EasyFeedback.FormInput
{
	public class TabNext : TabNextBase
	{
		private void Update()
		{
			if (input.IsFocused)
			{
				bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
				bool keyDown = Input.GetKeyDown(KeyCode.Tab);
				if ((bool)Next && keyDown && !flag)
				{
					Select(Next);
				}
				else if ((bool)Previous && keyDown && flag)
				{
					Select(Previous);
				}
			}
		}
	}
}

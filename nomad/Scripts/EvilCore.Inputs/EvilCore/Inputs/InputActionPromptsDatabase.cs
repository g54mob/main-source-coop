using System.Collections.Generic;
using EvilCore.Inputs.UI;
using UnityEngine;

namespace EvilCore.Inputs
{
	[CreateAssetMenu(menuName = "EvilCore/Inputs/InputActionPromptsDatabase", fileName = "InputActionPromptsDatabase")]
	public class InputActionPromptsDatabase : ScriptableObject
	{
		public List<InputActionPrompt> allPrompts;

		public InputActionPrompt GetPrimaryActionData(string inputId)
		{
			return allPrompts.Find((InputActionPrompt prompt) => prompt.inputId == inputId);
		}
	}
}

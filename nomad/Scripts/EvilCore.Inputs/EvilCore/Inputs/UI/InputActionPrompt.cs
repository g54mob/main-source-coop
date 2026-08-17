using System;
using EvilCore.Localization;

namespace EvilCore.Inputs.UI
{
	[Serializable]
	public class InputActionPrompt
	{
		public string inputId;

		[LocalizationKey(null)]
		public string actionName;

		[RewiredAction]
		public string rewiredActionName;
	}
}

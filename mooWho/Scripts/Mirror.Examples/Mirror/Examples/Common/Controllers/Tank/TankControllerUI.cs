using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class TankControllerUI : ControllerUIBase
	{
		[Serializable]
		public struct MoveTexts
		{
			public Text keyTextTurnLeft;

			public Text keyTextForward;

			public Text keyTextTurnRight;

			public Text keyTextBack;

			public Text keyTextShoot;
		}

		public struct OtherTexts
		{
			public Text keyTextShoot;
		}

		[Serializable]
		public struct OptionsTexts
		{
			public Text keyTextMouseLock;

			public Text keyTextAutoRun;

			public Text keyTextToggleUI;
		}

		[SerializeField]
		private MoveTexts moveTexts;

		[SerializeField]
		private OtherTexts otherKeys;

		[SerializeField]
		private OptionsTexts optionsTexts;

		public void Refresh(TankControllerBase.MoveKeys moveKeys, TankControllerBase.OptionsKeys optionsKeys)
		{
			moveTexts.keyTextTurnLeft.text = GetKeyText(moveKeys.TurnLeft);
			moveTexts.keyTextForward.text = GetKeyText(moveKeys.Forward);
			moveTexts.keyTextTurnRight.text = GetKeyText(moveKeys.TurnRight);
			moveTexts.keyTextBack.text = GetKeyText(moveKeys.Back);
			optionsTexts.keyTextAutoRun.text = GetKeyText(optionsKeys.AutoRun);
			optionsTexts.keyTextToggleUI.text = GetKeyText(optionsKeys.ToggleUI);
		}
	}
}

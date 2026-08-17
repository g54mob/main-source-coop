using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Common.Controllers.Flyer
{
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class FlyerControllerUI : ControllerUIBase
	{
		[Serializable]
		public struct MoveTexts
		{
			public Text keyTextTurnLeft;

			public Text keyTextForward;

			public Text keyTextTurnRight;

			public Text keyTextStrafeLeft;

			public Text keyTextBack;

			public Text keyTextStrafeRight;
		}

		[Serializable]
		public struct FlightTexts
		{
			public Text keyTextPitchDown;

			public Text keyTextPitchUp;

			public Text keyTextRollLeft;

			public Text keyTextRollRight;

			public Text keyTextAutoLevel;
		}

		[Serializable]
		public struct OptionsTexts
		{
			public Text keyTextMouseSteer;

			public Text keyTextAutoRun;

			public Text keyTextToggleUI;
		}

		[SerializeField]
		private MoveTexts moveTexts;

		[SerializeField]
		private FlightTexts flightTexts;

		[SerializeField]
		private OptionsTexts optionsTexts;

		public void Refresh(FlyerControllerBase.MoveKeys moveKeys, FlyerControllerBase.FlightKeys flightKeys, FlyerControllerBase.OptionsKeys optionsKeys)
		{
			moveTexts.keyTextTurnLeft.text = GetKeyText(moveKeys.TurnLeft);
			moveTexts.keyTextForward.text = GetKeyText(moveKeys.Forward);
			moveTexts.keyTextTurnRight.text = GetKeyText(moveKeys.TurnRight);
			moveTexts.keyTextStrafeLeft.text = GetKeyText(moveKeys.StrafeLeft);
			moveTexts.keyTextBack.text = GetKeyText(moveKeys.Back);
			moveTexts.keyTextStrafeRight.text = GetKeyText(moveKeys.StrafeRight);
			flightTexts.keyTextPitchDown.text = GetKeyText(flightKeys.PitchDown);
			flightTexts.keyTextPitchUp.text = GetKeyText(flightKeys.PitchUp);
			flightTexts.keyTextRollLeft.text = GetKeyText(flightKeys.RollLeft);
			flightTexts.keyTextRollRight.text = GetKeyText(flightKeys.RollRight);
			flightTexts.keyTextAutoLevel.text = GetKeyText(flightKeys.AutoLevel);
			optionsTexts.keyTextMouseSteer.text = GetKeyText(optionsKeys.MouseSteer);
			optionsTexts.keyTextAutoRun.text = GetKeyText(optionsKeys.AutoRun);
			optionsTexts.keyTextToggleUI.text = GetKeyText(optionsKeys.ToggleUI);
		}
	}
}

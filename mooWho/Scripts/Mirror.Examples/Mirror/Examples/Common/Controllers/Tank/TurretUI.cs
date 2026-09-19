using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class TurretUI : ControllerUIBase
	{
		[Serializable]
		public struct MoveTexts
		{
			public Text keyTextPitchUp;

			public Text keyTextPitchDown;

			public Text keyTextTurnLeft;

			public Text keyTextTurnRight;
		}

		[SerializeField]
		private MoveTexts moveTexts;

		public void Refresh(TankTurretBase.MoveKeys moveKeys, TankTurretBase.OptionsKeys optionsKeys)
		{
			moveTexts.keyTextPitchUp.text = GetKeyText(moveKeys.PitchUp);
			moveTexts.keyTextPitchDown.text = GetKeyText(moveKeys.PitchDown);
			moveTexts.keyTextTurnLeft.text = GetKeyText(moveKeys.TurnLeft);
			moveTexts.keyTextTurnRight.text = GetKeyText(moveKeys.TurnRight);
		}
	}
}

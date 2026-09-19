using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine.UI;

namespace Features.GamePhasesModule.Scripts.Views
{
	public abstract class GamePhaseDebugViewBase : ViewBehaviour
	{
		public TMP_Text CurrentPhaseTimeText;

		public TMP_Text CurrentMultiplierText;

		public TMP_Text CurrentPhaseCountText;

		public Button SkipGamePhaseButton;
	}
}

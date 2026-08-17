using Rewired;
using UnityEngine;

namespace UIControllers
{
	public class PlayMenuController : UIController
	{
		private Player _greg;

		[SerializeField]
		private GameObject _pullRopeButton;

		[SerializeField]
		private GameObject _checkPointRope;

		[SerializeField]
		private GameObject _infiniteJump;

		public void SetGameModeSingle(bool value)
		{
			_greg = ReInput.players.GetPlayer(2);
			StaticInstance<AssistModeManager>.Instance.PullingRope = true;
			StaticInstance<DataBetweenScenes>.Instance.SinglePlayer = value;
			foreach (Controller controller in ReInput.controllers.Controllers)
			{
				_greg.controllers.AddController(controller, removeFromOtherPlayers: false);
			}
		}

		public void Update()
		{
			_pullRopeButton.SetActive(StaticInstance<AssistModeManager>.Instance.PullingRope);
			_checkPointRope.SetActive(StaticInstance<AssistModeManager>.Instance.CheckPoints);
			_infiniteJump.SetActive(StaticInstance<AssistModeManager>.Instance.InfiniteJumps);
		}
	}
}

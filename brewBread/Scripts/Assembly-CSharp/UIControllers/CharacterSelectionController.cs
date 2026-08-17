using Rewired;
using UnityEngine;

namespace UIControllers
{
	public class CharacterSelectionController : UIController
	{
		[SerializeField]
		private CharacterSelectionAnimation _animationController;

		private Player _player1;

		private Player _player2;

		private int _playersJoined;

		public override void Start()
		{
			base.Start();
			CleanControllers();
			StaticInstance<UIManager>.Instance.UIConfirm.AddListener(ManageConfirmInput);
			StaticInstance<UIManager>.Instance.UICancel.AddListener(ManageCancelInput);
			_player1 = ReInput.players.GetPlayer(0);
			_player2 = ReInput.players.GetPlayer(1);
			_playersJoined = 0;
		}

		private void CleanControllers()
		{
			foreach (Player player in ReInput.players.Players)
			{
				if (player.id == 3)
				{
					continue;
				}
				foreach (Controller controller in ReInput.controllers.Controllers)
				{
					if (controller.type != ControllerType.Keyboard)
					{
						player.controllers.RemoveController(controller);
					}
				}
			}
		}

		private void Update()
		{
			if (_playersJoined == 2)
			{
				_animationController.AllPlayersJoined();
			}
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			StaticInstance<UIManager>.Instance?.UIConfirm.RemoveListener(ManageConfirmInput);
			StaticInstance<UIManager>.Instance?.UICancel.RemoveListener(ManageCancelInput);
		}

		private void ManageCancelInput(Controller controller)
		{
			if (controller.type == ControllerType.Keyboard)
			{
				KeyboardLeft();
			}
			else
			{
				ControllerLeft(controller);
			}
		}

		private void KeyboardLeft()
		{
			if (_playersJoined == 2)
			{
				_playersJoined--;
				_animationController.PlayerLeaves(2);
			}
			else if (_playersJoined == 1)
			{
				_playersJoined--;
				_animationController.PlayerLeaves(1);
			}
			else if (_playersJoined == 0)
			{
				_animationController.QuitCharacterSelection();
			}
		}

		private void ControllerLeft(Controller controller)
		{
			if (_player1.controllers.ContainsController(controller.type, controller.id))
			{
				_player1?.controllers.RemoveController(controller);
				_playersJoined--;
				_animationController.PlayerLeaves(1);
			}
			else if (_player2.controllers.ContainsController(controller.type, controller.id))
			{
				_player2?.controllers.RemoveController(controller);
				_playersJoined--;
				_animationController.PlayerLeaves(2);
			}
			else if (_playersJoined == 0)
			{
				_animationController.QuitCharacterSelection();
			}
		}

		private void ManageConfirmInput(Controller controller)
		{
			if (controller.type == ControllerType.Keyboard)
			{
				KeyboardJoined();
			}
			else
			{
				ControllerJoined(controller);
			}
		}

		private void KeyboardJoined()
		{
			if (_playersJoined == 0)
			{
				_playersJoined++;
				_animationController.PlayerJoined(1);
			}
			else if (_playersJoined == 1)
			{
				_playersJoined++;
				_animationController.PlayerJoined(2);
			}
		}

		private void ControllerJoined(Controller controller)
		{
			if (_player1.controllers.joystickCount == 0)
			{
				_player1?.controllers.AddController(controller, removeFromOtherPlayers: false);
				_playersJoined++;
				_animationController.PlayerJoined(1);
			}
			else if (!_player1.controllers.ContainsController(controller.type, controller.id) && _player2.controllers.joystickCount == 0)
			{
				_player2?.controllers.AddController(controller, removeFromOtherPlayers: false);
				_animationController.PlayerJoined(2);
				_playersJoined++;
			}
		}
	}
}

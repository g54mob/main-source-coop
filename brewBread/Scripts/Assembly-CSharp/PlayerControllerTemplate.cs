using Rewired;
using UnityEngine;

public class PlayerControllerTemplate : MonoBehaviour
{
	[SerializeField]
	private int _playerID;

	private Player _player;

	private void Start()
	{
		_player = ReInput.players.GetPlayer(_playerID);
		Controller currentController = GetCurrentController();
		if (currentController != null && currentController.templateCount != 0)
		{
			_ = currentController.Templates[0];
		}
	}

	private Controller GetCurrentController()
	{
		Controller lastActiveController = _player.controllers.GetLastActiveController();
		if (lastActiveController != null)
		{
			return lastActiveController;
		}
		if (_player.controllers.joystickCount > 0)
		{
			return _player.controllers.Joysticks[0];
		}
		return null;
	}
}

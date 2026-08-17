using EvilCore.UI.Scripts;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class PlayerMenuFreezeHandler : MonoBehaviour
	{
		private IGameUIManager _gameUIManager;

		private IPlayerService _playerService;

		[Inject]
		private void Construct(IGameUIManager gameUIManager, IPlayerService playerService)
		{
			_gameUIManager = gameUIManager;
			_playerService = playerService;
			_gameUIManager.OnMenuOpened += OnMenuOpened;
			_gameUIManager.OnMenuClosed += OnMenuClosed;
		}

		private void OnDestroy()
		{
			if (_gameUIManager != null)
			{
				_gameUIManager.OnMenuOpened -= OnMenuOpened;
				_gameUIManager.OnMenuClosed -= OnMenuClosed;
			}
		}

		private void OnMenuOpened()
		{
			if (_playerService != null && _playerService.IsPlayerSpawned)
			{
				if (_playerService.TryGetFirstPersonController(out var controller))
				{
					controller.DisableMovement();
					controller.DisableCameraRotate();
					controller.DisableCharacterRotate();
					controller.DisableJump();
					controller.DisableCrouch();
					controller.DisableSprint();
					controller.DisableZoom();
				}
				if (_playerService.TryGetInteractionManager(out var manager))
				{
					manager.DisableInteraction();
				}
			}
		}

		private void OnMenuClosed()
		{
			if (_playerService == null || !_playerService.IsPlayerSpawned)
			{
				return;
			}
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.EnableCameraRotate();
				controller.EnableZoom();
				if (!controller.IsSitting)
				{
					controller.EnableMovement();
					controller.EnableCharacterRotate();
					controller.EnableJump();
					controller.EnableCrouch();
					controller.EnableSprint();
				}
			}
			if (_playerService.TryGetInteractionManager(out var manager))
			{
				manager.EnableInteraction();
			}
		}
	}
}

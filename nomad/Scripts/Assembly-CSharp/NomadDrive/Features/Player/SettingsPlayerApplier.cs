using EvilCore.Settings;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class SettingsPlayerApplier : MonoBehaviour
	{
		[Inject]
		private ISettingsManager _settingsManager;

		[Inject]
		private IPlayerService _playerService;

		private void OnEnable()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered += ApplyDeferredSettings;
			}
		}

		private void OnDisable()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= ApplyDeferredSettings;
			}
		}

		private void ApplyDeferredSettings()
		{
			if (_settingsManager != null && _playerService.TryGetFirstPersonController(out var controller))
			{
				Vector2 sensitivityScale = new Vector2(_settingsManager.SensitivityX, _settingsManager.SensitivityY);
				controller.ApplySettingsOverrides(sensitivityScale, _settingsManager.Fov, _settingsManager.InvertMouseY);
			}
		}
	}
}

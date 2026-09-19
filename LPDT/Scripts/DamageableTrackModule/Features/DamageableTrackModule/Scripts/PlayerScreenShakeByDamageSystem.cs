using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.ScreenShakeModule.Scripts;
using Zenject;

namespace Features.DamageableTrackModule.Scripts
{
	public class PlayerScreenShakeByDamageSystem : IInitializable, IDisposable
	{
		private readonly IScreenShakeService _screenShakeService;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ScreenShakeByDamageConfiguration _screenShakeByDamageConfiguration;

		private IDamageable _damagable;

		public PlayerScreenShakeByDamageSystem(IScreenShakeService screenShakeService, PlayerDamageablesTrackModel playerDamageablesTrackModel, MultiplayerModel multiplayerModel, ScreenShakeByDamageConfiguration screenShakeByDamageConfiguration)
		{
			_screenShakeService = screenShakeService;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_multiplayerModel = multiplayerModel;
			_screenShakeByDamageConfiguration = screenShakeByDamageConfiguration;
		}

		public void Initialize()
		{
			_playerDamageablesTrackModel.OnPlayerDamageableAdded += SubscribeOnDamage;
		}

		public void Dispose()
		{
			_playerDamageablesTrackModel.OnPlayerDamageableAdded -= SubscribeOnDamage;
			if (_damagable != null)
			{
				_damagable.OnDamaged -= OnDamaged;
			}
		}

		private void SubscribeOnDamage(int playerId, IDamageable damagable)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_damagable = damagable;
				damagable.OnDamaged += OnDamaged;
			}
		}

		private void OnDamaged(DamageData damageData)
		{
			_screenShakeService.TriggerLocalScreenShake(_screenShakeByDamageConfiguration.ScreenShakeDataByDamage);
		}
	}
}

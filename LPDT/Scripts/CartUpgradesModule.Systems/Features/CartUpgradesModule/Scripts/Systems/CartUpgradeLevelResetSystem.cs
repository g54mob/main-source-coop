using System;
using Features.CartUpgradesModule.Scripts.Core;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Zenject;

namespace Features.CartUpgradesModule.Scripts.Systems
{
	public class CartUpgradeLevelResetSystem : IInitializable, IDisposable
	{
		private readonly ICartUpgradeService _cartUpgradeService;

		private readonly BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		public CartUpgradeLevelResetSystem(ICartUpgradeService cartUpgradeService, BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent, MultiplayerModel multiplayerModel)
		{
			_cartUpgradeService = cartUpgradeService;
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend += ResetModules;
		}

		public void Dispose()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend -= ResetModules;
		}

		private void ResetModules(BeforeLevelChangeNetworkEvent _)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient)
			{
				_cartUpgradeService.ResetModules();
			}
		}
	}
}

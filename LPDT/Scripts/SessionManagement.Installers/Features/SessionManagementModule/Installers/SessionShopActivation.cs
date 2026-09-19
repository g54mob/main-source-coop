using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.StoreModule.Scripts;

namespace Features.SessionManagementModule.Installers
{
	public sealed class SessionShopActivation : IShopActivation
	{
		private readonly StorePhaseModel _storePhaseModel;

		private readonly MultiplayerModel _multiplayerModel;

		public SessionShopActivation(StorePhaseModel storePhaseModel, MultiplayerModel multiplayerModel)
		{
			_storePhaseModel = storePhaseModel;
			_multiplayerModel = multiplayerModel;
		}

		public void ActivateShop()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_storePhaseModel.SetStoreActive(isActive: true);
			}
		}

		public void DeactivateShop()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_storePhaseModel.SetStoreActive(isActive: false);
			}
		}
	}
}

using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	public class ShopVoteModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<ShopVoteModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(ShopVoteModel), ModelScope.Shop, ModelOwnership.Individual, "PlayerShopVoteNetworkObject", typeof(ShopVoteNetworkObject), (NetworkedModelBase model) => new ShopVoteModelBridge((ShopVoteModel)model))).AsCached();
		}
	}
}

using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	public class ShopModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<ShopModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(ShopModel), ModelScope.Shop, ModelOwnership.Shared, "ShopNetworkObject", typeof(ShopNetworkObject), (NetworkedModelBase model) => new ShopModelBridge((ShopModel)model))).AsCached();
		}
	}
}

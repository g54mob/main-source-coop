using Features.CartUpgradesModule.Scripts.Data;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.CartUpgradesModule.Scripts.Networked
{
	public class CartUpgradesModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<CartUpgradesModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(CartUpgradesModel), ModelScope.Run, ModelOwnership.Shared, "CartUpgradesNetworkObject", typeof(CartUpgradesNetworkObject), (NetworkedModelBase model) => new CartUpgradesModelBridge((CartUpgradesModel)model))).AsCached();
		}
	}
}

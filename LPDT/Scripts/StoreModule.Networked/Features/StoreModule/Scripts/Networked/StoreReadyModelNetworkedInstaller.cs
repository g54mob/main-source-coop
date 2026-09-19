using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.StoreModule.Scripts.Networked
{
	public class StoreReadyModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<StoreReadyModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(StoreReadyModel), ModelScope.Shop, ModelOwnership.Individual, "PlayerStoreReadyNetworkObject", typeof(StoreReadyNetworkObject), (NetworkedModelBase model) => new StoreReadyModelBridge((StoreReadyModel)model))).AsCached();
		}
	}
}

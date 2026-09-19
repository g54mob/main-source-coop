using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.StoreModule.Scripts.Networked
{
	public class StorePhaseModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<StorePhaseModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(StorePhaseModel), ModelScope.Shop, ModelOwnership.Shared, "StorePhaseNetworkObject", typeof(StorePhaseNetworkObject), (NetworkedModelBase model) => new StorePhaseModelBridge((StorePhaseModel)model))).AsCached();
		}
	}
}

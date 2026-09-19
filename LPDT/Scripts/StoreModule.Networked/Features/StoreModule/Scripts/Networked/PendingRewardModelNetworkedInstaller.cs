using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.StoreModule.Scripts.Networked
{
	public class PendingRewardModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<PendingRewardModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(PendingRewardModel), ModelScope.Run, ModelOwnership.Shared, "PendingRewardNetworkObject", typeof(PendingRewardNetworkObject), (NetworkedModelBase model) => new PendingRewardModelBridge((PendingRewardModel)model))).AsCached();
		}
	}
}

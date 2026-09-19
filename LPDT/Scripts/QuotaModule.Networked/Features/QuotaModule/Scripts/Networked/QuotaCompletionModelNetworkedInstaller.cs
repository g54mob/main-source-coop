using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.QuotaModule.Scripts.Networked
{
	public class QuotaCompletionModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<QuotaCompletionModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(QuotaCompletionModel), ModelScope.Level, ModelOwnership.Shared, "QuotaCompletionNetworkObject", typeof(QuotaCompletionNetworkObject), (NetworkedModelBase model) => new QuotaCompletionModelBridge((QuotaCompletionModel)model))).AsCached();
		}
	}
}

using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.QuotaModule.Scripts.Networked
{
	public class QuotaSynchronizedModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<QuotaSynchronizedModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(QuotaSynchronizedModel), ModelScope.Level, ModelOwnership.Shared, "QuotaSynchronizedNetworkObject", typeof(QuotaSynchronizedNetworkObject), (NetworkedModelBase model) => new QuotaSynchronizedModelBridge((QuotaSynchronizedModel)model))).AsCached();
		}
	}
}

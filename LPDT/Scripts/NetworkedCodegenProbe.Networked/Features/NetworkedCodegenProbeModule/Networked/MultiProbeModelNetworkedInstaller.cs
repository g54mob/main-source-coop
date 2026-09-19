using Features.NetworkedCodegenProbeModule.Data;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.NetworkedCodegenProbeModule.Networked
{
	public class MultiProbeModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<MultiProbeModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(MultiProbeModel), ModelScope.Session, ModelOwnership.Shared, "MultiProbeNetworkObject", typeof(MultiProbeNetworkObject), (NetworkedModelBase model) => new MultiProbeModelBridge((MultiProbeModel)model))).AsCached();
		}
	}
}

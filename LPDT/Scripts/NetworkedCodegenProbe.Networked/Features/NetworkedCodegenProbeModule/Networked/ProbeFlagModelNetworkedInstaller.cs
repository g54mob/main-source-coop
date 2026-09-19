using Features.NetworkedCodegenProbeModule.Data;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.NetworkedCodegenProbeModule.Networked
{
	public class ProbeFlagModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<ProbeFlagModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(ProbeFlagModel), ModelScope.Level, ModelOwnership.Individual, "PlayerProbeFlagNetworkObject", typeof(ProbeFlagNetworkObject), (NetworkedModelBase model) => new ProbeFlagModelBridge((ProbeFlagModel)model))).AsCached();
		}
	}
}

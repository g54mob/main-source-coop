using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	public class LevelModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<LevelModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(LevelModel), ModelScope.Level, ModelOwnership.Shared, "LevelNetworkObject", typeof(LevelNetworkObject), (NetworkedModelBase model) => new LevelModelBridge((LevelModel)model))).AsCached();
		}
	}
}

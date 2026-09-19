using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	public class LevelPlayerModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<LevelPlayerModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(LevelPlayerModel), ModelScope.Level, ModelOwnership.Individual, "PlayerLevelPlayerNetworkObject", typeof(LevelPlayerNetworkObject), (NetworkedModelBase model) => new LevelPlayerModelBridge((LevelPlayerModel)model))).AsCached();
		}
	}
}

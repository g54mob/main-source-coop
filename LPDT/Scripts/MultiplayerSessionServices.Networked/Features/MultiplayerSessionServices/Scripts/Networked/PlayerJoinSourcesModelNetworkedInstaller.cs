using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts.Networked
{
	public class PlayerJoinSourcesModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<PlayerJoinSourcesModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(PlayerJoinSourcesModel), ModelScope.Session, ModelOwnership.Shared, "PlayerJoinSourcesNetworkObject", typeof(PlayerJoinSourcesNetworkObject), (NetworkedModelBase model) => new PlayerJoinSourcesModelBridge((PlayerJoinSourcesModel)model))).AsCached();
		}
	}
}

using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using PlayerCustomization.LobbyAvatar.Data;
using Zenject;

namespace PlayerCustomization.LobbyAvatar.Networked
{
	public class PlayerLobbyAvatarModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<PlayerLobbyAvatarModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(PlayerLobbyAvatarModel), ModelScope.Lobby, ModelOwnership.Individual, "PlayerPlayerLobbyAvatarNetworkObject", typeof(PlayerLobbyAvatarNetworkObject), (NetworkedModelBase model) => new PlayerLobbyAvatarModelBridge((PlayerLobbyAvatarModel)model))).AsCached();
		}
	}
}

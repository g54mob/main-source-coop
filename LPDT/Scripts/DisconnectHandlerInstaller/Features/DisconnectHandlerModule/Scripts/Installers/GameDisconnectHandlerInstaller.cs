using Features.DisconnectHandlerModule.Scripts.Systems;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts.Installers
{
	public class GameDisconnectHandlerInstaller : Installer<GameDisconnectHandlerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SessionTeardownService>().AsSingle();
			base.Container.BindInterfacesTo<GameDisconnectHandlerSystem>().AsSingle();
			base.Container.BindInterfacesTo<LobbyKickHandlerSystem>().AsSingle();
		}
	}
}

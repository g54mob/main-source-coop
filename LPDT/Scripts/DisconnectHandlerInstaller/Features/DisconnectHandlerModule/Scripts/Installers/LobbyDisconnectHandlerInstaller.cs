using Features.DisconnectHandlerModule.Scripts.Systems;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts.Installers
{
	public class LobbyDisconnectHandlerInstaller : Installer<LobbyDisconnectHandlerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LobbyDisconnectHandlerSystem>().AsSingle();
		}
	}
}

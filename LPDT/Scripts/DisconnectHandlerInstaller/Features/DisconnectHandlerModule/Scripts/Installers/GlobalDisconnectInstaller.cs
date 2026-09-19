using Features.DisconnectHandlerModule.Scripts.Data;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts.Installers
{
	public class GlobalDisconnectInstaller : Installer<GlobalDisconnectInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<DisconnectRequestEventClass>().AsSingle();
		}
	}
}

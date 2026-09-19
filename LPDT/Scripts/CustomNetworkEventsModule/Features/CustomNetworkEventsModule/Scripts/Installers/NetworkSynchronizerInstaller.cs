using Zenject;

namespace Features.CustomNetworkEventsModule.Scripts.Installers
{
	public class NetworkSynchronizerInstaller : Installer<NetworkSynchronizerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<NetworkEventsSynchronizerSpawnSystem>().AsSingle();
		}
	}
}

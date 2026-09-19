using Zenject;

namespace Features.NetworkRandomModule.Scripts.Installers
{
	public class NetworkRandomModuleInstaller : Installer<NetworkRandomModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<NetworkRandomInitializationService>().AsSingle();
			base.Container.BindInterfacesTo<NetworkRandomRequestProcessSystem>().AsSingle();
		}
	}
}

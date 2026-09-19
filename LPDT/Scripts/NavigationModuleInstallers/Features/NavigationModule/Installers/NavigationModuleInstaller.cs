using Features.NavigationModule.Scripts;
using Zenject;

namespace Features.NavigationModule.Installers
{
	public class NavigationModuleInstaller : Installer<NavigationModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<NavigationService>().AsSingle();
		}
	}
}

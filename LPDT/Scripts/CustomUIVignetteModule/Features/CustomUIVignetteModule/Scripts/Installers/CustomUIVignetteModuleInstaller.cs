using Zenject;

namespace Features.CustomUIVignetteModule.Scripts.Installers
{
	public class CustomUIVignetteModuleInstaller : Installer<CustomUIVignetteModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<CustomUIVignetteService>().AsSingle();
		}
	}
}

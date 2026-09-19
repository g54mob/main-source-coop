using Zenject;

namespace Features.WorldTokenModule.Scripts.Installers
{
	public class WorldTokenModuleInstaller : Installer<WorldTokenModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<WorldTokenService>().AsSingle();
		}
	}
}

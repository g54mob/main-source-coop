using Zenject;

namespace Features.GameOverModule.Scripts.Installers
{
	public class GameOverModuleInstaller : Installer<GameOverModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<GameOverSystem>().AsSingle();
		}
	}
}

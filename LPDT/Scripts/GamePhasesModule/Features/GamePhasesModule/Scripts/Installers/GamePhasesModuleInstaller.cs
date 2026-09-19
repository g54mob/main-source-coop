using Zenject;

namespace Features.GamePhasesModule.Scripts.Installers
{
	public class GamePhasesModuleInstaller : Installer<GamePhasesModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<GamePhaseService>().AsSingle();
			base.Container.BindInterfacesTo<GamePhaseTimerSystem>().AsSingle();
			base.Container.BindInterfacesTo<GamePhaseStartupSystem>().AsSingle();
		}
	}
}

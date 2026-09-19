using Zenject;

namespace Features.LevelModule.Scripts.LevelTransition.Installers
{
	public class LevelTransitionModuleInstaller : Installer<LevelTransitionModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<BeachOccupancyModel>().AsSingle();
			base.Container.BindInterfacesTo<LevelTransitionBeachReadinessService>().AsSingle();
			base.Container.BindInterfacesTo<LevelTransitionStragglerDamageService>().AsSingle();
			base.Container.BindInterfacesTo<TimerSystem>().AsSingle();
		}
	}
}

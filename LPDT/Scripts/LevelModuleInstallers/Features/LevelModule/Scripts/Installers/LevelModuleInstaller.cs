using Features.LevelModule.Scripts.Systems;
using Zenject;

namespace Features.LevelModule.Scripts.Installers
{
	public class LevelModuleInstaller : Installer<LevelModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LevelService>().AsSingle();
			base.Container.Bind<TeleportationPointsEventClass>().AsSingle();
			base.Container.BindInterfacesTo<TeleportationPointsSystem>().AsSingle();
			base.Container.BindInterfacesTo<SpawnedRoomsShutdownResetSystem>().AsSingle();
			base.Container.BindInterfacesTo<LevelCleanupSystem>().AsSingle();
			base.Container.Bind<PlayerTransitedToLevelEvent>().AsSingle();
			base.Container.BindInterfacesTo<LevelInitializationSystem>().AsSingle();
			base.Container.BindInterfacesTo<ChapterProgressSaveSystem>().AsSingle();
		}
	}
}

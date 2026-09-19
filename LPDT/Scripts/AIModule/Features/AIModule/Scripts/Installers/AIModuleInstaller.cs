using Features.AIModule.Scripts.ManInShadows;
using Features.AIModule.Scripts.Services;
using Zenject;

namespace Features.AIModule.Scripts.Installers
{
	public class AIModuleInstaller : Installer<AIModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayersLocationService>().AsSingle();
			base.Container.BindInterfacesTo<EnemyFactory>().AsSingle();
			base.Container.BindInterfacesTo<EnemySpawnService>().AsSingle();
			base.Container.BindInterfacesTo<EnemyForceDespawnService>().AsSingle();
			base.Container.BindInterfacesTo<EnemyManualSpawnService>().AsSingle();
			base.Container.BindInterfacesTo<PlayerTrackingPositionService>().AsSingle();
			base.Container.BindInterfacesTo<PlayerDebuffCleanupSystem>().AsSingle();
			base.Container.BindInterfacesTo<EnemySpawnSystem>().AsSingle();
			base.Container.BindInterfacesTo<EnemyPrefabWarmup>().AsSingle();
			base.Container.BindInterfacesTo<SideBossSpawnSystem>().AsSingle();
			base.Container.BindInterfacesTo<EnemySpawnConfigurationProvider>().AsSingle();
			base.Container.BindInterfacesTo<PlayerPositionsProvider>().AsSingle();
			base.Container.BindInterfacesTo<EnemyPlayerReboundListener>().AsSingle();
			base.Container.Bind<EnemiesPoolModel>().AsSingle();
			base.Container.Bind<SideBossSpawnPoolModel>().AsSingle();
			base.Container.BindInterfacesTo<ManInShadowFlickerAdjustingService>().AsSingle();
		}
	}
}

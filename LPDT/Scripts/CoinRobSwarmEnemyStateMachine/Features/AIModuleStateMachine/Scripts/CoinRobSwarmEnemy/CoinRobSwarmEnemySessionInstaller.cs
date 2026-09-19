using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Systems;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	public class CoinRobSwarmEnemySessionInstaller : Installer<CoinRobSwarmEnemySessionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<CoinRobPlayerProximityService>().AsSingle();
			base.Container.Bind<CoinRobSwarmReactiveAggroService>().AsSingle();
			base.Container.BindInterfacesTo<CoinRobLifetimeSystem>().AsSingle();
			base.Container.Bind<IRatsHoleSpawnService>().To<RatsHoleSpawnService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<RatsHoleRegistry>().AsSingle();
		}
	}
}

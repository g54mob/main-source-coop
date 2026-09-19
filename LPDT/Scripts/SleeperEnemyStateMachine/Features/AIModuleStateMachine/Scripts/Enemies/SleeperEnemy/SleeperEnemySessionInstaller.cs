using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	public class SleeperEnemySessionInstaller : Installer<SleeperEnemySessionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SleeperFearHomeReassignmentSystem>().AsSingle();
		}
	}
}

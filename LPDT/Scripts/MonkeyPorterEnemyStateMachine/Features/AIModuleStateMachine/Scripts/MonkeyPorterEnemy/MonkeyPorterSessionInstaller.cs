using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class MonkeyPorterSessionInstaller : Installer<MonkeyPorterSessionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<MonkeyPorterSpawnPointsModel>().AsSingle();
			base.Container.BindInterfacesTo<MonkeyPorterSpawnSystem>().AsSingle();
		}
	}
}

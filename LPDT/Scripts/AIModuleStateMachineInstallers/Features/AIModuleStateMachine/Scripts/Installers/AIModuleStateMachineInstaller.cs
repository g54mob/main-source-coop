using Features.AIModuleStateMachine.Scripts.Services;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Installers
{
	public class AIModuleStateMachineInstaller : Installer<AIModuleStateMachineInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<EnemyPlayerAttackabilityService>().AsSingle();
			base.Container.BindInterfacesTo<EnemyTrackingService>().AsSingle();
		}
	}
}

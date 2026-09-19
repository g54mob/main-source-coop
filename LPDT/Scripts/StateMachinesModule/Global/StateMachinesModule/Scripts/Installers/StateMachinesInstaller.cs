using Zenject;

namespace Global.StateMachinesModule.Scripts.Installers
{
	public class StateMachinesInstaller : Installer<StateMachinesInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<GameFlowStateMachine>().AsSingle();
		}
	}
}

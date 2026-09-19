using Zenject;

namespace Features.GameCycle.Scripts.SessionCleanup
{
	public class SessionCleanupInstaller : Installer<SessionCleanupInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<SessionCleanupEvent>().AsSingle();
			base.Container.Bind<ContextDependentCleanupModel>().AsSingle();
			base.Container.BindInterfacesTo<SessionCleanupSystem>().AsSingle();
			base.Container.Bind<SessionCleanUpStartedEvent>().AsSingle();
		}
	}
}

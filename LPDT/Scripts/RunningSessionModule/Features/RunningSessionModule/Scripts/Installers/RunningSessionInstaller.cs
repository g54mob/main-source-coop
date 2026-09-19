using Zenject;

namespace Features.RunningSessionModule.Scripts.Installers
{
	public class RunningSessionInstaller : Installer<RunningSessionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<RunningSessionSystem>().AsSingle();
			base.Container.BindInterfacesTo<RunningSessionService>().AsSingle();
			base.Container.Bind<RunningSessionModel>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<RunningSessionPersistenceModel>().AsSingle();
		}
	}
}

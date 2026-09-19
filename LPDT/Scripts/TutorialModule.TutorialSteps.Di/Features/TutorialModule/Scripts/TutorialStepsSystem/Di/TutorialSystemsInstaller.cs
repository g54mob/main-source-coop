using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.API;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.Di
{
	public class TutorialSystemsInstaller : Installer<TutorialSystemsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<BaseTutorialService>().AsSingle();
			base.Container.Bind<BaseTutorialSkipService>().AsSingle();
			base.Container.BindInterfacesTo<TutorialStartupService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<TutorialUISystem>().AsSingle();
			base.Container.BindInterfacesTo<TutorialService>().AsSingle();
			base.Container.Bind<BaseTutorialStepsQuery>().AsSingle();
			base.Container.Bind<TutorialStepFactory>().AsSingle();
			base.Container.Bind<TutorialStepFactoryHolder>().AsSingle();
			base.Container.BindInterfacesTo<TutorialCompleteReactionSystem>().AsSingle();
		}
	}
}

using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.Installers
{
	public class BaseTutorialAdditionalProvessorsInstaller : Installer<BaseTutorialAdditionalProvessorsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<BaseTutorialContainerDataHolder>().AsSingle();
			base.Container.Bind<BaseTutorialLocationContainerDataHolder>().AsSingle();
			base.Container.Bind<BaseTutorialTargetEventClass>().AsSingle();
			base.Container.Bind<BaseTutorialTargetTrackerDataHolder>().AsSingle();
			base.Container.Bind<BaseTutorialEnemiesDataHolder>().AsSingle();
			base.Container.Bind<BaseTutorialBellDataHolder>().AsSingle();
			base.Container.Bind<BaseTutorialChosenRewardDataHolder>().AsSingle();
			base.Container.BindInterfacesTo<BaseTutorialRewardStepFactory>().AsSingle();
			base.Container.Bind<BaseTutorialStoreDataHolder>().AsSingle();
			base.Container.Bind<BaseTutorialUIDataHolder>().AsSingle();
			base.Container.BindInterfacesTo<BaseTutorialDirectionAssistSystem>().AsSingle();
		}
	}
}

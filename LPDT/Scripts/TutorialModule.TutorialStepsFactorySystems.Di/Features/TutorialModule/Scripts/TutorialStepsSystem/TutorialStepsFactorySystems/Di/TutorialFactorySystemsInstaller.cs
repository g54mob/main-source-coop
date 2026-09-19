using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.Internal;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.Di
{
	public class TutorialFactorySystemsInstaller : Installer<TutorialFactorySystemsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LocationTutorialStepFactoryRegistrationSystem>().AsSingle();
			base.Container.Bind<TutorialStepFactory>().AsSingle();
		}
	}
}

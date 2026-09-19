using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API.Installers
{
	public class TutorialDataInstaller : Installer<TutorialDataInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<TutorialModel>().AsSingle();
		}
	}
}

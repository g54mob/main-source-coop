using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class LocationTutorialSystemInstaller : Installer<LocationTutorialSystemInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LocationTutorialSystem>().AsSingle();
		}
	}
}

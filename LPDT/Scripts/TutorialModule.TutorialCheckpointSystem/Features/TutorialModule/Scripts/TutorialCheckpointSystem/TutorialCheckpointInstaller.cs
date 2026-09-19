using Zenject;

namespace Features.TutorialModule.Scripts.TutorialCheckpointSystem
{
	public class TutorialCheckpointInstaller : Installer<TutorialCheckpointInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<TutorialCheckpointService>().AsSingle();
		}
	}
}

using Zenject;

namespace Features.TutorialModule.Scripts.Analytics
{
	public class TutorialAnalyticsInstaller : Installer<TutorialAnalyticsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<TutorialAnalyticsSystem>().AsSingle();
		}
	}
}

using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts
{
	public class GameanalyticsJournalingModuleInstaller : Installer<GameanalyticsJournalingModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<InitializeSentAnalyticsModelSystem>().AsSingle();
		}
	}
}

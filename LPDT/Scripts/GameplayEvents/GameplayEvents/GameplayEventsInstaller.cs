using Zenject;

namespace GameplayEvents
{
	public class GameplayEventsInstaller : Installer<GameplayEventsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<GameplayEventBus>().AsSingle();
		}
	}
}

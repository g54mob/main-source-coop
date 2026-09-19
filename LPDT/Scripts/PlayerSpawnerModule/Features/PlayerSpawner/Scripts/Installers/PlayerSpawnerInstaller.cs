using Zenject;

namespace Features.PlayerSpawner.Scripts.Installers
{
	public class PlayerSpawnerInstaller : Installer<PlayerSpawnerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerStatsInitializeService>().AsSingle();
			base.Container.BindInterfacesTo<SpawnedPlayersMasterTrackingSystem>().AsSingle();
		}
	}
}

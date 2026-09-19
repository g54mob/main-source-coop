using Zenject;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class PlayersStatisticsInstaller : Installer<PlayersStatisticsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<PlayersDeathsStatisticsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayersContributedQuotaStatisticsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayersKillsStatisticsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayersRevivesStatisticsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StatisticsWindowSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<LevelStatisticsResetSystem>().AsSingle();
		}
	}
}

using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.Factories.EntityStatTypeFactories;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories;
using Zenject;

namespace Features.StatsUsageModule.Scripts.Installers
{
	public class StatsInstaller : Installer<StatsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IStatEntityFactory<EntityStatType>>().To<EntityStatEntityFactory>().AsSingle();
			base.Container.Bind<IStatFactory<EntityStatType>>().To<EntityStatFactory>().AsSingle();
			base.Container.BindInterfacesTo<PlayerStatsUpgradeService>().AsSingle();
		}
	}
}

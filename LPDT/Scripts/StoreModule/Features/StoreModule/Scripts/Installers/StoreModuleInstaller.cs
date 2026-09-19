using Zenject;

namespace Features.StoreModule.Scripts.Installers
{
	public class StoreModuleInstaller : Installer<StoreModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<CardItemSpawner>().AsSingle();
			base.Container.BindInterfacesTo<StoreRewardSpawnSystem>().AsSingle();
			base.Container.Bind<StoreRewardModel>().AsSingle();
			base.Container.Bind<StoreRewardServices>().AsSingle();
			base.Container.Bind<StoreRewardFactory>().AsSingle();
			base.Container.BindInterfacesTo<StoreReconnectSystem>().AsSingle();
			base.Container.BindInterfacesTo<StoreSessionCloseAnalyticsSystem>().AsSingle();
		}
	}
}

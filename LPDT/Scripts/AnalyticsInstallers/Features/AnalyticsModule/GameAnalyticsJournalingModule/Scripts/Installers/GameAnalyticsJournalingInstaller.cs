using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems;
using Features.GameJournalingModule.Scripts.Core;
using JetBrains.Annotations;
using RSG.Muffin.AssetLoaderModule.Core;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Installers
{
	[PublicAPI]
	public class GameAnalyticsJournalingInstaller : Installer<GameAnalyticsJournalingInstaller>, IConcreteJournalingSystemInstaller, IMockableInstaller
	{
		public override void InstallBindings()
		{
			IAssetLoaderFacadeService loaderService = base.Container.Resolve<IAssetLoaderFacadeService>();
			BindGameAnalyticsConfiguration(loaderService);
			BindGameAnalyticsServices();
			BindGameAnalyticsJournalingSystem();
		}

		private void BindGameAnalyticsServices()
		{
			base.Container.Bind<AnalyticsEventQuota>().AsSingle();
			base.Container.Bind<GameAnalyticsEventSendService>().AsSingle();
			base.Container.Bind<GameAnalyticsMarkUserService>().AsSingle();
		}

		private void BindGameAnalyticsJournalingSystem()
		{
			base.Container.BindInterfacesTo<GameAnalyticsJournalingSystem>().AsSingle();
			base.Container.BindInterfacesTo<AnalyticsSystem>().AsSingle();
			base.Container.BindInterfacesTo<StoreSessionAnalyticsHostCoordinatorSystem>().AsSingle();
			base.Container.BindInterfacesTo<StoreSessionCloseAnalyticsSystem>().AsSingle();
			base.Container.BindInterfacesTo<PlayerDeathCausedAnalyticsSystem>().AsSingle();
			base.Container.BindInterfacesTo<SessionStartAnalyticsSystem>().AsSingle();
			base.Container.BindInterfacesTo<SessionEndAnalyticsSystem>().AsSingle();
			base.Container.BindInterfacesTo<SessionEndPlaceTrackingSystem>().AsSingle();
		}

		private void BindGameAnalyticsConfiguration(IAssetLoaderFacadeService loaderService)
		{
			string key = "GameAnalyticsConfiguration_Default";
			GameAnalyticsConfiguration resource = loaderService.LoadAsset<GameAnalyticsConfiguration>(key, AssetLoadSource.Addressables);
			base.Container.Bind<GameAnalyticsConfiguration>().FromScriptableObject(resource).AsSingle();
		}
	}
}

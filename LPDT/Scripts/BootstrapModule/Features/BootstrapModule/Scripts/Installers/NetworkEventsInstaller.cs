using Features.CurrencyModule.Scripts;
using Features.CustomUIVignetteModule.Scripts;
using Features.EmotesModule.Scripts;
using Features.GameOverModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.LevelModule.Scripts;
using Features.MainMenuModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.RagdollModule.Scripts;
using Features.SceneTransitionsModule.Scripts;
using Features.StruggleBarModule.Scripts;
using Zenject;

namespace Features.BootstrapModule.Scripts.Installers
{
	public class NetworkEventsInstaller : Installer<NetworkEventsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<GameOverNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<AddCurrencyNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<ItemCostLossNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<BeforeLevelChangeNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<ChapterCompletedNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StartGameLoadingNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StartLevelLoadingTransitionNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StartFadeTransitionNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<OnLevelLoadedNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayerSpawnRequestNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayerSpawnReadyNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<OnVignetteStartedNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<OnVignetteDisabledNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<OnVignettePausedNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<NetworkPlayerFollowRequest>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayerBodyEmoteNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StruggleBarCompletedNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StruggleBarFailedNetworkEvent>().AsSingle();
		}
	}
}

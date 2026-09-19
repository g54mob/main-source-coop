using Features.EmotesModule.Scripts;
using Features.ItemsModule.Scripts.Installers;
using Features.Movement.Scripts.Installers;
using Features.PlayerStatesModule.Scripts;
using Features.ScreenShakeModule.Scripts.Installers;
using Features.StatsUsageModule.Scripts.Installers;
using Zenject;

namespace Features.BootstrapModule.Scripts.Installers
{
	public class GlobalNetworkPrefabInjectionInstaller : Installer<GlobalNetworkPrefabInjectionInstaller>
	{
		public override void InstallBindings()
		{
			Installer<ScreenShakeInstallers>.Install(base.Container);
			Installer<StatsInstaller>.Install(base.Container);
			Installer<ItemsModuleInstaller>.Install(base.Container);
			Installer<MovementInstaller>.Install(base.Container);
			base.Container.BindInterfacesTo<PlayerStateService>().AsSingle();
			base.Container.Bind<EmotesTriggerModel>().AsSingle();
		}
	}
}

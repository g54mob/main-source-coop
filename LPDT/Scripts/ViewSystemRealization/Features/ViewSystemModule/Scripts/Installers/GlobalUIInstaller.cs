using Features.ConfirmExitPopupService.Scripts;
using Features.SettingsMenuModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using Zenject;

namespace Features.ViewSystemModule.Scripts.Installers
{
	public class GlobalUIInstaller : Installer<GlobalUIInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<SettingsWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<SteamDeckSettingsWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<MenuSettingsWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<SteamDeckMenuSettingsWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<OverlayGlobalWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<GammaSetupWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<ConfirmationPopupWindow>().AsSingle();
			base.Container.Bind<ISettingsWindowProvider>().To<SettingsWindowProvider>().AsSingle();
		}
	}
}

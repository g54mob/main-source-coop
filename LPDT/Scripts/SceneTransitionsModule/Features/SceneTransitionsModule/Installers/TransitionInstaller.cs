using Features.SceneTransitionsModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Zenject;

namespace Features.SceneTransitionsModule.Installers
{
	public class TransitionInstaller : Installer<TransitionInstaller>
	{
		public override void InstallBindings()
		{
			TransitionInstallerConfiguration transitionInstallerConfiguration = base.Container.Resolve<TransitionInstallerConfiguration>();
			base.Container.Bind<ILoadingScreenPresetResolver>().To<LoadingScreenPresetResolver>().AsSingle();
			base.Container.Bind<LoadingScreenModel>().AsSingle();
			base.Container.Bind<SceneTransitionCanvas>().FromComponentInNewPrefab(transitionInstallerConfiguration.SceneTransitionCanvas).AsSingle()
				.NonLazy();
			base.Container.Bind<SceneTransitionCamera>().FromComponentInNewPrefab(transitionInstallerConfiguration.SceneTransitionCamera).AsSingle()
				.NonLazy();
			base.Container.BindInterfacesAndSelfTo<TransitionElementsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<NetworkTransitionProcessSystem>().AsSingle();
			base.Container.Bind<LoadingScreenWindow>().AsSingle();
			base.Container.BindInterfacesTo<LoadingScreenBootstrapSystem>().AsSingle().NonLazy();
			base.Container.BindInterfacesTo<FontsLoadingScreenSystem>().AsSingle();
			base.Container.Bind<ILoadingScreenUIService>().To<LoadingScreenUIService>().AsSingle();
			base.Container.Bind<ScreensPresenter>().AsTransient();
			base.Container.Bind<LoadingScreenTipsPresenter>().AsTransient();
			base.Container.Bind<LoadingScreenOverlayController>().AsSingle();
			base.Container.Bind<ILoadingScreenService>().To<LoadingScreenService>().AsSingle()
				.NonLazy();
		}
	}
}

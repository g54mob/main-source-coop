using Features.VignetteUIEffectModule.Scripts.Views;
using Zenject;

namespace Features.VignetteUIEffectModule.Scripts.Installers
{
	public class VignetteUIEffectModuleInstaller : Installer<VignetteUIEffectModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<VignetteUIService>().AsSingle();
			base.Container.BindInterfacesTo<VignetteUIEffectViewFactory>().AsSingle();
			base.Container.BindInterfacesTo<VignetteUILayersBootstrapService>().AsSingle();
			base.Container.BindInterfacesTo<VignetteUIEffectApplier>().AsSingle();
			base.Container.Bind<VignetteEffectContainerPresenter>().AsSingle();
			base.Container.Bind<VignetteUIEffectUIPresenter>().AsTransient();
		}
	}
}

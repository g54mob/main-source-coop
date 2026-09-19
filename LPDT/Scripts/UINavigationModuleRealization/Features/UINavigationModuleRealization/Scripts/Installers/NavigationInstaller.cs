using Features.UINavigationModuleRealization.Scripts.BackButton;
using Features.UINavigationModuleRealization.Scripts.TextInput;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using Zenject;

namespace Features.UINavigationModuleRealization.Scripts.Installers
{
	public class NavigationInstaller : Installer<NavigationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<NavigationModel>().AsSingle();
			base.Container.BindInterfacesTo<NavigationSystem>().AsSingle();
			base.Container.BindInterfacesTo<NavigationService>().AsSingle();
			base.Container.Bind<UIBackButtonModel>().AsSingle();
			base.Container.Bind<IUIBackButtonRegistrationService>().To<UIBackButtonRegistrationService>().AsSingle();
			base.Container.Bind<ITextInputFocusService>().To<TextInputFocusService>().AsSingle();
			base.Container.BindInterfacesTo<UIBackButtonSystem>().AsSingle();
		}
	}
}

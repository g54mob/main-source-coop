using RSG.Muffin.EventBusModule;
using Zenject;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class ViewSystemInstaller : Installer<ViewSystemInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<WindowsService>().AsSingle();
			base.Container.BindInterfacesTo<FocusablesService>().AsSingle();
			base.Container.BindInterfacesTo<GenericEventBus<WindowsEventData>>().AsSingle();
			base.Container.BindInterfacesTo<GenericEventBus<FocusEventData>>().AsSingle();
			base.Container.BindInterfacesTo<WindowFocusSystem>().AsSingle().NonLazy();
			base.Container.Bind<PreloadedWindowsModel>().AsSingle();
		}
	}
}

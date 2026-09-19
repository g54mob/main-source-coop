using Zenject;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class UIByContextInstaller : Installer<UIByContextInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IPresenterFactory>().To<DIPresenterFactory>().AsSingle();
			base.Container.Bind<IWindowsComponentsFinderService>().To<WindowsComponentsFinderService>().AsSingle();
			base.Container.Bind<IWindowsFactory>().To<MonoWindowsFactory>().AsSingle();
		}
	}
}

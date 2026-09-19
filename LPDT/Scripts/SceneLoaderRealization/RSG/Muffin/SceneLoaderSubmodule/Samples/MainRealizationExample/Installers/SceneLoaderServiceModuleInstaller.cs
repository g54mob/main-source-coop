using RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts;
using Zenject;

namespace RSG.Muffin.SceneLoaderSubmodule.Samples.MainRealizationExample.Installers
{
	public class SceneLoaderServiceModuleInstaller : Installer<SceneLoaderServiceModuleInstaller>
	{
		public override void InstallBindings()
		{
			Installer<SceneLoaderConfigInstaller>.Install(base.Container);
			base.Container.Bind<BuildInSceneLoaderService>().AsSingle();
			base.Container.Bind<AddressablesSceneLoaderService>().AsSingle();
			base.Container.BindInterfacesTo<SceneLoaderServiceFacade>().AsSingle();
		}
	}
}

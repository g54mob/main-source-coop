using Zenject;

namespace Features.SceneManagement.Installers
{
	public class NetworkSceneLoaderInstaller : Installer<NetworkSceneLoaderInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<INetworkSceneLoader>().To<NetworkSceneLoader>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DespawnNetworkObjectsNetworkEvent>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PeerDespawnedObjectsNetworkEvent>().AsSingle();
			base.Container.BindInterfacesTo<NetworkSceneLoaderServiceFacade>().AsSingle();
		}
	}
}

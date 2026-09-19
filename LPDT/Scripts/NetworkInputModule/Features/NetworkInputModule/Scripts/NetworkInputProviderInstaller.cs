using Zenject;

namespace Features.NetworkInputModule.Scripts
{
	public class NetworkInputProviderInstaller : Installer<NetworkInputProviderInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<NetworkInputProvider>().AsSingle();
		}
	}
}

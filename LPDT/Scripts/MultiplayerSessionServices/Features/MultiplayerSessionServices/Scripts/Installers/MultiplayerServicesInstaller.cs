using Features.MultiplayerSessionServices.Scripts.NetworkMasterClientTracking;
using NetworkServices.NetworkEvents;
using NetworkServices.RoomCodeGenerators;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts.Installers
{
	public class MultiplayerServicesInstaller : Installer<MultiplayerServicesInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<NetworkRunnerEventBus>().AsSingle();
			base.Container.Bind<IMultiplayerFactory>().To<MultiplayerFactory>().AsSingle();
			base.Container.Bind<IRoomCodeGenerator>().To<DefaultRoomCodeGenerator>().AsSingle();
			base.Container.Bind<IMultiplayerService>().To<MultiplayerService>().AsSingle();
			base.Container.Bind<IStartSessionService>().To<StartSessionService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<KickPlayerRequestNetworkEvent>().AsSingle();
			base.Container.BindInterfacesTo<NetworkMasterClientTrackerFactory>().AsSingle();
			base.Container.BindInterfacesTo<NetworkMasterClientTrackerSpawnSystem>().AsSingle();
		}
	}
}

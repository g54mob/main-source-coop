using Features.SessionManagementModule.Models;
using Features.WeatherModule.Scripts;
using Zenject;

namespace Features.SessionManagementModule.Installers
{
	public class SessionManagementSceneInstaller : Installer<SessionManagementSceneInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<ILevelSelectionSource>().To<SessionLevelSelectionSource>().AsSingle();
			base.Container.Bind<INetworkedSceneLoader>().To<SessionNetworkedSceneLoader>().AsSingle();
			base.Container.Bind<ILevelLoadObservation>().To<SessionLevelLoadObservation>().AsSingle();
			base.Container.Bind<IMasterWeatherSelector>().To<MasterWeatherSelector>().AsSingle();
			base.Container.Bind<IShopActivation>().To<SessionShopActivation>().AsSingle();
			base.Container.Bind<IShopPlayerControl>().To<SessionShopPlayerControl>().AsSingle();
			base.Container.Bind<IShopSeatHealthObservation>().To<SessionShopSeatHealth>().AsSingle();
			base.Container.Bind<ISessionStateContext>().To<SessionStateMachine>().FromResolve();
			base.Container.BindInterfacesAndSelfTo<SessionRunStarter>().AsSingle();
			Installer<SessionManagementInstaller>.Install(base.Container);
		}
	}
}

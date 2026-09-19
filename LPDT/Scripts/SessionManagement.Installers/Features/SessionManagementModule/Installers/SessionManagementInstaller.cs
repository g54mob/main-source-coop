using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Gate;
using Features.SessionManagementModule.Models;
using Features.SessionManagementModule.Presence;
using Zenject;

namespace Features.SessionManagementModule.Installers
{
	public class SessionManagementInstaller : Installer<SessionManagementInstaller>
	{
		public override void InstallBindings()
		{
			Installer<SessionPlayerPresenceInstaller>.Install(base.Container);
			base.Container.BindInterfacesAndSelfTo<SessionGate>().AsSingle();
			base.Container.Bind<ISessionGateWindowPolicy>().To<SessionGateWindowPolicy>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<ShopLeaveVoteObservation>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<EnemySpawnGate>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<LobbyState>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<LevelState>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<ShopState>().AsSingle();
			base.Container.Bind<ISessionProfiler>().To<NullSessionProfiler>().AsSingle();
			base.Container.Bind<ISessionRecoveryController>().To<SessionRecoveryController>().AsSingle();
		}
	}
}

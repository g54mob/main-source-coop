using Zenject;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamInviteModuleInstaller : Installer<SteamInviteModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<SteamLobbyModel>().AsSingle();
			base.Container.Bind<ISteamInviteService>().To<SteamInviteService>().AsSingle();
			base.Container.BindInterfacesTo<SteamPlayerGroupSyncSystem>().AsSingle();
			base.Container.BindInterfacesTo<SteamLobbyAutoJoinSystem>().AsSingle();
			base.Container.BindInterfacesTo<SteamLobbyJoinSystem>().AsSingle();
			base.Container.BindInterfacesTo<SteamLobbyHostSystem>().AsSingle();
			base.Container.BindInterfacesTo<SteamLobbyJoinabilitySyncSystem>().AsSingle();
		}
	}
}

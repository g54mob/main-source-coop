using Features.DisconnectHandlerModule.Scripts.Installers;
using Features.MainMenuModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.UsersStatsModule.Scripts.Steam;
using Features.ViewSystemModule.Scripts.Installers;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Installers
{
	[CreateAssetMenu(menuName = "Configurations/GameBootstrap/LobbyContextInstaller", fileName = "LobbyContextInstaller_Default", order = 0)]
	public class LobbyContextInstaller : ScriptableObjectInstaller<LobbyContextInstaller>
	{
		public override void InstallBindings()
		{
			Installer<UIByContextInstaller>.Install(base.Container);
			Installer<LobbyUIInstaller>.Install(base.Container);
			Installer<LobbyDisconnectHandlerInstaller>.Install(base.Container);
			base.Container.Bind<WantToLeaveLobbyPopupModel>().AsSingle();
			base.Container.BindInterfacesTo<SteamUserStatsService>().AsSingle();
			base.Container.BindInterfacesTo<MatchmakingAutoCloseSystem>().AsSingle();
		}
	}
}

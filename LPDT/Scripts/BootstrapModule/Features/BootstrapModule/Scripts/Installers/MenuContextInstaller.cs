using Features.MainMenuModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.SettingsMenuModule.Scripts;
using Features.ViewSystemModule.Scripts.Installers;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Installers
{
	[CreateAssetMenu(menuName = "Configurations/GameBootstrap/MenuContextInstaller", fileName = "MenuContextInstaller_Default", order = 0)]
	public class MenuContextInstaller : ScriptableObjectInstaller<MenuContextInstaller>
	{
		public override void InstallBindings()
		{
			Installer<UIByContextInstaller>.Install(base.Container);
			Installer<MenuUIInstaller>.Install(base.Container);
			base.Container.BindInterfacesTo<NetworkRunnerCreateSystem>().AsSingle();
			base.Container.BindInterfacesTo<RegionsPingTracingSystem>().AsSingle();
			base.Container.BindInterfacesTo<SessionReconnectService>().AsSingle();
			base.Container.BindInterfacesTo<QuickJoinService>().AsSingle();
			base.Container.Bind<JoinCrewPopupModel>().AsSingle();
			base.Container.Bind<MatchmakingPreviewPopupModel>().AsSingle();
		}
	}
}

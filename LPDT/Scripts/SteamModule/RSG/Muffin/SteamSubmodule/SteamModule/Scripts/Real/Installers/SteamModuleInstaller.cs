using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API.Installers;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace RSG.Muffin.SteamSubmodule.SteamModule.Scripts.Real.Installers
{
	public class SteamModuleInstaller : Installer<SteamModuleInstaller>, ISteamModuleInstaller, IMockableInstaller
	{
		public void CallInstall(DiContainer container)
		{
			Installer<SteamModuleInstaller>.Install(container);
		}

		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SteamAuthenticationSystem>().AsSingle();
		}
	}
}

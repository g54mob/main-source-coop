using RSG.Muffin.PlatformStatusSubmodule.Scripts.API.Installers;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API.Installers;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.SteamImplementationModule.Scripts
{
	public class SteamRealizationInstaller : Installer<SteamRealizationInstaller>
	{
		public override void InstallBindings()
		{
			Installer<SteamAPIInstaller>.Install(base.Container);
			Installer<PlatformStatusAPIInstaller>.Install(base.Container);
			base.Container.BindAndInstall<ISteamModuleInstaller>();
			base.Container.BindAndInstall<ISteamLanguageInstaller>();
			base.Container.BindAndInstall<ISteamRegionInstaller>();
		}
	}
}

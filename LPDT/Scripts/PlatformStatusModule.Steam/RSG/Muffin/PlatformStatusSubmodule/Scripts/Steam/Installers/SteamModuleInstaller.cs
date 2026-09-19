using RSG.Muffin.PlatformStatusSubmodule.Scripts.API.Installers;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace RSG.Muffin.PlatformStatusSubmodule.Scripts.Steam.Installers
{
	public class SteamModuleInstaller : Installer<SteamModuleInstaller>, IPlatformStatusModuleInstaller, IMockableInstaller
	{
		public void CallInstall(DiContainer container)
		{
			Installer<SteamModuleInstaller>.Install(container);
		}

		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SteamPlatformStatusService>().AsSingle();
		}
	}
}

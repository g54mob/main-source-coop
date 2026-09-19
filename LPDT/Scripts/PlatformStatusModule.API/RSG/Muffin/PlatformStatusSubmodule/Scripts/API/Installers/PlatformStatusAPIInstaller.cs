using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace RSG.Muffin.PlatformStatusSubmodule.Scripts.API.Installers
{
	public class PlatformStatusAPIInstaller : Installer<PlatformStatusAPIInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindAndInstall<IPlatformStatusModuleInstaller>();
		}
	}
}

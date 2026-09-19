using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.SteamImplementationModule.Scripts.Real
{
	public class SteamRegionDetectionInstaller : Installer<SteamRegionDetectionInstaller>, ISteamRegionInstaller, IMockableInstaller
	{
		public override void InstallBindings()
		{
			base.Container.Bind<ISteamRegionProvider>().To<SteamRegionProvider>().AsSingle();
		}
	}
}

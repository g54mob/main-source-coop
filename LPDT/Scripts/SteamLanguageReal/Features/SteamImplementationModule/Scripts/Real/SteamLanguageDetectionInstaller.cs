using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.SteamImplementationModule.Scripts.Real
{
	public class SteamLanguageDetectionInstaller : Installer<SteamLanguageDetectionInstaller>, ISteamLanguageInstaller, IMockableInstaller
	{
		public void CallInstall(DiContainer container)
		{
			Installer<SteamLanguageDetectionInstaller>.Install(container);
		}

		public override void InstallBindings()
		{
			base.Container.Bind<ISteamLanguageProvider>().To<SteamLanguageProvider>().AsSingle();
		}
	}
}

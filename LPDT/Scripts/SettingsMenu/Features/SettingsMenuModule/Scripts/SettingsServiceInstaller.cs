using Features.SettingsMenuModule.Scripts.Services;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsServiceInstaller : Installer<SettingsServiceInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<SettingsInitializerSystem>().AsSingle();
			base.Container.Bind<IRegionLanguageDetectionService>().To<RegionLanguageDetectionService>().AsSingle();
			base.Container.Bind<IQualitySettingsService>().To<QualitySettingsService>().AsSingle();
			base.Container.Bind<IPerformanceDetectionService>().To<PerformanceDetectionService>().AsSingle();
			base.Container.Bind<ISettingsService>().To<SettingsService>().AsSingle();
			base.Container.Bind<IApplySettingsService>().To<ApplySettingsService>().AsSingle();
		}
	}
}

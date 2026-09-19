using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class GuideModuleInstaller : Installer<GuideModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<GuideLineBuilderDataHolder>().AsSingle();
			base.Container.BindInterfacesTo<GuideLineBuildService>().AsSingle();
			base.Container.BindInterfacesTo<GuideLineFactory>().AsSingle();
			base.Container.BindInterfacesTo<GuideLinePoolService>().AsSingle();
			base.Container.BindInterfacesTo<ArcBuildService>().AsSingle();
			base.Container.BindInterfacesTo<TipFactory>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<TipPoolService>().AsSingle();
			base.Container.BindInterfacesTo<TipService>().AsSingle();
			base.Container.Bind<TipFollowerDataHolder>().AsSingle();
		}
	}
}
